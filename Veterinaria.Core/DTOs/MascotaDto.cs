using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.DTOs
{
    /// <summary>
    /// DTO que representa una mascota.
    /// </summary>
    /// <remarks>
    /// Se utiliza para registrar y actualizar mascotas en el sistema.
    /// </remarks>
    public class MascotaDto
    {
        /// <summary>
        /// Identificador unico de la mascota.
        /// </summary>
        /// <example>10</example>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la mascota.
        /// </summary>
        /// <example>Neron</example>
        public string Nombre { get; set; }

        /// <summary>
        /// Especie de la mascota.
        /// </summary>
        /// <example>Perro</example>
        public string Especie { get; set; }

        /// <summary>
        /// Raza de la mascota.
        /// </summary>
        /// <example>Pastor Aleman</example>
        public string Raza { get; set; }

        /// <summary>
        /// Edad de la mascota en años.
        /// </summary>
        /// <example>3</example>
        public int Edad { get; set; }

        /// <summary>
        /// Identificador del dueño asociado.
        /// </summary>
        /// <example>1</example>
        public int DuenoId { get; set; }
    }
}
