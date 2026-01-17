using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class ReaddCountriesTables : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Countries",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                Language = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Countries", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Regions",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                CountryId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Regions", x => x.Id);
                table.ForeignKey(
                    name: "FK_Regions_Countries_CountryId",
                    column: x => x.CountryId,
                    principalTable: "Countries",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Cities",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                RegionId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cities", x => x.Id);
                table.ForeignKey(
                    name: "FK_Cities_Regions_RegionId",
                    column: x => x.RegionId,
                    principalTable: "Regions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ApplicationUsers_CityId",
            table: "ApplicationUsers",
            column: "CityId");

        migrationBuilder.CreateIndex(
            name: "IX_Cities_RegionId",
            table: "Cities",
            column: "RegionId");

        migrationBuilder.CreateIndex(
            name: "IX_Regions_CountryId",
            table: "Regions",
            column: "CountryId");

        migrationBuilder.AddForeignKey(
            name: "FK_ApplicationUsers_Cities_CityId",
            table: "ApplicationUsers",
            column: "CityId",
            principalTable: "Cities",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ApplicationUsers_Cities_CityId",
            table: "ApplicationUsers");

        migrationBuilder.DropTable(
            name: "Cities");

        migrationBuilder.DropTable(
            name: "Regions");

        migrationBuilder.DropTable(
            name: "Countries");

        migrationBuilder.DropIndex(
            name: "IX_ApplicationUsers_CityId",
            table: "ApplicationUsers");
    }
}
