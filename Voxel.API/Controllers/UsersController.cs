using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Voxel.Infrastructure.Context;
using Voxel.Domain.Entities;
using Voxel.Application.DTOs;

namespace Voxel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly VoxelContext _context;

        public UsersController(VoxelContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            // Verifica se já existe um usuário com o mesmo email
            var existing = _context.Usuarios.FirstOrDefault(u => u.Email == request.Email);
            if (existing != null)
            {
                return Conflict(new { mensagem = "Email já cadastrado" });
            }

            // Hash da senha usando BCrypt
            var hashed = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            var user = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = hashed
            };

            _context.Usuarios.Add(user);
            _context.SaveChanges();

            // Não retornar a senha no payload
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, new { user.Id, user.Nome, user.Email });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Usuarios.Find(id);
            if (user == null) return NotFound();
            return Ok(new { user.Id, user.Nome, user.Email });
        }
    }
}
