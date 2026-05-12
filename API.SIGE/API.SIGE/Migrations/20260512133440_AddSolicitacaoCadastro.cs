using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.SIGE.Migrations
{
    /// <inheritdoc />
    public partial class AddSolicitacaoCadastro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Empresa_IdEmpresa",
                table: "Obra");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Empresa_IdEmpresa",
                table: "Usuario");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Empresa",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SolicitacaoCadastro",
                columns: table => new
                {
                    IdSolicitacaoCadastro = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeEmpresa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false),
                    Cep = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Uf = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Logradouro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NomeResponsavel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmailResponsavel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TelefoneResponsavel = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAnalise = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MotivoRecusa = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoCadastro", x => x.IdSolicitacaoCadastro);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Empresa_IdEmpresa",
                table: "Obra",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Empresa_IdEmpresa",
                table: "Usuario",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Empresa_IdEmpresa",
                table: "Obra");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Empresa_IdEmpresa",
                table: "Usuario");

            migrationBuilder.DropTable(
                name: "SolicitacaoCadastro");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Empresa");

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Empresa_IdEmpresa",
                table: "Obra",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Empresa_IdEmpresa",
                table: "Usuario",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
