CREATE TABLE [dbo].[Ingredients] (
    [Id]                    INT           IDENTITY (1, 1) NOT NULL,
    [IngredientName]        VARCHAR (80)  NULL,
    [IngredientDescription] VARCHAR (512) NULL,
    [ImageUrl]              VARCHAR (255) NULL,
    [MeasureUnitId]         INT           NULL,
    [Calories]              FLOAT (53)    NULL,
    [FruitVeg]              BIT           NULL,
    [Fat]                   FLOAT (53)    NULL,
    [Salt]                  FLOAT (53)    NULL,
    [Protein]               FLOAT (53)    NULL,
    [Carbs]                 FLOAT (53)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([IngredientName] ASC)
);

