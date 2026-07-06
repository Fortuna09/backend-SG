using backend_SG.Data;
using backend_SG.DTOs;
using backend_SG.Enums;
using backend_SG.Models;
using backend_SG.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_SG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MovimentacaoEstoqueController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ProdutoService _produtoService;

        public MovimentacaoEstoqueController(AppDbContext dbContext, ProdutoService produtoService)
        {
            _dbContext = dbContext;
            _produtoService = produtoService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarMovimentacao([FromBody] CriarMovimentacaoDTO dto)
        {
            if(dto.TipoMovimentacao == TipoMovimentacao.Saida)
            {
                bool podeSair = await _produtoService.ValidarSaidaEstoque(dto.ProdutoId, dto.Quantidade);

                if (!podeSair)
                {
                    return BadRequest("Quantidade insuficiente em estoque para a saída.");
                }
            }

            var novaMovimentacao = new MovimentacaoEstoque
            {
                Id = Guid.NewGuid(),
                ProdutoId = dto.ProdutoId,
                Quantidade = dto.Quantidade,
                TipoMovimentacao = dto.TipoMovimentacao,
                DataMovimentacao = DateTime.UtcNow
            };

            await _dbContext.MovimentacoesEstoque.AddAsync(novaMovimentacao);
            await _dbContext.SaveChangesAsync();

            return Ok(novaMovimentacao);
        }
    }

}
