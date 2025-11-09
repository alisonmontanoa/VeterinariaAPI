using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.DTOs;
using Veterinaria.Core.Entities;
using Veterinaria.Core.QueryFilters;

namespace Veterinaria.Core.Interfaces
{
    public interface IMascotaService
    {
        Task<int> RegistrarMascotaAsync(MascotaDto mascotaDto);
        Task<IEnumerable<Mascota>> ObtenerMascotasAsync(MascotaQueryFilter filters);

    }
}
