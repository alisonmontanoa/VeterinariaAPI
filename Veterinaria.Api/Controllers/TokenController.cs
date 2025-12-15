using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Enums;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.Services;

namespace Veterinaria.Api.Controllers
{
    /// <summary>
    /// Controlador responsable de la autenticacion y generacion de tokens JWT.
    /// </summary>
    /// <remarks>
    /// Este controlador permite a los usuarios autenticarse y obtener un token JWT
    /// que sera utilizado para acceder a los endpoints protegidos de la API.
    /// 
    /// El token incluye informacion del usuario y su rol.
    /// </remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;

        public TokenController(
            IConfiguration configuration,
            ISecurityService securityService)
        {
            _configuration = configuration;
            _securityService = securityService;
        }

        /// <summary>
        /// Autentica un usuario y genera un token JWT.
        /// </summary>
        /// <remarks>
        /// Este endpoint valida las credenciales del usuario y, si son correctas,
        /// devuelve un token JWT valido por un tiempo determinado.
        /// 
        /// El token incluye:
        /// - Login
        /// - Nombre
        /// - Rol del usuario
        /// 
        /// El token debe enviarse en el header:
        /// Authorization: Bearer {token}
        /// </remarks>
        /// <param name="userLogin">Credenciales del usuario.</param>
        /// <returns>Token JWT generado.</returns>
        /// <response code="200">Autenticacion exitosa.</response>
        /// <response code="404">Credenciales incorrectas.</response>
        /// <response code="500">Error interno del servidor.</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [SwaggerOperation(
            Summary = "Login de usuario",
            Description = "Autentica un usuario y devuelve un token JWT."
        )]
        public async Task<IActionResult> Authentication(UserLogin userLogin)
        {
            var user = await _securityService.GetLoginByCredentials(userLogin);

            if (user == null)
                return NotFound("Credenciales incorrectas");

            var token = GenerateToken(user);
            return Ok(new { token });
        }

        /// <summary>
        /// Genera el token JWT a partir de los datos del usuario.
        /// </summary>
        private string GenerateToken(Security security)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"])
            );

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
            );

            var header = new JwtHeader(credentials);

            var claims = new[]
            {
            new Claim("Login", security.Login),
            new Claim("Name", security.Name),
            new Claim(ClaimTypes.Role, security.Role.ToString())
        };

            var payload = new JwtPayload(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(15)
            );

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}