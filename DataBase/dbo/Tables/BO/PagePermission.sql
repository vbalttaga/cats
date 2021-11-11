CREATE TABLE [dbo].[PagePermission]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [PageId] BIGINT NOT NULL, 
    [PermissionId] BIGINT NOT NULL
)
