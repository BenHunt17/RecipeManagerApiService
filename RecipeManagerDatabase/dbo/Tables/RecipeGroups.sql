CREATE TABLE [dbo].[RecipeGroups] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [GroupName]        VARCHAR (80)  NULL,
    [GroupDescription] VARCHAR (512) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([GroupName] ASC)
);

