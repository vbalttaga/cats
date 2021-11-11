CREATE TABLE [dbo].[Message]
(
	[MessageId] BIGINT NOT NULL PRIMARY KEY  IDENTITY, 
	[Text] NVARCHAR(MAX) NOT NULL, 
    [Date] DATE NOT NULL, 
    [Title] NVARCHAR(300) NOT NULL, 
    [UserId] BIGINT NOT NULL, 
    [DeletedBy] BIGINT NULL, 
    [CreatedBy] BIGINT NOT NULL, 
    [DateCreated] DATETIME NOT NULL, 
    [DateUpdated] DATETIME NULL,
    [DateCancel]  DATETIME NULL, 
    [UpdatedBy] BIGINT NULL
)
