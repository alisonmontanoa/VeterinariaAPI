using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Interfaces;
using Veterinaria.Infrastructure.Data;

namespace Veterinaria.Infrastructure.Repositories
{
    public class MascotaRepository : IMascotaRepository
    {
        private readonly VeterinariaContext _context;
        public MascotaRepository(VeterinariaContext context) => _context = context;

        public async Task AddAsync(Mascota entity)
        {
            _context.Mascotas.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
