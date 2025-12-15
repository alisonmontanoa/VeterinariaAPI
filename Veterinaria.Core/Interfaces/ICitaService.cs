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
    public interface ICitaService
    {
        Task<int> CrearCitaAsync(CitaDto citaDto);
        Task CancelarCitaAsync(int citaId);
        Task<PagedList<CitaDto>> ObtenerCitasAsync(CitaQueryFilter filters);
        Task ActualizarEstadoCitaAsync(ActualizarEstadoCitaDto dto);
        Task<PagedList<ReporteCitaPendienteDto>> ObtenerReporteCitasPendientesAsync(PaginationQueryFilter filters);
    }
}
