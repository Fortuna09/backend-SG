namespace backend_SG.DTOs
{
    public class CriarProdutoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string CodigoDeBarras { get; set; } = string.Empty;
        public decimal PrecoCusto { get; set; }
        public decimal? PrecoVenda { get; set; }
        public int TipoProdutoId { get; set; }
    }
}
