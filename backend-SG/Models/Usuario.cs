using backend_SG.Enums;

namespace backend_SG.Models
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string? Cpf { get; set; }
        public PerfilUsuario Perfil { get; set; }
    }
}