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
    public class MascotaRepository : BaseRepository<Mascota>, IMascotaRepository
    {
        private readonly IDapperContext _dapper;

        public MascotaRepository(VeterinariaContext context, IDapperContext dapper) : base(context)
        {
            _dapper = dapper;
        }

        public async Task<int> CountByDuenoIdAsync(int duenoId)
        {
            return await _context.Mascotas.CountAsync(m => m.DuenoId == duenoId);
        }

        public async Task<IEnumerable<Mascota>> GetByDuenoIdAsync(int duenoId)
        {
            return await _context.Mascotas
                .Where(m => m.DuenoId == duenoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Mascota>> GetAllMascotasDapperAsync(MascotaQueryFilter filters)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => MascotaQueries.MascotaQuerySqlServer,
                    DatabaseProvider.MySql => MascotaQueries.MascotaQueryMySQL,
                    _ => throw new NotSupportedException("Proveedor no soportado")
                };

                return await _dapper.QueryAsync<Mascota>(sql, new
                {
                    filters.Especie,
                    filters.Raza,
                    filters.DuenoId
                });
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

    }
}