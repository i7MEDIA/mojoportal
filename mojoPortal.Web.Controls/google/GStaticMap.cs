using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls.google
{
    /// <summary>
    /// Author:					
    /// Created:				2008-03-23
    /// Last Modified:			2008-03-23
    ///		
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.	
    /// 
    /// implements support for google static map
    /// http://code.google.com/apis/maps/documentation/staticmaps/
    /// 
    /// 
    /// 
    /// </summary>
    public class GStaticMap : Image
    {
        private string gMapApiKey = string.Empty;
        private string gMapWebConfigKey = "GoogleMapsAPIKey";
        private bool apiKeyFound = false;
        private GStaticMapType gmapType = GStaticMapType.mobile;
        private int zoomLevel = 13; //default is 13

        private decimal longitude = -300; //between -180 and +180 precision = 6 decimal places
        private decimal latitude = -300;  //between -90 and +90 precision = 6 decimal places
        private int mapHeight = 300;
        private int mapWidth = 500;


        // GeoCode service example
        //http://maps.google.com/maps/geo?q=1600+Amphitheatre+Parkway,+Mountain+View,+CA&output=xml&key=abcdefg


        /// <summary>
        /// default is 13, lower number zooms out higher number zooms in
        /// </summary>
        public int ZoomLevel
        {
            get { return zoomLevel; }
            set { zoomLevel = value; }
        }


        public GStaticMapType GmapType
        {
            get { return gmapType; }
            set { gmapType = value; }
        }

        /// <summary>
        /// To use google maps you need an API key.
        /// Get it here: http://code.google.com/apis/maps/signup.html
        /// put it in Web.config as <add key="GoogleMapsAPIKey" value="" />
        /// </summary>
        public string GMapApiKey
        {
            get { return gMapApiKey; }
            set { gMapApiKey = value; }
        }

        /// <summary>
        /// You can set this on the control if you need to use a different Web.config key for some reason.
        /// </summary>
        protected string GMapWebConfigKey
        {
            get { return gMapWebConfigKey; }
            set { gMapWebConfigKey = value; }
        }

        /// <summary>
        /// pixel height of rendered map
        /// </summary>
        public int MapHeight
        {
            get { return mapHeight; }
            set { mapHeight = value; }
        }

        /// <summary>
        /// pixel width of rendered map
        /// </summary>
        public int MapWidth
        {
            get { return mapWidth; }
            set { mapWidth = value; }
        }

        /// <summary>
        /// Indicates whether the GMapApiKey was initialized.
        /// Initialization happens in on pre-render
        /// </summary>
        protected bool ApiKeyFound
        {
            get { return apiKeyFound; }

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            LoadSettings();

            if (
                (!apiKeyFound)
                ||(longitude == -300)
                ||(latitude == -300)
                )
            {
                this.Visible = false;
                return;
            }

            this.ImageUrl = "http://maps.google.com/staticmap?"
             + "center=" + latitude.ToString(CultureInfo.InvariantCulture) + ","
             + longitude.ToString(CultureInfo.InvariantCulture) 
             + "&zoom=" + zoomLevel.ToString(CultureInfo.InvariantCulture) 
             + "&size=" + mapWidth.ToString(CultureInfo.InvariantCulture) 
             + "x" + mapHeight.ToString(CultureInfo.InvariantCulture) 
             + "&maptype=" + gmapType.ToString()
             //+ "&markers=40.702147,-74.015794,blues%7C40.711614,-74.012318,greeng%7C40.718217,-73.998284,redc"
             + "&key=" + this.gMapApiKey;

            //http://maps.google.com/staticmap?center=40.714728,-73.998672&zoom=14&size=512x512&maptype=mobile&markers=40.702147,-74.015794,blues%7C40.711614,-74.012318,greeng%7C40.718217,-73.998284,redc&key=ABQIAAAAzr2EBOXUKnm_jVnk0OJI7xSosDVG8KKPE1-m51RBrvYughuyMxQ-i1QfUnH94QxWIa6N4U6MouMmBA

        }

        private void LoadSettings()
        {
            if (gMapApiKey.Length > 0)
            {
                // don't look in Web.config if setting has been already made.
                apiKeyFound = true;
                return;

            }

            if (ConfigurationManager.AppSettings[gMapWebConfigKey] != null)
            {
                gMapApiKey = ConfigurationManager.AppSettings[gMapWebConfigKey];
                if (gMapApiKey.Length > 0) apiKeyFound = true;

            }

        }

    }
}
