using Microsoft.AspNetCore.Mvc;
using Voxel.Application.DTOs;
using Voxel.Application.Services;
using Voxel.Domain.Entities;
using Voxel.Infrastructure.Context;

namespace Voxel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly VoxelContext _context;
        private readonly IPasswordService _passwordService;

        public AuthController(VoxelContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
            {
                return BadRequest(new LoginResponse 
                { 
                    Sucesso = false, 
                    Mensagem = "Email e senha são obrigatórios" 
                });
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == request.Email);
            
            if (usuario == null)
            {
                return Unauthorized(new LoginResponse 
                { 
                    Sucesso = false, 
                    Mensagem = "Usuário ou senha inválidos" 
                });
            }

            // Verificar senha (suporta texto plano e hash BCrypt)
            bool senhaValida = false;
            
            // Tentar verificar como hash BCrypt
            try
            {
                senhaValida = _passwordService.VerifyPassword(request.Senha, usuario.Senha);
            }
            catch
            {
                // Se falhar, tentar como texto plano (para teste)
                senhaValida = (usuario.Senha == request.Senha);
            }
            
            if (!senhaValida)
            {
                return Unauthorized(new LoginResponse 
                { 
                    Sucesso = false, 
                    Mensagem = "Usuário ou senha inválidos" 
                });
            }

            // Gerar token JWT simples (você pode melhorar isso depois)
            var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{usuario.Id}:{usuario.Email}:{DateTime.UtcNow.Ticks}"));

            return Ok(new LoginResponse
            {
                Sucesso = true,
                Mensagem = "Login realizado com sucesso!",
                Token = token,
                Usuario = new UsuarioDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                }
            });
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha) || string.IsNullOrEmpty(request.Nome))
            {
                return BadRequest(new LoginResponse 
                { 
                    Sucesso = false, 
                    Mensagem = "Nome, email e senha são obrigatórios" 
                });
            }

            // Verificar se o email já existe
            var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.Email == request.Email);
            if (usuarioExistente != null)
            {
                return Conflict(new LoginResponse 
                { 
                    Sucesso = false, 
                    Mensagem = "Este email já está registrado" 
                });
            }

            // Criar novo usuário com senha hasheada
            var novoUsuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = _passwordService.HashPassword(request.Senha)
            };

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Login), new LoginResponse
            {
                Sucesso = true,
                Mensagem = "Usuário registrado com sucesso!",
                Usuario = new UsuarioDto
                {
                    Id = novoUsuario.Id,
                    Nome = novoUsuario.Nome,
                    Email = novoUsuario.Email
                }
            });
        }

        [HttpGet("validar-token/{token}")]
        public IActionResult ValidarToken(string token)
        {
            try
            {
                var decodedToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var parts = decodedToken.Split(':');
                
                if (parts.Length < 3)
                    return Unauthorized(new { Sucesso = false, Mensagem = "Token inválido" });

                return Ok(new { Sucesso = true, Mensagem = "Token válido", UsuarioId = parts[0] });
            }
            catch
            {
                return Unauthorized(new { Sucesso = false, Mensagem = "Token inválido" });
            }
        }
    }
}

