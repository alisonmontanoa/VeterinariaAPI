using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.QueryFilters 
{
    /// <summary>
    /// Filtros de busqueda para dueños.
    /// </summary>
    /// <remarks>
    /// Permite aplicar filtros opcionales para consultar dueños
    /// junto con parametros de paginacion.
    /// 
    /// Los filtros pueden combinarse entre si.
    /// </remarks>
    public class DuenoQueryFilter : PaginationQueryFilter
    {
        /// Nombre completo del dueño.
        /// </summary>
        /// <remarks>
        /// Permite realizar busquedas parciales por nombre.
        /// </remarks>
        /// <example>Alejandra Sandoval</example>
        public string? Nombre { get; set; }

        /// Direccion de la vivienda del dueño.
        /// </summary>
        /// <remarks>
        /// Permite realizar busquedas parciales por direccion.
        /// </remarks>
        /// <example>Av. Circunvalacion</example>
        public string? Direccion { get; set; }

        /// Numero de telefono del dueño.
        /// </summary>
        /// <remarks>
        /// Permite realizar busquedas parciales por numero de telefono.
        /// </remarks>
        /// <example>65821499</example>
        public string? Telefono { get; set; }
    }
}
