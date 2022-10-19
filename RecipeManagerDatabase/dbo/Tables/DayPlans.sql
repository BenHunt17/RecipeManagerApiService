CREATE TABLE [dbo].[DayPlans] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [DayNumber]  INT NULL,
    [MealPlanId] INT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([MealPlanId]) REFERENCES [dbo].[MealPlans] ([Id])
);

