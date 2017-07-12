IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaVersion_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SchemaVersion_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectByEmail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SelectByEmail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaVersion_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SchemaVersion_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_ConfirmRegistration]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_ConfirmRegistration]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaVersion_SelectAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SchemaVersion_SelectAll]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaVersion_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SchemaVersion_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SetRegistrationConfirmationGuid]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SetRegistrationConfirmationGuid]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaVersion_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SchemaVersion_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_DecrementTotalPosts]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_DecrementTotalPosts]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_HtmlContent_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_AccountClearLockout]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_AccountClearLockout]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_HtmlContent_SelectByPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_FlagAsDeleted]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_FlagAsDeleted]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_HtmlContent_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_LoginByEmail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_LoginByEmail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_HtmlContent_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_UpdateLastPasswordChangeTime]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_UpdateLastPasswordChangeTime]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_HtmlContent_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_CountOnlineSinceTime]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_CountOnlineSinceTime]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_SelectAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_HtmlContent_SelectAll]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_HtmlContent_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserRoles_SelectNotInRole]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserRoles_SelectNotInRole]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreadSubscriptions_UnsubscribeThread]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreadSubscriptions_UnsubscribeThread]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_UpdatePasswordQuestionAndAnswer]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_UpdatePasswordQuestionAndAnswer]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreadSubscriptions_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreadSubscriptions_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreadSubscriptions_UnsubscribeAllThreads]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreadSubscriptions_UnsubscribeAllThreads]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectByGuid]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SelectByGuid]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreadSubscribers_SelectByThread]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreadSubscribers_SelectByThread]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_AccountLockout]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_AccountLockout]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_UpdateCountOfUseOnMyPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_WebParts_UpdateCountOfUseOnMyPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_WebParts_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectByLoginName]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SelectByLoginName]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_GetWebPartsForMyPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_WebParts_GetWebPartsForMyPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserRoles_SelectByRoleID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserRoles_SelectByRoleID]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_GetMostPopularWebPartsForMyPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_WebParts_GetMostPopularWebPartsForMyPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_Login]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_Login]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_GetCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_WebParts_GetCount]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SmartDropDown]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SmartDropDown]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_SelectBySite]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_WebParts_SelectBySite]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_WebParts_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_UpdateLastActivityTime]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_UpdateLastActivityTime]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_WebParts_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_UpdateLastLoginTime]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_UpdateLastLoginTime]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_WebPartExists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_WebParts_WebPartExists]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_IncrementTotalPosts]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_IncrementTotalPosts]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_WebParts_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_GetNewestUserID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_GetNewestUserID]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Links_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_CountByRegistrationDateRange]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_CountByRegistrationDateRange]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Links_SelectByPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_Count]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_Count]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Links_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SetFailedPasswordAnswerAttemptStartWindow]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SetFailedPasswordAnswerAttemptStartWindow]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Links_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SetFailedPasswordAttemptCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SetFailedPasswordAttemptCount]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Links_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectTop50UsersOnlineSinceTime]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SelectTop50UsersOnlineSinceTime]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Links_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectUsersOnlineSinceTime]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SelectUsersOnlineSinceTime]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleSettings_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleSettings_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SetFailedPasswordAnswerAttemptCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SetFailedPasswordAnswerAttemptCount]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleSettings_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleSettings_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_CountByFirstLetter]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_CountByFirstLetter]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleSettings_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleSettings_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SetFailedPasswordAttemptStartWindow]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SetFailedPasswordAttemptStartWindow]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleSettings_CreateDefaultSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleSettings_CreateDefaultSettings]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserRoles_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserRoles_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleSettings_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleSettings_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserRoles_DeleteUserRoles]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserRoles_DeleteUserRoles]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_DeleteInstance]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Modules_DeleteInstance]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserRoles_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserRoles_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PageModules_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_PageModules_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserPages_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Modules_SelectByPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserPages_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PageModules_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_PageModules_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_GetNextPageOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserPages_GetNextPageOrder]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_UpdateModuleOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Modules_UpdateModuleOrder]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_UpdatePageOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserPages_UpdatePageOrder]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_GetAuthRoles]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Pages_GetAuthRoles]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_SelectByUser]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserPages_SelectByUser]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Modules_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserPages_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_GalleryImages_SelectByPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserPages_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PageModule_Exists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_PageModule_Exists]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_CalendarEvents_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFiles_SelectByPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_CalendarEvents_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_CalendarEvents_SelectByPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_CalendarEvents_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Modules_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_CalendarEvents_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_SelectOneByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Modules_SelectOneByPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents_SelectByDate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_CalendarEvents_SelectByDate]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_UpdatePage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Modules_UpdatePage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreads_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SchemaScriptHistory_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_UpdateViewStats]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreads_UpdateViewStats]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory_SelectErrorsByApp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SchemaScriptHistory_SelectErrorsByApp]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_IncrementReplyCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreads_IncrementReplyCount]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory_Exists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SchemaScriptHistory_Exists]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_SelectOneByHost]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Sites_SelectOneByHost]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SchemaScriptHistory_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Sites_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SchemaScriptHistory_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Sites_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory_SelectByApp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SchemaScriptHistory_SelectByApp]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Sites_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Modules_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_UpdateExtendedProperties]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Sites_UpdateExtendedProperties]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_SelectForMyPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Modules_SelectForMyPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Sites_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_UpdateCountOfUseOnMyPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Modules_UpdateCountOfUseOnMyPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_SelectAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Sites_SelectAll]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Modules_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_Count]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Sites_Count]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_GalleryImages_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogCategories_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_GalleryImages_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogCategories_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_GalleryImages_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogCategories_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_GalleryImages_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteHosts_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SiteHosts_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_GalleryImages_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteHosts_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SiteHosts_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitionSettings_DeleteByID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleDefinitionSettings_DeleteByID]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteHosts_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SiteHosts_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitionSettings_UpdateByID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleDefinitionSettings_UpdateByID]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteHosts_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SiteHosts_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitionSettings_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleDefinitionSettings_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteHosts_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SiteHosts_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitionSettings_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleDefinitionSettings_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogComments_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogComments_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleDefinitions_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleDefinitions_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions_SelectUserModules]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleDefinitions_SelectUserModules]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationPerUser_SetPageSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SitePersonalizationPerUser_SetPageSettings]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleDefinitions_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationAllUsers_SetPageSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SitePersonalizationAllUsers_SetPageSettings]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleDefinitions_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ModuleDefinitions_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_SelectPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Modules_SelectPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_SelectPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationAllUsers_GetPageSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SitePersonalizationAllUsers_GetPageSettings]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_SelectTreeForModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Pages_SelectTreeForModule]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_SelectByForumDesc]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreads_SelectByForumDesc]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationAdministration_FindState]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SitePersonalizationAdministration_FindState]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_SelectByForum]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreads_SelectByForum]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_SelectThumbsByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_GalleryImages_SelectThumbsByPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreads_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_SelectTree]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Pages_SelectTree]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_DecrementPostCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_DecrementPostCount]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_SelectPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_WebParts_SelectPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_SelectWebImageByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_GalleryImages_SelectWebImageByPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_SelectByForumDesc_v2]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreads_SelectByForumDesc_v2]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_DeleteByPageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_FriendlyUrls_DeleteByPageID]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_FriendlyUrls_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_IncrementPostCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_IncrementPostCount]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_FriendlyUrls_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_IncrementPostCountOnly]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_IncrementPostCountOnly]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_SelectBySiteUrl]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_FriendlyUrls_SelectBySiteUrl]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_UpdatePostStats]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_UpdatePostStats]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_FriendlyUrls_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_DecrementThreadCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_DecrementThreadCount]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_FriendlyUrls_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_UpdateThreadStats]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_UpdateThreadStats]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_SelectOneByUrl]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_FriendlyUrls_SelectOneByUrl]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_IncrementThreadCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_IncrementThreadCount]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_SelectByHost]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_FriendlyUrls_SelectByHost]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Blog_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_RecalculatePostStats]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_RecalculatePostStats]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Blog_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Roles_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_SelectByCategory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Blog_SelectByCategory]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Roles_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_SelectByEndDate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Blog_SelectByEndDate]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_GetUserRoles]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Users_GetUserRoles]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Blog_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Roles_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Blog_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_RoleExists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Roles_RoleExists]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogComment_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogComment_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Roles_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogComment_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogComment_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_SelectOneByName]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Roles_SelectOneByName]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_SelectArchiveByMonth]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Blog_SelectArchiveByMonth]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Roles_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Blog_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Pages_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_SelectByMonth]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Blog_SelectByMonth]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteSettings_SelectPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SiteSettings_SelectPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Blog_SelectByPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteSettings_SelectDefaultPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SiteSettings_SelectDefaultPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFileFolders_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteSettings_SelectDefaultPageByID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SiteSettings_SelectDefaultPageByID]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders_SelectAllByModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFileFolders_SelectAllByModule]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Pages_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFileFolders_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Pages_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders_SelectByModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFileFolders_SelectByModule]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_GetBreadcrumbs]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Pages_GetBreadcrumbs]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFileFolders_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_GetNextPageOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Pages_GetNextPageOrder]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFileFolders_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteSettings_GetPageList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SiteSettings_GetPageList]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_SelectList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Pages_SelectList]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreads_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_SelectChildPages]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Pages_SelectChildPages]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumPosts_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_UpdatePageOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Pages_UpdatePageOrder]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_SelectAllByThread]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumPosts_SelectAllByThread]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Pages_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_SelectByThread]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumPosts_SelectByThread]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_RssFeeds_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_RssFeeds_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumPosts_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_RssFeeds_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_RssFeeds_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumPosts_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_RssFeeds_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_RssFeeds_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_DecrementReplyCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreads_DecrementReplyCount]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_RssFeeds_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_RssFeeds_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumPosts_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_RssFeeds_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_RssFeeds_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_CountByThread]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumPosts_CountByThread]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFiles_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_SelectForRss]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumPosts_SelectForRss]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFiles_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreads_SelectByPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles_SelectByModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFiles_SelectByModule]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogItemCategories_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogItemCategories_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFiles_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogItemCategories_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogItemCategories_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFiles_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogItemCategories_SelectByItem]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogItemCategories_SelectByItem]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFilesHistory_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFilesHistory_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogCategories_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFilesHistory_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFilesHistory_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories_SelectListByModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogCategories_SelectListByModule]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFilesHistory_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFilesHistory_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories_SelectByModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogCategories_SelectByModule]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFilesHistory_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SharedFilesHistory_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationAdministration_GetCountOfState]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SitePersonalizationAdministration_GetCountOfState]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserProperties_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationPerUser_GetPageSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SitePersonalizationPerUser_GetPageSettings]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties_PropertyExists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserProperties_PropertyExists]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationPerUser_ResetPageSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SitePersonalizationPerUser_ResetPageSettings]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserProperties_Update]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumThreads_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserProperties_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_Forums_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties_SelectByUser]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserProperties_SelectByUser]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumSubscriptions_Unsubscribe]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumSubscriptions_Unsubscribe]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_UserProperties_SelectOne]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumSubscriptions_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_ForumSubscriptions_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteModuleDefinitions_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SiteModuleDefinitions_Delete]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogStats_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_BlogStats_Select]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteModuleDefinitions_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SiteModuleDefinitions_Insert]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePaths_CreatePath]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[mp_SitePaths_CreatePath]

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_SelectPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Modules_SelectPage]

/*
Author:			
Created:		5/16/2006
Last Modified:	5/20/2006

*/

@SiteID			int,
@PageNumber 	int,
@PageSize 		int,
@SortByModuleType	bit,
@SortByAuthor	bit



AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1



CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	ModuleID int
)	


 IF @SortByModuleType = 1
	BEGIN
	    	INSERT INTO 	#PageIndex (ModuleID)

	    	SELECT 			m.ModuleID 
			FROM 			mp_Modules m
			JOIN			mp_ModuleDefinitions md
			ON				md.ModuleDefID = m.ModuleDefID
			WHERE 			m.SiteID = @SiteID
							AND md.IsAdmin = 0
				
			ORDER BY 		md.FeatureName, m.ModuleTitle

	END
 Else IF @SortByAuthor = 1
	BEGIN
		INSERT INTO 	#PageIndex (ModuleID)

	    	SELECT 			m.ModuleID 
			FROM 			mp_Modules m
			JOIN			mp_ModuleDefinitions md
			ON				md.ModuleDefID = m.ModuleDefID
			LEFT OUTER JOIN	mp_Users u
			ON				m.CreatedByUserID = u.UserID
			WHERE 			m.SiteID = @SiteID
							AND md.IsAdmin = 0
				
			ORDER BY 		u.[Name], m.ModuleTitle
	END
 ELSE
	BEGIN
	    	INSERT INTO 	#PageIndex (ModuleID)

	    	SELECT 			m.ModuleID 
			FROM 			mp_Modules m
			JOIN			mp_ModuleDefinitions md
			ON				md.ModuleDefID = m.ModuleDefID
			WHERE 			m.SiteID = @SiteID
							AND md.IsAdmin = 0
				
			ORDER BY 		m.ModuleTitle

	END

DECLARE @TotalRows int
DECLARE @TotalPages int
DECLARE @Remainder int

SET @TotalRows = (SELECT Count(*) FROM #PageIndex)
SET @TotalPages = @TotalRows / @PageSize
SET @Remainder = @TotalRows % @PageSize
IF @Remainder > 0 
SET @TotalPages = @TotalPages + 1


SELECT		m.*,
			md.FeatureName,
			md.ControlSrc,
			u.[Name] As CreatedBy,
			''TotalPages'' = @TotalPages

FROM			mp_Modules m

JOIN			mp_ModuleDefinitions md
ON				md.ModuleDefID = m.ModuleDefID

LEFT OUTER JOIN	mp_Users u
ON				m.CreatedByUserID = u.UserID

JOIN			#PageIndex p
ON				m.ModuleID = p.ModuleID

WHERE 		
			p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndex
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_SelectTreeForModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Pages_SelectTreeForModule]

/*
Author:			
Created:		5/18/2006
Last Modified:	5/18/2006

*/
        
@SiteID int,
@ModuleID	int

AS
CREATE TABLE #PageTree
(
        [PageID] [int],
        [PageName] [nvarchar] (100),
        [ParentID] [int],
        [PageOrder] [int],
        [NestLevel] [int],
        [TreeOrder] [nvarchar] (1000)
)

SET NOCOUNT ON  
DECLARE @LastLevel smallint
SET @LastLevel = 0
-- First, the parent levels
INSERT INTO     	#PageTree

SELECT  		PageID,
        			PageName,
        			ParentID,
        			PageOrder,
       			0,
        			CAST( 100000000 + PageOrder as nvarchar(20) )

FROM   		mp_Pages

WHERE   		ParentID IS NULL  OR ParentID = -1
			AND SiteID = @SiteID

ORDER BY 		PageOrder

-- Next, the children levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT        #PageTree 
	(
		PageID, 
		PageName, 
		ParentID, 
		PageOrder, 
		NestLevel, 
		TreeOrder
	) 
                
    SELECT  	p.PageID,
                        	Replicate(''-'', @LastLevel *2) + p.PageName,
                        	p.ParentID,
                        	p.PageOrder,
                        	@LastLevel,
                        	CAST(t.TreeOrder as nvarchar) + ''.'' + CAST(100000000 + p.PageOrder as nvarchar)

      FROM    	mp_Pages p
 
      join 		#PageTree t
      on 		p.ParentID= t.PageID

      WHERE   	EXISTS (SELECT ''X'' FROM #PageTree WHERE PageID = p.ParentID AND NestLevel = @LastLevel - 1)
                 	AND SiteID = @SiteID

      ORDER BY 	t.PageOrder


END
--Get the Orphans
  INSERT        	#PageTree 
		(
		PageID, 
		PageName, 
		ParentID, 
		PageOrder, 
		NestLevel, 
		TreeOrder
		) 
                

   SELECT  	p.PageID,
                        	''(Orphan)'' + p.PageName,
                        	p.ParentID,
                        	p.PageOrder,
                        	999999999,
                        	''999999999''
                
    FROM    	mp_Pages p 

    WHERE   	NOT EXISTS (SELECT ''X'' FROM #PageTree WHERE PageID = p.PageID)
                         	AND SiteID  = @SiteID


-- Reorder the Pages by using a 2nd Temp table and an identity field to keep them straight.
SELECT 	IDENTITY(int,1,2) as ord , 
		CAST(PageID as nvarchar) as PageID 

INTO 		#Pages

FROM 		#PageTree

ORDER BY 	NestLevel, TreeOrder

-- Change the taborder in the sirt temp table so that tabs are ordered in sequence
/*
UPDATE 	#PageTree 

SET 		PageOrder = (SELECT ord FROM #Pages WHERE CAST(#Pages.PageID as int) = #PageTree.PageID) 
*/

-- Return Temporary Table
SELECT 	pt.PageID, 
		pt.ParentID, 
		pt.PageName, 
		pt.PageOrder, 
		pt.NestLevel,
		p.PageTitle,
		p.AuthorizedRoles,
		p.EditRoles,
		p.CreateChildPageRoles,
		''IsPublished'' = CONVERT(bit,ISNULL(pm.ModuleID + 1,0)),
		pm.PaneName,
		pm.ModuleOrder,
		pm.PublishBeginDate,
		pm.PublishEndDate

FROM 		#PageTree  pt

JOIN		mp_Pages p
ON		p.PageID = pt.PageID

LEFT OUTER JOIN	mp_PageModules pm
ON		pm.PageID = p.PageID
		AND pm.ModuleID = @ModuleID

ORDER BY 	pt.TreeOrder

DROP TABLE #Pages
DROP TABLE #PageTree
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationAdministration_FindState]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_SitePersonalizationAdministration_FindState] 


@SiteID			int,
@Path 				nvarchar(255) = NULL,
@AllUsersScope 		bit,
@UserID 			uniqueidentifier = NULL,
@InactiveSinceDate 		datetime = NULL,
@PageIndex              		int,
@PageSize               		int



AS

BEGIN
   

    -- Set the page bounds
    DECLARE @PageLowerBound INT
    DECLARE @PageUpperBound INT
    DECLARE @TotalRecords   INT
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table to store the selected results
    CREATE TABLE #PageIndex (
        IndexId int IDENTITY (0, 1) NOT NULL,
        ItemId UNIQUEIDENTIFIER
    )

    IF (@AllUsersScope = 1)
    BEGIN
        -- Insert into our temp table
        INSERT INTO 	#PageIndex (ItemId)

        SELECT 		Paths.PathID
        FROM 		mp_SitePaths Paths,
             			(
			(SELECT 		Paths.PathID
               		FROM 			mp_SitePersonalizationAllUsers AllUsers, mp_SitePaths Paths
               		WHERE 		Paths.SiteID = @SiteID
                      					AND AllUsers.PathId = Paths.PathId
                      					AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              			) AS SharedDataPerPath
       			FULL OUTER JOIN
             		 (		SELECT DISTINCT 	Paths.PathID
               			FROM 			mp_SitePersonalizationPerUser PerUser, mp_SitePaths Paths
               			WHERE 		Paths.SiteID = @SiteID
                      						AND PerUser.PathID = Paths.PathID
                      						AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              			) AS UserDataPerPath
      			ON SharedDataPerPath.PathID = UserDataPerPath.PathID
             		)

        WHERE 		Paths.PathID = SharedDataPerPath.PathID 
			OR Paths.PathID = UserDataPerPath.PathID

        ORDER BY 	Paths.Path ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT 			Paths.Path,
               			SharedDataPerPath.LastUpdate,
               			SharedDataPerPath.DataSize,
               			UserDataPerPath.UserDataLength,
               			UserDataPerPath.UserCount,
				@TotalRecords As TotalRecords

        FROM 			mp_SitePaths Paths,
             (
		(		SELECT 		PageIndex.ItemId AS PathID,
                      						AllUsers.LastUpdatedDate AS LastUpdatedDate,
                      						DATALENGTH(AllUsers.PageSettings) AS DataSize
               			FROM 			mp_SitePersonalizationAllUsers AllUsers, #PageIndex PageIndex
               			WHERE 		AllUsers.PathID = PageIndex.ItemId
                     						AND PageIndex.IndexId >= @PageLowerBound 
							AND PageIndex.IndexId <= @PageUpperBound
              	) 		AS SharedDataPerPath
              
				FULL OUTER JOIN

              	(		SELECT 		PageIndex.ItemId AS PathID,
                      						SUM(DATALENGTH(PerUser.PageSettings)) AS UserDataLength,
                      						COUNT(*) AS UserCount

               			FROM 			mp_SitePersonalizationPerUser PerUser, #PageIndex PageIndex
               			WHERE 		PerUser.PathID = PageIndex.ItemId
                     						AND PageIndex.IndexId >= @PageLowerBound 
							AND PageIndex.IndexId <= @PageUpperBound

               			GROUP BY 		PageIndex.ItemId

              	) 		AS UserDataPerPath

              			ON SharedDataPerPath.PathID = UserDataPerPath.PathID
             )

        WHERE 			Paths.PathID = SharedDataPerPath.PathID 
				OR Paths.PathID = UserDataPerPath.PathID

        ORDER BY 		Paths.Path ASC

    END
    ELSE
    BEGIN
        -- Insert into our temp table
        INSERT INTO 		#PageIndex (ItemId)

        SELECT 			PerUser.[ID]

        FROM 			mp_SitePersonalizationPerUser PerUser, mp_Users Users, mp_SitePaths Paths

        WHERE 			Paths.SiteID = @SiteID
              			AND PerUser.UserID = Users.UserGuid
              			AND PerUser.PathID = Paths.PathID
              			AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
				AND (@UserID = ''{00000000-0000-0000-0000-000000000000}'' OR Users.UserGuid = @UserID)
             
              			AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)

        ORDER BY 		Paths.Path ASC, Users.LoginName ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT 			Paths.Path, 
				PerUser.LastUpdate, 
				DATALENGTH(PerUser.PageSettings) As DataSize, 
				Users.LoginName, 
				Users.LastActivityDate,
				@TotalRecords As TotalRecords
        
        FROM 			mp_SitePersonalizationPerUser PerUser, mp_Users Users, mp_SitePaths Paths, #PageIndex PageIndex
        
        WHERE 			PerUser.[ID] = PageIndex.ItemId
              			AND PerUser.UserID = Users.UserGuid
              			AND PerUser.PathID = Paths.PathID
              			AND PageIndex.IndexId >= @PageLowerBound 
				AND PageIndex.IndexId <= @PageUpperBound

        ORDER BY 		Paths.Path ASC, Users.LoginName ASC

    END

    --RETURN @TotalRecords
END' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_SelectThumbsByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_GalleryImages_SelectThumbsByPage]

/*
Author:			
Created:		12/5/2004
Last Modified:		12/5/2004

*/

@ModuleID			int,
@PageNumber 			int,
@PageSize 			int


AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #PageIndex
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	ItemID int
)	


 
INSERT INTO 	#PageIndex (ItemID)

SELECT	t.ItemID
FROM		mp_GalleryImages t
WHERE	t.ModuleID = @ModuleID	
ORDER BY	t.DisplayOrder, t.ItemID ASC

DECLARE @TotalRows int
DECLARE @TotalPages int
DECLARE @Remainder int

SET @TotalRows = (SELECT Count(*) FROM #PageIndex)
SET @TotalPages = @TotalRows / @PageSize
SET @Remainder = @TotalRows % @PageSize
IF @Remainder > 0 
SET @TotalPages = @TotalPages + 1


SELECT	t.ItemID,
		t.Caption,
		t.ThumbnailFile,
		t.WebImageFile,
		p.IndexID,
		''TotalPages'' = @TotalPages
		


FROM		mp_GalleryImages t

JOIN		#PageIndex p
ON		p.ItemID = t.ItemID



WHERE	t.ModuleID = @ModuleID	
		AND p.IndexID > @PageLowerBound 
		AND p.IndexID < @PageUpperBound

ORDER BY	p.IndexID 

DROP TABLE #PageIndex
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Users_SelectPage]

/*
Author:			
Created:		10/3/2004
Last Modified:		12/20/2006

*/

@PageNumber 			int,
@PageSize 			int,
@UserNameBeginsWith 		nvarchar(1),
@SiteID			int


AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

