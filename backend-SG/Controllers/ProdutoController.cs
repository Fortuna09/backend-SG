using backend_SG.Data;
using backend_SG.DTOs;
using backend_SG.Models;
using backend_SG.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_SG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ProdutoService _produtoService;

        public ProdutoController(AppDbContext dbContext, ProdutoService produtoService)
        {
            _dbContext = dbContext;
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var produtos = await _dbContext.Produtos
                .Include(p => p.TipoProduto)
                .ToListAsync();

            var listaDto = new List<ProdutoListaDTO>();

            foreach (var produto in produtos)
            {
                int saldo = await _produtoService.CalculoSaldoAtual(produto.Id);

                var dto = new ProdutoListaDTO
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    CodigoDeBarras = produto.CodigoDeBarras,
                    PrecoCusto = produto.PrecoCusto,
                    PrecoVenda = produto.PrecoVenda,
                    SaldoAtual = saldo,
                    TipoProdutoId = produto.TipoProdutoId,
                    TipoProdutoNome = produto.TipoProduto?.Nome ?? "Não Informado"
                };

                listaDto.Add(dto);
            }

            return Ok(listaDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var produto = await _dbContext.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }

        [HttpPost]
        public async Task<IActionResult> CriarProduto([FromBody] CriarProdutoDTO dto)
        {
            var novoProduto = new Produto
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome,
                CodigoDeBarras = dto.CodigoDeBarras,
                PrecoCusto = dto.PrecoCusto,
                PrecoVenda = dto.PrecoVenda ?? 0,
                TipoProdutoId = dto.TipoProdutoId
            };

            await _dbContext.Produtos.AddAsync(novoProduto);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarPorId), new { id = novoProduto.Id }, novoProduto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] EditarProdutoDTO dto)
        {
            bool atualizou = await _produtoService.AtualizarProduto(id, dto);

            if (!atualizou)
            {
                return NotFound("Produto não encontrado para atualização.");
            }

            return Ok("Produto atualizado com sucesso!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            bool deletou = await _produtoService.DeletarProduto(id);

            if (!deletou)
            {
                var produtoExiste = await _dbContext.Produtos.AnyAsync(p => p.Id == id);
                if (!produtoExiste) return NotFound("Produto não encontrado.");

                return BadRequest("Não é possível deletar este produto porque ele já possui histórico de movimentações no estoque.");
            }

            return Ok("Produto removido com sucesso!");
        }
    }
}