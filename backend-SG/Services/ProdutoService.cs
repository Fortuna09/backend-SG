using backend_SG.Data;
using backend_SG.Enums;
using Microsoft.EntityFrameworkCore;

namespace backend_SG.Services
{
    public class ProdutoService
    {
        private readonly AppDbContext _dbContext;
        private readonly ProdutoService _produtoService;

        public ProdutoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CalculoSaldoAtual(Guid produtoId)
        {
            var movimentacao = await _dbContext.MovimentacaoEstoque
                .Where(m => m.ProdutoId == produtoId)
                .OrderByDescending(m => m.DataMovimentacao)
                .ToListAsync();

            int saldoTotal = 0;

            foreach (var mov in movimentacao)
            {
                if (mov.TipoMovimentacao == TipoMovimentacao.Entrada)
                {
                    saldoTotal += mov.Quantidade;
                }
                else if (mov.TipoMovimentacao == TipoMovimentacao.Saida)
                {
                    saldoTotal -= mov.Quantidade;
                }
            }

            return saldoTotal;
        }
    }
}
