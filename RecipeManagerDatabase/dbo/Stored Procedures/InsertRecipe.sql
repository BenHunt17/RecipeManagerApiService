-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertRecipe] 
	-- Add the parameters for the stored procedure here
	@Id int = 0 OUTPUT,
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
	INSERT INTO Recipes (RecipeName, RecipeDescription, ImageUrl, Rating, PrepTime, ServingSize, Breakfast, Lunch, Dinner)
	VALUES (@RecipeName, @RecipeDescription, @ImageUrl, @Rating, @PrepTime, @ServingSize, @Breakfast, @Lunch, @Dinner);

	SET @Id = SCOPE_IDENTITY();

	RETURN @Id;
END
