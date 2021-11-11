CREATE PROCEDURE [dbo].[Field_Populate_Page]
(
	@PageObjectId nvarchar(50),
	@LaboratoryId bigint=null
)

AS
	BEGIN
		
		SELECT f.FieldId,f.FieldName,f.Name,f.PrintName,f.LaboratoryId,f.Permission
		FROM Field f
		INNER JOIN [Page] p ON p.PageId=f.PageId
		WHERE f.LaboratoryId=@LaboratoryId AND p.PageObjectId=@PageObjectId
		ORDER BY f.DateCreated asc

		
		SELECT fdm.FieldId,fdm.DisplayModeId
		FROM FieldDisplayMode fdm 
		INNER JOIN Field f ON f.FieldId=fdm.FieldId
		INNER JOIN [Page] p ON p.PageId=f.PageId
		WHERE f.LaboratoryId=@LaboratoryId AND p.PageObjectId=@PageObjectId
		
	END
