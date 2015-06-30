/*
Navicat SQL Server Data Transfer

Source Server         : uv1pb56i09
Source Server Version : 110000
Source Host           : uv1pb56i09.database.chinacloudapi.cn:1433
Source Database       : jym-dev
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 110000
File Encoding         : 65001

Date: 2015-05-27 13:48:24
*/
/*USE jym-biz*/
IF NOT EXISTS (SELECT * FROM [sys].[database_principals] WHERE [name] = N'db-user-backoffice')
  SET NOEXEC ON
GO

DROP USER [db-user-backoffice]
GO

SET NOEXEC OFF
GO

CREATE USER [db-user-backoffice] FOR LOGIN [db-user-backoffice]
GO

EXEC sp_addrolemember N'db_datareader', N'db-user-backoffice' 
GO

IF NOT EXISTS (SELECT * FROM [sys].[database_principals] WHERE [name] = N'db-user-front')
  SET NOEXEC ON
GO

DROP USER [db-user-front]
GO

SET NOEXEC OFF
GO

CREATE USER [db-user-front] FOR LOGIN [db-user-front]
GO

EXEC sp_addrolemember N'db_datareader', N'db-user-front' 
GO

EXEC sp_addrolemember N'db_datawriter', N'db-user-front' 
GO

-- ----------------------------
-- Table structure for AccountTransactions
-- ----------------------------
IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE [name] = N'AccountTransactions')
  SET NOEXEC ON
GO

DROP TABLE [dbo].[AccountTransactions]
GO

SET NOEXEC OFF
GO

