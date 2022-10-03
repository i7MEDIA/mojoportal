using mojoPortal.Web.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
namespace mojoPortal.Web.BlogUI
{
	/// <summary>
	/// encapsulates the feature instance configuration loaded from module settings into a more friendly object
	/// </summary>
	public class BlogConfiguration
	{
		public BlogConfiguration()
		{ }

		public BlogConfiguration(Hashtable settings)
		{
			LoadSettings(settings);

		}

		private void LoadSettings(Hashtable settings)
		{
			if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

			useExcerpt = WebUtils.ParseBoolFromHashtable(settings, "BlogUseExcerptSetting", useExcerpt);
			useExcerptInFeed = WebUtils.ParseBoolFromHashtable(settings, "UseExcerptInFeedSetting", useExcerptInFeed);

		   

			titleOnly = WebUtils.ParseBoolFromHashtable(settings, "BlogShowTitleOnlySetting", titleOnly);

			showPager = WebUtils.ParseBoolFromHashtable(settings, "BlogShowPagerInListSetting", showPager);

			googleMapIncludeWithExcerpt = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapIncludeWithExcerptSetting", googleMapIncludeWithExcerpt);

			enableContentRating = WebUtils.ParseBoolFromHashtable(settings, "EnableContentRatingSetting", enableContentRating);

			enableRatingComments = WebUtils.ParseBoolFromHashtable(settings, "EnableRatingCommentsSetting", enableRatingComments);

			excerptLength = WebUtils.ParseInt32FromHashtable(settings, "BlogExcerptLengthSetting", excerptLength);

			if (settings.Contains("BlogExcerptSuffixSetting"))
			{
				excerptSuffix = settings["BlogExcerptSuffixSetting"].ToString();
			}

			if (settings.Contains("BlogMoreLinkText"))
			{
				moreLinkText = settings["BlogMoreLinkText"].ToString();
			}

			if (settings.Contains("BlogAuthorSetting"))
			{
				blogAuthor = settings["BlogAuthorSetting"].ToString();
			}

			if (settings.Contains("CustomCssClassSetting"))
			{
				instanceCssClass = settings["CustomCssClassSetting"].ToString();
			}

			if (settings.Contains("BlogDateTimeFormat"))
			{
				string format = settings["BlogDateTimeFormat"].ToString().Trim();
				if (format.Length > 0)
				{
					try
					{
						string d = DateTime.Now.ToString(format, CultureInfo.CurrentCulture);
						dateTimeFormat = format;
					}
					catch (FormatException) { }
				}

			}

			useTagCloudForCategories = WebUtils.ParseBoolFromHashtable(settings, "BlogUseTagCloudForCategoriesSetting", useTagCloudForCategories);

			showCalendar = WebUtils.ParseBoolFromHashtable(settings, "BlogShowCalendarSetting", showCalendar);

			showCategories = WebUtils.ParseBoolFromHashtable(settings, "BlogShowCategoriesSetting", showCategories);

			showArchives = WebUtils.ParseBoolFromHashtable(settings, "BlogShowArchiveSetting", showArchives);

			showStatistics = WebUtils.ParseBoolFromHashtable(settings, "BlogShowStatisticsSetting", showStatistics);

			showFeedLinks = WebUtils.ParseBoolFromHashtable(settings, "BlogShowFeedLinksSetting", showFeedLinks);

			showAddFeedLinks = WebUtils.ParseBoolFromHashtable(settings, "BlogShowAddFeedLinksSetting", showAddFeedLinks);

			navigationOnRight = WebUtils.ParseBoolFromHashtable(settings, "BlogNavigationOnRightSetting", navigationOnRight);

			allowComments = WebUtils.ParseBoolFromHashtable(settings, "BlogAllowComments", allowComments);

			includeCommentBodyInNotification = WebUtils.ParseBoolFromHashtable(settings, "BlogIncludeCommentBodyInNotification", includeCommentBodyInNotification);

			useLinkForHeading = WebUtils.ParseBoolFromHashtable(settings, "BlogUseLinkForHeading", useLinkForHeading);

			usePostTitleAsPageHeading = WebUtils.ParseBoolFromHashtable(settings, "UsePostTitleAsPageHeadingSetting", usePostTitleAsPageHeading);

			//showPostAuthor = WebUtils.ParseBoolFromHashtable(settings, "ShowPostAuthorSetting", showPostAuthor);

			//if (settings.Contains("GoogleMapInitialMapTypeSetting"))
			//{
			//    string gmType = settings["GoogleMapInitialMapTypeSetting"].ToString();
			//    try
			//    {
			//        mapType = (MapType)Enum.Parse(typeof(MapType), gmType);
			//    }
			//    catch (ArgumentException) { }
			//}

			//googleMapHeight = WebUtils.ParseInt32FromHashtable(settings, "GoogleMapHeightSetting", googleMapHeight);

			////googleMapWidth = WebUtils.ParseInt32FromHashtable(settings, "GoogleMapWidthSetting", googleMapWidth);
			//if (settings.Contains("GoogleMapWidthSetting"))
			//{
			//    googleMapWidth = settings["GoogleMapWidthSetting"].ToString();
			//}


			//googleMapEnableMapType = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableMapTypeSetting", googleMapEnableMapType);

			//googleMapEnableZoom = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableZoomSetting", googleMapEnableZoom);

			//googleMapShowInfoWindow = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapShowInfoWindowSetting", googleMapShowInfoWindow);

			//googleMapEnableLocalSearch = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableLocalSearchSetting", googleMapEnableLocalSearch);

			//googleMapEnableDirections = WebUtils.ParseBoolFromHashtable(settings, "GoogleMapEnableDirectionsSetting", googleMapEnableDirections);

			//googleMapInitialZoom = WebUtils.ParseInt32FromHashtable(settings, "GoogleMapInitialZoomSetting", googleMapInitialZoom);

			pageSize = WebUtils.ParseInt32FromHashtable(settings, "BlogEntriesToShowSetting", pageSize);

			if (settings.Contains("OdiogoFeedIDSetting"))
			{
				odiogoFeedId = settings["OdiogoFeedIDSetting"].ToString();
			}

			if (settings.Contains("OdiogoPodcastUrlSetting"))
				odiogoPodcastUrl = settings["OdiogoPodcastUrlSetting"].ToString();

			hideAddThisButton = WebUtils.ParseBoolFromHashtable(settings, "BlogHideAddThisButtonSetting", hideAddThisButton);

			if (settings.Contains("BlogAddThisDotComUsernameSetting"))
			{
				addThisAccountId = settings["BlogAddThisDotComUsernameSetting"].ToString().Trim();
			}

			//useAddThisMouseOverWidget = WebUtils.ParseBoolFromHashtable(settings, "BlogAddThisDotComUseMouseOverWidgetSetting", useAddThisMouseOverWidget);


			//if (settings.Contains("BlogAddThisButtonImageUrlSetting"))
			//{
			//    string buttonImage = settings["BlogAddThisButtonImageUrlSetting"].ToString().Trim();
			//    if (buttonImage.Length > 0)
			//    {
			//        addThisButtonImageUrl = buttonImage;
			//    }
			//}

			if (settings.Contains("BlogAddThisRssButtonImageUrlSetting"))
				addThisRssButtonImageUrl = settings["BlogAddThisRssButtonImageUrlSetting"].ToString().Trim();

			//if (settings.Contains("BlogAddThisCustomBrandSetting"))
			//{
			//    addThisCustomBrand = settings["BlogAddThisCustomBrandSetting"].ToString().Trim();
			//}

			//if (settings.Contains("BlogAddThisCustomOptionsSetting"))
			//{
			//    addThisCustomOptions = settings["BlogAddThisCustomOptionsSetting"].ToString().Trim();
			//}

			//if (settings.Contains("BlogAddThisCustomLogoUrlSetting"))
			//{
			//    addThisCustomLogoUrl = settings["BlogAddThisCustomLogoUrlSetting"].ToString().Trim();
			//}

			//if (settings.Contains("BlogAddThisCustomLogoBackColorSetting"))
			//{
			//    addThisCustomLogoBackColor = settings["BlogAddThisCustomLogoBackColorSetting"].ToString().Trim();
			//}

			//if (settings.Contains("BlogAddThisCustomLogoForeColorSetting"))
			//{
			//    addThisCustomLogoForeColor = settings["BlogAddThisCustomLogoForeColorSetting"].ToString().Trim();
			//}

			if (settings.Contains("BlogFeedburnerFeedUrl"))
			{
				feedburnerFeedUrl = settings["BlogFeedburnerFeedUrl"].ToString().Trim();
			}

			if (settings.Contains("DisqusSiteShortName"))
			{
				disqusSiteShortName = settings["DisqusSiteShortName"].ToString();
			}

			if (settings.Contains("CommentSystemSetting"))
			{
				commentSystem = settings["CommentSystemSetting"].ToString();
			}

			if (settings.Contains("IntenseDebateAccountId"))
			{
				intenseDebateAccountId = settings["IntenseDebateAccountId"].ToString();
			}

			allowWebSiteUrlForComments = WebUtils.ParseBoolFromHashtable(settings, "AllowWebSiteUrlForComments", allowWebSiteUrlForComments);

			hideDetailsFromUnauthencticated = WebUtils.ParseBoolFromHashtable(settings, "HideDetailsFromUnauthencticated", hideDetailsFromUnauthencticated);

			if (settings.Contains("BlogCopyrightSetting"))
			{
				copyright = settings["BlogCopyrightSetting"].ToString();
			}

			showLeftContent = WebUtils.ParseBoolFromHashtable(settings, "ShowPageLeftContentSetting", showLeftContent);

			showRightContent = WebUtils.ParseBoolFromHashtable(settings, "ShowPageRightContentSetting", showRightContent);

			enableContentVersioning = WebUtils.ParseBoolFromHashtable(settings, "BlogEnableVersioningSetting", enableContentVersioning);

			defaultCommentDaysAllowed = WebUtils.ParseInt32FromHashtable(settings, "BlogCommentForDaysDefault", defaultCommentDaysAllowed);

			if (settings.Contains("BlogEditorHeightSetting"))
			{
				editorHeight = Unit.Parse(settings["BlogEditorHeightSetting"].ToString());

			}

			useCaptcha = WebUtils.ParseBoolFromHashtable(settings, "BlogUseCommentSpamBlocker", useCaptcha);

			requireAuthenticationForComments = WebUtils.ParseBoolFromHashtable(settings, "RequireAuthenticationForComments", requireAuthenticationForComments);

			notifyOnComment = WebUtils.ParseBoolFromHashtable(settings, "ContentNotifyOnComment", notifyOnComment);

			if (settings.Contains("BlogAuthorEmailSetting"))
			{
				notifyEmail = settings["BlogAuthorEmailSetting"].ToString();
			}

			useFacebookLikeButton = WebUtils.ParseBoolFromHashtable(settings, "UseFacebookLikeButton", useFacebookLikeButton);

			if (settings.Contains("FacebookLikeButtonTheme"))
			{
				facebookLikeButtonTheme = settings["FacebookLikeButtonTheme"].ToString().Trim();
			}

			facebookLikeButtonShowFaces = WebUtils.ParseBoolFromHashtable(settings, "FacebookLikeButtonShowFaces", facebookLikeButtonShowFaces);

			facebookLikeButtonWidth = WebUtils.ParseInt32FromHashtable(settings, "FacebookLikeButtonWidth", facebookLikeButtonWidth);

			facebookLikeButtonHeight = WebUtils.ParseInt32FromHashtable(settings, "FacebookLikeButtonHeight", facebookLikeButtonHeight);

			showTweetThisLink = WebUtils.ParseBoolFromHashtable(settings, "ShowTweetThisLink", showTweetThisLink);

			if (settings.Contains("UpperSidebar"))
			{
				upperSidebar = settings["UpperSidebar"].ToString();
			}

			if (settings.Contains("LowerSidebar"))
			{
				lowerSidebar = settings["LowerSidebar"].ToString();
			}

			//useBingMap = WebUtils.ParseBoolFromHashtable(settings, "UseBingMap", useBingMap);

			showNextPreviousLinks = WebUtils.ParseBoolFromHashtable(settings, "ShowNextPreviousLinks", showNextPreviousLinks);

			feedIsDisabled = WebUtils.ParseBoolFromHashtable(settings, "BlogDisableFeedSetting", feedIsDisabled);
			feedTimeToLive = WebUtils.ParseInt32FromHashtable(settings, "BlogRSSCacheTimeSetting", feedTimeToLive);
			addSignature = WebUtils.ParseBoolFromHashtable(settings, "RSSAddSignature", addSignature);
			addTweetThisToFeed = WebUtils.ParseBoolFromHashtable(settings, "AddTweetThisToFeed", addTweetThisToFeed);
			addFacebookLikeToFeed = WebUtils.ParseBoolFromHashtable(settings, "AddFacebookLikeToFeed", addFacebookLikeToFeed);
			addCommentsLinkToFeed = WebUtils.ParseBoolFromHashtable(settings, "RSSAddCommentsLink", addCommentsLinkToFeed);
			if (settings.Contains("BlogDescriptionSetting"))
			{
				channelDescription = settings["BlogDescriptionSetting"].ToString();
			}

			addFeedDiscoveryLink = WebUtils.ParseBoolFromHashtable(settings, "AddFeedDiscoveryLink", addFeedDiscoveryLink);

			showPlusOneButton = WebUtils.ParseBoolFromHashtable(settings, "ShowPlusOneButton", showPlusOneButton);

			maxFeedItems = WebUtils.ParseInt32FromHashtable(settings, "MaxFeedItems", maxFeedItems);
			relatedItemsToShow = WebUtils.ParseInt32FromHashtable(settings, "RelatedItemsToShow", relatedItemsToShow);

			requireApprovalForComments = WebUtils.ParseBoolFromHashtable(settings, "RequireApprovalForComments", requireApprovalForComments);
			allowCommentTitle = WebUtils.ParseBoolFromHashtable(settings, "AllowCommentTitle", allowCommentTitle);
			sortCommentsDescending = WebUtils.ParseBoolFromHashtable(settings, "SortCommentsDescending", sortCommentsDescending);

			allowedEditMinutesForUnModeratedPosts = WebUtils.ParseInt32FromHashtable(settings, "AllowedEditMinutesForUnModeratedPosts", allowedEditMinutesForUnModeratedPosts);

			//includeDownloadLinkForMediaAttachments = WebUtils.ParseBoolFromHashtable(settings, "IncludeDownloadLinkForMediaAttachments", includeDownloadLinkForMediaAttachments);

			//showAuthorAvatar = WebUtils.ParseBoolFromHashtable(settings, "ShowAuthorAvatar", showAuthorAvatar);

			showBlogSearchBox = WebUtils.ParseBoolFromHashtable(settings, "ShowBlogSearchBox", showBlogSearchBox);

			if (settings.Contains("ManagingEditorName"))
			{
				managingEditorName = settings["ManagingEditorName"].ToString();
			}

			if (settings.Contains("ManagingEditorEmail"))
			{
				managingEditorEmail = settings["ManagingEditorEmail"].ToString();
			}

			if (settings.Contains("LanguageCode"))
			{
				feedLanguageCode = settings["LanguageCode"].ToString();
			}

			if (settings.Contains("FeedLogoUrl"))
			{
				feedLogoUrl = settings["FeedLogoUrl"].ToString();
			}

			hasExplicitContent = WebUtils.ParseBoolFromHashtable(settings, "HasExplicitContent", hasExplicitContent);

			
			if (settings.Contains("iTunesMainCategory"))
			{
				feedMainCategory = settings["iTunesMainCategory"].ToString();
			}

			if (settings.Contains("iTunesSubCategory"))
			{
				feedSubCategory = settings["iTunesSubCategory"].ToString();
			}

			showCalendarOnPostDetail = WebUtils.ParseBoolFromHashtable(settings, "ShowCalendarOnPostDetail", showCalendarOnPostDetail);

			showGoogleNewsTabInEditPage = WebUtils.ParseBoolFromHashtable(settings, "ShowGoogleNewsTabInEditPage", showGoogleNewsTabInEditPage);

			if (settings.Contains("DefaultPublicationName"))
			{
				defaultPublicationName = settings["DefaultPublicationName"].ToString();
			}

			if (settings.Contains("DefaultPublicationLanguage"))
			{
				defaultPublicationLanguage = settings["DefaultPublicationLanguage"].ToString();
			}

			if (settings.Contains("PublicationAccess"))
			{
				publicationAccess = settings["PublicationAccess"].ToString();
			}

			if (settings.Contains("DefaultGenres"))
			{
				defaultGenres = settings["DefaultGenres"].ToString();
			}

			defaultIncludeInNewsChecked = WebUtils.ParseBoolFromHashtable(settings, "DefaultIncludeInNewsChecked", defaultIncludeInNewsChecked);
			defaultIncludeImageInExcerptChecked = WebUtils.ParseBoolFromHashtable(settings, "DefaultIncludeImageInExcerptChecked", defaultIncludeImageInExcerptChecked);
			defaultIncludeImageInPostChecked = WebUtils.ParseBoolFromHashtable(settings, "DefaultIncludeImageInPostChecked", defaultIncludeImageInPostChecked);

			featuredPostId = WebUtils.ParseInt32FromHashtable(settings, "FeaturedPostId", featuredPostId);

			if (settings.Contains("DefaultUrlPrefix"))
			{
				defaultUrlPrefix = settings["DefaultUrlPrefix"].ToString();
			}

			if (settings.Contains("ContentModules"))
			{
				//saved as JSON
				//{
				//	"ContentModules": [
				//			      {
				//      "ModuleID": 0,
				//			        "Location": [
				//			          "bottom",
				//			          "right"
				//        ],
				//      "SortOrder": 0
				//    },
				//    {
				//      "ModuleID": 1,
				//      "Location": [
				//        "left",
				//        "top"
				//        ],
				//      "SortOrder": 2
				//    }
				//  ]
				//}

				var contentModules = settings["ContentModules"].ToString();

				ContentModules = JsonConvert.DeserializeObject<List<ContentModule>>(contentModules);
			}
		}

