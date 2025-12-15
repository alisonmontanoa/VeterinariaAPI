using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Enums;

namespace Veterinaria.Core.DTOs
{
    /// <summary>
    /// DTO para registrar usuarios del sistema.
    /// </summary>
    /// <remarks>
    /// Solo accesible por usuarios con rol Administrador.
    /// </remarks>
    public class SecurityDto
    {
        /// <summary>
        /// Nombre completo del usuario.
        /// </summary>
        /// <example>Administrador</example>
        public string Name { get; set; }

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
        /// Rol asignado al usuario.
        /// </summary>
        /// <example>Administrador</example>
        public RoleType? Role { get; set; }
    }
}