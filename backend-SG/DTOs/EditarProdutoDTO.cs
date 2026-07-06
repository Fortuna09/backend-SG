using System.Reflection.Metadata;

namespace backend_SG.DTOs
{
    public class EditarProdutoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string CodigoDeBarras { get; set; } = string.Empty;
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public int TipoProdutoId { get; set; }
    }
}
