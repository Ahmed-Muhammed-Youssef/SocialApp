using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class Add_UserPictures : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "UserPictures",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>(type: "int", nullable: false),
                PictureId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserPictures", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserPictures_ApplicationUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "ApplicationUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserPictures_Pictures_PictureId",
                    column: x => x.PictureId,
                    principalTable: "Pictures",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_UserPictures_PictureId",
            table: "UserPictures",
            column: "PictureId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_UserPictures_UserId",
            table: "UserPictures",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserPictures");
    }
}
