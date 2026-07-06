using backend_SG.Data;
using backend_SG.Enums;
using backend_SG.Models;
using backend_SG.Services;
using Microsoft.EntityFrameworkCore;

namespace backend_SG.Tests
{
    public class ProdutoServiceTests
    {
        private AppDbContext CriarDbContextNaMemoria()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CalculoSaldoAtual_DeveRetornarOSaldoCorreto_BaseadoNasMovimentacoes()
        {
            var dbContext = CriarDbContextNaMemoria();
            var produtoId = Guid.NewGuid();

            dbContext.MovimentacoesEstoque.AddRange(
                new MovimentacaoEstoque { Id = Guid.NewGuid(), ProdutoId = produtoId, Quantidade = 50, TipoMovimentacao = TipoMovimentacao.Entrada, DataMovimentacao = DateTime.UtcNow },
                new MovimentacaoEstoque { Id = Guid.NewGuid(), ProdutoId = produtoId, Quantidade = 10, TipoMovimentacao = TipoMovimentacao.Saida, DataMovimentacao = DateTime.UtcNow },
                new MovimentacaoEstoque { Id = Guid.NewGuid(), ProdutoId = produtoId, Quantidade = 5, TipoMovimentacao = TipoMovimentacao.Saida, DataMovimentacao = DateTime.UtcNow }
            );
            await dbContext.SaveChangesAsync();

            var service = new ProdutoService(dbContext);

            int saldoResultado = await service.CalculoSaldoAtual(produtoId);

            Assert.Equal(35, saldoResultado);
        }

        [Fact]
        public async Task DeletarProduto_DeveRetornarFalso_QuandoProdutoPossuiMovimentacoes()
        {
            var dbContext = CriarDbContextNaMemoria();
            var produtoId = Guid.NewGuid();

            dbContext.Produtos.Add(new Produto { Id = produtoId, Nome = "Produto Teste" });
            dbContext.MovimentacoesEstoque.Add(new MovimentacaoEstoque { Id = Guid.NewGuid(), ProdutoId = produtoId, Quantidade = 10, TipoMovimentacao = TipoMovimentacao.Entrada });
            await dbContext.SaveChangesAsync();

            var service = new ProdutoService(dbContext);

            bool resultadoExclusao = await service.DeletarProduto(produtoId);

            Assert.False(resultadoExclusao);
        }
    }
}