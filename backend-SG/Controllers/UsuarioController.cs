using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend_SG.DTOs;
using backend_SG.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace backend_SG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly IConfiguration _configuration;

        public UsuarioController(UsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _configuration = configuration;
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

            // === 🚀 GERAÇÃO DO TOKEN JWT ===
            var tokenHandler = new JwtSecurityTokenHandler();

            // Lendo a chave secreta que criamos no appsettings.json
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var key = Encoding.ASCII.GetBytes(secretKey!);

            // Montando os dados que vão trafegar dentro do Token (Claims)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Role, usuario.Perfil.ToString()) // Guarda se é Administrador ou Cliente
                }),
                Expires = DateTime.UtcNow.AddHours(3), // O token expira sozinho em 3 horas
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature // Criptografia SHA256
                )
            };

            // Cria o token de fato
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Retorna o token para o Angular salvar na memória local
            return Ok(new
            {
                mensagem = "Login efetuado com sucesso!",
                token = tokenString,
                usuario = new
                {
                    id = usuario.Id,
                    nome = usuario.Nome,
                    perfil = usuario.Perfil.ToString()
                }
            });
        }
    }
}