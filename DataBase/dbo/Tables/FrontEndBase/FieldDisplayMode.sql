CREATE TABLE [dbo].[FieldDisplayMode]
(
	[FieldDisplayModeId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
	[DisplayModeId] BIGINT NOT NULL, 
	[FieldId] BIGINT NOT NULL
)
