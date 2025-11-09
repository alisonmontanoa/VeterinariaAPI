using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;

namespace Veterinaria.Core.Interfaces
{
    public interface ICitaRepository : IBaseRepository<Cita>
    {
        Task<int> CountPendientesByMascotaAsync(int mascotaId);
        Task<IEnumerable<Cita>> GetCitasByVeterinarioAsync(int veterinarioId);
    }
}
