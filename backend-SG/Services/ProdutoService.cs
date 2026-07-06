using backend_SG.Data;
using backend_SG.DTOs;
using backend_SG.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_SG.Services
{
    public class ProdutoService
    {
        private readonly AppDbContext _dbContext;

        public ProdutoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Produto>> ListarTodos()
        {
            return await _dbContext.Produtos
                .Include(p => p.TipoProduto)
                .ToListAsync();
        }

        public async Task<Produto?> BuscarPorId(Guid id)
        {
            return await _dbContext.Produtos
                .Include(p => p.TipoProduto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> AtualizarProduto(Guid id, EditarProdutoDTO dto)
        {
            var produto = await _dbContext.Produtos.FindAsync(id);
            if (produto == null) return false;

            produto.Nome = dto.Nome;
            produto.PrecoCusto = dto.PrecoCusto;
            produto.PrecoVenda = dto.PrecoVenda;
            produto.CodigoDeBarras = dto.CodigoDeBarras;
            produto.TipoProdutoId = dto.TipoProdutoId;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletarProduto(Guid id)
        {
            var produto = await _dbContext.Produtos.FindAsync(id);
            if (produto == null) return false;

            bool temMovimentacao = await _dbContext.MovimentacoesEstoque.AnyAsync(m => m.ProdutoId == id);
            if (temMovimentacao)
            {
                return false;
            }

            _dbContext.Produtos.Remove(produto);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidarSaidaEstoque(Guid produtoId, int quantidadeNecessaria)
        {
            var entradas = await _dbContext.MovimentacoesEstoque
                .Where(m => m.ProdutoId == produtoId && m.TipoMovimentacao == Enums.TipoMovimentacao.Entrada)
                .SumAsync(m => m.Quantidade);

            var saidas = await _dbContext.MovimentacoesEstoque
                .Where(m => m.ProdutoId == produtoId && m.TipoMovimentacao == Enums.TipoMovimentacao.Saida)
                .SumAsync(m => m.Quantidade);

            return (entradas - saidas) >= quantidadeNecessaria;
        }

        public async Task<int> CalculoSaldoAtual(Guid produtoId)
        {
            var entradas = await _dbContext.MovimentacoesEstoque
                .Where(m => m.ProdutoId == produtoId && m.TipoMovimentacao == Enums.TipoMovimentacao.Entrada)
                .SumAsync(m => m.Quantidade);

            var saidas = await _dbContext.MovimentacoesEstoque
                .Where(m => m.ProdutoId == produtoId && m.TipoMovimentacao == Enums.TipoMovimentacao.Saida)
                .SumAsync(m => m.Quantidade);

            return entradas - saidas;
        }
    }
}