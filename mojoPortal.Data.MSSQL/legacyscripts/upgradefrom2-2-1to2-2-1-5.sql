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

INSERT INTO mp_ModuleDefinitionSettings
( ModuleDefID, SettingName, SettingValue, ControlType)

SELECT
ModuleDefID, 
'BlogShowStatisticsSetting',
'true',
'CheckBox'
FROM mp_ModuleDefinitions
WHERE ControlSrc = "Modules/BlogModule.ascx"

INSERT INTO mp_ModuleDefinitionSettings
( ModuleDefID, SettingName, SettingValue, ControlType)

SELECT
ModuleDefID, 
'BlogShowFeedLinksSetting',
'true',
'CheckBox'
FROM mp_ModuleDefinitions
WHERE ControlSrc = "Modules/BlogModule.ascx"

INSERT INTO mp_ModuleDefinitionSettings
( ModuleDefID, SettingName, SettingValue, ControlType)

SELECT
ModuleDefID, 
'BlogShowAddFeedLinksSetting',
'true',
'CheckBox'
FROM mp_ModuleDefinitions
WHERE ControlSrc = "Modules/BlogModule.ascx"


