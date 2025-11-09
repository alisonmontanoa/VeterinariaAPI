using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.DTOs;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Exceptions;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.QueryFilters;

namespace Veterinaria.Core.Services
{
    public class VeterinarioService : IVeterinarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VeterinarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> RegistrarVeterinarioAsync(VeterinarioDto veterinarioDto)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(veterinarioDto.Nombre) ||
                string.IsNullOrWhiteSpace(veterinarioDto.Especialidad) ||
                string.IsNullOrWhiteSpace(veterinarioDto.Telefono))
            {
                throw new BusinessException("Todos los campos del veterinario son requeridos.", 400);
            }

            // Verificar si el telefono ya existe
            var existe = await _unitOfWork.Veterinarios.ExistsByTelefonoAsync(veterinarioDto.Telefono);
            if (existe)
            {
                throw new BusinessException($"El telefono {veterinarioDto.Telefono} ya esta registrado.", 409);
            }

            // Crear entidad
            var veterinario = new Veterinario
            {
                Nombre = veterinarioDto.Nombre.Trim(),
                Especialidad = veterinarioDto.Especialidad.Trim(),
                Telefono = veterinarioDto.Telefono.Trim()
            };

            // Guardar
            await _unitOfWork.Veterinarios.AddAsync(veterinario);
            await _unitOfWork.SaveChangesAsync();

            return veterinario.Id;


        }
        public async Task<IEnumerable<Veterinario>> ObtenerVeterinariosAsync(VeterinarioQueryFilter filters)
        {
            var veterinarios = await _unitOfWork.Veterinarios.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(filters.Nombre))
                veterinarios = veterinarios.Where(v => v.Nombre.Contains(filters.Nombre, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filters.Especialidad))
                veterinarios = veterinarios.Where(v => v.Especialidad.Contains(filters.Especialidad, StringComparison.OrdinalIgnoreCase));

            return veterinarios;
        }
    }
}