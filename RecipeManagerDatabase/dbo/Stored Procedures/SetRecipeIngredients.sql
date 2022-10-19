-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SetRecipeIngredients] 
	-- Add the parameters for the stored procedure here
	@RecipeIngredients RecipeIngredientsUDT READONLY,
	@recipeId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	MERGE RecipeIngredients AS TARGET
	USING @RecipeIngredients AS SOURCE
	ON TARGET.IngredientId = SOURCE.IngredientId and TARGET.RecipeId = @RecipeId
	WHEN MATCHED and TARGET.Quantity <> SOURCE.Quantity THEN
	    UPDATE SET TARGET.Quantity = SOURCE.Quantity
	WHEN NOT MATCHED BY TARGET THEN --when the source isnt matched by target then it needs to be inserted
		INSERT (Quantity, IngredientId, RecipeId)
		VALUES (SOURCE.Quantity, SOURCE.IngredientId, SOURCE.RecipeId)
	WHEN NOT MATCHED BY SOURCE and (TARGET.RecipeId = @RecipeId) THEN --when target isn't matched by source then delete it
		DELETE;
END
