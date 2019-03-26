CREATE TABLE [dbo].[EventProcessor] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [HubId]            INT           NOT NULL,
    [Name]             VARCHAR (255) NOT NULL,
    [CreationDateTime] DATETIME      CONSTRAINT [DF_EventProcessor_CreationDateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_EventProcessor] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EventProcessor_EventHub] FOREIGN KEY ([HubId]) REFERENCES [dbo].[EventHub] ([Id]),
    CONSTRAINT [UK_EventProcessor_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

