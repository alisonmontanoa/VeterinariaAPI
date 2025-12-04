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
using Veterinaria.Core.Services;
using Veterinaria.Infrastructure.Queries;
using Veterinaria.Infrastructure.Repositories;
using Veterinaria.Infrastructure.Validators;

namespace Veterinaria.Api.Controllers.v1
{
    /// <summary>
    /// Controlador encargado de gestionar las operaciones relacionadas con los dueños
    /// dentro del sistema veterinario.
    /// </summary>
    /// <remarks>
    /// Este controlador permite registrar dueños junto a sus mascotas, así como realizar
    /// busquedas filtradas utilizando Entity Framework o consultas Dapper para obtener 
    /// un rendimiento superior.  
    /// 
    /// Todos los metodos de este controlador requieren autenticación mediante JWT
    /// y estan protegidos con el atributo <see cref="Authorize"/>.
    /// </remarks>
    [Authorize]
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
        /// Registra un nuevo dueño junto con una mascota asociada.
        /// </summary>
        /// <remarks>
        /// Este endpoint forma parte del **Caso de Uso 1**.  
        /// 
        /// Reglas aplicadas:
        /// - El telefono del dueño debe ser unico en la base de datos.  
        /// - Todos los campos del dueño y la mascota son obligatorios.  
        /// - Se valida la estructura del request mediante validadores personalizados.  
        /// - No se permite registrar una mascota sin un dueño valido.  
        ///
        /// Flujo del proceso:
        /// 1. Se validan los datos del dueño y la mascota.  
        /// 2. Si la validacion falla, se devuelven todos los errores detectados.  
        /// 3. Si es correcto, se registra el dueño y luego su mascota asociada.  
        /// 4. Se retorna el estado **201 Created** con los IDs generados.
        ///
        /// Ejemplo de Request:
        /// 
        ///     {
        ///         "dueno": {
        ///             "nombre": "Carlos Gomez",
        ///             "telefono": "77712345",
        ///             "direccion": "Zona Norte"
        ///         },
        ///         "mascota": {
        ///             "nombre": "Firulais",
        ///             "especie": "Perro",
        ///             "raza": "Pastor Aleman",
        ///             "edad": 3
        ///         }
        ///     }
        /// </remarks>
        /// <param name="request">Objeto que contiene los datos del dueño y su mascota asociada.</param>
        /// <returns>Un objeto con los IDs generados y un mensaje de confirmación.</returns>
        /// <response code="201">Registro exitoso.</response>
        /// <response code="400">Datos inválidos o errores de validación.</response>
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
        /// Obtiene una lista de dueños filtrada segun parametros opcionales.
        /// </summary>
        /// <remarks>
        /// Este metodo permite filtrar por:
        /// - Nombre  
        /// - Direccion  
        /// - Telefono  
        ///
        /// La busqueda se realiza mediante Entity Framework Core y retorna los resultados
        /// ordenados por ID de manera descendente.
        ///
        /// Ejemplo de uso:
        ///
        ///     GET /api/v1/Dueno/filtrar-duenos?nombre=Carlos
        ///
        /// </remarks>
        /// <param name="filters">Parametros opcionales para el filtrado.</param>
        /// <returns>Lista filtrada de dueños.</returns>
        /// <response code="200">Lista obtenida exitosamente.</response>
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
        /// Filtra los dueños utilizando consultas SQL optimizadas mediante Dapper.
        /// </summary>
        /// <remarks>
        /// Este metodo ofrece una alternativa más rapida frente a Entity Framework
        /// cuando se requieren grandes volumenes de datos y operaciones solo de lectura.
        ///
        /// Soporta filtrado por:
        /// - Nombre  
        /// - Direccion  
        /// - Telefono  
        ///
        /// Ejemplo de llamada:
        ///
        ///     GET /api/v1/Dueno/dapper/filtrar-duenos?telefono=77712345
        ///
        /// </remarks>
        /// <param name="filters">Filtros aplicados al query.</param>
        /// <returns>Lista filtrada de dueños.</returns>
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