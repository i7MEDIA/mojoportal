USE [mojodev]
GO
/****** Object:  Table [dbo].[mp_UserEmailAccounts]    Script Date: 06/17/2007 07:56:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_UserEmailAccounts](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_mp_UserEmailAcoounts_ID]  DEFAULT (newid()),
	[UserGuid] [uniqueidentifier] NOT NULL,
	[AccountName] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](75) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Pop3Server] [nvarchar](75) NOT NULL,
	[Pop3Port] [int] NOT NULL CONSTRAINT [DF_mp_UserEmailAcoounts_Pop3Port]  DEFAULT ((110)),
	[SmtpServer] [nvarchar](75) NOT NULL,
	[SmtpPort] [int] NOT NULL CONSTRAINT [DF_mp_UserEmailAcoounts_SmtpPort]  DEFAULT ((25)),
	[UseSSL] [bit] NOT NULL CONSTRAINT [DF_mp_UserEmailAccounts_UseSSL]  DEFAULT ((1)),
 CONSTRAINT [PK_mp_UserEmailAcoounts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mp_PrivateMessageAttachments]    Script Date: 06/17/2007 07:56:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_PrivateMessageAttachments](
	[AttachmentID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_mp_PrivateMessageAttachments_AttachmentID]  DEFAULT (newid()),
	[MessageID] [uniqueidentifier] NOT NULL,
	[OriginalFileName] [nvarchar](255) NOT NULL,
	[ServerFileName] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_mp_PrivateMessageAttachments_CreatedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_mp_PrivateMessageAttachments] PRIMARY KEY CLUSTERED 
(
	[AttachmentID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mp_PrivateMessages]    Script Date: 06/17/2007 07:56:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_PrivateMessages](
	[MessageID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_mp_PrivateMessages_MessageID]  DEFAULT (newid()),
	[FromUser] [uniqueidentifier] NOT NULL,
	[PriorityID] [uniqueidentifier] NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[Body] [ntext] NOT NULL,
	[ToCSVList] [ntext] NULL,
	[CcCSVList] [ntext] NULL,
	[BccCSVList] [ntext] NULL,
	[ToCSVLabels] [ntext] NULL,
	[CcCSVLabels] [ntext] NULL,
	[BccCSVLabels] [ntext] NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_mp_PrivateMessages_CreatedDate]  DEFAULT (getdate()),
	[SentDate] [datetime] NULL,
 CONSTRAINT [PK_mp_PrivateMessages] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mp_PrivateMessagePriority]    Script Date: 06/17/2007 07:56:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mp_PrivateMessagePriority](
	[PriorityID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_mp_PrivateMessagePriority_PriorityID]  DEFAULT (newid()),
	[Priority] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_mp_PrivateMessagePriority] PRIMARY KEY CLUSTERED 
(
	[PriorityID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
