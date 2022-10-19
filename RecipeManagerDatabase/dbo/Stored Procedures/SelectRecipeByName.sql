-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SelectRecipeByName] 
	@RecipeName varchar(80)
AS
BEGIN
	SELECT * 
	FROM dbo.Recipes
	WHERE RecipeName = @RecipeName;
END
