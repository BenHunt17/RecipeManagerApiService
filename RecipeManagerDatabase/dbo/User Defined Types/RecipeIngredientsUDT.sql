CREATE TYPE [dbo].[RecipeIngredientsUDT] AS TABLE (
    [Quantity]     FLOAT (53) NULL,
    [IngredientId] INT        NULL,
    [RecipeId]     INT        NULL);

