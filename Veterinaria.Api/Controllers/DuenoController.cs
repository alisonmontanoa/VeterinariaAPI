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
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.QueryFilters;
using Veterinaria.Core.Services;
using Veterinaria.Infrastructure.Queries;
using Veterinaria.Infrastructure.Repositories;
using Veterinaria.Infrastructure.Validators;

namespace Veterinaria.Api.Controllers.v1
{
    /// <summary>
    /// Controlador para la gestion de dueños.
    /// </summary>
    /// <remarks>
    /// Permite registrar dueños con sus mascotas y consultar listados paginados.
    /// 
    /// Acceso permitido para:
    /// - Administrador
    /// - Recepcionista
    /// </remarks>
    [Authorize(Roles = $"{nameof(RoleType.Administrador)},{nameof(RoleType.Recepcionista)}")]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DuenoController : ControllerBase
    {
        private readonly IDuenoService _duenoService;
        private readonly IValidationService _validationService;
        private readonly IDapperContext _dapper;
        private readonly IMapper _mapper;

        public DuenoController(IDuenoService duenoService, IValidationService validationService, IDapperContext dapper, IMapper mapper)
        {
            _duenoService = duenoService;
            _validationService = validationService;
            _dapper = dapper;
            _mapper = mapper;
        }

        /// <summary>
        /// Registra un nuevo dueño junto con una mascota asociada
        /// </summary>
        /// <remarks>
        /// Caso de Uso 1.
        /// Registra un dueño y su mascota aplicando todas las validaciones de negocio.
        /// 
        /// Reglas:
        /// - Telefono único
        /// - Campos obligatorios
        /// - Maximo de mascotas por dueño
        /// </remarks>
        /// <param name="request">Datos del dueño y su mascota.</param>
        /// <returns>IDs generados del dueño y la mascota.</returns>
        /// <response code="201">Registro exitoso</response>
        /// <response code="400">Errores de validación</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost("registrar-duenoConMascota")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [SwaggerOperation(
            Summary = "Registrar dueño con mascota",
            Description = "Registra un dueño junto con una mascota asociada."
        )]
        public async Task<IActionResult> Registrar(
            [FromBody] RegistrarDuenoMascotaRequest request)
        {
            var duenoValidation = await _validationService.ValidateAsync(request.Dueno);
            var mascotaValidation = await _validationService.ValidateAsync(request.Mascota);

            if (!duenoValidation.IsValid || !mascotaValidation.IsValid)
            {
                var errores = duenoValidation.Errors
                    .Concat(mascotaValidation.Errors)
                    .ToList();

                return BadRequest(new ApiResponse<object>(null)
                {
                    Messages = errores
                });
            }

            var (duenoId, mascotaId) =
                await _duenoService.RegistrarDuenoConMascotaAsync(
                    request.Dueno,
                    request.Mascota);

            return StatusCode(201, new ApiResponse<object>(new
            {
                DuenoId = duenoId,
                MascotaId = mascotaId
            }));
        }

        /// <summary>
        /// Obtiene un listado paginado de dueños
        /// </summary>
        /// <remarks>
        /// Permite filtrar por nombre, direccion y teléfono.
        /// 
        /// Retorna resultados paginados.
        /// </remarks>
        /// <param name="filters">Parametros de filtrado y paginacion.</param>
        /// <returns>Listado paginado de dueños.</returns>
        /// <response code="200">Listado obtenido correctamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("filtrar-duenos")]
        [ProducesResponseType((int)HttpStatusCode.OK,
            Type = typeof(ApiResponse<IEnumerable<DuenoDto>>))]
        [SwaggerOperation(
            Summary = "Filtrar dueños",
            Description = "Obtiene un listado paginado de dueños usando Entity Framework."
        )]
        public async Task<IActionResult> FiltrarDuenos(
            [FromQuery] DuenoQueryFilter filters)
        {
            var pagedResult = await _duenoService.ObtenerDuenosAsync(filters);

            var dueniosDto = _mapper.Map<IEnumerable<DuenoDto>>(pagedResult);

            return Ok(new ApiResponse<IEnumerable<DuenoDto>>(dueniosDto)
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
        /// Obtiene un listado de dueños utilizando Dapper.
        /// </summary>
        /// <remarks>
        /// Consulta optimizada solo para lectura.
        /// No aplica paginacion.
        /// </remarks>
        /// <param name="filters">Parametros de filtrado.</param>
        /// <returns>Listado de dueños.</returns>
        /// <response code="200">Listado obtenido correctamente</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("dapper/filtrar-duenos")]
        [ProducesResponseType((int)HttpStatusCode.OK,
            Type = typeof(ApiResponse<IEnumerable<DuenoDto>>))]
        [SwaggerOperation(
            Summary = "Filtrar dueños (Dapper)",
            Description = "Obtiene un listado de dueños usando consultas SQL con Dapper."
        )]
        public async Task<IActionResult> FiltrarDuenosDapper(
            [FromQuery] DuenoQueryFilter filters)
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => DuenoQueries.DuenoQuerySqlServer,
                DatabaseProvider.MySql => DuenoQueries.DuenoQueryMySQL,
                _ => throw new NotSupportedException("Proveedor no soportado")
            };

            var resultado = await _dapper.QueryAsync<DuenoDto>(sql, new
            {
                filters.Nombre,
                filters.Direccion,
                filters.Telefono
            });

            return Ok(new ApiResponse<IEnumerable<DuenoDto>>(resultado));
        }
    }
}