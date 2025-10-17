using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.Entities
{
    public class Cita
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public int MascotaId { get; set; }
        public int VeterinarioId { get; set; }
        public int ServicioId { get; set; }

        public Mascota Mascota { get; set; } = null!;
        public Veterinario Veterinario { get; set; } = null!;
        public Servicio Servicio { get; set; } = null!;
    }
}
