CREATE TYPE [dbo].[InstructionsUDT] AS TABLE (
    [InstructionNumber] INT           NULL,
    [InstructionText]   VARCHAR (255) NULL,
    [recipeId]          INT           NULL);

