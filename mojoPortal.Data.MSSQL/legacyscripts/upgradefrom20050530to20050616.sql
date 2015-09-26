
CREATE TABLE [dbo].[mp_FriendlyUrls] (
	[UrlID] [int] IDENTITY (1, 1) NOT NULL ,
	[SiteID] [int] NULL ,
	[FriendlyUrl] [varchar] (255)  NULL ,
	[RealUrl] [varchar] (255)  NULL ,
	[IsPattern] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE  INDEX [idxFriendlyUrl] ON [dbo].[mp_FriendlyUrls]([FriendlyUrl]) ON [PRIMARY]
GO

CREATE  INDEX [idxUserName] ON [dbo].[mp_Users]([Name]) ON [PRIMARY]
GO

 CREATE  INDEX [idxUserEmail] ON [dbo].[mp_Users]([Email]) ON [PRIMARY]
GO

DECLARE @UrlManagerID int
INSERT INTO mp_ModuleDefinitions (FeatureName, ControlSrc, SortOrder, IsAdmin)

VALUES ('Freindly Url Manager','Admin/FriendlyUrlManager.ascx',700,1)

SET @UrlManagerID = @@IDENTITY


INSERT INTO mp_SiteModuleDefinitions (SiteID, ModuleDefID)

VALUES (1, @UrlManagerID)



GO

INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)

VALUES (9,'BlogShowCalendarSetting','false','CheckBox',NULL)


INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (9,'BlogShowArchiveSetting','true','CheckBox',NULL)

INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (9,'BlogShowCategoriesSetting','true','CheckBox',NULL)

INSERT INTO mp_ModuleDefinitionSettings (ModuleDefID, SettingName, SettingValue, ControlType, RegexValidationExpression)
 
VALUES (9,'BlogNavigationOnRightSetting','true','CheckBox',NULL)


GO

DELETE FROM mp_ModuleDefinitionSettings WHERE SettingName = 'BlogMonthsToShow'
GO

DELETE FROM mp_ModuleDefinitionSettings WHERE SettingName = 'BlogCategoriesToShow'
GO

DELETE FROM mp_ModuleDefinitionSettings WHERE SettingName = 'BlogEditorWidthSetting'
GO

DELETE FROM mp_ModuleDefinitionSettings WHERE SettingName = 'HtmlEditorWidthSetting'
GO



DELETE FROM mp_ModuleSettings WHERE SettingName = 'BlogMonthsToShow'
GO

DELETE FROM mp_ModuleSettings WHERE SettingName = 'BlogCategoriesToShow'
GO

DELETE FROM mp_ModuleSettings WHERE SettingName = 'BlogEditorWidthSetting'
GO

DELETE FROM mp_ModuleSettings WHERE SettingName = 'HtmlEditorWidthSetting'
GO
