using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.CustomEntities;
using Veterinaria.Core.DTOs;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Exceptions;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.QueryFilters;

namespace Veterinaria.Core.Services
{
    public class MascotaService : IMascotaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MascotaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> RegistrarMascotaAsync(MascotaDto mascotaDto)
        {
            // Regla 1: El dueno debe existir
            var dueno = await _unitOfWork.Duenos.GetByIdAsync(mascotaDto.DuenoId);
            if (dueno == null)
                throw new BusinessException("No se puede registrar una mascota sin un duenio valido.");

            // Regla 2: Evitar lenguaje ofensivo
            string[] palabrasProhibidas = { "odio", "violencia", "groseria", "discriminacion", "pornografia" };
            if (palabrasProhibidas.Any(p => mascotaDto.Nombre.Contains(p, StringComparison.OrdinalIgnoreCase)))
                throw new BusinessException("El nombre de la mascota contiene lenguaje inapropiado.");

            // Regla 3: Verificar duplicados
            var mascotasDelDueno = (await _unitOfWork.Mascotas.GetAllAsync())
                .Where(m => m.DuenoId == mascotaDto.DuenoId);

            if (mascotasDelDueno.Any(m => m.Nombre.Equals(mascotaDto.Nombre, StringComparison.OrdinalIgnoreCase)))
                throw new BusinessException("El dueno ya tiene una mascota con ese nombre.");

            // Regla 4: Maximo 3 mascotas
            if (mascotasDelDueno.Count() >= 3)
                throw new BusinessException("El dueno no puede registrar mas de 3 mascotas.");

            // Registro
            var mascota = _mapper.Map<Mascota>(mascotaDto);
            await _unitOfWork.Mascotas.AddAsync(mascota);
            await _unitOfWork.SaveChangesAsync();

            return mascota.Id;
        }

        public async Task<Mascota?> ObtenerMascotaPorIdAsync(int id)
        {
            return await _unitOfWork.Mascotas.GetByIdAsync(id);
        }

        public async Task ActualizarMascotaAsync(MascotaDto mascotaDto)
        {
            var mascotaExistente = await _unitOfWork.Mascotas.GetByIdAsync(mascotaDto.Id);
            if (mascotaExistente == null)
                throw new BusinessException("La mascota no existe.");

            mascotaExistente.Nombre = mascotaDto.Nombre;
            mascotaExistente.Raza = mascotaDto.Raza;
            mascotaExistente.Especie = mascotaDto.Especie;
            mascotaExistente.Edad = mascotaDto.Edad;

            _unitOfWork.Mascotas.Update(mascotaExistente);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EliminarMascotaAsync(int id)
        {
            await _unitOfWork.Mascotas.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedList<MascotaDto>> ObtenerMascotasAsync(MascotaQueryFilter filters)
        {
            var mascotas = await _unitOfWork.Mascotas.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(filters.Especie))
                mascotas = mascotas.Where(m => m.Especie.Equals(filters.Especie, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filters.Raza))
                mascotas = mascotas.Where(m => m.Raza.Contains(filters.Raza, StringComparison.OrdinalIgnoreCase));

            if (filters.DuenoId.HasValue)
                mascotas = mascotas.Where(m => m.DuenoId == filters.DuenoId.Value);

            if (filters.Edad.HasValue)
                mascotas = mascotas.Where(m => m.Edad == filters.Edad.Value);

            var mascotasDto = _mapper.Map<IEnumerable<MascotaDto>>(mascotas);

            return PagedList<MascotaDto>.Create(
                mascotasDto,
                filters.PageNumber,
                filters.PageSize
            );
        }

        // Obtener Mascotas con Dueno
        private async Task<IEnumerable<Mascota>> ObtenerMascotasConDuenoBaseAsync()
        {
            var mascotas = await _unitOfWork.Mascotas.GetAllAsync();
            return mascotas;
        }

        public async Task<PagedList<MascotaConDuenoDto>> ListarMascotasConDuenoAsync(MascotaConDuenoQueryFilter filters)
        {
            var mascotas = await ObtenerMascotasConDuenoBaseAsync();

            // Filtros 
            if (!string.IsNullOrWhiteSpace(filters.NombreMascota))
            {
                mascotas = mascotas.Where(m =>
                    m.Nombre.Contains(filters.NombreMascota, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(filters.Especie))
            {
                mascotas = mascotas.Where(m =>
                    m.Especie.Equals(filters.Especie, StringComparison.OrdinalIgnoreCase));
            }

            var resultado = mascotas.Select(m => new MascotaConDuenoDto
            {
                MascotaId = m.Id,
                NombreMascota = m.Nombre,
                Especie = m.Especie,
                Raza = m.Raza,
                Edad = m.Edad,

                NombreDueno = m.Dueno.Nombre,
                TelefonoDueno = m.Dueno.Telefono,
                DireccionDueno = m.Dueno.Direccion
            });

            return PagedList<MascotaConDuenoDto>.Create(
                resultado,
                filters.PageNumber,
                filters.PageSize
            );
        }

        public async Task<IEnumerable<MascotasPorRazaServiciosDto>> ObtenerServiciosPorRazaAsync()
        {
            var mascotas = await _unitOfWork.Mascotas.GetAllAsync();
            var citas = await _unitOfWork.Citas.GetAllAsync();

            var resultado = mascotas
                .Join(
                    citas,
                    m => m.Id,
                    c => c.MascotaId,
                    (m, c) => new { m.Raza }
                )
                .GroupBy(x => x.Raza)
                .Select(g => new MascotasPorRazaServiciosDto
                {
                    Raza = g.Key,
                    CantidadServicios = g.Count()
                });

            return resultado;
        }
    }
}