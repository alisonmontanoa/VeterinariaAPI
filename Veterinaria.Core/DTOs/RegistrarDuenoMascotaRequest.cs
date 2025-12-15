using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.DTOs
{
    /// <summary>
    /// DTO que agrupa la informacion del dueño y su mascota para el registro conjunto.
    /// </summary>
    public class RegistrarDuenoMascotaRequest
    {
        /// <summary>
        /// Datos del dueño a registrar.
        /// </summary>
        public DuenoDto Dueno { get; set; } = null!;

        /// <summary>
        /// Datos de la mascota a registrar.
        /// </summary>
        public MascotaDto Mascota { get; set; } = null!;
    }
}
