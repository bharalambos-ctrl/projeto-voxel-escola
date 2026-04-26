using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Voxel.Application.DTOs;
using Voxel.Infrastructure.Context;

namespace Voxel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly VoxelContext _context;
        private readonly IConfiguration _config;

        public AuthController(VoxelContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.Email == request.Email);
            if (user == null) return Unauthorized(new { mensagem = "Usuário ou senha inválidos" });

            var valid = BCrypt.Net.BCrypt.Verify(request.Senha, user.Senha);
            if (!valid) return Unauthorized(new { mensagem = "Usuário ou senha inválidos" });

            var jwtKey = _config.GetValue<string>("Jwt:Key") ?? "CHAVE_SECRETA_DE_DESEMPENHO_LOCAL";
            var jwtIssuer = _config.GetValue<string>("Jwt:Issuer") ?? "voxel.local";

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim("nome", user.Nome)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenStr, mensagem = "Login realizado" });
        }
    }
}
