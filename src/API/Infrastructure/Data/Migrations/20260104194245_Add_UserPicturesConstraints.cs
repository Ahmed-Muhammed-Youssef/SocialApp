using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class Add_UserPicturesConstraints : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ApplicationUsers_Pictures_ProfilePictureId",
            table: "ApplicationUsers");

        migrationBuilder.AddForeignKey(
            name: "FK_ApplicationUsers_Pictures_ProfilePictureId",
            table: "ApplicationUsers",
            column: "ProfilePictureId",
            principalTable: "Pictures",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ApplicationUsers_Pictures_ProfilePictureId",
            table: "ApplicationUsers");

        migrationBuilder.AddForeignKey(
            name: "FK_ApplicationUsers_Pictures_ProfilePictureId",
            table: "ApplicationUsers",
            column: "ProfilePictureId",
            principalTable: "Pictures",
            principalColumn: "Id");
    }
}
