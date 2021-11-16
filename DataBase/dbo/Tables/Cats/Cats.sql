CREATE TABLE [dbo].[Cats]
(
	[CatsId] BIGINT NOT NULL PRIMARY KEY IDENTiTY,
	[Name] NVARCHAR(50) NOT NULL,
	[Birthdate]	date NULL,
	[SexId]	BIGINT NULL,
	[ColorId]	BIGINT NULL,
	[SourceId]	BIGINT NULL,
	[OwnerId]	BIGINT NULL,
	[SpeciesId]	BIGINT	 NULL,
	[DeletedBy] BIGINT NULL, 
    [CreatedBy] BIGINT NOT NULL, 
    [DateCreated] DATETIME NOT NULL
)
