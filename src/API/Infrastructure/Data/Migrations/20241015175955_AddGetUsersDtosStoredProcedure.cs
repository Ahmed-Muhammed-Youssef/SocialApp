using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddGetUsersDtosStoredProcedure : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"CREATE PROCEDURE[dbo].[GetUsersDtos]
    @userId INT,
            @minAge INT,
    @maxAge INT,
            @orderBy SMALLINT,
            @pageNumber INT,
    @pageSize INT
AS
BEGIN
    SET NOCOUNT ON;

            --take all the forbidden ids in this table
            DECLARE @ForbiddenIdsTable TABLE(Id INT);

            --get friend requested users
            INSERT INTO @ForbiddenIdsTable

    SELECT U.Id
            FROM FriendRequests AS FR
    INNER JOIN dbo.ApplicationUsers AS U ON FR.RequestedId = U.Id
    WHERE RequesterId = @userId;

            --get the user friends
            INSERT INTO @ForbiddenIdsTable

    SELECT FriendId

    FROM dbo.Friends
    WHERE UserId = @userId;

            --Calculate the date of birth ranges based on the age parameters
    DECLARE @maxDoB AS DATETIME;
            DECLARE @minDoB AS DATETIME;

            IF NOT(@maxAge IS NULL)
    BEGIN
        SET @minDoB = DATEADD(YEAR, -@maxAge - 1, GETUTCDATE());
            END

            IF NOT(@minAge IS NULL)
    BEGIN
        SET @maxDoB = DATEADD(YEAR, -@minAge, GETUTCDATE());
            END

            -- Calculate the number of rows to skip
            DECLARE @skip INT = (@pageNumber - 1) * @pageSize;

            --Build the query
    SELECT U.Id,
		U.FirstName,
		U.LastName,
		U.ProfilePictureUrl,
		U.Sex,
		CONVERT(int, ROUND(DATEDIFF(hour, U.DateOfBirth, GETDATE()) / 8766.0, 0)) As 'Age',
		U.Created,
		U.LastActive,
		U.Bio,
		U.CityId
    FROM dbo.ApplicationUsers AS U
    WHERE Id != @userId
    AND(@minAge IS NULL OR DateOfBirth <= @maxDoB)
    AND(@maxAge IS NULL OR DateOfBirth >= @minDoB)
    AND Id NOT IN(SELECT Id FROM @ForbiddenIdsTable)
    ORDER BY
        CASE @orderBy
            WHEN 2 THEN Created
            WHEN 1 THEN DateOfBirth
            ELSE LastActive
        END DESC
    OFFSET @skip ROWS
    FETCH NEXT @pageSize ROWS ONLY;
            END");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP PROCEDURE GetUsersDtos");
    }
}
