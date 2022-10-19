-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SelectRecipeIngredientsByRecipeId]
	@RecipeId int
AS
BEGIN
	SELECT *
	FROM dbo.RecipeIngredients
	WHERE RecipeId = @RecipeId;
END
