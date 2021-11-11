﻿CREATE TABLE [dbo].[User] (
    [UserId]      BIGINT           IDENTITY (1, 1) NOT NULL,
    [Login]       NVARCHAR (100)   NOT NULL,
    [Password]    NVARCHAR (50)    NOT NULL,
    [Timeout]     INT              NOT NULL,
    [Permission]  BIGINT           NOT NULL,
    [RoleId]      BIGINT           NOT NULL,
    [PersonId]    BIGINT           NOT NULL,
    [UniqueId]    UNIQUEIDENTIFIER NOT NULL,
    [LastLogin]   DATETIME         NULL,
    [ImageId]     BIGINT NULL,
    [Enabled]     BIT NOT NULL DEFAULT 1,
    [DisplayError]     BIT NULL,
    [UpdatedById]   BIGINT           NULL,
    [DeletedBy]   BIGINT           NULL,
    [CreatedBy]   BIGINT           NOT NULL,
    [DateCreated] DATETIME         NOT NULL,
    [DateUpdated] DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_User_Person] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
);


