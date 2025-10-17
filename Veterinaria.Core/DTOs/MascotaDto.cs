using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.DTOs
{
    public class MascotaDto
    {
        public int Id { get; set; }           
        public string Nombre { get; set; } = null!;
        public string Especie { get; set; } = null!;
        public string Raza { get; set; } = null!;
        public int Edad { get; set; }
        public int DuenoId { get; set; }      
    }
}
