using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.DTOs
{
    public class CitaDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public int MascotaId { get; set; }
        public int VeterinarioId { get; set; }
        public int ServicioId { get; set; }
    }
}
