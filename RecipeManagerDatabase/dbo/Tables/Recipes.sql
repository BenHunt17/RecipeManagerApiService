CREATE TABLE [dbo].[Recipes] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [RecipeName]        VARCHAR (80)  NULL,
    [RecipeDescription] VARCHAR (512) NULL,
    [ImageUrl]          VARCHAR (255) NULL,
    [Rating]            INT           NULL,
    [PrepTime]          INT           NULL,
    [ServingSize]       INT           NULL,
    [Breakfast]         BIT           NULL,
    [Lunch]             BIT           NULL,
    [Dinner]            BIT           NULL,
    [RecipeGroupId]     INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([RecipeGroupId]) REFERENCES [dbo].[RecipeGroups] ([Id]),
    UNIQUE NONCLUSTERED ([RecipeName] ASC)
);

