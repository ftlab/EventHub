CREATE TABLE [dbo].[EventData] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [SourceId]        INT             NOT NULL,
    [Timestamp]       DATETIME        NOT NULL,
    [EnqueueDateTime] DATETIME        CONSTRAINT [DF_EventData_EnqueueDateTime] DEFAULT (getdate()) NOT NULL,
    [Body]            VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_EventData] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EventData_EventSource] FOREIGN KEY ([SourceId]) REFERENCES [dbo].[EventSource] ([Id])
);

