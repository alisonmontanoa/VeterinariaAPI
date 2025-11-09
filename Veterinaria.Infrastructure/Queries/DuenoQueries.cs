using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Infrastructure.Queries
{
    public static class DuenoQueries
    {
        public static string DuenoQuerySqlServer = @"
            SELECT Id, Nombre, Direccion, Telefono
            FROM Dueno
            WHERE 1=1
                AND (@Nombre IS NULL OR LOWER(Nombre) LIKE '%' + LOWER(@Nombre) + '%')
                AND (@Direccion IS NULL OR LOWER(Direccion) LIKE '%' + LOWER(@Direccion) + '%')
                AND (@Telefono IS NULL OR Telefono LIKE '%' + @Telefono + '%')
            ORDER BY Id DESC;
        ";

        public static string DuenoQueryMySQL = @"
            SELECT Id, Nombre, Direccion, Telefono
            FROM Dueno
            WHERE 1=1
                AND (@Nombre IS NULL OR LOWER(Nombre) LIKE CONCAT('%', LOWER(@Nombre), '%'))
                AND (@Direccion IS NULL OR LOWER(Direccion) LIKE CONCAT('%', LOWER(@Direccion), '%'))
                AND (@Telefono IS NULL OR Telefono LIKE CONCAT('%', @Telefono, '%'))
            ORDER BY Id DESC;
        ";
    }
}
