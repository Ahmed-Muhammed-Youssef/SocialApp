CREATE TABLE [dbo].[Posts] (
    [Id]         DECIMAL (20)   IDENTITY (1, 1) NOT NULL,
    [UserId]     INT            NOT NULL,
    [Content]    NVARCHAR (MAX) NOT NULL,
    [DatePosted] DATETIME2 (7)  NOT NULL,
    [DateEdited] DATETIME2 (7)  NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Posts_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Posts_UserId]
    ON [dbo].[Posts]([UserId] ASC);

