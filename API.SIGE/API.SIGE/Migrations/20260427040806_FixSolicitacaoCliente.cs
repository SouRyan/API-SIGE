using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.SIGE.Migrations
{
    /// <inheritdoc />
    public partial class FixSolicitacaoCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Usuario_IdCliente",
                table: "Obra");

            migrationBuilder.CreateTable(
                name: "SolicitacaoCliente",
                columns: table => new
                {
                    IdSolicitacao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCaixilho = table.Column<int>(type: "integer", nullable: false),
                    IdCliente = table.Column<int>(type: "integer", nullable: false),
                    DataNecessidadeEmObra = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ObservacaoCliente = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Prioridade = table.Column<int>(type: "integer", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoCliente", x => x.IdSolicitacao);
                    table.ForeignKey(
                        name: "FK_SolicitacaoCliente_Caixilho_IdCaixilho",
                        column: x => x.IdCaixilho,
                        principalTable: "Caixilho",
                        principalColumn: "IdCaixilho",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoCliente_Usuario_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoCliente_IdCaixilho",
                table: "SolicitacaoCliente",
                column: "IdCaixilho");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoCliente_IdCliente",
                table: "SolicitacaoCliente",
                column: "IdCliente");

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Usuario_IdCliente",
                table: "Obra",
                column: "IdCliente",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Usuario_IdCliente",
                table: "Obra");

            migrationBuilder.DropTable(
                name: "SolicitacaoCliente");

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Usuario_IdCliente",
                table: "Obra",
                column: "IdCliente",
                principalTable: "Usuario",
                principalColumn: "IdUsuario");
        }
    }
}
