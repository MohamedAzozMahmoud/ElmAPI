using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Curriculums_CurriculumId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionsBanks_QuestionBankId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsBanks_Curriculums_CurriculumId",
                table: "QuestionsBanks");

            migrationBuilder.DropForeignKey(
                name: "FK_Universities_Images_ImgId",
                table: "Universities");

            migrationBuilder.DropIndex(
                name: "IX_Years_CollegeId",
                table: "Years");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Years",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "EndMonth",
                table: "Curriculums",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Curriculums",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "StartMonth",
                table: "Curriculums",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Years_CollegeId_Name",
                table: "Years",
                columns: new[] { "CollegeId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Settings_Key",
                table: "Settings",
                column: "Key",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Curriculums_CurriculumId",
                table: "Files",
                column: "CurriculumId",
                principalTable: "Curriculums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionsBanks_QuestionBankId",
                table: "Questions",
                column: "QuestionBankId",
                principalTable: "QuestionsBanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsBanks_Curriculums_CurriculumId",
                table: "QuestionsBanks",
                column: "CurriculumId",
                principalTable: "Curriculums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Universities_Images_ImgId",
                table: "Universities",
                column: "ImgId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Curriculums_CurriculumId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionsBanks_QuestionBankId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsBanks_Curriculums_CurriculumId",
                table: "QuestionsBanks");

            migrationBuilder.DropForeignKey(
                name: "FK_Universities_Images_ImgId",
                table: "Universities");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Years_CollegeId_Name",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "EndMonth",
                table: "Curriculums");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Curriculums");

            migrationBuilder.DropColumn(
                name: "StartMonth",
                table: "Curriculums");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Years",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Years_CollegeId",
                table: "Years",
                column: "CollegeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Curriculums_CurriculumId",
                table: "Files",
                column: "CurriculumId",
                principalTable: "Curriculums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionsBanks_QuestionBankId",
                table: "Questions",
                column: "QuestionBankId",
                principalTable: "QuestionsBanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsBanks_Curriculums_CurriculumId",
                table: "QuestionsBanks",
                column: "CurriculumId",
                principalTable: "Curriculums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Universities_Images_ImgId",
                table: "Universities",
                column: "ImgId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
