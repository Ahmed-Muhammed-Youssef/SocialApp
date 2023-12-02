CREATE PROCEDURE GetUserById
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * 
	FROM dbo.AspNetUsers
	WHERE Id = @id;
END