-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateIngredient]
	@Id INT,
	@IngredientName varchar(80),
    @IngredientDescription varchar(512),
    @ImageUrl varchar(255),
    @MeasureUnitId INT,
    @Calories FLOAT,
    @FruitVeg BIT,
    @Fat FLOAT,
    @Salt FLOAT,
    @Protein FLOAT,
    @Carbs FLOAT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE dbo.Ingredients
	SET IngredientName = @IngredientName,
		IngredientDescription = @IngredientDescription,
		ImageUrl = @ImageUrl, 
		MeasureUnitId = @MeasureUnitId, 
		Calories = @Calories, 
		FruitVeg = @FruitVeg, 
		Fat = @Fat,
		Salt = @Salt, 
		Protein = @Protein, 
		Carbs = @Carbs
	WHERE Id = @Id
END
