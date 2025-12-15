using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.QueryFilters
{
    /// <summary>
    /// Filtros de basqueda para mascotas con sus dueños.
    /// </summary>
    /// <remarks>
    /// Permite aplicar filtros opcionales para consultar mascotas con dueños
    /// junto con parametros de paginacion.
    /// 
    /// Los filtros pueden combinarse entre si.
    /// </remarks>
    public class MascotaConDuenoQueryFilter : PaginationQueryFilter
    {
        /// Nombre de la mascota.
        /// </summary>
        /// <remarks>
        /// Permite realizar busquedas parciales por nombre.
        /// </remarks>
        /// <example>Luna</example>
        public string? NombreMascota { get; set; }

        /// <summary>
        /// Especie de la mascota.
        /// </summary>
        /// <remarks>
        /// Ejemplos comunes: Perro, Gato.
        /// </remarks>
        /// <example>Gato</example>
        public string? Especie { get; set; }
    }
}
