CREATE PROCEDURE [dbo].[MenueGroup_Populate](	
	@RoleID bigint=null
)
AS
	
	BEGIN

		SELECT mg.MenuGroupId, mg.Name, mg.[Permission],mg.Visible,
		mt.MenuTypeId,mt.Alias as MenuTypeAlias,mt.Controller as MenuTypeController,mt.Name as MenuTypeName
		FROM MenuGroup mg
		INNER JOIN MenuType mt ON mt.MenuTypeId=mg.MenuTypeId
		LEFT JOIN MenuGroupRole mgr ON mgr.MenuGroupId=mg.MenuGroupId
		WHERE
		 mg.DeletedBy is NULL AND
		 (@RoleID IS NULL OR mgr.RoleId=@RoleID)
		ORDER BY
		 mg.SortOrder
		 
		SELECT mi.MenuItemId, mi.Name, mi.[Permission],mi.MenuGroupId,mi.[Object],mi.[Namespace],mi.Visible
		,p.PageId,p.PageObjectId,p.Permission as PagePermission,
		mt.MenuTypeId,mt.Alias as MenuTypeAlias,mt.Controller as MenuTypeController,mt.Name as MenuTypeName
		,mtp.MenuTypeId as PageMenuTypeId,mtp.Controller as PageMenuTypeController
		FROM MenuItem mi
		LEFT JOIN MenuItemRole mir ON mir.MenuItemId=mir.MenuItemId
		INNER JOIN MenuGroup mg ON mg.MenuGroupId=mi.MenuGroupId
		INNER JOIN MenuType mt ON mt.MenuTypeId=mi.MenuTypeId
		LEFT JOIN MenuGroupRole mgr ON mgr.MenuGroupId=mg.MenuGroupId
		LEFT JOIN [Page] p ON p.PageId=mi.PageId
		LEFT JOIN  MenuType mtp ON mtp.MenuTypeId=p.MenuTypeId
		WHERE
		 mi.DeletedBy is NULL AND
		 (@RoleID IS NULL OR mir.RoleId=@RoleID) AND
		 mg.DeletedBy is NULL AND
		 (@RoleID IS NULL OR mgr.RoleId=@RoleID)
		ORDER BY
		 mi.SortOrder
		
	END