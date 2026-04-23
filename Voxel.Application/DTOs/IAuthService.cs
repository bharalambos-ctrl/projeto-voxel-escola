using Voxel.Application.DTOs;
using System.Threading.Tasks;

namespace Voxel.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<bool> RegisterAsync(RegisterRequest request);
    }
}