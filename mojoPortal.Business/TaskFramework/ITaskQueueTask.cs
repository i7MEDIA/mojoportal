// Author:						
// Created:					    2005-12-18
// Last Modified:				2009-10-31

using System;

namespace mojoPortal.Business
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITaskQueueTask
    {
        Guid TaskGuid { get; set;}
        Guid SiteGuid { get; set;}
        Guid QueuedBy { get; set; }
        String TaskName { get; set; }
        bool NotifyOnCompletion { get; set; }
        String NotificationToEmail { get; set; }
        String NotificationFromEmail { get; set; }
        String NotificationSubject { get; set; }
        String TaskCompleteMessage { get; set; }

        String StatusQueuedMessage { get; set; }
        String StatusStartedMessage { get; set; }
        String StatusRunningMessage { get; set; }
        String StatusCompleteMessage { get; set; }

        int UpdateFrequency { get; }
        bool CanStop {get;}
        bool CanResume { get; }

        void QueueTask();
        void StartTask();
        void StopTask();
        void ResumeTask();
        


    }
}
