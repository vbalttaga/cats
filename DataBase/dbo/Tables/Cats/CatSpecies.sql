CREATE TABLE [dbo].[CatSpecies]
(
	[CatSpeciesId] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NULL,
	[DeletedBy] BIGINT NULL, 
    [CreatedBy] BIGINT NOT NULL, 
    [DateCreated] DATETIME NOT NULL
)
