using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Infrastructure.Queries
{
    public static class VeterinarioQueries
    {
        public static string VeterinarioQuerySqlServer = @"
            SELECT Id, Nombre, Telefono, Especialidad
            FROM Veterinario
            WHERE 1=1
                AND (@Nombre IS NULL OR LOWER(Nombre) LIKE '%' + LOWER(@Nombre) + '%')
                AND (@Telefono IS NULL OR LOWER(Telefono) LIKE '%' + LOWER(@Telefono) + '%')
                AND (@Especialidad IS NULL OR LOWER(Especialidad) LIKE '%' + LOWER(@Especialidad) + '%')
            ORDER BY Id DESC;
        ";

        public static string VeterinarioQueryMySQL = @"
            SELECT Id, Nombre, Telefono, Especialidad
            FROM Veterinario
            WHERE 1=1
                AND (@Nombre IS NULL OR LOWER(Nombre) LIKE CONCAT('%', LOWER(@Nombre), '%'))
                AND (@Telefono IS NULL OR LOWER(Telefono) LIKE CONCAT('%', LOWER(@Telefono), '%'))
                AND (@Especialidad IS NULL OR LOWER(Especialidad) LIKE CONCAT('%', LOWER(@Especialidad), '%'))
            ORDER BY Id DESC;
        ";
    }
}