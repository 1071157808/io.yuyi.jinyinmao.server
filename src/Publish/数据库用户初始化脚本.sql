/*USE master*/
IF EXISTS (SELECT * FROM [sys].[sql_logins] WHERE [name] = N'db-user-front')
  SET NOEXEC ON
GO

CREATE LOGIN [db-user-front] 
WITH PASSWORD = N'0SmDXp8i7MRfg29HJk1N' 
GO

SET NOEXEC OFF
GO

IF EXISTS (SELECT * FROM [sys].[sql_logins] WHERE [name] = N'db-user-backoffice')
  SET NOEXEC ON
GO

CREATE LOGIN [db-user-backoffice] 
WITH PASSWORD = N'0SmDXp8i7MRfg29HJk1N' 
GO

SET NOEXEC OFF
GO