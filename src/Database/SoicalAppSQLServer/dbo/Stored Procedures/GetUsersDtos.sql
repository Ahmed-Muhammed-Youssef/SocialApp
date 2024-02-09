CREATE PROCEDURE [dbo].[GetUsersDtos] 
    @userId NVARCHAR(50),
    @sex SMALLINT,
    @minAge INT,
    @maxAge INT,
    @orderBy SMALLINT,
    @pageNumber INT,
    @pageSize INT
AS
BEGIN
    SET NOCOUNT ON;

	-- check interest
    DECLARE @userInterest AS NVARCHAR(1);

    IF @sex = 1 -- male
    BEGIN
        SET @userInterest = 'm';
    END

    ELSE IF @sex = 2 -- female
    BEGIN
        SET @userInterest = 'f';
    END

    ELSE IF @sex = 3 -- both
    BEGIN 
        SET @userInterest = 'b';
    END

	ELSE
	BEGIN
		SELECT TOP 1 @userInterest = AspNetUsers.Interest
		FROM dbo.AspNetUsers
		WHERE Id = @userId;
	END

    -- take all the forbidden ids in this table
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
    DECLARE @maxDoB AS DATETIME;
    DECLARE @minDoB AS DATETIME;

    IF @maxAge <> NULL
    BEGIN
        SET @minDoB = DATEADD(YEAR, -@maxAge - 1, GETUTCDATE());
    END

    IF @minAge <> NULL
    BEGIN
        SET @maxDoB = DATEADD(YEAR, -@minAge, GETUTCDATE());
    END

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
    AND (@userInterest = 'b' OR Sex = @userInterest)
    AND (@minAge IS NULL OR DateOfBirth <= @maxDoB)
    AND (@maxAge IS NULL OR DateOfBirth >= @minDoB)
    AND Id NOT IN (SELECT Id FROM @ForbiddenIdsTable)
    ORDER BY 
        CASE @orderBy
            WHEN 2 THEN Created
            WHEN 1 THEN DateOfBirth
            ELSE LastActive
        END DESC
    OFFSET @skip ROWS
    FETCH NEXT @pageSize ROWS ONLY;
END