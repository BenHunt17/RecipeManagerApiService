-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE SelectRecipesByIdOrName 
	-- Add the parameters for the stored procedure here
	@IdList IdListUDT READONLY,
	@NaturalKeyList NaturalKeyListUDT READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT *
	FROM dbo.Recipes
	WHERE 
		(((SELECT COUNT(*) FROM @IdList) = 0) OR Id IN (SELECT * FROM @IdList)) AND
		(((SELECT COUNT(*) FROM @NaturalKeyList) = 0) OR RecipeName IN (SELECT * FROM @NaturalKeyList))
END
