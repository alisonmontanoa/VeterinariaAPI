using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Infrastructure.Queries
{
    public static class ServicioQueries
    {
        public static string ServicioQuerySqlServer = @"
            SELECT Id, Nombre, Descripcion, Precio
            FROM Servicio
            WHERE 1=1
                AND (@Nombre IS NULL OR LOWER(Nombre) LIKE '%' + LOWER(@Nombre) + '%')
                AND (@Descripcion IS NULL OR LOWER(Descripcion) LIKE '%' + LOWER(@Descripcion) + '%')
            ORDER BY Id DESC;
        ";

        public static string ServicioQueryMySQL = @"
            SELECT Id, Nombre, Descripcion, Precio
            FROM Servicio
            WHERE 1=1
                AND (@Nombre IS NULL OR LOWER(Nombre) LIKE CONCAT('%', LOWER(@Nombre), '%'))
                AND (@Descripcion IS NULL OR LOWER(Descripcion) LIKE CONCAT('%', LOWER(@Descripcion), '%'))
            ORDER BY Id DESC;
        ";
    }
}