		private bool defaultIncludeImageInExcerptChecked = false;

		public bool DefaultIncludeImageInExcerptChecked
		{
			get { return defaultIncludeImageInExcerptChecked; }
		}

		private bool defaultIncludeImageInPostChecked = false;

		public bool DefaultIncludeImageInPostChecked
		{
			get { return defaultIncludeImageInPostChecked; }
		}

		private bool defaultIncludeInNewsChecked = false;

		public bool DefaultIncludeInNewsChecked
		{
			get { return defaultIncludeInNewsChecked; }
		}

		private int featuredPostId = 0;

		public int FeaturedPostId
		{
			get { return featuredPostId; }
		}

		private string defaultGenres = string.Empty;

		public string DefaultGenres
		{
			get { return defaultGenres; }
		}

		private string publicationAccess = string.Empty;

		public string PublicationAccess
		{
			get { return publicationAccess; }
		}

		private string defaultPublicationLanguage = "en";

		public string DefaultPublicationLanguage
		{
			get { return defaultPublicationLanguage; }
		}

		private string defaultPublicationName = string.Empty;

		public string DefaultPublicationName
		{
			get { return defaultPublicationName; }
		}

		private bool showGoogleNewsTabInEditPage = false;

		public bool ShowGoogleNewsTabInEditPage
		{
			get { return showGoogleNewsTabInEditPage; }
		}

