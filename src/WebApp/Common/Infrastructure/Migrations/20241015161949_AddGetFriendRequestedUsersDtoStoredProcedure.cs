using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGetFriendRequestedUsersDtoStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE GetFriendRequestedUsersDTO
                            @senderId INT
                        AS
                        BEGIN
                            SELECT U.Id,
		                        U.UserName,
		                        U.FirstName,
		                        U.LastName,
		                        U.ProfilePictureUrl,
		                        U.Sex,
		                        U.Interest,
		                        CONVERT(int, ROUND(DATEDIFF(hour,U.DateOfBirth,GETDATE())/8766.0,0)) As 'Age',
		                        U.Created,
		                        U.LastActive,
		                        U.Bio,
		                        U.City,
		                        U.Country
                            FROM FriendRequests AS FR
	                        INNER JOIN dbo.AspNetUsers AS U ON FR.RequestedId = U.Id 
                            WHERE RequesterId = @senderId;
                        END";

            migrationBuilder.Sql(sp);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetFriendRequestedUsersDTO");
        }
    }
}
