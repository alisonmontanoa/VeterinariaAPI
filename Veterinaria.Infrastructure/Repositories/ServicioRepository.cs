using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Enums;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.QueryFilters;
using Veterinaria.Infrastructure.Data;
using Veterinaria.Infrastructure.Queries;

namespace Veterinaria.Infrastructure.Repositories
{
    public class ServicioRepository : BaseRepository<Servicio>, IServicioRepository
    {
        private readonly IDapperContext _dapper;

        public ServicioRepository(VeterinariaContext context, IDapperContext dapper) : base(context)
        {
            _dapper = dapper;
        }

        public async Task<bool> ExistsByNombreAsync(string nombre)
        {
            return await _context.Servicios.AnyAsync(s => s.Nombre == nombre);
        }

        public async Task<IEnumerable<Servicio>> GetAllServiciosDapperAsync(ServicioQueryFilter filters)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => ServicioQueries.ServicioQuerySqlServer,
                    DatabaseProvider.MySql => ServicioQueries.ServicioQueryMySQL,
                    _ => throw new NotSupportedException("Proveedor no soportado")
                };

                return await _dapper.QueryAsync<Servicio>(sql, new
                {
                    filters.Nombre,
                    filters.Descripcion
                });
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

    }
}