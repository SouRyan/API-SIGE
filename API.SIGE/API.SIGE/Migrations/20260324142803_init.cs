using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.SIGE.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FamiliaCaixilho",
                columns: table => new
                {
                    IdFamiliaCaixilho = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DescricaoFamilia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PesoTotal = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamiliaCaixilho", x => x.IdFamiliaCaixilho);
                });

            migrationBuilder.CreateTable(
                name: "TipoUsuario",
                columns: table => new
                {
                    IdTipoUsuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeTipoUsuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoUsuario", x => x.IdTipoUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeUsuario = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Senha = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    IdTipoUsuario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuario_TipoUsuario_IdTipoUsuario",
                        column: x => x.IdTipoUsuario,
                        principalTable: "TipoUsuario",
                        principalColumn: "IdTipoUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Obra",
                columns: table => new
                {
                    IdObra = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Construtora = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Nro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Logradouro = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cep = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Uf = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataTermino = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PesoFinal = table.Column<float>(type: "real", nullable: false),
                    PesoProduzido = table.Column<float>(type: "real", nullable: false),
                    PercentualConclusao = table.Column<float>(type: "real", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Finalizado = table.Column<bool>(type: "boolean", nullable: false),
                    ImagemObraPath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obra", x => x.IdObra);
                    table.ForeignKey(
                        name: "FK_Obra_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Producao",
                columns: table => new
                {
                    IdProducao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeProducao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DataProducao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Produzido = table.Column<bool>(type: "boolean", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EhLiberado = table.Column<bool>(type: "boolean", nullable: false),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    FamiliaCaixilhoId = table.Column<int>(type: "integer", nullable: false)
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
                    DataRelatorio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ano = table.Column<int>(type: "integer", nullable: false),
                    Mes = table.Column<int>(type: "integer", nullable: false),
                    PesoTotalProduzido = table.Column<float>(type: "real", nullable: false),
                    TotalCaixilhosProduzidos = table.Column<int>(type: "integer", nullable: false),
                    TotalFamiliasProduzidas = table.Column<int>(type: "integer", nullable: false),
                    EficienciaProducao = table.Column<float>(type: "real", nullable: false),
                    TempoMedioProducao = table.Column<float>(type: "real", nullable: false),
                    StatusMeta = table.Column<string>(type: "text", nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Caixilho",
                columns: table => new
                {
                    IdCaixilho = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeCaixilho = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Largura = table.Column<int>(type: "integer", nullable: false),
                    Altura = table.Column<int>(type: "integer", nullable: false),
                    Quantidade = table.Column<int>(type: "integer", nullable: false),
                    PesoUnitario = table.Column<float>(type: "real", nullable: false),
                    Liberado = table.Column<bool>(type: "boolean", nullable: false),
                    DataLiberacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DescricaoCaixilho = table.Column<string>(type: "text", nullable: true),
                    ObraId = table.Column<int>(type: "integer", nullable: false),
                    IdFamiliaCaixilho = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caixilho", x => x.IdCaixilho);
                    table.ForeignKey(
                        name: "FK_Caixilho_FamiliaCaixilho_IdFamiliaCaixilho",
                        column: x => x.IdFamiliaCaixilho,
                        principalTable: "FamiliaCaixilho",
                        principalColumn: "IdFamiliaCaixilho",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Caixilho_Obra_ObraId",
                        column: x => x.ObraId,
                        principalTable: "Obra",
                        principalColumn: "IdObra",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Caixilho_IdFamiliaCaixilho",
                table: "Caixilho",
                column: "IdFamiliaCaixilho");

            migrationBuilder.CreateIndex(
                name: "IX_Caixilho_ObraId",
                table: "Caixilho",
                column: "ObraId");

            migrationBuilder.CreateIndex(
                name: "IX_Obra_IdUsuario",
                table: "Obra",
                column: "IdUsuario");

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

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdTipoUsuario",
                table: "Usuario",
                column: "IdTipoUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Caixilho");

            migrationBuilder.DropTable(
                name: "Producao");

            migrationBuilder.DropTable(
                name: "RelatorioProducao");

            migrationBuilder.DropTable(
                name: "Obra");

            migrationBuilder.DropTable(
                name: "FamiliaCaixilho");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "TipoUsuario");
        }
    }
}
