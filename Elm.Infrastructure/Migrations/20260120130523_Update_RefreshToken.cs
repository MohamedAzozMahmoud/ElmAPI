using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_RefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_AppUserId",
                table: "RefreshToken",
                column: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_AppUserId",
                table: "RefreshToken");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken",
                columns: new[] { "AppUserId", "Id" });
        }
    }
}
