using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_File : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "Files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DownloadCount",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