CREATE TABLE [dbo].[AccountTransactions] (
[Id] int NOT NULL IDENTITY(1,1) ,
[TransactionIdentifier] varchar(50) NOT NULL ,
[SequenceNo] varchar(30) NOT NULL ,
[UserIdentifier] varchar(50) NOT NULL ,
[OrderIdentifier] varchar(50) NOT NULL ,
[BankCardNo] varchar(25) NOT NULL ,
[TradeCode] int NOT NULL ,
[Amount] bigint NOT NULL ,
[TransactionTime] datetime2(7) NOT NULL ,
[ChannelCode] int NOT NULL ,
[ResultCode] int NOT NULL ,
[ResultTime] datetime2(7) NULL ,
[TransDesc] nvarchar(200) NOT NULL ,
[UserInfo] nvarchar(MAX) NOT NULL ,
[Info] nvarchar(MAX) NOT NULL ,
[Args] nvarchar(MAX) NOT NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table AccountTransactions
-- ----------------------------
CREATE UNIQUE INDEX [IN_TransactionIdentifier] ON [dbo].[AccountTransactions]
([TransactionIdentifier] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE INDEX [IN_UserIdentifier] ON [dbo].[AccountTransactions]
([UserIdentifier] ASC) 
GO
CREATE INDEX [IN_UserIdentifier_TradeCode] ON [dbo].[AccountTransactions]
([UserIdentifier] ASC, [TradeCode] ASC) 
GO
CREATE INDEX [IN_UserIdentifier_ResultCode] ON [dbo].[AccountTransactions]
([UserIdentifier] ASC, [ResultCode] ASC) 
GO
CREATE INDEX [IN_TradeCode] ON [dbo].[AccountTransactions]
([TradeCode] ASC) 
GO
CREATE INDEX [IN_ResultCode] ON [dbo].[AccountTransactions]
([ResultCode] ASC) 
GO
CREATE INDEX [IN_UserIdentifier_TradeCode_ResultCode] ON [dbo].[AccountTransactions]
([UserIdentifier] ASC, [TradeCode] ASC, [ResultCode] ASC) 
GO
CREATE INDEX [IN_OrderIdentifier] ON [dbo].[AccountTransactions]
([OrderIdentifier] ASC) 
GO
CREATE INDEX [IN_BankCardNo] ON [dbo].[AccountTransactions]
([BankCardNo] ASC) 
GO
CREATE INDEX [IN_SequenceNo] ON [dbo].[AccountTransactions]
([SequenceNo] ASC) 
GO

-- ----------------------------
-- Table structure for BankCards
-- ----------------------------
IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE [name] = N'BankCards')
  SET NOEXEC ON
GO

DROP TABLE [dbo].[BankCards]
GO

SET NOEXEC OFF
GO

CREATE TABLE [dbo].[BankCards] (
[Id] int NOT NULL IDENTITY(1,1) ,
[UserIdentifier] varchar(50) NOT NULL ,
[BankCardNo] varchar(25) NOT NULL ,
[BankName] nvarchar(20) NOT NULL ,
[CityName] nvarchar(20) NOT NULL ,
[Display] bit NOT NULL ,
[Verified] bit NOT NULL ,
[VerifiedTime] datetime2(7) NULL ,
[Info] nvarchar(MAX) NOT NULL ,
[Args] nvarchar(MAX) NOT NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table BankCards
-- ----------------------------
CREATE INDEX [IN_UserIdentifier] ON [dbo].[BankCards]
([UserIdentifier] ASC) 
GO
CREATE INDEX [IN_BankCardNo] ON [dbo].[BankCards]
([BankCardNo] ASC) 
GO
CREATE INDEX [IN_Display] ON [dbo].[BankCards]
([Display] ASC) 
GO
CREATE INDEX [IN_BankCardNo_Display] ON [dbo].[BankCards]
([BankCardNo] ASC, [Display] ASC) 
GO
CREATE INDEX [IN_UserIdentifier_BankCardNo] ON [dbo].[BankCards]
([UserIdentifier] ASC, [BankCardNo] ASC) 
GO

-- ----------------------------
-- Table structure for JBYProducts
-- ----------------------------
IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE [name] = N'JBYProducts')
  SET NOEXEC ON
GO

DROP TABLE [dbo].[JBYProducts]
GO

SET NOEXEC OFF
GO

CREATE TABLE [dbo].[JBYProducts] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ProductIdentifier] varchar(50) NOT NULL ,
[ProductCategory] bigint NOT NULL ,
[ProductName] nvarchar(50) NOT NULL ,
[ProductNo] varchar(100) NOT NULL ,
[IssueNo] int NOT NULL ,
[FinancingSumAmount] bigint NOT NULL ,
[UnitPrice] bigint NOT NULL ,
[IssueTime] datetime2(7) NOT NULL ,
[StartSellTime] datetime2(7) NOT NULL ,
[EndSellTime] datetime2(7) NOT NULL ,
[SoldOut] bit NOT NULL ,
[SoldOutTime] datetime2(7) NULL ,
[ValueDateMode] int NOT NULL ,
[Info] nvarchar(MAX) NOT NULL ,
[Yield] int NOT NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table JBYProducts
-- ----------------------------
CREATE UNIQUE INDEX [IN_ProductIdentifier] ON [dbo].[JBYProducts]
([ProductIdentifier] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE INDEX [IN_ProductCategory] ON [dbo].[JBYProducts]
([ProductCategory] ASC) 
GO
CREATE INDEX [IN_ProductNo] ON [dbo].[JBYProducts]
([ProductNo] ASC) 
GO
CREATE INDEX [IN_IssueNo] ON [dbo].[JBYProducts]
([IssueNo] ASC) 
GO
CREATE INDEX [IN_IssueTime] ON [dbo].[JBYProducts]
([IssueTime] ASC) 
GO
CREATE INDEX [IN_SoldOut] ON [dbo].[JBYProducts]
([SoldOut] ASC) 
GO
CREATE INDEX [IN_ProductCategory_SoldOut] ON [dbo].[JBYProducts]
([ProductCategory] ASC, [SoldOut] ASC) 
GO

-- ----------------------------
-- Table structure for JBYTransactions
-- ----------------------------
IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE [name] = N'JBYTransactions')
  SET NOEXEC ON
GO

DROP TABLE [dbo].[JBYTransactions]
GO

SET NOEXEC OFF
GO

