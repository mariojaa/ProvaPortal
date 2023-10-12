using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProvaPortal.Migrations
{
    public partial class CriarVinculoProfessorXProva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "Provas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfessorModelId",
                table: "Provas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provas_ProfessorId",
                table: "Provas",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Provas_ProfessorModelId",
                table: "Provas",
                column: "ProfessorModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Provas_Professores_ProfessorId",
                table: "Provas",
                column: "ProfessorId",
                principalTable: "Professores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Provas_Professores_ProfessorModelId",
                table: "Provas",
                column: "ProfessorModelId",
                principalTable: "Professores",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Provas_Professores_ProfessorId",
                table: "Provas");

            migrationBuilder.DropForeignKey(
                name: "FK_Provas_Professores_ProfessorModelId",
                table: "Provas");

            migrationBuilder.DropIndex(
                name: "IX_Provas_ProfessorId",
                table: "Provas");

            migrationBuilder.DropIndex(
                name: "IX_Provas_ProfessorModelId",
                table: "Provas");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Provas");

            migrationBuilder.DropColumn(
                name: "ProfessorModelId",
                table: "Provas");
        }
    }
}
