using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.CustomEntities
{
    public class Pagination
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }

        public Pagination() { }

        public Pagination(PagedList<object> list)
        {
            TotalCount = list.TotalCount;
            PageSize = list.PageSize;
            CurrentPage = list.CurrentPage;
            TotalPages = list.TotalPages;
            HasNextPage = list.HasNextPage;
            HasPreviousPage = list.HasPreviousPage;
        }
    }
}
