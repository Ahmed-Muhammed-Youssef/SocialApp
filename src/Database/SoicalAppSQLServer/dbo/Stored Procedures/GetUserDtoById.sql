CREATE PROCEDURE GetUserDtoById
	@id int
AS
BEGIN
	SET NOCOUNT ON;
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
	WHERE Id = @id;
END