--SET @PageLowerBound = @PageSize * @PageNumber
--SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #PageIndexForUsers 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	UserName nvarchar(50),
	LoginName nvarchar(50)
)	


 IF @UserNameBeginsWith IS NULL OR @UserNameBeginsWith = ''''
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserName, LoginName)

	    	SELECT 	[Name], LoginName
		FROM 		mp_Users 
		WHERE 	ProfileApproved = 1
				 AND DisplayInMemberList = 1  
				AND SiteID = @SiteID
				AND IsDeleted = 0
		ORDER BY 	[Name]
	END
ELSE
	BEGIN
	    	INSERT INTO 	#PageIndexForUsers (UserName, LoginName)

	    	SELECT 	[Name] , LoginName
		FROM 		mp_Users 
		WHERE 	ProfileApproved = 1 
				AND DisplayInMemberList = 1  
				AND SiteID = @SiteID
				AND IsDeleted = 0
				AND LEFT([Name], 1) = @UserNameBeginsWith 
		ORDER BY 	[Name]

	END

DECLARE @TotalRows int
DECLARE @TotalPages int
DECLARE @Remainder int

SET @TotalRows = (SELECT Count(*) FROM #PageIndexForUsers)
SET @TotalPages = @TotalRows / @PageSize
SET @Remainder = @TotalRows % @PageSize
IF @Remainder > 0 
SET @TotalPages = @TotalPages + 1


SELECT		*,
			''TotalPages'' = @TotalPages

FROM			mp_Users u

JOIN			#PageIndexForUsers p
ON			u.LoginName = p.LoginName

WHERE 		u.ProfileApproved = 1 
			AND u.SiteID = @SiteID
			AND u.IsDeleted = 0
			AND p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndexForUsers
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_SelectTree]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Pages_SelectTree]

/*
Author:			
Created:		11/6/2004
Last Modified:		9/10/2005

*/
        
@SiteID int

AS
CREATE TABLE #PageTree
(
        [PageID] [int],
        [PageName] [nvarchar] (100),
        [ParentID] [int],
        [PageOrder] [int],
        [NestLevel] [int],
        [TreeOrder] [nvarchar] (1000)
)

SET NOCOUNT ON  
DECLARE @LastLevel smallint
SET @LastLevel = 0
-- First, the parent levels
INSERT INTO     	#PageTree

SELECT  		PageID,
        			PageName,
        			ParentID,
        			PageOrder,
       			0,
        			CAST( 100000000 + PageOrder as nvarchar(20) )

FROM   		mp_Pages

WHERE   		ParentID IS NULL  OR ParentID = -1
			AND SiteID = @SiteID

ORDER BY 		PageOrder

-- Next, the children levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT        #PageTree 
	(
		PageID, 
		PageName, 
		ParentID, 
		PageOrder, 
		NestLevel, 
		TreeOrder
	) 
                
    SELECT  	p.PageID,
                        	Replicate(''-'', @LastLevel *2) + p.PageName,
                        	p.ParentID,
                        	p.PageOrder,
                        	@LastLevel,
                        	CAST(t.TreeOrder as nvarchar) + ''.'' + CAST(100000000 + p.PageOrder as nvarchar)

      FROM    	mp_Pages p
 
      join 		#PageTree t
      on 		p.ParentID= t.PageID

      WHERE   	EXISTS (SELECT ''X'' FROM #PageTree WHERE PageID = p.ParentID AND NestLevel = @LastLevel - 1)
                 	AND SiteID = @SiteID

      ORDER BY 	t.PageOrder


END
--Get the Orphans
  INSERT        	#PageTree 
		(
		PageID, 
		PageName, 
		ParentID, 
		PageOrder, 
		NestLevel, 
		TreeOrder
		) 
                

   SELECT  	p.PageID,
                        	''(Orphan)'' + p.PageName,
                        	p.ParentID,
                        	p.PageOrder,
                        	999999999,
                        	''999999999''
                
    FROM    	mp_Pages p 

    WHERE   	NOT EXISTS (SELECT ''X'' FROM #PageTree WHERE PageID = p.PageID)
                         	AND SiteID  = @SiteID


-- Reorder the Pages by using a 2nd Temp table and an identity field to keep them straight.
SELECT 	IDENTITY(int,1,2) as ord , 
		CAST(PageID as nvarchar) as PageID 

INTO 		#Pages

FROM 		#PageTree

ORDER BY 	NestLevel, TreeOrder

-- Change the taborder in the sirt temp table so that tabs are ordered in sequence
/*
UPDATE 	#PageTree 

SET 		PageOrder = (SELECT ord FROM #Pages WHERE CAST(#Pages.PageID as int) = #PageTree.PageID) 
*/

-- Return Temporary Table
SELECT 	pt.PageID, 
		pt.ParentID, 
		pt.PageName, 
		pt.PageOrder, 
		pt.NestLevel,
		p.PageTitle,
		p.AuthorizedRoles,
		p.EditRoles,
		p.CreateChildPageRoles

FROM 		#PageTree  pt

JOIN		mp_Pages p
ON		p.PageID = pt.PageID

ORDER BY 	pt.TreeOrder

DROP TABLE #Pages
DROP TABLE #PageTree
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_SelectPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_WebParts_SelectPage]

/*
Author:			
Created:		6/7/2006
Last Modified:	6/7/2006

*/

@SiteID			int,
@PageNumber 	int,
@PageSize 		int,
@SortByClassName	bit,
@SortByAssemblyName	bit



AS
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int


SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1



CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	WebPartID uniqueidentifier
)	


 IF @SortByClassName = 1
	BEGIN
	    	INSERT INTO 	#PageIndex (WebPartID)

	    	SELECT 			w.WebPartID 
			FROM 			mp_WebParts w
			WHERE 			w.SiteID = @SiteID
			ORDER BY 		w.ClassName, w.Title

	END
 Else IF @SortByAssemblyName = 1
	BEGIN
		INSERT INTO 	#PageIndex (WebPartID)

	    	SELECT 			w.WebPartID 
			FROM 			mp_WebParts w
			WHERE 			w.SiteID = @SiteID
			ORDER BY 		w.AssemblyName, w.Title
	END
 ELSE
	BEGIN
	    	INSERT INTO 	#PageIndex (WebPartID)

	    	SELECT 			w.WebPartID 
			FROM 			mp_WebParts w
			WHERE 			w.SiteID = @SiteID
			ORDER BY 		w.Title, w.ClassName

	END

DECLARE @TotalRows int
DECLARE @TotalPages int
DECLARE @Remainder int

SET @TotalRows = (SELECT Count(*) FROM #PageIndex)
SET @TotalPages = @TotalRows / @PageSize
SET @Remainder = @TotalRows % @PageSize
IF @Remainder > 0 
SET @TotalPages = @TotalPages + 1


SELECT		w.*,
			''TotalPages'' = @TotalPages

FROM			mp_WebParts w

JOIN			#PageIndex p
ON				w.WebPartID = p.WebPartID

WHERE 		
			p.IndexID > @PageLowerBound 
			AND p.IndexID < @PageUpperBound

ORDER BY		p.IndexID

DROP TABLE #PageIndex
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_SelectWebImageByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_GalleryImages_SelectWebImageByPage]

/*
Author:			
Created:		12/5/2004
Last Modified:		12/5/2004

*/

@ModuleID			int,
@PageNumber 			int


AS

DECLARE @PageSize 		int
DECLARE @PageLowerBound 	int
DECLARE @PageUpperBound 	int

SET @PageSize = 1
SET @PageLowerBound = (@PageSize * @PageNumber) - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #PageIndex
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	ItemID int
)	


 
INSERT INTO 	#PageIndex (ItemID)

SELECT	t.ItemID
FROM		mp_GalleryImages t
WHERE	t.ModuleID = @ModuleID	
ORDER BY	t.DisplayOrder, t.ItemID ASC

DECLARE @TotalRows int
DECLARE @TotalPages int
DECLARE @Remainder int

SET @TotalRows = (SELECT Count(*) FROM #PageIndex)
SET @TotalPages = @TotalRows / @PageSize
SET @Remainder = @TotalRows % @PageSize
IF @Remainder > 0 
SET @TotalPages = @TotalPages + 1


SELECT	t.ModuleID,
		t.ItemID,
		''TotalPages'' = @TotalPages
		


FROM		mp_GalleryImages t

JOIN		#PageIndex p
ON		p.ItemID = t.ItemID



WHERE	t.ModuleID = @ModuleID	
		AND p.IndexID > @PageLowerBound 
		AND p.IndexID < @PageUpperBound

ORDER BY	p.IndexID 

DROP TABLE #PageIndex










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_FriendlyUrls_SelectOne]

/*
Author:   			
Created: 			6/1/2005
Last Modified: 			6/1/2005

*/

@UrlID int

AS


SELECT
		[UrlID],
		[SiteID],
		[FriendlyUrl],
		[RealUrl],
		[IsPattern]
		
FROM
		[dbo].[mp_FriendlyUrls]
		
WHERE
		[UrlID] = @UrlID



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_SelectBySiteUrl]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_FriendlyUrls_SelectBySiteUrl]

/*
Author:   			
Created: 			9/10/2006
Last Modified: 		9/10/2006
*/

@SiteID				varchar(100),
@FriendlyUrl		varchar(255)

AS
SELECT *
		
FROM
		[dbo].[mp_FriendlyUrls] u


WHERE	u.SiteID = @SiteID
		AND u.FriendlyUrl = @FriendlyUrl
		 
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_FriendlyUrls_Update]

/*
Author:   			
Created: 			6/1/2005
Last Modified: 			6/1/2005


*/
	
@UrlID int, 
@SiteID int, 
@FriendlyUrl varchar(255), 
@RealUrl varchar(255), 
@IsPattern bit 


AS

UPDATE 		[dbo].[mp_FriendlyUrls] 

SET
			[SiteID] = @SiteID,
			[FriendlyUrl] = @FriendlyUrl,
			[RealUrl] = @RealUrl,
			[IsPattern] = @IsPattern
			
WHERE
			[UrlID] = @UrlID



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_FriendlyUrls_Insert]

/*
Author:   			
Created: 			6/1/2005
Last Modified: 			6/1/2005


*/

@SiteID int,
@FriendlyUrl varchar(255),
@RealUrl varchar(255),
@IsPattern bit

	
AS

INSERT INTO 	[dbo].[mp_FriendlyUrls] 
(
				[SiteID],
				[FriendlyUrl],
				[RealUrl],
				[IsPattern]
) 

VALUES 
(
				@SiteID,
				@FriendlyUrl,
				@RealUrl,
				@IsPattern
				
)
SELECT @@IDENTITY 



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_SelectOneByUrl]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_FriendlyUrls_SelectOneByUrl]

/*
Author:   			
Created: 			6/16/2005
Last Modified: 			6/16/2005
*/

@HostName		varchar(100),
@FriendlyUrl		varchar(255)

AS




SELECT TOP 1
		u.[UrlID],
		u.[SiteID],
		u.[FriendlyUrl],
		u.[RealUrl],
		u.[IsPattern]
		
FROM
		[dbo].[mp_FriendlyUrls] u



WHERE	u.FriendlyUrl = @FriendlyUrl
		AND u.SiteID = COALESCE(
					(SELECT TOP 1 SiteID From mp_SiteHosts WHERE HostName = @HostName),

					(SELECT TOP 1 SiteID From mp_Sites ORDER BY SiteID )
					)



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_SelectByHost]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_FriendlyUrls_SelectByHost]

/*
Author:   			
Created: 			6/1/2005
Last Modified: 			6/1/2005
*/

@HostName		varchar(100)

AS




SELECT
		[UrlID],
		[SiteID],
		[FriendlyUrl],
		[RealUrl],
		[IsPattern]
		
FROM
		[dbo].[mp_FriendlyUrls]

WHERE	SiteID = COALESCE(
					(SELECT TOP 1 SiteID From mp_SiteHosts WHERE HostName = @HostName),

					(SELECT TOP 1 SiteID From mp_Sites ORDER BY SiteID )
					)



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_DeleteByPageID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_FriendlyUrls_DeleteByPageID]

/*
Author:   			
Created: 			6/1/2005
Last Modified: 			6/1/2005

*/

@PageID int

AS

DELETE FROM [dbo].[mp_FriendlyUrls]
WHERE
	RealUrl LIKE ''%pageid='' + CONVERT(varchar(255), @PageID) + ''%''
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_FriendlyUrls_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_FriendlyUrls_Delete]

/*
Author:   			
Created: 			6/1/2005
Last Modified: 			6/1/2005

*/

@UrlID int

AS

DELETE FROM [dbo].[mp_FriendlyUrls]
WHERE
	[UrlID] = @UrlID



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_SelectByCategory]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Blog_SelectByCategory]

/*
Author:			
Created:		6/12/2005
Last Modified:		6/12/2005

*/



@ModuleID 		int,
@CategoryID		int


AS

SELECT	b.*

		

FROM 		mp_Blogs b

WHERE 	b.ModuleID = @ModuleID
		AND b.ItemID IN (SELECT ItemID FROM mp_BlogItemCategories WHERE CategoryID = @CategoryID)


ORDER BY	 b.StartDate DESC



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_SelectByEndDate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Blog_SelectByEndDate]

/*
Author:		
Created:	6/5/2005
Last Modified:	6/12/2005

*/
    
@ModuleID int,
@EndDate datetime

AS

DECLARE @RowsToGet int

SET @RowsToGet = COALESCE((SELECT TOP 1 SettingValue FROM mp_ModuleSettings WHERE SettingName = ''BlogEntriesToShowSetting'' AND ModuleID = @ModuleID),10)

SET rowcount @RowsToGet

SELECT		b.ItemID, 
			b.ModuleID, 
			b.CreatedByUser, 
			b.CreatedDate, 
			b.Title, 
			b.Excerpt, 
			b.[Description], 
			b.StartDate,
			b.IsInNewsletter, 
			b.IncludeInFeed,
			''CommentCount'' = CONVERT(nvarchar(20), b.CommentCount)
			

FROM        		mp_Blogs b

WHERE
    			(b.ModuleID = @ModuleID)  and (@EndDate >= b.StartDate)

ORDER BY
   			b.ItemID DESC,  b.StartDate DESC


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Blog_Insert]

/*
Author:			
Last Modified:		8/7/2005

*/

    
@ModuleID       		int,
@UserName       	nvarchar(100),
@Title          		nvarchar(100),
@Excerpt	    	nvarchar(512),
@Description    		ntext,
@StartDate      		datetime,
@IsInNewsletter 	bit,
@IncludeInFeed		bit,
@ItemID         		int OUTPUT



AS



INSERT INTO 		mp_Blogs
(

    			ModuleID,
    			CreatedByUser,
    			CreatedDate,
    			Title,
    			Excerpt,
			[Description],
			StartDate,
			IsInNewsletter,
			IncludeInFeed
			


)

VALUES
(

    		@ModuleID,
    		@UserName,
    		GetDate(),
    		@Title,
    		@Excerpt,
    		@Description,
    		@StartDate,
    		@IsInNewsletter,
		@IncludeInFeed
    		



)



SELECT

    @ItemID = @@Identity


IF EXISTS(SELECT ModuleID FROM mp_BlogStats WHERE ModuleID = @ModuleID)
	BEGIN
		UPDATE mp_BlogStats
		SET 	EntryCount = EntryCount + 1
		WHERE ModuleID = @ModuleID

	END
ELSE
	BEGIN
		INSERT INTO mp_BlogStats(ModuleID, EntryCount)
		VALUES (@ModuleID, 1)


	END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'














CREATE PROCEDURE [dbo].[mp_Blog_Delete]
(
    @ItemID int
)
AS

DECLARE @ModuleID int
SET @ModuleID = (SELECT TOP 1 ModuleID FROM mp_Blogs WHERE ItemID = @ItemID)

DELETE FROM
    mp_Blogs

WHERE
    ItemID = @ItemID

UPDATE mp_BlogStats
SET 	EntryCount = EntryCount - 1
WHERE ModuleID = @ModuleID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_SelectByMonth]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Blog_SelectByMonth]

(
	@Month int,
	@Year int,
	@ModuleID int
)

AS

SELECT	b.*

		

FROM 		mp_Blogs b

WHERE 	b.ModuleID = @ModuleID
		AND Month(b.StartDate)  = @Month 
		AND Year(b.StartDate)  = @Year


ORDER BY	 b.StartDate DESC



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Blog_SelectByPage]

/*
Author:			Joe Audettte
Created:		6/30/2005
Last Modified:		6/30/2005

*/


@SiteID		int,
@PageID		int

AS
SELECT  	b.*,
		m.ModuleTitle,
		md.FeatureName

FROM		mp_Blogs b

JOIN		mp_Modules m
ON		b.ModuleID = m.ModuleID

JOIN		mp_ModuleDefinitions md
ON		m.ModuleDefID = md.ModuleDefID

JOIN		mp_PageModules pm
ON			pm.ModuleID = m.ModuleID

JOIN		mp_Pages p
ON		p.PageID = pm.PageID

WHERE	p.SiteID = @SiteID
		AND pm.PageID = @PageID
		AND pm.PublishBeginDate < GetDate()
		AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > GetDate())
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Blog_Update]

/*
Author:			
Last Modified:		6/30/2005

*/





@ItemID         		int,
@ModuleID       		int,
@UserName       	nvarchar(100),
@Title          		nvarchar(100),
@Excerpt       		nvarchar(512),
@Description    		ntext,
@StartDate      		datetime,
@IsInNewsletter 	bit,
@IncludeInFeed		bit

   
AS



UPDATE mp_Blogs



SET 

		ModuleID = @ModuleID,
		CreatedByUser = @UserName,
		CreatedDate = GetDate(),
		Title =@Title ,
		Excerpt =@Excerpt,
		[Description] = @Description,
		StartDate = @StartDate,
		IsInNewsletter = @IsInNewsletter,
		IncludeInFeed = @IncludeInFeed
		



WHERE 

		ItemID = @ItemID



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogComment_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'












CREATE PROCEDURE [dbo].[mp_BlogComment_Delete]
(
    @BlogCommentID int
)
AS

DECLARE @ModuleID int
DECLARE @ItemID int

SELECT @ModuleID = ModuleID, @ItemID = ItemID
FROM	mp_BlogComments
WHERE BlogCommentID = @BlogCommentID

DELETE FROM
    mp_BlogComments

WHERE
    BlogCommentID = @BlogCommentID



UPDATE mp_Blogs
SET CommentCount = CommentCount - 1
WHERE ModuleID = @ModuleID AND ItemID = @ItemID

UPDATE mp_BlogStats
SET 	CommentCount = CommentCount - 1
WHERE ModuleID = @ModuleID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_SelectArchiveByMonth]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'












CREATE PROCEDURE [dbo].[mp_Blog_SelectArchiveByMonth]


(
	@ModuleID int
)

AS

SELECT 	Month(StartDate) as [Month], 
		DATENAME(month,StartDate) as [MonthName],
		Year(StartDate) as [Year], 
		1 as Day, 
		Count(*) as [Count] 

FROM 		mp_Blogs
 
WHERE 	ModuleID = @ModuleID 

GROUP BY 	Year(StartDate), 
		Month(StartDate) ,
		DATENAME(month,StartDate)

ORDER BY 	[Year] desc, [Month] desc











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogComment_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_BlogComment_Insert]

@ModuleID       int,
@ItemID	int,
@Name       nvarchar(100),
@Title          nvarchar(100),
@URL       nvarchar(200),
@Comment    ntext,
@DateCreated	datetime


AS
INSERT INTO mp_BlogComments

(

    ModuleID,
	ItemID,
    [Name],
    Title,
	URL,
    Comment,
	DateCreated

)

VALUES
(

    @ModuleID,
    @ItemID,
   @Name,
    @Title,
    @URL,
    @Comment,
    @DateCreated

)



UPDATE mp_Blogs
SET CommentCount = CommentCount + 1
WHERE ModuleID = @ModuleID AND ItemID = @ItemID


UPDATE mp_BlogStats
SET 	CommentCount = CommentCount + 1
WHERE ModuleID = @ModuleID
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Blog_SelectOne]

/*
Author:			
Last Modified:		7/1/2005

*/

    
@ItemID int

AS

SELECT		*
			
			
FROM			mp_Blogs


WHERE   		(ItemID = @ItemID)



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Blog_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Blog_Select]

/*
Author:		
Last Modified:	6/12/2005

*/
    
@ModuleID int,
@BeginDate datetime

AS
DECLARE @RowsToGet int

SET @RowsToGet = COALESCE((SELECT TOP 1 SettingValue FROM mp_ModuleSettings WHERE SettingName = ''BlogEntriesToShowSetting'' AND ModuleID = @ModuleID),1)

SET rowcount @RowsToGet

SELECT		b.ItemID, 
			b.ModuleID, 
			b.CreatedByUser, 
			b.CreatedDate, 
			b.Title, 
			b.Excerpt, 
			b.[Description], 
			b.StartDate,
			b.IsInNewsletter, 
			b.IncludeInFeed,
			''CommentCount'' = CONVERT(nvarchar(20), b.CommentCount)
			

FROM        		mp_Blogs b

WHERE
    			(b.ModuleID = @ModuleID)  and (@BeginDate >= b.StartDate)

ORDER BY
   			b.ItemID DESC,  b.StartDate DESC
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders_SelectAllByModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFileFolders_SelectAllByModule]

/*
Author:   			
Created: 			1/5/2005
Last Modified: 			1/9/2005
*/

@ModuleID		int


AS


SELECT
		[FolderID],
		ModuleID,
		[FolderName],
		[ParentID]
		
FROM
		[dbo].[mp_SharedFileFolders]

WHERE	ModuleID = @ModuleID
		










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFileFolders_Insert]

/*
Author:   			
Created: 			1/5/2005
Last Modified: 			1/5/2005


*/

@ModuleID	int,
@FolderName 	nvarchar(255),
@ParentID 	int

	
AS

INSERT INTO 	[dbo].[mp_SharedFileFolders] 
(
				ModuleID,
				[FolderName],
				[ParentID]
) 

VALUES 
(
				@ModuleID,
				@FolderName,
				@ParentID
				
)
SELECT @@IDENTITY 










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders_SelectByModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFileFolders_SelectByModule]

/*
Author:   			
Created: 			1/5/2005
Last Modified: 			1/5/2005
*/

@ModuleID		int,
@ParentID		int


AS


SELECT
		[FolderID],
		[FolderName],
		[ParentID]
		
FROM
		[dbo].[mp_SharedFileFolders]

WHERE	ModuleID = @ModuleID
		AND ParentID = @ParentID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFileFolders_Update]

/*
Author:   			
Created: 			1/5/2005
Last Modified: 			1/5/2005
*/
	
@FolderID int, 
@ModuleID	int,
@FolderName nvarchar(255), 
@ParentID int 


AS

UPDATE 		[dbo].[mp_SharedFileFolders] 

SET
			ModuleID = @ModuleID,
			[FolderName] = @FolderName,
			[ParentID] = @ParentID
			
WHERE
			[FolderID] = @FolderID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFileFolders_Delete]

/*
Author:   			
Created: 			1/5/2005
Last Modified: 			1/5/2005


*/

@FolderID int

AS

DELETE FROM [dbo].[mp_SharedFileFolders]
WHERE
	[FolderID] = @FolderID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFileFolders_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFileFolders_SelectOne]

/*
Author:   			
Created: 			1/5/2005
Last Modified: 			1/8/2005

*/

@FolderID int

AS


SELECT
		[FolderID],
		ModuleID,
		[FolderName],
		[ParentID]
		
FROM
		[dbo].[mp_SharedFileFolders]
		
WHERE
		[FolderID] = @FolderID










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Forums_Delete]

/*
Author:				
Created:			6/27/2006
Last Modified:		8/14/2006

*/

@ItemID			int

AS

DELETE FROM mp_ForumPosts

WHERE ThreadID IN (SELECT ThreadID FROM mp_ForumThreads
					WHERE ForumID = @ItemID)

DELETE
FROM mp_ForumThreads
WHERE ForumID = @ItemID


DELETE
FROM			mp_Forums 

WHERE		ItemID = @ItemID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_ForumThreads_Delete]

/*
Author:			
Created:		11/28/2004
Last Modified:	8/14/2006

*/

@ThreadID			int

AS

DELETE FROM mp_ForumPosts

WHERE	ThreadID = @ThreadID

DELETE FROM 		mp_ForumThreads


WHERE		ThreadID = @ThreadID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumPosts_SelectOne]

/*
Author:				
Created:			10/17/2004
Last Modified:			10/17/2004

*/

@PostID		int

AS


SELECT	fp.*

FROM		mp_ForumPosts fp

WHERE	fp.PostID = @PostID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_SelectAllByThread]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'




CREATE PROCEDURE [dbo].[mp_ForumPosts_SelectAllByThread]

/*
Author:				
Created:			2/19/2005
Last Modified:			9/11/2005

An approach to paging grids in the database, hopefully more efficient than bringing back a zillion rows and
paging with a DataGrid using viewstate

ThreadSequence is the integer order that postss were created within the thread

*/

@ThreadID			int


AS


SELECT	p.*,
		ft.ForumID,
		''MostRecentPostUser'' = COALESCE(u.[Name],''Guest''),
		''StartedBy'' = COALESCE(s.[Name],''Guest''),
		''PostAuthor'' = COALESCE(up.[Name], ''Guest''),
		''PostAuthorTotalPosts'' = up.TotalPosts,
		''PostAuthorAvatar'' = up.AvatarUrl,
		''PostAuthorWebSiteUrl'' = up.WebSiteURL,
		''PostAuthorSignature'' = up.Signature


FROM		mp_ForumPosts p

JOIN		mp_ForumThreads ft
ON		p.ThreadID = ft.ThreadID

LEFT OUTER JOIN		mp_Users u
ON		ft.MostRecentPostUserID = u.UserID

LEFT OUTER JOIN		mp_Users s
ON		ft.StartedByUserID = s.UserID

LEFT OUTER JOIN		mp_Users up
ON		up.UserID = p.UserID

WHERE	ft.ThreadID = @ThreadID
		

ORDER BY	p.SortOrder, p.PostID DESC



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_SelectByThread]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumPosts_SelectByThread]

/*
Author:				
Created:			9/14/2004
Last Modified:			9/11/2005

An approach to paging grids in the database, hopefully more efficient than bringing back a zillion rows and
paging with a DataGrid using viewstate

ThreadSequence is the integer order that postss were created within the thread

*/

@ThreadID			int,
@PageNumber			int

AS

DECLARE @PostsPerPage	int
--DECLARE @TotalPosts		int
DECLARE @CurrentPageMaxThreadSequence	int

DECLARE @BeginSequence int
DECLARE @EndSequence int



SELECT	@PostsPerPage = f.PostsPerPage
		--@TotalPosts = ft.TotalReplies

FROM		mp_ForumThreads ft

JOIN		mp_Forums f
ON		ft.ForumID = f.ItemID

WHERE	ft.ThreadID = @ThreadID

SET @CurrentPageMaxThreadSequence = (@PostsPerPage * @PageNumber) 
IF @CurrentPageMaxThreadSequence > @PostsPerPage
	BEGIN
		SET @BeginSequence = @CurrentPageMaxThreadSequence  - @PostsPerPage + 1
	END
ELSE
	BEGIN
		SET @BeginSequence = 1
	END

SET @EndSequence = @BeginSequence + @PostsPerPage  -1



SELECT	p.*,
		ft.ForumID,
		''MostRecentPostUser'' = COALESCE(u.[Name],''Guest''),
		''StartedBy'' = COALESCE(s.[Name],''Guest''),
		''PostAuthor'' = COALESCE(up.[Name], ''Guest''),
		''PostAuthorTotalPosts'' = up.TotalPosts,
		''PostAuthorAvatar'' = up.AvatarUrl,
		''PostAuthorWebSiteUrl'' = up.WebSiteURL,
		''PostAuthorSignature'' = up.Signature


FROM		mp_ForumPosts p

JOIN		mp_ForumThreads ft
ON		p.ThreadID = ft.ThreadID

LEFT OUTER JOIN		mp_Users u
ON		ft.MostRecentPostUserID = u.UserID

LEFT OUTER JOIN		mp_Users s
ON		ft.StartedByUserID = s.UserID

LEFT OUTER JOIN		mp_Users up
ON		up.UserID = p.UserID

WHERE	ft.ThreadID = @ThreadID
		AND p.ThreadSequence >= @BeginSequence
		AND  p.ThreadSequence <= @EndSequence

ORDER BY	p.SortOrder, p.PostID



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_ForumPosts_Delete]

/*
Author:			
Created:		11/6/2004
Last Modified:		11/6/2004

*/

@PostID		int


AS

DELETE FROM mp_ForumPosts

WHERE PostID = @PostID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumPosts_Update]

/*
Author:			
Created:		9/19/2004
Last Modified:		9/19/2004

*/

@PostID			int,
@Subject			nvarchar(255),
@Post				ntext,
@SortOrder			int,
@Approved			bit

AS

UPDATE		mp_ForumPosts

SET			Subject = @Subject,
			Post = @Post,
			SortOrder = @SortOrder,
			Approved = @Approved


WHERE		PostID = @PostID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_DecrementReplyCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_ForumThreads_DecrementReplyCount]

/*
Author:			
Created:		2/19/2005
Last Modified:		2/19/2005

*/

@ThreadID			int


AS

DECLARE @MostRecentPostUserID int
DECLARE @MostRecentPostDate datetime
 
SELECT TOP 1  @MostRecentPostUserID = UserID,
		@MostRecentPostDate = PostDate

FROM mp_ForumPosts 
WHERE ThreadID = @ThreadID 
ORDER BY PostID DESC


UPDATE		mp_ForumThreads

SET			MostRecentPostUserID = @MostRecentPostUserID,
			TotalReplies = TotalReplies - 1,
			MostRecentPostDate = @MostRecentPostDate


WHERE		ThreadID = @ThreadID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_ForumPosts_Insert]

/*
Author:			
Created:		9/19/2004
Last Modified:		1/14/2007

*/


@ThreadID			int,
@Subject			nvarchar(255),
@Post				ntext,
@Approved			bit,
@UserID			int,
@PostDate		datetime



AS
DECLARE @ThreadSequence int
SET @ThreadSequence = (SELECT COALESCE(Max(ThreadSequence) + 1,1) FROM mp_ForumPosts WHERE ThreadID = @ThreadID)



INSERT INTO		mp_ForumPosts
(
			ThreadID,
			Subject,
			Post,
			Approved,
			UserID,
			ThreadSequence,
			PostDate
)

VALUES
(
			@ThreadID,
			@Subject,
			@Post,
			@Approved,
			@UserID,
			@ThreadSequence,
			@PostDate

)

SELECT  @@IDENTITY As PostID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_CountByThread]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_ForumPosts_CountByThread]

/*
Author:				
Created:			11/28/2004
Last Modified:			11/28/2004

*/


@ThreadID		int



AS


SELECT	COUNT(*)

FROM		mp_ForumPosts

WHERE	ThreadID = @ThreadID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumPosts_SelectForRss]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_ForumPosts_SelectForRss]

/*
Author:				Joseph Hill
Created:			11/01/2005
Last Modified:			11/04/2005

A list of all forums posts (and associated data) filtered by the specified criteria

MaximumDays is the maximum age in days of the posts to return

*/
@SiteID				int,
@PageID				int,
@ModuleID			int,
@ItemID				int,
@ThreadID			int,
@MaximumDays			int


AS
SELECT		fp.*,
		ft.ThreadSubject,
		ft.ForumID,
		''StartedBy'' = COALESCE(s.[Name],''Guest''),
		''PostAuthor'' = COALESCE(up.[Name], ''Guest''),
		''PostAuthorTotalPosts'' = up.TotalPosts,
		''PostAuthorAvatar'' = up.AvatarUrl,
		''PostAuthorWebSiteUrl'' = up.WebSiteURL,
		''PostAuthorSignature'' = up.Signature


FROM		mp_ForumPosts fp

JOIN		mp_ForumThreads ft
ON		fp.ThreadID = ft.ThreadID

JOIN		mp_Forums f
ON		ft.ForumID = f.ItemID

JOIN		mp_Modules m
ON		f.ModuleID = m.ModuleID

JOIN	mp_PageModules pm
ON	    m.ModuleID = pm.ModuleID

JOIN		mp_Pages p
ON		pm.PageID = p.PageID

LEFT OUTER JOIN		mp_Users u
ON		ft.MostRecentPostUserID = u.UserID

LEFT OUTER JOIN		mp_Users s
ON		ft.StartedByUserID = s.UserID

LEFT OUTER JOIN		mp_Users up
ON		up.UserID = fp.UserID

WHERE	p.SiteID = @SiteID
AND	(@PageID = -1 OR p.PageID = @PageID)
AND	(@ModuleID = -1 OR m.ModuleID = @ModuleID)
AND	(@ItemID = -1 OR f.ItemID = @ItemID)
AND	(@ThreadID = -1 OR ft.ThreadID = @ThreadID)
AND	(@MaximumDays = -1 OR datediff(dd, getdate(), fp.PostDate) <= @MaximumDays)

ORDER BY	fp.PostDate DESC
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_ForumThreads_SelectByPage]

/*
Author:			Joe Audettte
Created:		7/2/2005
Last Modified:		7/2/2005

*/


@SiteID		int,
@PageID		int

AS
SELECT  	fp.*,
		f.ModuleID,
		f.ItemID,
		m.ModuleTitle,
		md.FeatureName

FROM		mp_ForumPosts fp

JOIN		mp_ForumThreads ft
ON		fp.ThreadID = ft.ThreadID

JOIN		mp_Forums f
ON		f.ItemID = ft.ForumID

JOIN		mp_Modules m
ON		f.ModuleID = m.ModuleID

JOIN		mp_ModuleDefinitions md
ON		m.ModuleDefID = md.ModuleDefID

JOIN		mp_PageModules pm
ON			pm.ModuleID = m.ModuleID

JOIN		mp_Pages p
ON		p.PageID = pm.PageID

WHERE	p.SiteID = @SiteID
		AND pm.PageID = @PageID
		AND pm.PublishBeginDate < GetDate()
		AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > GetDate())
		AND fp.Approved = 1
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories_SelectByModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_BlogCategories_SelectByModule]

/*
Author:   			
Created: 			6/7/2005
Last Modified: 			9/11/2005
*/

@ModuleID			int

AS


SELECT
				bc.CategoryID, 
				bc.Category,
				COUNT(bic.ItemID) As PostCount
		
		
FROM
			[dbo].[mp_BlogCategories] bc

JOIN			mp_BlogItemCategories bic
ON 			bc.CategoryID = bic.CategoryID

WHERE		bc.ModuleID = @ModuleID
			

GROUP BY		bc.CategoryID, bc.Category

ORDER BY		bc.Category



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories_SelectListByModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_BlogCategories_SelectListByModule]

/*
Author:   			
Created: 			9/11/2005
Last Modified: 			9/11/2005
*/

@ModuleID			int

AS


SELECT
				bc.CategoryID, 
				bc.Category,
				COUNT(bic.ItemID) As PostCount
		
		
FROM
			[dbo].[mp_BlogCategories] bc

LEFT OUTER JOIN	mp_BlogItemCategories bic
ON 			bc.CategoryID = bic.CategoryID

WHERE		bc.ModuleID = @ModuleID
			

GROUP BY		bc.CategoryID, bc.Category

ORDER BY		bc.Category



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogItemCategories_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_BlogItemCategories_Insert]

/*
Author:   			
Created: 			6/7/2005
Last Modified: 			6/7/2005


*/

@ItemID int,
@CategoryID int

	
AS

INSERT INTO 	[dbo].[mp_BlogItemCategories] 
(
				[ItemID],
				[CategoryID]
) 

VALUES 
(
				@ItemID,
				@CategoryID
				
)
SELECT @@IDENTITY 



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogItemCategories_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_BlogItemCategories_Delete]

/*
Author:   			
Created: 			6/7/2005
Last Modified: 			6/7/2005

*/

@ItemID int

AS

DELETE FROM [dbo].[mp_BlogItemCategories]
WHERE
	[ItemID] = @ItemID



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogItemCategories_SelectByItem]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_BlogItemCategories_SelectByItem]

/*
Author:   			
Created: 			6/7/2005
Last Modified: 			6/7/2005
*/

@ItemID	int

AS


SELECT
		bic.[ID],
		bic.[ItemID],
		bic.[CategoryID],
		bc.Category
		
FROM
		[dbo].[mp_BlogItemCategories] bic

JOIN		mp_BlogCategories bc
ON		bc.CategoryID = bic.CategoryID

WHERE	ItemID = @ItemID

ORDER BY	bc.Category



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_BlogCategories_Delete]

/*
Author:   			
Created: 			6/7/2005
Last Modified: 			6/12/2005

*/

@CategoryID int

AS

DELETE FROM mp_BlogItemCategories
WHERE	CategoryID = @CategoryID

DELETE FROM [dbo].[mp_BlogCategories]
WHERE
	[CategoryID] = @CategoryID



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationPerUser_ResetPageSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_SitePersonalizationPerUser_ResetPageSettings]
 (
    @SiteID		int,
    @Path             	NVARCHAR(255),
    @UserId        	UNIQUEIDENTIFIER

)
AS
BEGIN
    
    DECLARE @PathId UNIQUEIDENTIFIER
    SELECT @PathId = NULL
   

    SELECT @PathId = u.PathId FROM mp_SitePaths u WHERE u.SiteID = @SiteID AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END


    DELETE FROM mp_SitePersonalizationPerUser WHERE PathID = @PathId AND UserId = @UserId
    RETURN 0
END' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationPerUser_GetPageSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_SitePersonalizationPerUser_GetPageSettings] 
(
   
@SiteID		int,
@Path             	NVARCHAR(255),
@UserId        	UNIQUEIDENTIFIER


)

AS
BEGIN
   
    DECLARE @PathId UNIQUEIDENTIFIER
    SELECT @PathId = NULL
    
    SELECT @PathId = u.PathID 
    FROM mp_SitePaths u WHERE u.SiteID = @SiteID AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END


    SELECT p.PageSettings 
	FROM mp_SitePersonalizationPerUser p 
	WHERE p.PathID = @PathId AND p.UserId = @UserId

END
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationAdministration_GetCountOfState]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_SitePersonalizationAdministration_GetCountOfState] 

@SiteID		int,
@Path 			nvarchar(255) = NULL,
@AllUsersScope 	bit,
@UserID 		uniqueidentifier ,
@InactiveSinceDate 	datetime = NULL


AS

BEGIN

    
        IF (@AllUsersScope = 1)
            SELECT 		COUNT(*)
            FROM 			mp_SitePersonalizationAllUsers spa
	JOIN			mp_SitePaths sp
	ON 			spa.PathID = sp.PathID
            	WHERE 		sp.SiteID = @SiteID
                  			AND (@Path IS NULL OR sp.LoweredPath LIKE LOWER(@Path))
        ELSE
            	SELECT 		COUNT(*)
            	FROM 			mp_SitePersonalizationPerUser spu
	JOIN	 		mp_SitePaths sp
	ON 			spu.PathID = sp.PathID
	LEFT OUTER JOIN	mp_Users u
	ON 			spu.UserID = u.UserGuid

            WHERE 		sp.SiteID = @SiteID
                  			AND (@Path IS NULL OR sp.LoweredPath LIKE LOWER(@Path))
                  			AND (@UserID = ''{00000000-0000-0000-0000-000000000000}'' OR u.UserGuid = @UserID)
                 			 AND (@InactiveSinceDate IS NULL OR u.LastActivityDate <= @InactiveSinceDate)
END' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_ForumThreads_Insert]

/*
Author:			
Created:		9/19/2004
Last Modified:		1/14/2007

*/

@ForumID			int,
@ThreadSubject		nvarchar(255),
@SortOrder			int,
@IsLocked			bit,
@StartedByUserID		int,
@ThreadDate		datetime


AS
DECLARE @ThreadID int
DECLARE @ForumSequence int
SET @ForumSequence = (SELECT COALESCE(Max(ForumSequence) + 1,1) FROM mp_ForumThreads WHERE ForumID = @ForumID)

INSERT INTO		mp_ForumThreads
(
			ForumID,
			ThreadSubject,
			SortOrder,
			ForumSequence,
			IsLocked,
			StartedByUserID,
			ThreadDate,
			MostRecentPostUserID,
			MostRecentPostDate

)

VALUES
(
			
			@ForumID,
			@ThreadSubject,
			@SortOrder,
			@ForumSequence,
			@IsLocked,
			@StartedByUserID,
			@ThreadDate,
			@StartedByUserID,
			@ThreadDate


)

SELECT @ThreadID = @@IDENTITY 


INSERT INTO mp_ForumThreadSubscriptions (ThreadID, UserID)
	SELECT @ThreadID, UserID FROM mp_ForumSubscriptions fs 
		WHERE fs.ForumID = @ForumID AND fs.SubscribeDate IS NOT NULL 
				AND fs.UnSubscribeDate IS NULL

SELECT @ThreadID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumSubscriptions_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_ForumSubscriptions_Insert]

/*
Author:			Dean Brettle
Created:		9/11/2005
Last Modified:		9/11/2005

*/

@ForumID		int,
@UserID		int


AS

IF EXISTS (SELECT UserID FROM mp_ForumSubscriptions WHERE ForumID = @ForumID AND UserID = @UserID)
BEGIN
	UPDATE 	mp_ForumSubscriptions

	SET		SubscribeDate = GetDate(),
			UnSubscribeDate = Null
	

	WHERE 	ForumID = @ForumID AND UserID = @UserID

END

ELSE

BEGIN

	INSERT INTO	mp_ForumSubscriptions
	(
			ForumID,
			UserID
	)
	VALUES
	(
			@ForumID,
			@UserID
	)

END











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumSubscriptions_Unsubscribe]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumSubscriptions_Unsubscribe]

/*
Author:				Dean Brettle
Created:			9/11/2005
Last Modified:			9/11/2005

*/

@ForumID		int,
@UserID		int

AS

UPDATE		mp_ForumSubscriptions

SET			UnSubscribeDate = GetDate()

WHERE		ForumID = @ForumID
			AND UserID = @UserID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_Forums_Select]


/*
Author:			
Created:		9/12/2004
Last Modified:		9/12/2004

*/

@ModuleID			int,
@UserID				int

AS


SELECT		f.*,
			''MostRecentPostUser'' = u.[Name],
			''Subscribed'' = CASE WHEN s.[SubscribeDate] IS NOT NULL and s.[UnSubscribeDate] IS NULL THEN 1
					Else 0
					End

FROM			mp_Forums f

LEFT OUTER JOIN	mp_Users u
ON			f.MostRecentPostUserID = u.UserID

LEFT OUTER JOIN mp_ForumSubscriptions s
on			f.ItemID = s.ForumID and s.UserID = @UserID

WHERE		f.ModuleID	= @ModuleID
			AND f.IsActive = 1

ORDER BY		f.SortOrder, f.ItemID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogStats_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'












CREATE PROCEDURE [dbo].[mp_BlogStats_Select]
(
    @ModuleID int
)
AS

SELECT		
			ModuleID, 
			EntryCount,
			CommentCount

FROM       		 mp_BlogStats

WHERE
    			ModuleID = @ModuleID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaVersion_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[mp_SchemaVersion_Update]

/*
Author:   			
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/
	
@ApplicationID uniqueidentifier, 
@ApplicationName nvarchar(255), 
@Major int, 
@Minor int, 
@Build int, 
@Revision int 


AS

UPDATE 		[dbo].[mp_SchemaVersion] 

SET
			[ApplicationName] = @ApplicationName,
			[Major] = @Major,
			[Minor] = @Minor,
			[Build] = @Build,
			[Revision] = @Revision
			
WHERE
			[ApplicationID] = @ApplicationID

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaVersion_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[mp_SchemaVersion_Delete]

/*
Author:   			
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/

@ApplicationID uniqueidentifier

AS

DELETE FROM [dbo].[mp_SchemaVersion]
WHERE
	[ApplicationID] = @ApplicationID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaVersion_SelectAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_SchemaVersion_SelectAll]

/*
Author:   			
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/

AS


SELECT
		[ApplicationID],
		[ApplicationName],
		[Major],
		[Minor],
		[Build],
		[Revision]
		
FROM
		[dbo].[mp_SchemaVersion]

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaVersion_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[mp_SchemaVersion_SelectOne]

/*
Author:   			
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/

@ApplicationID uniqueidentifier

AS


SELECT
		[ApplicationID],
		[ApplicationName],
		[Major],
		[Minor],
		[Build],
		[Revision]
		
FROM
		[dbo].[mp_SchemaVersion]
		
WHERE
		[ApplicationID] = @ApplicationID

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaVersion_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[mp_SchemaVersion_Insert]

/*
Author:   			
Created: 			1/29/2007
Last Modified: 		1/29/2007
*/

@ApplicationID uniqueidentifier,
@ApplicationName nvarchar(255),
@Major int,
@Minor int,
@Build int,
@Revision int

	
AS

INSERT INTO 	[dbo].[mp_SchemaVersion] 
(
				[ApplicationID],
				[ApplicationName],
				[Major],
				[Minor],
				[Build],
				[Revision]
) 

VALUES 
(
				@ApplicationID,
				@ApplicationName,
				@Major,
				@Minor,
				@Build,
				@Revision
				
)


' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_HtmlContent_SelectByPage]

/*
Author:			Joe Audettte
Created:		6/26/2005
Last Modified:		6/27/2005

*/


@SiteID		int,
@PageID		int

AS
SELECT  	h.*,
		m.ModuleTitle,
		md.FeatureName

FROM		mp_HtmlContent h

JOIN		mp_Modules m
ON		h.ModuleID = m.ModuleID

JOIN		mp_ModuleDefinitions md
ON		m.ModuleDefID = md.ModuleDefID

JOIN		mp_PageModules pm
ON			pm.ModuleID = m.ModuleID

JOIN		mp_Pages p
ON		p.PageID = pm.PageID

WHERE	p.SiteID = @SiteID
		AND pm.PageID = @PageID
		AND pm.PublishBeginDate < GetDate()
		AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > GetDate())
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_HtmlContent_Select]

/*
Author:			Joe Audettte
Created:		12/23/2004
Last Modified:		1/14/2007

*/


@ModuleID int,
@BeginDate datetime

AS
SELECT  	*

FROM		mp_HtmlContent

WHERE	ModuleID = @ModuleID
		AND BeginDate <= @BeginDate
		AND EndDate >= @BeginDate
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_HtmlContent_Insert]

/*
Author:   			
Created: 			12/23/2004
Last Modified: 			12/23/2004

*/

@ModuleID int,
@Title nvarchar(255),
@Excerpt ntext,
@Body ntext,
@MoreLink nvarchar(255),
@SortOrder int,
@BeginDate datetime,
@EndDate datetime,
@CreatedDate datetime,
@UserID int

	
AS

INSERT INTO 	[dbo].[mp_HtmlContent] 
(
				[ModuleID],
				[Title],
				[Excerpt],
				[Body],
				[MoreLink],
				[SortOrder],
				[BeginDate],
				[EndDate],
				[CreatedDate],
				[UserID]
) 

VALUES 
(
				@ModuleID,
				@Title,
				@Excerpt,
				@Body,
				@MoreLink,
				@SortOrder,
				@BeginDate,
				@EndDate,
				@CreatedDate,
				@UserID
				
)
SELECT @@IDENTITY 










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_HtmlContent_Update]

/*
Author:   			
Created: 			12/23/2004
Last Modified: 			12/23/2004


*/
	
@ItemID int, 
@ModuleID int, 
@Title nvarchar(255), 
@Excerpt ntext, 
@Body ntext, 
@MoreLink nvarchar(255), 
@SortOrder int, 
@BeginDate datetime, 
@EndDate datetime, 
@CreatedDate datetime, 
@UserID int 


AS

UPDATE 		[dbo].[mp_HtmlContent] 

SET
			[ModuleID] = @ModuleID,
			[Title] = @Title,
			[Excerpt] = @Excerpt,
			[Body] = @Body,
			[MoreLink] = @MoreLink,
			[SortOrder] = @SortOrder,
			[BeginDate] = @BeginDate,
			[EndDate] = @EndDate,
			[CreatedDate] = @CreatedDate,
			[UserID] = @UserID
			
WHERE
			[ItemID] = @ItemID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_SelectAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_HtmlContent_SelectAll]

/*
Author:   			
Created: 			12/23/2004
Last Modified: 			12/23/2004


*/

AS


SELECT	*
		
FROM
		[dbo].[mp_HtmlContent]










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_HtmlContent_Delete]

/*
Author:   			
Created: 			12/23/2004
Last Modified: 			12/23/2004

*/

@ItemID int

AS

DELETE FROM [dbo].[mp_HtmlContent]
WHERE
	[ItemID] = @ItemID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_HtmlContent_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_HtmlContent_SelectOne]

/*
Author:			Joe Audettte
Created:		12/23/2004
Last Modified:		12/23/2004

*/


@ItemID	int

AS

SELECT  	*

FROM		mp_HtmlContent

WHERE	ItemID = @ItemID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreadSubscriptions_UnsubscribeThread]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumThreadSubscriptions_UnsubscribeThread]

/*
Author:				
Created:			10/14/2004
Last Modified:			10/14/2004

*/

@ThreadID		int,
@UserID		int

AS

UPDATE		mp_ForumThreadSubscriptions

SET			UnSubscribeDate = GetDate()

WHERE		ThreadID = @ThreadID
			AND UserID = @UserID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreadSubscriptions_UnsubscribeAllThreads]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumThreadSubscriptions_UnsubscribeAllThreads]

/*
Author:				
Created:			10/14/2004
Last Modified:			10/14/2004

*/

@UserID		int

AS

UPDATE		mp_ForumThreadSubscriptions

SET			UnSubscribeDate = GetDate()

WHERE		
			UserID = @UserID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreadSubscriptions_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[mp_ForumThreadSubscriptions_Insert]

/*
Author:			
Created:		10/14/2004
Last Modified:		1/21/2006

*/

@ThreadID		int,
@UserID		int


AS

IF EXISTS (SELECT UserID FROM mp_ForumThreadSubscriptions WHERE ThreadID = @ThreadID AND UserID = @UserID)
BEGIN
	UPDATE 	mp_ForumThreadSubscriptions

	SET		SubscribeDate = GetDate(),
			UnSubscribeDate = Null
	

	WHERE 	ThreadID = @ThreadID AND UserID = @UserID

END

ELSE

BEGIN

	INSERT INTO	mp_ForumThreadSubscriptions
	(
			ThreadID,
			UserID
	)
	VALUES
	(
			@ThreadID,
			@UserID
	)

END

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreadSubscribers_SelectByThread]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumThreadSubscribers_SelectByThread]

/*
Author:				
Created:			10/13/2004
Last Modified:			10/13/2004

*/


@ThreadID		int,
@CurrentPostUserID 	int


AS

SELECT		u.Email


FROM			mp_Users u

JOIN			mp_ForumThreadSubscriptions s
ON			u.UserID = s.UserID

WHERE		s.ThreadID = @ThreadID
			AND s.UnSubscribeDate IS NULL
			AND u.UserID <> @CurrentPostUserID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_GetMostPopularWebPartsForMyPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_WebParts_GetMostPopularWebPartsForMyPage]

/*
Author:				
Created:			6/11/2006
Last Modified:		6/11/2006

*/

@SiteID		int


AS

SELECT TOP 15 dt.* 
FROM
(

SELECT  		m.ModuleID,
				m.SiteID,
				m.ModuleDefID,
				m.ModuleTitle,
				m.AllowMultipleInstancesOnMyPage,
				m.CountOfUseOnMyPage ,
				m.Icon As ModuleIcon,
				md.Icon As FeatureIcon,
				md.FeatureName,
				0 As IsAssembly,
				'''' As WebPartID
				
				
				
				
    
