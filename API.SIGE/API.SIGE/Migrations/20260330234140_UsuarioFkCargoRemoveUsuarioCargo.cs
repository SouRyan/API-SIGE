using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.SIGE.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioFkCargoRemoveUsuarioCargo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCargo",
                table: "Usuario",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql(@"
UPDATE ""Usuario"" u
SET ""IdCargo"" = sub.""IdCargo""
FROM (
    SELECT DISTINCT ON (""IdUsuario"") ""IdUsuario"", ""IdCargo""
    FROM ""UsuarioCargo""
    ORDER BY ""IdUsuario"", ""IdUsuarioCargo""
) sub
WHERE u.""IdUsuario"" = sub.""IdUsuario"";
");

            migrationBuilder.DropTable(
                name: "UsuarioCargo");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdCargo",
                table: "Usuario",
                column: "IdCargo");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Cargo_IdCargo",
                table: "Usuario",
                column: "IdCargo",
                principalTable: "Cargo",
                principalColumn: "IdCargo",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Cargo_IdCargo",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_IdCargo",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "IdCargo",
                table: "Usuario");

            migrationBuilder.CreateTable(
                name: "UsuarioCargo",
                columns: table => new
                {
                    IdUsuarioCargo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCargo = table.Column<int>(type: "integer", nullable: false),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCargo_IdCargo",
                table: "UsuarioCargo",
                column: "IdCargo");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCargo_IdUsuario_IdCargo",
                table: "UsuarioCargo",
                columns: new[] { "IdUsuario", "IdCargo" },
                unique: true);
        }
    }
}
