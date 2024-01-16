using System;
using System.Globalization;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ELetterUI;

public partial class ConfirmPage : NonCmsBasePage
{
	private SubscriberRepository subscriptions = new();
	private Guid subscriptionGuid = Guid.Empty;
	private LetterInfo letterInfo = null;
	private LetterSubscriber subscription = null;

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateLabels();
		PopulateControls();
	}

	private void PopulateControls()
	{
		if (subscription == null)
		{
			pnlNotFound.Visible = true;
			pnlConfirmed.Visible = false;
		}
		else
		{
			letterInfo = new LetterInfo(subscription.LetterInfoGuid);
			litConfirmDetails.Text = string.Format(CultureInfo.InvariantCulture, Resource.NewsletterConfirmedFormat, letterInfo.Title);
			pnlNotFound.Visible = false;
			pnlConfirmed.Visible = true;
		}
	}

	private void PopulateLabels()
	{
		lnkThisPage.Text = Resource.NewslettersLink;
		lnkThisPage.NavigateUrl = $"{SiteRoot}/eletter/Default.aspx";
	}

	private void LoadSettings()
	{
		subscriptionGuid = WebUtils.ParseGuidFromQueryString("s", subscriptionGuid);
		if (subscriptionGuid != Guid.Empty)
		{
			subscription = subscriptions.Fetch(subscriptionGuid);
			if ((subscription != null) && (subscription.SiteGuid == siteSettings.SiteGuid))
			{
				subscriptions.Verify(subscription.SubscribeGuid, true, Guid.Empty);
				if (subscription.UserGuid == Guid.Empty)
				{
					SiteUser user = SiteUser.GetByEmail(siteSettings, subscription.EmailAddress);
					if (user != null)
					{
						subscription.UserGuid = user.UserGuid;
						subscriptions.Save(subscription);
					}
				}

				LetterInfo.UpdateSubscriberCount(subscription.LetterInfoGuid);
			}
			else
			{
				subscription = null;
			}
		}

		AddClassToBody("administration");
		AddClassToBody("eletterconfirm");
	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		SuppressMenuSelection();
	}

	#endregion
}