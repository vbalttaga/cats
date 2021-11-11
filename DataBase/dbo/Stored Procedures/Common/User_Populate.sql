CREATE PROCEDURE [dbo].[User_Populate](	
	@UserID bigint
)
AS
	
	BEGIN

		SELECT u.UserId,u.Login,u.Timeout,u.UniqueId,u.Permission,u.RoleId,r.Name as RoleName, r.Permission as RolePermissions, u.Password, u.PersonId
				,u.DateCreated,u.LastLogin,u.CreatedBy,p.FirstName as PersonFirstName,p.LastName as PersonLastName,p.Email as PersonEmail
		FROM [User] u
		INNER JOIN [Person] p ON p.PersonId=u.PersonId
		INNER JOIN [Role] r ON r.RoleId=u.RoleId
		WHERE u.UserId=@UserID 
		 
		SELECT m.MessageId, m.Title,m.[Text],m.Date,m.UserId 
		FROM [Message] m
		WHERE m.UserId=@UserID 
		UNION
		SELECT m.MessageId, m.Title,m.[Text],m.Date,m.UserId 
		FROM [UserMessage] um
		INNER JOIN [Message] m ON um.MessageId=m.MessageId
		WHERE um.UserId=@UserID 
		ORDER BY [Date]		
	END