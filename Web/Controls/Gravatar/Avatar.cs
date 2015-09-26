// Author:					Joe Audette
// Created:				    2012-08-08
// Last Modified:			2012-10-27
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//

using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;


namespace mojoPortal.Web.UI
{
    /// <summary>
    /// encapsulates the logic for showing (or not) an avatar either using gravatar or the internal user upload avatar system
    /// based on bindable properties
    /// </summary>
    public class Avatar : WebControl
    {

        

        public const string SecureUrl = "https://secure.gravatar.com/avatar/";
        public const string StandardUrl = "http://www.gravatar.com/avatar/";

        public enum RatingType { G, PG, R, X }

        private string _email;
        private short _size = 80;
        private RatingType _maxAllowedRating;
        //private string _defaultImage = string.Empty;

        

        // customise the link title:
        private string _linkTitle = "Get your avatar";
        private string _linkUrl = "http://www.gravatar.com";


        private int siteId = -1;

        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        private int userId = -1;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string siteRoot = string.Empty;

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }


        private bool useGravatar = true;

        /// <summary>
        /// if true uses gravatar, if false uses the internal site avatar system, default is true
        /// </summary>
        public bool UseGravatar
        {
            get { return useGravatar; }
            set { useGravatar = value; }
        }

        private bool disable = false;

        /// <summary>
        /// if true doesn't render at all
        /// </summary>
        public bool Disable
        {
            get { return disable; }
            set { disable = value; }
        }

        private string avatarFile = string.Empty;

        public string AvatarFile
        {
            get { return avatarFile; }
            set { avatarFile = value; }
        }

        /// <summary>
        /// The Email for the user
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Email
        {
            get { return _email; }
            set { _email = value.ToLower(); }
        }

        private string userName = string.Empty;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string userNameTooltipFormat = string.Empty;
        /// <summary>
        /// if specified the alt text for the image will use this format string and pass in the username
        /// </summary>
        public string UserNameTooltipFormat
        {
            get { return userNameTooltipFormat; }
            set { userNameTooltipFormat = value; }
        }

