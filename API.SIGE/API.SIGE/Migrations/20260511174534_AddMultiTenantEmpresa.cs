using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.SIGE.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiTenantEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cargo_TipoCargo",
                table: "Cargo");

            migrationBuilder.DeleteData(
                table: "Cargo",
                keyColumn: "IdCargo",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cargo",
                keyColumn: "IdCargo",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cargo",
                keyColumn: "IdCargo",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cargo",
                keyColumn: "IdCargo",
                keyValue: 4);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "Usuario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "TipoUsuario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "SolicitacaoCliente",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "ProducaoFamilia",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "Obra",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "Notificacao",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "Medicao",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "FamiliaCaixilho",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "Cargo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "Caixilho",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "Anexo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    IdEmpresa = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeEmpresa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cep = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmailResponsavel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Uf = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Logradouro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.IdEmpresa);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdEmpresa",
                table: "Usuario",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_TipoUsuario_IdEmpresa",
                table: "TipoUsuario",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoCliente_IdEmpresa",
                table: "SolicitacaoCliente",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_ProducaoFamilia_IdEmpresa",
                table: "ProducaoFamilia",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Obra_IdEmpresa",
                table: "Obra",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacao_IdEmpresa",
                table: "Notificacao",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Medicao_IdEmpresa",
                table: "Medicao",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_FamiliaCaixilho_IdEmpresa",
                table: "FamiliaCaixilho",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Cargo_IdEmpresa",
                table: "Cargo",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Cargo_TipoCargo_IdEmpresa",
                table: "Cargo",
                columns: new[] { "TipoCargo", "IdEmpresa" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Caixilho_IdEmpresa",
                table: "Caixilho",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Anexo_IdEmpresa",
                table: "Anexo",
                column: "IdEmpresa");

            migrationBuilder.AddForeignKey(
                name: "FK_Anexo_Empresa_IdEmpresa",
                table: "Anexo",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Caixilho_Empresa_IdEmpresa",
                table: "Caixilho",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cargo_Empresa_IdEmpresa",
                table: "Cargo",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FamiliaCaixilho_Empresa_IdEmpresa",
                table: "FamiliaCaixilho",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicao_Empresa_IdEmpresa",
                table: "Medicao",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacao_Empresa_IdEmpresa",
                table: "Notificacao",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Empresa_IdEmpresa",
                table: "Obra",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProducaoFamilia_Empresa_IdEmpresa",
                table: "ProducaoFamilia",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacaoCliente_Empresa_IdEmpresa",
                table: "SolicitacaoCliente",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TipoUsuario_Empresa_IdEmpresa",
                table: "TipoUsuario",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anexo_Empresa_IdEmpresa",
                table: "Anexo");

            migrationBuilder.DropForeignKey(
                name: "FK_Caixilho_Empresa_IdEmpresa",
                table: "Caixilho");

            migrationBuilder.DropForeignKey(
                name: "FK_Cargo_Empresa_IdEmpresa",
                table: "Cargo");

            migrationBuilder.DropForeignKey(
                name: "FK_FamiliaCaixilho_Empresa_IdEmpresa",
                table: "FamiliaCaixilho");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicao_Empresa_IdEmpresa",
                table: "Medicao");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacao_Empresa_IdEmpresa",
                table: "Notificacao");

            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Empresa_IdEmpresa",
                table: "Obra");

            migrationBuilder.DropForeignKey(
                name: "FK_ProducaoFamilia_Empresa_IdEmpresa",
                table: "ProducaoFamilia");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacaoCliente_Empresa_IdEmpresa",
                table: "SolicitacaoCliente");

            migrationBuilder.DropForeignKey(
                name: "FK_TipoUsuario_Empresa_IdEmpresa",
                table: "TipoUsuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Empresa_IdEmpresa",
                table: "Usuario");

            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_IdEmpresa",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_TipoUsuario_IdEmpresa",
                table: "TipoUsuario");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacaoCliente_IdEmpresa",
                table: "SolicitacaoCliente");

            migrationBuilder.DropIndex(
                name: "IX_ProducaoFamilia_IdEmpresa",
                table: "ProducaoFamilia");

            migrationBuilder.DropIndex(
                name: "IX_Obra_IdEmpresa",
                table: "Obra");

            migrationBuilder.DropIndex(
                name: "IX_Notificacao_IdEmpresa",
                table: "Notificacao");

            migrationBuilder.DropIndex(
                name: "IX_Medicao_IdEmpresa",
                table: "Medicao");

            migrationBuilder.DropIndex(
                name: "IX_FamiliaCaixilho_IdEmpresa",
                table: "FamiliaCaixilho");

            migrationBuilder.DropIndex(
                name: "IX_Cargo_IdEmpresa",
                table: "Cargo");

            migrationBuilder.DropIndex(
                name: "IX_Cargo_TipoCargo_IdEmpresa",
                table: "Cargo");

            migrationBuilder.DropIndex(
                name: "IX_Caixilho_IdEmpresa",
                table: "Caixilho");

            migrationBuilder.DropIndex(
                name: "IX_Anexo_IdEmpresa",
                table: "Anexo");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "TipoUsuario");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "SolicitacaoCliente");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "ProducaoFamilia");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "Notificacao");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "Medicao");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "FamiliaCaixilho");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "Cargo");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "Caixilho");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "Anexo");

            migrationBuilder.InsertData(
                table: "Cargo",
                columns: new[] { "IdCargo", "DescricaoCargo", "TipoCargo" },
                values: new object[,]
                {
                    { 1, "Gerente", 1 },
                    { 2, "Responsável pela verificação", 2 },
                    { 3, "Responsável pela medição", 3 },
                    { 4, "Responsável pela produção", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cargo_TipoCargo",
                table: "Cargo",
                column: "TipoCargo",
                unique: true);
        }
    }
}
