CREATE TABLE [dbo].[MealPlans] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [MealPlanName] VARCHAR (80) NULL,
    [StartDate]    DATE         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([MealPlanName] ASC)
);

