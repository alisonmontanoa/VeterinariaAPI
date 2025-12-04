using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Api.Responses;
using Veterinaria.Core.DTOs;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Enums;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.Services;

namespace Veterinaria.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public SecurityController(ISecurityService securityService, IMapper mapper, IPasswordService passwordService)
        {
            _securityService = securityService;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        //[Authorize(Roles = nameof(RoleType.Administrador))]
        [HttpPost]
        public async Task<IActionResult> Post(SecurityDto securityDto)
        {
            var security = _mapper.Map<Security>(securityDto);

            // Generar HASH antes de guardar
            security.Password = _passwordService.Hash(security.Password);

            await _securityService.RegisterUser(security);

            securityDto = _mapper.Map<SecurityDto>(security);
            var response = new ApiResponse<SecurityDto>(securityDto);

            return Ok(response);
        }
    }
}