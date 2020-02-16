CREATE TRIGGER SetRoleLeaderAfterUpdateOrInsert ON dbo.Users_Teams
AFTER INSERT, UPDATE
AS

DECLARE @LeaderId NVARCHAR(450); 
DECLARE @IsLeader BIT;
BEGIN
	SELECT @LeaderId = i.UserId, @IsLeader = i.IsLeader FROM inserted i;
	IF @IsLeader = 1
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

