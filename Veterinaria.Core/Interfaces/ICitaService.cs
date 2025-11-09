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
    public interface ICitaService
    {
        Task<int> CrearCitaAsync(CitaDto citaDto);
        Task CancelarCitaAsync(int citaId);
        Task<IEnumerable<Cita>> ObtenerCitasAsync(CitaQueryFilter filters);
    }
}
