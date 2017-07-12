

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessagePriority_SelectAll]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessagePriority_SelectAll]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

AS


SELECT
		[PriorityID],
		[Priority]
		
FROM
		[dbo].[mp_PrivateMessagePriority]
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessagePriority_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessagePriority_SelectOne]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

@PriorityID uniqueidentifier

AS


SELECT
		[PriorityID],
		[Priority]
		
FROM
		[dbo].[mp_PrivateMessagePriority]
		
WHERE
		[PriorityID] = @PriorityID' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessagePriority_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessagePriority_Insert]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

@PriorityID		uniqueidentifier,
@Priority nvarchar(50)

	
AS


INSERT INTO 	[dbo].[mp_PrivateMessagePriority] 
(
				[PriorityID],
				[Priority]
) 

VALUES 
(
				@PriorityID,
				@Priority
				
)
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessagePriority_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessagePriority_Update]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/
	
@PriorityID uniqueidentifier, 
@Priority nvarchar(50) 


AS

UPDATE 		[dbo].[mp_PrivateMessagePriority] 

SET
			[Priority] = @Priority
			
WHERE
			[PriorityID] = @PriorityID' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessagePriority_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessagePriority_Delete]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

@PriorityID uniqueidentifier

AS

DELETE FROM [dbo].[mp_PrivateMessagePriority]
WHERE
	[PriorityID] = @PriorityID' 
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessages_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessages_Insert]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

@MessageID uniqueidentifier,
@FromUser uniqueidentifier,
@PriorityID uniqueidentifier,
@Subject nvarchar(255),
@Body ntext,
@ToCSVList ntext,
@CcCSVList ntext,
@BccCSVList ntext,
@ToCSVLabels ntext,
@CcCSVLabels ntext,
@BccCSVLabels ntext,
@CreatedDate datetime,
@SentDate datetime

	
AS



INSERT INTO 	[dbo].[mp_PrivateMessages] 
(
				[MessageID],
				[FromUser],
				[PriorityID],
				[Subject],
				[Body],
				[ToCSVList],
				[CcCSVList],
				[BccCSVList],
				[ToCSVLabels],
				[CcCSVLabels],
				[BccCSVLabels],
				[CreatedDate],
				[SentDate]
) 

VALUES 
(
				@MessageID,
				@FromUser,
				@PriorityID,
				@Subject,
				@Body,
				@ToCSVList,
				@CcCSVList,
				@BccCSVList,
				@ToCSVLabels,
				@CcCSVLabels,
				@BccCSVLabels,
				@CreatedDate,
				@SentDate
				
)
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessages_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessages_Update]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/
	
@MessageID uniqueidentifier, 
@FromUser uniqueidentifier, 
@PriorityID uniqueidentifier, 
@Subject nvarchar(255), 
@Body ntext, 
@ToCSVList ntext, 
@CcCSVList ntext, 
@BccCSVList ntext, 
@ToCSVLabels ntext, 
@CcCSVLabels ntext, 
@BccCSVLabels ntext, 
@CreatedDate datetime, 
@SentDate datetime 


AS

UPDATE 		[dbo].[mp_PrivateMessages] 

SET
			[FromUser] = @FromUser,
			[PriorityID] = @PriorityID,
			[Subject] = @Subject,
			[Body] = @Body,
			[ToCSVList] = @ToCSVList,
			[CcCSVList] = @CcCSVList,
			[BccCSVList] = @BccCSVList,
			[ToCSVLabels] = @ToCSVLabels,
			[CcCSVLabels] = @CcCSVLabels,
			[BccCSVLabels] = @BccCSVLabels,
			[CreatedDate] = @CreatedDate,
			[SentDate] = @SentDate
			
WHERE
			[MessageID] = @MessageID' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessages_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessages_SelectOne]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

@MessageID uniqueidentifier

AS


SELECT
		[MessageID],
		[FromUser],
		[PriorityID],
		[Subject],
		[Body],
		[ToCSVList],
		[CcCSVList],
		[BccCSVList],
		[ToCSVLabels],
		[CcCSVLabels],
		[BccCSVLabels],
		[CreatedDate],
		[SentDate]
		
FROM
		[dbo].[mp_PrivateMessages]
		
WHERE
		[MessageID] = @MessageID
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessages_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessages_Delete]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

@MessageID uniqueidentifier

AS

DELETE FROM [dbo].[mp_PrivateMessages]
WHERE
	[MessageID] = @MessageID' 
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessageAttachments_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessageAttachments_Delete]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

@AttachmentID uniqueidentifier

AS

DELETE FROM [dbo].[mp_PrivateMessageAttachments]
WHERE
	[AttachmentID] = @AttachmentID' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessageAttachments_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessageAttachments_Insert]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

@AttachmentID uniqueidentifier,
@MessageID uniqueidentifier,
@OriginalFileName nvarchar(255),
@ServerFileName nvarchar(50),
@CreatedDate datetime

	
AS



