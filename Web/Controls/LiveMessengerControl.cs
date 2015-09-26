///	Created:			    2009-04-14
///	Last Modified:		    2009-04-18
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using log4net;

namespace mojoPortal.Web.UI 
{
    public class LiveMessengerControl : WebControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LiveMessengerControl));

        // References

        //http://messenger.mslivelabs.com/
        //http://msdn.microsoft.com/en-us/library/dd570035.aspx

        // Delegated authentication
        //http://msdn.microsoft.com/en-us/library/dd570048.aspx

        // consent
        //http://msdn.microsoft.com/en-us/library/cc287631.aspx

        //Working with Dynamic UI Controls
        //http://msdn.microsoft.com/en-us/library/dd570109.aspx

        //http://blogs.msdn.com/messenger/archive/2009/04/03/better-experiences-around-user-generated-content.aspx

        // IM control paramter ref
        //http://msdn.microsoft.com/en-us/library/bb936685.aspx

        // format from copy paste from here http://settings.messenger.live.com/applications/CreateHtml.aspx?wa=wsignin1.0

        //<iframe src="http://settings.messenger.live.com/Conversation/IMMe.aspx?invitee=xxxxxxxxx@apps.messenger.live.com&amp;mkt=en-US" 
        //width="300" 
        //height="300" 
        //style="border: solid 1px black; width: 300px; height: 300px;" 
        //frameborder="0"></iframe>

        // format for consent token
        //http://settings.messenger.live.com/conversation/imme.aspx?invitee=@C@{0}&mkt={1}&dt={2}

       

        #region Private Properties

        const string BlueTheme = "blue";
        const string GreenTheme = "green";
        const string OrangeTheme = "orange";
        const string PinkTheme = "pink";
        const string PurpleTheme = "purple";
        const string GrayTheme = "gray";

        // required
        private string invitee = string.Empty; //invitee The user being messaged.
        private string overrideCulture = string.Empty; //mkt Specifies a culture ID indicating the language in which to localize the UI text for the control.

        //optional
        private string delegationToken = string.Empty;
        private string inviterEmailAddress = string.Empty; //wl_sig_source Inviter e-mail address. Used only for Delegated Authentication.
        private string inviterDisplayName = string.Empty; //wl_sig_source_display_name Inviter display name. Used only for Delegated Authentication.
        private string inviteeDisplayName = string.Empty; //wl_sig_target_display_name Invitee display name. Used only for Delegated Authentication.
        private bool useTheme = false; //useTheme Boolean indicating whether custom themes should be applied.
        private string themeName = string.Empty;
        private string foreColor = string.Empty; //foreColor Text color. 8 characters text string is always gray colored.
        private string backColor = string.Empty;
        private string linkColor = string.Empty;
        private string borderColor = string.Empty;
        private string buttonForeColor = string.Empty;
        private string buttonBackColor = string.Empty;
        private string buttonBorderColor = string.Empty;
        private string buttonDisabledColor = string.Empty;
        private string headerForeColor = string.Empty;
        private string headerBackColor = string.Empty;
        private string menuForeColor = string.Empty;
        private string menuBackColor = string.Empty;
        private string chatForeColor = string.Empty;
        private string chatBackColor = string.Empty;
        private string chatDisabledColor = string.Empty;
        private string chatErrorColor = string.Empty;
        private string chatLabelColor = string.Empty;

        private string signedParams = string.Empty;

        

        #endregion

        #region Public Properties

        public string Invitee
        {
            get { return invitee; }
            set { invitee = value; }
        }

        public string InviteeDisplayName
        {
            get { return inviteeDisplayName; }
            set { inviteeDisplayName = value; }
        }

        public string OverrideCulture
        {
            get { return overrideCulture; }
            set { overrideCulture = value; }
        }

        public string DelegationToken
        {
            get { return delegationToken; }
            set { delegationToken = value; }
        }

        public string SignedParams
        {
            get { return signedParams; }
            set { signedParams = value; }
        }

        public string InviterEmailAddress
        {
            get { return inviterEmailAddress; }
            set { inviterEmailAddress = value; }
        }

        public string InviterDisplayName
        {
            get { return inviterDisplayName; }
            set { inviterDisplayName = value; }
        }

        public bool UseTheme
        {
            get { return useTheme; }
            set { useTheme = value; }
        }

        /// <summary>
        /// valid values are empty, blue, green, orange, pink, purple, gray
        /// </summary>
        public string ThemeName
        {
            get { return themeName; }
            set { themeName = value; }
        }

        //public new string ForeColor
        //{
        //    get { return foreColor; }
        //    set { foreColor = value; }
        //}

        //public new string BackColor
        //{
        //    get { return backColor; }
        //    set { backColor = value; }
        //}

        //public string LinkColor
        //{
        //    get { return linkColor; }
        //    set { linkColor = value; }
        //}

        //public new string BorderColor
        //{
        //    get { return borderColor; }
        //    set { borderColor = value; }
        //}

        //public string ButtonForeColor
        //{
        //    get { return buttonForeColor; }
        //    set { buttonForeColor = value; }
        //}

        //public string ButtonBackColor
        //{
        //    get { return buttonBackColor; }
        //    set { buttonBackColor = value; }
        //}

        //public string ButtonBorderColor
        //{
        //    get { return buttonBorderColor; }
        //    set { buttonBorderColor = value; }
        //}

        //public string ButtonDisabledColor
        //{
        //    get { return buttonDisabledColor; }
        //    set { buttonDisabledColor = value; }
        //}

        //public string HeaderForeColor
        //{
        //    get { return headerForeColor; }
        //    set { headerForeColor = value; }
        //}

        //public string HeaderBackColor
        //{
        //    get { return headerBackColor; }
        //    set { headerBackColor = value; }
        //}

        //public string MenuForeColor
        //{
        //    get { return menuForeColor; }
        //    set { menuForeColor = value; }
        //}

        //public string MenuBackColor
        //{
        //    get { return menuBackColor; }
        //    set { menuBackColor = value; }
        //}

        //public string ChatForeColor
        //{
        //    get { return chatForeColor; }
        //    set { chatForeColor = value; }
        //}

        //public string ChatBackColor
        //{
        //    get { return chatBackColor; }
        //    set { chatBackColor = value; }
        //}

        //public string ChatDisabledColor
        //{
        //    get { return chatDisabledColor; }
        //    set { chatDisabledColor = value; }
        //}

        //public string ChatErrorColor
        //{
        //    get { return chatErrorColor; }
        //    set { chatErrorColor = value; }
        //}

        //public string ChatLabelColor
        //{
        //    get { return chatLabelColor; }
        //    set { chatLabelColor = value; }
        //}

        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }
            

            if (invitee.Length == 0)
            {
                log.Info("LiveMessengerControl did not render because no invitee is configured.");
                return;
            }

            writer.Write("<iframe ");
            
            if (inviteeDisplayName.Length > 0)
            {
                writer.Write("title='" + Page.Server.HtmlEncode(string.Format(CultureInfo.InvariantCulture, Resource.ChatTitleFormat, inviteeDisplayName)) + "' ");
            }
            else
            {
                writer.Write("title='" + Page.Server.HtmlEncode(Resource.LiveMessengerChat) + "' ");
            }

            writer.Write("src='http://settings.messenger.live.com/conversation/imme.aspx?invitee=");

            if (invitee.Contains("@apps.messenger.live.com"))
            {
                writer.Write(invitee);
            }
            else
            {
                writer.Write("@C@");
                writer.Write(invitee);
                //writer.Write("@apps.messenger.live.com");
            }

            writer.Write("&amp;mkt=");
            if (overrideCulture.Length > 0)
            {
                writer.Write(Page.Server.UrlEncode(overrideCulture));
            }
            else
            {
                writer.Write(CultureInfo.CurrentUICulture.Name);
            }

            // passing this seems to break it
            if (delegationToken.Length > 0)
            {
                writer.Write("&amp;dt=");
                if (WebConfigSettings.EncodeLiveMessengerToken)
                {
                    writer.Write(Page.Server.UrlEncode(delegationToken));
                }
                else
                {
                    writer.Write(delegationToken);
                }
            }

            if (inviterEmailAddress.Length > 0)
            {
                writer.Write("&amp;wl_sig_source=");
                writer.Write(Page.Server.UrlEncode(inviterEmailAddress));
            }

            if (inviterDisplayName.Length > 0)
            {
                writer.Write("&amp;wl_sig_source_display_name=");
                writer.Write(Page.Server.UrlEncode(inviterDisplayName));
            }

            if ((delegationToken.Length > 0)&&(inviteeDisplayName.Length > 0))
            {
                writer.Write("&amp;wl_sig_target_display_name=");
                writer.Write(Page.Server.UrlEncode(inviteeDisplayName));
            }

            if (signedParams.Length > 0)
            {
                writer.Write("&amp;sparams=");
                writer.Write(Uri.EscapeDataString(signedParams));

            }
            

            if (themeName.Length > 0)
            {
                

                switch (themeName)
                {
                    case BlueTheme:
                        SetBlueTheme();
                        break;

                    case GreenTheme:
                        SetGreenTheme();
                        break;

                    case OrangeTheme:
                        SetOrangeTheme();
                        break;

                    case PinkTheme:
                        SetPinkTheme();
                        break;

                    case PurpleTheme:
                        SetPurpleTheme();
                        break;

                    case GrayTheme:
                        SetGrayTheme();
                        break;

                    default:

                        useTheme = false;
                        themeName = string.Empty;
                        break;

                }

                if (themeName.Length > 0)
                {
                    writer.Write("&amp;useTheme=true");

                    writer.Write("&amp;themeName=");
                    writer.Write(Page.Server.UrlEncode(themeName));

                    if (foreColor.Length > 0)
                    {
                        writer.Write("&amp;foreColor=");
                        writer.Write(Page.Server.UrlEncode(foreColor));
                    }

                    if (backColor.Length > 0)
                    {
                        writer.Write("&amp;backColor=");
                        writer.Write(Page.Server.UrlEncode(backColor));
                    }

                    if (linkColor.Length > 0)
                    {
                        writer.Write("&amp;linkColor=");
                        writer.Write(Page.Server.UrlEncode(linkColor));
                    }

                    if (borderColor.Length > 0)
                    {
                        writer.Write("&amp;borderColor=");
                        writer.Write(Page.Server.UrlEncode(borderColor));
                    }

                    if (buttonForeColor.Length > 0)
                    {
                        writer.Write("&amp;buttonForeColor=");
                        writer.Write(Page.Server.UrlEncode(buttonForeColor));
                    }

                    if (buttonBackColor.Length > 0)
                    {
                        writer.Write("&amp;buttonBackColor=");
                        writer.Write(Page.Server.UrlEncode(buttonBackColor));
                    }

                    if (buttonBorderColor.Length > 0)
                    {
                        writer.Write("&amp;buttonBorderColor=");
                        writer.Write(Page.Server.UrlEncode(buttonBorderColor));
                    }

                    if (buttonDisabledColor.Length > 0)
                    {
                        writer.Write("&amp;buttonDisabledColor=");
                        writer.Write(Page.Server.UrlEncode(buttonDisabledColor));
                    }

                    if (headerForeColor.Length > 0)
                    {
                        writer.Write("&amp;headerForeColor=");
                        writer.Write(Page.Server.UrlEncode(headerForeColor));
                    }

                    if (headerBackColor.Length > 0)
                    {
                        writer.Write("&amp;headerBackColor=");
                        writer.Write(Page.Server.UrlEncode(headerBackColor));
                    }

                    if (menuForeColor.Length > 0)
                    {
                        writer.Write("&amp;menuForeColor=");
                        writer.Write(Page.Server.UrlEncode(menuForeColor));
                    }

                    if (menuBackColor.Length > 0)
                    {
                        writer.Write("&amp;menuBackColor=");
                        writer.Write(Page.Server.UrlEncode(menuBackColor));
                    }

                    if (chatForeColor.Length > 0)
                    {
                        writer.Write("&amp;chatForeColor=");
                        writer.Write(Page.Server.UrlEncode(chatForeColor));
                    }

                    if (chatBackColor.Length > 0)
                    {
                        writer.Write("&amp;chatBackColor=");
                        writer.Write(Page.Server.UrlEncode(chatBackColor));
                    }

                    if (chatDisabledColor.Length > 0)
                    {
                        writer.Write("&amp;chatDisabledColor=");
                        writer.Write(Page.Server.UrlEncode(chatDisabledColor));
                    }

                    if (chatErrorColor.Length > 0)
                    {
                        writer.Write("&amp;chatErrorColor=");
                        writer.Write(Page.Server.UrlEncode(chatErrorColor));
                    }

                    if (chatLabelColor.Length > 0)
                    {
                        writer.Write("&amp;chatLabelColor=");
                        writer.Write(Page.Server.UrlEncode(chatLabelColor));
                    }
                }


            }

            
            //end quote for url
            writer.Write("' ");

            writer.Write("width='" + this.Width.ToString() + "' ");

            writer.Write("height='" + this.Height.ToString() + "' ");

            writer.Write("style='border: solid 1px black; width: " + this.Width.ToString() + "; height: " + this.Height.ToString() + ";' ");

            // not valid html 5
            writer.Write("frameborder='0' scrolling='no'");

            writer.Write(">");

            writer.Write("</iframe>");

        }



        private void SetBlueTheme()
        {
            useTheme = true;
            foreColor = "333333";
            backColor = "E8F1F8";
            linkColor = "333333";
            borderColor = "AFD3EB";
            buttonForeColor = "333333";
            buttonBackColor = "EEF7FE";
            buttonBorderColor = "AFD3EB";
            buttonDisabledColor = "EEF7FE";
            headerForeColor = "0066A7";
            headerBackColor = "8EBBD8";
            menuForeColor = "333333";
            menuBackColor = "FFFFFF";
            chatForeColor = "333333";
            chatBackColor = "FFFFFF";
            chatDisabledColor = "F6F6F6";
            chatErrorColor = "760502";
            chatLabelColor = "6E6C6C";

        }

        private void SetGreenTheme()
        {
            useTheme = true;
            foreColor = "333333";
            backColor = "DCF2E5";
            linkColor = "333333";
            borderColor = "8ED4AB";
            buttonForeColor = "2C0034";
            buttonBackColor = "CFE9D9";
            buttonBorderColor = "8ED4AB";
            buttonDisabledColor = "CFE9D9";
            headerForeColor = "006629";
            headerBackColor = "92D6AE";
            menuForeColor = "006629";
            menuBackColor = "FFFFFF";
            chatForeColor = "333333";
            chatBackColor = "F4FBF7";
            chatDisabledColor = "F6F6F6";
            chatErrorColor = "760502";
            chatLabelColor = "6E6C6C";

        }

        private void SetOrangeTheme()
        {
            useTheme = true;
            foreColor = "333333";
            backColor = "FDC098";
            linkColor = "333333";
            borderColor = "FB8233";
            buttonForeColor = "333333";
            buttonBackColor = "FFC9A5";
            buttonBorderColor = "FB8233";
            buttonDisabledColor = "FFC9A5";
            headerForeColor = "333333";
            headerBackColor = "FC9E60";
            menuForeColor = "333333";
            menuBackColor = "FFFFFF";
            chatForeColor = "333333";
            chatBackColor = "FFFFFF";
            chatDisabledColor = "F6F6F6";
            chatErrorColor = "760502";
            chatLabelColor = "6E6C6C";

        }

        private void SetPinkTheme()
        {
            useTheme = true;
            foreColor = "444444";
            backColor = "FFD5D5";
            linkColor = "444444";
            borderColor = "ED7B7B";
            buttonForeColor = "AA3636";
            buttonBackColor = "FAD6D6";
            buttonBorderColor = "AA3636";
            buttonDisabledColor = "FAD6D6";
            headerForeColor = "444444";
            headerBackColor = "F9A3A3";
            menuForeColor = "E45A5A";
            menuBackColor = "FFFFFF";
            chatForeColor = "444444";
            chatBackColor = "FEF6F6";
            chatDisabledColor = "F6F6F6";
            chatErrorColor = "760502";
            chatLabelColor = "6E6C6C";

        }

        private void SetPurpleTheme()
        {
            useTheme = true;
            foreColor = "333333";
            backColor = "F1EFF4";
            linkColor = "333333";
            borderColor = "AFA9B4";
            buttonForeColor = "333333";
            buttonBackColor = "DED6DE";
            buttonBorderColor = "AFA9B4";
            buttonDisabledColor = "DED6DE";
            headerForeColor = "513663";
            headerBackColor = "AEA1B9";
            menuForeColor = "333333";
            menuBackColor = "FFFFFF";
            chatForeColor = "333333";
            chatBackColor = "FFFFFF";
            chatDisabledColor = "F6F6F6";
            chatErrorColor = "760502";
            chatLabelColor = "6E6C6C";

        }

        private void SetGrayTheme()
        {
            useTheme = true;
            foreColor = "676769";
            backColor = "DBDBDB";
            linkColor = "444444";
            borderColor = "8D8D8D";
            buttonForeColor = "99CC33";
            buttonBackColor = "676769";
            buttonBorderColor = "99CC33";
            buttonDisabledColor = "F1F1F1";
            headerForeColor = "729527";
            headerBackColor = "B2B2B2";
            menuForeColor = "676769";
            menuBackColor = "BBBBBB";
            chatForeColor = "99CC33";
            chatBackColor = "EAEAEA";
            chatDisabledColor = "B2B2B2";
            chatErrorColor = "760502";
            chatLabelColor = "6E6C6C";

        }

    }
}
