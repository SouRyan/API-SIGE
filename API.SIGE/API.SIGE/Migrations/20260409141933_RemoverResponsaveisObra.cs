using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.SIGE.Migrations
{
    /// <inheritdoc />
    public partial class RemoverResponsaveisObra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Usuario_IdResponsavelMedicao",
                table: "Obra");

            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Usuario_IdResponsavelProducao",
                table: "Obra");

            migrationBuilder.DropForeignKey(
                name: "FK_Obra_Usuario_IdResponsavelVerificacao",
                table: "Obra");

            migrationBuilder.DropIndex(
                name: "IX_Obra_IdResponsavelMedicao",
                table: "Obra");

            migrationBuilder.DropIndex(
                name: "IX_Obra_IdResponsavelProducao",
                table: "Obra");

            migrationBuilder.DropIndex(
                name: "IX_Obra_IdResponsavelVerificacao",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "IdResponsavelMedicao",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "IdResponsavelProducao",
                table: "Obra");

            migrationBuilder.DropColumn(
                name: "IdResponsavelVerificacao",
                table: "Obra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
