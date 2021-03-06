CREATE TABLE [dbo].[MenuGroup]
(
	[MenuGroupId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [MenuTypeId] BIGINT NOT NULL, 
    [Permission] BIGINT NULL, 
    [Visible] BIT NOT NULL, 
    [SortOrder] INT NOT NULL, 
    [DeletedBy] BIGINT NULL, 
    [CreatedBy] BIGINT NOT NULL, 
    [DateCreated] DATETIME NOT NULL, 
    [DateUpdated] DATETIME NULL,
    [DateCancel]  DATETIME NULL, 
    [UpdatedBy] BIGINT NULL
)
