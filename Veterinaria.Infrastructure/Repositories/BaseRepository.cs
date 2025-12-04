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
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly VeterinariaContext _context;
        protected readonly DbSet<T> _entities;

        public BaseRepository(VeterinariaContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        // -------------------
        // Metodos sincronos
        // -------------------
        public IEnumerable<T> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public T? GetById(int id)
        {
            return _entities.Find(id);
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public void Delete(int id)
        {
            var entity = _entities.Find(id);
            if (entity != null)
                _entities.Remove(entity);
        }

        // -------------------
        // Metodos asincronos
        // -------------------
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _entities.FindAsync(id);
            if (entity != null)
                _entities.Remove(entity);
        }
    }
}