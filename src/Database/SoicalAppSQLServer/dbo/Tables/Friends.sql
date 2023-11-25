CREATE TABLE [dbo].[Friends] (
    [UserId]   INT           NOT NULL,
    [FriendId] INT           NOT NULL,
    [Created]  DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Friends] PRIMARY KEY CLUSTERED ([UserId] ASC, [FriendId] ASC),
    CONSTRAINT [FK_Friends_AspNetUsers_FriendId] FOREIGN KEY ([FriendId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Friends_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Friends_FriendId]
    ON [dbo].[Friends]([FriendId] ASC);

