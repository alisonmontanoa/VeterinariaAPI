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
    public class DuenoRepository : IDuenoRepository
    {
        private readonly VeterinariaContext _context;
        public DuenoRepository(VeterinariaContext context) => _context = context;

        public Task<Dueno?> GetByIdAsync(int id) =>
            _context.Duenos.FirstOrDefaultAsync(d => d.Id == id);

        public Task<Dueno?> GetByTelefonoAsync(string telefono) =>
            _context.Duenos.FirstOrDefaultAsync(d => d.Telefono == telefono);

        public async Task AddAsync(Dueno entity)
        {
            _context.Duenos.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
