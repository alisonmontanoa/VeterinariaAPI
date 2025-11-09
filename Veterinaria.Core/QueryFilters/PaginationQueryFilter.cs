using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.QueryFilters
{
    public abstract class PaginationQueryFilter
    {
        /// <summary>
        /// Cantidad de registros por pagina.
        /// </summary>
        /// <example>10</example>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Numero de pagina actual.
        /// </summary>
        /// <example>1</example>
        public int PageNumber { get; set; } = 1;
    }
}
