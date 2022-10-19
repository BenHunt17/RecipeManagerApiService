CREATE TABLE [dbo].[Users] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Username]     VARCHAR (80)  NOT NULL,
    [UserPassword] VARCHAR (128) NOT NULL,
    [Salt]         VARCHAR (255) NOT NULL,
    [RefreshToken] VARCHAR (128) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

