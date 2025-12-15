using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.Entities
{
    /// <summary>
    /// Representa a los veterinarios dentro del sistema veterinario.
    /// </summary>
    /// <remarks>
    /// Esta entidad almacena la informacion principal de un veterinario, incluyendo nombre,
    /// especialidad y telefono.  
    /// Un veterinario puede tener una o varias citas asociadas.
    /// </remarks>
    public class Veterinario : BaseEntity
    {
        /// <summary>
        /// Nombre completo del veterinario.
        /// </summary>
        /// <example>Dra. Lucia Soliz</example>
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Especialidad del veterinario.
        /// </summary>
        /// <example>Cardiologia</example>
        public string Especialidad { get; set; } = null!;

        /// <summary>
        /// Numero de telefono del veterinario.
        /// </summary>
        /// <example>74596213</example>
        public string Telefono { get; set; } = null!;

        /// <summary>
        /// Lista de citas asociadas al veterinario.
        /// </summary>
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
