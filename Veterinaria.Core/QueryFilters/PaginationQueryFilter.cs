using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.QueryFilters
{
    /// <summary>
    /// Filtro base para manejar paginacion en las consultas.
    /// </summary>
    /// <remarks>
    /// Esta clase define los parametros estandar de paginacion
    /// que seran heredados por los filtros de consulta especificos.
    /// </remarks>
    public abstract class PaginationQueryFilter
    {
        /// <summary>
        /// Numero de registros a retornar por pagina.
        /// </summary>
        /// <remarks>
        /// Define el tamaño de la pagina.
        /// Valor recomendado entre 5 y 50.
        /// </remarks>
        /// <example>10</example>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Numero de la pagina actual.
        /// </summary>
        /// <remarks>
        /// La numeracion comienza desde 1.
        /// </remarks>
        /// <example>1</example>
        public int PageNumber { get; set; } = 1;
    }
}
