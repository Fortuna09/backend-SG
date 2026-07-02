namespace backend_SG.Models
{
    public class Produto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CodigoDeBarras { get; set; } = string.Empty;
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public int TipoProdutoId { get; set; }
        public TipoProduto? TipoProduto { get; set; } = new TipoProduto();
    }
}
