using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.DTOs
{
    /// <summary>
    /// DTO para la gestion de citas veterinarias.
    /// </summary>
    /// <remarks>
    /// Se utiliza para agendar y consultar citas.
    /// </remarks>
    public class CitaDto
    {
        // <summary>
        /// Identificador de la cita.
        /// </summary>
        /// <example>5</example>
        public int Id { get; set; }

        /// <summary>
        /// Fecha programada de la cita.
        /// </summary>
        /// <example>2025-06-10</example>
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
        /// Identificador de la mascota.
        /// </summary>
        /// <example>10</example>
        public int MascotaId { get; set; }

        /// <summary>
        /// Identificador del veterinario.
        /// </summary>
        /// <example>1</example>
        public int VeterinarioId { get; set; }

        /// <summary>
        /// Identificador del Servicio.
        /// </summary>
        /// <example>3</example>
        public int ServicioId { get; set; }
    }
}
