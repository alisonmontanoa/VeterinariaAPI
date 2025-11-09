using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Api.Responses;
using Veterinaria.Core.DTOs;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Enums;
using Veterinaria.Core.Exceptions;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.QueryFilters;
using Veterinaria.Infrastructure.Queries;

namespace Veterinaria.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly IServicioService _servicioService;
        private readonly IDapperContext _dapper;

        public ServicioController(IServicioService servicioService, IDapperContext dapper)
        {
            _servicioService = servicioService;
            _dapper = dapper;
        }

        [HttpPost("registrar-servicio")]
        public async Task<IActionResult> RegistrarServicio([FromBody] ServicioDto servicioDto)
        {
            int id = await _servicioService.CrearServicioAsync(servicioDto);
            return CreatedAtAction(nameof(RegistrarServicio), new { id }, new { mensaje = "Servicio registrado correctamente", id });
        }

        // 🔹 EF
        [HttpGet("filtrar-servicios")]
        public async Task<IActionResult> FiltrarServicios([FromQuery] ServicioQueryFilter filters)
        {
            var result = await _servicioService.ObtenerServiciosAsync(filters);
            return Ok(new ApiResponse<IEnumerable<Servicio>>(result));
        }

        // 🔹 DAPPER
        [HttpGet("dapper/filtrar-servicios")]
        public async Task<IActionResult> FiltrarServiciosDapper([FromQuery] ServicioQueryFilter filters)
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => ServicioQueries.ServicioQuerySqlServer,
                DatabaseProvider.MySql => ServicioQueries.ServicioQueryMySQL,
                _ => throw new NotSupportedException("Proveedor no soportado.")
            };

            var result = await _dapper.QueryAsync<Servicio>(sql, new
            {
                filters.Nombre,
                filters.Descripcion
            });

            return Ok(new ApiResponse<IEnumerable<Servicio>>(result));
        }
    }
}