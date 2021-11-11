CREATE PROCEDURE [dbo].[User_Populate_Latests](	
	@LastestId bigint=null
)
AS
	
	BEGIN

		SELECT top 20 u.UserId, u.Login,u.UpdatedById,u.UniqueId,u.DateCreated,u.LastLogin
		,r.RoleId,r.Name as RoleName
		,p.PersonId, p.FirstName as PersonFirstName, p.LastName as PersonLastName
		FROM [User] u
		INNER JOIN Person p ON p.PersonId=u.PersonId
		INNER JOIN [Role] r ON r.RoleId=u.RoleId
		WHERE u.DeletedBy iS NULL
		AND (@LastestId IS NULL OR u.UserId>@LastestId)
		ORDER BY u.UserId desc
		
	END