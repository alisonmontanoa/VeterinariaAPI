using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Api.Responses;
using Veterinaria.Core.DTOs;
using Veterinaria.Core.Interfaces;

namespace Veterinaria.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DuenoController : ControllerBase
    {
        private readonly IDuenoService _service;

        public DuenoController(IDuenoService service)
        {
            _service = service;
        }

        public class RegistrarDuenoMascotaRequest
        {
            public DuenoDto Dueno { get; set; } = null!;
            public MascotaDto Mascota { get; set; } = null!;
        }

        #region REGISTRAR DUENO
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarDuenoMascotaRequest request)
        {
            try
            {
                var (duenoId, mascotaId) = await _service.RegistrarDuenoConMascotaAsync(request.Dueno, request.Mascota);

                var payload = new { DuenoId = duenoId, MascotaId = mascotaId, Mensaje = "Registro exitoso" };
                return StatusCode(201, new ApiResponse<object>(payload)); // 201 Created
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
        #endregion
    }
}
