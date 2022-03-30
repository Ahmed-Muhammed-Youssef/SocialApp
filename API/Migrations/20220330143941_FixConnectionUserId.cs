using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class FixConnectionUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Connections");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Connections",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Connections");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Connections",
                type: "TEXT",
                nullable: true);
        }
    }
}
