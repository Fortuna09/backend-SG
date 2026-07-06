using backend_SG.Data;
using backend_SG.DTOs;
using backend_SG.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend_SG.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _dbContext;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        public async Task<List<Usuario>> ListarTodos()
        {
            return await _dbContext.Usuarios.ToListAsync();
        }

        public async Task<Usuario> BuscarPorId(Guid id)
        {
            return await _dbContext.Usuarios.FindAsync(id);
        }

        public async Task<Usuario?> Cadastrar(CriarUsuarioDTO dto)
        {
            var usuarioExiste = await _dbContext.Usuarios
                .AnyAsync(u => u.Email == dto.Email || u.Cpf == dto.Cpf);

            if (usuarioExiste) return null;

            var novoUsuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome,
                Email = dto.Email,
                Cpf = dto.Cpf,
                Perfil = dto.Perfil
            };

            novoUsuario.SenhaHash = _passwordHasher.HashPassword(novoUsuario, dto.Senha);

            await _dbContext.Usuarios.AddAsync(novoUsuario);
            await _dbContext.SaveChangesAsync();
            return novoUsuario;
        }

        public async Task<Usuario?> Autenticar(string email, string senhaLimpa)
        {
            var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null) return null;

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, senhaLimpa);

            if (resultado == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return usuario;
        }


    }
}
