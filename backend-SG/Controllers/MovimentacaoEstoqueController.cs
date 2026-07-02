using backend_SG.Data;
using backend_SG.DTOs;
using backend_SG.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_SG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacaoEstoqueController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public MovimentacaoEstoqueController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CriarMovimentacao([FromBody] CriarMovimentacaoDTO dto)
        {
            // 1. Mapeia o DTO para a Entidade do Banco
            var novaMovimentacao = new MovimentacaoEstoque
            {
                Id = Guid.NewGuid(),
                ProdutoId = dto.ProdutoId,
                Quantidade = dto.Quantidade,
                TipoMovimentacao = dto.TipoMovimentacao,
                DataMovimentacao = DateTime.UtcNow // Carimbo de data seguro feito no servidor
            };

            // 2. Adiciona e salva no banco de dados
            await _dbContext.MovimentacaoEstoque.AddAsync(novaMovimentacao);
            await _dbContext.SaveChangesAsync();

            return Ok(novaMovimentacao);
        }
    }

}
