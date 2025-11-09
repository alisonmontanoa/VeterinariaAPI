using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Api.Responses;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Enums;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.QueryFilters;
using Veterinaria.Infrastructure.Queries;

namespace Veterinaria.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MascotaController : ControllerBase
    {
        private readonly IMascotaService _mascotaService;
        private readonly IDapperContext _dapper;

        public MascotaController(IMascotaService mascotaService, IDapperContext dapper)
        {
            _mascotaService = mascotaService;
            _dapper = dapper;
        }

        [HttpGet("filtrar-mascotas")]
        public async Task<IActionResult> FiltrarMascotas([FromQuery] MascotaQueryFilter filters)
        {
            var result = await _mascotaService.ObtenerMascotasAsync(filters);
            return Ok(new ApiResponse<IEnumerable<Mascota>>(result));
        }

        [HttpGet("dapper/filtrar-mascotas")]
        public async Task<IActionResult> FiltrarMascotasDapper([FromQuery] MascotaQueryFilter filters)
        {
            var sql = _dapper.Provider switch
            {
                DatabaseProvider.SqlServer => MascotaQueries.MascotaQuerySqlServer,
                DatabaseProvider.MySql => MascotaQueries.MascotaQueryMySQL,
                _ => throw new NotSupportedException("Proveedor no soportado.")
            };

            var result = await _dapper.QueryAsync<Mascota>(sql, new
            {
                filters.Nombre,
                filters.Raza,
                filters.Especie
            });

            return Ok(new ApiResponse<IEnumerable<Mascota>>(result));
        }
    }
}