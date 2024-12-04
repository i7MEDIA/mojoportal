using System;

namespace mojoPortal.Web.Models;

public class ExternalLoginErrorModel
{
	public Guid IncidentNumber { get; set; }
	public string Title { get; set; }
	public string Message { get; set; }
}