		private bool showCalendarOnPostDetail = true;

		public bool ShowCalendarOnPostDetail
		{
			get { return showCalendarOnPostDetail; }
		}

		private string feedMainCategory = string.Empty;

		public string FeedMainCategory
		{
			get { return feedMainCategory; }   
		}

		private string feedSubCategory = string.Empty;

		public string FeedSubCategory
		{
			get { return feedSubCategory; }
		}

		private bool hasExplicitContent = false;

		public bool HasExplicitContent
		{
			get { return hasExplicitContent; }
		}

		private string feedLogoUrl = string.Empty;

		public string FeedLogoUrl
		{
			get { return feedLogoUrl; }
		}

		private string feedLanguageCode = "en-US";

		public string FeedLanguageCode
		{
			get { return feedLanguageCode; }
		}

		private string managingEditorName = string.Empty;

		public string ManagingEditorName
		{
			get { return managingEditorName; }
		}

		private string managingEditorEmail = string.Empty;

		public string ManagingEditorEmail
		{
			get { return managingEditorEmail; }
		}


		private bool showBlogSearchBox = false;

		public bool ShowBlogSearchBox
		{
			get { return showBlogSearchBox; }
		}

		// for now this is hard coded maybe we will promote it to a configurable setting later
		// users can only edit their own posts unless in one of these roles
		// we don't currently have an approval process for blog posts
		// we are only now adding support for multiple users who can only edit their own posts
		private string approverRoles = "Admins;Content Administrators;";

