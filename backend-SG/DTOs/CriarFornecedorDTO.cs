namespace backend_SG.DTOs
{
    public class CriarFornecedorDTO
    {
        public string RazaoSocial { get; set; } = string.Empty;
        public string NomeFantasia { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string? InscricaoEstadual { get; set; }
        public string? EmailCorporativo { get; set; }
        public string? Telefone { get; set; }
    }
}
