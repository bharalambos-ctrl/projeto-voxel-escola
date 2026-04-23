using Voxel.Application.DTOs;
using Voxel.Application.Interfaces;
using Voxel.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Voxel.Domain.Entities;
using System.Threading.Tasks;

namespace Voxel.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly VoxelContext _context;

        public AuthService(VoxelContext context)
        {
            _context = context;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            // Simulação de verificação de hash (em prod use BCrypt)
            if (usuario == null || usuario.SenhaHash != request.Senha) 
                return null;

            return new LoginResponse
            {
                Token = "JWT-GENERATED-TOKEN", // Aqui integraria com um TokenService
                Mensagem = $"Bem-vindo, {usuario.Nome}!"
            };
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            if (!request.AceitouTermos) return false;

            var existe = await _context.Usuarios.AnyAsync(u => u.Email == request.Email);
            if (existe) return false;

            var novoUsuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = request.Senha, // Ideal: aplicar Hash aqui
                AceitouTermos = request.AceitouTermos,
                DataConsentimento = DateTime.UtcNow,
                DataCadastro = DateTime.UtcNow
            };

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}