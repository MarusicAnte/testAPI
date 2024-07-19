using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedExamstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Classrooms_ClassRoomId",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "ClassRoomId",
                table: "Exams",
                newName: "ClassroomId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_ClassRoomId",
                table: "Exams",
                newName: "IX_Exams_ClassroomId");

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "Exams",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Exams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Classrooms_ClassroomId",
                table: "Exams",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Classrooms_ClassroomId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "ClassroomId",
                table: "Exams",
                newName: "ClassRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Exams_ClassroomId",
                table: "Exams",
                newName: "IX_Exams_ClassRoomId");

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Exams",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Classrooms_ClassRoomId",
                table: "Exams",
                column: "ClassRoomId",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
