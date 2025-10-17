using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;

namespace Veterinaria.Core.Interfaces
{
    public interface IDuenoRepository
    {
        Task<Dueno?> GetByIdAsync(int id);
        Task<Dueno?> GetByTelefonoAsync(string telefono);
        Task AddAsync(Dueno entity);
        Task SaveChangesAsync();

    }
}
