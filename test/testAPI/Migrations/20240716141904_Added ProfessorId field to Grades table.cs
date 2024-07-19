using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedProfessorIdfieldtoGradestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Users_StudentId",
                table: "Grades");

            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "Grades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ProfessorId",
                table: "Grades",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Users_ProfessorId",
                table: "Grades",
                column: "ProfessorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Users_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Users_ProfessorId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Users_StudentId",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Grades_ProfessorId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Grades");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Users_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