		public string ApproverRoles
		{
			get { return approverRoles; }
		}

		//private bool showAuthorAvatar = false;

		//public bool ShowAuthorAvatar
		//{
		//    get { return showAuthorAvatar; }
		//}


		//private bool includeDownloadLinkForMediaAttachments = false;
		//public bool IncludeDownloadLinkForMediaAttachments
		//{
		//    get { return includeDownloadLinkForMediaAttachments; }
		//}
		

		private int allowedEditMinutesForUnModeratedPosts = 10;

		public int AllowedEditMinutesForUnModeratedPosts
		{
			get { return allowedEditMinutesForUnModeratedPosts; }
		}

		private bool sortCommentsDescending = false;
		public bool SortCommentsDescending
		{
			get { return sortCommentsDescending; }
		}

		private bool requireApprovalForComments = false;

		public bool RequireApprovalForComments
		{
			get { return requireApprovalForComments; }
		}

		private bool allowCommentTitle = true;
		public bool AllowCommentTitle
		{
			get { return allowCommentTitle; }
		}

		
		private int maxFeedItems = 20;

		public int MaxFeedItems
		{
			get { return maxFeedItems; }
		}

		private int relatedItemsToShow = 0;

		public int RelatedItemsToShow
		{
			get { return relatedItemsToShow; }
		}

		
		private bool feedIsDisabled = false;

