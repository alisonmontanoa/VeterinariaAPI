using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;

namespace Veterinaria.Core.Interfaces
{
    public interface IServicioRepository : IBaseRepository<Servicio>
    {
        Task<bool> ExistsByNombreAsync(string nombre);

    }
}
