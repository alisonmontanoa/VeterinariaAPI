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
    public interface IVeterinarioService
    {
        Task<int> RegistrarVeterinarioAsync(VeterinarioDto veterinarioDto);
        Task<PagedList<VeterinarioDto>> ObtenerVeterinariosAsync(VeterinarioQueryFilter filters);
    }
}