		public bool FeedIsDisabled
		{
			get { return feedIsDisabled; }
		}

		private bool addFeedDiscoveryLink = true;

		public bool AddFeedDiscoveryLink
		{
			get { return addFeedDiscoveryLink; }
		}

		private string channelDescription = string.Empty;

		public string ChannelDescription
		{
			get { return channelDescription; }
		}

		private bool addTweetThisToFeed = false;

		public bool AddTweetThisToFeed
		{
			get { return addTweetThisToFeed; }
		}

		private bool addCommentsLinkToFeed = false;

		public bool AddCommentsLinkToFeed
		{
			get { return addCommentsLinkToFeed; }
		}

		private bool addFacebookLikeToFeed = false;

		public bool AddFacebookLikeToFeed
		{
			get { return addFacebookLikeToFeed; }
		}

		private bool addSignature = false;

		public bool AddSignature
		{
			get { return addSignature; }
		}

		private bool showNextPreviousLinks = true;

		public bool ShowNextPreviousLinks
		{
			get { return showNextPreviousLinks; }
		}

		private string upperSidebar = string.Empty;

		public string UpperSidebar
		{
			get { return upperSidebar; }
		}

		private string lowerSidebar = string.Empty;

		public string LowerSidebar
		{
			get { return lowerSidebar; }
		}

		private bool showPlusOneButton = false;

		public bool ShowPlusOneButton
		{
			get { return showPlusOneButton; }
		}

		private bool showTweetThisLink = false;

		public bool ShowTweetThisLink
		{
			get { return showTweetThisLink; }
		}

		private bool useFacebookLikeButton = false;

		public bool UseFacebookLikeButton
		{
			get { return useFacebookLikeButton; }
		}

		private int facebookLikeButtonHeight = 35;

		public int FacebookLikeButtonHeight
		{
			get { return facebookLikeButtonHeight; }
		}

		private int facebookLikeButtonWidth = 450;

		public int FacebookLikeButtonWidth
		{
			get { return facebookLikeButtonWidth; }
		}

		private bool facebookLikeButtonShowFaces = false;

		public bool FacebookLikeButtonShowFaces
		{
			get { return facebookLikeButtonShowFaces; }
		}

		private string facebookLikeButtonTheme = "light";

		public string FacebookLikeButtonTheme
		{
			get { return facebookLikeButtonTheme; }
		}


		private string notifyEmail = string.Empty;

		public string NotifyEmail
		{
			get { return notifyEmail; }
		}

		private bool notifyOnComment = false;

		public bool NotifyOnComment
		{
			get { return notifyOnComment; }
		}

		private bool requireAuthenticationForComments = false;

		public bool RequireAuthenticationForComments
		{
			get { return requireAuthenticationForComments; }
		}

		private bool useCaptcha = true;

		public bool UseCaptcha
		{
			get { return useCaptcha; }
		}

		private Unit editorHeight = Unit.Parse("350");

		public Unit EditorHeight
		{
			get { return editorHeight; }
		}

		private bool enableContentVersioning = false;

		public bool EnableContentVersioning
		{
			get { return enableContentVersioning; }
		}

		private int defaultCommentDaysAllowed = 90;

		public int DefaultCommentDaysAllowed
		{
			get { return defaultCommentDaysAllowed; }
		}

		private bool showLeftContent = false;

		public bool ShowLeftContent
		{
			get { return showLeftContent; }
		}


		private bool showRightContent = false;

		public bool ShowRightContent
		{
			get { return showRightContent; }
		}


		private string copyright = string.Empty;

		public string Copyright
		{
			get { return copyright; }
		}

		private bool hideDetailsFromUnauthencticated = false;

