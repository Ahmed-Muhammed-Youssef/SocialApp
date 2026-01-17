using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class Update_FriendRequestsWorkFlow : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Pictures_ApplicationUsers_AppUserId",
            table: "Pictures");

        migrationBuilder.DropIndex(
            name: "IX_Pictures_AppUserId",
            table: "Pictures");

        migrationBuilder.DropPrimaryKey(
            name: "PK_FriendRequests",
            table: "FriendRequests");

        migrationBuilder.DropColumn(
            name: "AppUserId",
            table: "Pictures");

        migrationBuilder.DropColumn(
            name: "ProfilePictureUrl",
            table: "ApplicationUsers");

        migrationBuilder.DropColumn(
            name: "Sex",
            table: "ApplicationUsers");

        migrationBuilder.AddColumn<int>(
            name: "Id",
            table: "FriendRequests",
            type: "int",
            nullable: false,
            defaultValue: 0)
            .Annotation("SqlServer:Identity", "1, 1");

        migrationBuilder.AddColumn<int>(
            name: "Status",
            table: "FriendRequests",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "Gender",
            table: "ApplicationUsers",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "ProfilePictureId",
            table: "ApplicationUsers",
            type: "int",
            nullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_FriendRequests",
            table: "FriendRequests",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_FriendRequests_RequesterId",
            table: "FriendRequests",
            column: "RequesterId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_FriendRequests",
            table: "FriendRequests");

        migrationBuilder.DropIndex(
            name: "IX_FriendRequests_RequesterId",
            table: "FriendRequests");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "FriendRequests");

        migrationBuilder.DropColumn(
            name: "Status",
            table: "FriendRequests");

        migrationBuilder.DropColumn(
            name: "Gender",
            table: "ApplicationUsers");

        migrationBuilder.DropColumn(
            name: "ProfilePictureId",
            table: "ApplicationUsers");

        migrationBuilder.AddColumn<int>(
            name: "AppUserId",
            table: "Pictures",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "ProfilePictureUrl",
            table: "ApplicationUsers",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Sex",
            table: "ApplicationUsers",
            type: "nvarchar(1)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddPrimaryKey(
            name: "PK_FriendRequests",
            table: "FriendRequests",
            columns: new[] { "RequesterId", "RequestedId" });

        migrationBuilder.CreateIndex(
            name: "IX_Pictures_AppUserId",
            table: "Pictures",
            column: "AppUserId");

        migrationBuilder.AddForeignKey(
            name: "FK_Pictures_ApplicationUsers_AppUserId",
            table: "Pictures",
            column: "AppUserId",
            principalTable: "ApplicationUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
