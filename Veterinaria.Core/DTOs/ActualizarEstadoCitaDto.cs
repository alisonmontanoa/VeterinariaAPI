using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.DTOs
{
    /// <summary>
    /// DTO para actualizar el estado de una cita
    /// </summary>
    public class ActualizarEstadoCitaDto
    {
        /// <summary>
        /// Identificador de la cita
        /// </summary>
        /// <example>5</example>
        public int CitaId { get; set; }

        /// <summary>
        /// Estado de la cita
        /// (Pendiente, Atendida, Cancelada)
        /// </summary>
        /// <example>Atendida</example>
        public string Estado { get; set; } = null!;
    }
}

