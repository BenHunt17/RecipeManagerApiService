-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertInstructions] 
	-- Add the parameters for the stored procedure here
	@Instructions InstructionsUDT readonly
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Instructions (InstructionNumber, InstructionText, RecipeId)
	SELECT [InstructionNumber], [InstructionText], [RecipeId]
	FROM @Instructions;
END
