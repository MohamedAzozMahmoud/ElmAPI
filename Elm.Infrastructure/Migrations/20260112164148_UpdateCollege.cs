using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCollege : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Colleges_ImgId",
                table: "Colleges");

            migrationBuilder.AlterColumn<int>(
                name: "ImgId",
                table: "Colleges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Colleges_ImgId",
                table: "Colleges",
                column: "ImgId",
                unique: true,
                filter: "[ImgId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Colleges_ImgId",
                table: "Colleges");

            migrationBuilder.AlterColumn<int>(
                name: "ImgId",
                table: "Colleges",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colleges_ImgId",
                table: "Colleges",
                column: "ImgId",
                unique: true);
        }
    }
}
