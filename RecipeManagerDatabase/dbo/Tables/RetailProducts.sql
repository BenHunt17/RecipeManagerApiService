CREATE TABLE [dbo].[RetailProducts] (
    [Id]                INT          IDENTITY (1, 1) NOT NULL,
    [RetailProductName] VARCHAR (80) NULL,
    [Quantity]          FLOAT (53)   NULL,
    [Price]             MONEY        NULL,
    [lifespan]          INT          NULL,
    [IngredientId]      INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IngredientId]) REFERENCES [dbo].[Ingredients] ([Id]),
    UNIQUE NONCLUSTERED ([RetailProductName] ASC)
);