FROM
    			mp_Modules m
  
INNER JOIN
    			mp_ModuleDefinitions md
ON 			m.ModuleDefID = md.ModuleDefID

WHERE   
    			m.SiteID = @SiteID
				AND m.AvailableForMyPage = 1

UNION

SELECT
				0 As ModuleID,
				w.SiteID,
				0 As MuduleDefID,
				w.Title As ModuleTitle,
				w.AllowMultipleInstancesOnMyPage,
				w.CountOfUseOnMyPage ,
				w.ImageUrl As ModuleIcon,
				w.ImageUrl As FeatureIcon,
				w.Description As FeatureName,
				1 As IsAssembly,
				CONVERT(varchar(36),w.WebPartID) As WebPartID

FROM			mp_WebParts w

WHERE			w.SiteID = @SiteID
				AND w.AvailableForMyPage = 1
				



		
    
--ORDER BY		CountOfUseOnMyPage DESC, ModuleTitle

) dt

ORDER BY dt.CountOfUseOnMyPage DESC, dt.ModuleTitle


' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_GetCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_WebParts_GetCount]

/*
Author:				
Created:			6/11/2006
Last Modified:		6/11/2006

*/

@SiteID		int


AS

SELECT Count(*)
FROM
(

SELECT  		m.ModuleID
				
				
FROM
    			mp_Modules m
  
INNER JOIN
    			mp_ModuleDefinitions md
ON 			m.ModuleDefID = md.ModuleDefID

WHERE   
    			m.SiteID = @SiteID
				AND m.AvailableForMyPage = 1

UNION

SELECT
				0 As ModuleID
				

FROM			mp_WebParts w

WHERE			w.SiteID = @SiteID
				AND w.AvailableForMyPage = 1
				


) dt




' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_UpdateCountOfUseOnMyPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_WebParts_UpdateCountOfUseOnMyPage]

/*
Author:   			
Created: 			6/15/2006
Last Modified: 		6/15/2006
*/
	
@WebPartID uniqueidentifier, 
@Increment	int


AS
UPDATE 		[dbo].[mp_WebParts] 

SET
			CountOfUseOnMyPage = CountOfUseOnMyPage + @Increment
			
WHERE
			[WebPartID] = @WebPartID
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_WebParts_Update]

/*
Author:   			
Created: 			6/3/2006
Last Modified: 		6/3/2006
*/
	
@WebPartID uniqueidentifier, 
@SiteID int, 
@Title nvarchar(255), 
@Description nvarchar(255), 
@ImageUrl nvarchar(255), 
@ClassName nvarchar(255), 
@AssemblyName nvarchar(255), 
@AvailableForMyPage bit, 
@AllowMultipleInstancesOnMyPage bit, 
@AvailableForContentSystem bit 


AS

UPDATE 		[dbo].[mp_WebParts] 

SET
			[SiteID] = @SiteID,
			[Title] = @Title,
			[Description] = @Description,
			[ImageUrl] = @ImageUrl,
			[ClassName] = @ClassName,
			[AssemblyName] = @AssemblyName,
			[AvailableForMyPage] = @AvailableForMyPage,
			[AllowMultipleInstancesOnMyPage] = @AllowMultipleInstancesOnMyPage,
			[AvailableForContentSystem] = @AvailableForContentSystem
			
WHERE
			[WebPartID] = @WebPartID

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_GetWebPartsForMyPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_WebParts_GetWebPartsForMyPage]

/*
Author:				
Created:			6/11/2006
Last Modified:		6/11/2006

*/

@SiteID		int


