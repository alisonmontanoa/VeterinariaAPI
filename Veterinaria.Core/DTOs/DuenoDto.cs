using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.DTOs
{
    /// <summary>
    /// DTO que representa la informacion de un dueño.
    /// </summary>
    /// <remarks>
    /// Se utiliza para registrar y consultar dueños dentro del sistema veterinario.
    /// </remarks>
    public class DuenoDto
    {
        /// <summary>
        /// Identificador unico del dueño.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Nombre completo del dueño.
        /// </summary>
        /// <example>Carlos Gomez</example>
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Numero de telefono del dueño.
        /// </summary>
        /// <example>77712345</example>
        public string Telefono { get; set; } = null!;

        /// <summary>
        /// Direccion del domicilio del dueño.
        /// </summary>
        /// <example>Calle Calama #564</example>
        public string Direccion { get; set; } = null!;
    }
}
