using backend_SG.Data;
using backend_SG.Enums;
using backend_SG.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_SG.Services
{
    public class PedidoService
    {
        private readonly AppDbContext _dbContext;
        private readonly ProdutoService _produtoService;

        public PedidoService(AppDbContext dbContext, ProdutoService produtoService)
        {
            _dbContext = dbContext;
            _produtoService = produtoService;
        }

        public async Task<bool> ConfirmarPedido(Guid pedidoId)
        {
            var pedido = await _dbContext.Pedidos
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null || pedido.Status == StatusPedido.Confirmado)
                return false; 

            foreach (var item in pedido.Itens)
            {
                bool temEstoque = await _produtoService.ValidarSaidaEstoque(item.ProdutoId, item.Quantidade);
                if (!temEstoque)
                {
                    return false;
                }
            }

            pedido.Status = StatusPedido.Confirmado;

            foreach (var item in pedido.Itens)
            {
                var movimentacao = new MovimentacaoEstoque
                {
                    Id = Guid.NewGuid(),
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    TipoMovimentacao = TipoMovimentacao.Saida,
                    DataMovimentacao = DateTime.UtcNow
                };

                await _dbContext.MovimentacoesEstoque.AddAsync(movimentacao);
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}