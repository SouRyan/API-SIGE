using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.SIGE.Migrations
{
    /// <inheritdoc />
    public partial class EvolucaoSistemaCompleto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Producao");

            migrationBuilder.DropTable(
                name: "RelatorioProducao");

            migrationBuilder.AddColumn<int>(
                name: "IdResponsavelMedicao",
                table: "Obra",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdResponsavelProducao",
                table: "Obra",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdResponsavelVerificacao",
                table: "Obra",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PercentualMedicao",
                table: "Obra",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PercentualProducao",
                table: "Obra",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "StatusObra",
                table: "Obra",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "IdObra",
                table: "FamiliaCaixilho",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusFamilia",
                table: "FamiliaCaixilho",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "StatusProducao",
                table: "Caixilho",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "Cargo",
                columns: table => new
                {
                    IdCargo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoCargo = table.Column<int>(type: "integer", nullable: false),
                    DescricaoCargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargo", x => x.IdCargo);
                });

            migrationBuilder.CreateTable(
                name: "Medicao",
                columns: table => new
                {
                    IdMedicao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdFamiliaCaixilho = table.Column<int>(type: "integer", nullable: false),
                    IdResponsavel = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataEstimadaConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Descricao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicao", x => x.IdMedicao);
                    table.ForeignKey(
                        name: "FK_Medicao_FamiliaCaixilho_IdFamiliaCaixilho",
                        column: x => x.IdFamiliaCaixilho,
                        principalTable: "FamiliaCaixilho",
                        principalColumn: "IdFamiliaCaixilho",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medicao_Usuario_IdResponsavel",
                        column: x => x.IdResponsavel,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notificacao",
                columns: table => new
                {
                    IdNotificacao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUsuarioDestino = table.Column<int>(type: "integer", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Mensagem = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Lida = table.Column<bool>(type: "boolean", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TipoNotificacao = table.Column<int>(type: "integer", nullable: false),
                    IdObra = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacao", x => x.IdNotificacao);
                    table.ForeignKey(
                        name: "FK_Notificacao_Obra_IdObra",
                        column: x => x.IdObra,
                        principalTable: "Obra",
                        principalColumn: "IdObra",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Notificacao_Usuario_IdUsuarioDestino",
                        column: x => x.IdUsuarioDestino,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProducaoFamilia",
                columns: table => new
                {
                    IdProducaoFamilia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdFamiliaCaixilho = table.Column<int>(type: "integer", nullable: false),
                    IdResponsavel = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataEstimadaConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Descricao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducaoFamilia", x => x.IdProducaoFamilia);
                    table.ForeignKey(
                        name: "FK_ProducaoFamilia_FamiliaCaixilho_IdFamiliaCaixilho",
                        column: x => x.IdFamiliaCaixilho,
                        principalTable: "FamiliaCaixilho",
                        principalColumn: "IdFamiliaCaixilho",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProducaoFamilia_Usuario_IdResponsavel",
                        column: x => x.IdResponsavel,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioCargo",
                columns: table => new
                {
                    IdUsuarioCargo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdCargo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioCargo", x => x.IdUsuarioCargo);
                    table.ForeignKey(
                        name: "FK_UsuarioCargo_Cargo_IdCargo",
                        column: x => x.IdCargo,
                        principalTable: "Cargo",
                        principalColumn: "IdCargo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioCargo_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Anexo",
                columns: table => new
                {
                    IdAnexo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeArquivo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CaminhoArquivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TipoArquivo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: false),
                    DataUpload = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TipoAnexo = table.Column<int>(type: "integer", nullable: false),
                    IdMedicao = table.Column<int>(type: "integer", nullable: true),
                    IdProducaoFamilia = table.Column<int>(type: "integer", nullable: true),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anexo", x => x.IdAnexo);
                    table.ForeignKey(
                        name: "FK_Anexo_Medicao_IdMedicao",
                        column: x => x.IdMedicao,
                        principalTable: "Medicao",
                        principalColumn: "IdMedicao",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Anexo_ProducaoFamilia_IdProducaoFamilia",
                        column: x => x.IdProducaoFamilia,
                        principalTable: "ProducaoFamilia",
                        principalColumn: "IdProducaoFamilia",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Anexo_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_Obra_IdResponsavelMedicao",
                table: "Obra",
                column: "IdResponsavelMedicao");

            migrationBuilder.CreateIndex(
                name: "IX_Obra_IdResponsavelProducao",
                table: "Obra",
                column: "IdResponsavelProducao");

            migrationBuilder.CreateIndex(
                name: "IX_Obra_IdResponsavelVerificacao",
                table: "Obra",
                column: "IdResponsavelVerificacao");

            migrationBuilder.CreateIndex(
                name: "IX_FamiliaCaixilho_IdObra",
                table: "FamiliaCaixilho",
                column: "IdObra");

            migrationBuilder.CreateIndex(
                name: "IX_Anexo_IdMedicao",
                table: "Anexo",
                column: "IdMedicao");

            migrationBuilder.CreateIndex(
                name: "IX_Anexo_IdProducaoFamilia",
                table: "Anexo",
                column: "IdProducaoFamilia");

            migrationBuilder.CreateIndex(
                name: "IX_Anexo_IdUsuario",
                table: "Anexo",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Cargo_TipoCargo",
                table: "Cargo",
                column: "TipoCargo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicao_IdFamiliaCaixilho",
                table: "Medicao",
                column: "IdFamiliaCaixilho");

            migrationBuilder.CreateIndex(
                name: "IX_Medicao_IdResponsavel",
                table: "Medicao",
                column: "IdResponsavel");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacao_IdObra",
                table: "Notificacao",
                column: "IdObra");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacao_IdUsuarioDestino",
                table: "Notificacao",
                column: "IdUsuarioDestino");

            migrationBuilder.CreateIndex(
                name: "IX_ProducaoFamilia_IdFamiliaCaixilho",
                table: "ProducaoFamilia",
                column: "IdFamiliaCaixilho");

            migrationBuilder.CreateIndex(
                name: "IX_ProducaoFamilia_IdResponsavel",
                table: "ProducaoFamilia",
                column: "IdResponsavel");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCargo_IdCargo",
                table: "UsuarioCargo",
                column: "IdCargo");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCargo_IdUsuario_IdCargo",
                table: "UsuarioCargo",
                columns: new[] { "IdUsuario", "IdCargo" },
                unique: true);

            migrationBuilder.Sql(@"
UPDATE ""FamiliaCaixilho"" f
SET ""IdObra"" = (SELECT c.""ObraId"" FROM ""Caixilho"" c WHERE c.""IdFamiliaCaixilho"" = f.""IdFamiliaCaixilho"" LIMIT 1)
WHERE f.""IdObra"" = 0;
UPDATE ""FamiliaCaixilho"" SET ""IdObra"" = (SELECT ""IdObra"" FROM ""Obra"" ORDER BY ""IdObra"" LIMIT 1) WHERE ""IdObra"" = 0;
");

            migrationBuilder.AddForeignKey(
                name: "FK_FamiliaCaixilho_Obra_IdObra",
                table: "FamiliaCaixilho",
                column: "IdObra",
                principalTable: "Obra",
                principalColumn: "IdObra",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Usuario_IdResponsavelMedicao",
                table: "Obra",
                column: "IdResponsavelMedicao",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Usuario_IdResponsavelProducao",
                table: "Obra",
                column: "IdResponsavelProducao",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Usuario_IdResponsavelVerificacao",
                table: "Obra",
                column: "IdResponsavelVerificacao",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamiliaCaixilho_Obra_IdObra",
                table: "FamiliaCaixilho");

            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Usuario_IdResponsavelMedicao",
                table: "Obra");

            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Usuario_IdResponsavelProducao",
                table: "Obra");

            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Usuario_IdResponsavelVerificacao",
                table: "Obra");

            migrationBuilder.DropTable(
                name: "Anexo");

            migrationBuilder.DropTable(
                name: "Notificacao");

            migrationBuilder.DropTable(
                name: "UsuarioCargo");

            migrationBuilder.DropTable(
                name: "Medicao");

            migrationBuilder.DropTable(
                name: "ProducaoFamilia");

            migrationBuilder.DropTable(
                name: "Cargo");

            migrationBuilder.DropIndex(
                name: "IX_Obra_IdResponsavelMedicao",
                table: "Obra");

            migrationBuilder.DropIndex(
                name: "IX_Obra_IdResponsavelProducao",
                table: "Obra");

            migrationBuilder.DropIndex(
                name: "IX_Obra_IdResponsavelVerificacao",
                table: "Obra");

            migrationBuilder.DropIndex(
                name: "IX_FamiliaCaixilho_IdObra",
                table: "FamiliaCaixilho");

            migrationBuilder.DropColumn(
                name: "IdResponsavelMedicao",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "IdResponsavelProducao",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "IdResponsavelVerificacao",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "PercentualMedicao",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "PercentualProducao",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "StatusObra",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "IdObra",
                table: "FamiliaCaixilho");

            migrationBuilder.DropColumn(
                name: "StatusFamilia",
                table: "FamiliaCaixilho");

            migrationBuilder.DropColumn(
                name: "StatusProducao",
                table: "Caixilho");

            migrationBuilder.CreateTable(
                name: "Producao",
                columns: table => new
                {
                    IdProducao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FamiliaCaixilhoId = table.Column<int>(type: "integer", nullable: false),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    DataProducao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EhLiberado = table.Column<bool>(type: "boolean", nullable: false),
                    NomeProducao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Produzido = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producao", x => x.IdProducao);
                    table.ForeignKey(
                        name: "FK_Producao_FamiliaCaixilho_FamiliaCaixilhoId",
                        column: x => x.FamiliaCaixilhoId,
                        principalTable: "FamiliaCaixilho",
                        principalColumn: "IdFamiliaCaixilho",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producao_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatorioProducao",
                columns: table => new
                {
                    IdRelatorio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    Ano = table.Column<int>(type: "integer", nullable: false),
                    DataRelatorio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EficienciaProducao = table.Column<float>(type: "real", nullable: false),
                    Mes = table.Column<int>(type: "integer", nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PesoTotalProduzido = table.Column<float>(type: "real", nullable: false),
                    StatusMeta = table.Column<string>(type: "text", nullable: false),
                    TempoMedioProducao = table.Column<float>(type: "real", nullable: false),
                    TotalCaixilhosProduzidos = table.Column<int>(type: "integer", nullable: false),
                    TotalFamiliasProduzidas = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatorioProducao", x => x.IdRelatorio);
                    table.ForeignKey(
                        name: "FK_RelatorioProducao_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Producao_FamiliaCaixilhoId",
                table: "Producao",
                column: "FamiliaCaixilhoId");

            migrationBuilder.CreateIndex(
                name: "IX_Producao_IdUsuario",
                table: "Producao",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_RelatorioProducao_IdUsuario",
                table: "RelatorioProducao",
                column: "IdUsuario");
        }
    }
}
