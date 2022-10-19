-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertIngredient] 
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
	INSERT INTO Ingredients (IngredientName, IngredientDescription, ImageUrl, MeasureUnitId, Calories, FruitVeg, Fat,Salt, Protein, Carbs)
	VALUES (@IngredientName, @IngredientDescription, @ImageUrl, @MeasureUnitId, @Calories, @FruitVeg, @Fat, @Salt, @Protein, @Carbs);
END
