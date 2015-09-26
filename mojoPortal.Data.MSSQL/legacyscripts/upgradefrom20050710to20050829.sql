
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.mp_Links
	DROP CONSTRAINT FK_Links_Modules
GO
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.Tmp_mp_Links
	(
	ItemID int NOT NULL IDENTITY (0, 1),
	ModuleID int NOT NULL,
	Title nvarchar(255) NULL,
	Url nvarchar(255) NULL,
	Target nvarchar(20) NOT NULL,
	ViewOrder int NULL,
	Description ntext NULL,
	CreatedDate datetime NULL,
	CreatedBy int NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_mp_Links ADD CONSTRAINT
	DF_mp_Links_Target DEFAULT '_blank' FOR Target
GO
SET IDENTITY_INSERT dbo.Tmp_mp_Links ON
GO
IF EXISTS(SELECT * FROM dbo.mp_Links)
	 EXEC('INSERT INTO dbo.Tmp_mp_Links (ItemID, ModuleID, Title, Url, ViewOrder, Description, CreatedDate, CreatedBy)
		SELECT ItemID, ModuleID, Title, Url, ViewOrder, Description, CreatedDate, CreatedBy FROM dbo.mp_Links TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_mp_Links OFF
GO
DROP TABLE dbo.mp_Links
GO
EXECUTE sp_rename N'dbo.Tmp_mp_Links', N'mp_Links', 'OBJECT'
GO
ALTER TABLE dbo.mp_Links ADD CONSTRAINT
	PK_Links PRIMARY KEY NONCLUSTERED 
	(
	ItemID
	) ON [PRIMARY]

GO
ALTER TABLE dbo.mp_Links WITH NOCHECK ADD CONSTRAINT
	FK_Links_Modules FOREIGN KEY
	(
	ModuleID
	) REFERENCES dbo.mp_Modules
	(
	ModuleID
	) ON DELETE CASCADE
	 NOT FOR REPLICATION

GO
COMMIT




INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (2,'LinksShowDeleteIconSetting','false','CheckBox',NULL)
