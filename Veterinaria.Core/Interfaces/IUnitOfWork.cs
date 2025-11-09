using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;

namespace Veterinaria.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDuenoRepository Duenos { get; }
        IMascotaRepository Mascotas { get; }
        ICitaRepository Citas { get; }
        IServicioRepository Servicios { get; }
        IVeterinarioRepository Veterinarios { get; }

        IDapperContext Dapper { get; }

        Task SaveChangesAsync();
        void SaveChanges();
    }
}
