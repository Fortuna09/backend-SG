using backend_SG.Enums;

namespace backend_SG.Models
{
    public class Pedido
    {
        public Guid Id { get; set; }
        public DateTime DataPedido { get; set; }
        public StatusPedido Status { get; set; }
        public List<PedidoItem> Itens { get; set; } = new();
        public decimal ValorTotal => Itens.Sum(item => item.Quantidade * item.PrecoUnitario);
    }
}
