-- for stored procedures you can run the latest script 
-- to replace yours with the current version 
-- after running this script to modify tables/data

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
ALTER TABLE dbo.mp_Modules ADD
	EditUserID int NOT NULL CONSTRAINT DF_mp_Modules_EditUserID DEFAULT 0
GO
COMMIT



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
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_AllowUserSkins
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_AllowNewRegistration
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_UseSecureRegistration
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_EncryptPasswords
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_UseSSLOnAllPages
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_IsServerAdminSite
GO
CREATE TABLE dbo.Tmp_mp_Sites
	(
	SiteID int NOT NULL IDENTITY (-1, 1),
	SiteAlias nvarchar(50) NULL,
	SiteName nvarchar(255) NOT NULL,
	Skin nvarchar(100) NULL,
	Logo nvarchar(50) NULL,
	Icon nvarchar(50) NULL,
	AllowUserSkins bit NOT NULL,
	AllowPageSkins bit NULL,
	AllowHideMenuOnPages bit NULL,
	AllowNewRegistration bit NOT NULL,
	UseSecureRegistration bit NOT NULL,
	EncryptPasswords bit NOT NULL,
	UseSSLOnAllPages bit NOT NULL,
	DefaultPageKeyWords nvarchar(255) NULL,
	DefaultPageDescription nvarchar(255) NULL,
	DefaultPageEncoding nvarchar(255) NULL,
	DefaultAdditionalMetaTags nvarchar(255) NULL,
	IsServerAdminSite bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_AllowUserSkins DEFAULT (0) FOR AllowUserSkins
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_AllowPageSkins DEFAULT 1 FOR AllowPageSkins
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_AllowHideMenuOnPages DEFAULT 1 FOR AllowHideMenuOnPages
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_AllowNewRegistration DEFAULT (1) FOR AllowNewRegistration
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_UseSecureRegistration DEFAULT (0) FOR UseSecureRegistration
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_EncryptPasswords DEFAULT (0) FOR EncryptPasswords
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_UseSSLOnAllPages DEFAULT (0) FOR UseSSLOnAllPages
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_IsServerAdminSite DEFAULT (0) FOR IsServerAdminSite
GO
SET IDENTITY_INSERT dbo.Tmp_mp_Sites ON
GO
IF EXISTS(SELECT * FROM dbo.mp_Sites)
	 EXEC('INSERT INTO dbo.Tmp_mp_Sites (SiteID, SiteAlias, SiteName, Skin, Logo, Icon, AllowUserSkins, AllowNewRegistration, UseSecureRegistration, EncryptPasswords, UseSSLOnAllPages, DefaultPageKeyWords, DefaultPageDescription, DefaultPageEncoding, DefaultAdditionalMetaTags, IsServerAdminSite)
		SELECT SiteID, SiteAlias, SiteName, Skin, Logo, Icon, AllowUserSkins, AllowNewRegistration, UseSecureRegistration, EncryptPasswords, UseSSLOnAllPages, DefaultPageKeyWords, DefaultPageDescription, DefaultPageEncoding, DefaultAdditionalMetaTags, IsServerAdminSite FROM dbo.mp_Sites TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_mp_Sites OFF
GO
ALTER TABLE dbo.mp_Roles
	DROP CONSTRAINT FK_Roles_Portals
GO
ALTER TABLE dbo.mp_Pages
	DROP CONSTRAINT FK_Tabs_Portals
GO
DROP TABLE dbo.mp_Sites
GO
EXECUTE sp_rename N'dbo.Tmp_mp_Sites', N'mp_Sites', 'OBJECT'
GO
ALTER TABLE dbo.mp_Sites ADD CONSTRAINT
	PK_Portals PRIMARY KEY NONCLUSTERED 
	(
	SiteID
	) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.mp_Pages WITH NOCHECK ADD CONSTRAINT
	FK_Tabs_Portals FOREIGN KEY
	(
	SiteID
	) REFERENCES dbo.mp_Sites
	(
	SiteID
	) ON DELETE CASCADE
	 NOT FOR REPLICATION

GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.mp_Roles WITH NOCHECK ADD CONSTRAINT
	FK_Roles_Portals FOREIGN KEY
	(
	SiteID
	) REFERENCES dbo.mp_Sites
	(
	SiteID
	) ON DELETE CASCADE
	 NOT FOR REPLICATION

GO
COMMIT

---

ALTER TABLE [dbo].[mp_Roles]
	ADD [DisplayName] nvarchar(50) NULL
	
	
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
ALTER TABLE dbo.mp_Pages
	DROP CONSTRAINT FK_Tabs_Portals
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.mp_Pages
	DROP CONSTRAINT DF_mp_Pages_ParentID
GO
ALTER TABLE dbo.mp_Pages
	DROP CONSTRAINT DF_mp_Pages_RequireSSL
GO
ALTER TABLE dbo.mp_Pages
	DROP CONSTRAINT DF_mp_Pages_ShowBreadcrumbs
GO
ALTER TABLE dbo.mp_Pages
	DROP CONSTRAINT DF_mp_Pages_UseLinkInsteadOfPage


GO
ALTER TABLE dbo.mp_Pages
	DROP CONSTRAINT DF_mp_Pages_OpenInNewWindow
GO
ALTER TABLE dbo.mp_Pages
	DROP CONSTRAINT DF_mp_Pages_ShowChildPageMenu
GO
CREATE TABLE dbo.Tmp_mp_Pages
	(
	PageID int NOT NULL IDENTITY (0, 1),
	ParentID int NULL,
	PageOrder int NOT NULL,
	SiteID int NOT NULL,
	PageName nvarchar(50) NOT NULL,
	AuthorizedRoles ntext NULL,
	EditRoles ntext NULL,
	CreateChildPageRoles ntext NULL,
	RequireSSL bit NOT NULL,
	ShowBreadcrumbs bit NOT NULL,
	PageKeyWords nvarchar(255) NULL,
	PageDescription nvarchar(255) NULL,
	PageEncoding nvarchar(255) NULL,
	AdditionalMetaTags nvarchar(255) NULL,
	MenuImage nvarchar(50) NULL,
	UseUrl bit NOT NULL,
	Url nvarchar(255) NULL,
	OpenInNewWindow bit NOT NULL,
	ShowChildPageMenu bit NOT NULL,
	ShowChildBreadCrumbs bit NOT NULL,
	HideMainMenu bit NOT NULL,
	Skin nvarchar(100) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_mp_Pages ADD CONSTRAINT
	DF_mp_Pages_ParentID DEFAULT ((-1)) FOR ParentID
GO
ALTER TABLE dbo.Tmp_mp_Pages ADD CONSTRAINT
	DF_mp_Pages_RequireSSL DEFAULT (0) FOR RequireSSL
GO
ALTER TABLE dbo.Tmp_mp_Pages ADD CONSTRAINT
	DF_mp_Pages_ShowBreadcrumbs DEFAULT (0) FOR ShowBreadcrumbs
GO
ALTER TABLE dbo.Tmp_mp_Pages ADD CONSTRAINT
	DF_mp_Pages_UseLinkInsteadOfPage DEFAULT (0) FOR UseUrl
GO
ALTER TABLE dbo.Tmp_mp_Pages ADD CONSTRAINT
	DF_mp_Pages_OpenInNewWindow DEFAULT (0) FOR OpenInNewWindow
GO
ALTER TABLE dbo.Tmp_mp_Pages ADD CONSTRAINT
	DF_mp_Pages_ShowChildPageMenu DEFAULT (0) FOR ShowChildPageMenu
GO
ALTER TABLE dbo.Tmp_mp_Pages ADD CONSTRAINT
	DF_mp_Pages_ShowChildBreadCrumbs DEFAULT 0 FOR ShowChildBreadCrumbs
GO

ALTER TABLE dbo.Tmp_mp_Pages ADD CONSTRAINT
	DF_mp_Pages_HideMainMenu DEFAULT (0) FOR HideMainMenu
GO

SET IDENTITY_INSERT dbo.Tmp_mp_Pages ON
GO
IF EXISTS(SELECT * FROM dbo.mp_Pages)
	 EXEC('INSERT INTO dbo.Tmp_mp_Pages (PageID, ParentID, PageOrder, SiteID, PageName, AuthorizedRoles, RequireSSL, ShowBreadcrumbs, PageKeyWords, PageDescription, PageEncoding, AdditionalMetaTags, MenuImage, UseUrl, Url, OpenInNewWindow, ShowChildPageMenu)
		SELECT PageID, ParentID, PageOrder, SiteID, PageName, CONVERT(ntext, AuthorizedRoles), RequireSSL, ShowBreadcrumbs, PageKeyWords, PageDescription, PageEncoding, AdditionalMetaTags, MenuImage, UseUrl, Url, OpenInNewWindow, ShowChildPageMenu FROM dbo.mp_Pages TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_mp_Pages OFF
GO
ALTER TABLE dbo.mp_Modules
	DROP CONSTRAINT FK_Modules_Tabs
GO
DROP TABLE dbo.mp_Pages
GO
EXECUTE sp_rename N'dbo.Tmp_mp_Pages', N'mp_Pages', 'OBJECT'
GO
ALTER TABLE dbo.mp_Pages ADD CONSTRAINT
	PK_Tabs PRIMARY KEY NONCLUSTERED 
	(
	PageID
	) ON [PRIMARY]

GO
ALTER TABLE dbo.mp_Pages WITH NOCHECK ADD CONSTRAINT
	FK_Tabs_Portals FOREIGN KEY
	(
	SiteID
	) REFERENCES dbo.mp_Sites
	(
	SiteID
	) ON DELETE CASCADE
	 NOT FOR REPLICATION

GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.mp_Modules WITH NOCHECK ADD CONSTRAINT
	FK_Modules_Tabs FOREIGN KEY
	(
	PageID
	) REFERENCES dbo.mp_Pages
	(
	PageID
	) ON DELETE CASCADE
	 NOT FOR REPLICATION

GO
COMMIT


------------------------------------------------------

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
ALTER TABLE dbo.mp_Modules
	DROP CONSTRAINT FK_Modules_ModuleDefinitions
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.mp_Modules
	DROP CONSTRAINT FK_Modules_Tabs
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.mp_Modules
	DROP CONSTRAINT DF_mp_Modules_ShowTitle
GO
CREATE TABLE dbo.Tmp_mp_Modules
	(
	ModuleID int NOT NULL IDENTITY (0, 1),
	PageID int NOT NULL,
	ModuleDefID int NOT NULL,
	ModuleOrder int NOT NULL,
	PaneName nvarchar(50) NOT NULL,
	ModuleTitle nvarchar(255) NULL,
	AuthorizedEditRoles ntext NULL,
	CacheTime int NOT NULL,
	ShowTitle bit NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_mp_Modules ADD CONSTRAINT
	DF_mp_Modules_ShowTitle DEFAULT (1) FOR ShowTitle
GO
SET IDENTITY_INSERT dbo.Tmp_mp_Modules ON
GO
IF EXISTS(SELECT * FROM dbo.mp_Modules)
	 EXEC('INSERT INTO dbo.Tmp_mp_Modules (ModuleID, PageID, ModuleDefID, ModuleOrder, PaneName, ModuleTitle, AuthorizedEditRoles, CacheTime, ShowTitle)
		SELECT ModuleID, PageID, ModuleDefID, ModuleOrder, PaneName, ModuleTitle, CONVERT(ntext, AuthorizedEditRoles), CacheTime, ShowTitle FROM dbo.mp_Modules TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_mp_Modules OFF
GO
ALTER TABLE dbo.mp_HtmlContent
	DROP CONSTRAINT FK_HtmlText_Modules
GO
ALTER TABLE dbo.mp_Links
	DROP CONSTRAINT FK_Links_Modules
GO
ALTER TABLE dbo.mp_ModuleSettings
	DROP CONSTRAINT FK_ModuleSettings_Modules
GO
DROP TABLE dbo.mp_Modules
GO
EXECUTE sp_rename N'dbo.Tmp_mp_Modules', N'mp_Modules', 'OBJECT'
GO
ALTER TABLE dbo.mp_Modules ADD CONSTRAINT
	PK_Modules PRIMARY KEY NONCLUSTERED 
	(
	ModuleID
	) ON [PRIMARY]

GO
ALTER TABLE dbo.mp_Modules WITH NOCHECK ADD CONSTRAINT
	FK_Modules_Tabs FOREIGN KEY
	(
	PageID
	) REFERENCES dbo.mp_Pages
	(
	PageID
	) ON DELETE CASCADE
	 NOT FOR REPLICATION

GO
ALTER TABLE dbo.mp_Modules WITH NOCHECK ADD CONSTRAINT
	FK_Modules_ModuleDefinitions FOREIGN KEY
	(
	ModuleDefID
	) REFERENCES dbo.mp_ModuleDefinitions
	(
	ModuleDefID
	) ON DELETE CASCADE
	 NOT FOR REPLICATION

GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.mp_ModuleSettings WITH NOCHECK ADD CONSTRAINT
	FK_ModuleSettings_Modules FOREIGN KEY
	(
	ModuleID
	) REFERENCES dbo.mp_Modules
	(
	ModuleID
	) ON DELETE CASCADE
	 NOT FOR REPLICATION

GO
COMMIT
BEGIN TRANSACTION
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
BEGIN TRANSACTION
ALTER TABLE dbo.mp_HtmlContent WITH NOCHECK ADD CONSTRAINT
	FK_HtmlText_Modules FOREIGN KEY
	(
	ModuleID
	) REFERENCES dbo.mp_Modules
	(
	ModuleID
	) ON DELETE CASCADE
	 NOT FOR REPLICATION

GO
COMMIT






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
ALTER TABLE dbo.mp_Blogs ADD
	IncludeInFeed bit NOT NULL CONSTRAINT DF_mp_Blogs_IncludeInFeed DEFAULT 1
GO
COMMIT

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
ALTER TABLE dbo.mp_Users ADD
	Skin nvarchar(100) NULL
GO
COMMIT


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
ALTER TABLE dbo.mp_SiteModuleDefinitions ADD
	AuthorizedRoles ntext NULL
GO
COMMIT



INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (9,'BlogEditorWidthSetting','600','TextBox','^[1-9][0-9]{0,4}$')

INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (9,'BlogEditorHeightSetting','350','TextBox','^[1-9][0-9]{0,4}$')

INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (1,'HtmlEditorWidthSetting','600','TextBox','^[1-9][0-9]{0,4}$')

INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (1,'HtmlEditorHeightSetting','350','TextBox','^[1-9][0-9]{0,4}$')

INSERT INTO mp_Roles (SiteID, RoleName, DisplayName)
VALUES (1,'Content Administrators', 'Content Administrators')

GO

UPDATE mp_Roles
SET DisplayName = RoleName

GO


UPDATE mp_Roles
SET RoleName = 'Content Publishers',
DisplayName = 'Content Publishers'
WHERE RoleName = 'Content Approvers'

GO

INSERT INTO mp_Roles (SiteID, RoleName, DisplayName)
SELECT SiteID, 'Content Administrators', 'Content Administrators'
FROM mp_Sites

GO
