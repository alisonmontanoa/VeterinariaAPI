using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.DTOs
{
    public class MascotasPorRazaServiciosDto
    {
        public string Raza { get; set; } = null!;
        public int CantidadServicios { get; set; }
    }
}
