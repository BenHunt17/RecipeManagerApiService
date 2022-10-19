-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SelectRecipeCount]
	-- Add the parameters for the stored procedure here@Offset int = NULL,
	@Offset int = NULL,
	@Limit int = NULL,
	@RecipeNameSnippet varchar(80) = NULL,
	@MinRating int = NULL,
	@MaxRating int = NULL,
	@MinPrepTime int = NULL,
	@MaxPrepTime int = NULL,
	@MinServingSize int = NULL,
	@MaxServingSize int = NULL,
	@Breakfast bit = NULL,
	@Lunch bit = NULL,
	@Dinner bit = NULL
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*)
	FROM Recipes
	WHERE 
		(@RecipeNameSnippet IS NULL OR RecipeName LIKE '%' + @RecipeNameSnippet + '%') AND
		(@MinRating IS NULL OR Rating >= @MinRating) AND
		(@MaxRating IS NULL OR Rating <= @MaxRating) AND
		(@MinPrepTime IS NULL OR PrepTime >= @MinPrepTime) AND
		(@MaxPrepTime IS NULL OR PrepTime <= @MaxPrepTime) AND
		(@MinServingSize IS NULL OR ServingSize >= @MinServingSize) AND
		(@MaxServingSize IS NULL OR ServingSize <= @MaxServingSize) AND
		(@Breakfast IS NULL OR Breakfast = @Breakfast) AND
		(@Lunch IS NULL OR Lunch = @Lunch) AND
		(@Dinner IS NULL OR Dinner = @Dinner);
END
