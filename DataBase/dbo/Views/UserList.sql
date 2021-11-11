CREATE VIEW [dbo].[UserList]
	AS
			
	SELECT u.UserId,u.Login,u.PersonId, u.RoleId, u.LastLogin,u.DateCreated,u.CreatedBy as CreatedById
	FROM [User] u
	WHERE u.DeletedBy IS NULL