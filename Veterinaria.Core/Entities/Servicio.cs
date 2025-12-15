using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.Entities
{
    /// <summary>
    /// Representa los servicios veterinarios dentro del sistema veterinario.
    /// </summary>
    /// <remarks>
    /// Esta entidad almacena la informacion principal de un servicio, incluyendo nombre,
    /// descripcion y precio.  
    /// Un servicio puede tener una o varias citas asociadas.
    /// </remarks>
    public class Servicio : BaseEntity
    {
        /// <summary>
        /// Nombre del servicio.
        /// </summary>
        /// <example>Vacunacion</example>
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Descripcion del servicio.
        /// </summary>
        /// <example>Vacunacion contra la rabia canina</example>
        public string Descripcion { get; set; } = null!;

        /// <summary>
        /// Precio del servicio.
        /// </summary>
        /// <example>12.00</example>
        public decimal Precio { get; set; }

        /// <summary>
        /// Lista de citas asociadas al servicio.
        /// </summary>
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
