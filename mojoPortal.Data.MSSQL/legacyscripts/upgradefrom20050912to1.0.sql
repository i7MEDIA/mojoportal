
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
CREATE TABLE [dbo].[mp_ForumSubscriptions] (
	[SubscriptionID] [int] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[ForumID] [int] NOT NULL ,
	[UserID] [int] NOT NULL ,
	[SubscribeDate] [datetime] NOT NULL ,
	[UnSubscribeDate] [datetime] NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[mp_ForumSubscriptions] WITH NOCHECK ADD 
	CONSTRAINT [PK_mp_ForumSubscriptions] PRIMARY KEY  CLUSTERED 
	(
		[SubscriptionID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[mp_ForumSubscriptions] ADD 
	CONSTRAINT [DF_mp_ForumSubscriptions_SubscribeDate] DEFAULT (getdate()) FOR [SubscribeDate]
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
ALTER TABLE dbo.mp_Sites ADD
	UseLdapAuth bit NOT NULL CONSTRAINT DF_mp_Sites_UseLdapAuth DEFAULT 0,
	AutoCreateLdapUserOnFirstLogin bit NOT NULL CONSTRAINT DF_mp_Sites_AutoCreateLdapUserOnFirstLogin DEFAULT 1,
	LdapServer nvarchar(255) NULL,
	LdapPort int NOT NULL CONSTRAINT DF_mp_Sites_LdapPort DEFAULT 389,
	LdapRootDN nvarchar(255) NULL,
	ReallyDeleteUsers bit NOT NULL CONSTRAINT DF_mp_Sites_ReallyDeleteUsers DEFAULT 1,
	UseEmailForLogin bit NOT NULL CONSTRAINT DF_mp_Sites_UseEmailForLogin DEFAULT 1,
	AllowUserFullNameChange bit NOT NULL CONSTRAINT DF_mp_Sites_AllowUserFullNameChange DEFAULT 0,
	EditorSkin nvarchar(50) NOT NULL CONSTRAINT DF_mp_Sites_EditorSkin DEFAULT 'normal',
	DefaultFriendlyUrlPatternEnum nvarchar(50) NOT NULL CONSTRAINT DF_mp_Sites_DefaultFriendlyUrlPatternEnum DEFAULT 'PageNameWithDotASPX'
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
ALTER TABLE dbo.mp_Users
	DROP CONSTRAINT DF_Users_ProfileApproved
GO
ALTER TABLE dbo.mp_Users
	DROP CONSTRAINT DF_Users_Approved
GO
ALTER TABLE dbo.mp_Users
	DROP CONSTRAINT DF_Users_Trusted
GO
ALTER TABLE dbo.mp_Users
	DROP CONSTRAINT DF_mp_Users_DisplayInMemberList
GO
ALTER TABLE dbo.mp_Users
	DROP CONSTRAINT DF_Users_TotalPosts
GO
ALTER TABLE dbo.mp_Users
	DROP CONSTRAINT DF_mp_Users_AvatarUrl
GO
ALTER TABLE dbo.mp_Users
	DROP CONSTRAINT DF_mp_Users_TimeOffSetHours
GO
ALTER TABLE dbo.mp_Users
	DROP CONSTRAINT DF_Users_DateCreated
GO
ALTER TABLE dbo.mp_Users
	DROP CONSTRAINT DF_mp_Users_UserGuid
GO
CREATE TABLE dbo.Tmp_mp_Users
	(
	UserID int NOT NULL IDENTITY (1, 1),
	SiteID int NOT NULL,
	Name nvarchar(100) NOT NULL,
	LoginName nvarchar(50) NULL,
	Email nvarchar(100) NOT NULL,
	Password nvarchar(50) NOT NULL,
	Gender nchar(10) NULL,
	ProfileApproved bit NOT NULL,
	ApprovedForForums bit NOT NULL,
	Trusted bit NOT NULL,
	DisplayInMemberList bit NULL,
	WebSiteURL nvarchar(100) NULL,
	Country nvarchar(100) NULL,
	State nvarchar(100) NULL,
	Occupation nvarchar(100) NULL,
	Interests nvarchar(100) NULL,
	MSN nvarchar(50) NULL,
	Yahoo nvarchar(50) NULL,
	AIM nvarchar(50) NULL,
	ICQ nvarchar(50) NULL,
	TotalPosts int NOT NULL,
	AvatarUrl nvarchar(255) NULL,
	TimeOffsetHours int NOT NULL,
	Signature nvarchar(255) NULL,
	DateCreated datetime NOT NULL,
	UserGuid nchar(36) NULL,
	Skin nvarchar(100) NULL,
	IsDeleted bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_mp_Users ADD CONSTRAINT
	DF_Users_ProfileApproved DEFAULT (1) FOR ProfileApproved
GO
ALTER TABLE dbo.Tmp_mp_Users ADD CONSTRAINT
	DF_Users_Approved DEFAULT (1) FOR ApprovedForForums
GO
ALTER TABLE dbo.Tmp_mp_Users ADD CONSTRAINT
	DF_Users_Trusted DEFAULT (0) FOR Trusted
GO
ALTER TABLE dbo.Tmp_mp_Users ADD CONSTRAINT
	DF_mp_Users_DisplayInMemberList DEFAULT (1) FOR DisplayInMemberList
GO
ALTER TABLE dbo.Tmp_mp_Users ADD CONSTRAINT
	DF_Users_TotalPosts DEFAULT (0) FOR TotalPosts
GO
ALTER TABLE dbo.Tmp_mp_Users ADD CONSTRAINT
	DF_mp_Users_AvatarUrl DEFAULT ('blank.gif') FOR AvatarUrl
GO
ALTER TABLE dbo.Tmp_mp_Users ADD CONSTRAINT
	DF_mp_Users_TimeOffSetHours DEFAULT (0) FOR TimeOffsetHours
GO
ALTER TABLE dbo.Tmp_mp_Users ADD CONSTRAINT
	DF_Users_DateCreated DEFAULT (getdate()) FOR DateCreated
GO
ALTER TABLE dbo.Tmp_mp_Users ADD CONSTRAINT
	DF_mp_Users_UserGuid DEFAULT (newid()) FOR UserGuid
GO
ALTER TABLE dbo.Tmp_mp_Users ADD CONSTRAINT
	DF_mp_Users_IsDeleted DEFAULT 0 FOR IsDeleted
GO
SET IDENTITY_INSERT dbo.Tmp_mp_Users ON
GO
IF EXISTS(SELECT * FROM dbo.mp_Users)
	 EXEC('INSERT INTO dbo.Tmp_mp_Users (UserID, SiteID, Name, LoginName, Email, Password, Gender, ProfileApproved, ApprovedForForums, Trusted, DisplayInMemberList, WebSiteURL, Country, State, Occupation, Interests, MSN, Yahoo, AIM, ICQ, TotalPosts, AvatarUrl, TimeOffsetHours, Signature, DateCreated, UserGuid, Skin)
		SELECT UserID, SiteID, Name, Name, Email, Password, Gender, ProfileApproved, ApprovedForForums, Trusted, DisplayInMemberList, WebSiteURL, Country, State, Occupation, Interests, MSN, Yahoo, AIM, ICQ, TotalPosts, AvatarUrl, TimeOffsetHours, Signature, DateCreated, UserGuid, Skin FROM dbo.mp_Users TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_mp_Users OFF
GO
DROP TABLE dbo.mp_Users
GO
EXECUTE sp_rename N'dbo.Tmp_mp_Users', N'mp_Users', 'OBJECT'
GO
ALTER TABLE dbo.mp_Users ADD CONSTRAINT
	PK_mp_Users PRIMARY KEY CLUSTERED 
	(
	UserID
	) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX idxUserName ON dbo.mp_Users
	(
	Name
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX idxUserEmail ON dbo.mp_Users
	(
	Email
	) ON [PRIMARY]
GO
COMMIT

INSERT INTO mp_ModuleDefinitions (FeatureName, ControlSrc, SortOrder, IsAdmin)

VALUES ('Html Fragment Include','Modules/HtmlFragmentInclude.ascx',500,0)


