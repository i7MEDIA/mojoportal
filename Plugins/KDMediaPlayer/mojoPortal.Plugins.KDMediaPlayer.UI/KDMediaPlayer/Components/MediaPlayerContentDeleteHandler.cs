using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Features.Business;
using System;

namespace mojoPortal.MediaPlayerUI
{
	/// <summary>
	/// Performs the proper actions when the associated content has been deleted from the site.
	/// </summary>
	public class MediaPlayerContentDeleteHandler : ContentDeleteHandlerProvider
	{
		public static ILog log = LogManager.GetLogger(typeof(MediaPlayerContentDeleteHandler));

		public MediaPlayerContentDeleteHandler()
		{ }

		public override void DeleteContent(int moduleId, Guid moduleGuid)
		{
			MediaPlayer.RemoveByModule(moduleId);
			log.Info("MediaPlayer data for ModuleID: " + moduleId.ToString() + " has been deleted.");
		}
	}
}