CREATE TABLE [dbo].[FriendRequests] (
    [RequesterId] INT           NOT NULL,
    [RequestedId] INT           NOT NULL,
    [Date]        DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_FriendRequests] PRIMARY KEY CLUSTERED ([RequesterId] ASC, [RequestedId] ASC),
    CONSTRAINT [FK_FriendRequests_AspNetUsers_RequestedId] FOREIGN KEY ([RequestedId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_FriendRequests_AspNetUsers_RequesterId] FOREIGN KEY ([RequesterId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FriendRequests_RequestedId]
    ON [dbo].[FriendRequests]([RequestedId] ASC);

