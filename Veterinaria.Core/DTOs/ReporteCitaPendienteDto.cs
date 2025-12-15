using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.DTOs
{
    /// <summary>
    /// DTO para el reporte de citas pendientes.
    /// </summary>
    /// <remarks>
    /// Se utiliza exclusivamente para el Caso de Uso 3.
    /// </remarks>
    public class ReporteCitaPendienteDto
    {
        /// <summary>
        /// Identificador de la cita.
        /// </summary>
        /// <example>7</example>
        public int CitaId { get; set; }

        /// <summary>
        /// Fecha de la cita.
        /// </summary>
        /// <example>2025-06-12</example>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Motivo de la cita.
        /// </summary>
        /// <example>Control general</example>
        public string Motivo { get; set; }

        // Mascota
        /// <summary>
        /// Nombre de la mascota.
        /// </summary>
        /// <example>Charlie</example>
        public string NombreMascota { get; set; } = null!;

        /// <summary>
        /// Especie de la mascota.
        /// </summary>
        /// <example>Gato</example>
        public string Especie { get; set; } = null!;

        // Dueno
        /// <summary>
        /// Nombre del dueño.
        /// </summary>
        /// <example>Natalia Soliz</example>
        public string NombreDueno { get; set; } = null!;

        /// <summary>
        /// Numero de telefono del dueño.
        /// </summary>
        /// <example>78921566</example>
        public string TelefonoDueno { get; set; } = null!;

        // Veterinario
        /// <summary>
        /// Nombre del veterinario.
        /// </summary>
        /// <example>Dr. Israel Diaz</example>
        public string NombreVeterinario { get; set; } = null!;

        /// <summary>
        /// Especialidad del veterinario.
        /// </summary>
        /// <example>Cardiologia</example>
        public string Especialidad { get; set; } = null!;
    }
}