		public bool HideDetailsFromUnauthencticated
		{
			get { return hideDetailsFromUnauthencticated; }
		}

		private bool allowWebSiteUrlForComments = true;

		public bool AllowWebSiteUrlForComments
		{
			get { return allowWebSiteUrlForComments; }
		}

		private string commentSystem = "internal";

		public string CommentSystem
		{
			get { return commentSystem; }
		}

		private string intenseDebateAccountId = string.Empty;

		public string IntenseDebateAccountId
		{
			get { return intenseDebateAccountId; }
		}

		private string disqusSiteShortName = string.Empty;

		public string DisqusSiteShortName
		{
			get { return disqusSiteShortName; }
		}

		private string feedburnerFeedUrl = string.Empty;

		public string FeedburnerFeedUrl
		{
			get { return feedburnerFeedUrl; }
		}

		//private string addThisCustomLogoForeColor = string.Empty;

		//public string AddThisCustomLogoForeColor
		//{
		//    get { return addThisCustomLogoForeColor; }
		//}

		//private string addThisCustomLogoBackColor = string.Empty;

		//public string AddThisCustomLogoBackColor
		//{
		//    get { return addThisCustomLogoBackColor; }
		//}

		//private string addThisCustomLogoUrl = string.Empty;

		//public string AddThisCustomLogoUrl
		//{
		//    get { return addThisCustomLogoUrl; }
		//}

		//private string addThisCustomOptions = string.Empty;

		//public string AddThisCustomOptions
		//{
		//    get { return addThisCustomOptions; }
		//}

		//private string addThisCustomBrand = string.Empty;

		//public string AddThisCustomBrand
		//{
		//    get { return addThisCustomBrand; }
		//}

		//private string addThisButtonImageUrl = "~/Data/SiteImages/addthissharebutton.gif";

		//public string AddThisButtonImageUrl
		//{
		//    get { return addThisButtonImageUrl; }
		//}

		private string addThisRssButtonImageUrl = "~/Data/SiteImages/addthisrss.gif";

		public string AddThisRssButtonImageUrl
		{
			get { return addThisRssButtonImageUrl; }
		}

		//private bool useAddThisMouseOverWidget = true;

		//public bool UseAddThisMouseOverWidget
		//{
		//    get { return useAddThisMouseOverWidget; }
		//}

		private string addThisAccountId = string.Empty;

		public string AddThisAccountId
		{
			get { return addThisAccountId; }
		}

		private string odiogoFeedId = string.Empty;

		public string OdiogoFeedId
		{
			get { return odiogoFeedId; }
		}

		private string odiogoPodcastUrl = string.Empty;

		public string OdiogoPodcastUrl
		{
			get { return odiogoPodcastUrl; }
		}

		private int pageSize = 10;

		public int PageSize
		{
			get { return pageSize; }
		}

		//private bool useBingMap = false;

		//public bool UseBingMap
		//{
		//    get { return useBingMap; }
		//}

		//public string BingMapStyle
		//{
		//    get
		//    {
		//        switch (mapType)
		//        {
		//            case MapType.G_NORMAL_MAP:
		//                return "VEMapStyle.Road";

		//            case MapType.G_HYBRID_MAP:
		//                return "VEMapStyle.Hybrid";

		//            case MapType.G_PHYSICAL_MAP:
		//                return "VEMapStyle.Birdseye";

		//            case MapType.G_SATELLITE_MAP:
		//            default:
		//                return "VEMapStyle.Aerial";
		//        }
		//    }
		//}

		//private int googleMapInitialZoom = 13;

		//public int GoogleMapInitialZoom
		//{
		//    get { return googleMapInitialZoom; }
		//}

		//private bool googleMapEnableDirections = false;

		//public bool GoogleMapEnableDirections
		//{
		//    get { return googleMapEnableDirections; }
		//}

		//private bool googleMapEnableLocalSearch = false;

		//public bool GoogleMapEnableLocalSearch
		//{
		//    get { return googleMapEnableLocalSearch; }
		//}

		//private bool googleMapShowInfoWindow = false;

		//public bool GoogleMapShowInfoWindow
		//{
		//    get { return googleMapShowInfoWindow; }
		//}

		//private bool googleMapEnableZoom = false;

		//public bool GoogleMapEnableZoom
		//{
		//    get { return googleMapEnableZoom; }
		//}

		//private bool googleMapEnableMapType = false;

		//public bool GoogleMapEnableMapType
		//{
		//    get { return googleMapEnableMapType; }
		//}

		//private string googleMapWidth = "500px";

		//public string GoogleMapWidth
		//{
		//    get { return googleMapWidth; }
		//}

		//private int googleMapHeight = 300;

		//public int GoogleMapHeight
		//{
		//    get { return googleMapHeight; }
		//}

		

		//private MapType mapType = MapType.G_SATELLITE_MAP;

		//public MapType GoogleMapType
		//{
		//    get { return mapType; }
		//}

		//private bool showPostAuthor = false;

		//public bool ShowPostAuthor
		//{
		//    get { return showPostAuthor; }
		//}

		private bool useLinkForHeading = true;

		public bool UseLinkForHeading
		{
			get { return useLinkForHeading; }
		}

		private bool allowComments = true;

		public bool AllowComments
		{
			get { return allowComments; }
		}

