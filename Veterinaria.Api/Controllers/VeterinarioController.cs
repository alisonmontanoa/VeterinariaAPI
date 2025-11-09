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
    public class VeterinarioController : ControllerBase
    {
        private readonly IVeterinarioService _veterinarioService;
        private readonly IDapperContext _dapper;

        public VeterinarioController(IVeterinarioService veterinarioService, IDapperContext dapper)
        {
            _veterinarioService = veterinarioService;
            _dapper = dapper;
        }

        [HttpPost("registrar-veterinario")]
        public async Task<IActionResult> RegistrarVeterinario([FromBody] VeterinarioDto veterinarioDto)
        {
            int id = await _veterinarioService.RegistrarVeterinarioAsync(veterinarioDto);
            return CreatedAtAction(nameof(RegistrarVeterinario), new { id }, new { mensaje = "Veterinario registrado correctamente", id });
        }

        [HttpGet("filtrar-veterinarios")]
        public async Task<IActionResult> FiltrarVeterinarios([FromQuery] VeterinarioQueryFilter filters)
        {
            var result = await _veterinarioService.ObtenerVeterinariosAsync(filters);
            return Ok(new ApiResponse<IEnumerable<Veterinario>>(result));
        }

        [HttpGet("dapper/filtrar-veterinarios")]
        public async Task<IActionResult> FiltrarVeterinariosDapper([FromQuery] VeterinarioQueryFilter filters)
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => VeterinarioQueries.VeterinarioQuerySqlServer,
                DatabaseProvider.MySql => VeterinarioQueries.VeterinarioQueryMySQL,
                _ => throw new NotSupportedException("Proveedor no soportado.")
            };

            var result = await _dapper.QueryAsync<Veterinario>(sql, new
            {
                filters.Nombre,
                filters.Especialidad
            });

            return Ok(new ApiResponse<IEnumerable<Veterinario>>(result));
        }
    }
}