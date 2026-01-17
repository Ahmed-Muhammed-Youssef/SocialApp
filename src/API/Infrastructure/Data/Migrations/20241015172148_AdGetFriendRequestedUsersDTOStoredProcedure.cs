using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class AdGetFriendRequestedUsersDTOStoredProcedure : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"CREATE PROCEDURE GetFriendRequestedUsersDTO
    @senderId INT
AS
BEGIN
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
    FROM FriendRequests AS FR
	INNER JOIN dbo.ApplicationUsers AS U ON FR.RequestedId = U.Id 
    WHERE RequesterId = @senderId;
END");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP PROCEDURE GetFriendRequestedUsersDTO");
    }
}
