// Author:				    
// Created:			        2012-09-05
// Last Modified:		    2014-05-14
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Web.Controls.google;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI
{
	/// <summary>
	/// encapsulates the feature instance configuration loaded from module settings into a more friendly object
	/// </summary>
	public class CommentsConfiguration
	{
		public CommentsConfiguration()
		{ }

		public CommentsConfiguration(Hashtable settings)
		{
			LoadSettings(settings);


		}

		private void LoadSettings(Hashtable settings)
		{
			if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

			requireApprovalForComments = WebUtils.ParseBoolFromHashtable(settings, "RequireApprovalForComments", requireApprovalForComments);
			allowCommentTitle = WebUtils.ParseBoolFromHashtable(settings, "AllowCommentTitle", allowCommentTitle);
			sortCommentsDescending = WebUtils.ParseBoolFromHashtable(settings, "SortCommentsDescending", sortCommentsDescending);

			notifyOnComment = WebUtils.ParseBoolFromHashtable(settings, "ContentNotifyOnComment", notifyOnComment);

			if (settings.Contains("NotifyEmailSetting"))
			{
				notifyEmail = settings["NotifyEmailSetting"].ToString();
			}

			useCaptcha = WebUtils.ParseBoolFromHashtable(settings, "UseCommentSpamBlocker", useCaptcha);

			requireAuthenticationForComments = WebUtils.ParseBoolFromHashtable(settings, "RequireAuthenticationForComments", requireAuthenticationForComments);

			AllowComments = WebUtils.ParseBoolFromHashtable(settings, "AllowComments", AllowComments);

			allowWebSiteUrlForComments = WebUtils.ParseBoolFromHashtable(settings, "AllowWebSiteUrlForComments", allowWebSiteUrlForComments);

			if (settings.Contains("DateTimeFormat"))
			{
				string format = settings["DateTimeFormat"].ToString().Trim();
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

			if (settings.Contains("CustomCssClassSetting"))
			{
				InstanceCssClass = settings["CustomCssClassSetting"].ToString();
			}

			allowedEditMinutesForUnModeratedPosts = WebUtils.ParseInt32FromHashtable(settings, "AllowedEditMinutesForUnModeratedPosts", allowedEditMinutesForUnModeratedPosts);

			if (settings.Contains("CommentSystemSetting"))
			{
				commentSystem = settings["CommentSystemSetting"].ToString();
			}

			disableAvatars = WebUtils.ParseBoolFromHashtable(settings, "DisableAvatars", disableAvatars);
			CheckKeywordBlacklist = WebUtils.ParseBoolFromHashtable(settings, "CheckKeywordBlacklist", CheckKeywordBlacklist);

		}

		private bool disableAvatars = false;
		public bool DisableAvatars
		{
			get { return disableAvatars; }
		}

		private string commentSystem = "internal";

		public string CommentSystem
		{
			get { return commentSystem; }
		}

		private int allowedEditMinutesForUnModeratedPosts = 10;

		public int AllowedEditMinutesForUnModeratedPosts
		{
			get { return allowedEditMinutesForUnModeratedPosts; }
		}

		private string dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;

		public string DateTimeFormat
		{
			get { return dateTimeFormat; }
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

		private bool allowWebSiteUrlForComments = true;

		public bool AllowWebSiteUrlForComments
		{
			get { return allowWebSiteUrlForComments; }
		}

		public bool AllowComments { get; private set; } = true;

		public string InstanceCssClass { get; private set; } = string.Empty;

		public bool CheckKeywordBlacklist { get; set; } = true;

		private const string featureGuid = "06451ec6-d4d7-47e3-a1ce-d19aaf7f98fe";

		public static Guid FeatureGuid
		{
			get { return new Guid(featureGuid); }
		}

	}
}