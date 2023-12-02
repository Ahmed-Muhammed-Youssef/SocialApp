CREATE PROCEDURE [dbo].[GetUsersDtos] 
    @userId NVARCHAR(50),
    @sex NVARCHAR(1),
    @minAge INT,
    @maxAge INT,
    @orderBy NVARCHAR(50),
    @pageNumber INT,
    @pageSize INT
AS
BEGIN
    SET NOCOUNT ON;
	-- check interest
	IF ISNULL(@sex, '') = ''
	BEGIN
		SELECT TOP 1 @sex = AspNetUsers.Interest
		FROM dbo.AspNetUsers
		WHERE Id = @userId;
	END

    -- take all the forbidden id in this table
    DECLARE @ForbiddenIdsTable TABLE (Id INT);

	-- get friend requested users
	INSERT INTO @ForbiddenIdsTable
	SELECT U.Id
    FROM FriendRequests AS FR
	INNER JOIN dbo.AspNetUsers AS U ON FR.RequestedId = U.Id 
    WHERE RequesterId = @userId;

	-- get the user friends
	INSERT INTO @ForbiddenIdsTable
	SELECT FriendId
	FROM dbo.Friends
	WHERE UserId = @userId;

    -- Calculate the date of birth ranges based on the age parameters
    DECLARE @maxDoB DATETIME = DATEADD(YEAR, -@minAge, GETUTCDATE());
    DECLARE @minDoB DATETIME = DATEADD(YEAR, -@maxAge - 1, GETUTCDATE());

    -- Calculate the number of rows to skip
    DECLARE @skip INT = (@pageNumber - 1) * @pageSize;

    -- Build the query
    SELECT U.Id,
		U.Username,
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
    FROM dbo.AspNetUsers AS U
    WHERE Id != @userId
    AND (@sex = 'b' OR Sex = @sex)
    AND (DateOfBirth <= @maxDoB OR @minAge IS NULL)
    AND (DateOfBirth >= @minDoB OR @maxAge IS NULL)
    AND Id NOT IN (SELECT Id FROM @ForbiddenIdsTable)
    ORDER BY 
        CASE @orderBy
            WHEN 'creationTime' THEN Created
            WHEN 'age' THEN DateOfBirth
            ELSE LastActive
        END DESC
    OFFSET @skip ROWS
    FETCH NEXT @pageSize ROWS ONLY;
END