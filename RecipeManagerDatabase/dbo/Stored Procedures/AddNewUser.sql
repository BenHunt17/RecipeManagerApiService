-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE AddNewUser
	-- Add the parameters for the stored procedure here
	@Id int = 0 OUTPUT,
	@Username varchar(80),
	@UserPassword varchar(128),
	@Salt varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	INSERT INTO Users (Username, UserPassword, Salt)
	VALUES (@Username, @UserPassword, @Salt);

	SET @Id = SCOPE_IDENTITY();

	RETURN @Id;
END
