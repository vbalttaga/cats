CREATE PROCEDURE [dbo].[Manuals_Populate]
	@RoleId BIGINT,
	@LanguageId BIGINT
	AS
		SELECT DISTINCT 
		m.ManualId,
		m.Name,
		m.RoleId,
		m.LanguageId,
	    m.DocumentId,
		d.Name AS DocumentName,
		d.FileName AS DocumentFileName,
		d.Ext AS DocumentExt
	FROM Manual m
	JOIN Document d ON d.DocumentId=m.DocumentId
	WHERE d.DeletedBy IS NULL AND m.DeletedBy IS NULL
	AND m.RoleId = @RoleId AND m.LanguageId = @LanguageId