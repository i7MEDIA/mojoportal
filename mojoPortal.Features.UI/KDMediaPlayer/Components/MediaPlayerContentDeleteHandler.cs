using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Features.Business;
using log4net;

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