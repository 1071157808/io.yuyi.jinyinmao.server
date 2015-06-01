/* 
	可以通过VisualStudio打开并且执行
	需要先链接到server的master database，再执行以下脚本
	脚本会删除原有的数据库，并且重新创建，如果需要删除的数据库不存在，删除语句会报错，忽略错误，或者连续执行直至没有错误
	数据库版本
	EDITION = { 'web' | 'business' | 'basic' | 'standard' | 'premium' } 
	SERVICE_OBJECTIVE = { 'shared' | 'basic' | 'S0' | 'S1' | 'S2' | 'P1' | 'P2' | 'P3' }
*/
/*USE master*/
IF NOT EXISTS (SELECT * FROM [sys].[databases] WHERE [name] = N'jym-grains-0')
  SET NOEXEC ON
GO

DROP DATABASE [jym-grains-0]
GO

SET NOEXEC OFF
GO

CREATE DATABASE [jym-grains-0] collate Chinese_PRC_CI_AS (EDITION='basic', SERVICE_OBJECTIVE='basic')
GO

IF NOT EXISTS (SELECT * FROM [sys].[databases] WHERE [name] = N'jym-grains-1')
  SET NOEXEC ON
GO

DROP DATABASE [jym-grains-1]
GO

SET NOEXEC OFF
GO

CREATE DATABASE [jym-grains-1] collate Chinese_PRC_CI_AS (EDITION='basic', SERVICE_OBJECTIVE='basic')
GO

IF NOT EXISTS (SELECT * FROM [sys].[databases] WHERE [name] = N'jym-grains-2')
  SET NOEXEC ON
GO

DROP DATABASE [jym-grains-2]
GO

SET NOEXEC OFF
GO

CREATE DATABASE [jym-grains-2] collate Chinese_PRC_CI_AS (EDITION='basic', SERVICE_OBJECTIVE='basic')
GO

IF NOT EXISTS (SELECT * FROM [sys].[databases] WHERE [name] = N'jym-grains-3')
  SET NOEXEC ON
GO

DROP DATABASE [jym-grains-3]
GO

SET NOEXEC OFF
GO

CREATE DATABASE [jym-grains-3] collate Chinese_PRC_CI_AS (EDITION='basic', SERVICE_OBJECTIVE='basic')
GO

IF NOT EXISTS (SELECT * FROM [sys].[databases] WHERE [name] = N'jym-grains-4')
  SET NOEXEC ON
GO

DROP DATABASE [jym-grains-4]
GO

SET NOEXEC OFF
GO

CREATE DATABASE [jym-grains-4] collate Chinese_PRC_CI_AS (EDITION='basic', SERVICE_OBJECTIVE='basic')
GO

IF NOT EXISTS (SELECT * FROM [sys].[databases] WHERE [name] = N'jym-grains-5')
  SET NOEXEC ON
GO

DROP DATABASE [jym-grains-5]
GO

SET NOEXEC OFF
GO

CREATE DATABASE [jym-grains-5] collate Chinese_PRC_CI_AS (EDITION='basic', SERVICE_OBJECTIVE='basic')
GO