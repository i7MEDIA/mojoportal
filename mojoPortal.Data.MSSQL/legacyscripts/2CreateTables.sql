SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_BlogCategories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[Category] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_mp_BlogCategories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteHosts]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_SiteHosts](
	[HostID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[HostName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_mp_SiteHosts] PRIMARY KEY CLUSTERED 
(
	[HostID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogComments]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_BlogComments](
	[BlogCommentID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[ItemID] [int] NOT NULL,
	[Comment] [ntext] NOT NULL,
	[Title] [nvarchar](100) NULL,
	[Name] [nvarchar](100) NULL,
	[URL] [nvarchar](200) NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_mp_BlogComments_DateCreated]  DEFAULT (getdate()),
 CONSTRAINT [PK_mp_BlogComments] PRIMARY KEY CLUSTERED 
(
	[BlogCommentID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_FriendlyUrls](
	[UrlID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NULL,
	[FriendlyUrl] [varchar](255) NULL,
	[RealUrl] [varchar](255) NULL,
	[IsPattern] [bit] NOT NULL CONSTRAINT [DF_mp_FriendlyUrls_IsPattern]  DEFAULT ((0)),
 CONSTRAINT [PK_mp_FriendlyUrls] PRIMARY KEY CLUSTERED 
(
	[UrlID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blogs]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_Blogs](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[CreatedByUser] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[Title] [nvarchar](100) NULL,
	[Excerpt] [nvarchar](512) NULL,
	[StartDate] [datetime] NULL,
	[IsInNewsletter] [bit] NULL,
	[Description] [ntext] NULL,
	[CommentCount] [int] NOT NULL CONSTRAINT [DF_mp_Blogs_Comments]  DEFAULT ((0)),
	[TrackBackCount] [int] NOT NULL CONSTRAINT [DF_mp_Blogs_TrackBackCount]  DEFAULT ((0)),
	[Categories] [ntext] NULL,
	[IncludeInFeed] [bit] NOT NULL CONSTRAINT [DF_mp_Blogs_IncludeInFeed]  DEFAULT ((1)),
 CONSTRAINT [PK_mp_Blogs] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_ForumPosts](
	[PostID] [int] IDENTITY(1,1) NOT NULL,
	[ThreadID] [int] NOT NULL,
	[ThreadSequence] [int] NOT NULL CONSTRAINT [DF_mp_ForumPosts_ThreadSequence]  DEFAULT ((1)),
	[Subject] [nvarchar](255) NOT NULL,
	[PostDate] [datetime] NOT NULL CONSTRAINT [DF_mp_ForumPosts_PostDate]  DEFAULT (getdate()),
	[Approved] [bit] NOT NULL CONSTRAINT [DF_mp_ForumPosts_Approved]  DEFAULT ((0)),
	[UserID] [int] NOT NULL CONSTRAINT [DF_mp_ForumPosts_UserID]  DEFAULT ((-1)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF_mp_ForumPosts_SortOrder]  DEFAULT ((100)),
	[Post] [ntext] NOT NULL,
 CONSTRAINT [PK_mp_ForumPosts] PRIMARY KEY CLUSTERED 
(
	[PostID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumSubscriptions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_ForumSubscriptions](
	[SubscriptionID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ForumID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[SubscribeDate] [datetime] NOT NULL CONSTRAINT [DF_mp_ForumSubscriptions_SubscribeDate]  DEFAULT (getdate()),
	[UnSubscribeDate] [datetime] NULL,
 CONSTRAINT [PK_mp_ForumSubscriptions] PRIMARY KEY CLUSTERED 
(
	[SubscriptionID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaVersion]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_SchemaVersion](
	[ApplicationID] [uniqueidentifier] NOT NULL,
	[ApplicationName] [nvarchar](255) NOT NULL,
	[Major] [int] NOT NULL CONSTRAINT [DF_mp_SchemaVersion_Major]  DEFAULT ((0)),
	[Minor] [int] NOT NULL CONSTRAINT [DF_mp_SchemaVersion_Minor]  DEFAULT ((0)),
	[Build] [int] NOT NULL CONSTRAINT [DF_mp_SchemaVersion_Build]  DEFAULT ((0)),
	[Revision] [int] NOT NULL CONSTRAINT [DF_mp_SchemaVersion_Revision]  DEFAULT ((0)),
 CONSTRAINT [PK_mp_SchemaVersion] PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreadSubscriptions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_ForumThreadSubscriptions](
	[ThreadSubscriptionID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[ThreadID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[SubscribeDate] [datetime] NOT NULL CONSTRAINT [DF_mp_ForumThreadSubscriptions_SubscribeDate]  DEFAULT (getdate()),
	[UnSubscribeDate] [datetime] NULL,
 CONSTRAINT [PK_mp_ForumThreadSubscriptions] PRIMARY KEY CLUSTERED 
(
	[ThreadSubscriptionID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_WebParts](
	[WebPartID] [uniqueidentifier] NOT NULL,
	[SiteID] [int] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[ImageUrl] [nvarchar](255) NULL,
	[ClassName] [nvarchar](255) NOT NULL,
	[AssemblyName] [nvarchar](255) NOT NULL,
	[AvailableForMyPage] [bit] NOT NULL CONSTRAINT [DF_mp_WebParts_AvailableForMyPage]  DEFAULT ((0)),
	[AllowMultipleInstancesOnMyPage] [bit] NOT NULL CONSTRAINT [DF_mp_WebParts_AllowMultipleInstancesOnMyPage]  DEFAULT ((1)),
	[AvailableForContentSystem] [bit] NOT NULL CONSTRAINT [DF_mp_WebParts_AvailableForContentSystem]  DEFAULT ((0)),
	[CountOfUseOnMyPage] [int] NOT NULL CONSTRAINT [DF_mp_WebParts_CountOfUseOnMyPage]  DEFAULT ((0)),
 CONSTRAINT [PK_mp_WebParts] PRIMARY KEY CLUSTERED 
(
	[WebPartID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_SchemaScriptHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [uniqueidentifier] NOT NULL,
	[ScriptFile] [nvarchar](255) NOT NULL,
	[RunTime] [datetime] NOT NULL CONSTRAINT [DF_mp_SchemaScriptHistory_RunCompletedTime]  DEFAULT (getutcdate()),
	[ErrorOccurred] [bit] NOT NULL CONSTRAINT [DF_mp_SchemaScriptHistory_ErrorOccurred]  DEFAULT ((0)),
	[ErrorMessage] [ntext] NULL,
	[ScriptBody] [ntext] NULL,
 CONSTRAINT [PK_mp_SchemaScriptHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_GalleryImages](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_mp_GalleryImages_DisplayOrder]  DEFAULT ((100)),
	[Caption] [nvarchar](255) NULL,
	[Description] [ntext] NULL,
	[MetaDataXml] [ntext] NULL,
	[ImageFile] [nvarchar](100) NULL,
	[WebImageFile] [nvarchar](100) NULL,
	[ThumbnailFile] [nvarchar](100) NULL,
	[UploadDate] [datetime] NOT NULL CONSTRAINT [DF_mp_GalleryImages_UploadDate]  DEFAULT (getdate()),
	[UploadUser] [nvarchar](100) NULL,
 CONSTRAINT [PK_mp_GalleryImages] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitionSettings]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_ModuleDefinitionSettings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleDefID] [int] NOT NULL,
	[SettingName] [nvarchar](50) NOT NULL,
	[SettingValue] [nvarchar](255) NOT NULL,
	[ControlType] [nvarchar](50) NULL,
	[RegexValidationExpression] [ntext] NULL,
 CONSTRAINT [PK_mp_ModuleDefinitionSettings_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_mp_ModuleDefinitionSettings] UNIQUE NONCLUSTERED 
(
	[ModuleDefID] ASC,
	[SettingName] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_ModuleDefinitions](
	[ModuleDefID] [int] IDENTITY(1,1) NOT NULL,
	[FeatureName] [nvarchar](255) NOT NULL,
	[ControlSrc] [nvarchar](255) NOT NULL,
	[SortOrder] [int] NOT NULL CONSTRAINT [DF_mp_ModuleDefinitions_SortOrder]  DEFAULT ((500)),
	[IsAdmin] [bit] NOT NULL CONSTRAINT [DF_mp_ModuleDefinitions_IsAdmin]  DEFAULT ((0)),
	[Icon] [nvarchar](255) NULL,
	[DefaultCacheTime] [int] NOT NULL CONSTRAINT [DF_mp_ModuleDefinitions_DefaultCacheTime]  DEFAULT ((0)),
 CONSTRAINT [PK_ModuleDefinitions] PRIMARY KEY NONCLUSTERED 
(
	[ModuleDefID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_Forums](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_mp_ForumBoards_DateCreated]  DEFAULT (getdate()),
	[CreatedBy] [int] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [ntext] NOT NULL,
	[IsModerated] [bit] NOT NULL CONSTRAINT [DF_mp_ForumBoards_Moderated]  DEFAULT ((0)),
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_mp_ForumBoards_Active]  DEFAULT ((1)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF_mp_ForumBoards_SortOrder]  DEFAULT ((100)),
	[ThreadCount] [int] NOT NULL CONSTRAINT [DF_mp_ForumBoards_TotalPosts]  DEFAULT ((0)),
	[PostCount] [int] NOT NULL CONSTRAINT [DF_mp_ForumBoards_TotalThreads]  DEFAULT ((0)),
	[MostRecentPostDate] [datetime] NULL,
	[MostRecentPostUserID] [int] NOT NULL CONSTRAINT [DF_mp_ForumBoards_MostRecentPostUserID]  DEFAULT ((-1)),
	[PostsPerPage] [int] NOT NULL CONSTRAINT [DF_mp_Forums_EntriesPerPage]  DEFAULT ((10)),
	[ThreadsPerPage] [int] NOT NULL CONSTRAINT [DF_mp_Forums_ThreadsPerPage]  DEFAULT ((40)),
	[AllowAnonymousPosts] [bit] NOT NULL CONSTRAINT [DF_mp_Forums_AllowAnonymousPosts]  DEFAULT ((1)),
 CONSTRAINT [PK_mp_ForumBoards] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_RssFeeds]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_RssFeeds](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_mp_RssFeeds_CreatedDate]  DEFAULT (getdate()),
	[UserID] [int] NOT NULL,
	[Author] [nvarchar](100) NOT NULL,
	[Url] [nvarchar](255) NOT NULL,
	[RssUrl] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_mp_RssFeeds] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_SharedFiles](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[UploadUserID] [int] NOT NULL,
	[FriendlyName] [nvarchar](255) NOT NULL,
	[OriginalFileName] [nvarchar](255) NOT NULL,
	[ServerFileName] [nvarchar](255) NOT NULL,
	[SizeInKB] [int] NOT NULL CONSTRAINT [DF_mp_SharedFiles_SizeInKB]  DEFAULT ((0)),
	[UploadDate] [datetime] NOT NULL CONSTRAINT [DF_mp_SharedFiles_UploadDate]  DEFAULT (getdate()),
	[FolderID] [int] NOT NULL CONSTRAINT [DF_mp_SharedFiles_FolderID]  DEFAULT ((0)),
 CONSTRAINT [PK_mp_SharedFiles] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFilesHistory]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_SharedFilesHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[ModuleID] [int] NOT NULL,
	[FriendlyName] [nvarchar](255) NOT NULL,
	[OriginalFileName] [nvarchar](255) NULL,
	[ServerFileName] [nvarchar](50) NOT NULL,
	[SizeInKB] [int] NOT NULL CONSTRAINT [DF_mp_SharedFilesHistory_SizeInKB]  DEFAULT ((0)),
	[UploadDate] [datetime] NOT NULL,
	[ArchiveDate] [datetime] NOT NULL CONSTRAINT [DF_mp_SharedFilesHistory_ArchiveDate]  DEFAULT (getdate()),
	[UploadUserID] [int] NOT NULL CONSTRAINT [DF_mp_SharedFilesHistory_UploadUserID]  DEFAULT ((0)),
 CONSTRAINT [PK_mp_SharedFilesHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_UserProperties](
	[PropertyID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_mp_UserProperties_PropertyID]  DEFAULT (newid()),
	[UserGuid] [uniqueidentifier] NOT NULL,
	[PropertyName] [nvarchar](255) NULL,
	[PropertyValueString] [ntext] NULL,
	[PropertyValueBinary] [image] NULL,
	[LastUpdatedDate] [datetime] NOT NULL CONSTRAINT [DF_mp_UserProperties_LastUpdatedDate]  DEFAULT (getdate()),
	[IsLazyLoaded] [bit] NOT NULL CONSTRAINT [DF_mp_UserProperties_IsLazyLoaded]  DEFAULT ((0)),
 CONSTRAINT [PK_mp_UserProperties] PRIMARY KEY CLUSTERED 
(
	[PropertyID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_SiteModuleDefinitions](
	[SiteID] [int] NOT NULL,
	[ModuleDefID] [int] NOT NULL,
	[AuthorizedRoles] [ntext] NULL,
 CONSTRAINT [PK_mp_SiteModuleDefinitions] PRIMARY KEY CLUSTERED 
(
	[SiteID] ASC,
	[ModuleDefID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[LoginName] [nvarchar](50) NULL,
	[Email] [nvarchar](100) NOT NULL,
	[LoweredEmail] [nvarchar](100) NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordQuestion] [nvarchar](255) NULL,
	[PasswordAnswer] [nvarchar](255) NULL,
	[Gender] [nchar](10) NULL,
	[ProfileApproved] [bit] NOT NULL CONSTRAINT [DF_Users_ProfileApproved]  DEFAULT ((1)),
	[RegisterConfirmGuid] [uniqueidentifier] NULL,
	[ApprovedForForums] [bit] NOT NULL CONSTRAINT [DF_Users_Approved]  DEFAULT ((1)),
	[Trusted] [bit] NOT NULL CONSTRAINT [DF_Users_Trusted]  DEFAULT ((0)),
	[DisplayInMemberList] [bit] NULL CONSTRAINT [DF_mp_Users_DisplayInMemberList]  DEFAULT ((1)),
	[WebSiteURL] [nvarchar](100) NULL,
	[Country] [nvarchar](100) NULL,
	[State] [nvarchar](100) NULL,
	[Occupation] [nvarchar](100) NULL,
	[Interests] [nvarchar](100) NULL,
	[MSN] [nvarchar](50) NULL,
	[Yahoo] [nvarchar](50) NULL,
	[AIM] [nvarchar](50) NULL,
	[ICQ] [nvarchar](50) NULL,
	[TotalPosts] [int] NOT NULL CONSTRAINT [DF_Users_TotalPosts]  DEFAULT ((0)),
	[AvatarUrl] [nvarchar](255) NULL CONSTRAINT [DF_mp_Users_AvatarUrl]  DEFAULT ('blank.gif'),
	[TimeOffsetHours] [int] NOT NULL CONSTRAINT [DF_mp_Users_TimeOffSetHours]  DEFAULT ((0)),
	[Signature] [nvarchar](255) NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_Users_DateCreated]  DEFAULT (getdate()),
	[UserGuid] [uniqueidentifier] NULL CONSTRAINT [DF_mp_Users_UserGuid]  DEFAULT (newid()),
	[Skin] [nvarchar](100) NULL,
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_mp_Users_IsDeleted]  DEFAULT ((0)),
	[LastActivityDate] [datetime] NULL,
	[LastLoginDate] [datetime] NULL,
	[LastPasswordChangedDate] [datetime] NULL,
	[LastLockoutDate] [datetime] NULL,
	[FailedPasswordAttemptCount] [int] NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NULL,
	[FailedPasswordAnswerAttemptCount] [int] NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NULL,
	[IsLockedOut] [bit] NOT NULL CONSTRAINT [DF_mp_Users_IsLockedOut]  DEFAULT ((0)),
	[MobilePIN] [nvarchar](16) NULL,
	[PasswordSalt] [nvarchar](128) NULL,
	[Comment] [ntext] NULL,
 CONSTRAINT [PK_mp_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserRoles]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_UserRoles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_mp_UserRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_UserPages](
	[UserPageID] [uniqueidentifier] NOT NULL,
	[SiteID] [int] NOT NULL,
	[UserGuid] [uniqueidentifier] NOT NULL,
	[PageName] [nvarchar](255) NOT NULL,
	[PagePath] [nvarchar](255) NOT NULL,
	[PageOrder] [int] NOT NULL CONSTRAINT [DF_mp_UserPages_PageOrder]  DEFAULT ((3)),
 CONSTRAINT [PK_mp_UserPages] PRIMARY KEY CLUSTERED 
(
	[UserPageID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_CalendarEvents](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[Title] [nvarchar](255) NULL,
	[Description] [ntext] NULL,
	[ImageName] [nvarchar](100) NULL,
	[EventDate] [datetime] NULL,
	[StartTime] [smalldatetime] NULL,
	[EndTime] [smalldatetime] NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_mp_CalendarEvents_CreatedDate]  DEFAULT (getdate()),
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_mp_CalendarEvents] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationAllUsers]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_SitePersonalizationAllUsers](
	[PathID] [uniqueidentifier] NOT NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdate] [datetime] NOT NULL CONSTRAINT [DF_mp_PersonalizationAllUsers_LastUpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_mp_PersonalizationAllUsers] PRIMARY KEY CLUSTERED 
(
	[PathID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_ForumThreads](
	[ThreadID] [int] IDENTITY(1,1) NOT NULL,
	[ForumID] [int] NOT NULL,
	[ThreadSubject] [nvarchar](255) NOT NULL,
	[ThreadDate] [datetime] NOT NULL CONSTRAINT [DF_mp_ForumThreads_ThreadDate]  DEFAULT (getdate()),
	[TotalViews] [int] NOT NULL CONSTRAINT [DF_mp_ForumThreads_TotalViews]  DEFAULT ((0)),
	[TotalReplies] [int] NOT NULL CONSTRAINT [DF_mp_ForumThreads_TotalReplies]  DEFAULT ((0)),
	[SortOrder] [int] NOT NULL CONSTRAINT [DF_mp_ForumThreads_SortOrder]  DEFAULT ((1000)),
	[IsLocked] [bit] NOT NULL CONSTRAINT [DF_mp_ForumThreads_IsLocked]  DEFAULT ((0)),
	[ForumSequence] [int] NOT NULL CONSTRAINT [DF_mp_ForumThreads_ForumSequence]  DEFAULT ((1)),
	[MostRecentPostDate] [datetime] NULL CONSTRAINT [DF_mp_ForumThreads_MostRecentPostDate]  DEFAULT (getdate()),
	[MostRecentPostUserID] [int] NULL,
	[StartedByUserID] [int] NOT NULL,
 CONSTRAINT [PK_mp_ForumThreads] PRIMARY KEY CLUSTERED 
(
	[ThreadID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_Sites](
	[SiteID] [int] IDENTITY(1,1) NOT NULL,
	[SiteGuid] [uniqueidentifier] NOT NULL,
	[SiteAlias] [nvarchar](50) NULL,
	[SiteName] [nvarchar](255) NOT NULL,
	[Skin] [nvarchar](100) NULL,
	[Logo] [nvarchar](50) NULL,
	[Icon] [nvarchar](50) NULL,
	[AllowUserSkins] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowUserSkins]  DEFAULT ((0)),
	[AllowPageSkins] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowPageSkins]  DEFAULT ((1)),
	[AllowHideMenuOnPages] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowHideMenuOnPages]  DEFAULT ((1)),
	[AllowNewRegistration] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowNewRegistration]  DEFAULT ((1)),
	[UseSecureRegistration] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_UseSecureRegistration]  DEFAULT ((0)),
	[UseSSLOnAllPages] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_UseSSLOnAllPages]  DEFAULT ((0)),
	[DefaultPageKeyWords] [nvarchar](255) NULL,
	[DefaultPageDescription] [nvarchar](255) NULL,
	[DefaultPageEncoding] [nvarchar](255) NULL,
	[DefaultAdditionalMetaTags] [nvarchar](255) NULL,
	[IsServerAdminSite] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_IsServerAdminSite]  DEFAULT ((0)),
	[UseLdapAuth] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_UseLdapAuth]  DEFAULT ((0)),
	[AutoCreateLdapUserOnFirstLogin] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AutoCreateLdapUserOnFirstLogin]  DEFAULT ((1)),
	[LdapServer] [nvarchar](255) NULL,
	[LdapPort] [int] NOT NULL CONSTRAINT [DF_mp_Sites_LdapPort]  DEFAULT ((389)),
	[LdapDomain] [nvarchar](255) NULL,
	[LdapRootDN] [nvarchar](255) NULL,
	[LdapUserDNKey] [nvarchar](10) NOT NULL CONSTRAINT [DF_mp_Sites_LdapUserDNKey]  DEFAULT ('uid'),
	[ReallyDeleteUsers] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_ReallyDeleteUsers]  DEFAULT ((1)),
	[UseEmailForLogin] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_UseEmailForLogin]  DEFAULT ((1)),
	[AllowUserFullNameChange] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowUserFullNameChange]  DEFAULT ((0)),
	[EditorSkin] [nvarchar](50) NOT NULL CONSTRAINT [DF_mp_Sites_EditorSkin]  DEFAULT ('normal'),
	[DefaultFriendlyUrlPatternEnum] [nvarchar](50) NOT NULL CONSTRAINT [DF_mp_Sites_DefaultFriendlyUrlPatternEnum]  DEFAULT ('PageNameWithDotASPX'),
	[AllowPasswordRetrieval] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowPasswordRetrieval]  DEFAULT ((1)),
	[AllowPasswordReset] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_AllowPasswordReset]  DEFAULT ((1)),
	[RequiresQuestionAndAnswer] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_RequiresQuestionAndAnswer]  DEFAULT ((0)),
	[MaxInvalidPasswordAttempts] [int] NOT NULL CONSTRAINT [DF_mp_Sites_MaxInvalidPasswordAttempts]  DEFAULT ((5)),
	[PasswordAttemptWindowMinutes] [int] NOT NULL CONSTRAINT [DF_mp_Sites_PasswordAttemptWindowMinutes]  DEFAULT ((5)),
	[RequiresUniqueEmail] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_RequiresUniqueEmail]  DEFAULT ((1)),
	[PasswordFormat] [int] NOT NULL CONSTRAINT [DF_mp_Sites_PasswordFormat]  DEFAULT ((0)),
	[MinRequiredPasswordLength] [int] NOT NULL CONSTRAINT [DF_mp_Sites_MinRequiredPasswordLength]  DEFAULT ((4)),
	[MinRequiredNonAlphanumericCharacters] [int] NOT NULL CONSTRAINT [DF_mp_Sites_MinRequiredNonAlphanumericCharacters]  DEFAULT ((0)),
	[PasswordStrengthRegularExpression] [ntext] NULL,
	[DefaultEmailFromAddress] [nvarchar](100) NULL,
	[EnableMyPageFeature] [bit] NOT NULL CONSTRAINT [DF_mp_Sites_EnableMyPageFeature]  DEFAULT ((1)),
 CONSTRAINT [PK_Portals] PRIMARY KEY NONCLUSTERED 
