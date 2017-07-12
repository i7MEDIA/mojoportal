USE [mojodev]
GO
/****** Object:  StoredProcedure [dbo].[mp_UserEmailAccounts_Update]    Script Date: 06/17/2007 07:58:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserEmailAccounts_Update]

/*
Author:   			
Created: 			2006-08-20
Last Modified: 		2007-06-17
*/
	
@ID uniqueidentifier, 
@AccountName nvarchar(50), 
@UserName nvarchar(75), 
@Email nvarchar(100), 
@Password nvarchar(255), 
@Pop3Server nvarchar(75), 
@Pop3Port int, 
@SmtpServer nvarchar(75), 
@SmtpPort int,
@UseSSL bit


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
			[SmtpPort] = @SmtpPort,
			[UseSSL] = @UseSSL
			
WHERE
			[ID] = @ID
GO
/****** Object:  StoredProcedure [dbo].[mp_UserEmailAccounts_SelectByUser]    Script Date: 06/17/2007 07:58:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserEmailAccounts_SelectByUser]

/*
Author:   			
Created: 			2006-08-20
Last Modified: 		2007-06-17
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
		[SmtpPort],
		[UseSSL]
		
FROM
		[dbo].[mp_UserEmailAccounts]

WHERE UserGuid = @UserGuid

ORDER BY AccountName
GO
/****** Object:  StoredProcedure [dbo].[mp_UserEmailAccounts_Insert]    Script Date: 06/17/2007 07:58:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserEmailAccounts_Insert]

/*
Author:   			
Created: 			2006-9-08-20
Last Modified: 		2007-06-17
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
@SmtpPort int,
@UseSSL	bit

	
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
				[SmtpPort],
				[UseSSL]
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
				@SmtpPort,
				@UseSSL
				
)
GO
/****** Object:  StoredProcedure [dbo].[mp_UserEmailAccounts_SelectOne]    Script Date: 06/17/2007 07:58:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_UserEmailAccounts_SelectOne]

/*
Author:   			
Created: 			2006-08-20
Last Modified: 		2007-06-17
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
		[SmtpPort],
		[UseSSL]
		
FROM
		[dbo].[mp_UserEmailAccounts]
		
WHERE
		[ID] = @ID
GO
/****** Object:  StoredProcedure [dbo].[mp_UserEmailAccounts_Delete]    Script Date: 06/17/2007 07:58:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessageAttachments_Delete]    Script Date: 06/17/2007 07:58:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessageAttachments_Delete]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

@AttachmentID uniqueidentifier

AS

DELETE FROM [dbo].[mp_PrivateMessageAttachments]
WHERE
	[AttachmentID] = @AttachmentID
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessageAttachments_Insert]    Script Date: 06/17/2007 07:58:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessageAttachments_Insert]

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
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessageAttachments_SelectOne]    Script Date: 06/17/2007 07:58:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessageAttachments_SelectOne]

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
		[AttachmentID] = @AttachmentID
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessageAttachments_Update]    Script Date: 06/17/2007 07:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessageAttachments_Update]

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
			[AttachmentID] = @AttachmentID
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessages_Insert]    Script Date: 06/17/2007 07:58:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessages_Insert]

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
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessages_Update]    Script Date: 06/17/2007 07:58:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessages_Update]

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
			[MessageID] = @MessageID
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessages_SelectOne]    Script Date: 06/17/2007 07:58:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessages_SelectOne]

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
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessages_Delete]    Script Date: 06/17/2007 07:58:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessages_Delete]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

@MessageID uniqueidentifier

AS

DELETE FROM [dbo].[mp_PrivateMessages]
WHERE
	[MessageID] = @MessageID
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessagePriority_SelectAll]    Script Date: 06/17/2007 07:58:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessagePriority_SelectAll]

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
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessagePriority_SelectOne]    Script Date: 06/17/2007 07:58:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessagePriority_SelectOne]

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
		[PriorityID] = @PriorityID
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessagePriority_Insert]    Script Date: 06/17/2007 07:58:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessagePriority_Insert]

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
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessagePriority_Update]    Script Date: 06/17/2007 07:58:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessagePriority_Update]

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
			[PriorityID] = @PriorityID
GO
/****** Object:  StoredProcedure [dbo].[mp_PrivateMessagePriority_Delete]    Script Date: 06/17/2007 07:58:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[mp_PrivateMessagePriority_Delete]

/*
Author:   			
Created: 			8/2/2006
Last Modified: 		8/2/2006
*/

@PriorityID uniqueidentifier

AS

DELETE FROM [dbo].[mp_PrivateMessagePriority]
WHERE
	[PriorityID] = @PriorityID
GO
