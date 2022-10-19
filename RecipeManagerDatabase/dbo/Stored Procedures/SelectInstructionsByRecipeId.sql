-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SelectInstructionsByRecipeId]
	@RecipeId int
AS
BEGIN
	SELECT * 
	FROM dbo.Instructions
	WHERE RecipeId = @RecipeId;
END
