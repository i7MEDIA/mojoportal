using System;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Plugins.HtmlContent.Components;

public class HtmlContentDeleteHandler : ContentDeleteHandlerProvider
{
	public HtmlContentDeleteHandler()
	{ }

	public override void DeleteContent(int moduleId, Guid moduleGuid)
	{
		ContentHistory.DeleteByContent(moduleGuid);
		ContentWorkflow.DeleteByModule(moduleGuid);
		var repository = new HtmlRepository();
		repository.DeleteByModule(moduleId);
	}
}
