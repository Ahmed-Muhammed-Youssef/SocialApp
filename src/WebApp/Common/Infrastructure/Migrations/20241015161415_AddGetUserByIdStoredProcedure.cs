using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGetUserByIdStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
                        CREATE PROCEDURE GetUserById
                            @id int
                        AS
                        BEGIN
                            SET NOCOUNT ON;
                            SELECT * 
                            FROM dbo.AspNetUsers
                            WHERE Id = @id;
                        END";

            migrationBuilder.Sql(sp);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetUserById");
        }
    }
}
