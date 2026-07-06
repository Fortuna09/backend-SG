using System.Collections;
using backend_SG.Data;
using backend_SG.DTOs;
using backend_SG.Enums;
using backend_SG.Models;
using backend_SG.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_SG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly PedidoService _pedidoService;

        public PedidoController(AppDbContext dbContext, PedidoService pedidoService)
        {
            _dbContext = dbContext;
            _pedidoService = pedidoService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPedidoDTO dto)
        {
            var novoPedido = new Pedido
            {
                Id = Guid.NewGuid(),
                DataPedido = DateTime.UtcNow,
                Status = StatusPedido.Pendente
            };

            // Mapeia os itens do DTO para a entidade do banco
            foreach (var itemDto in dto.Itens)
            {
                novoPedido.Itens.Add(new PedidoItem
                {
                    Id = Guid.NewGuid(),
                    PedidoId = novoPedido.Id,
                    ProdutoId = itemDto.ProdutoId,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = itemDto.PrecoUnitario
                });
            }

            await _dbContext.Pedidos.AddAsync(novoPedido);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Criar), new { id = novoPedido.Id }, novoPedido);
        }

        [HttpPost("{id}/confirmar")]
        public async Task<IActionResult> Confirmar(Guid id)
        {
            bool sucesso = await _pedidoService.ConfirmarPedido(id);

            if (!sucesso)
            {
                return BadRequest("Não foi possível confirmar o pedido. Verifique se o ID está correto ou se há saldo suficiente no estoque para todos os itens.");
            }

            return Ok("Pedido confirmado e estoque atualizado com sucesso!");
        }
    }
}
