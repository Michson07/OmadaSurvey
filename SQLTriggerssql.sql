CREATE TRIGGER SetRoleLeaderAfterUpdateOrInsert ON dbo.Users_Teams
AFTER INSERT, UPDATE
AS
	DECLARE @LeaderId NVARCHAR(450); 
	DECLARE @IsLeader BIT;
	DECLARE @IsAlreadyLeader INT;
BEGIN
	SELECT @LeaderId = i.UserId, @IsLeader = i.IsLeader 
		FROM inserted i;
	SELECT @IsAlreadyLeader = COUNT(*) 
		FROM dbo.AspNetUserRoles
		JOIN inserted as i
		ON dbo.AspNetUserRoles.UserId = i.UserId
		AND dbo.AspNetUserRoles.RoleId = 2
	IF @IsLeader = 1 AND @IsAlreadyLeader <= 0
		INSERT INTO dbo.AspNetUserRoles VALUES(@LeaderId, 2);
END
GO

CREATE TRIGGER SetRoleUserAfterInsert ON dbo.AspNetUsers
AFTER INSERT 
AS

DECLARE @UserId NVARCHAR(450);
BEGIN
	SELECT @UserId = i.Id FROM inserted i;
	INSERT INTO dbo.AspNetUserRoles VALUES(@UserId, 1);
END
GO

CREATE TRIGGER DeleteLeaderRole
ON dbo.Users_Teams
AFTER DELETE
AS
	DECLARE @HowManyLeader INT;
	DECLARE @UserId NVARCHAR(450);
BEGIN
	SELECT @UserId = UserId 
	FROM deleted;

	SELECT @HowManyLeader = COUNT(UserId)
	FROM dbo.Users_Teams 
	WHERE UserID = @UserId
	AND  IsLeader = 1;

	IF @HowManyLeader = 0
		DELETE FROM AspNetUserRoles
		WHERE UserId = @UserId
		AND RoleId = 2;
END
GO

