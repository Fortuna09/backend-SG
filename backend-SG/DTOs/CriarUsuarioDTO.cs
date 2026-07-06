using backend_SG.Enums;

namespace backend_SG.DTOs
{
    public class CriarUsuarioDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public PerfilUsuario Perfil { get; set; }
    }
}
