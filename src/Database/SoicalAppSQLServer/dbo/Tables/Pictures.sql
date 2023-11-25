CREATE TABLE [dbo].[Pictures] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Url]       NVARCHAR (MAX) NOT NULL,
    [Created]   DATETIME2 (7)  NOT NULL,
    [PublicId]  NVARCHAR (MAX) NOT NULL,
    [AppUserId] INT            NOT NULL,
    CONSTRAINT [PK_Pictures] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Pictures_AspNetUsers_AppUserId] FOREIGN KEY ([AppUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Pictures_AppUserId]
    ON [dbo].[Pictures]([AppUserId] ASC);

