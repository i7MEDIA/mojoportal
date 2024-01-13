using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Net;

public class ForumNotificationInfo
{
	public ForumNotificationInfo() { }

	public int ThreadId { get; set; } = -1;

	public int PostId { get; set; } = -1;

	public List<string> ModeratorEmailAddresses { get; set; } = null;

	public SmtpSettings SmtpSettings { get; set; } = null;

	public string SubjectTemplate { get; set; } = string.Empty;

	public string BodyTemplate { get; set; } = string.Empty;

	public string ForumOnlyTemplate { get; set; } = string.Empty;

	public string ThreadOnlyTemplate { get; set; } = string.Empty;

	public string ModeratorTemplate { get; set; } = string.Empty;

	public string FromEmail { get; set; } = string.Empty;

	public string FromAlias { get; set; } = string.Empty;

	public string SiteName { get; set; } = string.Empty;

	public string ModuleName { get; set; } = string.Empty;

	public string ForumName { get; set; } = string.Empty;

	public string Subject { get; set; } = string.Empty;

	public string MessageBody { get; set; } = string.Empty;

	public DataSet Subscribers { get; set; } = null;

	public string MessageLink { get; set; } = string.Empty;

	public string UnsubscribeForumThreadLink { get; set; } = string.Empty;

	public string UnsubscribeForumLink { get; set; } = string.Empty;
}