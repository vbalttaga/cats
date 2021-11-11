CREATE PROCEDURE [dbo].[MenuItem_PopulateRecent](	
	@UserID bigint=null,
	@RoleID bigint=null,
	@Recent nvarchar(1000)
)
AS
	
	BEGIN
		 
		SELECT mi.MenuItemId, mi.Name, mi.[Permission],mi.MenuTypeId,mi.MenuGroupId,mi.Permission,mi.[Object],mi.[Namespace]
		,mt.Controller as MenuTypeController
		,p.PageId,p.PageObjectId
		,mtp.MenuTypeId as PageMenuTypeId,mtp.Controller as PageMenuTypeController
		FROM dbo.Split(@Recent,',') dr
		INNER JOIN  MenuItem mi ON mi.MenuItemId=CAST(dr.Value as bigint)
		INNER JOIN  MenuType mt ON mt.MenuTypeId=mi.MenuTypeId
		LEFT JOIN MenuItemRole mir ON mir.MenuItemId=mir.MenuItemId
		INNER JOIN MenuGroup mg ON mg.MenuGroupId=mi.MenuGroupId
		LEFT JOIN MenuGroupRole mgr ON mgr.MenuGroupId=mg.MenuGroupId
		LEFT JOIN [Page] p ON p.PageId=mi.PageId
		LEFT JOIN  MenuType mtp ON mtp.MenuTypeId=p.MenuTypeId
		WHERE
		 mi.DeletedBy is NULL AND
		 mi.Visible = 1 AND
		 (@RoleID IS NULL OR mir.RoleId=@RoleID) AND
		 mg.DeletedBy is NULL AND
		 mg.Visible = 1 AND
		 (@RoleID IS NULL OR mgr.RoleId=@RoleID)
		ORDER BY
		 dr.Ident
		
	END