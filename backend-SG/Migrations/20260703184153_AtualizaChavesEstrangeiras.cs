using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_SG.Migrations
{
    /// <inheritdoc />
    public partial class AtualizaChavesEstrangeiras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FornecedorId",
                table: "MovimentacoesEstoque",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_FornecedorId",
                table: "MovimentacoesEstoque",
                column: "FornecedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesEstoque_Fornecedores_FornecedorId",
                table: "MovimentacoesEstoque",
                column: "FornecedorId",
                principalTable: "Fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesEstoque_Fornecedores_FornecedorId",
                table: "MovimentacoesEstoque");

            migrationBuilder.DropIndex(
                name: "IX_MovimentacoesEstoque_FornecedorId",
                table: "MovimentacoesEstoque");

            migrationBuilder.DropColumn(
                name: "FornecedorId",
                table: "MovimentacoesEstoque");
        }
    }
}
