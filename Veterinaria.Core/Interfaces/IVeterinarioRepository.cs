using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;

namespace Veterinaria.Core.Interfaces
{
    public interface IVeterinarioRepository : IBaseRepository<Veterinario>
    {
        Task<bool> ExistsByTelefonoAsync(string telefono);
    }
}
