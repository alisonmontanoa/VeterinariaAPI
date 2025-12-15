using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.DTOs
{
    /// <summary>
    /// DTO que representa la informacion de un servicio.
    /// </summary>
    /// <remarks>
    /// Se utiliza para crear y filtrar servicios dentro del sistema veterinario.
    /// </remarks>
    public class ServicioDto
    {
        /// <summary>
        /// Identificador unico del servicio.
        /// </summary>
        /// <example>5</example>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del servicio.
        /// </summary>
        /// <example>Vacunacion</example>
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Descripcion del servicio.
        /// </summary>
        /// <example>Vacuna Antirrabica</example>
        public string Descripcion { get; set; } = null!;

        /// <summary>
        /// Precio del servicio.
        /// </summary>
        /// <example>10.00</example>
        public decimal Precio { get; set; }
    }
}