        /// <summary>
        /// Size of Gravatar image.  Must be between 1 and 512.
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("80")]
        public short Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// An optional "rating" parameter may follow with a value of [ G | PG | R | X ] that determines the highest rating (inclusive) that will be returned.
        /// </summary>
        public RatingType MaxAllowedRating
        {
            get { return _maxAllowedRating; }
            set { _maxAllowedRating = value; }
        }

        // output gravatar site link true by default:
        private bool _outputGravatarSiteLink = true;

        /// <summary>
        /// Determines whether the image is wrapped in an anchor tag linking to the Gravatar sit
        /// </summary>
        public bool OutputGravatarSiteLink
        {
            get { return _outputGravatarSiteLink; }
            set { _outputGravatarSiteLink = value; }
        }

        /// <summary>
        /// Optional property for link title for gravatar website link
        /// </summary>
        public string LinkTitle
        {
            get { return _linkTitle; }
            set { _linkTitle = value; }
        }

        /// <summary>
        /// By default links to Gravatar so users can get a gravatar, but you can override it and link to a user profile for example
        /// </summary>
        public string LinkUrl
        {
            get { return _linkUrl; }
            set { _linkUrl = value; }
        }

        ///// <summary>
        ///// An optional "default" parameter may follow that specifies the full, URL encoded URL, protocol included, of a GIF, JPEG, or PNG image that should be returned if either the requested email address has no associated gravatar, or that gravatar has a rating higher than is allowed by the "rating" parameter.
        ///// </summary>
        //[Bindable(true), Category("Appearance"), DefaultValue("")]
        //public string DefaultImage
        //{
        //    get { return _defaultImage; }
        //    set { _defaultImage = value; }
        //}

        private string defaultInternalAvatar = "~/Data/SiteImages/anonymous.png";

        public string DefaultInternalAvatar
        {
            get { return defaultInternalAvatar; }
            set { defaultInternalAvatar = value; }
        }

        private bool useInternalDefaultForGravatar = true;
        /// <summary>
        /// if true uses the DefaultInternalAvatar for users who have no gravatar
        /// note that this doesn't work from localhost because gravatar cannot load the image
        /// </summary>
        public bool UseInternalDefaultForGravatar
        {
            get { return useInternalDefaultForGravatar; }
            set { useInternalDefaultForGravatar = value; }
        }

        private string target = string.Empty;

        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        private string imageUrl = string.Empty;

        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        /// <summary>
        /// Gets the base Url based on whether the request is secure or not.
        /// Added by Joe Audette 2008-08-13
        /// </summary>
        public string GravatarBaseUrl
        {
            get
            {
                if (Page.Request.IsSecureConnection) { return SecureUrl; }
                return StandardUrl;
            }
        }

        private bool useLink = false;

        public bool UseLink
        {
            get { return useLink; }
            set { useLink = value; }
        }

        private bool autoConfigure = false;
        public bool AutoConfigure
        {
            get { return autoConfigure; }
            set { autoConfigure = value; }
        }

        private bool disableUseLinkWithAutoConfigure = true;
        public bool DisableUseLinkWithAutoConfigure
        {
            get { return disableUseLinkWithAutoConfigure; }
            set { disableUseLinkWithAutoConfigure = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!autoConfigure) { return; }

            DoAutoConfiguration();
        }

        private string gravatarFallbackEmailAddress = string.Empty;

        public string GravatarFallbackEmailAddress
        {
            get { return gravatarFallbackEmailAddress; }
            set { gravatarFallbackEmailAddress = value; }
        }

        private void DoAutoConfiguration()
        {
            if (disable) { return; }
            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) { disable = true; return; }
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { disable = true; return; }

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    useGravatar = true;
                    disable = false;
                    break;

                case "internal":
                    useGravatar = false;
                    disable = false;
                    break;

                case "none":
                default:
                    useGravatar = false;
                    disable = true;
                    break;

            }

            siteId = siteSettings.SiteId;
            _maxAllowedRating = SiteUtils.GetMaxAllowedGravatarRating();
            userId = currentUser.UserId;
            avatarFile = currentUser.AvatarUrl;
            Email = currentUser.Email;
            if (disableUseLinkWithAutoConfigure)
            {
                UseLink = false;
            }

        }


        protected override void Render(HtmlTextWriter output)
        {
            if (HttpContext.Current == null)
            {
                output.Write("[" + this.ID + "]");
                return;
            }

            if (disable) { return; }
            //if (siteId == -1) { return; }

            

            if (useGravatar)
            {
                if (string.IsNullOrEmpty(Email)) 
                {
                    if (gravatarFallbackEmailAddress.Length > 0)
                    {
                        Email = gravatarFallbackEmailAddress;
                    }
                    else
                    {
                        return;
                    }
                }

                if (CssClass.Length == 0) { CssClass = "avatar"; }
                RenderGravater(output);
            }
            else
            {
                RenderInternalAvatar(output);
            }

            
        }

        protected void RenderInternalAvatar(HtmlTextWriter output)
        {
            output.Write(GetAvatarMarkup());
        }

        private string GetAvatarMarkup()
        {
            

            //// if we get here we are using our own crappy avatars
            //if ((avatar == null) || (avatar == string.Empty))
            //{
            //    avatar = "blank.gif";
            //}

            // user profile follows the same view rules as member list
            //if (Page.Request.IsAuthenticated)
            //{
                // if we know the user is signed in and not in a role allowed then return username without a profile link
                if ((!useLink)||(_linkUrl.Length == 0))
                {
                    return "<img  alt='" + LinkTitle + "' src='" + GetInternalAvatarUrl() + "' class='avatar' />";
                }
            //}


                return "<a rel='nofollow' href='" + GetProfileUrl() + "' class='avatar'><img  alt='" + GetAltText() + "' src='" + GetInternalAvatarUrl() + "' /></a>";


        }

        private string GetProfileUrl()
        {

            if (userId > -1)
            {
                return SiteRoot + "/ProfileView.aspx?userid=" + userId.ToInvariantString();
            }

            return _linkUrl;
            
        }

        private string GetInternalAvatarUrl()
        {
           
            // if we get here we are using our own avatars
            if ((SiteId == -1)||(string.IsNullOrEmpty(avatarFile))||(avatarFile == "blank.gif"))
            {
                return Page.ResolveUrl(defaultInternalAvatar);
            }

            return Page.ResolveUrl("~/Data/Sites/" + SiteId.ToInvariantString() + "/useravatars/" + avatarFile);

        }

        private string GetAltText()
        {
            if (userNameTooltipFormat.Length > 0)
            {
                return string.Format(CultureInfo.InvariantCulture, userNameTooltipFormat, Page.Server.HtmlEncode(userName));
            }

            return LinkTitle;

        }

        protected void RenderGravater(HtmlTextWriter output)
        {
            
            
            // output the default attributes:
            AddAttributesToRender(output);

            if (useLink) { _linkUrl = GetProfileUrl(); }

            // if the size property has been specified, ensure it is a short, and in the range 
            // 1..512:
            try
            {
                // if it's not in the allowed range, throw an exception:
                if (Size < 1 || Size > 512)
                    throw new ArgumentOutOfRangeException();
            }
            catch
            {
                Size = 80;
            }

            // default the image url:
            //string imageUrl = "http://www.gravatar.com/avatar.php?";
            // changes by Joe Audette 2008-08-13
            string imageUrl = GravatarBaseUrl;

            if (!string.IsNullOrEmpty(Email))
            {
                // build up image url, including MD5 hash for supplied email:
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                UTF8Encoding encoder = new UTF8Encoding();
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

                byte[] hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(Email));

                StringBuilder sb = new StringBuilder(hashedBytes.Length * 2);
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    sb.Append(hashedBytes[i].ToString("X2"));
                }

                // output parameters:
                //imageUrl += "gravatar_id=" + sb.ToString().ToLower();
                //imageUrl += "&rating=" + MaxAllowedRating.ToString();
                //imageUrl += "&size=" + Size.ToString();

                // changes by Joe Audette 2008-08-13
                imageUrl += sb.ToString().ToLower();
                imageUrl += ".jpg?r=" + MaxAllowedRating.ToString();
                imageUrl += "&s=" + Size.ToString();
            }

            // output default parameter if specified
            //if (!string.IsNullOrEmpty(DefaultImage))
            //{
            if((useInternalDefaultForGravatar)&&(defaultInternalAvatar.Length > 0))
            {
                string defaultImageUrl = WebUtils.ResolveServerUrl(defaultInternalAvatar);
                if (!defaultImageUrl.Contains("localhost"))
                {
                    imageUrl += "&default=" + HttpUtility.UrlEncode(defaultImageUrl);
                }
            }

            // if we need to output the site link:
            if (OutputGravatarSiteLink)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Href, LinkUrl);
                output.AddAttribute(HtmlTextWriterAttribute.Title, LinkTitle);
                if (CssClass.Length > 0)
                {
                    output.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                }
                if (target.Length > 0)
                {
                    output.AddAttribute(HtmlTextWriterAttribute.Target, target);
                }
                output.RenderBeginTag(HtmlTextWriterTag.A);
            }

            // output required attributes/img tag:
            output.AddAttribute(HtmlTextWriterAttribute.Width, Size.ToString());
            output.AddAttribute(HtmlTextWriterAttribute.Height, Size.ToString());
            output.AddAttribute(HtmlTextWriterAttribute.Src, imageUrl);
            output.AddAttribute(HtmlTextWriterAttribute.Alt, "Gravatar");
            output.RenderBeginTag("img");
            output.RenderEndTag();

            // if we need to output the site link:
            if (OutputGravatarSiteLink)
            {
                output.RenderEndTag();
            }
        }

    }
}