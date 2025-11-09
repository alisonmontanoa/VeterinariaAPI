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
using Veterinaria.Core.Services;
using Veterinaria.Infrastructure.Queries;
using Veterinaria.Infrastructure.Repositories;
using Veterinaria.Infrastructure.Validators;

namespace Veterinaria.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DuenoController : ControllerBase
    {
        private readonly IDuenoService _duenoService;
        private readonly IValidationService _validationService;
        private readonly IDapperContext _dapper;

        public DuenoController(IDuenoService duenoService, IValidationService validationService, IDapperContext dapper)
        {
            _duenoService = duenoService;
            _validationService = validationService;
            _dapper = dapper;
        }

        /// <summary>
        /// Registra un dueño junto con su mascota.
        /// </summary>
        /// <remarks>
        /// Este endpoint valida los datos del dueño y la mascota, aplica reglas de negocio
        /// y devuelve los IDs generados tras el registro exitoso.
        /// </remarks>
        /// <param name="request">Objeto que contiene la información del dueño y la mascota.</param>
        /// <returns>Retorna los IDs del dueño y la mascota recién registrados.</returns>
        /// <response code="201">Registro exitoso.</response>
        /// <response code="400">Error de validación o datos inválidos.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost("registrar-duenoConMascota")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ApiResponse<object>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Registrar un duenio con su mascota", Description = "Registra un nuevo duenio junto con su mascota en el sistema.")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarDuenoMascotaRequest request)
        {
            var duenoValidation = await _validationService.ValidateAsync(request.Dueno);
            var mascotaValidation = await _validationService.ValidateAsync(request.Mascota);

            if (!duenoValidation.IsValid || !mascotaValidation.IsValid)
            {
                var errores = duenoValidation.Errors.Concat(mascotaValidation.Errors);
                return BadRequest(new { Errors = errores });
            }

            (int duenoId, int mascotaId) = await _duenoService.RegistrarDuenoConMascotaAsync(request.Dueno, request.Mascota);

            return StatusCode(201, new ApiResponse<object>(new
            {
                DuenoId = duenoId,
                MascotaId = mascotaId,
                Mensaje = "Registro exitoso"
            }));
        }

        /// <summary>
        /// Filtra los dueños registrados según parámetros opcionales.
        /// </summary>
        /// <remarks>
        /// Se puede filtrar por nombre, dirección o teléfono.  
        /// Este método devuelve una lista de dueños ordenada por ID descendente.
        /// </remarks>
        /// <param name="filters">Filtros de búsqueda aplicables.</param>
        /// <returns>Lista de dueños filtrada según los criterios ingresados.</returns>
        /// <response code="200">Retorna la lista de dueños.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("filtrar-duenos")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<Dueno>>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Filtrar duenios", Description = "Obtiene una lista filtrada de duenios mediante Entity Framework.")]
        public async Task<IActionResult> FiltrarDuenos([FromQuery] DuenoQueryFilter filters)
        {
            var result = await _duenoService.ObtenerDuenosAsync(filters);
            return Ok(new ApiResponse<IEnumerable<Dueno>>(result));
        }

        /// <summary>
        /// Filtra los dueños usando consultas Dapper.
        /// </summary>
        /// <remarks>
        /// Permite búsqueda más rápida directamente por SQL nativo (solo lectura).
        /// </remarks>
        /// <param name="filters">Filtros por nombre, dirección o teléfono.</param>
        /// <returns>Lista de dueños filtrada por Dapper.</returns>
        [HttpGet("dapper/filtrar-duenos")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<Dueno>>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(Summary = "Filtrar duenios (Dapper)", Description = "Obtiene una lista de duenios mediante Dapper SQL.")]
        public async Task<IActionResult> FiltrarDuenosDapper([FromQuery] DuenoQueryFilter filters)
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => DuenoQueries.DuenoQuerySqlServer,
                DatabaseProvider.MySql => DuenoQueries.DuenoQueryMySQL,
                _ => throw new NotSupportedException("Proveedor no soportado.")
            };

            var result = await _dapper.QueryAsync<Dueno>(sql, new
            {
                filters.Nombre,
                filters.Direccion,
                filters.Telefono
            });

            return Ok(new ApiResponse<IEnumerable<Dueno>>(result));
        }
    }
}