CREATE TABLE [dbo].[JBYTransactions] (
[Id] int NOT NULL IDENTITY(1,1) ,
[TransactionIdentifier] varchar(50) NOT NULL ,
[AccountTransactionIdentifier] varchar(50) NOT NULL ,
[JBYProductIdentifier] varchar(50) NOT NULL ,
[UserIdentifier] varchar(50) NOT NULL ,
[TradeCode] int NOT NULL ,
[Amount] bigint NOT NULL ,
[TransactionTime] datetime2(7) NOT NULL ,
[ResultCode] int NOT NULL ,
[ResultTime] datetime2(7) NULL ,
[TransDesc] nvarchar(200) NOT NULL ,
[UserInfo] nvarchar(MAX) NOT NULL ,
[Info] nvarchar(MAX) NOT NULL ,
[Args] nvarchar(MAX) NOT NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table JBYTransactions
-- ----------------------------
CREATE UNIQUE INDEX [IN_TransactionIdentifier] ON [dbo].[JBYTransactions]
([TransactionIdentifier] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE INDEX [IN_AccountTransactionIdentifier] ON [dbo].[JBYTransactions]
([AccountTransactionIdentifier] ASC) 
GO
CREATE INDEX [IN_JBYProductIdentifier] ON [dbo].[JBYTransactions]
([JBYProductIdentifier] ASC) 
GO
CREATE INDEX [IN_UserIdentifier] ON [dbo].[JBYTransactions]
([UserIdentifier] ASC) 
GO
CREATE INDEX [IN_TradeCode] ON [dbo].[JBYTransactions]
([TradeCode] ASC) 
GO
CREATE INDEX [IN_ResultCode] ON [dbo].[JBYTransactions]
([ResultCode] ASC) 
GO
CREATE INDEX [IN_UserIdentifier_TradeCode] ON [dbo].[JBYTransactions]
([UserIdentifier] ASC, [TradeCode] ASC) 
GO
CREATE INDEX [IN_UserIdentifier_ResultCode] ON [dbo].[JBYTransactions]
([UserIdentifier] ASC, [ResultCode] ASC) 
GO
CREATE INDEX [IN_UserIdentifier_TradeCode_ResultCode] ON [dbo].[JBYTransactions]
([UserIdentifier] ASC, [TradeCode] ASC, [ResultCode] ASC) 
GO

-- ----------------------------
-- Table structure for Orders
-- ----------------------------
IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE [name] = N'Orders')
  SET NOEXEC ON
GO

DROP TABLE [dbo].[Orders]
GO

SET NOEXEC OFF
GO

CREATE TABLE [dbo].[Orders] (
[Id] int NOT NULL IDENTITY(1,1) ,
[OrderIdentifier] varchar(50) NOT NULL ,
[AccountTransactionIdentifier] varchar(50) NOT NULL ,
[UserIdentifier] varchar(50) NOT NULL ,
[OrderTime] datetime2(7) NOT NULL ,
[OrderNo] varchar(20) NOT NULL ,
[ProductIdentifier] varchar(50) NOT NULL ,
[ProductCategory] bigint NOT NULL ,
[ProductSnapshot] nvarchar(MAX) NOT NULL ,
[Principal] bigint NOT NULL ,
[Yield] int NOT NULL ,
[ExtraYield] int NOT NULL ,
[Interest] bigint NOT NULL ,
[ExtraInterest] bigint NOT NULL ,
[ValueDate] datetime2(7) NOT NULL ,
[SettleDate] datetime2(7) NOT NULL ,
[ResultCode] int NOT NULL ,
[ResultTime] datetime2(7) NULL ,
[IsRepaid] bit NOT NULL ,
[RepaidTime] datetime2(7) NULL ,
[TransDesc] nvarchar(200) NOT NULL ,
[Cellphone] varchar(15) NOT NULL ,
[UserInfo] nvarchar(MAX) NOT NULL ,
[Info] nvarchar(MAX) NOT NULL ,
[Args] nvarchar(MAX) NOT NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table Orders
-- ----------------------------
CREATE UNIQUE INDEX [IN_OrderIdentifier] ON [dbo].[Orders]
([OrderIdentifier] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE INDEX [IN_AccountTransactionIdentifier] ON [dbo].[Orders]
([AccountTransactionIdentifier] ASC) 
GO
CREATE INDEX [IN_UserIdentifier] ON [dbo].[Orders]
([UserIdentifier] ASC) 
GO
CREATE INDEX [IN_OrderNo] ON [dbo].[Orders]
([OrderNo] ASC) 
GO
CREATE INDEX [IN_ProductIdentifier] ON [dbo].[Orders]
([ProductIdentifier] ASC) 
GO
CREATE INDEX [IN_ProductCategory] ON [dbo].[Orders]
([ProductCategory] ASC) 
GO
CREATE INDEX [IN_OrderTime] ON [dbo].[Orders]
([OrderTime] ASC) 
GO
CREATE INDEX [IN_ResultCode] ON [dbo].[Orders]
([ResultCode] ASC) 
GO
CREATE INDEX [IN_IsRepaid] ON [dbo].[Orders]
([IsRepaid] ASC) 
GO
CREATE INDEX [IN_Cellphone] ON [dbo].[Orders]
([Cellphone] ASC) 
GO
CREATE INDEX [IN_UserIdentifier_ProductIdentifier] ON [dbo].[Orders]
([UserIdentifier] ASC, [ProductIdentifier] ASC) 
GO
CREATE INDEX [IN_UserIdentifier_ResultCode] ON [dbo].[Orders]
([UserIdentifier] ASC, [ResultCode] ASC) 
GO
CREATE INDEX [IN_UserIdentifier_IsRepaid] ON [dbo].[Orders]
([UserIdentifier] ASC, [IsRepaid] ASC) 
GO
CREATE INDEX [IN_UserIdentifier_ResultCode_IsRepaid] ON [dbo].[Orders]
([UserIdentifier] ASC, [ResultCode] ASC, [IsRepaid] ASC) 
GO
CREATE INDEX [IN_UserIdentifier_ProductIdentifier_ResultCode] ON [dbo].[Orders]
([UserIdentifier] ASC, [ProductIdentifier] ASC, [ResultCode] ASC) 
GO
CREATE INDEX [IN_ProductIdentifier_ResultCode] ON [dbo].[Orders]
([ProductIdentifier] ASC, [ResultCode] ASC) 
GO
CREATE INDEX [IN_ProductIdentifier_IsRepaid] ON [dbo].[Orders]
([ProductIdentifier] ASC, [IsRepaid] ASC) 
GO
CREATE INDEX [IN_ProductIdentifier_ResultCode_IsRepaid] ON [dbo].[Orders]
([ProductIdentifier] ASC, [ResultCode] ASC, [IsRepaid] ASC) 
GO

-- ----------------------------
-- Table structure for RegularProducts
-- ----------------------------
IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE [name] = N'RegularProducts')
  SET NOEXEC ON
GO

DROP TABLE [dbo].[RegularProducts]
GO

SET NOEXEC OFF
GO

CREATE TABLE [dbo].[RegularProducts] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ProductIdentifier] varchar(50) NOT NULL ,
[ProductCategory] bigint NOT NULL ,
[ProductName] nvarchar(50) NOT NULL ,
[ProductNo] varchar(40) NOT NULL ,
[IssueNo] int NOT NULL ,
[PledgeNo] varchar(40) NOT NULL ,
[Yield] int NOT NULL ,
[FinancingSumAmount] bigint NOT NULL ,
[UnitPrice] bigint NOT NULL ,
[IssueTime] datetime2(7) NOT NULL ,
[StartSellTime] datetime2(7) NOT NULL ,
[EndSellTime] datetime2(7) NOT NULL ,
[SoldOut] bit NOT NULL ,
[SoldOutTime] datetime2(7) NULL ,
[ValueDate] datetime2(7) NULL ,
[ValueDateMode] int NULL ,
[SettleDate] datetime2(7) NOT NULL ,
[RepaymentDeadline] datetime2(7) NOT NULL ,
[Repaid] bit NOT NULL ,
[RepaidTime] datetime2(7) NULL ,
[Info] nvarchar(MAX) NOT NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table RegularProducts
-- ----------------------------
CREATE UNIQUE INDEX [IN_ProductIdentifier] ON [dbo].[RegularProducts]
([ProductIdentifier] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE INDEX [IN_ProductCategory] ON [dbo].[RegularProducts]
([ProductCategory] ASC) 
GO
CREATE INDEX [IN_ProductNo] ON [dbo].[RegularProducts]
([ProductNo] ASC) 
GO
CREATE INDEX [IN_IssueNo] ON [dbo].[RegularProducts]
([IssueNo] ASC) 
GO
CREATE INDEX [IN_IssueTime] ON [dbo].[RegularProducts]
([IssueTime] ASC) 
GO
CREATE INDEX [IN_SoldOut] ON [dbo].[RegularProducts]
([SoldOut] ASC) 
GO
CREATE INDEX [IN_ProductCategory_SoldOut] ON [dbo].[RegularProducts]
([ProductCategory] ASC, [SoldOut] ASC) 
GO
CREATE INDEX [IN_PledgeNo] ON [dbo].[RegularProducts]
([PledgeNo] ASC) 
GO
CREATE INDEX [IN_Repaid] ON [dbo].[RegularProducts]
([Repaid] ASC) 
GO
CREATE INDEX [IN_ProductCategory_Repaid] ON [dbo].[RegularProducts]
([ProductCategory] ASC, [Repaid] ASC) 
GO

-- ----------------------------
-- Table structure for Users
-- ----------------------------
IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE [name] = N'Users')
  SET NOEXEC ON
GO

DROP TABLE [dbo].[Users]
GO

SET NOEXEC OFF
GO

CREATE TABLE [dbo].[Users] (
[Id] int NOT NULL IDENTITY(1,1) ,
[UserIdentifier] varchar(50) NOT NULL ,
[Cellphone] varchar(20) NOT NULL ,
[RegisterTime] datetime2(7) NOT NULL ,
[LoginNames] varchar(200) NOT NULL ,
[RealName] nvarchar(20) NOT NULL ,
[Credential] int NOT NULL ,
[CredentialNo] varchar(50) NOT NULL ,
[Verified] bit NOT NULL ,
[VerifiedTime] datetime2(7) NULL ,
[OutletCode] varchar(50) NOT NULL ,
[ClientType] bigint NOT NULL ,
[InviteBy] nvarchar(50) NOT NULL ,
[ContractId] bigint NOT NULL ,
[Closed] bit NOT NULL ,
[Info] nvarchar(MAX) NOT NULL ,
[Args] nvarchar(MAX) NOT NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table Users
-- ----------------------------
CREATE UNIQUE INDEX [IN_UserIdentifier] ON [dbo].[Users]
([UserIdentifier] ASC) 
WITH (IGNORE_DUP_KEY = ON)
GO
CREATE INDEX [IN_Cellphone] ON [dbo].[Users]
([Cellphone] ASC) 
GO
CREATE INDEX [IN_CredentialNo] ON [dbo].[Users]
([CredentialNo] ASC) 
GO
CREATE INDEX [IN_Verified] ON [dbo].[Users]
([Verified] ASC) 
GO
CREATE INDEX [IN_ContractId] ON [dbo].[Users]
([ContractId] ASC) 
GO
CREATE INDEX [IN_ClientType] ON [dbo].[Users]
([ClientType] ASC) 
GO

-- ----------------------------
-- Table structure for VeriCodes
-- ----------------------------
IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE [name] = N'VeriCodes')
  SET NOEXEC ON
GO

DROP TABLE [dbo].[VeriCodes]
GO

SET NOEXEC OFF
GO

CREATE TABLE [dbo].[VeriCodes] (
[Id] int NOT NULL IDENTITY(1,1) ,
[BuildAt] datetime2(7) NOT NULL ,
[Cellphone] varchar(20) NOT NULL ,
[Code] varchar(200) NOT NULL ,
[ErrorCount] int NOT NULL ,
[Times] int NOT NULL ,
[Token] varchar(32) NOT NULL ,
[Type] int NOT NULL ,
[Used] bit NOT NULL ,
[Verified] bit NOT NULL ,
[Args] nvarchar(MAX) NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table VeriCodes
-- ----------------------------
CREATE INDEX [IN_Cellphone] ON [dbo].[VeriCodes]
([Cellphone] ASC) 
GO
CREATE INDEX [IN_Type] ON [dbo].[VeriCodes]
([Type] ASC) 
GO
CREATE INDEX [IN_Cellphone_Type] ON [dbo].[VeriCodes]
([Cellphone] ASC, [Type] ASC) 
GO
CREATE INDEX [IN_Used] ON [dbo].[VeriCodes]
([Used] ASC) 
GO
CREATE INDEX [IN_Verified] ON [dbo].[VeriCodes]
([Verified] ASC) 
GO
CREATE INDEX [IN_Cellphone_Type_Verified] ON [dbo].[VeriCodes]
([Cellphone] ASC, [Type] ASC, [Verified] ASC) 
GO

IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE [name] = N'PrincipalVolumes')
  SET NOEXEC ON
GO

DROP TABLE [dbo].[PrincipalVolumes]
GO

SET NOEXEC OFF
GO

CREATE TABLE [dbo].[PrincipalVolumes] (
[Id] int NOT NULL IDENTITY(1,1) ,
[UserIdentifier] varchar(50) COLLATE Chinese_PRC_CI_AS NOT NULL ,
[Amount] decimal(18,6) NOT NULL ,
[AddDate] datetime NOT NULL ,
[EffectiveStartDate] datetime NOT NULL ,
[EffectiveEndDate] datetime NOT NULL ,
[UseFlag] int NOT NULL ,
[UseTime] datetime NULL ,
[OrderNo] varchar(20) COLLATE Chinese_PRC_CI_AS NULL ,
[Remark] nvarchar(500) COLLATE Chinese_PRC_CI_AS NULL ,
CONSTRAINT [PK_PRINCIPALVOLUMES] PRIMARY KEY ([Id])
)
ON [PRIMARY]
GO
