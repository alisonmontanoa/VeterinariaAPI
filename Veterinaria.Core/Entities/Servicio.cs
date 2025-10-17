using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.Entities
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!; 
        public decimal Precio { get; set; }
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
