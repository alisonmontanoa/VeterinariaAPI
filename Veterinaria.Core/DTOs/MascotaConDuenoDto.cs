using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.DTOs
{
    /// <summary>
    /// DTO para listar mascotas con informacion de su dueño
    /// </summary>
    /// /// <remarks>
    /// Se utiliza para consultar mascotas con sus dueños.
    /// </remarks>
    public class MascotaConDuenoDto
    {
        /// <summary>
        /// Identificador unico de la mascota.
        /// </summary>
        /// <example>2</example>
        public int MascotaId { get; set; }

        /// <summary>
        /// Nombre de la mascota.
        /// </summary>
        /// <example>Kiara</example>
        public string NombreMascota { get; set; } = null!;

        /// <summary>
        /// Especie de la mascota.
        /// </summary>
        /// <example>Gato</example>
        public string Especie { get; set; } = null!;

        /// <summary>
        /// Raza de la mascota.
        /// </summary>
        /// <example>Persa</example>
        public string Raza { get; set; } = null!;

        /// <summary>
        /// Edad de la mascota en años.
        /// </summary>
        /// <example>5</example>
        public int Edad { get; set; }

        // Dueno
        /// <summary>
        /// Nombre del dueño.
        /// </summary>
        /// <example>Carla Rios</example>
        public string NombreDueno { get; set; } = null!;

        /// <summary>
        /// Numero de telefono del dueño.
        /// </summary>
        /// <example>69472256</example>
        public string TelefonoDueno { get; set; } = null!;

        /// <summary>
        /// Direecion del domicilio del dueño.
        /// </summary>
        /// <example>Av. America #123</example>
        public string DireccionDueno { get; set; } = null!;
    }
}
