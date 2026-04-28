using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.SIGE.Migrations
{
    /// <inheritdoc />
    public partial class AddSolicitacaoClienteEClienteObra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "Usuario",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14);

            migrationBuilder.AddColumn<int>(
                name: "IdCliente",
                table: "Obra",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Obra_IdCliente",
                table: "Obra",
                column: "IdCliente");

            migrationBuilder.AddForeignKey(
                name: "FK_Obra_Usuario_IdCliente",
                table: "Obra",
                column: "IdCliente",
                principalTable: "Usuario",
                principalColumn: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Usuario_IdCliente",
                table: "Obra");

            migrationBuilder.DropIndex(
                name: "IX_Obra_IdCliente",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "Obra");

            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "Usuario",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);
        }
    }
}
