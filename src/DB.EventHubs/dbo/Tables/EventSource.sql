CREATE TABLE [dbo].[EventSource] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [HubId]            INT           NOT NULL,
    [Name]             VARCHAR (255) NOT NULL,
    [CreationDateTime] DATETIME      CONSTRAINT [DF_EventSource_CreationDateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_EventSource] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EventSource_EventHub] FOREIGN KEY ([HubId]) REFERENCES [dbo].[EventHub] ([Id]),
    CONSTRAINT [UK_EventSource_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

