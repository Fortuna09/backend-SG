using backend_SG.DTOs;
using backend_SG.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_SG.Data;

namespace backend_SG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FornecedorController : ControllerBase
    {
        private readonly FornecedorService _fornecedorService;
        private readonly AppDbContext _dbContext;

        public FornecedorController(FornecedorService fornecedorService, AppDbContext dbContext)
        {
            _fornecedorService = fornecedorService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var fornecedores = await _fornecedorService.ListarTodos();
            return Ok(fornecedores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var fornecedor = await _fornecedorService.BuscarPorId(id);
            if (fornecedor == null)
            {
                return NotFound("Fornecedor não encontrado.");
            }
            return Ok(fornecedor);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] CriarFornecedorDTO dto)
        {
            var novoFornecedor = await _fornecedorService.Cadastrar(dto);

            if (novoFornecedor == null)
            {
                return BadRequest("Este CNPJ já está cadastrado no sistema.");
            }

            return CreatedAtAction(nameof(BuscarPorId), new { id = novoFornecedor.Id }, novoFornecedor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] EditarFornecedorDTO dto)
        {
            bool atualizou = await _fornecedorService.Atualizar(id, dto);
            if (!atualizou)
            {
                return NotFound("Fornecedor não encontrado para atualização.");
            }

            return Ok("Dados do fornecedor atualizados com sucesso!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            bool deletou = await _fornecedorService.Deletar(id);
            if (!deletou)
            {
                var fornecedorExiste = await _dbContext.Fornecedores.AnyAsync(f => f.Id == id);
                if (!fornecedorExiste)
                {
                    return NotFound("Fornecedor não encontrado.");
                }

                return BadRequest("Não é possível remover este fornecedor porque ele está vinculado a movimentações de entrada no estoque.");
            }

            return Ok("Fornecedor removido com sucesso!");
        }
    }
}