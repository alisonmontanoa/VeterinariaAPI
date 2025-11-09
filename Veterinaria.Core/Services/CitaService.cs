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
    public class CitaService : ICitaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly string[] ForbiddenWords =
        {
            "odio", "violencia", "groseria", "discriminacion", "pornografia"
        };

        public CitaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Metodo para detectar lenguaje inapropiado
        private bool ContainsForbiddenContent(string text)
        {
            return !string.IsNullOrWhiteSpace(text) &&
                   ForbiddenWords.Any(w => text.Contains(w, StringComparison.OrdinalIgnoreCase));
        }

        // Crear nueva cita
        public async Task<int> CrearCitaAsync(CitaDto citaDto)
        {
            // Regla 1. Mascota existente
            var mascota = await _unitOfWork.Mascotas.GetByIdAsync(citaDto.MascotaId); 
            if (mascota == null)
                throw new BusinessException("La mascota no existe.", 404);

            // Regla 2. Veterinario existente
            var veterinario = await _unitOfWork.Veterinarios.GetByIdAsync(citaDto.VeterinarioId);
            if (veterinario == null)
                throw new BusinessException("Debe asignar un veterinario valido.", 404);

            // Regla 3. Servicio valido
            var servicio = await _unitOfWork.Servicios.GetByIdAsync(citaDto.ServicioId);
            if (servicio == null)
                throw new BusinessException("Debe seleccionar un servicio valido.", 404);

            // Regla 4. Validar fecha
            if (citaDto.Fecha < DateTime.Now)
                throw new BusinessException("La fecha de la cita debe ser posterior a la actual.", 400);

            // Regla 5. Validar lenguaje en motivo
            if (ContainsForbiddenContent(citaDto.Motivo))
                throw new BusinessException("El motivo de la cita contiene lenguaje inapropiado.", 400);

            // Regla 6. Mascota: maximo 2 citas pendientes
            var citasMascota = (await _unitOfWork.Citas.GetAllAsync())
                .Where(c => c.MascotaId == citaDto.MascotaId && c.Estado == "Pendiente");

            if (citasMascota.Count() >= 2)
                throw new BusinessException("La mascota ya tiene 2 citas pendientes activas.", 400);

            // Regla 7. Veterinario: maximo 8 citas diarias
            var citasVeterinario = (await _unitOfWork.Citas.GetAllAsync())
                .Where(c => c.VeterinarioId == citaDto.VeterinarioId &&
                            c.Fecha.Date == citaDto.Fecha.Date &&
                            c.Estado == "Pendiente");

            if (citasVeterinario.Count() >= 8)
                throw new BusinessException("El veterinario ya alcanzo el límite de 8 citas para ese dia.", 400);

            // Regla 9. Veterinario con menos de 10 citas totales solo puede tener 1 cita por dia
            var totalCitasVeterinario = (await _unitOfWork.Citas.GetAllAsync())
                .Count(c => c.VeterinarioId == citaDto.VeterinarioId);

            if (totalCitasVeterinario < 10)
            {
                var citasHoy = (await _unitOfWork.Citas.GetAllAsync())
                    .Count(c => c.VeterinarioId == citaDto.VeterinarioId &&
                                c.Fecha.Date == citaDto.Fecha.Date);

                if (citasHoy >= 1)
                    throw new BusinessException("El veterinario con menos de 10 citas solo puede tener una cita por dia.", 400);
            }

            // Regla 8. Crear cita
            var cita = _mapper.Map<Cita>(citaDto);
            cita.Estado = "Pendiente";

            await _unitOfWork.Citas.AddAsync(cita);
            await _unitOfWork.SaveChangesAsync();

            return cita.Id;

        }

        // Cancelar Cita
        public async Task CancelarCitaAsync(int citaId)
        {
            var cita = await _unitOfWork.Citas.GetByIdAsync(citaId);
            if (cita == null)
                throw new BusinessException("La cita no existe.", 404);

            if (cita.Estado != "Pendiente")
                throw new BusinessException("Solo se pueden cancelar citas pendientes.", 400);

            cita.Estado = "Cancelada";
            _unitOfWork.Citas.Update(cita);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cita>> ObtenerCitasAsync(CitaQueryFilter filters)
        {
            var citas = await _unitOfWork.Citas.GetAllAsync();

            if (filters.VeterinarioId.HasValue)
                citas = citas.Where(c => c.VeterinarioId == filters.VeterinarioId.Value);

            if (filters.MascotaId.HasValue)
                citas = citas.Where(c => c.MascotaId == filters.MascotaId.Value);

            if (filters.Fecha.HasValue)
                citas = citas.Where(c => c.Fecha.Date == filters.Fecha.Value.Date);

            if (!string.IsNullOrWhiteSpace(filters.Estado))
                citas = citas.Where(c => c.Estado.Equals(filters.Estado, StringComparison.OrdinalIgnoreCase));

            return citas;
        }
    }
}
