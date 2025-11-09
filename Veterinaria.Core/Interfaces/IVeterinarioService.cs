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
    public interface IVeterinarioService
    {
        Task<int> RegistrarVeterinarioAsync(VeterinarioDto veterinarioDto);
        Task<IEnumerable<Veterinario>> ObtenerVeterinariosAsync(VeterinarioQueryFilter filters);

    }
}
