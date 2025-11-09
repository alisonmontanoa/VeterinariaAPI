using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;

namespace Veterinaria.Core.Interfaces
{
    public interface IMascotaRepository : IBaseRepository<Mascota>
    {
        Task<int> CountByDuenoIdAsync(int duenoId);
        Task<IEnumerable<Mascota>> GetByDuenoIdAsync(int duenoId);
    }
}
