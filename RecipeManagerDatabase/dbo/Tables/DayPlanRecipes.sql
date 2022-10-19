CREATE TABLE [dbo].[DayPlanRecipes] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [DayPlanId] INT NULL,
    [RecipeId]  INT NULL,
    FOREIGN KEY ([DayPlanId]) REFERENCES [dbo].[DayPlans] ([Id]),
    FOREIGN KEY ([RecipeId]) REFERENCES [dbo].[Recipes] ([Id])
);

