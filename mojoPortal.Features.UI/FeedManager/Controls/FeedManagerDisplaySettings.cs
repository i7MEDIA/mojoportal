using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.FeedUI
{
	/// <summary>
	/// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
	/// </summary>
	public class FeedManagerDisplaySettings : WebControl
	{
		public string DateFormat { get; set; } = string.Empty;
		public bool DisableUseFeedListAsFilter { get; set; } = false;
		public bool DisableShowAggregateFeedLink { get; set; } = false;
		public bool DisableRepeatColumns { get; set; } = false;
		public bool DisableShowIndividualFeedLinks { get; set; } = false;
		public bool DisableUseCalendar { get; set; } = false;
		public bool DisableScroller { get; set; } = false;
		public bool ForceExcerptMode { get; set; } = false;
		public bool ForceShowHeadingsOnly { get; set; } = false;
		public bool UseNoFollowOnHeadingLinks { get; set; } = false;
		public bool UseNoFollowOnFeedSiteLinks { get; set; } = false;
		/// <summary>
		/// disables adding the feed link in the head of the page
		/// </summary>
		public bool DisableUseAutoDiscoveryAggregateFeedLink { get; set; } = false;
		public int OverridePageSize { get; set; } = 0;
		public bool UseBottomNavForRight { get; set; } = false;
		public bool UseUlForSingleColumn { get; set; } = true;


		protected override void Render(HtmlTextWriter writer)
		{
			if (HttpContext.Current == null)
			{
				writer.Write("[" + ID + "]");
				return;
			}
		}
	}
}
