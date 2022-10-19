-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SelectRecipeById]
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM Recipes 
	WHERE Id = @Id;

	SELECT r.*, rI.*
FROM recipes r 
INNER JOIN RecipeIngredients rI
ON r.Id = rI.RecipeId 
WHERE r.Id = 40;

END
