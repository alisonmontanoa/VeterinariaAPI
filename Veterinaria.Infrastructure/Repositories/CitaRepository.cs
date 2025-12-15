using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.QueryFilters;
using Veterinaria.Infrastructure.Data;
using Veterinaria.Infrastructure.Queries;
using Veterinaria.Core.Enums;

namespace Veterinaria.Infrastructure.Repositories
{
    public class CitaRepository : BaseRepository<Cita>, ICitaRepository
    {
        private readonly IDapperContext _dapper;

        public CitaRepository(VeterinariaContext context, IDapperContext dapper) : base(context)
        {
            _dapper = dapper;
        }

        // Citas por veterinario
        public async Task<IEnumerable<Cita>> GetCitasByVeterinarioAsync(int veterinarioId)
        {
            return await _context.Citas
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .Include(c => c.Servicio)
                .Where(c => c.VeterinarioId == veterinarioId)
                .ToListAsync();
        }

        // Metodo personalizado: contar citas pendientes por mascota
        public async Task<int> CountPendientesByMascotaAsync(int mascotaId)
        {
            return await _context.Citas
                .CountAsync(c => c.MascotaId == mascotaId && c.Estado == "Pendiente");
        }

        public async Task<IEnumerable<Cita>> GetAllCitasDapperAsync(CitaQueryFilter filters)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => CitaQueries.CitaQuerySqlServer,
                    DatabaseProvider.MySql => CitaQueries.CitaQueryMySQL,
                    _ => throw new NotSupportedException("Proveedor no soportado")
                };

                return await _dapper.QueryAsync<Cita>(sql, new
                {
                    filters.VeterinarioId,
                    filters.MascotaId,
                    filters.Fecha,
                    filters.Estado
                });
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

    }
}
