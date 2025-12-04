using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.Services;

namespace Veterinaria.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService;

        public TokenController(IConfiguration configuration,
                       ISecurityService securityService,
                       IPasswordService passwordService)
        {
            _configuration = configuration;
            _securityService = securityService;
            _passwordService = passwordService;
        }

        [HttpPost]
        public async Task<IActionResult> Authentication(UserLogin userLogin)
        {
            // Validar usuario en BD
            var validation = await IsValidUser(userLogin);
            if (validation.Item1)
            {
                var token = GenerateToken(validation.Item2);
                return Ok(new { token });
            }

            return NotFound("Credenciales incorrectas");
        }

        private async Task<(bool, Security)> IsValidUser(UserLogin login)
        {
            var user = await _securityService.GetLoginByCredentials(login);
            if (user == null) return (false, null);

            var isValid = _passwordService.Check(user.Password, login.Password);
            return (isValid, user);
        }

        private string GenerateToken(Security security)
        {
            var symmetricSecurityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(signingCredentials);

            // CLAIMS
            var claims = new[]
            {
                new Claim("Login", security.Login),
                new Claim("Name", security.Name),
                new Claim(ClaimTypes.Role, security.Role.ToString()),  // importante
            };

            // PAYLOAD
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

        [HttpGet("TestConeccion")]
        public IActionResult TestConeccion()
        {
            try
            {
                var result = new
                {
                    ConnectionMySql = _configuration["ConnectionStrings:ConnectionMySql"],
                    ConnectionSqlServer = _configuration["ConnectionStrings:ConnectionSqlServer"]
                };

                return Ok(result);
            }
            catch (Exception err)
            {
                return StatusCode(500, err.Message);
            }
        }

        [HttpGet("Config")]
        public IActionResult GetConfig()
        {
            try
            {
                var connectionStringMySql = _configuration["ConnectionStrings:ConnectionMySql"];
                var connectionStringSqlServer = _configuration["ConnectionStrings:ConnectionSqlServer"];

                var result = new
                {
                    connectionStringMySql = connectionStringMySql ?? "MySQL NO CONFIGURADO",
                    connectionStringSqlServer = connectionStringSqlServer ?? "SQL SERVER NO CONFIGURADO",
                    AllConnectionStrings = _configuration.GetSection("ConnectionStrings")
                        .GetChildren()
                        .Select(x => new { x.Key, x.Value }),
                    Environment = _configuration["ASPNETCORE_ENVIRONMENT"] ?? "ASPNETCORE_ENVIRONMENT NO CONFIGURADO",
                    Authentication = _configuration.GetSection("Authentication")
                        .GetChildren()
                        .Select(x => new { x.Key, x.Value })
                };

                return Ok(result);
            }
            catch (Exception err)
            {
                return StatusCode(500, err.Message);
            }
        }
    }
}