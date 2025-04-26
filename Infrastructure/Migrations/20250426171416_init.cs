using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assuntos",
                columns: table => new
                {
                    CodAs = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assuntos", x => x.CodAs);
                });

            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    CodAu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.CodAu);
                });

            migrationBuilder.CreateTable(
                name: "Livros",
                columns: table => new
                {
                    Codl = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Editora = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Edicao = table.Column<int>(type: "integer", nullable: false),
                    AnoPublicacao = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Preco = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0.00m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livros", x => x.Codl);
                });

            migrationBuilder.CreateTable(
                name: "BookTransactions",
                columns: table => new
                {
                    Codtr = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LivroCodl = table.Column<int>(type: "integer", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FormaDeCompra = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTransactions", x => x.Codtr);
                    table.ForeignKey(
                        name: "FK_BookTransactions_Livros_LivroCodl",
                        column: x => x.LivroCodl,
                        principalTable: "Livros",
                        principalColumn: "Codl",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivroAssuntos",
                columns: table => new
                {
                    LivroCodl = table.Column<int>(type: "integer", nullable: false),
                    AssuntoCodAs = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivroAssuntos", x => new { x.LivroCodl, x.AssuntoCodAs });
                    table.ForeignKey(
                        name: "FK_LivroAssuntos_Assuntos_AssuntoCodAs",
                        column: x => x.AssuntoCodAs,
                        principalTable: "Assuntos",
                        principalColumn: "CodAs",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LivroAssuntos_Livros_LivroCodl",
                        column: x => x.LivroCodl,
                        principalTable: "Livros",
                        principalColumn: "Codl",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LivroAutores",
                columns: table => new
                {
                    LivroCodl = table.Column<int>(type: "integer", nullable: false),
                    AutorCodAu = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivroAutores", x => new { x.LivroCodl, x.AutorCodAu });
                    table.ForeignKey(
                        name: "FK_LivroAutores_Autores_AutorCodAu",
                        column: x => x.AutorCodAu,
                        principalTable: "Autores",
                        principalColumn: "CodAu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LivroAutores_Livros_LivroCodl",
                        column: x => x.LivroCodl,
                        principalTable: "Livros",
                        principalColumn: "Codl",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Assuntos",
                columns: new[] { "CodAs", "Descricao" },
                values: new object[,]
                {
                    { 1, "Literatura" },
                    { 2, "Ficção" },
                    { 3, "Religião" },
                    { 4, "Mangá" }
                });

            migrationBuilder.InsertData(
                table: "Autores",
                columns: new[] { "CodAu", "Nome" },
                values: new object[,]
                {
                    { 1, "Machado de Assis" },
                    { 2, "Paulo Coelho" },
                    { 3, "Diversos Autores" },
                    { 4, "Tsugumi Ohba" }
                });

            migrationBuilder.InsertData(
                table: "Livros",
                columns: new[] { "Codl", "AnoPublicacao", "Edicao", "Editora", "Preco", "Titulo" },
                values: new object[,]
                {
                    { 1, "1899", 1, "Companhia das Letras", 49.90m, "Dom Casmurro" },
                    { 2, "1988", 1, "HarperCollins", 39.90m, "O Alquimista" }
                });

            migrationBuilder.InsertData(
                table: "Livros",
                columns: new[] { "Codl", "AnoPublicacao", "Edicao", "Editora", "Titulo" },
                values: new object[] { 3, "1969", 1, "Sociedade Bíblica do Brasil", "Bíblia Sagrada" });

            migrationBuilder.InsertData(
                table: "Livros",
                columns: new[] { "Codl", "AnoPublicacao", "Edicao", "Editora", "Preco", "Titulo" },
                values: new object[] { 4, "2003", 1, "Shueisha", 59.90m, "Death Note" });

            migrationBuilder.CreateIndex(
                name: "IX_BookTransactions_LivroCodl",
                table: "BookTransactions",
                column: "LivroCodl");

            migrationBuilder.CreateIndex(
                name: "IX_LivroAssuntos_AssuntoCodAs",
                table: "LivroAssuntos",
                column: "AssuntoCodAs");

            migrationBuilder.CreateIndex(
                name: "IX_LivroAutores_AutorCodAu",
                table: "LivroAutores",
                column: "AutorCodAu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookTransactions");

            migrationBuilder.DropTable(
                name: "LivroAssuntos");

            migrationBuilder.DropTable(
                name: "LivroAutores");

            migrationBuilder.DropTable(
                name: "Assuntos");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropTable(
                name: "Livros");
        }
    }
}
