using backend_SG.Data;
using backend_SG.DTOs;
using backend_SG.Extensions;
using backend_SG.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_SG.Services
{
    public class FornecedorService
    {
        private readonly AppDbContext _dbContext;

        public FornecedorService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Fornecedor>> ListarTodos()
        {
            return await _dbContext.Fornecedores.ToListAsync();
        }

        public async Task<Fornecedor> BuscarPorId(Guid id)
        {
            return await _dbContext.Fornecedores.FindAsync(id);
        }

        public async Task<Fornecedor?> Cadastrar(CriarFornecedorDTO dto)
        {
            var cnpjExiste = await _dbContext.Fornecedores.AnyAsync(f => f.Cnpj == dto.Cnpj);
            if (cnpjExiste) return null;

            if(!dto.Cnpj.IsCnpjValido() ||
               !dto.EmailCorporativo.IsEmailValido() ||
               !dto.Telefone.IsTelefoneValido())
            {
                return null;
            }

                var novoFornecedor = new Fornecedor
            {
                Id = Guid.NewGuid(),
                RazaoSocial = dto.RazaoSocial,
                NomeFantasia = dto.NomeFantasia,
                Cnpj = dto.Cnpj,
                InscricaoEstadual = dto.InscricaoEstadual,
                EmailCorporativo = dto.EmailCorporativo,
                Telefone = dto.Telefone
            };

            await _dbContext.Fornecedores.AddAsync(novoFornecedor);
            await _dbContext.SaveChangesAsync();
            return novoFornecedor;
        }

        public async Task<bool> Atualizar(Guid id, EditarFornecedorDTO dto)
        {
            var fornecedor = await _dbContext.Fornecedores.FindAsync(id);
            if (fornecedor == null) return false;

            fornecedor.RazaoSocial = dto.RazaoSocial;
            fornecedor.NomeFantasia = dto.NomeFantasia;
            fornecedor.InscricaoEstadual = dto.InscricaoEstadual;
            fornecedor.EmailCorporativo = dto.EmailCorporativo;
            fornecedor.Telefone = dto.Telefone;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(Guid id)
        {
            var fornecedor = await _dbContext.Fornecedores.FindAsync(id);
            if (fornecedor == null) return false;

            bool temHistoricoNoEstoque = await _dbContext.MovimentacoesEstoque.AnyAsync(m => m.FornecedorId == id);
            if (temHistoricoNoEstoque) return false;

            _dbContext.Fornecedores.Remove(fornecedor);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
