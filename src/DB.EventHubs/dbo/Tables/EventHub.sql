CREATE TABLE [dbo].[EventHub] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (255) NOT NULL,
    [CreationDateTime] DATETIME      CONSTRAINT [DF_EventHub_CreationDateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_EventHub] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_EventHub_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

