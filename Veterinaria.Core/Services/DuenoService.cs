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
    public class DuenoService : IDuenoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDapperContext _dapper;

        private readonly string[] ForbiddenWords =
        {
            "odio", "violencia", "groseria", "discriminacion", "pornografia"
        };

        public DuenoService(IUnitOfWork unitOfWork, IMapper mapper, IDapperContext dapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dapper = dapper;
        }

        private bool ContainsForbiddenContent(string text)
        {
            return !string.IsNullOrWhiteSpace(text) &&
                   ForbiddenWords.Any(w => text.Contains(w, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<(int duenoId, int mascotaId)> RegistrarDuenoConMascotaAsync(DuenoDto duenoDto, MascotaDto mascotaDto)
        {
            // === VALIDACIONES DEL DUEÑO ===
            if (ContainsForbiddenContent(duenoDto.Nombre))
                throw new BusinessException("El nombre del duenio contiene lenguaje inapropiado.", 400);

            if (string.IsNullOrWhiteSpace(duenoDto.Nombre) ||
                string.IsNullOrWhiteSpace(duenoDto.Direccion) ||
                string.IsNullOrWhiteSpace(duenoDto.Telefono))
                throw new BusinessException("Todos los campos del duenio son obligatorios.", 400);

            var duenios = await _unitOfWork.Duenos.GetAllAsync();
            var duenoExistente = duenios.FirstOrDefault(d => d.Telefono == duenoDto.Telefono);

            Dueno dueno;
            if (duenoExistente == null)
            {
                dueno = _mapper.Map<Dueno>(duenoDto);
                await _unitOfWork.Duenos.AddAsync(dueno);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                dueno = duenoExistente;
            }

            // Validar que el duenio tenga un Id valido (tras guardar)
            if (dueno.Id <= 0)
                throw new BusinessException("Debe asociar un dueño valido.", 400);

            // === VALIDACIONES DE LA MASCOTA ===
            if (string.IsNullOrWhiteSpace(mascotaDto.Nombre) ||
                string.IsNullOrWhiteSpace(mascotaDto.Especie) ||
                string.IsNullOrWhiteSpace(mascotaDto.Raza))
                throw new BusinessException("Todos los campos de la mascota son obligatorios.", 400);

            if (ContainsForbiddenContent(mascotaDto.Nombre))
                throw new BusinessException("El nombre de la mascota contiene lenguaje inapropiado.", 400);

            var mascotas = await _unitOfWork.Mascotas.GetAllAsync();
            var mascotasDueno = mascotas.Where(m => m.DuenoId == dueno.Id);
            if (mascotasDueno.Count() >= 3)
                throw new BusinessException("El duenoo ya tiene el maximo de 3 mascotas registradas.", 400);

            // === CREACION DE LA MASCOTA ===
            var mascota = _mapper.Map<Mascota>(mascotaDto);
            mascota.DuenoId = dueno.Id;

            await _unitOfWork.Mascotas.AddAsync(mascota);
            await _unitOfWork.SaveChangesAsync();

            return (dueno.Id, mascota.Id);
        }

        public async Task<IEnumerable<Dueno>> ObtenerDuenosAsync(DuenoQueryFilter filters)
        {
            var duenios = await _unitOfWork.Duenos.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(filters.Nombre))
                duenios = duenios.Where(d => d.Nombre.Contains(filters.Nombre, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filters.Direccion))
                duenios = duenios.Where(d => d.Direccion.Contains(filters.Direccion, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filters.Telefono))
                duenios = duenios.Where(d => d.Telefono.Contains(filters.Telefono));

            return duenios.OrderByDescending(d => d.Id);
        }
    }
}