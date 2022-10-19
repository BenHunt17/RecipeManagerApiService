-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateUser]
	-- Add the parameters for the stored procedure here
	@Id int,
	@Username varchar(80),
	@UserPassword varchar(128),
	@RefreshToken varchar(128),
	@Salt varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	UPDATE dbo.Users
	SET Username = @Username,
		UserPassword = @UserPassword,
		RefreshToken = @RefreshToken,
		Salt = @Salt
		
	WHERE Id = @Id
END
