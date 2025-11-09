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
    public class DuenoRepository : BaseRepository<Dueno>, IDuenoRepository
    {
        private readonly IDapperContext _dapper;
        
        // private readonly VeterinariaContext _context;

        public DuenoRepository(VeterinariaContext context, IDapperContext dapper) : base(context)
        {
            //_context = context;
            _dapper = dapper;
        }

        public async Task<Dueno?> GetByTelefonoAsync(string telefono)
        {
            return await _context.Duenos.FirstOrDefaultAsync(d => d.Telefono == telefono);
        }

        public async Task<IEnumerable<Dueno>> GetAllDuenosDapperAsync(DuenoQueryFilter filters)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => DuenoQueries.DuenoQuerySqlServer,
                    DatabaseProvider.MySql => DuenoQueries.DuenoQueryMySQL,
                    _ => throw new NotSupportedException("Proveedor no soportado")
                };

                return await _dapper.QueryAsync<Dueno>(sql, new
                {
                    filters.Nombre,
                    filters.Direccion,
                    filters.Telefono
                });
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

    }
}