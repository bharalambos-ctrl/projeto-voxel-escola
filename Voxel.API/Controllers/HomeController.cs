using Microsoft.AspNetCore.Mvc;
using Voxel.Application.DTOs;
using Voxel.Infrastructure.Context;

namespace Voxel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly VoxelContext _context;

        public HomeController(VoxelContext context)
        {
            _context = context;
        }

        [HttpGet("trilhas")]
        public IActionResult GetTrilhas()
        {
            // Retornar trilhas disponíveis
            var trilhas = new List<string>
            {
                "Desenvolvimento Web",
                "Inteligência Artificial",
                "Segurança da Informação",
                "Desenvolvimento Mobile",
                "Cloud Computing",
                "Big Data"
            };

            return Ok(new { Sucesso = true, Trilhas = trilhas });
        }

        [HttpGet("usuario/{id}")]
        public IActionResult GetUsuario(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);

            if (usuario == null)
                return NotFound(new { Sucesso = false, Mensagem = "Usuário não encontrado" });

            return Ok(new
            {
                Sucesso = true,
                Usuario = new UsuarioDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                }
            });
        }

        [HttpGet("teste")]
        public IActionResult Teste()
        {
            var totalUsuarios = _context.Usuarios.Count();
            return Ok(new { Sucesso = true, Mensagem = "Backend funcionando!", TotalUsuarios = totalUsuarios });
        }
    }
}