(
	[SiteID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_SharedFileFolders](
	[FolderID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[FolderName] [nvarchar](255) NOT NULL,
	[ParentID] [int] NOT NULL CONSTRAINT [DF_mp_SharedFileFolders_ParentID]  DEFAULT ((0)),
 CONSTRAINT [PK_mp_SharedFileFolders] PRIMARY KEY CLUSTERED 
(
	[FolderID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogItemCategories]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_BlogItemCategories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
 CONSTRAINT [PK_mp_BlogItemCategories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationPerUser]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_SitePersonalizationPerUser](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_mp_SitePersonalizationPerUser_ID]  DEFAULT (newid()),
	[PathID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdate] [datetime] NOT NULL CONSTRAINT [DF_mp_PersonalizationPerUser_LastUpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_mp_PersonalizationPerUser] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogStats]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_BlogStats](
	[ModuleID] [int] NOT NULL,
	[EntryCount] [int] NOT NULL CONSTRAINT [DF_mp_BlogStats_EntryCount]  DEFAULT ((0)),
	[CommentCount] [int] NOT NULL CONSTRAINT [DF_mp_BlogStats_CommentCount]  DEFAULT ((0)),
	[TrackBackCount] [int] NOT NULL CONSTRAINT [DF_mp_BlogStats_TrackBackCount]  DEFAULT ((0)),
 CONSTRAINT [PK_mp_BlogStats] PRIMARY KEY CLUSTERED 
(
	[ModuleID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_HtmlContent](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[Title] [nvarchar](255) NULL,
	[Excerpt] [ntext] NULL,
	[Body] [ntext] NOT NULL,
	[MoreLink] [nvarchar](255) NULL,
	[SortOrder] [int] NOT NULL CONSTRAINT [DF_mp_HtmlText_SortOrder]  DEFAULT ((500)),
	[BeginDate] [datetime] NOT NULL CONSTRAINT [DF_mp_HtmlText_BeginDate]  DEFAULT (((1)/(1))/(1901)),
	[EndDate] [datetime] NOT NULL CONSTRAINT [DF_mp_HtmlText_EndDate]  DEFAULT (((1)/(1))/(2200)),
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_mp_HtmlText_CreatedDate]  DEFAULT (getdate()),
	[UserID] [int] NULL,
 CONSTRAINT [PK_mp_HtmlText] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_Links](
	[ItemID] [int] IDENTITY(0,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[Title] [nvarchar](255) NULL,
	[Url] [nvarchar](255) NULL,
	[Target] [nvarchar](20) NOT NULL CONSTRAINT [DF_mp_Links_Target]  DEFAULT ('_blank'),
	[ViewOrder] [int] NULL,
	[Description] [ntext] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_Links] PRIMARY KEY NONCLUSTERED 
(
	[ItemID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleSettings]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_ModuleSettings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NOT NULL,
	[SettingName] [nvarchar](50) NOT NULL,
	[SettingValue] [nvarchar](255) NOT NULL,
	[ControlType] [nvarchar](50) NULL,
	[RegexValidationExpression] [ntext] NULL,
 CONSTRAINT [PK_mp_ModuleSettings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PageModules]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_PageModules](
	[PageID] [int] NOT NULL,
	[ModuleID] [int] NOT NULL,
	[PaneName] [nvarchar](50) NOT NULL,
	[ModuleOrder] [int] NOT NULL CONSTRAINT [DF_mp_PageModules_ModuleOrder]  DEFAULT ((3)),
	[PublishBeginDate] [datetime] NOT NULL CONSTRAINT [DF_mp_PageModules_PublishBeginDate]  DEFAULT (getdate()),
	[PublishEndDate] [datetime] NULL,
 CONSTRAINT [PK_mp_PageModules] PRIMARY KEY CLUSTERED 
(
	[PageID] ASC,
	[ModuleID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_Modules](
	[ModuleID] [int] IDENTITY(0,1) NOT NULL,
	[SiteID] [int] NULL,
	[ModuleDefID] [int] NOT NULL,
	[ModuleTitle] [nvarchar](255) NULL,
	[AuthorizedEditRoles] [ntext] NULL,
	[CacheTime] [int] NOT NULL,
	[ShowTitle] [bit] NULL CONSTRAINT [DF_mp_Modules_ShowTitle]  DEFAULT ((1)),
	[EditUserID] [int] NOT NULL CONSTRAINT [DF_mp_Modules_EditUserID]  DEFAULT ((0)),
	[AvailableForMyPage] [bit] NOT NULL CONSTRAINT [DF_mp_Modules_AvailableForMyPage]  DEFAULT ((0)),
	[AllowMultipleInstancesOnMyPage] [bit] NOT NULL CONSTRAINT [DF_mp_Modules_AllowMultipleInstancesOnMyPage]  DEFAULT ((1)),
	[Icon] [nvarchar](255) NULL,
	[CreatedByUserID] [int] NULL CONSTRAINT [DF_mp_Modules_CreatedByUserID]  DEFAULT ((-1)),
	[CreatedDate] [datetime] NULL CONSTRAINT [DF_mp_Modules_CreatedDate]  DEFAULT (getdate()),
	[CountOfUseOnMyPage] [int] NOT NULL CONSTRAINT [DF_mp_Modules_CountOfUseOnMyPage]  DEFAULT ((0)),
 CONSTRAINT [PK_Modules] PRIMARY KEY NONCLUSTERED 
(
	[ModuleID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePaths]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_SitePaths](
	[PathID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_mp_SitePaths_PathID]  DEFAULT (newid()),
	[SiteID] [int] NOT NULL,
	[Path] [nvarchar](255) NOT NULL,
	[LoweredPath] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_mp_SitePaths] PRIMARY KEY NONCLUSTERED 
(
	[PathID] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_mp_SitePaths] UNIQUE NONCLUSTERED 
(
	[SiteID] ASC,
	[Path] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[DisplayName] [nvarchar](50) NULL,
 CONSTRAINT [PK_mp_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [dbo].[mp_Pages](
	[PageID] [int] IDENTITY(0,1) NOT NULL,
	[ParentID] [int] NULL CONSTRAINT [DF_mp_Pages_ParentID]  DEFAULT ((-1)),
	[PageOrder] [int] NOT NULL,
	[SiteID] [int] NOT NULL,
	[PageName] [nvarchar](50) NOT NULL,
	[PageTitle] [nvarchar](255) NULL,
	[AuthorizedRoles] [ntext] NULL,
	[EditRoles] [ntext] NULL,
	[CreateChildPageRoles] [ntext] NULL,
	[RequireSSL] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_RequireSSL]  DEFAULT ((0)),
	[AllowBrowserCache] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_AllowBrowserCache]  DEFAULT ((1)),
	[ShowBreadcrumbs] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_ShowBreadcrumbs]  DEFAULT ((0)),
	[PageKeyWords] [nvarchar](255) NULL,
	[PageDescription] [nvarchar](255) NULL,
	[PageEncoding] [nvarchar](255) NULL,
	[AdditionalMetaTags] [nvarchar](255) NULL,
	[MenuImage] [nvarchar](50) NULL,
	[UseUrl] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_UseLinkInsteadOfPage]  DEFAULT ((0)),
	[Url] [nvarchar](255) NULL,
	[OpenInNewWindow] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_OpenInNewWindow]  DEFAULT ((0)),
	[ShowChildPageMenu] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_ShowChildPageMenu]  DEFAULT ((0)),
	[ShowChildBreadCrumbs] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_ShowChildBreadCrumbs]  DEFAULT ((0)),
	[Skin] [nvarchar](100) NULL,
	[HideMainMenu] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_HideMainMenu]  DEFAULT ((0)),
	[IncludeInMenu] [bit] NOT NULL CONSTRAINT [DF_mp_Pages_IncludeInMenu]  DEFAULT ((1)),
 CONSTRAINT [PK_Tabs] PRIMARY KEY NONCLUSTERED 
(
	[PageID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_HtmlText_Modules]') AND type = 'F')
ALTER TABLE [dbo].[mp_HtmlContent]  WITH NOCHECK ADD  CONSTRAINT [FK_HtmlText_Modules] FOREIGN KEY([ModuleID])
REFERENCES [dbo].[mp_Modules] ([ModuleID])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[mp_HtmlContent] CHECK CONSTRAINT [FK_HtmlText_Modules]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Links_Modules]') AND type = 'F')
ALTER TABLE [dbo].[mp_Links]  WITH NOCHECK ADD  CONSTRAINT [FK_Links_Modules] FOREIGN KEY([ModuleID])
REFERENCES [dbo].[mp_Modules] ([ModuleID])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[mp_Links] CHECK CONSTRAINT [FK_Links_Modules]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_ModuleSettings_Modules]') AND type = 'F')
ALTER TABLE [dbo].[mp_ModuleSettings]  WITH NOCHECK ADD  CONSTRAINT [FK_ModuleSettings_Modules] FOREIGN KEY([ModuleID])
REFERENCES [dbo].[mp_Modules] ([ModuleID])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[mp_ModuleSettings] CHECK CONSTRAINT [FK_ModuleSettings_Modules]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_mp_PageModules_mp_Modules]') AND type = 'F')
ALTER TABLE [dbo].[mp_PageModules]  WITH CHECK ADD  CONSTRAINT [FK_mp_PageModules_mp_Modules] FOREIGN KEY([ModuleID])
REFERENCES [dbo].[mp_Modules] ([ModuleID])
GO
ALTER TABLE [dbo].[mp_PageModules] CHECK CONSTRAINT [FK_mp_PageModules_mp_Modules]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_mp_PageModules_mp_PageModules]') AND type = 'F')
ALTER TABLE [dbo].[mp_PageModules]  WITH CHECK ADD  CONSTRAINT [FK_mp_PageModules_mp_PageModules] FOREIGN KEY([PageID])
REFERENCES [dbo].[mp_Pages] ([PageID])
GO
ALTER TABLE [dbo].[mp_PageModules] CHECK CONSTRAINT [FK_mp_PageModules_mp_PageModules]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Modules_ModuleDefinitions]') AND type = 'F')
ALTER TABLE [dbo].[mp_Modules]  WITH NOCHECK ADD  CONSTRAINT [FK_Modules_ModuleDefinitions] FOREIGN KEY([ModuleDefID])
REFERENCES [dbo].[mp_ModuleDefinitions] ([ModuleDefID])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[mp_Modules] CHECK CONSTRAINT [FK_Modules_ModuleDefinitions]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_mp_SitePaths_mp_Sites]') AND type = 'F')
ALTER TABLE [dbo].[mp_SitePaths]  WITH NOCHECK ADD  CONSTRAINT [FK_mp_SitePaths_mp_Sites] FOREIGN KEY([SiteID])
REFERENCES [dbo].[mp_Sites] ([SiteID])
GO
ALTER TABLE [dbo].[mp_SitePaths] CHECK CONSTRAINT [FK_mp_SitePaths_mp_Sites]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Roles_Portals]') AND type = 'F')
ALTER TABLE [dbo].[mp_Roles]  WITH NOCHECK ADD  CONSTRAINT [FK_Roles_Portals] FOREIGN KEY([SiteID])
REFERENCES [dbo].[mp_Sites] ([SiteID])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[mp_Roles] CHECK CONSTRAINT [FK_Roles_Portals]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FK_Tabs_Portals]') AND type = 'F')
ALTER TABLE [dbo].[mp_Pages]  WITH NOCHECK ADD  CONSTRAINT [FK_Tabs_Portals] FOREIGN KEY([SiteID])
REFERENCES [dbo].[mp_Sites] ([SiteID])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[mp_Pages] CHECK CONSTRAINT [FK_Tabs_Portals]
