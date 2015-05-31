/* 
	可以通过VisualStudio打开并且执行
	需要先链接到server的各个database，再执行以下脚本（由于azure的限制，不能使用use语句切换database，必须手动切换）
	执行前，务必删除原有的表，或者重新创建database
*/
CREATE USER [db-user-front] FOR LOGIN [db-user-front]
GO

EXEC sp_addrolemember N'db_datareader', N'db-user-front' 
GO

EXEC sp_addrolemember N'db_datawriter', N'db-user-front' 
GO


CREATE TABLE [dbo].[Grains_0] (
[Id] varchar(100) NOT NULL ,
[LongId] bigint NOT NULL ,
[Key] varchar(200) NOT NULL ,
[Type] varchar(200) NOT NULL ,
[TimeStamp] bigint NOT NULL ,
[Data] nvarchar(MAX) NOT NULL ,
CONSTRAINT PK_Key_TimeStamp_0 PRIMARY KEY ([Key],[TimeStamp])
)
GO

CREATE INDEX [IN_Key_0] ON [dbo].[Grains_0]
([Key] ASC) 
GO

CREATE INDEX [IN_Id_0] ON [dbo].[Grains_0]
([Id] ASC) 
GO

CREATE INDEX [IN_Key_TimeStamp_0] ON [dbo].[Grains_0]
([Key] ASC, [TimeStamp] DESC) 
GO

CREATE TABLE [dbo].[Grains_1] (
[Id] varchar(100) NOT NULL ,
[LongId] bigint NOT NULL,
[Key] varchar(200) NOT NULL ,
[Type] varchar(200) NOT NULL ,
[TimeStamp] bigint NOT NULL ,
[Data] nvarchar(MAX) NOT NULL ,
CONSTRAINT PK_Key_TimeStamp_1 PRIMARY KEY ([Key],[TimeStamp])
)
GO

CREATE INDEX [IN_Key_1] ON [dbo].[Grains_1]
([Key] ASC) 
GO

CREATE INDEX [IN_Id_1] ON [dbo].[Grains_1]
([Id] ASC) 
GO

CREATE INDEX [IN_Key_TimeStamp_1] ON [dbo].[Grains_1]
([Key] ASC, [TimeStamp] DESC) 
GO

CREATE TABLE [dbo].[Grains_2] (
[Id] varchar(100) NOT NULL ,
[LongId] bigint NOT NULL,
[Key] varchar(200) NOT NULL ,
[Type] varchar(200) NOT NULL ,
[TimeStamp] bigint NOT NULL ,
[Data] nvarchar(MAX) NOT NULL ,
CONSTRAINT PK_Key_TimeStamp_2 PRIMARY KEY ([Key],[TimeStamp])
)
GO

CREATE INDEX [IN_Key_2] ON [dbo].[Grains_2]
([Key] ASC) 
GO

CREATE INDEX [IN_Id_2] ON [dbo].[Grains_2]
([Id] ASC) 
GO

CREATE INDEX [IN_Key_TimeStamp_2] ON [dbo].[Grains_2]
([Key] ASC, [TimeStamp] DESC) 
GO

CREATE TABLE [dbo].[Grains_3] (
[Id] varchar(100) NOT NULL ,
[LongId] bigint NOT NULL,
[Key] varchar(200) NOT NULL ,
[Type] varchar(200) NOT NULL ,
[TimeStamp] bigint NOT NULL ,
[Data] nvarchar(MAX) NOT NULL ,
CONSTRAINT PK_Key_TimeStamp_3 PRIMARY KEY ([Key],[TimeStamp])
)
GO

CREATE INDEX [IN_Key_3] ON [dbo].[Grains_3]
([Key] ASC) 
GO

CREATE INDEX [IN_Id_3] ON [dbo].[Grains_3]
([Id] ASC) 
GO

CREATE INDEX [IN_Key_TimeStamp_3] ON [dbo].[Grains_3]
([Key] ASC, [TimeStamp] DESC) 
GO

