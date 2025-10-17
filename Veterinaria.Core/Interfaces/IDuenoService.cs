using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.DTOs;

namespace Veterinaria.Core.Interfaces
{
    public interface IDuenoService
    {
        Task<(int duenoId, int mascotaId)> RegistrarDuenoConMascotaAsync(DuenoDto duenoDto, MascotaDto mascotaDto);
    }
}
