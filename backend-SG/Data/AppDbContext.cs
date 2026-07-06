using backend_SG.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_SG.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; } = null!;
        public DbSet<TipoProduto> TiposProdutos { get; set; } = null!;
        public DbSet<MovimentacaoEstoque> MovimentacoesEstoque { get; set; } = null!;
        public DbSet<Pedido> Pedidos { get; set; } = null!;
        public DbSet<PedidoItem> PedidoItens { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Fornecedor> Fornecedores { get; set; } = null!;
    }
}