AS
SELECT  		m.ModuleID,
				m.SiteID,
				m.ModuleDefID,
				m.ModuleTitle,
				m.AllowMultipleInstancesOnMyPage,
				m.CountOfUseOnMyPage ,
				m.Icon As ModuleIcon,
				md.Icon As FeatureIcon,
				md.FeatureName,
				0 As IsAssembly,
				'''' As WebPartID
				
				
				
				
    
FROM
    			mp_Modules m
  
INNER JOIN
    			mp_ModuleDefinitions md
ON 			m.ModuleDefID = md.ModuleDefID

WHERE   
    			m.SiteID = @SiteID
				AND m.AvailableForMyPage = 1

UNION

SELECT
				-1 As ModuleID,
				w.SiteID,
				0 As MuduleDefID,
				w.Title As ModuleTitle,
				w.AllowMultipleInstancesOnMyPage,
				w.CountOfUseOnMyPage ,
				w.ImageUrl As ModuleIcon,
				w.ImageUrl As FeatureIcon,
				w.Description As FeatureName,
				1 As IsAssembly,
				CONVERT(varchar(36),w.WebPartID) As WebPartID

FROM			mp_WebParts w

WHERE			w.SiteID = @SiteID
				AND w.AvailableForMyPage = 1
				



		
    
ORDER BY
    			ModuleTitle
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_WebParts_Insert]

/*
Author:   			
Created: 			6/3/2006
Last Modified: 		6/3/2006
*/

@WebPartID	uniqueidentifier,
@SiteID int,
@Title nvarchar(255),
@Description nvarchar(255),
@ImageUrl nvarchar(255),
@ClassName nvarchar(255),
@AssemblyName nvarchar(255),
@AvailableForMyPage bit,
@AllowMultipleInstancesOnMyPage bit,
@AvailableForContentSystem bit

	
AS



INSERT INTO 	[dbo].[mp_WebParts] 
(
				[WebPartID],
				[SiteID],
				[Title],
				[Description],
				[ImageUrl],
				[ClassName],
				[AssemblyName],
				[AvailableForMyPage],
				[AllowMultipleInstancesOnMyPage],
				[AvailableForContentSystem]
) 

VALUES 
(
				@WebPartID,
				@SiteID,
				@Title,
				@Description,
				@ImageUrl,
				@ClassName,
				@AssemblyName,
				@AvailableForMyPage,
				@AllowMultipleInstancesOnMyPage,
				@AvailableForContentSystem
				
)


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_WebParts_SelectOne]

/*
Author:   			
Created: 			6/3/2006
Last Modified: 		6/3/2006
*/

@WebPartID uniqueidentifier

AS


SELECT
		[WebPartID],
		[SiteID],
		[Title],
		[Description],
		[ImageUrl],
		[ClassName],
		[AssemblyName],
		[AvailableForMyPage],
		[AllowMultipleInstancesOnMyPage],
		[AvailableForContentSystem]
		
FROM
		[dbo].[mp_WebParts]
		
WHERE
		[WebPartID] = @WebPartID' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_SelectBySite]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_WebParts_SelectBySite]

/*
Author:   			
Created: 			6/11/2006
Last Modified: 		6/11/2006
*/

@SiteID		int

AS
SELECT
		[WebPartID],
		[SiteID],
		[Title],
		[Description],
		[ImageUrl],
		[ClassName],
		[AssemblyName],
		[AvailableForMyPage],
		[AllowMultipleInstancesOnMyPage],
		[AvailableForContentSystem]
		
FROM
		[dbo].[mp_WebParts]
		
WHERE
		[SiteID] = @SiteID
		AND AvailableForContentSystem = 1
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_WebParts_Delete]

/*
Author:   			
Created: 			6/3/2006
Last Modified: 		6/3/2006
*/

@WebPartID uniqueidentifier

AS

DELETE FROM [dbo].[mp_WebParts]
WHERE
	[WebPartID] = @WebPartID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_WebParts_WebPartExists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_WebParts_WebPartExists]

/*
Author:			
Created:		6/7/2006
Last Modified:	6/7/2006

*/
    
@SiteID  	int,
@ClassName	nvarchar(255),
@AssemblyName	nvarchar(255)

AS
IF EXISTS (	SELECT 	WebPartID
		FROM		mp_WebParts
		WHERE	SiteID = @SiteID
				AND (ClassName = @ClassName AND AssemblyName = @AssemblyName))
SELECT 1
ELSE
SELECT 0
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Links_SelectByPage]

/*
Author:			Joe Audettte
Created:		7/4/2005
Last Modified:		7/4/2005

*/


@SiteID		int,
@PageID		int

AS
SELECT  	u.*,
		
		m.ModuleTitle,
		md.FeatureName

FROM		mp_Links u

JOIN		mp_Modules m
ON		u.ModuleID = m.ModuleID

JOIN		mp_ModuleDefinitions md
ON		m.ModuleDefID = md.ModuleDefID

JOIN		mp_PageModules pm
ON			pm.ModuleID = m.ModuleID

JOIN		mp_Pages p
ON		p.PageID = pm.PageID

WHERE	p.SiteID = @SiteID
		AND pm.PageID = @PageID
		AND pm.PublishBeginDate < GetDate()
		AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > GetDate())

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Links_Insert]

/*
Author:   			
Created: 			12/24/2004
Last Modified: 			8/7/2005

*/

@ModuleID int,
@Title nvarchar(255),
@Url nvarchar(255),
@ViewOrder int,
@Description ntext,
@CreatedDate datetime,
@CreatedBy int,
@Target nvarchar(20)

	
AS

INSERT INTO 	[dbo].[mp_Links] 
(
				[ModuleID],
				[Title],
				[Url],
				[ViewOrder],
				[Description],
				[CreatedDate],
				[CreatedBy],
				Target
) 

VALUES 
(
				@ModuleID,
				@Title,
				@Url,
				@ViewOrder,
				@Description,
				@CreatedDate,
				@CreatedBy,
				@Target
				
)
SELECT @@IDENTITY



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Links_SelectOne]

    
@ItemID int

AS

SELECT
   *

FROM
    mp_Links

WHERE
    ItemID = @ItemID










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Links_Delete]

    
@ItemID int


AS

DELETE FROM
    mp_Links

WHERE
    ItemID = @ItemID










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Links_Update]

/*
Author:   			
Created: 			12/24/2004
Last Modified: 			8/7/2005

*/
	
@ItemID int, 
@ModuleID int, 
@Title nvarchar(255), 
@Url nvarchar(255), 
@ViewOrder int, 
@Description ntext, 
@CreatedDate datetime, 
@CreatedBy int ,
@Target nvarchar(20)


AS

UPDATE 		[dbo].[mp_Links] 

SET
			[ModuleID] = @ModuleID,
			[Title] = @Title,
			[Url] = @Url,
			Target = @Target,
			[ViewOrder] = @ViewOrder,
			[Description] = @Description,
			[CreatedDate] = @CreatedDate,
			[CreatedBy] = @CreatedBy
			
WHERE
			[ItemID] = @ItemID



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Links_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Links_Select]

    
@ModuleID int

AS

SELECT	*

FROM
    mp_Links

WHERE
    ModuleID = @ModuleID

ORDER BY
    ViewOrder, ItemID










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleSettings_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'













CREATE PROCEDURE [dbo].[mp_ModuleSettings_Update]
(
    @ModuleID      int,
    @SettingName   nvarchar(50),
    @SettingValue  nvarchar(255)
)
AS

IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        mp_ModuleSettings 
    WHERE 
        ModuleID = @ModuleID
      AND
        SettingName = @SettingName
)
INSERT INTO mp_ModuleSettings (
    ModuleID,
    SettingName,
    SettingValue
) 
VALUES (
    @ModuleID,
    @SettingName,
    @SettingValue
)
ELSE
UPDATE
    mp_ModuleSettings

SET
    SettingValue = @SettingValue

WHERE
    ModuleID = @ModuleID
  AND
    SettingName = @SettingName










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleSettings_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_ModuleSettings_Delete]

/*
Author:			
Created:		1/1/2005
Last Modified:		1/1/2005

*/

@ModuleID		int

AS

DELETE FROM mp_ModuleSettings

WHERE	ModuleID = @ModuleID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleSettings_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_ModuleSettings_Insert]

/*
Author:			
Created:		6/9/2005
Last Modified:		6/9/2005

*/



@ModuleID			int,
@SettingName			nvarchar(50),
@SettingValue			nvarchar(255),
@ControlType			nvarchar(50),
@RegexValidationExpression 	ntext




AS

INSERT INTO 	mp_ModuleSettings
(
			ModuleID,
			SettingName,
			SettingValue,
			ControlType,
			RegexValidationExpression
)

VALUES
(
			@ModuleID,
			@SettingName,
			@SettingValue,
			@ControlType,
			@RegexValidationExpression
)



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleSettings_CreateDefaultSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_ModuleSettings_CreateDefaultSettings]

/*
Author:			
Created:		1/1/2005
Last Modified:		1/1/2005

*/



@ModuleID		int


AS

INSERT INTO 	mp_ModuleSettings
(
			ModuleID,
			SettingName,
			SettingValue,
			ControlType,
			RegexValidationExpression
)

SELECT		m.ModuleID,
			ds.SettingName,
			ds.SettingValue,
			ds.ControlType,
			ds.RegexValidationExpression


FROM			mp_Modules m

JOIN			mp_ModuleDefinitionSettings ds
ON			ds.ModuleDefID = m.ModuleDefID

WHERE		m.ModuleID = @ModuleID

ORDER BY		ds.[ID]










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleSettings_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_ModuleSettings_Select]
(
    @ModuleID int
)
AS

SELECT	*

FROM		mp_ModuleSettings

WHERE	ModuleID = @ModuleID










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_UpdatePage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_Modules_UpdatePage]

	
@OldPageID				int,
@NewPageID			int,
@ModuleID           int
    

AS
UPDATE
    mp_PageModules

SET
    PageID = @NewPageID

WHERE
    ModuleID = @ModuleID
	AND PageID = @OldPageID' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PageModule_Exists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_PageModule_Exists]

/*
Author:			
Created:		5/18/2006
Last Modified:	5/18/2006

*/
    
@ModuleID  	int,
@PageID		int

AS
IF EXISTS (	SELECT 	ModuleID
		FROM		mp_PageModules
		WHERE	ModuleID = @ModuleID
				AND PageID = @PageID )
SELECT 1
ELSE
SELECT 0
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Modules_Insert]

/*
Author:   			
Created: 			12/26/2004
Last Modified: 		6/4/2006

*/

@PageID int,
@SiteID		int,
@ModuleDefID int,
@ModuleOrder int,
@PaneName nvarchar(50),
@ModuleTitle nvarchar(255),
@AuthorizedEditRoles ntext,
@CacheTime int,
@ShowTitle bit,
@AvailableForMyPage	bit,
@CreatedByUserID	int,
@CreatedDate		datetime,
@AllowMultipleInstancesOnMyPage	bit,
@Icon	nvarchar(255)

	
AS
DECLARE @ModuleID int

INSERT INTO 	[dbo].[mp_Modules] 
(
				SiteID,
				[ModuleDefID],
				[ModuleTitle],
				[AuthorizedEditRoles],
				[CacheTime],
				[ShowTitle],
				AvailableForMyPage,
				AllowMultipleInstancesOnMyPage,
				Icon,
				CreatedByUserID,
				CreatedDate
) 

VALUES 
(
				@SiteID,
				@ModuleDefID,
				@ModuleTitle,
				@AuthorizedEditRoles,
				@CacheTime,
				@ShowTitle,
				@AvailableForMyPage,
				@AllowMultipleInstancesOnMyPage,
				@Icon,
				@CreatedByUserID,
				@CreatedDate
				
)
SELECT @ModuleID =  @@IDENTITY

IF @PageID > -1
BEGIN
INSERT INTO 	[dbo].[mp_PageModules] 
(
				[PageID],
				[ModuleID],
				[ModuleOrder],
				[PaneName]
				
) 

VALUES 
(
				@PageID,
				@ModuleID,
				@ModuleOrder,
				@PaneName
				
				
)
END


SELECT @ModuleID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_SelectOneByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Modules_SelectOneByPage]

/*
Author:   			
Created: 			12/26/2004
Last Modified: 		6/4/2006

*/

@ModuleID int,
@PageID		int

AS
SELECT  		m.ModuleID,
				m.SiteID,
				pm.PageID,
				m.ModuleDefID,
				pm.ModuleOrder,
				pm.PaneName,
				m.ModuleTitle,
				m.AuthorizedEditRoles,
				m.CacheTime,
				m.ShowTitle,
				m.EditUserID,
				m.AvailableForMyPage,
				m.AllowMultipleInstancesOnMyPage,
				m.Icon,
				m.CreatedByUserID,
				m.CreatedDate,
				pm.PublishBeginDate,
				pm.PublishEndDate,
				md.ControlSrc
    
FROM
    			mp_Modules m
  
INNER JOIN
    			mp_ModuleDefinitions md
ON 			m.ModuleDefID = md.ModuleDefID

INNER JOIN		mp_PageModules pm
ON				m.ModuleID = pm.ModuleID
    
WHERE   
    			pm.PageID = @PageID
				AND pm.ModuleID = @ModuleID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_CalendarEvents_SelectByPage]

/*
Author:			Joe Audettte
Created:		7/4/2005
Last Modified:		7/4/2005

*/


@SiteID		int,
@PageID		int

AS
SELECT  	ce.*,
		
		m.ModuleTitle,
		md.FeatureName

FROM		mp_CalendarEvents ce

JOIN		mp_Modules m
ON		ce.ModuleID = m.ModuleID

JOIN		mp_ModuleDefinitions md
ON		m.ModuleDefID = md.ModuleDefID

JOIN		mp_PageModules pm
ON			pm.ModuleID = m.ModuleID

JOIN		mp_Pages p
ON		p.PageID = pm.PageID

WHERE	p.SiteID = @SiteID
		AND pm.PageID = @PageID
		AND pm.PublishBeginDate < GetDate()
		AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > GetDate())
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_SharedFiles_SelectByPage]

/*
Author:			Joe Audettte
Created:		7/4/2005
Last Modified:		7/4/2005

*/


@SiteID		int,
@PageID		int

AS
SELECT  	sf.*,
		
		m.ModuleTitle,
		md.FeatureName

FROM		mp_SharedFiles sf

JOIN		mp_Modules m
ON		sf.ModuleID = m.ModuleID

JOIN		mp_ModuleDefinitions md
ON		m.ModuleDefID = md.ModuleDefID

JOIN		mp_PageModules pm
ON			pm.ModuleID = m.ModuleID

JOIN		mp_Pages p
ON		p.PageID = pm.PageID

WHERE	p.SiteID = @SiteID
		AND pm.PageID = @PageID
		AND pm.PublishBeginDate < GetDate()
		AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > GetDate())
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Modules_SelectByPage]

/*
Author:				
Created:			12/26/2004
Last Modified:		8/27/2006

*/

@PageID		int


AS
SELECT  		m.ModuleID,
				m.SiteID,
				pm.PageID,
				m.ModuleDefID,
				pm.ModuleOrder,
				pm.PaneName,
				m.ModuleTitle,
				m.AuthorizedEditRoles,
				m.CacheTime,
				m.ShowTitle,
				m.EditUserID,
				m.AvailableForMyPage,
				m.CreatedByUserID,
				m.CreatedDate,
				pm.PublishBeginDate,
				pm.PublishEndDate,
				md.ControlSrc,
				md.FeatureName
    
FROM
    			mp_Modules m
  
INNER JOIN
    			mp_ModuleDefinitions md
ON 			m.ModuleDefID = md.ModuleDefID

INNER JOIN		mp_PageModules pm
ON				m.ModuleID = pm.ModuleID
    
WHERE   
    			pm.PageID = @PageID
				AND pm.PublishBeginDate < GetDate()
				AND	(
					(pm.PublishEndDate IS NULL)
					OR
					(pm.PublishEndDate > GetDate())
					)
		
    
ORDER BY
    			pm.ModuleOrder
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_DeleteInstance]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Modules_DeleteInstance]

/*
Author:   			
Created: 			5/18/2006
Last Modified: 		5/18/2006


*/

@ModuleID int,
@PageID		int

AS
DELETE FROM [dbo].[mp_PageModules]
WHERE ModuleID = @ModuleID AND PageID = @PageID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PageModules_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
Create PROCEDURE [dbo].[mp_PageModules_Insert]

/*
Author:   			
Created: 			5/18/2006
Last Modified: 		5/18/2006

*/

@ModuleID			int,
@PageID				int,
@PaneName			nvarchar(50),
@ModuleOrder		int,
@PublishBeginDate	datetime,
@PublishEndDate		datetime

	
AS
INSERT INTO 	[dbo].[mp_PageModules] 
(
				ModuleID,
				PageID,
				PaneName,
				ModuleOrder,
				PublishBeginDate,
				PublishEndDate
				
) 

VALUES 
(
				@ModuleID,
				@PageID,
				@PaneName,
				@ModuleOrder,
				@PublishBeginDate,
				@PublishEndDate
				
				
)
SELECT @@IDENTITY
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Modules_Delete]

/*
Author:   			
Created: 			12/26/2004
Last Modified: 		6/28/2006


*/

@ModuleID int

AS

DELETE FROM [dbo].[mp_PageModules]
WHERE ModuleID = @ModuleID

DELETE FROM [dbo].[mp_Modules]
WHERE ModuleID = @ModuleID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_SelectByPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_GalleryImages_SelectByPage]

/*
Author:			Joe Audettte
Created:		7/4/2005
Last Modified:		7/4/2005

*/


@SiteID		int,
@PageID		int

AS
SELECT  	gi.*,
		
		m.ModuleTitle,
		md.FeatureName

FROM		mp_GalleryImages gi

JOIN		mp_Modules m
ON		gi.ModuleID = m.ModuleID

JOIN		mp_ModuleDefinitions md
ON		m.ModuleDefID = md.ModuleDefID

JOIN		mp_PageModules pm
ON			pm.ModuleID = m.ModuleID

JOIN		mp_Pages p
ON		p.PageID = pm.PageID

WHERE	p.SiteID = @SiteID
		AND pm.PageID = @PageID
		AND pm.PublishBeginDate < GetDate()
		AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > GetDate())

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_GetAuthRoles]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Pages_GetAuthRoles]
(
    @SiteID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @EditRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = p.AuthorizedRoles,
    @EditRoles   = m.AuthorizedEditRoles
    
FROM    
    mp_Modules m
INNER JOIN mp_PageModules pm
ON	pm.ModuleID = m.ModuleID
  INNER JOIN
    mp_Pages p
ON pm.PageID = p.PageID
    
WHERE   
    m.ModuleID = @ModuleID
  AND
    p.SiteID = @SiteID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PageModules_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_PageModules_Update]

/*
Author:   			
Created: 			5/18/2006
Last Modified: 		5/18/2006

*/

@ModuleID			int,
@PageID				int,
@PaneName			nvarchar(50),
@ModuleOrder		int,
@PublishBeginDate	datetime,
@PublishEndDate		datetime

	
AS
UPDATE 	[dbo].[mp_PageModules] 
SET
				PaneName = @PaneName,
				ModuleOrder = @ModuleOrder,
				PublishBeginDate = @PublishBeginDate,
				PublishEndDate = @PublishEndDate

WHERE			ModuleID = @ModuleID
				AND PageID = @PageID
				



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_UpdateModuleOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Modules_UpdateModuleOrder]
(
	@PageID				int,
    @ModuleID           int,
    @ModuleOrder        int,
    @PaneName           nvarchar(50)
)
AS
UPDATE
    mp_PageModules

SET
    ModuleOrder = @ModuleOrder,
    PaneName    = @PaneName

WHERE
    ModuleID = @ModuleID
	AND PageID = @PageID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[mp_SchemaScriptHistory_Insert]

/*
Author:   			
Created: 			1/30/2007
Last Modified: 		1/30/2007
*/

@ApplicationID uniqueidentifier,
@ScriptFile nvarchar(255),
@RunTime datetime,
@ErrorOccurred bit,
@ErrorMessage ntext,
@ScriptBody ntext

	
AS

INSERT INTO 	[dbo].[mp_SchemaScriptHistory] 
(
				[ApplicationID],
				[ScriptFile],
				[RunTime],
				[ErrorOccurred],
				[ErrorMessage],
				[ScriptBody]
) 

VALUES 
(
				@ApplicationID,
				@ScriptFile,
				@RunTime,
				@ErrorOccurred,
				@ErrorMessage,
				@ScriptBody
				
)
SELECT @@IDENTITY 


' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory_SelectErrorsByApp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_SchemaScriptHistory_SelectErrorsByApp]

/*
Author:   			
Created: 			1/30/2007
Last Modified: 		1/30/2007
*/

@ApplicationID uniqueidentifier

AS


SELECT
		[ID],
		[ApplicationID],
		[ScriptFile],
		[RunTime],
		[ErrorOccurred],
		[ErrorMessage],
		[ScriptBody]
		
FROM
		[dbo].[mp_SchemaScriptHistory]

WHERE 
		[ApplicationID] = @ApplicationID
		AND [ErrorOccurred] = 1

ORDER BY [ID]

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory_Exists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_SchemaScriptHistory_Exists]

/*
Author:			
Created:		1/30/2007
Last Modified:	1/30/2007

*/
    
@ApplicationID uniqueidentifier,
@ScriptFile		nvarchar(255)

AS
IF EXISTS (	SELECT 	[ID]
		FROM		mp_SchemaScriptHistory
		WHERE	[ApplicationID] = @ApplicationID
				AND [ScriptFile] = @ScriptFile )
SELECT 1
ELSE
SELECT 0
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[mp_SchemaScriptHistory_Delete]

/*
Author:   			
Created: 			1/30/2007
Last Modified: 		1/30/2007
*/

@ID int

AS

DELETE FROM [dbo].[mp_SchemaScriptHistory]
WHERE
	[ID] = @ID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[mp_SchemaScriptHistory_SelectOne]

/*
Author:   			
Created: 			1/30/2007
Last Modified: 		1/30/2007
*/

@ID int

AS


SELECT
		[ID],
		[ApplicationID],
		[ScriptFile],
		[RunTime],
		[ErrorOccurred],
		[ErrorMessage],
		[ScriptBody]
		
FROM
		[dbo].[mp_SchemaScriptHistory]
		
WHERE
		[ID] = @ID

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SchemaScriptHistory_SelectByApp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_SchemaScriptHistory_SelectByApp]

/*
Author:   			
Created: 			1/30/2007
Last Modified: 		1/30/2007
*/

@ApplicationID uniqueidentifier

AS


SELECT
		[ID],
		[ApplicationID],
		[ScriptFile],
		[RunTime],
		[ErrorOccurred],
		[ErrorMessage],
		[ScriptBody]
		
FROM
		[dbo].[mp_SchemaScriptHistory]

WHERE 
		[ApplicationID] = @ApplicationID
		AND [ErrorOccurred] = 0

ORDER BY [ID]

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_UpdateCountOfUseOnMyPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Modules_UpdateCountOfUseOnMyPage]

/*
Author:   			
Created: 			6/15/2006
Last Modified: 		6/15/2006

*/
	
@ModuleID int, 
@Increment	int


AS
UPDATE 		[dbo].[mp_Modules] 

SET
			
			CountOfUseOnMyPage = CountOfUseOnMyPage + @Increment
			
WHERE
			[ModuleID] = @ModuleID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Modules_SelectOne]

/*
Author:   			
Created: 			12/26/2004
Last Modified: 			12/26/2004

*/

@ModuleID int

AS


SELECT	*
		
FROM
		[dbo].[mp_Modules]
		
WHERE
		[ModuleID] = @ModuleID










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Modules_Update]

/*
Author:   			
Created: 			12/26/2004
Last Modified: 			6/19/2005

*/
	
@ModuleID int, 
@ModuleDefID int, 
@ModuleTitle nvarchar(255), 
@AuthorizedEditRoles ntext, 
@CacheTime int, 
@ShowTitle bit ,
@EditUserID	int,
@AvailableForMyPage	bit,
@AllowMultipleInstancesOnMyPage	bit,
@Icon	nvarchar(255)


AS
UPDATE 		[dbo].[mp_Modules] 

SET
			
			[ModuleDefID] = @ModuleDefID,
			
			[ModuleTitle] = @ModuleTitle,
			[AuthorizedEditRoles] = @AuthorizedEditRoles,
			[CacheTime] = @CacheTime,
			[ShowTitle] = @ShowTitle,
			EditUserID = @EditUserID,
			AvailableForMyPage = @AvailableForMyPage,
			AllowMultipleInstancesOnMyPage = @AllowMultipleInstancesOnMyPage,
			Icon = @Icon
			
WHERE
			[ModuleID] = @ModuleID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Modules_SelectForMyPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Modules_SelectForMyPage]

/*
Author:				
Created:			5/21/2006
Last Modified:		6/4/2006

*/

@SiteID		int


AS
SELECT  		m.ModuleID,
				m.SiteID,
				m.ModuleDefID,
				m.ModuleTitle,
				m.AllowMultipleInstancesOnMyPage,
				m.Icon As ModuleIcon,
				md.Icon As FeatureIcon,
				md.FeatureName
				
				
				
				
    
FROM
    			mp_Modules m
  
INNER JOIN
    			mp_ModuleDefinitions md
ON 			m.ModuleDefID = md.ModuleDefID


WHERE   
    			m.SiteID = @SiteID
				AND m.AvailableForMyPage = 1
		
    
ORDER BY
    			m.ModuleTitle
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_GalleryImages_Delete]

/*
Author:   			
Created: 			12/4/2004
Last Modified: 		12/4/2004
*/

@ItemID int

AS

DELETE FROM [dbo].[mp_GalleryImages]
WHERE
	[ItemID] = @ItemID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_GalleryImages_SelectOne]

/*
Author:   			
Created: 			12/4/2004
Last Modified: 		12/4/2004
*/

@ItemID int

AS


SELECT
		[ItemID],
		[ModuleID],
		[DisplayOrder],
		[Caption],
		[Description],
		[MetaDataXml],
		[ImageFile],
		[WebImageFile],
		[ThumbnailFile],
		[UploadDate],
		[UploadUser]
		
FROM
		[dbo].[mp_GalleryImages]
		
WHERE
		[ItemID] = @ItemID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_GalleryImages_Select]

/*
Author:   			
Created: 			12/4/2004
Last Modified: 			12/4/2004

*/

@ModuleID		int

AS


SELECT
		[ItemID],
		[ModuleID],
		[DisplayOrder],
		[Caption],
		[Description],
		[MetaDataXml],
		[ImageFile],
		[WebImageFile],
		[ThumbnailFile],
		[UploadDate],
		[UploadUser]
		
FROM
		[dbo].[mp_GalleryImages]

WHERE	ModuleID = @ModuleID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_GalleryImages_Insert]

/*
Author:   			
Created: 			12/4/2004
Last Modified: 			12/4/2004


*/

@ModuleID int,
@DisplayOrder int,
@Caption nvarchar(255),
@Description ntext,
@MetaDataXml ntext,
@ImageFile nvarchar(100),
@WebImageFile nvarchar(100),
@ThumbnailFile nvarchar(100),
@UploadDate datetime,
@UploadUser nvarchar(100)

	
AS

INSERT INTO 	[dbo].[mp_GalleryImages] 
(
				[ModuleID],
				[DisplayOrder],
				[Caption],
				[Description],
				[MetaDataXml],
				[ImageFile],
				[WebImageFile],
				[ThumbnailFile],
				[UploadDate],
				[UploadUser]
) 

VALUES 
(
				@ModuleID,
				@DisplayOrder,
				@Caption,
				@Description,
				@MetaDataXml,
				@ImageFile,
				@WebImageFile,
				@ThumbnailFile,
				@UploadDate,
				@UploadUser
				
)
SELECT @@IDENTITY










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_GalleryImages_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_GalleryImages_Update]

/*
Author:   			
Created: 			12/4/2004
Last Modified: 			12/4/2004

*/
	
@ItemID int, 
@ModuleID int, 
@DisplayOrder int, 
@Caption nvarchar(255), 
@Description ntext, 
@MetaDataXml ntext, 
@ImageFile nvarchar(100), 
@WebImageFile nvarchar(100), 
@ThumbnailFile nvarchar(100), 
@UploadDate datetime, 
@UploadUser nvarchar(100) 


AS

UPDATE 		[dbo].[mp_GalleryImages] 

SET
			[ModuleID] = @ModuleID,
			[DisplayOrder] = @DisplayOrder,
			[Caption] = @Caption,
			[Description] = @Description,
			[MetaDataXml] = @MetaDataXml,
			[ImageFile] = @ImageFile,
			[WebImageFile] = @WebImageFile,
			[ThumbnailFile] = @ThumbnailFile,
			[UploadDate] = @UploadDate,
			[UploadUser] = @UploadUser
			
WHERE
			[ItemID] = @ItemID










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitionSettings_DeleteByID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_ModuleDefinitionSettings_DeleteByID]

    
@ID int

AS
DELETE FROM
    mp_ModuleDefinitionSettings

WHERE
    ID = @ID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitionSettings_UpdateByID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_ModuleDefinitionSettings_UpdateByID]

@ID						int,
@ModuleDefID      		int,
@SettingName   		nvarchar(50),
@SettingValue  			nvarchar(255),
@ControlType   			nvarchar(50),
@RegexValidationExpression 	ntext



AS

UPDATE
    mp_ModuleDefinitionSettings

SET
	SettingName = @SettingName,
    SettingValue = @SettingValue,
	ControlType = @ControlType,
	RegexValidationExpression = @RegexValidationExpression

WHERE
    ID = @ID AND ModuleDefID = @ModuleDefID
  
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitionSettings_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ModuleDefinitionSettings_Select]
(
    @ModuleDefID int
)
AS

SELECT
    *

FROM
    mp_ModuleDefinitionSettings

WHERE
    ModuleDefID = @ModuleDefID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitionSettings_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[mp_ModuleDefinitionSettings_Update]

    
@ModuleDefID      		int,
@SettingName   		nvarchar(50),
@SettingValue  			nvarchar(255),
@ControlType   			nvarchar(50),
@RegexValidationExpression 	ntext



AS

IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        mp_ModuleDefinitionSettings 
    WHERE 
        ModuleDefID = @ModuleDefID
      AND
        SettingName = @SettingName
)
INSERT INTO mp_ModuleDefinitionSettings (
    ModuleDefID,
    SettingName,
    SettingValue,
	ControlType,
	RegexValidationExpression
) 
VALUES (
    @ModuleDefID,
    @SettingName,
    @SettingValue,
	@ControlType,
	@RegexValidationExpression
)
ELSE
UPDATE
    mp_ModuleDefinitionSettings

SET
    SettingValue = @SettingValue,
	ControlType = @ControlType,
	RegexValidationExpression = @RegexValidationExpression

WHERE
    ModuleDefID = @ModuleDefID
  AND
    SettingName = @SettingName

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_ModuleDefinitions_Delete]

    
@ModuleDefID int

AS

DELETE FROM
    mp_ModuleDefinitions

WHERE
    ModuleDefID = @ModuleDefID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions_SelectUserModules]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_ModuleDefinitions_SelectUserModules]

    
@SiteID  int


AS

SELECT   		md.*

FROM			mp_ModuleDefinitions md

JOIN			mp_SiteModuleDefinitions smd
ON			smd.ModuleDefID = md.ModuleDefID
    
WHERE   		smd.SiteID = @SiteID
			AND md.IsAdmin = 0

ORDER BY 		md.SortOrder, md.FeatureName








' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_ModuleDefinitions_SelectOne]

    
@ModuleDefID int

AS

SELECT	*

FROM
    mp_ModuleDefinitions

WHERE
    ModuleDefID = @ModuleDefID










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_ModuleDefinitions_Update]

/*
Author:   			
Created: 			12/26/2004
Last Modified: 		1/21/2007

*/
	
@ModuleDefID int, 
@FeatureName nvarchar(255), 
@ControlSrc nvarchar(255), 
@SortOrder int, 
@IsAdmin bit,
@Icon	nvarchar(255),
@DefaultCacheTime int


AS
UPDATE 		[dbo].[mp_ModuleDefinitions] 

SET

			[FeatureName] = @FeatureName,
			[ControlSrc] = @ControlSrc,
			[SortOrder] = @SortOrder,
			DefaultCacheTime = @DefaultCacheTime,
			Icon = @Icon,
			[IsAdmin] = @IsAdmin
			
WHERE
			[ModuleDefID] = @ModuleDefID
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_ModuleDefinitions_Select]

    
@SiteID  int


AS

SELECT   	md.*

FROM		mp_ModuleDefinitions md

JOIN		mp_SiteModuleDefinitions smd
ON		smd.ModuleDefID = md.ModuleDefID
    
WHERE   	smd.SiteID = @SiteID

ORDER BY 	md.SortOrder, md.FeatureName








' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePaths_CreatePath]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_SitePaths_CreatePath]

@SiteID	int,
@Path           nvarchar(255),
@PathId         UNIQUEIDENTIFIER OUTPUT

AS

BEGIN
    BEGIN TRANSACTION
    IF (NOT EXISTS(SELECT * FROM mp_SitePaths WHERE LoweredPath = LOWER(@Path) AND SiteID = @SiteID))
    BEGIN
        INSERT mp_SitePaths (SiteID, Path, LoweredPath) VALUES (@SiteID, @Path, LOWER(@Path))
    END
    COMMIT TRANSACTION
    SELECT @PathId = PathID FROM mp_SitePaths WHERE LOWER(@Path) = LoweredPath AND SiteID = @SiteID
END' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationAllUsers_GetPageSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_SitePersonalizationAllUsers_GetPageSettings] 
(
    
@SiteID	int,
@Path              nvarchar(255)

)

AS

BEGIN
    
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @PathId = NULL

   
    SELECT @PathId = u.PathId FROM mp_SitePaths u WHERE u.SiteID = @SiteID AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT p.PageSettings FROM mp_SitePersonalizationAllUsers p WHERE p.PathId = @PathId
END' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_DecrementPostCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Forums_DecrementPostCount]

/*
Author:				
Created:			11/6/2004
Last Modified:			11/6/2004

*/

@ForumID		int

AS


UPDATE mp_Forums

SET PostCount = PostCount - 1

WHERE ItemID = @ForumID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumThreads_SelectOne]

/*
Author:			
Created:		9/19/2004
Last Modified:		2/16/2005

*/

@ThreadID		int


AS

SELECT		t.*,
			''MostRecentPostUser'' = COALESCE(u.[Name], ''Guest''),
			''StartedBy'' = COALESCE(s.[Name], ''Guest''),
			f.PostsPerPage


FROM			mp_ForumThreads t

LEFT OUTER JOIN	mp_Users u
ON			t.MostRecentPostUserID = u.UserID

LEFT OUTER JOIN	mp_Users s
ON			t.StartedByUserID = s.UserID

JOIN			mp_Forums f
ON			f.ItemID = t.ForumID

WHERE		t.ThreadID = @ThreadID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_IncrementPostCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Forums_IncrementPostCount]

/*
Author:				
Created:			11/6/2004
Last Modified:			1/14/2007

*/

@ForumID			int,
@MostRecentPostUserID	int,
@MostRecentPostDate datetime

AS
UPDATE 	mp_Forums

SET 		MostRecentPostDate = @MostRecentPostDate,
		MostRecentPostUserID = @MostRecentPostUserID,
 		PostCount = PostCount + 1

WHERE 	ItemID = @ForumID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_Forums_Update]

/*
Author:			
Created:		9/12/2004
Last Modified:		10/20/2004

*/

@ItemID			int,
@Title          			nvarchar(100),
@Description    			ntext,
@IsModerated			bit,
@IsActive			bit,
@SortOrder			int,
@PostsPerPage			int,
@ThreadsPerPage		int,
@AllowAnonymousPosts		bit



AS


UPDATE		mp_Forums

SET			Title = @Title,
			[Description] = @Description,
			IsModerated = @IsModerated,
			IsActive = @IsActive,
			SortOrder = @SortOrder,
			PostsPerPage = @PostsPerPage,
			ThreadsPerPage = @ThreadsPerPage,
			AllowAnonymousPosts = @AllowAnonymousPosts



WHERE		ItemID = @ItemID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_SelectByForumDesc_v2]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumThreads_SelectByForumDesc_v2]

/*
Author:			
Created:		9/14/2004
Last Modified:		9/25/2004

*/


@ForumID			int,
@PageNumber			int

AS

DECLARE @ThreadsPerPage	int
DECLARE @TotalThreads	int
DECLARE @BeginSequence int
DECLARE @EndSequence int

SELECT	@ThreadsPerPage = ThreadsPerPage,
		@TotalThreads = ThreadCount
FROM		mp_Forums
WHERE	ItemID = @ForumID


SET @BeginSequence = @TotalThreads - (@ThreadsPerPage * @PageNumber) + 1
SET @EndSequence = @BeginSequence + @ThreadsPerPage  -1

CREATE TABLE #PageIndex 
(
	IndexID int IDENTITY (1, 1) NOT NULL,
	ThreadID int
	
)

INSERT INTO #PageIndex (ThreadID)


SELECT	t.ThreadID
FROM		mp_ForumThreads t
WHERE	t.ForumID = @ForumID	
ORDER BY	t.MostRecentPostDate 


SELECT	t.*,
		''MostRecentPostUser'' = u.[Name],
		''StartedBy'' = s.[Name]


FROM		mp_ForumThreads t

JOIN		#PageIndex p
ON		p.ThreadID = t.ThreadID

LEFT OUTER JOIN		mp_Users u
ON		t.MostRecentPostUserID = u.UserID

LEFT OUTER JOIN		mp_Users s
ON		t.StartedByUserID = s.UserID

WHERE	t.ForumID = @ForumID
		AND p.IndexID
Between @BeginSequence 
AND @EndSequence

ORDER BY	p.IndexID DESC

DROP TABLE #PageIndex










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_Forums_Insert]

/*
Author:				
Created:			9/12/2004
Last Modified:			10/20/2004

*/

@ModuleID			int,
@UserID			int,
@Title          			nvarchar(100),
@Description    			ntext,
@IsModerated			bit,
@IsActive			bit,
@SortOrder			int,
@PostsPerPage			int,
@ThreadsPerPage		int,
@AllowAnonymousPosts		bit



AS

INSERT INTO			mp_Forums
(
				ModuleID,
				CreatedBy,
				Title,
				[Description],
				IsModerated,
				IsActive,
				SortOrder,
				PostsPerPage,
				ThreadsPerPage,
				AllowAnonymousPosts

)

VALUES
(
				@ModuleID,
				@UserID,
				@Title,
				@Description,
				@IsModerated,
				@IsActive,
				@SortOrder,
				@PostsPerPage,
				@ThreadsPerPage,
				@AllowAnonymousPosts

)

SELECT @@IDENTITY As ItemID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_SelectByForum]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumThreads_SelectByForum]

/*
Author:			
Created:		9/14/2004
Last Modified:		2/16/2005

*/


@ForumID			int,
@PageNumber			int

AS

DECLARE @ThreadsPerPage	int
DECLARE @CurrentPageMaxForumSequence	int
DECLARE @BeginSequence int
DECLARE @EndSequence int

SELECT	@ThreadsPerPage = ThreadsPerPage
		
FROM		mp_Forums

WHERE	ItemID = @ForumID

SET @CurrentPageMaxForumSequence = (@ThreadsPerPage * @PageNumber)

IF @CurrentPageMaxForumSequence > @ThreadsPerPage + 1
	BEGIN
		SET @BeginSequence = @CurrentPageMaxForumSequence  
- @ThreadsPerPage + 1
	END
ELSE
	BEGIN
		SET @BeginSequence = 1
	END

SET @EndSequence = @BeginSequence + @ThreadsPerPage  -1

SELECT	t.*,
		''MostRecentPostUser'' = COALESCE(u.[Name], ''Guest''),
		''StartedBy'' = s.[Name]


FROM		mp_ForumThreads t

LEFT OUTER JOIN		mp_Users u
ON		t.MostRecentPostUserID = u.UserID

LEFT OUTER JOIN		mp_Users s
ON		t.StartedByUserID = s.UserID

WHERE	t.ForumID = @ForumID
		AND t.ForumSequence 
Between @BeginSequence 
AND @EndSequence

ORDER BY	t.SortOrder, t.ThreadID DESC










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_SelectByForumDesc]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumThreads_SelectByForumDesc]

/*
Author:			
Created:		9/14/2004
Last Modified:		9/25/2004

*/


@ForumID			int,
@PageNumber			int

AS

DECLARE @ThreadsPerPage	int
DECLARE @TotalThreads	int
DECLARE @BeginSequence int
DECLARE @EndSequence int

SELECT	@ThreadsPerPage = ThreadsPerPage,
		@TotalThreads = ThreadCount
FROM		mp_Forums
WHERE	ItemID = @ForumID


SET @BeginSequence = @TotalThreads - (@ThreadsPerPage * @PageNumber) + 1
SET @EndSequence = @BeginSequence + @ThreadsPerPage  -1

SELECT	t.*,
		''MostRecentPostUser'' = u.[Name],
		''StartedBy'' = s.[Name]


FROM		mp_ForumThreads t

LEFT OUTER JOIN		mp_Users u
ON		t.MostRecentPostUserID = u.UserID

LEFT OUTER JOIN		mp_Users s
ON		t.StartedByUserID = s.UserID

WHERE	t.ForumID = @ForumID
		AND t.ForumSequence 
Between @BeginSequence 
AND @EndSequence

ORDER BY	t.SortOrder, t.ThreadID DESC










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_IncrementThreadCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Forums_IncrementThreadCount]

/*
Author:			
Created:		11/28/2004
Last Modified:		11/28/2004

*/

@ForumID			int

AS

UPDATE		mp_Forums

SET			ThreadCount = ThreadCount + 1

WHERE		ItemID = @ForumID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_Forums_SelectOne]

/*
Author:				
Created:			9/12/2004
Last Modified:			9/12/2004

*/

@ItemID			int

AS

SELECT		f.*,
			''CreatedByUser'' = u.[Name],
			''MostRecentPostUser'' = up.[Name]

FROM			mp_Forums f

LEFT OUTER JOIN	mp_Users u
ON			f.CreatedBy = u.UserID

LEFT OUTER JOIN	mp_Users up
ON			f.MostRecentPostUserID = up.UserID

WHERE		f.ItemID = @ItemID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_UpdateThreadStats]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_Forums_UpdateThreadStats]

/*
Author:			
Created:		9/19/2004
Last Modified:		9/19/2004

*/

@ForumID			int

AS

UPDATE		mp_Forums

SET			ThreadCount = ThreadCount + 1

WHERE		ItemID = @ForumID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_DecrementThreadCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Forums_DecrementThreadCount]

/*
Author:			
Created:		11/28/2004
Last Modified:		11/28/2004

*/

@ForumID			int

AS

UPDATE		mp_Forums

SET			ThreadCount = ThreadCount - 1

WHERE		ItemID = @ForumID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_UpdatePostStats]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_Forums_UpdatePostStats]

/*
Author:			
Created:		9/19/2004
Last Modified:		9/19/2004

*/

@ForumID			int,
@MostRecentPostUserID	int

AS

UPDATE	mp_Forums

SET		MostRecentPostDate = GetDate(),
		MostRecentPostUserID = @MostRecentPostUserID,
		PostCount = PostCount + 1

WHERE	ItemID = @ForumID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_IncrementPostCountOnly]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Forums_IncrementPostCountOnly]

/*
Author:				
Created:			9/10/2005
Last Modified:			9/10/2005

*/

@ForumID			int


AS


UPDATE 	mp_Forums

SET 		
 		PostCount = PostCount + 1

WHERE 	ItemID = @ForumID



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Forums_RecalculatePostStats]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Forums_RecalculatePostStats]

/*
Author:			
Created:		9/11/2005
Last Modified:		9/11/2005

based ont he pgsql version by Dean Brettle

*/

@ForumID		int


AS

DECLARE @RowsAffected		int
DECLARE @MostRecentPostDate	datetime
DECLARE @MostRecentPostUserID	int
DECLARE @PostCount			int

SET @RowsAffected = 0

SELECT TOP 1	@MostRecentPostDate = MostRecentPostDate,
		@MostRecentPostUserID = MostRecentPostUserID

FROM		mp_ForumThreads

WHERE	ForumID = @ForumID

ORDER BY	MostRecentPostDate DESC

SET @PostCount = COALESCE(
				(	SELECT 	SUM(TotalReplies) + COUNT(*)
					FROM		mp_ForumThreads
					WHERE	ForumID = @ForumID

				),
				0
				)

UPDATE 	mp_Forums
SET		MostRecentPostDate = @MostRecentPostDate,
		MostRecentPostUserID = @MostRecentPostUserID,
		PostCount = @PostCount

WHERE	ItemID = @ForumID

SET @RowsAffected = @@ROWCOUNT


SELECT @RowsAffected



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_RoleExists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Roles_RoleExists]

/*
Author:			
Created:		5/19/2005
Last Modified:		5/19/2005

*/
    
@SiteID  	int,
@RoleName	nvarchar(50)

AS

IF EXISTS (	SELECT 	RoleID
		FROM		mp_Roles
		WHERE	SiteID = @SiteID
				AND (RoleName = @RoleName OR DisplayName = @RoleName))
SELECT 1
ELSE
SELECT 0



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_Roles_Insert]

/*
Author:			
Created:		7/19/2004
Last Modified:		5/19/2005

*/


@SiteID    		int,
@RoleName    nvarchar(50)

AS

INSERT INTO mp_Roles
(
    		SiteID,
    		RoleName,
    		DisplayName
)

VALUES
(
    	@SiteID,
    	@RoleName,
	@RoleName
)

SELECT  @@Identity As RoleID



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Roles_Select]

/*
Last Modified:		5/19/2005 

*/
    
@SiteID  int

AS

SELECT  *

FROM		mp_Roles

WHERE   	SiteID = @SiteID



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'














CREATE PROCEDURE [dbo].[mp_Roles_SelectOne]
(
    @RoleID int
)
AS

SELECT
    *

FROM
    mp_Roles

WHERE
    RoleID = @RoleID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_SelectOneByName]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Roles_SelectOneByName]

/*
Auhor:			
Created:		5/21/2005
Last Modified:		5/21/2005

*/



@SiteID	int,
@RoleName	nvarchar(50)



AS


SELECT *

FROM		mp_Roles

WHERE	SiteID = @SiteID
		AND RoleName = @RoleName



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Roles_Update]

/*
Last Modified:		5/19/2005 

*/

    
@RoleID      int,
@RoleName    nvarchar(50)

AS

UPDATE		mp_Roles

SET
    			DisplayName = @RoleName

WHERE
    			RoleID = @RoleID



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Roles_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Roles_Delete]

/*
Last Modified:		5/21/2005
don''t allow delete of admins role

*/
    
@RoleID int

AS

DELETE FROM	mp_Roles

WHERE	RoleID = @RoleID AND RoleName  <> ''Admins'' AND RoleName <> ''Content Administrators'' AND RoleName <> ''Authenticated Users''



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_GetUserRoles]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'





CREATE PROCEDURE [dbo].[mp_Users_GetUserRoles]



@SiteID	int,
@UserID       	int

AS

SELECT  
    		mp_Roles.RoleName,
    		mp_Roles.RoleID

FROM		 mp_UserRoles
  
INNER JOIN 	mp_Users 
ON 		mp_UserRoles.UserID = mp_Users.UserID

INNER JOIN 	mp_Roles 
ON 		mp_UserRoles.RoleID = mp_Roles.RoleID


WHERE   	mp_Users.SiteID = @SiteID
		AND mp_UserRoles.UserID = @UserID


' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Pages_SelectOne]

/*
Author:				
Created:			11/7/2004
Last Modified:		8/27/2006

*/

@SiteID		int,
@PageID		int


AS
SELECT TOP 1	*

FROM		mp_Pages

WHERE	(PageID = @PageID OR @PageID = -1)
		AND SiteID = @SiteID 
ORDER BY ParentID, PageOrder
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteSettings_SelectDefaultPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_SiteSettings_SelectDefaultPage]

/*
Author:				
Created:			7/27/2004
Last Modified:			1/8/2006

*/

@HostName		nvarchar(255)

AS
DECLARE @SiteID int

SET @SiteID = COALESCE(	(SELECT TOP 1 SiteID FROM mp_SiteHosts WHERE HostName = @HostName),
				 (SELECT TOP 1 SiteID FROM mp_Sites ORDER BY SiteID)
			)


 SELECT TOP 1
        			s.SiteID,
         			s.SiteName,
			s.Skin,
			s.Logo,
			s.Icon,
			s.AllowNewRegistration,
			s.AllowUserSkins,
			s.AllowPageSkins,
			s.AllowHideMenuOnPages,
			s.UseSecureRegistration,
			s.UseSSLOnAllPages,
			''MetaKeyWords'' = COALESCE(s.DefaultPageKeyWords,  ''''),
			''MetaDescription'' = COALESCE(s.DefaultPageDescription,  ''''),
			''MetaEncoding'' = COALESCE(s.DefaultPageEncoding,  ''''),
			''MetaAdditional'' = COALESCE(s.DefaultAdditionalMetaTags,  ''''),
			''PageMetaKeyWords'' = COALESCE(p.PageKeyWords, ''''),
			''PageMetaDescription'' = COALESCE(p.PageDescription, ''''),
			''PageMetaEncoding'' = COALESCE(p.PageEncoding, ''''),
			''PageMetaAdditional'' = COALESCE(p.AdditionalMetaTags, ''''),
			s.IsServerAdminSite,
			s.UseLdapAuth,
			s.AutoCreateLdapUserOnFirstLogin,
			s.LdapServer,
			s.LdapPort,
			s.LdapDomain,
			s.LdapRootDN,
			s.LdapUserDNKey,
			s.ReallyDeleteUsers,
			s.UseEmailForLogin,
			s.AllowUserFullNameChange,
			s.EditorSkin,
			s.DefaultFriendlyUrlPatternEnum,
			s.EnableMyPageFeature,
        			p.PageID,
			p.ParentID,
         			p.PageOrder,
        			p.PageName,
					p.PageTitle,
					p.MenuImage,
         			p.RequireSSL,
        			p.AuthorizedRoles,
			p.EditRoles,
			p.CreateChildPageRoles,
        			p.ShowBreadcrumbs,
			p.ShowChildPageMenu,
			p.ShowChildBreadCrumbs,
			p.HideMainMenu,
			''PageSkin'' = p.Skin


FROM			mp_Pages p
    
INNER JOIN		mp_Sites  s
ON 			p.SiteID = s.SiteID
        
    
WHERE
        			s.SiteID = @SiteID
        
ORDER BY
        			p.ParentID, p.PageOrder
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Pages_Insert]

/*
Author:			
Created:		11/7/2004
Last Modified:		1/8/2007

*/

@SiteID   		int,
@ParentID		int,
@PageName    		nvarchar(50),
@PageOrder   		int,
@AuthorizedRoles 	ntext,
@EditRoles		ntext,
@CreateChildPageRoles ntext,
@RequireSSL		bit,
@ShowBreadcrumbs 	bit,
@ShowChildPageBreadcrumbs 	bit,
@PageKeyWords	nvarchar(255),
@PageDescription	nvarchar(255),
@PageEncoding	nvarchar(255),
@AdditionalMetaTags	nvarchar(255),
@UseUrl		bit,
@Url			nvarchar(255),
@OpenInNewWindow	bit,
@ShowChildPageMenu	bit,
@HideMainMenu	bit,
@Skin			nvarchar(100),
@IncludeInMenu	bit,
@MenuImage			nvarchar(50),
@PageTitle    		nvarchar(255),
@AllowBrowserCache	bit

AS
INSERT INTO 		mp_Pages
(
    			SiteID,
			ParentID,
    			PageName,
				PageTitle,
    			PageOrder,
			AuthorizedRoles,
			EditRoles,
			CreateChildPageRoles,
    			RequireSSL,
			AllowBrowserCache,
    			ShowBreadcrumbs,
			ShowChildBreadCrumbs,
    			PageKeyWords,
			PageDescription,
			PageEncoding,
			AdditionalMetaTags,
			UseUrl,
			Url,
			OpenInNewWindow,
			ShowChildPageMenu,
			HideMainMenu,
			Skin,
			IncludeInMenu,
			MenuImage
)

VALUES
(
    			@SiteID,
			@ParentID,
    			@PageName,
				@PageTitle,
    			@PageOrder,
			@AuthorizedRoles,
			@EditRoles,
			@CreateChildPageRoles,
    			@RequireSSL,
			@AllowBrowserCache,
    			@ShowBreadcrumbs,
			@ShowChildPageBreadcrumbs,
			@PageKeyWords,
			@PageDescription,
			@PageEncoding,
			@AdditionalMetaTags,
			@UseUrl,
			@Url,
			@OpenInNewWindow,
			@ShowChildPageMenu,
			@HideMainMenu,
			@Skin,
			@IncludeInMenu,
			@MenuImage
)

SELECT  @@Identity As PageID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteSettings_SelectDefaultPageByID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_SiteSettings_SelectDefaultPageByID]

/*
Author:				
Created:			3/7/2005
Last Modified:			1/8/2006

*/

@SiteID int

AS
SELECT TOP 1
        			s.SiteID,
         			s.SiteName,
			s.Skin,
			s.Logo,
			s.Icon,
			s.AllowNewRegistration,
			s.AllowUserSkins,
			s.AllowPageSkins,
			s.AllowHideMenuOnPages,
			s.UseSecureRegistration,
			s.UseSSLOnAllPages,
			''MetaKeyWords'' = COALESCE(s.DefaultPageKeyWords,  ''''),
			''MetaDescription'' = COALESCE(s.DefaultPageDescription,  ''''),
			''MetaEncoding'' = COALESCE(s.DefaultPageEncoding,  ''''),
			''MetaAdditional'' = COALESCE(s.DefaultAdditionalMetaTags,  ''''),
			''PageMetaKeyWords'' = COALESCE(p.PageKeyWords, ''''),
			''PageMetaDescription'' = COALESCE(p.PageDescription, ''''),
			''PageMetaEncoding'' = COALESCE(p.PageEncoding, ''''),
			''PageMetaAdditional'' = COALESCE(p.AdditionalMetaTags, ''''),
			s.IsServerAdminSite,
			s.UseLdapAuth,
			s.AutoCreateLdapUserOnFirstLogin,
			s.LdapServer,
			s.LdapPort,
			s.LdapDomain,
			s.LdapRootDN,
			s.LdapUserDNKey,
			s.ReallyDeleteUsers,
			s.UseEmailForLogin,
			s.AllowUserFullNameChange,
			s.EditorSkin,
			s.DefaultFriendlyUrlPatternEnum,
			s.EnableMyPageFeature,
        			p.PageID,
			p.ParentID,
         			p.PageOrder,
        			p.PageName,
					p.PageTitle,
			p.MenuImage,
         			p.RequireSSL,
        			p.AuthorizedRoles,
			p.EditRoles,
			p.CreateChildPageRoles,
        			p.ShowBreadcrumbs,
			p.ShowChildPageMenu,
			p.ShowChildBreadCrumbs,
			p.HideMainMenu,
			''PageSkin'' = p.Skin


FROM			mp_Pages p
    
INNER JOIN		mp_Sites  s
ON 			p.SiteID = s.SiteID
        
    
WHERE
        			s.SiteID = @SiteID
        
ORDER BY
        			p.ParentID, p.PageOrder
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteSettings_SelectPage]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_SiteSettings_SelectPage]

/*
Author:				
Created:			7/27/2004
Last Modified:			1/8/2006

*/

@PageID		int,
@HostName		nvarchar(255)

AS
DECLARE @SiteID int

SET @SiteID = COALESCE(	(SELECT TOP 1 SiteID FROM mp_SiteHosts WHERE HostName = @HostName),
				 (SELECT TOP 1 SiteID FROM mp_Sites ORDER BY SiteID)
			)


 SELECT TOP 1
        			s.SiteID,
         			s.SiteName,
			s.Skin,
			s.Logo,
			s.Icon,
			s.AllowNewRegistration,
			s.AllowUserSkins,
			s.AllowPageSkins,
			s.AllowHideMenuOnPages,
			s.UseSecureRegistration,
			s.UseSSLOnAllPages,
			''MetaKeyWords'' = COALESCE(s.DefaultPageKeyWords,  ''''),
			''MetaDescription'' = COALESCE(s.DefaultPageDescription,  ''''),
			''MetaEncoding'' = COALESCE(s.DefaultPageEncoding,  ''''),
			''MetaAdditional'' = COALESCE(s.DefaultAdditionalMetaTags,  ''''),
			''PageMetaKeyWords'' = COALESCE(p.PageKeyWords, ''''),
			''PageMetaDescription'' = COALESCE(p.PageDescription, ''''),
			''PageMetaEncoding'' = COALESCE(p.PageEncoding, ''''),
			''PageMetaAdditional'' = COALESCE(p.AdditionalMetaTags, ''''),
			s.IsServerAdminSite,
			s.UseLdapAuth,
			s.AutoCreateLdapUserOnFirstLogin,
			s.LdapServer,
			s.LdapPort,
			s.LdapDomain,
			s.LdapRootDN,
			s.LdapUserDNKey,
			s.ReallyDeleteUsers,
			s.UseEmailForLogin,
			s.AllowUserFullNameChange,
			s.EditorSkin,
			s.DefaultFriendlyUrlPatternEnum,
			s.EnableMyPageFeature,
        			p.PageID,
			p.ParentID,
         			p.PageOrder,
        			p.PageName,
			p.PageTitle,
			p.MenuImage,
         			p.RequireSSL,
        			p.AuthorizedRoles,
			p.EditRoles,
			p.CreateChildPageRoles,
        			p.ShowBreadcrumbs,
			p.ShowChildPageMenu,
			p.ShowChildBreadCrumbs,
			p.HideMainMenu,
			''PageSkin'' = p.Skin


FROM			mp_Pages p
    
INNER JOIN		mp_Sites  s
ON 			p.SiteID = s.SiteID
        
    
WHERE		s.SiteID = @SiteID
        			AND p.PageID = @PageID
        
ORDER BY
        			p.PageOrder
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_SelectList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_Pages_SelectList]

/*
Author:				
Created:			7/27/2004
Last Modified:			7/27/2004

*/

@SiteID			int



AS

SELECT  			*
    				
    
FROM    
    				mp_Pages
    
WHERE   
    				SiteID = @SiteID

ORDER BY			ParentID,  PageName







' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_UpdatePageOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'













CREATE PROCEDURE [dbo].[mp_Pages_UpdatePageOrder]
(
    @PageID           int,
    @PageOrder        int
)
AS

UPDATE
    mp_Pages

SET
    PageOrder = @PageOrder

WHERE
    PageID = @PageID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteSettings_GetPageList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_SiteSettings_GetPageList]

/*
Author:				
Created:			7/27/2004
Last Modified:			3/19/2005

*/

@SiteID			int



AS
SELECT  
    				*
    
FROM    
    				mp_Pages
    
WHERE   
    				SiteID = @SiteID
					AND IncludeInMenu = 1

ORDER BY			ParentID, PageOrder, PageName
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_GetNextPageOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Pages_GetNextPageOrder]

/*
Author:			
Created:		11/7/2004
Last Modified:		11/7/2004

*/

@ParentID		int

AS

SELECT	COALESCE(MAX(PageOrder), -1) + 2

FROM		mp_Pages

WHERE	ParentID = @ParentID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_GetBreadcrumbs]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Pages_GetBreadcrumbs]

/*
Author:			
Created:		1/1/2005
Last Modified:		1/1/2005

*/


@PageID		int


AS


SELECT		p.PageID,
			p.PageName,
			p.ParentID As Parent1ID,
			p1.PageName As Parent1Name,
			COALESCE(p1.ParentID,-1) As Parent2ID,
			p2.PageName As Parent2Name,
			COALESCE(p2.ParentID,-1) As Parent3ID,
			p3.PageName As Parent3Name,
			COALESCE(p3.ParentID,-1) As Parent4ID,
			p4.PageName As Parent4Name,
			COALESCE(p4.ParentID,-1) As Parent5ID,
			p5.PageName As Parent5Name,
			COALESCE(p5.ParentID,-1) As Parent6ID,
			p6.PageName As Parent6Name,
			COALESCE(p6.ParentID,-1) As Parent7ID,
			p7.PageName As Parent7Name,
			COALESCE(p7.ParentID,-1) As Parent8ID,
			p8.PageName As Parent8Name,
			COALESCE(p8.ParentID,-1) As Parent9ID,
			p9.PageName As Parent9Name,
			COALESCE(p9.ParentID,-1) As Parent10ID,
			p10.PageName As Parent10Name,
			COALESCE(p10.ParentID,-1) As Parent11ID,
			p11.PageName As Parent11Name,
			COALESCE(p11.ParentID,-1) As Parent12ID,
			p12.PageName As Parent12Name


FROM			mp_Pages p

LEFT OUTER JOIN	mp_Pages p1
ON			p1.PageID = p.ParentID

LEFT OUTER JOIN	mp_Pages p2
ON			p2.PageID = p1.ParentID

LEFT OUTER JOIN	mp_Pages p3
ON			p3.PageID = p2.ParentID

LEFT OUTER JOIN	mp_Pages p4
ON			p4.PageID = p3.ParentID

LEFT OUTER JOIN	mp_Pages p5
ON			p5.PageID = p4.ParentID

LEFT OUTER JOIN	mp_Pages p6
ON			p6.PageID = p5.ParentID

LEFT OUTER JOIN	mp_Pages p7
ON			p7.PageID = p6.ParentID

LEFT OUTER JOIN	mp_Pages p8
ON			p8.PageID = p7.ParentID

LEFT OUTER JOIN	mp_Pages p9
ON			p9.PageID = p8.ParentID

LEFT OUTER JOIN	mp_Pages p10
ON			p10.PageID = p9.ParentID

LEFT OUTER JOIN	mp_Pages p11
ON			p11.PageID = p10.ParentID

LEFT OUTER JOIN	mp_Pages p12
ON			p12.PageID = p11.ParentID




WHERE 		p.PageID = @PageID










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Pages_Update]

/*
Author:			
Last Modified:		1/8/2007

*/


@SiteID        		int,
@PageID           	int,
@ParentID		int,
@PageOrder        	int,
@PageName         	nvarchar(50),
@AuthorizedRoles 	ntext,
@EditRoles		ntext,
@CreateChildPageRoles ntext,
@RequireSSL		bit,
@ShowBreadcrumbs	bit,
@ShowChildPageBreadcrumbs bit,
@PageKeyWords	nvarchar(255),
@PageDescription	nvarchar(255),
@PageEncoding	nvarchar(255),
@AdditionalMetaTags	nvarchar(255),
@UseUrl		bit,
@Url			nvarchar(255),
@OpenInNewWindow	bit,
@ShowChildPageMenu	bit,
@HideMainMenu	bit,
@Skin			nvarchar(100),
@IncludeInMenu	bit,
@MenuImage			nvarchar(50),
@PageTitle         	nvarchar(255),
@AllowBrowserCache	bit


AS
IF NOT EXISTS (
    SELECT PageID 
    FROM 
        mp_Pages 
    WHERE 
        PageID = @PageID
)
INSERT INTO mp_Pages
(
	ParentID,
    	SiteID,
    	PageOrder,
   	PageName,
	PageTitle,
   	AuthorizedRoles,
	EditRoles,
	CreateChildPageRoles,
    	RequireSSL,
	AllowBrowserCache,
	ShowBreadcrumbs,
	ShowChildBreadCrumbs,
     	PageKeyWords,
	PageDescription	,
	PageEncoding,
	AdditionalMetaTags,
	UseUrl,
	Url,
	OpenInNewWindow,
	ShowChildPageMenu,
	HideMainMenu,
	Skin,
	IncludeInMenu,
	MenuImage 

) 
VALUES
 (
	@ParentID,
    	@SiteID,
    	@PageOrder,
    	@PageName,
		@PageTitle,
    	@AuthorizedRoles,
	@EditRoles,
	@CreateChildPageRoles,
    	@RequireSSL,
	@AllowBrowserCache,
	@ShowBreadcrumbs,
	@ShowChildPageBreadcrumbs,
	@PageKeyWords,
	@PageDescription,
	@PageEncoding,
	@AdditionalMetaTags,
	@UseUrl,
	@Url,
	@OpenInNewWindow,
	@ShowChildPageMenu,
	@HideMainMenu,
	@Skin,
	@IncludeInMenu,
	@MenuImage
   
)
ELSE
UPDATE
    mp_Pages

SET
	ParentID = @ParentID,
    	PageOrder = @PageOrder,
    	PageName = @PageName,
		PageTitle = @PageTitle,
    	AuthorizedRoles = @AuthorizedRoles,
	EditRoles = @EditRoles,
	CreateChildPageRoles = @CreateChildPageRoles,
    	RequireSSL = @RequireSSL,
	AllowBrowserCache = @AllowBrowserCache,
	ShowBreadcrumbs = @ShowBreadcrumbs,
	ShowChildBreadCrumbs = @ShowChildPageBreadcrumbs,
	PageKeyWords = @PageKeyWords,
	PageDescription = @PageDescription,
	PageEncoding = @PageEncoding,
	AdditionalMetaTags = @AdditionalMetaTags,
	UseUrl = @UseUrl,
	Url = @Url,
	OpenInNewWindow = @OpenInNewWindow,
	ShowChildPageMenu = @ShowChildPageMenu,
	HideMainMenu = @HideMainMenu,
	Skin = @Skin,
	IncludeInMenu = @IncludeInMenu,
	MenuImage = @MenuImage

WHERE
    PageID = @PageID
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'














CREATE PROCEDURE [dbo].[mp_Pages_Delete]
(
    @PageID int
)
AS

DELETE FROM
    mp_Pages

WHERE
    PageID = @PageID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Pages_SelectChildPages]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_Pages_SelectChildPages]

/*
Author:				
Created:			2/20/2005
Last Modified:			2/27/2005

*/


@ParentID		int


AS


SELECT	*

FROM		mp_Pages

WHERE	ParentID = @ParentID

ORDER BY PageOrder









' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_RssFeeds_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'






CREATE PROCEDURE [dbo].[mp_RssFeeds_Delete]

/*
Author:   			
Created: 			3/27/2005
Last Modified: 			3/27/2005

*/

@ItemID int

AS

DELETE FROM [dbo].[mp_RssFeeds]
WHERE
	[ItemID] = @ItemID






' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_RssFeeds_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'






CREATE PROCEDURE [dbo].[mp_RssFeeds_Update]

/*
Author:   			
Created: 			3/27/2005
Last Modified: 			3/27/2005


*/
	
@ItemID int, 
@ModuleID int, 
@Author nvarchar(100), 
@Url nvarchar(255), 
@RssUrl nvarchar(255) 


AS

UPDATE 		[dbo].[mp_RssFeeds] 

SET
			[ModuleID] = @ModuleID,
			[Author] = @Author,
			[Url] = @Url,
			[RssUrl] = @RssUrl
			
WHERE
			[ItemID] = @ItemID






' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_RssFeeds_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'






CREATE PROCEDURE [dbo].[mp_RssFeeds_Insert]

/*
Author:   			
Created: 			3/27/2005
Last Modified: 			3/27/2005


*/

@ModuleID int,
@UserID int,
@Author nvarchar(100),
@Url nvarchar(255),
@RssUrl nvarchar(255)

	
AS

INSERT INTO 	[dbo].[mp_RssFeeds] 
(
				[ModuleID],
				[CreatedDate],
				[UserID],
				[Author],
				[Url],
				[RssUrl]
) 

VALUES 
(
				@ModuleID,
				GetDate(),
				@UserID,
				@Author,
				@Url,
				@RssUrl
				
)
SELECT @@IDENTITY 






' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_RssFeeds_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'






CREATE PROCEDURE [dbo].[mp_RssFeeds_Select]

/*
Author:   			
Created: 			3/27/2005
Last Modified: 			3/27/2005

*/

@ModuleID		int

AS


SELECT
		[ItemID],
		[ModuleID],
		[CreatedDate],
		[UserID],
		[Author],
		[Url],
		[RssUrl]
		
FROM
		[dbo].[mp_RssFeeds]


WHERE	ModuleID = @ModuleID

ORDER BY	Author






' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_RssFeeds_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'






CREATE PROCEDURE [dbo].[mp_RssFeeds_SelectOne]

/*
Author:   			
Created: 			3/27/2005
Last Modified: 		  	3/27/2005


*/


@ItemID int

AS


SELECT
		[ItemID],
		[ModuleID],
		[CreatedDate],
		[UserID],
		[Author],
		[Url],
		[RssUrl]
		
FROM
		[dbo].[mp_RssFeeds]
		
WHERE
		[ItemID] = @ItemID






' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFiles_Delete]

/*
Author:   			
Created: 			1/5/2005
Last Modified: 			1/5/2005


*/

@ItemID int

AS

DELETE FROM [dbo].[mp_SharedFiles]
WHERE
	[ItemID] = @ItemID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFiles_SelectOne]

/*
Author:   			
Created: 			1/5/2005
Last Modified: 			1/10/2005

*/

@ItemID int

AS


SELECT
		[ItemID],
		[ModuleID],
		[UploadUserID],
		[FriendlyName],
		[OriginalFileName],
		[ServerFileName],
		[SizeInKB],
		[UploadDate],
		[FolderID]
		
FROM
		[dbo].[mp_SharedFiles]
		
WHERE
		[ItemID] = @ItemID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles_SelectByModule]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_SharedFiles_SelectByModule]

/*
Author:   			
Created: 			1/5/2005
Last Modified: 		1/12/2007

*/

@ModuleID		int,
@FolderID		int

AS
SELECT
		sf.[ItemID],
		sf.[ModuleID],
		sf.[UploadUserID],
		sf.[FriendlyName],
		sf.[OriginalFileName],
		sf.[ServerFileName],
		sf.[SizeInKB],
		sf.[UploadDate],
		sf.[FolderID],
		u.[Name] As UserName
		
FROM
		[dbo].[mp_SharedFiles] sf

LEFT OUTER JOIN
		mp_Users u
ON		sf.UploadUserID = u.UserID

WHERE	sf.ModuleID = @ModuleID
		AND sf.FolderID = @FolderID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFiles_Update]

/*
Author:   			
Created: 			1/5/2005
Last Modified: 			1/10/2005

*/
	
@ItemID int, 
@ModuleID int, 
@UploadUserID int, 
@FriendlyName nvarchar(255), 
@OriginalFileName nvarchar(255), 
@ServerFileName nvarchar(255), 
@SizeInKB int, 
@UploadDate datetime, 
@FolderID int 


AS

UPDATE 		[dbo].[mp_SharedFiles] 

SET
			[ModuleID] = @ModuleID,
			[UploadUserID] = @UploadUserID,
			[FriendlyName] = @FriendlyName,
			[OriginalFileName] = @OriginalFileName,
			[ServerFileName] = @ServerFileName,
			[SizeInKB] = @SizeInKB,
			[UploadDate] = @UploadDate,
			[FolderID] = @FolderID
			
WHERE
			[ItemID] = @ItemID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFiles_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFiles_Insert]

/*
Author:   			
Created: 			1/5/2005
Last Modified: 			1/10/2005


*/

@ModuleID int,
@UploadUserID int,
@FriendlyName nvarchar(255),
@OriginalFileName nvarchar(255),
@ServerFileName nvarchar(255),
@SizeInKB int,
@UploadDate datetime,
@FolderID int

	
AS

INSERT INTO 			[dbo].[mp_SharedFiles] 
(
				[ModuleID],
				[UploadUserID],
				[FriendlyName],
				[OriginalFileName],
				[ServerFileName],
				[SizeInKB],
				[UploadDate],
				[FolderID]
) 

VALUES 
(
				@ModuleID,
				@UploadUserID,
				@FriendlyName,
				@OriginalFileName,
				@ServerFileName,
				@SizeInKB,
				@UploadDate,
				@FolderID
				
)


SELECT @@IDENTITY










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFilesHistory_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFilesHistory_Select]

/*
Author:   			
Created: 			1/9/2005
Last Modified: 			1/10/2005

*/

@ModuleID	int,
@ItemID	int

AS


SELECT
		h.[ID],
		h.[ItemID],
		h.[ModuleID],
		h.[FriendlyName],
		h.OriginalFileName,
		h.[ServerFileName],
		h.SizeInKB,
		h.UploadDate,
		h.UploadUserID,
		h.[ArchiveDate],
		u.[Name]
		
FROM
		[dbo].[mp_SharedFilesHistory] h

LEFT OUTER JOIN	mp_Users u
ON			u.UserID = h.UploadUserID

WHERE	h.ModuleID = @ModuleID
		AND h.ItemID = @ItemID

ORDER BY 	h.ArchiveDate DESC










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFilesHistory_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFilesHistory_Insert]

/*
Author:   			
Created: 			1/9/2005
Last Modified: 			1/10/2005

*/

@ItemID int,
@ModuleID int,
@FriendlyName nvarchar(255),
@OriginalFileName nvarchar(255),
@ServerFileName nvarchar(50),
@SizeInKB	int,
@UploadDate datetime,
@UploadUserID	int,
@ArchiveDate datetime

	
AS

INSERT INTO 	[dbo].[mp_SharedFilesHistory] 
(
				[ItemID],
				[ModuleID],
				[FriendlyName],
				OriginalFileName,
				[ServerFileName],
				SizeInKB,
				UploadDate,
				UploadUserID,
				[ArchiveDate]
) 

VALUES 
(
				@ItemID,
				@ModuleID,
				@FriendlyName,
				@OriginalFileName,
				@ServerFileName,
				@SizeInKB,
				@UploadDate,
				@UploadUserID,
				@ArchiveDate
				
)
SELECT @@IDENTITY










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFilesHistory_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFilesHistory_Delete]

/*
Author:   			
Created: 			1/9/2005
Last Modified: 			1/9/2005

*/

@ID int

AS

DELETE FROM [dbo].[mp_SharedFilesHistory]
WHERE
	[ID] = @ID










' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SharedFilesHistory_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'










CREATE PROCEDURE [dbo].[mp_SharedFilesHistory_SelectOne]

/*
Author:   			
Created: 			1/9/2005
Last Modified: 			1/10/2005

*/


@ID	int

AS


SELECT
		[ID],
		[ItemID],
		[ModuleID],
		[FriendlyName],
		OriginalFileName,
		[ServerFileName],
		SizeInKB,
		UploadDate,
		UploadUserID,
		[ArchiveDate]
		
FROM
		[dbo].[mp_SharedFilesHistory]

WHERE	[ID] = @ID










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_UserProperties_Insert]

/*
Author:   			
Created: 			12/31/2006
Last Modified: 		12/31/2006
*/

@PropertyID uniqueidentifier,
@UserGuid uniqueidentifier,
@PropertyName nvarchar(255),
@PropertyValueString ntext,
@PropertyValueBinary image,
@LastUpdatedDate datetime,
@IsLazyLoaded bit

	
AS



INSERT INTO 	[dbo].[mp_UserProperties] 
(
				[PropertyID],
				[UserGuid],
				[PropertyName],
				[PropertyValueString],
				[PropertyValueBinary],
				[LastUpdatedDate],
				[IsLazyLoaded]
) 

VALUES 
(
				@PropertyID,
				@UserGuid,
				@PropertyName,
				@PropertyValueString,
				@PropertyValueBinary,
				@LastUpdatedDate,
				@IsLazyLoaded
				
)
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_UserProperties_Update]

/*
Author:   			
Created: 			12/31/2006
Last Modified: 		12/31/2006
*/
	 
@UserGuid uniqueidentifier, 
@PropertyName nvarchar(255), 
@PropertyValueString ntext, 
@PropertyValueBinary image, 
@LastUpdatedDate datetime, 
@IsLazyLoaded bit 


AS

UPDATE 		[dbo].[mp_UserProperties] 

SET
			[UserGuid] = @UserGuid,
			[PropertyName] = @PropertyName,
			[PropertyValueString] = @PropertyValueString,
			[PropertyValueBinary] = @PropertyValueBinary,
			[LastUpdatedDate] = @LastUpdatedDate,
			[IsLazyLoaded] = @IsLazyLoaded
			
WHERE
			[UserGuid] = @UserGuid
			AND [PropertyName] = @PropertyName' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties_PropertyExists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_UserProperties_PropertyExists]

/*
Author:			
Created:		12/31/2006
Last Modified:	12/31/2006

*/
    
@UserGuid  	uniqueidentifier,
@PropertyName	nvarchar(255)

AS
SELECT Count(*)
FROM mp_UserProperties
WHERE UserGuid = @UserGuid
AND [PropertyName] = @PropertyName

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_UserProperties_Delete]

/*
Author:   			
Created: 			12/31/2006
Last Modified: 		12/31/2006
*/

@UserGuid uniqueidentifier,
@PropertyName	nvarchar(255)

AS

DELETE FROM [dbo].[mp_UserProperties]
WHERE
	[UserGuid] = @UserGuid
	AND [PropertyName] = @PropertyName' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties_SelectByUser]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_UserProperties_SelectByUser]

/*
Author:   			
Created: 			12/31/2006
Last Modified: 		12/31/2006
*/

@UserGuid uniqueidentifier


AS


SELECT	
		[PropertyID],
		[UserGuid],
		[PropertyName],
		[PropertyValueString],
		[PropertyValueBinary],
		[LastUpdatedDate],
		[IsLazyLoaded]
		
FROM
		[dbo].[mp_UserProperties]
		
WHERE
		[UserGuid] = @UserGuid
		AND IsLazyLoaded = 0' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserProperties_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_UserProperties_SelectOne]

/*
Author:   			
Created: 			12/31/2006
Last Modified: 		12/31/2006
*/

@UserGuid uniqueidentifier,
@PropertyName nvarchar(255)

AS


SELECT	TOP 1
		[PropertyID],
		[UserGuid],
		[PropertyName],
		[PropertyValueString],
		[PropertyValueBinary],
		[LastUpdatedDate],
		[IsLazyLoaded]
		
FROM
		[dbo].[mp_UserProperties]
		
WHERE
		[UserGuid] = @UserGuid
		AND PropertyName = @PropertyName' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteModuleDefinitions_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_SiteModuleDefinitions_Delete]


/*
Author:			
Created:		3/12/2005
Last Modified:		3/12/2005

*/


@SiteID		int,
@ModuleDefID		int


AS

DELETE FROM mp_SiteModuleDefinitions
WHERE	SiteID = @SiteID 
		AND ModuleDefID = @ModuleDefID








' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteModuleDefinitions_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_SiteModuleDefinitions_Insert]

/*
Author:			
Created:		3/12/2005
Last Modified:		3/12/2005

*/


@SiteID		int,
@ModuleDefID		int



AS


IF NOT EXISTS (SELECT SiteID FROM mp_SiteModuleDefinitions WHERE SiteID = @SiteID AND ModuleDefID = @ModuleDefID)
INSERT INTO mp_SiteModuleDefinitions (SiteID, ModuleDefID)

VALUES	(@SiteID, @ModuleDefID)








' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectByEmail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_Users_SelectByEmail]

    
@SiteID	int,
@Email 		nvarchar(100)


AS

SELECT	*

FROM
    mp_Users

WHERE
	SiteID = @SiteID
   	AND Email = @Email








' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[mp_Users_Delete]
(
    @UserID int
)
AS

DELETE FROM
    mp_Users

WHERE
    UserID=@UserID


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_ConfirmRegistration]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[mp_Users_ConfirmRegistration] 

@EmptyGuid					uniqueidentifier,
@RegisterConfirmGuid		uniqueidentifier

AS
UPDATE	mp_Users
SET		IsLockedOut = 0,
		RegisterConfirmGuid = @EmptyGuid
		

WHERE	RegisterConfirmGuid = @RegisterConfirmGuid
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_FlagAsDeleted]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[mp_Users_FlagAsDeleted]


@UserID int

AS

UPDATE
    mp_Users

SET	IsDeleted = 1

WHERE
    UserID=@UserID


' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_DecrementTotalPosts]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_Users_DecrementTotalPosts]

/*
Author:				
Created:			10/3/2004
Last Modified:			10/3/2004

*/

@UserID		int

AS


UPDATE		mp_Users

SET			TotalPosts = TotalPosts - 1

WHERE		UserID = @UserID











' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SetRegistrationConfirmationGuid]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[mp_Users_SetRegistrationConfirmationGuid] 

@UserGuid					uniqueidentifier,
@RegisterConfirmGuid		uniqueidentifier

AS
UPDATE	mp_Users
SET		IsLockedOut = 1,
		RegisterConfirmGuid = @RegisterConfirmGuid
		

WHERE	UserGuid = @UserGuid
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_LoginByEmail]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'





CREATE Procedure [dbo].[mp_Users_LoginByEmail]

   
@SiteID	int, 
@Email    	nvarchar(100), 
@Password 	nvarchar(20), 
@UserName 	nvarchar(100) OUTPUT

AS

SELECT
    @UserName = Name

FROM
    mp_Users

WHERE
		SiteID = @SiteID
    		AND Email = @Email
  		AND [Password] = @Password


' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_AccountClearLockout]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_Users_AccountClearLockout]

@UserGuid		uniqueidentifier


AS

UPDATE	mp_Users
SET		IsLockedOut = 0
		

WHERE	UserGuid = @UserGuid' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_UpdateLastPasswordChangeTime]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_Users_UpdateLastPasswordChangeTime]


@UserID	uniqueidentifier,
@PasswordChangeTime	datetime


AS

UPDATE mp_Users WITH (ROWLOCK)
SET LastPasswordChangedDate = @PasswordChangeTime
WHERE UserGuid = @UserID' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserRoles_SelectByRoleID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_UserRoles_SelectByRoleID]

    
@RoleID  int

AS

SELECT  
    		ur.UserID,
    		u.[Name],
    		u.Email,
		u.LoginName

FROM
    		mp_UserRoles ur
    
JOIN 		mp_Users  u
ON 		u.UserID = ur.UserID

WHERE   
    		ur.RoleID = @RoleID' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectByLoginName]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[mp_Users_SelectByLoginName]

    
@SiteID	int,
@LoginName 		nvarchar(50)


AS

SELECT	*

FROM
    mp_Users

WHERE
	SiteID = @SiteID
   	AND LoginName = @LoginName


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE Procedure [dbo].[mp_Users_Insert]

/*
Author:			
Created:		9/30/2004
Last Modified:		1/14/2007

*/

@SiteID	int,
@Name     	nvarchar(50),
@LoginName	nvarchar(50),
@Email    	nvarchar(100),
@Password 	nvarchar(50),
@UserGuid	uniqueidentifier,
@DateCreated datetime


AS
INSERT INTO 		mp_Users
(
			SiteID,
    			[Name],
			LoginName,
    			Email,
    			[Password],
			UserGuid,
			DateCreated
	

)

VALUES
(
			@SiteID,
    			@Name,
			@LoginName,
    			@Email,
    			@Password,
			@UserGuid,
			@DateCreated
)

SELECT		@@Identity As UserID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SmartDropDown]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Users_SmartDropDown]

/*
Author:		
Created:	6/19/2005
Last Modified:	6/19/2005

*/


@SiteID			int,
@Query				nvarchar(50),
@RowsToGet			int


AS


SET ROWCOUNT @RowsToGet


SELECT 		u1.UserID,
			u1.[Name] AS SiteUser

FROM			mp_Users u1

WHERE		u1.SiteID = @SiteID
			AND u1.[Name] LIKE @Query + ''%''

UNION

SELECT 		u2.UserID,
			u2.[Email] As SiteUser

FROM			mp_Users u2

WHERE		u2.SiteID = @SiteID
			AND u2.[Email] LIKE @Query + ''%''

ORDER BY		SiteUser







' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_Login]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE Procedure [dbo].[mp_Users_Login]

   
@SiteID	int, 
@LoginName    	nvarchar(50), 
@Password 	nvarchar(128), 
@UserName 	nvarchar(100) OUTPUT

AS
SELECT
    @UserName = Name

FROM
    mp_Users

WHERE
		SiteID = @SiteID
    		AND LoginName = @LoginName
  		AND [Password] = @Password
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_AccountLockout]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_Users_AccountLockout]

@UserGuid		uniqueidentifier,
@LockoutTime		datetime

AS

UPDATE	mp_Users
SET		IsLockedOut = 1,
		LastLockoutDate = @LockoutTime

WHERE	UserGuid = @UserGuid' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectByGuid]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'









CREATE PROCEDURE [dbo].[mp_Users_SelectByGuid]

/*
Author:			
Created:		4/15/2006
Last Modified:		4/15/2006

*/

@UserGuid	uniqueidentifier

AS

SELECT	*

FROM		mp_Users

WHERE	UserGuid = @UserGuid' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Users_Select]


@SiteID		int

AS
SELECT  
    UserID,
	LoginName,
    Email,
	Password

FROM
    mp_Users

WHERE SiteID = @SiteID
    
ORDER BY Email
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_UpdatePasswordQuestionAndAnswer]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_Users_UpdatePasswordQuestionAndAnswer]


@UserID		uniqueidentifier,
@PasswordQuestion	nvarchar(255),
@PasswordAnswer	nvarchar(255)


AS

UPDATE mp_Users WITH (ROWLOCK)
SET PasswordQuestion = @PasswordQuestion,
	PasswordAnswer = @PasswordAnswer

WHERE UserGuid = @UserID' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserRoles_SelectNotInRole]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_UserRoles_SelectNotInRole]

@SiteID	int,
@RoleID  int

AS
SELECT  DISTINCT
    		u.UserID,
    		u.[Name],
    		u.Email,
			u.LoginName

FROM		mp_Users  u
    		
    
LEFT OUTER JOIN 		
		mp_UserRoles ur

ON 		u.UserID = ur.UserID
		AND ur.RoleID = @RoleID

WHERE		u.SiteID = @SiteID
    		
			AND ur.RoleID IS NULL

ORDER BY	u.[Name]

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_CountOnlineSinceTime]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_Users_CountOnlineSinceTime]

/*
Author:			
Created:		4/15/2006
Last Modified:		4/15/2006

*/

@SiteID		int,
@SinceTime		datetime

AS

SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
		AND LastActivityDate > @SinceTime
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Users_Update]

/*
Author:			
Created:		9/30/2004
Last Modified:		11/8/2006

*/

    
@UserID        			int,   
@Name				nvarchar(100),
@LoginName			nvarchar(50),
@Email           			nvarchar(100),   
@Password    			nvarchar(20),
@Gender			nchar(1),
@ProfileApproved		bit,
@ApprovedForForums		bit,
@Trusted			bit,
@DisplayInMemberList		bit,
@WebSiteURL			nvarchar(100),
@Country			nvarchar(100),
@State				nvarchar(100),
@Occupation			nvarchar(100),
@Interests			nvarchar(100),
@MSN				nvarchar(50),
@Yahoo			nvarchar(50),
@AIM				nvarchar(50),
@ICQ				nvarchar(50),
@AvatarUrl			nvarchar(255),
@Signature			nvarchar(255),
@Skin				nvarchar(100),

@LoweredEmail		nvarchar(100),
@PasswordQuestion		nvarchar(255),
@PasswordAnswer		nvarchar(255),
@Comment		ntext,
@TimeOffsetHours	int



AS
UPDATE		mp_Users

SET			[Name] = @Name,
			LoginName = @LoginName,
			Email = @Email,
    			[Password] = @Password,
			Gender = @Gender,
			ProfileApproved = @ProfileApproved,
			ApprovedForForums = @ApprovedForForums,
			Trusted = @Trusted,
			DisplayInMemberList = @DisplayInMemberList,
			WebSiteURL = @WebSiteURL,
			Country = @Country,
			State = @State,
			Occupation = @Occupation,
			Interests = @Interests,
			MSN = @MSN,
			Yahoo = @Yahoo,
			AIM = @AIM,
			ICQ = @ICQ,
			AvatarUrl = @AvatarUrl,
			Signature = @Signature,
			Skin = @Skin,
			LoweredEmail = @LoweredEmail,
			PasswordQuestion = @PasswordQuestion,
			PasswordAnswer = @PasswordAnswer,
			Comment = @Comment,
			TimeOffsetHours = @TimeOffsetHours
			
WHERE		UserID = @UserID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SetFailedPasswordAttemptStartWindow]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Users_SetFailedPasswordAttemptStartWindow]

/*
Author:				
Created:			1/18/2007
Last Modified:		1/18/2007

*/

@UserGuid uniqueidentifier,
@WindowStartTime datetime

AS
UPDATE		mp_Users

SET			

FailedPasswordAttemptWindowStart = @WindowStartTime


WHERE		UserGuid = @UserGuid

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_Count]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_Users_Count]

/*
Author:			
Created:		11/29/2004
Last Modified:		5/12/2005

*/

@SiteID		int

AS

SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_GetNewestUserID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Users_GetNewestUserID]

/*
Author:			
Created:		12/3/2006
Last Modified:	12/3/2006

*/

@SiteID		int

AS
SELECT  	MAX(UserID)

FROM		mp_Users

WHERE	SiteID = @SiteID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_IncrementTotalPosts]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_Users_IncrementTotalPosts]

/*
Author:				
Created:			10/3/2004
Last Modified:			10/3/2004

*/

@UserID		int

AS


UPDATE		mp_Users

SET			TotalPosts = TotalPosts + 1

WHERE		UserID = @UserID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_UpdateLastLoginTime]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_Users_UpdateLastLoginTime]


@UserID	uniqueidentifier,
@LastLoginTime	datetime


AS

UPDATE mp_Users WITH (ROWLOCK)
SET LastLoginDate = @LastLoginTime
WHERE UserGuid = @UserID' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_Users_SelectOne]

/*
Author:			
Created:		10/3/2004
Last Modified:		10/3/2004

*/

@UserID		int

AS

SELECT	*

FROM		mp_Users

WHERE	UserID = @UserID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_UpdateLastActivityTime]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_Users_UpdateLastActivityTime]


@UserID	uniqueidentifier,
@LastUpdate	datetime


AS

UPDATE mp_Users WITH (ROWLOCK)
SET LastActivityDate = @LastUpdate
WHERE UserGuid = @UserID
	' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_CountByFirstLetter]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Users_CountByFirstLetter]

/*
Author:			
Created:		12/7/2006
Last Modified:	12/7/2006

*/

@SiteID		int,
@UserNameBeginsWith 		nvarchar(1)

AS
SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
AND (
	(LEFT([Name], 1) = @UserNameBeginsWith)
	OR @UserNameBeginsWith = ''''
	)

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SetFailedPasswordAnswerAttemptCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Users_SetFailedPasswordAnswerAttemptCount]

/*
Author:				
Created:			1/18/2007
Last Modified:		1/18/2007

*/

@UserGuid uniqueidentifier,
@AttemptCount int

AS
UPDATE		mp_Users

SET			

FailedPasswordAnswerAttemptCount = @AttemptCount


WHERE		UserGuid = @UserGuid
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectUsersOnlineSinceTime]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Users_SelectUsersOnlineSinceTime]

/*
Author:			
Created:		11/7/2006
Last Modified:	11/7/2006

*/

@SiteID		int,
@SinceTime		datetime

AS
SELECT  	*

FROM		mp_Users

WHERE	SiteID = @SiteID
		AND LastActivityDate > @SinceTime
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SelectTop50UsersOnlineSinceTime]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Users_SelectTop50UsersOnlineSinceTime]

/*
Author:			
Created:		11/7/2006
Last Modified:	11/7/2006

*/

@SiteID		int,
@SinceTime		datetime

AS
SELECT TOP 50 	*

FROM		mp_Users

WHERE	SiteID = @SiteID
		AND LastActivityDate > @SinceTime

ORDER BY LastActivityDate desc

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SetFailedPasswordAttemptCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Users_SetFailedPasswordAttemptCount]

/*
Author:				
Created:			1/18/2007
Last Modified:		1/18/2007

*/

@UserGuid uniqueidentifier,
@AttemptCount int

AS
UPDATE		mp_Users

SET			

FailedPasswordAttemptCount = @AttemptCount


WHERE		UserGuid = @UserGuid

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_CountByRegistrationDateRange]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Users_CountByRegistrationDateRange]

/*
Author:				
Created:			12/3/2006
Last Modified:		12/3/2006

*/

@SiteID		int,
@BeginDate	datetime,
@EndDate	datetime

AS
SELECT  	COUNT(*)

FROM		mp_Users

WHERE	SiteID = @SiteID
AND DateCreated Between @BeginDate And @EndDate


' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Users_SetFailedPasswordAnswerAttemptStartWindow]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Users_SetFailedPasswordAnswerAttemptStartWindow]

/*
Author:				
Created:			1/18/2007
Last Modified:		1/18/2007

*/

@UserGuid uniqueidentifier,
@WindowStartTime datetime

AS
UPDATE		mp_Users

SET			

FailedPasswordAnswerAttemptWindowStart = @WindowStartTime


WHERE		UserGuid = @UserGuid
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserRoles_DeleteUserRoles]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[mp_UserRoles_DeleteUserRoles]

/*
Author:			
Created:		11/30/2005
Last Modified:		11/30/2005

*/
    
@UserID int

AS

DELETE FROM	mp_UserRoles

WHERE	UserID = @UserID


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserRoles_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'













CREATE Procedure [dbo].[mp_UserRoles_Insert]
(
	@RoleID int,
    	@UserID int
    
)
AS

SELECT 
    *
FROM
    mp_UserRoles

WHERE
    UserID=@UserID
    AND
    RoleID=@RoleID

/* only insert if the record doesn''t yet exist */
IF @@Rowcount < 1

    INSERT INTO mp_UserRoles
    (
        UserID,
        RoleID
    )

    VALUES
    (
        @UserID,
        @RoleID
    )











' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserRoles_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'













CREATE PROCEDURE [dbo].[mp_UserRoles_Delete]
(
   
    	@RoleID int,
	 @UserID int
)
AS

DELETE FROM
    mp_UserRoles

WHERE
    UserID=@UserID
    AND
    RoleID=@RoleID











' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_UserPages_Insert]

/*
Author:   			
Created: 			6/12/2006
Last Modified: 		6/14/2006
*/

@UserPageID	uniqueidentifier,
@SiteID int,
@UserGuid uniqueidentifier,
@PageName nvarchar(255),
@PagePath nvarchar(255),
@PageOrder int

	
AS
INSERT INTO 	[dbo].[mp_UserPages] 
(
				[UserPageID],
				[SiteID],
				[UserGuid],
				[PageName],
				PagePath,
				PageOrder
) 

VALUES 
(
				@UserPageID,
				@SiteID,
				@UserGuid,
				@PageName,
				@PagePath,
				@PageOrder
				
)
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_UserPages_Delete]

/*
Author:   			
Created: 			6/12/2006
Last Modified: 		6/12/2006
*/

@UserPageID uniqueidentifier

AS

DELETE FROM [dbo].[mp_UserPages]
WHERE
	[UserPageID] = @UserPageID' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_SelectByUser]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_UserPages_SelectByUser]

/*
Author:   			
Created: 			6/12/2006
Last Modified: 		6/12/2006
*/

@UserGuid uniqueidentifier

AS


SELECT
		*
		
FROM
		[dbo].[mp_UserPages]
		
WHERE
		[UserGuid] = @UserGuid

ORDER BY PageOrder' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_UpdatePageOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_UserPages_UpdatePageOrder]
(
    @UserPageID       uniqueidentifier,
    @PageOrder        int
)
AS
UPDATE
    mp_UserPages

SET
    PageOrder = @PageOrder

WHERE
    UserPageID = @UserPageID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_GetNextPageOrder]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_UserPages_GetNextPageOrder]

