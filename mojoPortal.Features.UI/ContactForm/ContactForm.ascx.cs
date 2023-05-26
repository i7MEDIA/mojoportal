using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Net;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;
using log4net;

namespace mojoPortal.Web.ContactUI
{

	public partial class ContactForm : SiteModuleControl
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ContactForm));
		private ContactFormConfiguration config = new();

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += Page_Load;
			btnSend.Click += btnSend_Click;

			SiteUtils.SetupEditor(edMessage, WebConfigSettings.UseSkinCssInEditor, ContactFormConfiguration.OverrideEditorProvider, true, false, Page);

			LoadSettings();
			PopulateLabels();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			BindToList();
			PopulateControls();
		}

		private void PopulateControls()
		{
			if (!Page.IsPostBack)
			{
				if (Request.IsAuthenticated)
				{
					SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

					if (siteUser != null)
					{
						txtEmail.Text = siteUser.Email;
						txtName.Text = siteUser.Name;
					}
				}
			}
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			bool isValid = true;
			reqEmail.Validate();

			if (!reqEmail.IsValid)
			{
				isValid = false;
			}

			regexEmail.Validate();

			if (!regexEmail.IsValid)
			{
				isValid = false;
			}

			if (isValid && !string.IsNullOrWhiteSpace(edMessage.Text))
			{
				if (siteSettings.BadWordCheckingEnabled && (config.BlockBadWords || siteSettings.BadWordCheckingEnforced))
				{
					if (edMessage.Text.ContainsBadWords() 
						|| txtName.Text.ContainsBadWords()
						|| txtEmail.Text.ContainsBadWords()
						|| txtSubject.Text.ContainsBadWords())
					{
						lblMessage.Text = ContactFormResources.BadWordsFound;
						lblMessage.Visible = true;
						return;
					}
				}

				if (config.UseSpamBlocking && divCaptcha.Visible)
				{
					if (!captcha.IsValid)
					{
						return;
					}
				}

				string subjectPrefix = config.SubjectPrefix;

				if (subjectPrefix.Length == 0)
				{
					subjectPrefix = Title;
				}

				StringBuilder message = new StringBuilder();
				message.Append(txtName.Text + "<br />");
				message.Append(txtEmail.Text + "<br /><br />");
				message.Append(SiteUtils.ChangeRelativeUrlsToFullyQualifiedUrls(SiteUtils.GetNavigationSiteRoot(), WebUtils.GetSiteRoot(), edMessage.Text));
				message.Append("<br /><br />");

				if (config.AppendIPToMessageSetting)
				{
					message.Append("HTTP_USER_AGENT: " + Page.Request.ServerVariables["HTTP_USER_AGENT"] + "<br />");
					message.Append("REMOTE_HOST: " + Page.Request.ServerVariables[WebConfigSettings.RemoteHostServerVariable] + "<br />");
					message.Append("REMOTE_ADDR: " + SiteUtils.GetIP4Address() + "<br />");
					message.Append("LOCAL_ADDR: " + Page.Request.ServerVariables["LOCAL_ADDR"] + "<br />");
				}

				Module m = new Module(ModuleId);
				if (config.KeepMessages && (m.ModuleGuid != Guid.Empty))
				{
					ContactFormMessage contact = new ContactFormMessage();
					contact.ModuleGuid = m.ModuleGuid;
					contact.SiteGuid = siteSettings.SiteGuid;
					contact.Message = message.ToString();
					contact.Subject = config.SubjectPrefix + ": " + txtSubject.Text;
					contact.UserName = txtName.Text;
					contact.Email = txtEmail.Text;
					contact.CreatedFromIpAddress = SiteUtils.GetIP4Address();

					if (Request.IsAuthenticated)
					{
						SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
						if (currentUser != null)
							contact.UserGuid = currentUser.UserGuid;

					}

					contact.Save();
				}

				string fromAddress = siteSettings.DefaultEmailFromAddress;

				//if (config.UseInputAsFromAddress)
				//{
				//	fromAddress = txtEmail.Text;
				//}

				if ((config.EmailAddresses != null) && (config.EmailAddresses.Count > 0))
				{
					SmtpSettings smtpSettings = SiteUtils.GetSmtpSettings();

					smtpSettings.SenderHeader = "ContactForm";

					if ((pnlToAddresses.Visible) && (ddToAddresses.SelectedIndex > -1))
					{
						string to = config.EmailAddresses[ddToAddresses.SelectedIndex];

						try
						{
							Email.SendEmail(
								smtpSettings,
								fromAddress,
								txtEmail.Text,
								to,
								string.Empty,
								config.EmailBccAddresses,
								subjectPrefix + ": " + this.txtSubject.Text,
								message.ToString(),
								true,
								"Normal");
						}
						catch (Exception ex)
						{
							log.Error("error sending email from address was " + fromAddress + " to address was " + to, ex);
						}
					}
					else
					{
						foreach (string to in config.EmailAddresses)
						{
							try
							{
								Email.SendEmail(
									smtpSettings,
									fromAddress,
									txtEmail.Text,
									to,
									string.Empty,
									config.EmailBccAddresses,
									subjectPrefix + ": " + this.txtSubject.Text,
									message.ToString(),
									true,
									"Normal");
							}
							catch (Exception ex)
							{
								log.Error("error sending email from address was " + fromAddress + " to address was " + to, ex);
							}
						}
					}
				}
				else
				{
					log.Info("contact form submission received but not sending email because notification email address is not configured.");
				}

				lblMessage.Text = ContactFormResources.ContactFormThankYouLabel;
				pnlSend.Visible = false;
			}
			else
			{
				if (string.IsNullOrWhiteSpace(edMessage.Text))
				{
					lblMessage.Text = ContactFormResources.ContactFormEmptyMessageWarning;
				}
				else
				{
					lblMessage.Text = "invalid";
				}
			}

			btnSend.Text = ContactFormResources.ContactFormSendButtonLabel;
			btnSend.Enabled = true;
		}


		private void PopulateLabels()
		{
			btnSend.Text = ContactFormResources.ContactFormSendButtonLabel;
			SiteUtils.SetButtonAccessKey(btnSend, ContactFormResources.ContactFormSendButtonAccessKey);


			reqEmail.ErrorMessage = ContactFormResources.ContactFormValidAddressLabel;
			regexEmail.ErrorMessage = ContactFormResources.ContactFormValidAddressLabel;

			Title1.Visible = !RenderInWebPartMode;

			if (ModuleConfiguration != null)
			{
				Title = ModuleConfiguration.ModuleTitle;
				Description = ModuleConfiguration.FeatureName;
			}

			Title1.EditUrl = string.Empty;
			Title1.EditText = string.Empty;
			Title1.ToolTip = string.Empty;


			if (IsEditable)
			{
				if (Page is mojoBasePage basePage)
				{
					if (config.KeepMessages)
					{
						Title1.LiteralExtraMarkup =
							"&nbsp;<a href='" +
							SiteRoot +
							"/ContactForm/MessageListDialog.aspx?pageid=" +
							PageId.ToInvariantString() +
							"&amp;mid=" +
							ModuleId.ToInvariantString() +
							"' class='ModuleEditLink cblink' title='" +
							ContactFormResources.ContactFormViewMessagesLink +
							"' " +
							">" +
							ContactFormResources.ContactFormViewMessagesLink + "</a>"
						;
					}
				}
			}

			lblMessageLabel.Attributes.Add("onclick", $"CKEDITOR != undefined ? CKEDITOR.instances['{edMessage.ClientID}innerEditor'].focus() : null;");
		}

		private void BindToList()
		{
			if (Page.IsPostBack)
			{
				return;
			}

			if (!pnlToAddresses.Visible)
			{
				return;
			}

			if (config.EmailAddresses == null)
			{
				return;
			}

			if (config.EmailAddresses.Count <= 1)
			{
				return;
			}

			List<string> bindList = new List<string>();
			int index = 0;

			foreach (string a in config.EmailAddresses)
			{
				if ((index + 1) <= config.EmailAliases.Count)
				{
					bindList.Add(config.EmailAliases[index]);
				}
				else
				{
					bindList.Add(a);
				}

				index += 1;
			}

			index = 0;

			foreach (string a in bindList)
			{
				ListItem item = new ListItem(a, index.ToInvariantString());
				ddToAddresses.Items.Add(item);
				index += 1;
			}
		}


		private void LoadSettings()
		{
			config = new ContactFormConfiguration(Settings);

			if (config.InstanceCssClass.Length > 0)
			{
				pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass);
			}

			if ((config.EmailAddresses != null) && (config.EmailAddresses.Count > 1))
			{
				pnlToAddresses.Visible = true;
			}

			edMessage.WebEditor.ToolBar = ToolBar.AnonymousUser;
			edMessage.WebEditor.Height = Unit.Parse(config.EditorHeight);

			vSummary.ValidationGroup = "Contact" + ModuleId.ToInvariantString();
			reqEmail.ValidationGroup = "Contact" + ModuleId.ToInvariantString();
			regexEmail.ValidationGroup = "Contact" + ModuleId.ToInvariantString();
			btnSend.ValidationGroup = "Contact" + ModuleId.ToInvariantString();

			if (!config.UseSpamBlocking || Request.IsAuthenticated)
			{
				captcha.Enabled = false;
				captcha.Visible = false;
				divCaptcha.Visible = false;
			}
			else
			{
				captcha.ProviderName = siteSettings.CaptchaProvider;
				captcha.Captcha.ControlID = "captcha" + ModuleId;
				captcha.RecaptchaPrivateKey = siteSettings.RecaptchaPrivateKey;
				captcha.RecaptchaPublicKey = siteSettings.RecaptchaPublicKey;
			}

			mojoBasePage basePage = Page as mojoBasePage;

			if (basePage != null)
			{
				basePage.ScriptConfig.IncludeColorBox = true;
			}
		}
	}
}
