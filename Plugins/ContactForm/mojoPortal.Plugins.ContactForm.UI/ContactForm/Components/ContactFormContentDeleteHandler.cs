using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using System;

namespace mojoPortal.Features;

public class ContactFormContentDeleteHandler : ContentDeleteHandlerProvider
{
	public ContactFormContentDeleteHandler()
	{ }

	public override void DeleteContent(int moduleId, Guid moduleGuid)
	{
		ContactFormMessage.DeleteByModule(moduleGuid);
	}
}
