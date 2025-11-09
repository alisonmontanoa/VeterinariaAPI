using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.QueryFilters
{
    public class CitaQueryFilter
    {
        public int? VeterinarioId { get; set; }
        public int? MascotaId { get; set; }
        public int? DuenoId { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Estado { get; set; }
    }
}
