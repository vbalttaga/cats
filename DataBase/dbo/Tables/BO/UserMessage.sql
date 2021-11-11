CREATE TABLE [dbo].[UserMessage] (
    [Id]        BIGINT IDENTITY (1, 1) NOT NULL,
    [MessageId] BIGINT NOT NULL,
    [UserId]    BIGINT NOT NULL,
    [DateCancel]  DATETIME NULL, 
    [UpdatedBy] BIGINT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserMessage_Message] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Message] ([MessageId]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserMessage_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE CASCADE
);




