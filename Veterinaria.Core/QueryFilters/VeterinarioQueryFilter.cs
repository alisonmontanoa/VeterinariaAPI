using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.QueryFilters
{
    /// <summary>
    /// Filtros de busqueda para veterinarios.
    /// </summary>
    /// <remarks>
    /// Permite aplicar filtros opcionales para consultar veterinarios
    /// junto con parametros de paginacion.
    /// 
    /// Los filtros pueden combinarse entre si.
    /// </remarks>
    public class VeterinarioQueryFilter : PaginationQueryFilter
    {
        /// Nombre completo del veterinario.
        /// </summary>
        /// <remarks>
        /// Permite realizar busquedas parciales por nombre.
        /// </remarks>
        /// <example>Dr. Jorge Perez</example>
        public string? Nombre { get; set; }

        /// Especialidad del veterinario.
        /// </summary>
        /// <remarks>
        /// Permite realizar busquedas parciales por especialidad.
        /// </remarks>
        /// <example>Nutricion</example>
        public string? Especialidad { get; set; }

        /// Numero de telefono del veterinario.
        /// </summary>
        /// <remarks>
        /// Permite realizar busquedas parciales por numero de telefono.
        /// </remarks>
        /// <example>78652259</example>
        public string? Telefono { get; set; }
    }
}
