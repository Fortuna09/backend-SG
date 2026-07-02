using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_SG.Migrations
{
    /// <inheritdoc />
    public partial class AjustaNomeTabelaPlural : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_TipoProduto_TipoProdutoId",
                table: "Produto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TipoProduto",
                table: "TipoProduto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Produto",
                table: "Produto");

            migrationBuilder.RenameTable(
                name: "TipoProduto",
                newName: "TiposProdutos");

            migrationBuilder.RenameTable(
                name: "Produto",
                newName: "Produtos");

            migrationBuilder.RenameIndex(
                name: "IX_Produto_TipoProdutoId",
                table: "Produtos",
                newName: "IX_Produtos_TipoProdutoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TiposProdutos",
                table: "TiposProdutos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_TiposProdutos_TipoProdutoId",
                table: "Produtos",
                column: "TipoProdutoId",
                principalTable: "TiposProdutos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_TiposProdutos_TipoProdutoId",
                table: "Produtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TiposProdutos",
                table: "TiposProdutos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos");

            migrationBuilder.RenameTable(
                name: "TiposProdutos",
                newName: "TipoProduto");

            migrationBuilder.RenameTable(
                name: "Produtos",
                newName: "Produto");

            migrationBuilder.RenameIndex(
                name: "IX_Produtos_TipoProdutoId",
                table: "Produto",
                newName: "IX_Produto_TipoProdutoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TipoProduto",
                table: "TipoProduto",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Produto",
                table: "Produto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_TipoProduto_TipoProdutoId",
                table: "Produto",
                column: "TipoProdutoId",
                principalTable: "TipoProduto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