CREATE TABLE [dbo].[Grains_4] (
[Id] varchar(100) NOT NULL ,
[LongId] bigint NOT NULL,
[Key] varchar(200) NOT NULL ,
[Type] varchar(200) NOT NULL ,
[TimeStamp] bigint NOT NULL ,
[Data] nvarchar(MAX) NOT NULL ,
CONSTRAINT PK_Key_TimeStamp_4 PRIMARY KEY ([Key],[TimeStamp])
)
GO

CREATE INDEX [IN_Key_4] ON [dbo].[Grains_4]
([Key] ASC) 
GO

CREATE INDEX [IN_Id_4] ON [dbo].[Grains_4]
([Id] ASC) 
GO

CREATE INDEX [IN_Key_TimeStamp_4] ON [dbo].[Grains_4]
([Key] ASC, [TimeStamp] DESC) 
GO

CREATE TABLE [dbo].[Grains_5] (
[Id] varchar(100) NOT NULL ,
[LongId] bigint NOT NULL,
[Key] varchar(200) NOT NULL ,
[Type] varchar(200) NOT NULL ,
[TimeStamp] bigint NOT NULL ,
[Data] nvarchar(MAX) NOT NULL ,
CONSTRAINT PK_Key_TimeStamp_5 PRIMARY KEY ([Key],[TimeStamp])
)
GO

CREATE INDEX [IN_Key_5] ON [dbo].[Grains_5]
([Key] ASC) 
GO

CREATE INDEX [IN_Id_5] ON [dbo].[Grains_5]
([Id] ASC) 
GO

CREATE INDEX [IN_Key_TimeStamp_5] ON [dbo].[Grains_5]
([Key] ASC, [TimeStamp] DESC) 
GO

CREATE TABLE [dbo].[Grains_6] (
[Id] varchar(100) NOT NULL ,
[LongId] bigint NOT NULL,
[Key] varchar(200) NOT NULL ,
[Type] varchar(200) NOT NULL ,
[TimeStamp] bigint NOT NULL ,
[Data] nvarchar(MAX) NOT NULL ,
CONSTRAINT PK_Key_TimeStamp_6 PRIMARY KEY ([Key],[TimeStamp])
)
GO

CREATE INDEX [IN_Key_6] ON [dbo].[Grains_6]
([Key] ASC) 
GO

CREATE INDEX [IN_Id_6] ON [dbo].[Grains_6]
([Id] ASC) 
GO

CREATE INDEX [IN_Key_TimeStamp_6] ON [dbo].[Grains_6]
([Key] ASC, [TimeStamp] DESC) 
GO

CREATE TABLE [dbo].[Grains_7] (
[Id] varchar(100) NOT NULL ,
[LongId] bigint NOT NULL,
[Key] varchar(200) NOT NULL ,
[Type] varchar(200) NOT NULL ,
[TimeStamp] bigint NOT NULL ,
[Data] nvarchar(MAX) NOT NULL ,
CONSTRAINT PK_Key_TimeStamp_7 PRIMARY KEY ([Key],[TimeStamp])
)

GO

CREATE INDEX [IN_Key_7] ON [dbo].[Grains_7]
([Key] ASC) 
GO

CREATE INDEX [IN_Id_7] ON [dbo].[Grains_7]
([Id] ASC) 
GO

CREATE INDEX [IN_Key_TimeStamp_7] ON [dbo].[Grains_7]
([Key] ASC, [TimeStamp] DESC) 
GO

CREATE TABLE [dbo].[Grains_8] (
[Id] varchar(100) NOT NULL ,
[LongId] bigint NOT NULL,
[Key] varchar(200) NOT NULL ,
[Type] varchar(200) NOT NULL ,
[TimeStamp] bigint NOT NULL ,
[Data] nvarchar(MAX) NOT NULL ,
CONSTRAINT PK_Key_TimeStamp_8 PRIMARY KEY ([Key],[TimeStamp])
)
GO

CREATE INDEX [IN_Key_8] ON [dbo].[Grains_8]
([Key] ASC) 
GO

CREATE INDEX [IN_Id_8] ON [dbo].[Grains_8]
([Id] ASC) 
GO

CREATE INDEX [IN_Key_TimeStamp_8] ON [dbo].[Grains_8]
([Key] ASC, [TimeStamp] DESC) 
GO