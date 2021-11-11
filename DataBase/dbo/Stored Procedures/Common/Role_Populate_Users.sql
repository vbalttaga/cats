CREATE PROCEDURE [dbo].[Role_Populate_Users]
AS
	
	BEGIN

		SELECT r.RoleId,r.Name,COUNT(u.UserId) as UserCount
		FROM [User] u
		INNER JOIN [Role] r ON r.RoleId=u.RoleId
		WHERE u.DeletedBy iS NULL AND r.DeletedBy IS NULL
		GROUP BY r.RoleId,r.Name
		
	END