using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivroAssuntos_Livros_LivroCod",
                table: "LivroAssuntos");

            migrationBuilder.DropForeignKey(
                name: "FK_LivroAutores_Livros_LivroCod",
                table: "LivroAutores");

            migrationBuilder.RenameColumn(
                name: "Cod",
                table: "Livros",
                newName: "Codl");

            migrationBuilder.RenameColumn(
                name: "LivroCod",
                table: "LivroAutores",
                newName: "LivroCodl");

            migrationBuilder.RenameColumn(
                name: "LivroCod",
                table: "LivroAssuntos",
                newName: "LivroCodl");

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAssuntos_Livros_LivroCodl",
                table: "LivroAssuntos",
                column: "LivroCodl",
                principalTable: "Livros",
                principalColumn: "Codl",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAutores_Livros_LivroCodl",
                table: "LivroAutores",
                column: "LivroCodl",
                principalTable: "Livros",
                principalColumn: "Codl",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivroAssuntos_Livros_LivroCodl",
                table: "LivroAssuntos");

            migrationBuilder.DropForeignKey(
                name: "FK_LivroAutores_Livros_LivroCodl",
                table: "LivroAutores");

            migrationBuilder.RenameColumn(
                name: "Codl",
                table: "Livros",
                newName: "Cod");

            migrationBuilder.RenameColumn(
                name: "LivroCodl",
                table: "LivroAutores",
                newName: "LivroCod");

            migrationBuilder.RenameColumn(
                name: "LivroCodl",
                table: "LivroAssuntos",
                newName: "LivroCod");

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAssuntos_Livros_LivroCod",
                table: "LivroAssuntos",
                column: "LivroCod",
                principalTable: "Livros",
                principalColumn: "Cod",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAutores_Livros_LivroCod",
                table: "LivroAutores",
                column: "LivroCod",
                principalTable: "Livros",
                principalColumn: "Cod",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