		private bool includeCommentBodyInNotification = false;
		public bool IncludeCommentBodyInNotification
		{
			get { return includeCommentBodyInNotification; }
		}

		private bool navigationOnRight = false;

		public bool NavigationOnRight
		{
			get { return navigationOnRight; }
		}

		private bool showAddFeedLinks = true;

		public bool ShowAddFeedLinks
		{
			get { return showAddFeedLinks; }
		}

		private bool showFeedLinks = true;

		public bool ShowFeedLinks
		{
			get { return showFeedLinks; }
		}

		private bool showStatistics = true;

		public bool ShowStatistics
		{
			get { return showStatistics; }
		}

		private bool showArchives = false;

		public bool ShowArchives
		{
			get { return showArchives; }
		}

		private bool showCategories = false;

		public bool ShowCategories
		{
			get { return showCategories; }
		}

		private bool showCalendar = false;

		public bool ShowCalendar
		{
			get { return showCalendar; }
		}

		private bool useTagCloudForCategories = false;

		public bool UseTagCloudForCategories
		{
			get { return useTagCloudForCategories; }
		}

		private string dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;

		public string DateTimeFormat
		{
			get { return dateTimeFormat; }
		}

		private bool useExcerpt = false;

		public bool UseExcerpt
		{
			get { return useExcerpt; }
		}

		private bool useExcerptInFeed = false;

		public bool UseExcerptInFeed
		{
			get { return useExcerptInFeed; }
		}

		private bool titleOnly = false;

		public bool TitleOnly
		{
			get { return titleOnly; }
		}

		private bool showPager = true;

		public bool ShowPager
		{
			get { return showPager; }
		}

		private bool googleMapIncludeWithExcerpt = false;

		public bool GoogleMapIncludeWithExcerpt
		{
			get { return googleMapIncludeWithExcerpt; }
		}

		private bool enableContentRating = false;

		public bool EnableContentRating
		{
			get { return enableContentRating; }
		}

		private bool enableRatingComments = false;

		public bool EnableRatingComments
		{
			get { return enableRatingComments; }
		}

		private bool hideAddThisButton = false;

		public bool HideAddThisButton
		{
			get { return hideAddThisButton; }
		}

		private int excerptLength = 250;

		public int ExcerptLength
		{
			get { return excerptLength; }
		}

		private string excerptSuffix = "...";

		public string ExcerptSuffix
		{
			get { return excerptSuffix; }
		}

		private string moreLinkText = "read more";

		public string MoreLinkText
		{
			get { return moreLinkText; }
		}

		private string blogAuthor = string.Empty;

		public string BlogAuthor
		{
			get { return blogAuthor; }
		}

		private string instanceCssClass = string.Empty;

		public string InstanceCssClass
		{
			get { return instanceCssClass; }
		}

		private int feedTimeToLive = -1;

		public int FeedTimeToLive
		{
			get { return feedTimeToLive; }
		}

		private bool usePostTitleAsPageHeading = true;

		public bool UsePostTitleAsPageHeading
		{
			get => usePostTitleAsPageHeading;
		}

		private string defaultUrlPrefix = string.Empty;
		public string DefaultUrlPrefix
		{
			get => defaultUrlPrefix;
		}

		public static bool Create301OnPostRename
		{
			get { return ConfigHelper.GetBoolProperty("Blog:Create301OnPostRename", true); }
		}

		public static string BingMapDistanceUnit
		{
			get { return ConfigHelper.GetStringProperty("Blog:BingMapDistanceUnit", "VERouteDistanceUnit.Mile"); }
		}

		/// <summary>
		/// if true and the skin is using altcontent1 it will load the page content for that in the blog detail view
		/// </summary>
		public static bool ShowTopContent
		{
			get { return ConfigHelper.GetBoolProperty("Blog:ShowTopContent", false); }
		}

		/// <summary>
		/// if true and the skin is using altcontent2 it will load the page content for that in the blog detail view
		/// </summary>
		public static bool ShowBottomContent
		{
			get { return ConfigHelper.GetBoolProperty("Blog:ShowBottomContent", false); }
		}

		public static bool UseExcerptFromMetawblogAsMetaDescription
		{
			get { return ConfigHelper.GetBoolProperty("Blog:UseExcerptFromMetawblogAsMetaDescription", true); }
		}

		/// <summary>
		/// 165 is the max recommended by google
		/// </summary>
		public static int MetaDescriptionMaxLengthToGenerate
		{
			get { return ConfigHelper.GetIntProperty("Blog:MetaDescriptionMaxLengthToGenerate", 165); }
		}

		/// <summary>
		/// the database field holds up to 255 chars, so you can override this to shorter values but not longer values
		/// </summary>
		public static int PostTitleMaxLength
		{
			get { return ConfigHelper.GetIntProperty("Blog:PostTitleMaxLength", 255); }
		}

		/// <summary>
		/// google requires that only items published in the last 2 days (48 hours) are included in the news site map
		/// </summary>
		public static int NewsMapMaxHoursOld
		{
			get { return ConfigHelper.GetIntProperty("Blog:NewsMapMaxHoursOld", 48); }
		}

		public static bool UseRedirectForFeedburner
		{
			get { return ConfigHelper.GetBoolProperty("Blog:UseRedirectForFeedburner", true); }
		}

