using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Main : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Curriculums_CurriculumId1",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_CurriculumId1",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "CurriculumId1",
                table: "Files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurriculumId1",
                table: "Files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_CurriculumId1",
                table: "Files",
                column: "CurriculumId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Curriculums_CurriculumId1",
                table: "Files",
                column: "CurriculumId1",
                principalTable: "Curriculums",
                principalColumn: "Id");
        }
    }
}
