using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.DTOs;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Interfaces;

namespace Veterinaria.Core.Services
{
    public class DuenoService : IDuenoService
    {
        private readonly IDuenoRepository _duenoRepo;
        private readonly IMascotaRepository _mascotaRepo;
        private readonly IMapper _mapper;

        public DuenoService(IDuenoRepository duenoRepo, IMascotaRepository mascotaRepo, IMapper mapper)
        {
            _duenoRepo = duenoRepo;
            _mascotaRepo = mascotaRepo;
            _mapper = mapper;
        }

        public async Task<(int duenoId, int mascotaId)> RegistrarDuenoConMascotaAsync(DuenoDto duenoDto, MascotaDto mascotaDto)
        {
            // Validar telefono unico
            var existente = await _duenoRepo.GetByTelefonoAsync(duenoDto.Telefono);

            Dueno dueno;
            if (existente is null)
            {
                dueno = _mapper.Map<Dueno>(duenoDto);
                await _duenoRepo.AddAsync(dueno);
                //  Guardar dueno
                await _duenoRepo.SaveChangesAsync(); 
            }
            else
            {
                dueno = existente;
            }

            // Crear mascota asociada
            var mascota = _mapper.Map<Mascota>(mascotaDto);
            mascota.DuenoId = dueno.Id;

            await _mascotaRepo.AddAsync(mascota);
            //  Guardar mascota
            await _mascotaRepo.SaveChangesAsync(); 

            return (dueno.Id, mascota.Id);
        }
    }
}
