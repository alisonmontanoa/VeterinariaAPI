using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Enums;

namespace Veterinaria.Core.Entities
{
    /// <summary>
    /// Representa un usuario del sistema
    /// </summary>
    public class Security : BaseEntity
    {
        /// <summary>
        /// Login del usuario.
        /// </summary>
        /// <example>admin</example>
        public string Login { get; set; }

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        /// <example>admin123</example>
        public string Password { get; set; }

        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        /// <example>Administrador</example>
        public string Name { get; set; }

        /// <summary>
        /// Rol asignado al usuario.
        /// </summary>
        /// <example>Administrador</example>
        public RoleType Role { get; set; }
    }
}
