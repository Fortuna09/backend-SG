using backend_SG.Enums;
using backend_SG.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_SG.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; } = null!;
        public DbSet<TipoProduto> TiposProdutos { get; set; } = null!;
        public DbSet<MovimentacaoEstoque> MovimentacaoEstoque { get; set; } = null!;

    }
}
