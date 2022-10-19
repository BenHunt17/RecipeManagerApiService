-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SelectIngredients]

	@Offset int = NULL,
	@Limit int = NULL,
	@IngredientNameSnippet varchar(80) = NULL,
	@MinCalories float = NULL,
	@MaxCalories float = NULL,
	@MinFat float = NULL,
	@MaxFat float = NULL,
	@MinSalt float = NULL,
	@MaxSalt float = NULL,
	@MinProtein float = NULL,
	@MaxProtein float = NULL,
	@MinCarbs float = NULL,
	@MaxCarbs float = NULL,
	@FruitVeg bit = NULL

AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM dbo.Ingredients
	WHERE 
		(@IngredientNameSnippet IS NULL OR IngredientName LIKE '%' + @IngredientNameSnippet + '%') AND
		(@MinCalories IS NULL OR Calories >= @MinCalories) AND
		(@MaxCalories IS NULL OR Calories <= @MaxCalories) AND
		(@MinFat IS NULL OR Fat >= @MinFat) AND
		(@MaxFat IS NULL OR Fat <= @MaxFat) AND
		(@MinSalt IS NULL OR Salt >= @MinSalt) AND
		(@MaxSalt IS NULL OR Salt <= @MaxSalt) AND
		(@MinProtein IS NULL OR Protein >= @MinProtein) AND
		(@MaxProtein IS NULL OR Protein <= @MaxProtein) AND
		(@MinCarbs IS NULL OR Carbs >= @MinCarbs) AND
		(@MaxCarbs IS NULL OR Carbs <= @MaxCarbs) AND
		(@FruitVeg IS NULL OR FruitVeg = @FruitVeg)
	ORDER BY IngredientName ASC
	OFFSET @Offset ROWS
	FETCH NEXT @Limit ROWS ONLY 
END


