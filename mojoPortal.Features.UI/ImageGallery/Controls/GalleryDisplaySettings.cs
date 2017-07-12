// Author:					
// Created:				    2011-06-09
// Last Modified:			2011-06-09
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.GalleryUI
{
    /// <summary>
    /// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
    public class GalleryDisplaySettings : WebControl
    {
        private string nivoTheme = "theme-default";

        public string NivoTheme
        {
            get { return nivoTheme; }
            set { nivoTheme = value; }
        }

        private string nivoConfig = string.Empty; //empty means it will use all the defaults

        public string NivoConfig
        {
            get { return nivoConfig; }
            set { nivoConfig = value; }
        }

        private bool nivoLinkToFullSize = true;

        public bool NivoLinkToFullSize
        {
            get { return nivoLinkToFullSize; }
            set { nivoLinkToFullSize = value; }
        }

        private bool nivoUseFullSizeImages = false;

        public bool NivoUseFullSizeImages
        {
            get { return nivoUseFullSizeImages; }
            set { nivoUseFullSizeImages = value; }
        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            // nothing to render
        }
    }
}