using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Veterinaria.Api.Responses;
using Veterinaria.Core.CustomEntities;
using Veterinaria.Core.DTOs;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Enums;
using Veterinaria.Core.Exceptions;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.QueryFilters;
using Veterinaria.Infrastructure.Queries;

namespace Veterinaria.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestion de servicios veterinarios.
    /// </summary>
    /// <remarks>
    /// Permite registrar, consultar y filtrar los servicios ofrecidos
    /// por la clinica veterinaria.
    /// 
    /// Incluye consultas con Entity Framework y Dapper.
    /// Protegido mediante JWT.
    /// </remarks>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly IServicioService _servicioService;
        private readonly IDapperContext _dapper;
        private readonly IMapper _mapper;

        public ServicioController(IServicioService servicioService, IDapperContext dapper, IMapper mapper)
        {
            _servicioService = servicioService;
            _dapper = dapper;
            _mapper = mapper;
        }

        /// <summary>
        /// Registra un nuevo servicio veterinario.
        /// </summary>
        /// <remarks>
        /// <b>Caso de Uso 6</b>  
        /// Permite registrar los servicios ofrecidos por la clinica
        /// (consulta, vacunacion, cirugia, etc.).
        /// 
        /// Rol requerido:
        /// - Administrador
        /// </remarks>
        /// <param name="servicioDto">Datos del servicio a registrar.</param>
        /// <returns>ID del servicio creado.</returns>
        /// <response code="201">Servicio registrado correctamente</response>
        /// <response code="400">Datos invalidos</response>
        /// <response code="500">Error interno del servidor</response>
        [Authorize(Roles = nameof(RoleType.Administrador))]
        [HttpPost("registrar-servicio")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [SwaggerOperation(
            Summary = "Registrar servicio",
            Description = "Registra un nuevo servicio veterinario."
        )]
        public async Task<IActionResult> RegistrarServicio(
            [FromBody] ServicioDto servicioDto)
        {
            int id = await _servicioService.CrearServicioAsync(servicioDto);

            return StatusCode(201, new ApiResponse<object>(new
            {
                ServicioId = id,
                Mensaje = "Servicio registrado correctamente"
            }));
        }

        /// <summary>
        /// Obtiene un listado paginado de servicios.
        /// </summary>
        /// <remarks>
        /// Permite filtrar por nombre y descripcion.
        /// Retorna resultados paginados usando Entity Framework.
        /// </remarks>
        /// <param name="filters">Parametros de filtrado y paginacion.</param>
        /// <returns>Listado paginado de servicios.</returns>
        /// <response code="200">Listado obtenido correctamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("filtrar-servicios")]
        [ProducesResponseType((int)HttpStatusCode.OK,
            Type = typeof(ApiResponse<IEnumerable<ServicioDto>>))]
        [SwaggerOperation(
            Summary = "Filtrar servicios",
            Description = "Obtiene un listado paginado de servicios usando Entity Framework."
        )]
        public async Task<IActionResult> FiltrarServicios(
            [FromQuery] ServicioQueryFilter filters)
        {
            var pagedResult = await _servicioService.ObtenerServiciosAsync(filters);

            var serviciosDto = _mapper.Map<IEnumerable<ServicioDto>>(pagedResult);

            return Ok(new ApiResponse<IEnumerable<ServicioDto>>(serviciosDto)
            {
                Pagination = new Pagination
                {
                    TotalCount = pagedResult.TotalCount,
                    PageSize = pagedResult.PageSize,
                    CurrentPage = pagedResult.CurrentPage,
                    TotalPages = pagedResult.TotalPages,
                    HasNextPage = pagedResult.HasNextPage,
                    HasPreviousPage = pagedResult.HasPreviousPage
                }
            });
        }

        /// <summary>
        /// Obtiene un listado de servicios utilizando Dapper.
        /// </summary>
        /// <remarks>
        /// Consulta optimizada solo para lectura.
        /// No aplica paginacipn.
        /// </remarks>
        /// <param name="filters">Parametros de filtrado.</param>
        /// <returns>Listado de servicios.</returns>
        /// <response code="200">Listado obtenido correctamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("dapper/filtrar-servicios")]
        [ProducesResponseType((int)HttpStatusCode.OK,
            Type = typeof(ApiResponse<IEnumerable<ServicioDto>>))]
        [SwaggerOperation(
            Summary = "Filtrar servicios (Dapper)",
            Description = "Obtiene un listado de servicios usando consultas SQL optimizadas."
        )]
        public async Task<IActionResult> FiltrarServiciosDapper(
            [FromQuery] ServicioQueryFilter filters)
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => ServicioQueries.ServicioQuerySqlServer,
                DatabaseProvider.MySql => ServicioQueries.ServicioQueryMySQL,
                _ => throw new NotSupportedException("Proveedor no soportado")
            };

            var resultado = await _dapper.QueryAsync<ServicioDto>(sql, new
            {
                filters.Nombre,
                filters.Descripcion
            });

            return Ok(new ApiResponse<IEnumerable<ServicioDto>>(resultado));
        }

        /// <summary>
        /// Filtra servicios usando Dapper con paginacion.
        /// </summary>
        /// <remarks>
        /// Demuestra el uso formal de Dapper junto con paginacion manual.
        /// 
        /// Retorna:
        /// - Datos paginados
        /// - Metadata de paginacion
        /// </remarks>
        /// <param name="filters">Parametros de filtrado y paginacion.</param>
        /// <returns>Listado paginado de servicios.</returns>
        /// <response code="200">Listado paginado obtenido correctamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("dapper/filtrar-servicios-paginado")]
        [ProducesResponseType((int)HttpStatusCode.OK,
            Type = typeof(ApiResponse<IEnumerable<ServicioDto>>))]
        [SwaggerOperation(
            Summary = "Filtrar servicios (Dapper paginado)",
            Description = "Obtiene un listado paginado de servicios utilizando Dapper."
        )]
        public async Task<IActionResult> FiltrarServiciosDapperPaginado(
            [FromQuery] ServicioQueryFilter filters)
        {
            var parameters = new
            {
                filters.Nombre,
                filters.Descripcion,
                Offset = (filters.PageNumber - 1) * filters.PageSize,
                PageSize = filters.PageSize
            };

            var totalCount = await _dapper.ExecuteScalarAsync<int>(
                ServicioQueries.ServicioCountSqlServer,
                parameters);

            var data = await _dapper.QueryAsync<ServicioDto>(
                ServicioQueries.ServicioPagedSqlServer,
                parameters);

            var pagedResult = new PagedList<ServicioDto>(
                data.ToList(),
                totalCount,
                filters.PageNumber,
                filters.PageSize
            );

            return Ok(new ApiResponse<IEnumerable<ServicioDto>>(pagedResult)
            {
                Pagination = new Pagination
                {
                    TotalCount = pagedResult.TotalCount,
                    PageSize = pagedResult.PageSize,
                    CurrentPage = pagedResult.CurrentPage,
                    TotalPages = pagedResult.TotalPages,
                    HasNextPage = pagedResult.HasNextPage,
                    HasPreviousPage = pagedResult.HasPreviousPage
                }
            });
        }
    }
}