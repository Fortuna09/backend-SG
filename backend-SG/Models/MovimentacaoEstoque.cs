using backend_SG.Enums;

namespace backend_SG.Models
{
    public class MovimentacaoEstoque
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Produto? Produto { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }
        public Guid FornecedorId { get; set; }
        public Fornecedor? Fornecedor { get; set; }
    }
}
