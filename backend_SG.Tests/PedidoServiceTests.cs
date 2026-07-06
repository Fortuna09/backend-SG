using backend_SG.Data;
using backend_SG.Enums;
using backend_SG.Models;
using backend_SG.Services;
using Microsoft.EntityFrameworkCore;

namespace backend_SG.Tests
{
    public class PedidoServiceTests
    {
        private AppDbContext CriarDbContextNaMemoria()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task ConfirmarPedido_DeveAlterarStatusEGerarSaidas_QuandoHouverEstoqueSuficiente()
        {
            var dbContext = CriarDbContextNaMemoria();
            var produtoService = new ProdutoService(dbContext);
            var pedidoService = new PedidoService(dbContext, produtoService);

            var produtoId = Guid.NewGuid();

            dbContext.MovimentacoesEstoque.Add(new MovimentacaoEstoque
            {
                Id = Guid.NewGuid(),
                ProdutoId = produtoId,
                Quantidade = 10,
                TipoMovimentacao = TipoMovimentacao.Entrada,
                DataMovimentacao = DateTime.UtcNow
            });

            var pedidoId = Guid.NewGuid();
            var pedido = new Pedido
            {
                Id = pedidoId,
                Status = StatusPedido.Pendente,
                DataPedido = DateTime.UtcNow,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem { Id = Guid.NewGuid(), PedidoId = pedidoId, ProdutoId = produtoId, Quantidade = 4, PrecoUnitario = 100 }
                }
            };

            dbContext.Pedidos.Add(pedido);
            await dbContext.SaveChangesAsync();

            bool resultado = await pedidoService.ConfirmarPedido(pedidoId);

            Assert.True(resultado);

            var pedidoDoBanco = await dbContext.Pedidos.Include(p => p.Itens).FirstOrDefaultAsync(p => p.Id == pedidoId);
            Assert.Equal(StatusPedido.Confirmado, pedidoDoBanco!.Status);

            var temSaidaRegistrada = await dbContext.MovimentacoesEstoque.AnyAsync(m =>
                m.ProdutoId == produtoId &&
                m.Quantidade == 4 &&
                m.TipoMovimentacao == TipoMovimentacao.Saida
            );
            Assert.True(temSaidaRegistrada);

            int saldoFinal = await produtoService.CalculoSaldoAtual(produtoId);
            Assert.Equal(6, saldoFinal);
        }

        [Fact]
        public async Task ConfirmarPedido_DeveRetornarFalso_QuandoEstoqueForInsuficiente()
        {
            var dbContext = CriarDbContextNaMemoria();
            var produtoService = new ProdutoService(dbContext);
            var pedidoService = new PedidoService(dbContext, produtoService);

            var produtoId = Guid.NewGuid();

            dbContext.MovimentacoesEstoque.Add(new MovimentacaoEstoque
            {
                Id = Guid.NewGuid(),
                ProdutoId = produtoId,
                Quantidade = 2,
                TipoMovimentacao = TipoMovimentacao.Entrada,
                DataMovimentacao = DateTime.UtcNow
            });

            var pedidoId = Guid.NewGuid();
            var pedido = new Pedido
            {
                Id = pedidoId,
                Status = StatusPedido.Pendente,
                DataPedido = DateTime.UtcNow,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem { Id = Guid.NewGuid(), PedidoId = pedidoId, ProdutoId = produtoId, Quantidade = 5, PrecoUnitario = 100 }
                }
            };

            dbContext.Pedidos.Add(pedido);
            await dbContext.SaveChangesAsync();

            bool resultado = await pedidoService.ConfirmarPedido(pedidoId);

            Assert.False(resultado);

            var pedidoDoBanco = await dbContext.Pedidos.FindAsync(pedidoId);
            Assert.NotEqual(StatusPedido.Confirmado, pedidoDoBanco!.Status);

            bool gerouSaidaIndevida = await dbContext.MovimentacoesEstoque.AnyAsync(m => m.TipoMovimentacao == TipoMovimentacao.Saida);
            Assert.False(gerouSaidaIndevida);
        }
    }
}