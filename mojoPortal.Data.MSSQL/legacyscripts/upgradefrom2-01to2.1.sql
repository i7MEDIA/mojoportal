/*
Script created by SQL Compare version 5.0.0.1622 from Red Gate Software Ltd at 12/8/2006 10:40:12 AM
Run this script on (local).MeppelPortal to make it the same as (local).mojo21jan
Please back up your database before running this script
*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO

PRINT N'Creating [dbo].[mp_PageModules]'
GO
CREATE TABLE [dbo].[mp_PageModules]
(
[PageID] [int] NOT NULL,
[ModuleID] [int] NOT NULL,
[PaneName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ModuleOrder] [int] NOT NULL CONSTRAINT [DF_mp_PageModules_ModuleOrder] DEFAULT ((3)),
[PublishBeginDate] [datetime] NOT NULL CONSTRAINT [DF_mp_PageModules_PublishBeginDate] DEFAULT (getdate()),
[PublishEndDate] [datetime] NULL
)

INSERT INTO mp_PageModules (PageID, ModuleID, PaneName, ModuleOrder, PublishBeginDate )

SELECT PageID, ModuleID, PaneName, ModuleOrder, GetDate()
FROM mp_Modules

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO



PRINT N'Dropping foreign keys from [dbo].[mp_HtmlContent]'
GO
ALTER TABLE [dbo].[mp_HtmlContent] DROP
CONSTRAINT [FK_HtmlText_Modules]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[mp_Links]'
GO
ALTER TABLE [dbo].[mp_Links] DROP
CONSTRAINT [FK_Links_Modules]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[mp_ModuleSettings]'
GO
ALTER TABLE [dbo].[mp_ModuleSettings] DROP
CONSTRAINT [FK_ModuleSettings_Modules]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[mp_Modules]'
GO
ALTER TABLE [dbo].[mp_Modules] DROP
CONSTRAINT [FK_Modules_Tabs],
CONSTRAINT [FK_Modules_ModuleDefinitions]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[mp_Pages]'
GO
ALTER TABLE [dbo].[mp_Pages] DROP
CONSTRAINT [FK_Tabs_Portals]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[mp_Roles]'
GO
ALTER TABLE [dbo].[mp_Roles] DROP
CONSTRAINT [FK_Roles_Portals]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Modules]'
GO
ALTER TABLE [dbo].[mp_Modules] DROP CONSTRAINT [PK_Modules]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Modules]'
GO
ALTER TABLE [dbo].[mp_Modules] DROP CONSTRAINT [DF_mp_Modules_ShowTitle]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Modules]'
GO
ALTER TABLE [dbo].[mp_Modules] DROP CONSTRAINT [DF_mp_Modules_EditUserID]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Pages]'
GO
ALTER TABLE [dbo].[mp_Pages] DROP CONSTRAINT [PK_Tabs]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Pages]'
GO
ALTER TABLE [dbo].[mp_Pages] DROP CONSTRAINT [DF_mp_Pages_ParentID]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Pages]'
GO
ALTER TABLE [dbo].[mp_Pages] DROP CONSTRAINT [DF_mp_Pages_RequireSSL]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Pages]'
GO
ALTER TABLE [dbo].[mp_Pages] DROP CONSTRAINT [DF_mp_Pages_ShowBreadcrumbs]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Pages]'
GO
ALTER TABLE [dbo].[mp_Pages] DROP CONSTRAINT [DF_mp_Pages_UseLinkInsteadOfPage]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Pages]'
GO
ALTER TABLE [dbo].[mp_Pages] DROP CONSTRAINT [DF_mp_Pages_OpenInNewWindow]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Pages]'
GO
ALTER TABLE [dbo].[mp_Pages] DROP CONSTRAINT [DF_mp_Pages_ShowChildPageMenu]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Pages]'
GO
ALTER TABLE [dbo].[mp_Pages] DROP CONSTRAINT [DF_mp_Pages_ShowChildBreadCrumbs]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Pages]'
GO
ALTER TABLE [dbo].[mp_Pages] DROP CONSTRAINT [DF_mp_Pages_HideMainMenu]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [PK_Portals]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_AllowUserSkins]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_AllowPageSkins]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_AllowHideMenuOnPages]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_AllowNewRegistration]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_UseSecureRegistration]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_EncryptPasswords]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_UseSSLOnAllPages]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_IsServerAdminSite]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_UseLdapAuth]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_AutoCreateLdapUserOnFirstLogin]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_LdapPort]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_LdapUserDNKey]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_ReallyDeleteUsers]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_UseEmailForLogin]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_AllowUserFullNameChange]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_EditorSkin]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] DROP CONSTRAINT [DF_mp_Sites_DefaultFriendlyUrlPatternEnum]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Users]'
GO
ALTER TABLE [dbo].[mp_Users] DROP CONSTRAINT [DF_mp_Users_AvatarUrl]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[mp_Users]'
GO
ALTER TABLE [dbo].[mp_Users] DROP CONSTRAINT [DF_mp_Users_UserGuid]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping index [idxUserName] from [dbo].[mp_Users]'
GO
DROP INDEX [dbo].[mp_Users].[idxUserName]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping index [idxUserEmail] from [dbo].[mp_Users]'
GO
DROP INDEX [dbo].[mp_Users].[idxUserEmail]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[mp_UserPages]'
GO
CREATE TABLE [dbo].[mp_UserPages]
(
[UserPageID] [uniqueidentifier] NOT NULL,
[SiteID] [int] NOT NULL,
[UserGuid] [uniqueidentifier] NOT NULL,
[PageName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PagePath] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PageOrder] [int] NOT NULL CONSTRAINT [DF_mp_UserPages_PageOrder] DEFAULT ((3))
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_mp_UserPages] on [dbo].[mp_UserPages]'
GO
ALTER TABLE [dbo].[mp_UserPages] ADD CONSTRAINT [PK_mp_UserPages] PRIMARY KEY CLUSTERED  ([UserPageID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[mp_SitePaths]'
GO
CREATE TABLE [dbo].[mp_SitePaths]
(
[PathID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_mp_SitePaths_PathID] DEFAULT (newid()),
[SiteID] [int] NOT NULL,
[Path] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LoweredPath] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_mp_SitePaths] on [dbo].[mp_SitePaths]'
GO
ALTER TABLE [dbo].[mp_SitePaths] ADD CONSTRAINT [PK_mp_SitePaths] PRIMARY KEY NONCLUSTERED  ([PathID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[mp_UserProperties]'
GO
CREATE TABLE [dbo].[mp_UserProperties]
(
[UserID] [int] NOT NULL,
[PropertyName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PropertyNames] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PropertyValuesString] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PropertyValuesBinary] [image] NULL,
[LastUpdatedDate] [datetime] NOT NULL CONSTRAINT [DF_mp_UserProperties_LastUpdatedDate] DEFAULT (getdate())
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[mp_SitePersonalizationAllUsers]'
GO
CREATE TABLE [dbo].[mp_SitePersonalizationAllUsers]
(
[PathID] [uniqueidentifier] NOT NULL,
[PageSettings] [image] NOT NULL,
[LastUpdate] [datetime] NOT NULL CONSTRAINT [DF_mp_PersonalizationAllUsers_LastUpdateDate] DEFAULT (getdate())
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_mp_PersonalizationAllUsers] on [dbo].[mp_SitePersonalizationAllUsers]'
GO
ALTER TABLE [dbo].[mp_SitePersonalizationAllUsers] ADD CONSTRAINT [PK_mp_PersonalizationAllUsers] PRIMARY KEY CLUSTERED  ([PathID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[mp_Users]'
GO
ALTER TABLE [dbo].[mp_Users] ADD
[LoweredEmail] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PasswordQuestion] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PasswordAnswer] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RegisterConfirmGuid] [uniqueidentifier] NULL,
[LastActivityDate] [datetime] NULL,
[LastLoginDate] [datetime] NULL,
[LastPasswordChangedDate] [datetime] NULL,
[LastLockoutDate] [datetime] NULL,
[FailedPasswordAttemptCount] [int] NULL,
[FailedPasswordAttemptWindowStart] [datetime] NULL,
[FailedPasswordAnswerAttemptCount] [int] NULL,
[FailedPasswordAnswerAttemptWindowStart] [datetime] NULL,
[IsLockedOut] [bit] NOT NULL CONSTRAINT [DF_mp_Users_IsLockedOut] DEFAULT ((0)),
[MobilePIN] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PasswordSalt] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Comment] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [Name] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [LoginName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [Email] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [Password] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [Gender] [nchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [WebSiteURL] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [Country] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [State] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [Occupation] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [Interests] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [MSN] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [Yahoo] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [AIM] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [ICQ] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [AvatarUrl] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [Signature] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [UserGuid] [uniqueidentifier] NULL
ALTER TABLE [dbo].[mp_Users] ALTER COLUMN [Skin] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[mp_Pages]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_mp_Pages]
(
[PageID] [int] NOT NULL IDENTITY(0, 1),
[ParentID] [int] NULL CONSTRAINT [DF_mp_Pages_ParentID] DEFAULT ((-1)),
[PageOrder] [int] NOT NULL,
[SiteID] [int] NOT NULL,
[PageName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AuthorizedRoles] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EditRoles] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreateChildPageRoles] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RequireSSL] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_RequireSSL] DEFAULT ((0)),
[ShowBreadcrumbs] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_ShowBreadcrumbs] DEFAULT ((0)),
[PageKeyWords] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PageDescription] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PageEncoding] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AdditionalMetaTags] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MenuImage] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UseUrl] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_UseLinkInsteadOfPage] DEFAULT ((0)),
[Url] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OpenInNewWindow] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_OpenInNewWindow] DEFAULT ((0)),
[ShowChildPageMenu] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_ShowChildPageMenu] DEFAULT ((0)),
[ShowChildBreadCrumbs] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_ShowChildBreadCrumbs] DEFAULT ((0)),
[Skin] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HideMainMenu] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_HideMainMenu] DEFAULT ((0)),
[IncludeInMenu] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_IncludeInMenu] DEFAULT ((1))
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_mp_Pages] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_mp_Pages]([PageID], [ParentID], [PageOrder], [SiteID], [PageName], [AuthorizedRoles], [EditRoles], [CreateChildPageRoles], [RequireSSL], [ShowBreadcrumbs], [PageKeyWords], [PageDescription], [PageEncoding], [AdditionalMetaTags], [MenuImage], [UseUrl], [Url], [OpenInNewWindow], [ShowChildPageMenu], [ShowChildBreadCrumbs], [Skin], [HideMainMenu]) SELECT [PageID], [ParentID], [PageOrder], [SiteID], [PageName], [AuthorizedRoles], [EditRoles], [CreateChildPageRoles], [RequireSSL], [ShowBreadcrumbs], [PageKeyWords], [PageDescription], [PageEncoding], [AdditionalMetaTags], [MenuImage], [UseUrl], [Url], [OpenInNewWindow], [ShowChildPageMenu], [ShowChildBreadCrumbs], [Skin], [HideMainMenu] FROM [dbo].[mp_Pages]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_mp_Pages] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[mp_Pages]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
sp_rename N'[dbo].[tmp_rg_xx_mp_Pages]', N'mp_Pages'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Tabs] on [dbo].[mp_Pages]'
GO
ALTER TABLE [dbo].[mp_Pages] ADD CONSTRAINT [PK_Tabs] PRIMARY KEY NONCLUSTERED  ([PageID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[mp_Sites]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_mp_Sites]
(
[SiteID] [int] NOT NULL IDENTITY(1, 1),
[SiteGuid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_mp_Sites_SiteGuid] DEFAULT (newid()),
[SiteAlias] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SiteName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Skin] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Logo] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Icon] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AllowUserSkins] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowUserSkins] DEFAULT ((0)),
[AllowPageSkins] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowPageSkins] DEFAULT ((1)),
[AllowHideMenuOnPages] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowHideMenuOnPages] DEFAULT ((1)),
[AllowNewRegistration] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowNewRegistration] DEFAULT ((1)),
[UseSecureRegistration] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_UseSecureRegistration] DEFAULT ((0)),
[UseSSLOnAllPages] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_UseSSLOnAllPages] DEFAULT ((0)),
[DefaultPageKeyWords] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DefaultPageDescription] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DefaultPageEncoding] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DefaultAdditionalMetaTags] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsServerAdminSite] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_IsServerAdminSite] DEFAULT ((0)),
[UseLdapAuth] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_UseLdapAuth] DEFAULT ((0)),
[AutoCreateLdapUserOnFirstLogin] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AutoCreateLdapUserOnFirstLogin] DEFAULT ((1)),
[LdapServer] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LdapPort] [int] NOT NULL CONSTRAINT [DF_mp_Sites_LdapPort] DEFAULT ((389)),
[LdapRootDN] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LdapUserDNKey] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_mp_Sites_LdapUserDNKey] DEFAULT ('uid'),
[ReallyDeleteUsers] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_ReallyDeleteUsers] DEFAULT ((1)),
[UseEmailForLogin] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_UseEmailForLogin] DEFAULT ((1)),
[AllowUserFullNameChange] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowUserFullNameChange] DEFAULT ((0)),
[EditorSkin] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_mp_Sites_EditorSkin] DEFAULT ('normal'),
[DefaultFriendlyUrlPatternEnum] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_mp_Sites_DefaultFriendlyUrlPatternEnum] DEFAULT ('PageNameWithDotASPX'),
[AllowPasswordRetrieval] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowPasswordRetrieval] DEFAULT ((1)),
[AllowPasswordReset] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowPasswordReset] DEFAULT ((1)),
[RequiresQuestionAndAnswer] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_RequiresQuestionAndAnswer] DEFAULT ((0)),
[MaxInvalidPasswordAttempts] [int] NOT NULL CONSTRAINT [DF_mp_Sites_MaxInvalidPasswordAttempts] DEFAULT ((5)),
[PasswordAttemptWindowMinutes] [int] NOT NULL CONSTRAINT [DF_mp_Sites_PasswordAttemptWindowMinutes] DEFAULT ((5)),
[RequiresUniqueEmail] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_RequiresUniqueEmail] DEFAULT ((1)),
[PasswordFormat] [int] NOT NULL CONSTRAINT [DF_mp_Sites_PasswordFormat] DEFAULT ((0)),
[MinRequiredPasswordLength] [int] NOT NULL CONSTRAINT [DF_mp_Sites_MinRequiredPasswordLength] DEFAULT ((4)),
[MinRequiredNonAlphanumericCharacters] [int] NOT NULL CONSTRAINT [DF_mp_Sites_MinRequiredNonAlphanumericCharacters] DEFAULT ((0)),
[PasswordStrengthRegularExpression] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DefaultEmailFromAddress] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[EnableMyPageFeature] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_EnableMyPageFeature] DEFAULT ((1))
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_mp_Sites] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_mp_Sites]([SiteID], [SiteAlias], [SiteName], [Skin], [Logo], [Icon], [AllowUserSkins], [AllowPageSkins], [AllowHideMenuOnPages], [AllowNewRegistration], [UseSecureRegistration], [UseSSLOnAllPages], [DefaultPageKeyWords], [DefaultPageDescription], [DefaultPageEncoding], [DefaultAdditionalMetaTags], [IsServerAdminSite], [UseLdapAuth], [AutoCreateLdapUserOnFirstLogin], [LdapServer], [LdapPort], [LdapRootDN], [LdapUserDNKey], [ReallyDeleteUsers], [UseEmailForLogin], [AllowUserFullNameChange], [EditorSkin], [DefaultFriendlyUrlPatternEnum]) SELECT [SiteID], [SiteAlias], [SiteName], [Skin], [Logo], [Icon], [AllowUserSkins], [AllowPageSkins], [AllowHideMenuOnPages], [AllowNewRegistration], [UseSecureRegistration], [UseSSLOnAllPages], [DefaultPageKeyWords], [DefaultPageDescription], [DefaultPageEncoding], [DefaultAdditionalMetaTags], [IsServerAdminSite], [UseLdapAuth], [AutoCreateLdapUserOnFirstLogin], [LdapServer], [LdapPort], [LdapRootDN], [LdapUserDNKey], [ReallyDeleteUsers], [UseEmailForLogin], [AllowUserFullNameChange], [EditorSkin], [DefaultFriendlyUrlPatternEnum] FROM [dbo].[mp_Sites]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_mp_Sites] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[mp_Sites]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
sp_rename N'[dbo].[tmp_rg_xx_mp_Sites]', N'mp_Sites'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Portals] on [dbo].[mp_Sites]'
GO
ALTER TABLE [dbo].[mp_Sites] ADD CONSTRAINT [PK_Portals] PRIMARY KEY NONCLUSTERED  ([SiteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO

IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_mp_PageModules] on [dbo].[mp_PageModules]'
GO
ALTER TABLE [dbo].[mp_PageModules] ADD CONSTRAINT [PK_mp_PageModules] PRIMARY KEY CLUSTERED  ([PageID], [ModuleID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[mp_SitePersonalizationPerUser]'
GO
CREATE TABLE [dbo].[mp_SitePersonalizationPerUser]
(
[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_mp_SitePersonalizationPerUser_ID] DEFAULT (newid()),
[PathID] [uniqueidentifier] NOT NULL,
[UserID] [uniqueidentifier] NOT NULL,
[PageSettings] [image] NOT NULL,
[LastUpdate] [datetime] NOT NULL CONSTRAINT [DF_mp_PersonalizationPerUser_LastUpdateDate] DEFAULT (getdate())
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_mp_PersonalizationPerUser] on [dbo].[mp_SitePersonalizationPerUser]'
GO
ALTER TABLE [dbo].[mp_SitePersonalizationPerUser] ADD CONSTRAINT [PK_mp_PersonalizationPerUser] PRIMARY KEY NONCLUSTERED  ([ID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[mp_Modules]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_mp_Modules]
(
[ModuleID] [int] NOT NULL IDENTITY(0, 1),
[SiteID] [int] NULL,
[ModuleDefID] [int] NOT NULL,
[ModuleTitle] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AuthorizedEditRoles] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CacheTime] [int] NOT NULL,
[ShowTitle] [bit] NULL CONSTRAINT [DF_mp_Modules_ShowTitle] DEFAULT ((1)),
[EditUserID] [int] NOT NULL CONSTRAINT [DF_mp_Modules_EditUserID] DEFAULT ((0)),
[AvailableForMyPage] [bit] NOT NULL CONSTRAINT [DF_mp_Modules_AvailableForMyPage] DEFAULT ((0)),
[AllowMultipleInstancesOnMyPage] [bit] NOT NULL CONSTRAINT [DF_mp_Modules_AllowMultipleInstancesOnMyPage] DEFAULT ((1)),
[Icon] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedByUserID] [int] NULL CONSTRAINT [DF_mp_Modules_CreatedByUserID] DEFAULT ((-1)),
[CreatedDate] [datetime] NULL CONSTRAINT [DF_mp_Modules_CreatedDate] DEFAULT (getdate()),
[CountOfUseOnMyPage] [int] NOT NULL CONSTRAINT [DF_mp_Modules_CountOfUseOnMyPage] DEFAULT ((0))
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_mp_Modules] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_mp_Modules]([ModuleID], [ModuleDefID], [ModuleTitle], [AuthorizedEditRoles], [CacheTime], [ShowTitle], [EditUserID]) SELECT [ModuleID], [ModuleDefID], [ModuleTitle], [AuthorizedEditRoles], [CacheTime], [ShowTitle], [EditUserID] FROM [dbo].[mp_Modules]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_mp_Modules] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[mp_Modules]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
sp_rename N'[dbo].[tmp_rg_xx_mp_Modules]', N'mp_Modules'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Modules] on [dbo].[mp_Modules]'
GO
ALTER TABLE [dbo].[mp_Modules] ADD CONSTRAINT [PK_Modules] PRIMARY KEY NONCLUSTERED  ([ModuleID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[mp_WebParts]'
GO
CREATE TABLE [dbo].[mp_WebParts]
(
[WebPartID] [uniqueidentifier] NOT NULL,
[SiteID] [int] NOT NULL,
[Title] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ImageUrl] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ClassName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AssemblyName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AvailableForMyPage] [bit] NOT NULL CONSTRAINT [DF_mp_WebParts_AvailableForMyPage] DEFAULT ((0)),
[AllowMultipleInstancesOnMyPage] [bit] NOT NULL CONSTRAINT [DF_mp_WebParts_AllowMultipleInstancesOnMyPage] DEFAULT ((1)),
[AvailableForContentSystem] [bit] NOT NULL CONSTRAINT [DF_mp_WebParts_AvailableForContentSystem] DEFAULT ((0)),
[CountOfUseOnMyPage] [int] NOT NULL CONSTRAINT [DF_mp_WebParts_CountOfUseOnMyPage] DEFAULT ((0))
)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_mp_WebParts] on [dbo].[mp_WebParts]'
GO
ALTER TABLE [dbo].[mp_WebParts] ADD CONSTRAINT [PK_mp_WebParts] PRIMARY KEY CLUSTERED  ([WebPartID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[mp_ModuleDefinitions]'
GO
ALTER TABLE [dbo].[mp_ModuleDefinitions] ADD
[Icon] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[mp_ModuleDefinitions] ALTER COLUMN [FeatureName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
ALTER TABLE [dbo].[mp_ModuleDefinitions] ALTER COLUMN [ControlSrc] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[mp_SitePaths]'
GO
ALTER TABLE [dbo].[mp_SitePaths] ADD CONSTRAINT [IX_mp_SitePaths] UNIQUE NONCLUSTERED  ([SiteID], [Path])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[mp_Users]'
GO
ALTER TABLE [dbo].[mp_Users] ADD CONSTRAINT [DF_mp_Users_AvatarUrl] DEFAULT ('blank.gif') FOR [AvatarUrl]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[mp_Users] ADD CONSTRAINT [DF_mp_Users_UserGuid] DEFAULT (newid()) FOR [UserGuid]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[mp_HtmlContent]'
GO
ALTER TABLE [dbo].[mp_HtmlContent] WITH NOCHECK ADD
CONSTRAINT [FK_HtmlText_Modules] FOREIGN KEY ([ModuleID]) REFERENCES [dbo].[mp_Modules] ([ModuleID]) ON DELETE CASCADE NOT FOR REPLICATION
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[mp_Links]'
GO
ALTER TABLE [dbo].[mp_Links] WITH NOCHECK ADD
CONSTRAINT [FK_Links_Modules] FOREIGN KEY ([ModuleID]) REFERENCES [dbo].[mp_Modules] ([ModuleID]) ON DELETE CASCADE NOT FOR REPLICATION
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[mp_ModuleSettings]'
GO
ALTER TABLE [dbo].[mp_ModuleSettings] WITH NOCHECK ADD
CONSTRAINT [FK_ModuleSettings_Modules] FOREIGN KEY ([ModuleID]) REFERENCES [dbo].[mp_Modules] ([ModuleID]) ON DELETE CASCADE NOT FOR REPLICATION
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[mp_Modules]'
GO
ALTER TABLE [dbo].[mp_Modules] WITH NOCHECK ADD
CONSTRAINT [FK_Modules_ModuleDefinitions] FOREIGN KEY ([ModuleDefID]) REFERENCES [dbo].[mp_ModuleDefinitions] ([ModuleDefID]) ON DELETE CASCADE NOT FOR REPLICATION
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[mp_Pages]'
GO
ALTER TABLE [dbo].[mp_Pages] WITH NOCHECK ADD
CONSTRAINT [FK_Tabs_Portals] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[mp_Sites] ([SiteID]) ON DELETE CASCADE NOT FOR REPLICATION
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[mp_SitePaths]'
GO
ALTER TABLE [dbo].[mp_SitePaths] WITH NOCHECK ADD
CONSTRAINT [FK_mp_SitePaths_mp_Sites] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[mp_Sites] ([SiteID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[mp_Roles]'
GO
ALTER TABLE [dbo].[mp_Roles] WITH NOCHECK ADD
CONSTRAINT [FK_Roles_Portals] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[mp_Sites] ([SiteID]) ON DELETE CASCADE NOT FOR REPLICATION
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[mp_PageModules]'
GO
ALTER TABLE [dbo].[mp_PageModules] ADD
CONSTRAINT [FK_mp_PageModules_mp_Modules] FOREIGN KEY ([ModuleID]) REFERENCES [dbo].[mp_Modules] ([ModuleID]),
CONSTRAINT [FK_mp_PageModules_mp_PageModules] FOREIGN KEY ([PageID]) REFERENCES [dbo].[mp_Pages] ([PageID])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO