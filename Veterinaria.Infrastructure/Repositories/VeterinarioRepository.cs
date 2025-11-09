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
    public class VeterinarioRepository : BaseRepository<Veterinario>, IVeterinarioRepository
    {
        private readonly IDapperContext _dapper;

        // private readonly VeterinariaContext _context;

        public VeterinarioRepository(VeterinariaContext context, IDapperContext dapper) : base(context)
        {
            //_context = context;
            _dapper = dapper;
        }

        public async Task<bool> ExistsByTelefonoAsync(string telefono)
        {
            return await _context.Veterinarios.AnyAsync(v => v.Telefono == telefono);
        }

        public async Task<IEnumerable<Veterinario>> GetAllVeterinariosDapperAsync(VeterinarioQueryFilter filters)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => VeterinarioQueries.VeterinarioQuerySqlServer,
                    DatabaseProvider.MySql => VeterinarioQueries.VeterinarioQueryMySQL,
                    _ => throw new NotSupportedException("Proveedor no soportado")
                };

                return await _dapper.QueryAsync<Veterinario>(sql, new
                {
                    filters.Nombre,
                    filters.Telefono,
                    filters.Especialidad
                });
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

    }
}