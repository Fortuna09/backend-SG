namespace backend_SG.DTOs
{
    public class CriarPedidoItemDTO
    {
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