INSERT INTO 	[dbo].[mp_PrivateMessageAttachments] 
(
				[AttachmentID],
				[MessageID],
				[OriginalFileName],
				[ServerFileName],
				[CreatedDate]
) 

VALUES 
(
				@AttachmentID,
				@MessageID,
				@OriginalFileName,
				@ServerFileName,
				@CreatedDate
				
)
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessageAttachments_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessageAttachments_SelectOne]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

@AttachmentID uniqueidentifier

AS


SELECT
		[AttachmentID],
		[MessageID],
		[OriginalFileName],
		[ServerFileName],
		[CreatedDate]
		
FROM
		[dbo].[mp_PrivateMessageAttachments]
		
WHERE
		[AttachmentID] = @AttachmentID' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_PrivateMessageAttachments_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[mp_PrivateMessageAttachments_Update]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/
	
@AttachmentID uniqueidentifier, 
@MessageID uniqueidentifier, 
@OriginalFileName nvarchar(255), 
@ServerFileName nvarchar(50), 
@CreatedDate datetime 


AS

UPDATE 		[dbo].[mp_PrivateMessageAttachments] 

SET
			[MessageID] = @MessageID,
			[OriginalFileName] = @OriginalFileName,
			[ServerFileName] = @ServerFileName,
			[CreatedDate] = @CreatedDate
			
WHERE
			[AttachmentID] = @AttachmentID' 
END
GO




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserEmailAccounts_Update]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_UserEmailAccounts_Update]

/*
Author:   			
Created: 			8/20/2006
Last Modified: 		8/20/2006
*/
	
@ID uniqueidentifier, 
@AccountName nvarchar(50), 
@UserName nvarchar(75), 
@Email nvarchar(100), 
@Password nvarchar(255), 
@Pop3Server nvarchar(75), 
@Pop3Port int, 
@SmtpServer nvarchar(75), 
@SmtpPort int 


AS
UPDATE 		[dbo].[mp_UserEmailAccounts] 

SET
			[AccountName] = @AccountName,
			[UserName] = @UserName,
			[Email] = @Email,
			[Password] = @Password,
			[Pop3Server] = @Pop3Server,
			[Pop3Port] = @Pop3Port,
			[SmtpServer] = @SmtpServer,
			[SmtpPort] = @SmtpPort
			
WHERE
			[ID] = @ID
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserEmailAccounts_SelectByUser]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_UserEmailAccounts_SelectByUser]

/*
Author:   			
Created: 			8/20/2006
Last Modified: 		8/20/2006
*/

@UserGuid uniqueidentifier

AS
SELECT
		[ID],
		[UserGuid],
		[AccountName],
		[UserName],
		[Email],
		[Password],
		[Pop3Server],
		[Pop3Port],
		[SmtpServer],
		[SmtpPort]
		
FROM
		[dbo].[mp_UserEmailAccounts]

WHERE UserGuid = @UserGuid

ORDER BY AccountName
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserEmailAccounts_Insert]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_UserEmailAccounts_Insert]

/*
Author:   			
Created: 			8/20/2006
Last Modified: 		8/20/2006
*/

@ID uniqueidentifier,
@UserGuid uniqueidentifier,
@AccountName nvarchar(50),
@UserName nvarchar(75),
@Email nvarchar(100),
@Password nvarchar(255),
@Pop3Server nvarchar(75),
@Pop3Port int,
@SmtpServer nvarchar(75),
@SmtpPort int

	
AS
INSERT INTO 	[dbo].[mp_UserEmailAccounts] 
(
				[ID],
				[UserGuid],
				[AccountName],
				[UserName],
				[Email],
				[Password],
				[Pop3Server],
				[Pop3Port],
				[SmtpServer],
				[SmtpPort]
) 

VALUES 
(
				@ID,
				@UserGuid,
				@AccountName,
				@UserName,
				@Email,
				@Password,
				@Pop3Server,
				@Pop3Port,
				@SmtpServer,
				@SmtpPort
				
)
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserEmailAccounts_SelectOne]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_UserEmailAccounts_SelectOne]

/*
Author:   			
Created: 			8/20/2006
Last Modified: 		8/20/2006
*/

@ID uniqueidentifier

AS
SELECT
		[ID],
		[UserGuid],
		[AccountName],
		[UserName],
		[Email],
		[Password],
		[Pop3Server],
		[Pop3Port],
		[SmtpServer],
		[SmtpPort]
		
FROM
		[dbo].[mp_UserEmailAccounts]
		
WHERE
		[ID] = @ID
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[mp_UserEmailAccounts_Delete]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[mp_UserEmailAccounts_Delete]

/*
Author:   			
Created: 			8/20/2006
Last Modified: 		8/20/2006
*/

@ID uniqueidentifier

AS
DELETE FROM [dbo].[mp_UserEmailAccounts]
WHERE
	[ID] = @ID
' 
END
GO


