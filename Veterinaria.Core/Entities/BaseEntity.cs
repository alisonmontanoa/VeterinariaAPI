using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.Entities
{
    /// <summary>
    /// Esta clase define el identificador
    /// que sera heredado por las entidades especificas.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Identificador base que se puede reutilizar en varias entidades.
        /// </summary>
        /// <example>2</example>
        public int Id { get; set; }
    }
}
