-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.UpdateRecipe
	-- Add the parameters for the stored procedure here
	@Id int,
	@RecipeName varchar(80),
	@RecipeDescription varchar(512),
	@ImageUrl varchar(255),
	@Rating int,
	@PrepTime int,
	@ServingSize int,
	@Breakfast bit,
	@Lunch bit,
	@Dinner bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	 -- Insert statements for procedure here
	UPDATE Recipes 
	SET RecipeName = @RecipeName,
		RecipeDescription = @RecipeDescription, 
		ImageUrl = @ImageUrl, 
		Rating = @Rating, 
		PrepTime = @PrepTime, 
		ServingSize = @ServingSize, 
		Breakfast = @Breakfast,
		Lunch = @Lunch, 
		Dinner = @Dinner
	WHERE Id = @Id;
END


