using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.DTOs
{
    /// <summary>
    /// DTO que representa la informacion de un veterinario.
    /// </summary>
    /// <remarks>
    /// Se utiliza para registrar y consultar veterinarios dentro del sistema veterinario.
    /// </remarks>
    public class VeterinarioDto
    {
        /// <summary>
        /// Identificador unico del veterinario.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Nombre completo del veterinario.
        /// </summary>
        /// <example>Dra. Rebeca Gonzales</example>
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Especialidad del veterinario.
        /// </summary>
        /// <example>Medicina interna</example>
        public string Especialidad { get; set; } = null!;

        /// <summary>
        /// Numero de telefono del veterinario.
        /// </summary>
        /// <example>68521147</example>
        public string Telefono { get; set; } = null!;
    }
}
