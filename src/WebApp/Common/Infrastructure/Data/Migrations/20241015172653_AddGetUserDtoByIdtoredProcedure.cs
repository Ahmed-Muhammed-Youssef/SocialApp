using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGetUserDtoByIdtoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE GetUserDtoById
	@id int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT U.Id,
		U.FirstName,
		U.LastName,
		U.ProfilePictureUrl,
		U.Sex,
		CONVERT(int, ROUND(DATEDIFF(hour,U.DateOfBirth,GETDATE())/8766.0,0)) As 'Age',
		U.Created,
		U.LastActive,
		U.Bio,
		U.CityId 
	FROM dbo.ApplicationUsers AS U
	WHERE Id = @id;
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetUserDtoById");
        }
    }
}
