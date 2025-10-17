using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.Entities
{
    public class Mascota
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Especie { get; set; } = null!;
        public string Raza { get; set; } = null!;
        public int Edad { get; set; }
        public int DuenoId { get; set; }
        public Dueno Dueno { get; set; } = null!;
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