		public static bool IncludeAuthorEmailInFeed
		{
			get { return ConfigHelper.GetBoolProperty("Blog:IncludeAuthorEmailInFeed", true); }
		}

		public static bool AllowAttachments
		{
			get { return ConfigHelper.GetBoolProperty("Blog:AllowAttachments", true); }
		}

		public static bool ShowJanrainWidgetForLoginPrompt
		{
			get { return ConfigHelper.GetBoolProperty("Blog:ShowJanrainWidgetForLoginPrompt", true); }
		}

		public static bool UseCategoryFeedurlOnCategoryPage
		{
			get { return ConfigHelper.GetBoolProperty("Blog:UseCategoryFeedurlOnCategoryPage", true); }
		}

		public static int MaxAttachmentsToUploadAtOnce
		{
			get { return ConfigHelper.GetIntProperty("Blog:MaxAttachmentsToUploadAtOnce", 15); }
		}

		public static bool UseFriendlyUrls(int moduleId)
		{

			bool globalRule =  ConfigHelper.GetBoolProperty("Blog:UseFriendlyUrls", true);
			if (!globalRule) { return false; }

			bool moduleRule = ConfigHelper.GetBoolProperty("Blog:UseFriendlyUrls-" + moduleId.ToInvariantString(), true);
			return moduleRule;

		}

		public static bool BlogViewSuppressPageMenu
		{
			get { return ConfigHelper.GetBoolProperty("Blog:BlogViewSuppressPageMenu", true); }
		}

		public static bool EditPostSuppressPageMenu
		{
			get { return ConfigHelper.GetBoolProperty("Blog:EditPostSuppressPageMenu", true); }
		}

		public static bool UseNoIndexFollowMetaOnLists
		{
			get { return ConfigHelper.GetBoolProperty("Blog:UseNoIndexFollowMetaOnLists", true); }
		}

		public static bool UseHtmlDiff
		{
			get { return ConfigHelper.GetBoolProperty("Blog:UseHtmlDiff", true); }
		}

		public static bool UseLegacyCommentSystem
		{
			get { return ConfigHelper.GetBoolProperty("Blog:UseLegacyCommentSystem", false); }
		}

		public static bool SecurePostsByUser
		{
			get { return ConfigHelper.GetBoolProperty("Blog:SecurePostsByUser", true); }
		}


		public static bool IncludeInFeedCheckedByDefault
		{
			get { return ConfigHelper.GetBoolProperty("Blog:IncludeInFeedCheckedByDefault", true); }
		}

		public static bool IncludeInSearchIndexCheckedByDefault
		{
			get { return ConfigHelper.GetBoolProperty("Blog:IncludeInSearchIndexCheckedByDefault", true); }
		}

		public static bool ExcludeFromRecentContentCheckedByDefault
		{
			get { return ConfigHelper.GetBoolProperty("Blog:ExcludeFromRecentContentCheckedByDefault", false); }
		}

		public static bool IncludeInSiteMapCheckedByDefault
		{
			get { return ConfigHelper.GetBoolProperty("Blog:IncludeInSiteMapCheckedByDefault", true); }
		}

		public static bool IsPublishedCheckedByDefault
		{
			get { return ConfigHelper.GetBoolProperty("Blog:IsPublishedCheckedByDefault", true); }
		}

		public static bool ShowAuthorNameCheckedByDefault
		{
			get { return ConfigHelper.GetBoolProperty("Blog:ShowAuthorNameCheckedByDefault", true); }
		}


		public static bool ShowAuthorAvatarCheckedByDefault
		{
			get { return ConfigHelper.GetBoolProperty("Blog:ShowAuthorAvatarCheckedByDefault", false); }
		}

		public static bool ShowAuthorBioCheckedByDefault
		{
			get { return ConfigHelper.GetBoolProperty("Blog:ShowAuthorBioCheckedByDefault", false); }
		}

		public static List<ContentModule> ContentModules { get; set; }

		public static string GetBingMapType(string googleMapType)
		{
			switch (googleMapType)
			{
				case "G_NORMAL_MAP":
					return "VEMapStyle.Road";

				case "G_HYBRID_MAP":
					return "VEMapStyle.Hybrid";

				case "G_PHYSICAL_MAP":
					return "VEMapStyle.Birdseye";

				case "G_SATELLITE_MAP":
				default:
					return "VEMapStyle.Aerial";
			}
		   
		}

		public static string GetGoogleStaticMapType(string googleMapType)
		{
			switch (googleMapType)
			{
				case "G_NORMAL_MAP":
					return "roadmap";

				case "G_HYBRID_MAP":
					return "hybrid";

				case "G_PHYSICAL_MAP":
					return "terrain";

				case "G_SATELLITE_MAP":
				default:
					return "satellite";
			}

		}

		public static string GetBingStaticMapType(string googleMapType)
		{
			switch (googleMapType)
			{
				case "G_NORMAL_MAP":
					return "Road";

				case "G_HYBRID_MAP":
					return "AerialWithLabels";

				case "G_PHYSICAL_MAP":
					return "Aerial";

				case "G_SATELLITE_MAP":
				default:
					return "Aerial";
			}

		}
		public class ContentModule
		{
			public int ModuleID { get; set; }
			public int SortOrder { get; set; }
			public List<string> Locations { get; set; }
		}
	}

}