using System;

namespace mojoPortal.Business;

public interface ITaskQueueTask
{
	Guid TaskGuid { get; set; }
	Guid SiteGuid { get; set; }
	Guid QueuedBy { get; set; }
	string TaskName { get; set; }
	bool NotifyOnCompletion { get; set; }
	string NotificationToEmail { get; set; }
	string NotificationFromEmail { get; set; }
	string NotificationSubject { get; set; }
	string TaskCompleteMessage { get; set; }
	string StatusQueuedMessage { get; set; }
	string StatusStartedMessage { get; set; }
	string StatusRunningMessage { get; set; }
	string StatusCompleteMessage { get; set; }
	int UpdateFrequency { get; }
	bool CanStop { get; }
	bool CanResume { get; }
	void QueueTask();
	void StartTask();
	void StopTask();
	void ResumeTask();
}