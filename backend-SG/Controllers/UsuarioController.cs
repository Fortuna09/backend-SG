using backend_SG.DTOs;
using backend_SG.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend_SG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodos()
        {
            var usuarios = await _usuarioService.ListarTodos();
            var listaExibicao = usuarios.Select(u => new {
                u.Id,
                u.Nome,
                u.Email,
                u.Cpf,
                u.Perfil
            });
            return Ok(listaExibicao);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var usuario = await _usuarioService.BuscarPorId(id);
            if (usuario == null)
            {
                return NotFound();
            }
            var usuarioExibicao = new
            {
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                usuario.Cpf,
                usuario.Perfil
            };
            return Ok(usuarioExibicao);
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CriarUsuarioDTO dto)
        {
            var usuario = await _usuarioService.Cadastrar(dto);
            if (usuario == null)
            {
                return BadRequest("E-mail ou CPF já cadastrados no sistema.");
            }

            return Ok(new { mensagem = "Usuário cadastrado com sucesso!", nome = usuario.Nome });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var usuario = await _usuarioService.Autenticar(dto.Email, dto.Senha);

            if (usuario == null)
            {
                return Unauthorized("E-mail ou senha inválidos.");
            }

            return Ok(new
            {
                mensagem = "Login efetuado com sucesso!",
                usuarioId = usuario.Id,
                nome = usuario.Nome,
                perfil = usuario.Perfil.ToString()
            });
        }
    }
}