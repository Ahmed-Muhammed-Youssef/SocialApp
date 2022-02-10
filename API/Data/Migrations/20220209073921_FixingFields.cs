using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class FixingFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<char>(
                name: "Sex",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<char>(
                name: "Interest",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Sex",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(char),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Interest",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(char),
                oldType: "TEXT");
        }
    }
}
