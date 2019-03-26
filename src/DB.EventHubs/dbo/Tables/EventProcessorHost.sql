CREATE TABLE [dbo].[EventProcessorHost] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (255) NOT NULL,
    [LastActivityTime] DATETIME      NOT NULL,
    CONSTRAINT [PK_EventProcessorHost] PRIMARY KEY CLUSTERED ([Id] ASC)
);

