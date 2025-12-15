using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Veterinaria.Api.Responses;
using Veterinaria.Core.DTOs;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Enums;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.QueryFilters;
using Veterinaria.Infrastructure.Queries;
using Veterinaria.Core.CustomEntities;

namespace Veterinaria.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestion de mascotas.
    /// </summary>
    /// <remarks>
    /// Permite consultar mascotas registradas, aplicar filtros
    /// y obtener listados paginados utilizando Entity Framework
    /// o consultas optimizadas con Dapper.
    /// 
    /// Acceso permitido para:
    /// - Cliente
    /// - Recepcionista
    /// </remarks>
    [Authorize(Roles = $"{nameof(RoleType.Cliente)},{nameof(RoleType.Recepcionista)}")]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MascotaController : ControllerBase
    {
        private readonly IMascotaService _mascotaService;
        private readonly IDapperContext _dapper;
        private readonly IMapper _mapper;

        public MascotaController(IMascotaService mascotaService, IDapperContext dapper, IMapper mapper)
        {
            _mascotaService = mascotaService;
            _dapper = dapper;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene un listado paginado de mascotas.
        /// </summary>
        /// <remarks>
        /// Permite filtrar por nombre, especie, raza, edad y dueño.
        /// Retorna resultados paginados usando Entity Framework.
        /// </remarks>
        /// <param name="filters">Parametros de filtrado y paginacion.</param>
        /// <returns>Listado paginado de mascotas.</returns>
        /// <response code="200">Listado obtenido correctamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("filtrar-mascotas")]
        [ProducesResponseType((int)HttpStatusCode.OK,
            Type = typeof(ApiResponse<IEnumerable<MascotaDto>>))]
        [SwaggerOperation(
            Summary = "Filtrar mascotas",
            Description = "Obtiene un listado paginado de mascotas aplicando filtros."
        )]
        public async Task<IActionResult> FiltrarMascotas(
            [FromQuery] MascotaQueryFilter filters)
        {
            var pagedResult = await _mascotaService.ObtenerMascotasAsync(filters);

            var mascotasDto = _mapper.Map<IEnumerable<MascotaDto>>(pagedResult);

            return Ok(new ApiResponse<IEnumerable<MascotaDto>>(mascotasDto)
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
        /// Obtiene un listado de mascotas utilizando Dapper.
        /// </summary>
        /// <remarks>
        /// Consulta optimizada solo para lectura.
        /// No aplica paginacion.
        /// </remarks>
        /// <param name="filters">Parametros de filtrado.</param>
        /// <returns>Listado de mascotas.</returns>
        /// <response code="200">Listado obtenido correctamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("dapper/filtrar-mascotas")]
        [ProducesResponseType((int)HttpStatusCode.OK,
            Type = typeof(ApiResponse<IEnumerable<MascotaDto>>))]
        [SwaggerOperation(
            Summary = "Filtrar mascotas (Dapper)",
            Description = "Obtiene un listado de mascotas usando consultas SQL optimizadas."
        )]
        public async Task<IActionResult> FiltrarMascotasDapper(
            [FromQuery] MascotaQueryFilter filters)
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => MascotaQueries.MascotaQuerySqlServer,
                DatabaseProvider.MySql => MascotaQueries.MascotaQueryMySQL,
                _ => throw new NotSupportedException("Proveedor no soportado")
            };

            var resultado = await _dapper.QueryAsync<MascotaDto>(sql, new
            {
                filters.Nombre,
                filters.Raza,
                filters.Especie
            });

            return Ok(new ApiResponse<IEnumerable<MascotaDto>>(resultado));
        }

        /// <summary>
        /// Lista mascotas junto con la informacion de su dueño.
        /// </summary>
        /// <remarks>
        /// Caso de Uso 5:
        /// Retorna un listado paginado de mascotas junto con
        /// los datos basicos de su dueño.
        /// </remarks>
        /// <param name="filters">Parametros de filtrado y paginacion.</param>
        /// <returns>Listado paginado de mascotas con su dueño.</returns>
        /// <response code="200">Listado obtenido correctamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("listar-mascotas-con-dueno")]
        [ProducesResponseType((int)HttpStatusCode.OK,
            Type = typeof(ApiResponse<IEnumerable<MascotaConDuenoDto>>))]
        [SwaggerOperation(
            Summary = "Listar mascotas con su dueño",
            Description = "Obtiene un listado paginado de mascotas junto con los datos basicos de su dueño."
        )]
        public async Task<IActionResult> ListarMascotasConDueno(
            [FromQuery] MascotaConDuenoQueryFilter filters)
        {
            var pagedResult =
                await _mascotaService.ListarMascotasConDuenoAsync(filters);

            return Ok(new ApiResponse<IEnumerable<MascotaConDuenoDto>>(pagedResult)
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