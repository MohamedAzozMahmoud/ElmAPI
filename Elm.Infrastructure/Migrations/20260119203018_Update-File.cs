using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Files",
                newName: "ProfessorRating");

            migrationBuilder.RenameColumn(
                name: "RejectionReason",
                table: "Files",
                newName: "ProfessorComment");

            migrationBuilder.AddColumn<DateTime>(
                name: "RatedAt",
                table: "Files",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RatedByDoctorId",
                table: "Files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_RatedByDoctorId",
                table: "Files",
                column: "RatedByDoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Doctors_RatedByDoctorId",
                table: "Files",
                column: "RatedByDoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Doctors_RatedByDoctorId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_RatedByDoctorId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "RatedAt",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "RatedByDoctorId",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "ProfessorRating",
                table: "Files",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ProfessorComment",
                table: "Files",
                newName: "RejectionReason");
        }
    }
}
