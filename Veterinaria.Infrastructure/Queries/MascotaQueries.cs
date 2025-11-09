using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Infrastructure.Queries
{
    public static class MascotaQueries
    {
        public static string MascotaQuerySqlServer = @"
            SELECT Id, Nombre, Especie, Raza, Edad, DuenoId
            FROM Mascota
            WHERE 1=1
                AND (@Especie IS NULL OR LOWER(Especie) = LOWER(@Especie))
                AND (@Raza IS NULL OR LOWER(Raza) LIKE '%' + LOWER(@Raza) + '%')
                AND (@DuenoId IS NULL OR DuenoId = @DuenoId)
            ORDER BY Id DESC;
        ";

        public static string MascotaQueryMySQL = @"
            SELECT Id, Nombre, Especie, Raza, Edad, DuenoId
            FROM Mascota
            WHERE 1=1
                AND (@Especie IS NULL OR LOWER(Especie) = LOWER(@Especie))
                AND (@Raza IS NULL OR LOWER(Raza) LIKE CONCAT('%', LOWER(@Raza), '%'))
                AND (@DuenoId IS NULL OR DuenoId = @DuenoId)
            ORDER BY Id DESC;
        ";
    }
}