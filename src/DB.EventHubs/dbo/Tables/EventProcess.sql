CREATE TABLE [dbo].[EventProcess] (
    [Id]              INT      IDENTITY (1, 1) NOT NULL,
    [StartDateTime]   DATETIME CONSTRAINT [DF_EventProcess_CreationDateTime] DEFAULT (getdate()) NOT NULL,
    [ProcessStateId]  TINYINT  NOT NULL,
    [ProcessorId]     INT      NOT NULL,
    [ProcessorHostId] INT      NULL,
    [Savepoint]       INT      NOT NULL,
    [SaveDateTime]    DATETIME NULL,
    CONSTRAINT [PK_EventProcess] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EventProcess_EventData] FOREIGN KEY ([Savepoint]) REFERENCES [dbo].[EventData] ([Id]),
    CONSTRAINT [FK_EventProcess_EventProcessor] FOREIGN KEY ([ProcessorId]) REFERENCES [dbo].[EventProcessor] ([Id]),
    CONSTRAINT [FK_EventProcess_EventProcessorHost] FOREIGN KEY ([ProcessorHostId]) REFERENCES [dbo].[EventProcessorHost] ([Id]),
    CONSTRAINT [FK_EventProcess_EventProcessState] FOREIGN KEY ([ProcessStateId]) REFERENCES [dbo].[EventProcessState] ([Id])
);

