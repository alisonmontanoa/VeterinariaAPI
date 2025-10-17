using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.DTOs;
using Veterinaria.Core.Entities;

namespace Veterinaria.Infrastructure.Mappings
{
    public class VeterinariaProfile : Profile
    {
        public VeterinariaProfile()
        {
            CreateMap<Dueno, DuenoDto>().ReverseMap();
            CreateMap<Mascota, MascotaDto>().ReverseMap();
            CreateMap<Cita, CitaDto>().ReverseMap();
        }
    }
}
