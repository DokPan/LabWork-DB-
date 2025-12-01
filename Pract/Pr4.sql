USE [master]
GO

--1
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [ispp3407]    Script Date: 01.12.2025 13:44:00 ******/
CREATE LOGIN [ispp3407] 
WITH PASSWORD=N'Dnzz3w+dY0Hpe/gHDYlCCywZ/LmgOPMOPJiuaJ2hfDQ=', 
DEFAULT_DATABASE=[ispp3407], 
DEFAULT_LANGUAGE=[русский], 
CHECK_EXPIRATION=OFF, 
CHECK_POLICY=OFF
GO
EXEC sp_adduser 'login1', 'user1'
EXEC sp_adduser 'login2', 'user2'

CREATE USER [user3] FOR LOGIN [login3] WITH DEFAULT_SCHEMA=[dbo]
CREATE USER [user4] FOR LOGIN [login4] WITH DEFAULT_SCHEMA=[dbo]

EXEC sp_addlogin 'isppLoginNN2', 'Password!'

EXEC sp_addsrvrolemember 'isppLoginNN2', 'securityadmin'

--2
EXEC sp_addrolemember 'db_owner', 'user1'

EXEC sp_addrolemember 'db_datareader', 'user2'
EXEC sp_addrolemember 'db_datawriter', 'user2'

EXEC sp_droprolemember 'db_datawriter', 'user2'

--3
GRANT INSERT, DELETE 
ON Ticket
TO user3;

GRANT SELECT,  UPDATE (Name, Email)
ON Visitor 
TO user4;

DENY SELECT
ON Visitor 
TO user2;

DENY UPDATE(Name)
ON Visitor 
TO user4;

--4
declare @loginname INT = 1
declare @login NVARCHAR(100)

WHILE @loginname <= 4
BEGIN
	SET @login = 'reader' + CAST(@loginname AS NVARCHAR(10));
	--EXEC ('CREATE USER [' + @loginname + '] FOR LOGIN [' + @loginname + '] WITH DEFAULT_SCHEMA=[dbo]')
    EXEC('GRANT SELECT  TO [' + @login + '];')
	SET @loginname = @loginname + 1;
END

