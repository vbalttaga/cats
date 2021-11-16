CREATE TABLE [dbo].[CatRealization]
(
	[CatRealizationId] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[ClientId] BIGINT NOT NULL,
	[CatId] BIGINT NOT NULL,
	[DeletedBy] BIGINT NULL, 
    [CreatedBy] BIGINT NOT NULL, 
    [DateCreated] DATETIME NOT NULL
	

)
