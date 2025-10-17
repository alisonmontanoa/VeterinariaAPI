using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.Entities
{
    public class Veterinario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Especialidad { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
