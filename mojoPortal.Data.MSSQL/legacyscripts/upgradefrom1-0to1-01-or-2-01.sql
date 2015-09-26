
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
	DROP CONSTRAINT DF_mp_Sites_AllowPageSkins
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_AllowHideMenuOnPages
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
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_UseLdapAuth
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_AutoCreateLdapUserOnFirstLogin
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_LdapPort
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_ReallyDeleteUsers
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_UseEmailForLogin
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_AllowUserFullNameChange
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_EditorSkin
GO
ALTER TABLE dbo.mp_Sites
	DROP CONSTRAINT DF_mp_Sites_DefaultFriendlyUrlPatternEnum
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
	IsServerAdminSite bit NOT NULL,
	UseLdapAuth bit NOT NULL,
	AutoCreateLdapUserOnFirstLogin bit NOT NULL,
	LdapServer nvarchar(255) NULL,
	LdapPort int NOT NULL,
	LdapRootDN nvarchar(255) NULL,
	LdapUserDNKey nvarchar(10) NOT NULL,
	ReallyDeleteUsers bit NOT NULL,
	UseEmailForLogin bit NOT NULL,
	AllowUserFullNameChange bit NOT NULL,
	EditorSkin nvarchar(50) NOT NULL,
	DefaultFriendlyUrlPatternEnum nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_AllowUserSkins DEFAULT (0) FOR AllowUserSkins
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_AllowPageSkins DEFAULT (1) FOR AllowPageSkins
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_AllowHideMenuOnPages DEFAULT (1) FOR AllowHideMenuOnPages
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
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_UseLdapAuth DEFAULT (0) FOR UseLdapAuth
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_AutoCreateLdapUserOnFirstLogin DEFAULT (1) FOR AutoCreateLdapUserOnFirstLogin
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_LdapPort DEFAULT (389) FOR LdapPort
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_LdapUserDNKey DEFAULT 'uid' FOR LdapUserDNKey
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_ReallyDeleteUsers DEFAULT (1) FOR ReallyDeleteUsers
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_UseEmailForLogin DEFAULT (1) FOR UseEmailForLogin
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_AllowUserFullNameChange DEFAULT (0) FOR AllowUserFullNameChange
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_EditorSkin DEFAULT ('normal') FOR EditorSkin
GO
ALTER TABLE dbo.Tmp_mp_Sites ADD CONSTRAINT
	DF_mp_Sites_DefaultFriendlyUrlPatternEnum DEFAULT ('PageNameWithDotASPX') FOR DefaultFriendlyUrlPatternEnum
GO
SET IDENTITY_INSERT dbo.Tmp_mp_Sites ON
GO
IF EXISTS(SELECT * FROM dbo.mp_Sites)
	 EXEC('INSERT INTO dbo.Tmp_mp_Sites (SiteID, SiteAlias, SiteName, Skin, Logo, Icon, AllowUserSkins, AllowPageSkins, AllowHideMenuOnPages, AllowNewRegistration, UseSecureRegistration, EncryptPasswords, UseSSLOnAllPages, DefaultPageKeyWords, DefaultPageDescription, DefaultPageEncoding, DefaultAdditionalMetaTags, IsServerAdminSite, UseLdapAuth, AutoCreateLdapUserOnFirstLogin, LdapServer, LdapPort, LdapRootDN, ReallyDeleteUsers, UseEmailForLogin, AllowUserFullNameChange, EditorSkin, DefaultFriendlyUrlPatternEnum)
		SELECT SiteID, SiteAlias, SiteName, Skin, Logo, Icon, AllowUserSkins, AllowPageSkins, AllowHideMenuOnPages, AllowNewRegistration, UseSecureRegistration, EncryptPasswords, UseSSLOnAllPages, DefaultPageKeyWords, DefaultPageDescription, DefaultPageEncoding, DefaultAdditionalMetaTags, IsServerAdminSite, UseLdapAuth, AutoCreateLdapUserOnFirstLogin, LdapServer, LdapPort, LdapRootDN, ReallyDeleteUsers, UseEmailForLogin, AllowUserFullNameChange, EditorSkin, DefaultFriendlyUrlPatternEnum FROM dbo.mp_Sites TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_mp_Sites OFF
GO
ALTER TABLE dbo.mp_Pages
	DROP CONSTRAINT FK_Tabs_Portals
GO
ALTER TABLE dbo.mp_Roles
	DROP CONSTRAINT FK_Roles_Portals
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


