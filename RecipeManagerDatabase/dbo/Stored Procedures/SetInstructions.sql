-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SetInstructions] 
	-- Add the parameters for the stored procedure here
	@Instructions InstructionsUDT READONLY,
	@RecipeId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	MERGE Instructions AS TARGET
	USING @Instructions AS SOURCE
	ON TARGET.InstructionNumber = SOURCE.InstructionNumber and TARGET.RecipeId = @RecipeId
	WHEN MATCHED and TARGET.InstructionText <> SOURCE.InstructionText THEN
	    UPDATE SET TARGET.InstructionText = SOURCE.InstructionText
	WHEN NOT MATCHED BY TARGET THEN --when the source isnt matched by target then it needs to be inserted
		INSERT (InstructionNumber, InstructionText, RecipeId)
		VALUES (SOURCE.InstructionNumber, SOURCE.InstructionText, SOURCE.RecipeId)
	WHEN NOT MATCHED BY SOURCE and TARGET.RecipeId = @RecipeId THEN --when tagret isn't matched by source then delete it
		DELETE;
END
