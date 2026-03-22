using Microsoft.AspNetCore.Mvc;
using Voxel.Application.DTOs; // Você precisará criar o DTO abaixo

namespace Voxel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Simulação de lógica por enquanto
            if (request.Email == "admin@voxel.com" && request.Senha == "123456")
            {
                return Ok(new { token = "JWT-TOKEN-GERADO-AQUI", mensagem = "Bem-vindo à Voxel!" });
            }

            return Unauthorized(new { mensagem = "Usuário ou senha inválidos" });
        }
    }
}
