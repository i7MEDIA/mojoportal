using System;
using System.Collections;
using System.Collections.Generic;

namespace mojoPortal.Web.ContactUI;

public class ContactFormConfiguration
{
	public ContactFormConfiguration()
	{ }

	public ContactFormConfiguration(Hashtable settings)
	{
		LoadSettings(settings);
	}

	private void LoadSettings(Hashtable settings)
	{
		if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

		InstanceCssClass = settings.ParseString("CustomCssClassSetting", InstanceCssClass);
		EditorHeight = settings.ParseString("ContactFormEditorHeightSetting", EditorHeight);
		SubjectPrefix = settings.ParseString("ContactFormSubjectPrefixSetting", SubjectPrefix).Trim();

		emailReceiveAddresses = settings.ParseString("ContactFormEmailSetting", emailReceiveAddresses).Trim();
		EmailAddresses = emailReceiveAddresses.SplitOnChar('|');
		emailReceiveAliases = settings["ContactFormEmailAliasSetting"].ToString().Trim();
		EmailAliases = emailReceiveAliases.SplitOnChar('|');
		EmailBccAddresses = settings.ParseString("ContactFormEmailBccSetting", EmailBccAddresses);
		EmailAliases ??= [];

		UseSpamBlocking = settings.ParseBool("ContactFormUseCommentSpamBlocker", UseSpamBlocking);
		BlockBadWords = settings.ParseBool("BlockBadWords", BlockBadWords);

		AppendIPToMessageSetting = settings.ParseBool("AppendIPToMessageSetting", AppendIPToMessageSetting);
		KeepMessages = settings.ParseBool("KeepMessagesInDatabase", KeepMessages);
		OverrideEditorProvider = settings.ParseString("ContentEditorSetting", OverrideEditorProvider);
	}

	public bool UseSpamBlocking { get; private set; } = false;

	public bool BlockBadWords { get; private set; } = true;

	public bool AppendIPToMessageSetting { get; private set; } = true;

	private string emailReceiveAddresses = string.Empty;

	private string emailReceiveAliases = string.Empty;

	public List<string> EmailAddresses { get; private set; } = null;

	public List<string> EmailAliases { get; private set; } = null;

	public string EmailBccAddresses { get; private set; } = string.Empty;

	public bool UseInputAsFromAddress { get; } = false;

	public bool KeepMessages { get; private set; } = true;

	public string SubjectPrefix { get; private set; } = string.Empty;

	public string EditorHeight { get; private set; } = "350";

	public string InstanceCssClass { get; private set; } = string.Empty;

	public static string OverrideEditorProvider { get; set; } = string.Empty;
}