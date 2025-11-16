using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChatMessagesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ApplicationUsers_RecipientId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropColumn(
                name: "ReadDate",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "RecipientDeleted",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderDeleted",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "RecipientId",
                table: "Messages",
                newName: "ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_RecipientId",
                table: "Messages",
                newName: "IX_Messages_ChatId");

            migrationBuilder.CreateTable(
                name: "DirectChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User1Id = table.Column<int>(type: "int", nullable: false),
                    User2Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirectChats_ApplicationUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DirectChats_ApplicationUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DirectChats_User1Id",
                table: "DirectChats",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_DirectChats_User2Id",
                table: "DirectChats",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_DirectChats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "DirectChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_DirectChats_ChatId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "DirectChats");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "Messages",
                newName: "RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                newName: "IX_Messages_RecipientId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadDate",
                table: "Messages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RecipientDeleted",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SenderDeleted",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    ConnectionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.ConnectionId);
                    table.ForeignKey(
                        name: "FK_Connections_Groups_GroupName",
                        column: x => x.GroupName,
                        principalTable: "Groups",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_GroupName",
                table: "Connections",
                column: "GroupName");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ApplicationUsers_RecipientId",
                table: "Messages",
                column: "RecipientId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
