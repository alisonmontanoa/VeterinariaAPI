using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.Entities
{
    /// <summary>
    /// Representa un dueño dentro del sistema veterinario.
    /// </summary>
    /// <remarks>
    /// Esta entidad almacena la informacion principal del dueño, incluyendo su nombre,
    /// direccion y telefono.  
    /// Un dueño puede tener una o varias mascotas asociadas.
    /// </remarks>
    public class Dueno : BaseEntity
    {
        /// <summary>
        /// Nombre completo del dueño.
        /// </summary>
        /// <example>Carlos Gomez</example>
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Numero de telefono del dueño. Debe ser unico.
        /// </summary>
        /// <example>77712345</example>
        public string Telefono { get; set; } = null!;

        /// <summary>
        /// Direccion del domicilio del dueño.
        /// </summary>
        /// <example>Av. Blanco Galindo #1200</example>
        public string Direccion { get; set; } = null!;

        /// <summary>
        /// Lista de mascotas asociadas al dueño.
        /// </summary>
        public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
    }
}
