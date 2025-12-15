using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.Entities
{
    /// <summary>
    /// Representa las citas dentro del sistema veterinario.
    /// </summary>
    /// <remarks>
    /// Esta entidad almacena la informacion principal de una cita, incluyendo fecha de cita,
    /// motivo y estado.  
    /// </remarks>
    public class Cita : BaseEntity
    {
        /// <summary>
        /// Fecha programada de la cita.
        /// </summary>
        /// <example>2025-06-11</example>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Motivo de la consulta.
        /// </summary>
        /// <example>Vacunacion</example>
        public string Motivo { get; set; } = null!;

        /// <summary>
        /// Estado de la Cita.
        /// </summary>
        /// <example>Pendiente</example>
        public string Estado { get; set; } = null!;

        /// <summary>
        /// Identificador de la mascota que tiene la cita programada.
        /// </summary>
        /// <example>5</example>
        public int MascotaId { get; set; }

        /// <summary>
        /// Identificador del veterinario que tiene la cita programada.
        /// </summary>
        /// <example>2</example>
        public int VeterinarioId { get; set; }

        /// <summary>
        /// Identificador del servicio que pertenece a la cita programada.
        /// </summary>
        /// <example>7</example>
        public int ServicioId { get; set; }

        /// <summary>
        /// Referencia a la mascota asociada.
        /// </summary>
        public Mascota Mascota { get; set; } = null!;

        /// <summary>
        /// Referencia al veterinario asociado.
        /// </summary>
        public Veterinario Veterinario { get; set; } = null!;

        /// <summary>
        /// Referencia al servicio asociado.
        /// </summary>
        public Servicio Servicio { get; set; } = null!;
    }
}
