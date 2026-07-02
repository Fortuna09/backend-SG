using backend_SG.Enums;

namespace backend_SG.DTOs
{
    public class CriarMovimentacaoDTO
    {
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }
    }
}
