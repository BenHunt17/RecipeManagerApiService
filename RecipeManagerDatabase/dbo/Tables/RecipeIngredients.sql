CREATE TABLE [dbo].[RecipeIngredients] (
    [Id]           INT        IDENTITY (1, 1) NOT NULL,
    [Quantity]     FLOAT (53) NULL,
    [RecipeId]     INT        NULL,
    [IngredientId] INT        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__RecipeIngredient_recipe] FOREIGN KEY ([RecipeId]) REFERENCES [dbo].[Recipes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RecipeIngredients_ingredients] FOREIGN KEY ([IngredientId]) REFERENCES [dbo].[Ingredients] ([Id]) ON DELETE CASCADE
);

