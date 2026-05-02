namespace Voxel.Application.DTOs
{
    public class LoginResponse
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public string? Token { get; set; }
        public UsuarioDto? Usuario { get; set; }
    }

    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class RegistroRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    public class HomeResponse
    {
        public string Mensagem { get; set; } = string.Empty;
        public UsuarioDto? Usuario { get; set; }
        public List<string> Trilhas { get; set; } = new();
    }
}
