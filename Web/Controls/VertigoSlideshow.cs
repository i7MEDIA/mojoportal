// Author:					
// Created:				    2009-09-02
// Last Modified:			2009-11-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// http://www.vertigo.com/slideshow.aspx
    /// </summary>
    public class VertigoSlideshow : WebControl
    {
        #region Private Properties

        private string xapSource = "/ClientBin/Vertigo.SlideShow.xap";
        private string mimeType = "application/x-silverlight-2";

        private string theme = "LightTheme";
        private string backColor = "black";
        private int height = 480;
        private int width = 640;

        private string xmlConfigFileName = string.Empty;
        private string xmlDataUrl = string.Empty;
        private string flickrUserName = string.Empty;
        private string flickrApiKey = string.Empty;
        private bool windowless = false;


        

        #endregion

        #region Public Properties

        public string XmlDataUrl
        {
            get { return xmlDataUrl; }
            set { xmlDataUrl = value; }
        }

        /// <summary>
        /// Should be the name of an xml file located in the /ClientBin folder (the same folder as the .xap)
        /// Must be a valid configuration file for Vertigo slide show
        /// </summary>
        public string XmlConfigFileName
        {
            get { return xmlConfigFileName; }
            set { xmlConfigFileName = value; }
        }

        public string FlickrUserName
        {
            get { return flickrUserName; }
            set { flickrUserName = value; }
        }

        public string FlickrApiKey
        {
            get { return flickrApiKey; }
            set { flickrApiKey = value; }
        }

        public new string BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        public string Theme
        {
            get { return theme; }
            set { theme = value; }
        }

        public new int Height
        {
            get { return height; }
            set { height = value; }
        }

        public new int Width
        {
            get { return width; }
            set { width = value; }
        }

        public bool Windowless
        {
            get { return windowless; }
            set { windowless = value; }
        }


        #endregion



        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                if (!Visible) { return; }

                string themeToUse = "LightTheme";
                switch (theme)
                {
                    case "SimpleTheme":
                        themeToUse = "SimpleTheme";
                        break;

                    case "DarkTheme":
                        themeToUse = "DarkTheme";
                        break;

                    case "LightTheme":
                    default:
                        themeToUse = "LightTheme";
                        break;
                }

                if (xmlConfigFileName.Length == 0)
                {
                    xmlConfigFileName = WebConfigSettings.VertigoSlideShowOverrideXmlConfigFile;
                }

                string siteRoot = WebUtils.GetSiteRoot();

                writer.Write("<object type=\"" + mimeType + "\" ");
                writer.Write("data=\"data:" + mimeType + ",\" ");
                writer.Write("width=\"" + width.ToInvariantString() + "\" ");
                writer.Write("height=\"" + height.ToInvariantString() + "\" ");
                writer.Write(">");

                writer.Write("<param name=\"background\" value=\"" + backColor + "\" />");

                writer.Write("<param name=\"source\" value=\"" + siteRoot + xapSource + "\" />");

                if(windowless)
                {
                    writer. Write ("<param name=\"Windowless\" value=\"true\" />");
                }

                if ((flickrUserName.Length > 0) && (flickrApiKey.Length > 0))
                {
                    writer.Write("<param name=\"initParams\" ");
                    if (xmlConfigFileName.Length > 0)
                    {
                        writer.Write("value=\"ConfigurationProvider=XmlConfigurationProvider;Path=" + xmlConfigFileName);
                    }
                    else
                    {
                        writer.Write("value=\"ConfigurationProvider=" + themeToUse);
                    }

                    writer.Write(",DataProvider=FlickrDataProvider;");
                    writer.Write("UserName=" + flickrUserName + ";ApiKey=" + flickrApiKey + ";A=B\" />");
                }
                else if(xmlDataUrl.Length > 0)
                {
                    writer.Write("<param name=\"initParams\" ");
                    if (xmlConfigFileName.Length > 0)
                    {
                        writer.Write("value=\"ConfigurationProvider=XmlConfigurationProvider;Path=" + xmlConfigFileName);
                    }
                    else
                    {
                        writer.Write("value=\"ConfigurationProvider=" + themeToUse);
                    }

                    writer.Write(",DataProvider=XmlDataProvider;");
                    writer.Write("Path=" + xmlDataUrl + ";A=B\"/>");
                }

                //<param name="initParams" value="ConfigurationProvider=LightTheme,DataProvider=XmlDataProvider;Path=Data.xml"/>
                writer.Write("<p>This content requires Microsoft Silverlight. ");
                writer.Write("<a href=\"http://go.microsoft.com/fwlink/?LinkId=149156\" style=\"text-decoration: none;\"></p><p>");  
                writer.Write("<img src=\"http://go.microsoft.com/fwlink/?LinkId=108181\" alt=\"Get Microsoft Silverlight\" style=\"border-style: none\"/></a> </p>");

               

                writer.Write("</object>");

            }
        }

    }
}
