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
    public interface IServicioService
    {
        Task<int> CrearServicioAsync(ServicioDto servicioDto);
        Task<PagedList<ServicioDto>> ObtenerServiciosAsync(ServicioQueryFilter filters);
    }
}
