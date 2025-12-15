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
    /// Controlador para la gestion de veterinarios.
    /// </summary>
    /// <remarks>
    /// Permite registrar veterinarios y consultar listados
    /// paginados mediante Entity Framework o consultas optimizadas
    /// con Dapper.
    /// 
    /// Acceso permitido unicamente para:
    /// - Administrador
    /// </remarks>
    [Authorize(Roles = nameof(RoleType.Administrador))]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VeterinarioController : ControllerBase
    {
        private readonly IVeterinarioService _veterinarioService;
        private readonly IDapperContext _dapper;
        private readonly IMapper _mapper;

        public VeterinarioController(IVeterinarioService veterinarioService, IDapperContext dapper, IMapper mapper)
        {
            _veterinarioService = veterinarioService;
            _dapper = dapper;
            _mapper = mapper;
        }

        /// <summary>
        /// Registra un nuevo veterinario.
        /// </summary>
        /// <remarks>
        /// Caso de Uso 7:
        /// Permite registrar a un veterinario con su especialidad
        /// dentro del sistema clinico.
        /// </remarks>
        /// <param name="veterinarioDto">Datos del veterinario a registrar.</param>
        /// <returns>ID del veterinario creado.</returns>
        /// <response code="201">Veterinario registrado correctamente</response>
        /// <response code="400">Datos invalidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost("registrar-veterinario")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [SwaggerOperation(
            Summary = "Registrar veterinario",
            Description = "Registra un nuevo veterinario con su especialidad."
        )]
        public async Task<IActionResult> RegistrarVeterinario(
            [FromBody] VeterinarioDto veterinarioDto)
        {
            int id = await _veterinarioService.RegistrarVeterinarioAsync(veterinarioDto);

            return StatusCode(201, new ApiResponse<object>(new
            {
                VeterinarioId = id,
                Mensaje = "Veterinario registrado correctamente"
            }));
        }

        /// <summary>
        /// Obtiene un listado paginado de veterinarios.
        /// </summary>
        /// <remarks>
        /// Permite filtrar por nombre y especialidad.
        /// Retorna resultados paginados usando Entity Framework.
        /// </remarks>
        /// <param name="filters">Parametros de filtrado y paginacion.</param>
        /// <returns>Listado paginado de veterinarios.</returns>
        /// <response code="200">Listado obtenido correctamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("filtrar-veterinarios")]
        [ProducesResponseType((int)HttpStatusCode.OK,
            Type = typeof(ApiResponse<IEnumerable<VeterinarioDto>>))]
        [SwaggerOperation(
            Summary = "Filtrar veterinarios",
            Description = "Obtiene un listado paginado de veterinarios usando Entity Framework."
        )]
        public async Task<IActionResult> FiltrarVeterinarios(
            [FromQuery] VeterinarioQueryFilter filters)
        {
            var pagedResult = await _veterinarioService.ObtenerVeterinariosAsync(filters);

            var veterinariosDto = _mapper.Map<IEnumerable<VeterinarioDto>>(pagedResult);

            return Ok(new ApiResponse<IEnumerable<VeterinarioDto>>(veterinariosDto)
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
        /// Obtiene un listado de veterinarios utilizando Dapper.
        /// </summary>
        /// <remarks>
        /// Consulta optimizada solo para lectura.
        /// No aplica paginacion.
        /// </remarks>
        /// <param name="filters">Parametros de filtrado.</param>
        /// <returns>Listado de veterinarios.</returns>
        /// <response code="200">Listado obtenido correctamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("dapper/filtrar-veterinarios")]
        [ProducesResponseType((int)HttpStatusCode.OK,
            Type = typeof(ApiResponse<IEnumerable<VeterinarioDto>>))]
        [SwaggerOperation(
            Summary = "Filtrar veterinarios (Dapper)",
            Description = "Obtiene un listado de veterinarios usando consultas SQL optimizadas."
        )]
        public async Task<IActionResult> FiltrarVeterinariosDapper(
            [FromQuery] VeterinarioQueryFilter filters)
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => VeterinarioQueries.VeterinarioQuerySqlServer,
                DatabaseProvider.MySql => VeterinarioQueries.VeterinarioQueryMySQL,
                _ => throw new NotSupportedException("Proveedor no soportado")
            };

            var result = await _dapper.QueryAsync<VeterinarioDto>(sql, new
            {
                filters.Nombre,
                filters.Especialidad
            });

            return Ok(new ApiResponse<IEnumerable<VeterinarioDto>>(result));
        }
    }
}