using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.Entities
{
    /// <summary>
    /// Representa una mascota registrada dentro del sistema veterinario.
    /// </summary>
    /// <remarks>
    /// Cada mascota debe estar asociada a un dueño previamente registrado.
    /// Contiene datos como especie, raza y edad.
    /// </remarks>
    public class Mascota : BaseEntity
    {
        /// <summary>
        /// Nombre de la mascota.
        /// </summary>
        /// <example>Firulais</example>
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Especie de la mascota (perro, gato, conejo, etc.).
        /// </summary>
        /// <example>Perro</example>
        public string Especie { get; set; } = null!;

        /// <summary>
        /// Raza de la mascota.
        /// </summary>
        /// <example>Pastor Aleman</example>
        public string Raza { get; set; } = null!;

        /// <summary>
        /// Edad de la mascota expresada en años.
        /// </summary>
        /// <example>3</example>
        public int Edad { get; set; }

        /// <summary>
        /// Identificador del dueño al que pertenece esta mascota.
        /// </summary>
        /// <example>7</example>
        public int DuenoId { get; set; }

        /// <summary>
        /// Referencia al dueño asociado.
        /// </summary>
        public Dueno Dueno { get; set; } = null!;

        /// <summary>
        /// Lista de citas veterinarias asociadas a esta mascota.
        /// </summary>
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
