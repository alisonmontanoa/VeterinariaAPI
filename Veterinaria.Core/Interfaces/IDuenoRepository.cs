using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;

namespace Veterinaria.Core.Interfaces
{
    public interface IDuenoRepository : IBaseRepository<Dueno>
    {
        Task<Dueno?> GetByTelefonoAsync(string telefono);
    }
}
