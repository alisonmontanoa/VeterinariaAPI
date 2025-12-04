using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Interfaces;
using Veterinaria.Infrastructure.Data;
using Veterinaria.Infrastructure.Repositories;

namespace Veterinaria.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VeterinariaContext _context;
        private readonly IDapperContext _dapper;

        private IDuenoRepository? _duenos;
        private IMascotaRepository? _mascotas;
        private ICitaRepository? _citas;
        private IServicioRepository? _servicios;
        private IVeterinarioRepository? _veterinarios;
        private ISecurityRepository _securityRepository;

        public UnitOfWork(VeterinariaContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }
        public VeterinariaContext Context => _context;

        public IDapperContext Dapper => _dapper;

        // Repositorios
        public IDuenoRepository Duenos => _duenos ??= new DuenoRepository(_context, _dapper);
        public IMascotaRepository Mascotas => _mascotas ??= new MascotaRepository(_context, _dapper);
        public ICitaRepository Citas => _citas ??= new CitaRepository(_context, _dapper);
        public IServicioRepository Servicios => _servicios ??= new ServicioRepository(_context, _dapper);
        public IVeterinarioRepository Veterinarios => _veterinarios ??= new VeterinarioRepository(_context, _dapper);
        public ISecurityRepository SecurityRepository => _securityRepository ?? new SecurityRepository(_context);

        public void SaveChanges() => _context.SaveChanges();
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
