CREATE TABLE [dbo].[Connections] (
    [ConnectionId] NVARCHAR (450) NOT NULL,
    [UserId]       INT            NOT NULL,
    [GroupName]    NVARCHAR (450) NULL,
    CONSTRAINT [PK_Connections] PRIMARY KEY CLUSTERED ([ConnectionId] ASC),
    CONSTRAINT [FK_Connections_Groups_GroupName] FOREIGN KEY ([GroupName]) REFERENCES [dbo].[Groups] ([Name])
);


GO
CREATE NONCLUSTERED INDEX [IX_Connections_GroupName]
    ON [dbo].[Connections]([GroupName] ASC);

