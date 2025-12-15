using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.CustomEntities;
using Veterinaria.Core.DTOs;
using Veterinaria.Core.Entities;
using Veterinaria.Core.QueryFilters;

namespace Veterinaria.Core.Interfaces
{
    public interface IMascotaService
    {
        Task<int> RegistrarMascotaAsync(MascotaDto mascotaDto);
        Task<PagedList<MascotaDto>> ObtenerMascotasAsync(MascotaQueryFilter filters);
        Task<PagedList<MascotaConDuenoDto>>ListarMascotasConDuenoAsync(MascotaConDuenoQueryFilter filters);
        Task<IEnumerable<MascotasPorRazaServiciosDto>> ObtenerServiciosPorRazaAsync();
    }
}
