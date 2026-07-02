namespace backend_SG.DTOs
{
    public class ProdutoListaDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CodigoDeBarras { get; set; } = string.Empty;
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public int SaldoAtual { get; set; }
        public int TipoProdutoId { get; set; }
        public string TipoProdutoNome { get; set; } = string.Empty;
    }
}
