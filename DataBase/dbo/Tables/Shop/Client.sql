CREATE TABLE [dbo].[Client]
(
	[ClientId] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[IdentifyNumber] NVARCHAR(100) NOT NULL,
	[PersonId] BIGINT NOT NULL,
	[DeletedBy] BIGINT NULL, 
    [CreatedBy] BIGINT NOT NULL, 
    [DateCreated] DATETIME NOT NULL

)