/*
Author:			
Created:		11/7/2004
Last Modified:		11/7/2004

*/

@UserGuid uniqueidentifier

AS
SELECT	COALESCE(MAX(PageOrder), -1) + 2

FROM		mp_UserPages

WHERE	UserGuid = @UserGuid
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_UserPages_Update]

/*
Author:   			
Created: 			6/12/2006
Last Modified: 		6/12/2006
*/
	
@UserPageID uniqueidentifier, 
@PageName nvarchar(255), 
@PageOrder int 


AS

UPDATE 		[dbo].[mp_UserPages] 

SET
			[PageName] = @PageName,
			[PageOrder] = @PageOrder
			
WHERE
			[UserPageID] = @UserPageID' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserPages_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_UserPages_SelectOne]

/*
Author:   			
Created: 			6/12/2006
Last Modified: 		6/12/2006
*/

@UserPageID uniqueidentifier

AS
SELECT
		*
		
FROM
		[dbo].[mp_UserPages]
		
WHERE
		[UserPageID] = @UserPageID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'





CREATE PROCEDURE [dbo].[mp_CalendarEvents_Delete]

/*
Author:   			
Created: 			4/10/2005
Last Modified: 			4/10/2005

*/

@ItemID int

AS

DELETE FROM [dbo].[mp_CalendarEvents]
WHERE
	[ItemID] = @ItemID





' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'





CREATE PROCEDURE [dbo].[mp_CalendarEvents_SelectOne]

/*
Author:   			
Created: 			4/10/2005
Last Modified: 			4/10/2005


*/

@ItemID int

AS


SELECT
		[ItemID],
		[ModuleID],
		[Title],
		[Description],
		[ImageName],
		[EventDate],
		[StartTime],
		[EndTime],
		[CreatedDate],
		[UserID]
		
FROM
		[dbo].[mp_CalendarEvents]
		
WHERE
		[ItemID] = @ItemID





' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'





CREATE PROCEDURE [dbo].[mp_CalendarEvents_Insert]

/*
Author:   			
Created: 			4/10/2005
Last Modified: 			4/10/2005

*/

@ModuleID int,
@Title nvarchar(255),
@Description ntext,
@ImageName nvarchar(100),
@EventDate datetime,
@StartTime smalldatetime,
@EndTime smalldatetime,
@UserID int

	
AS

INSERT INTO 	[dbo].[mp_CalendarEvents] 
(
				[ModuleID],
				[Title],
				[Description],
				[ImageName],
				[EventDate],
				[StartTime],
				[EndTime],
				[CreatedDate],
				[UserID]
) 

VALUES 
(
				@ModuleID,
				@Title,
				@Description,
				@ImageName,
				@EventDate,
				@StartTime,
				@EndTime,
				GetDate(),
				@UserID
				
)
SELECT @@IDENTITY 





' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'





CREATE PROCEDURE [dbo].[mp_CalendarEvents_Update]

/*
Author:   			
Created: 			4/10/2005
Last Modified: 			4/10/2005

*/
	
@ItemID int, 
@ModuleID int, 
@Title nvarchar(255), 
@Description ntext, 
@ImageName nvarchar(100), 
@EventDate datetime, 
@StartTime smalldatetime, 
@EndTime smalldatetime, 
@UserID int 


AS

UPDATE 		[dbo].[mp_CalendarEvents] 

SET
			[ModuleID] = @ModuleID,
			[Title] = @Title,
			[Description] = @Description,
			[ImageName] = @ImageName,
			[EventDate] = @EventDate,
			[StartTime] = @StartTime,
			[EndTime] = @EndTime,
			[UserID] = @UserID
			
WHERE
			[ItemID] = @ItemID





' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_CalendarEvents_SelectByDate]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_CalendarEvents_SelectByDate]

/*
Author:   			
Created: 			4/10/2005
Last Modified: 			4/14/2005

*/

@ModuleID		int,
@BeginDate		datetime,
@EndDate		datetime

AS
SELECT
		[ItemID],
		[ModuleID],
		[Title],
		[Description],
		[ImageName],
		[EventDate],
		[StartTime],
		[EndTime],
		[CreatedDate],
		[UserID]
		
FROM
		[dbo].[mp_CalendarEvents]

WHERE	ModuleID = @ModuleID
		AND (EventDate >= @BeginDate)
		AND (EventDate <= @EndDate)

ORDER BY	EventDate, DATEPART(hh, StartTime)
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumThreads_Update]

/*
Author:			
Created:		9/19/2004
Last Modified:		9/19/2004

*/

@ThreadID			int,
@ForumID			int,
@ThreadSubject		nvarchar(255),
@SortOrder			int,
@IsLocked			bit


AS


UPDATE		mp_ForumThreads

SET			ForumID = @ForumID,
			ThreadSubject = @ThreadSubject,
			SortOrder = @SortOrder,
			IsLocked = @IsLocked


WHERE		ThreadID = @ThreadID












' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_IncrementReplyCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_ForumThreads_IncrementReplyCount]

/*
Author:			
Created:		9/19/2004
Last Modified:		1/14/2006

*/

@ThreadID			int,
@MostRecentPostUserID	int,
@MostRecentPostDate datetime



AS
UPDATE		mp_ForumThreads

SET			MostRecentPostUserID = @MostRecentPostUserID,
			TotalReplies = TotalReplies + 1,
			MostRecentPostDate = @MostRecentPostDate


WHERE		ThreadID = @ThreadID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ForumThreads_UpdateViewStats]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'











CREATE PROCEDURE [dbo].[mp_ForumThreads_UpdateViewStats]

/*
Author:			
Created:		9/19/2004
Last Modified:		9/19/2004

*/

@ThreadID			int



AS


UPDATE		mp_ForumThreads

SET		
			TotalViews = TotalViews + 1


WHERE		ThreadID = @ThreadID











' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_Sites_SelectOne]

/*
Author:   			
Created: 			3/7/2005
Last Modified: 			5/29/2005

*/

@SiteID int

AS


SELECT
		*
		
FROM
		[dbo].[mp_Sites]
		
WHERE
		[SiteID] = @SiteID

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Sites_Insert]

/*
Author:   			
Created: 			3/7/2005
Last Modified: 			5/21/2006

*/


@SiteName 				nvarchar(255),
@Skin 					nvarchar(100),
@Logo 					nvarchar(50),
@Icon 					nvarchar(50),
@AllowUserSkins 			bit,
@AllowNewRegistration 			bit,
@UseSecureRegistration 		bit,
@UseSSLOnAllPages 			bit,
@DefaultPageKeyWords 		nvarchar(255),
@DefaultPageDescription 		nvarchar(255),
@DefaultPageEncoding 			nvarchar(255),
@DefaultAdditionalMetaTags 		nvarchar(255),
@IsServerAdminSite 			bit,
@AllowPageSkins			bit,
@AllowHideMenuOnPages		bit,
@UseLdapAuth				bit,
@AutoCreateLdapUserOnFirstLogin	bit,
@LdapServer				nvarchar(255),
@LdapPort				int,
@LdapDomain				nvarchar(255),
@LdapRootDN				nvarchar(255),
@LdapUserDNKey			nvarchar(10),
@AllowUserFullNameChange		bit,
@UseEmailForLogin			bit,
@ReallyDeleteUsers			bit,
@EditorSkin				nvarchar(50),
@DefaultFriendlyUrlPatternEnum		nvarchar(50),
@SiteGuid					uniqueidentifier,
@EnableMyPageFeature 			bit

	
AS
INSERT INTO 	[dbo].[mp_Sites] 
(
				
				[SiteName],
				[Skin],
				[Logo],
				[Icon],
				[AllowUserSkins],
				[AllowNewRegistration],
				[UseSecureRegistration],
				[UseSSLOnAllPages],
				[DefaultPageKeyWords],
				[DefaultPageDescription],
				[DefaultPageEncoding],
				[DefaultAdditionalMetaTags],
				[IsServerAdminSite],
				AllowPageSkins,
				AllowHideMenuOnPages,
				UseLdapAuth,
				AutoCreateLdapUserOnFirstLogin,
				LdapServer,
				LdapPort,
				LdapDomain,
				LdapRootDN,
				LdapUserDNKey,
				ReallyDeleteUsers,
				UseEmailForLogin,
				AllowUserFullNameChange,
				EditorSkin,
				DefaultFriendlyUrlPatternEnum,
				SiteGuid,
				EnableMyPageFeature
) 

VALUES 
(
				
				@SiteName,
				@Skin,
				@Logo,
				@Icon,
				@AllowUserSkins,
				@AllowNewRegistration,
				@UseSecureRegistration,
				@UseSSLOnAllPages,
				@DefaultPageKeyWords,
				@DefaultPageDescription,
				@DefaultPageEncoding,
				@DefaultAdditionalMetaTags,
				@IsServerAdminSite,
				@AllowPageSkins,
				@AllowHideMenuOnPages,
				@UseLdapAuth,
				@AutoCreateLdapUserOnFirstLogin,
				@LdapServer,
				@LdapPort,
				@LdapDomain,
				@LdapRootDN,
				@LdapUserDNKey,
				@ReallyDeleteUsers,
				@UseEmailForLogin,
				@AllowUserFullNameChange,
				@EditorSkin,
				@DefaultFriendlyUrlPatternEnum,
				@SiteGuid,
				@EnableMyPageFeature
				
)
SELECT @@IDENTITY
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Sites_Update]

/*
Author:		
Last Modified:	5/21/2006

*/


@SiteID           			int,
@SiteName         		nvarchar(128),
@Skin				nvarchar(100),
@Logo				nvarchar(50),
@Icon				nvarchar(50),
@AllowNewRegistration		bit,
@AllowUserSkins		bit,
@UseSecureRegistration	bit,
@UseSSLOnAllPages		bit,
@DefaultPageKeywords		nvarchar(255),
@DefaultPageDescription	nvarchar(255),
@DefaultPageEncoding		nvarchar(255),
@DefaultAdditionalMetaTags	nvarchar(255),
@IsServerAdminSite		bit,
@AllowPageSkins		bit,
@AllowHideMenuOnPages	bit,
@UseLdapAuth				bit,
@AutoCreateLdapUserOnFirstLogin	bit,
@LdapServer				nvarchar(255),
@LdapPort				int,
@LdapRootDN				nvarchar(255),
@LdapUserDNKey			nvarchar(10),
@AllowUserFullNameChange		bit,
@UseEmailForLogin			bit,
@ReallyDeleteUsers			bit,
@EditorSkin				nvarchar(50),
@DefaultFriendlyUrlPatternEnum		nvarchar(50),
@EnableMyPageFeature		bit,
@LdapDomain				nvarchar(255)
	
	

AS
UPDATE	mp_Sites

SET
    	SiteName = @SiteName,
	Skin = @Skin,
	Logo = @Logo,
	Icon = @Icon,
	AllowNewRegistration = @AllowNewRegistration,
	AllowUserSkins = @AllowUserSkins,
	UseSecureRegistration = @UseSecureRegistration,
	UseSSLOnAllPages = @UseSSLOnAllPages,
	DefaultPageKeywords = @DefaultPageKeywords,
	DefaultPageDescription = @DefaultPageDescription,
	DefaultPageEncoding = @DefaultPageEncoding,
	DefaultAdditionalMetaTags = @DefaultAdditionalMetaTags,
	IsServerAdminSite = @IsServerAdminSite,
	AllowPageSkins = @AllowPageSkins,
	AllowHideMenuOnPages = @AllowHideMenuOnPages,
	UseLdapAuth = @UseLdapAuth,
	AutoCreateLdapUserOnFirstLogin = @AutoCreateLdapUserOnFirstLogin,
	LdapServer = @LdapServer,
	LdapPort = @LdapPort,
    LdapDomain = @LdapDomain,
	LdapRootDN = @LdapRootDN,
	LdapUserDNKey = @LdapUserDNKey,
	AllowUserFullNameChange = @AllowUserFullNameChange,
	UseEmailForLogin = @UseEmailForLogin,
	ReallyDeleteUsers = @ReallyDeleteUsers,
	EditorSkin = @EditorSkin,
	DefaultFriendlyUrlPatternEnum = @DefaultFriendlyUrlPatternEnum,
	EnableMyPageFeature = @EnableMyPageFeature

WHERE
    	SiteID = @SiteID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_SelectOneByHost]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Sites_SelectOneByHost]

/*
Author:   			
Created: 			8/27/2006
Last Modified: 		8/27/2006

*/

@HostName		nvarchar(255)

AS

DECLARE @SiteID int

SET @SiteID = COALESCE(	(SELECT TOP 1 SiteID FROM mp_SiteHosts WHERE HostName = @HostName),
				 (SELECT TOP 1 SiteID FROM mp_Sites ORDER BY SiteID)
			)

SELECT
		*
		
FROM
		[dbo].[mp_Sites]
		
WHERE
		[SiteID] = @SiteID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_Sites_Delete]

/*
Author:   			
Created: 			3/7/2005
Last Modified: 			3/7/2005

*/

@SiteID int

AS

DELETE FROM [dbo].[mp_Sites]
WHERE
	[SiteID] = @SiteID








' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_UpdateExtendedProperties]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[mp_Sites_UpdateExtendedProperties] 
	-- Add the parameters for the stored procedure here

@SiteID							int,
@AllowPasswordRetrieval			bit,
@AllowPasswordReset				bit,
@RequiresQuestionAndAnswer		bit,
@MaxInvalidPasswordAttempts		int,
@PasswordAttemptWindowMinutes	int,
@RequiresUniqueEmail			bit,
@PasswordFormat					int,
@MinRequiredPasswordLength		int,
@MinRequiredNonAlphanumericCharacters	int,
@PasswordStrengthRegularExpression	ntext,
@DefaultEmailFromAddress		nvarchar(100)

AS

UPDATE			mp_Sites

SET				AllowPasswordRetrieval = @AllowPasswordRetrieval,
				AllowPasswordReset = @AllowPasswordReset,
				RequiresQuestionAndAnswer = @RequiresQuestionAndAnswer,
				MaxInvalidPasswordAttempts = @MaxInvalidPasswordAttempts,
				PasswordAttemptWindowMinutes = @PasswordAttemptWindowMinutes,
				RequiresUniqueEmail = @RequiresUniqueEmail,
				PasswordFormat = @PasswordFormat,
				MinRequiredPasswordLength  = @MinRequiredPasswordLength,
				MinRequiredNonAlphanumericCharacters = @MinRequiredNonAlphanumericCharacters,
				PasswordStrengthRegularExpression = @PasswordStrengthRegularExpression,
				DefaultEmailFromAddress = @DefaultEmailFromAddress



WHERE			SiteID = @SiteID

' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_Count]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_Sites_Count]

/*
Author:		
Created:	3/13/2005
Last Modified:	3/13/2005

*/

AS

SELECT Count(*) FROM mp_Sites








' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_Sites_SelectAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_Sites_SelectAll]

/*
Author:   			
Created: 			3/7/2005
Last Modified: 			3/7/2005

*/

AS
SELECT
		*
		
FROM
		[dbo].[mp_Sites]
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_BlogCategories_Insert]

/*
Author:   			
Created: 			6/7/2005
Last Modified: 			6/12/2005

*/

@ModuleID int,
@Category nvarchar(255)

	
AS

IF NOT EXISTS (SELECT CategoryID FROM mp_BlogCategories WHERE ModuleID = @ModuleID AND Category = @Category)
BEGIN

INSERT INTO 	[dbo].[mp_BlogCategories] 
(
				[ModuleID],
				[Category]
) 

VALUES 
(
				@ModuleID,
				@Category
				
)
SELECT @@IDENTITY 
END



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_BlogCategories_Update]

/*
Author:   			
Created: 			6/7/2005
Last Modified: 			9/11/2005

*/
	
@CategoryID int, 
@Category nvarchar(255) 


AS

UPDATE 		[dbo].[mp_BlogCategories] 

SET
			[Category] = @Category
			
WHERE
			[CategoryID] = @CategoryID



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogCategories_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[mp_BlogCategories_SelectOne]

/*
Author:   			
Created: 			6/7/2005
Last Modified: 			6/7/2005

*/

@CategoryID int

AS


SELECT
		[CategoryID],
		[ModuleID],
		[Category]
		
FROM
		[dbo].[mp_BlogCategories]
		
WHERE
		[CategoryID] = @CategoryID



' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteHosts_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_SiteHosts_SelectOne]

/*
Author:   			
Created: 			3/6/2005
Last Modified: 			3/6/2005

*/

@HostID int

AS


SELECT
		[HostID],
		[SiteID],
		[HostName]
		
FROM
		[dbo].[mp_SiteHosts]
		
WHERE
		[HostID] = @HostID








' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteHosts_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_SiteHosts_Select]

/*
Author:   			
Created: 			3/6/2005
Last Modified: 			3/13/2005


*/

@SiteID		int

AS


SELECT
		[HostID],
		[SiteID],
		[HostName]
		
FROM
		[dbo].[mp_SiteHosts]

WHERE	SiteID = @SiteID








' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteHosts_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_SiteHosts_Update]

/*
Author:   			
Created: 			3/6/2005
Last Modified: 			3/6/2005


*/
	
@HostID int, 
@SiteID int, 
@HostName nvarchar(255) 


AS

UPDATE 		[dbo].[mp_SiteHosts] 

SET
			[SiteID] = @SiteID,
			[HostName] = @HostName
			
WHERE
			[HostID] = @HostID








' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteHosts_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_SiteHosts_Insert]

/*
Author:   			
Created: 			3/6/2005
Last Modified: 			3/6/2005

*/

@SiteID int,
@HostName nvarchar(255)

	
AS

INSERT INTO 	[dbo].[mp_SiteHosts] 
(
				[SiteID],
				[HostName]
) 

VALUES 
(
				@SiteID,
				@HostName
				
)
SELECT @@IDENTITY 








' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SiteHosts_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'








CREATE PROCEDURE [dbo].[mp_SiteHosts_Delete]

/*
Author:   			
Created: 			3/6/2005
Last Modified: 			3/6/2005


*/

@HostID int

AS

DELETE FROM [dbo].[mp_SiteHosts]
WHERE
	[HostID] = @HostID








' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_BlogComments_Select]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'












CREATE PROCEDURE [dbo].[mp_BlogComments_Select]
(
    @ModuleID int,
    @ItemID int
)
AS

SELECT		BlogCommentID,
			ItemID, 
			ModuleID, 
			Name, 
			Title, 
			URL, 
			Comment, 
			DateCreated

FROM        mp_BlogComments

WHERE
    		ModuleID = @ModuleID
		AND ItemID = @ItemID

   ORDER BY
   	BlogCommentID DESC,  DateCreated DESC











' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_ModuleDefinitions_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_ModuleDefinitions_Insert]

/*
Author:   			
Created: 			12/26/2004
Last Modified: 		1/21/2007

*/

@SiteID int,
@FeatureName nvarchar(255),
@ControlSrc nvarchar(255),
@SortOrder int,
@IsAdmin bit,
@Icon	nvarchar(255),
@DefaultCacheTime int


	
AS
DECLARE @ModuleDefID int

INSERT INTO 	[dbo].[mp_ModuleDefinitions] 
(
				[FeatureName],
				[ControlSrc],
				[SortOrder],
				DefaultCacheTime,
				Icon,
				[IsAdmin]
) 

VALUES 
(
	
				@FeatureName,
				@ControlSrc,
				@SortOrder,
				@DefaultCacheTime,
				@Icon,
				@IsAdmin
				
)


SET @ModuleDefID =  @@IDENTITY 

Exec mp_SiteModuleDefinitions_Insert @SiteID, @ModuleDefID


SELECT @ModuleDefID
' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationPerUser_SetPageSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_SitePersonalizationPerUser_SetPageSettings]
 (
    @SiteID		int,
    @Path             	NVARCHAR(255),
    @UserId        	UNIQUEIDENTIFIER,
    @PageSettings     	IMAGE,
    @LastUpdate   	DATETIME
)
AS
BEGIN
 
    DECLARE @PathId UNIQUEIDENTIFIER
    SELECT @PathId = NULL
   
    

    SELECT @PathId = u.PathID FROM mp_SitePaths u WHERE u.SiteID = @SiteID AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
     BEGIN
        EXEC mp_SitePaths_CreatePath @SiteID, @Path, @PathId OUTPUT
     END

    --SELECT @UserId = u.UserId FROM mp_Users u WHERE u.SiteID = @SiteID AND u.LoweredUserName = LOWER(@UserName)
   -- IF (@UserId IS NULL)
   -- BEGIN
   --     EXEC dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, 0, @CurrentTimeUtc, @UserId OUTPUT
   -- END

/*
	UPDATE mp_Users WITH (ROWLOCK)
	SET LastActivityDate = @LastUpdate
	WHERE UserGuid = @UserID
	IF (@@ROWCOUNT = 0) -- Username not found
        	RETURN
*/

    IF (EXISTS(SELECT PathId FROM mp_SitePersonalizationPerUser WHERE UserId = @UserId AND PathId = @PathId))
        UPDATE mp_SitePersonalizationPerUser 
	SET PageSettings = @PageSettings, LastUpdate = @LastUpdate 
	WHERE UserId = @UserId AND PathId = @PathId
    ELSE
        INSERT INTO mp_SitePersonalizationPerUser
	(
			UserID, 
			PathID, 
			PageSettings, 
			LastUpdate
	)
	 VALUES (
			@UserId, 
			@PathId, 
			@PageSettings, 
			@LastUpdate)
    RETURN 0
END' 
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_SitePersonalizationAllUsers_SetPageSettings]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_SitePersonalizationAllUsers_SetPageSettings] 

@SiteID		int,
@Path             		nvarchar(255),
@PageSettings    	image,
@LastUpdate   		datetime

AS


BEGIN
    
    DECLARE @PathID UNIQUEIDENTIFIER
    SELECT @PathID = NULL

    

    SELECT @PathID = u.PathID FROM mp_SitePaths u WHERE u.SiteID = @SiteID AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC mp_SitePaths_CreatePath @SiteID, @Path, @PathID OUTPUT
    END

    IF (EXISTS(SELECT PathID FROM mp_SitePersonalizationAllUsers WHERE PathID = @PathID))
        	UPDATE mp_SitePersonalizationAllUsers 
	SET PageSettings = @PageSettings, LastUpdate = @LastUpdate 
	WHERE PathID = @PathID
    ELSE
        INSERT INTO mp_SitePersonalizationAllUsers(PathID, PageSettings, LastUpdate) 
	VALUES (@PathID, @PageSettings, @LastUpdate)
    RETURN 0
END' 
END
