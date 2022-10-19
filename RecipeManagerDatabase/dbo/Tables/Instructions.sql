CREATE TABLE [dbo].[Instructions] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [InstructionNumber] INT           NULL,
    [InstructionText]   VARCHAR (255) NULL,
    [RecipeId]          INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__Instruction_recipe] FOREIGN KEY ([RecipeId]) REFERENCES [dbo].[Recipes] ([Id]) ON DELETE CASCADE
);

