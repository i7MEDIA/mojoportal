// Author:					
// Created:				2007-12-04
// Last Modified:			2007-12-05
//		
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls.google
{
   
    public class GMapBasePanel : Panel
    {
        private string googleJsBasePath = "~/ClientScript/google/";
        private string gMapApiKey = string.Empty;
        private string gMapWebConfigKey = "GoogleMapsAPIKey";
        private bool apiKeyFound = false;
        private MapType gmapType = MapType.G_NORMAL_MAP;
        private int zoomLevel = 13; //default is 13
        private string noApiKeyWarning = "Could not find Google Maps API Key in Site Settings or Web.config";

        private string protocol = "http";


        //const string MapTypeNormal = "G_NORMAL_MAP";
        //const string MapTypeSatellite = "G_SATELLITE_MAP";
        //const string MapTypeHybrid = "G_HYBRID_MAP";
        //const string MapTypeTerrain = "G_PHYSICAL_MAP";

        public string Protocol
        {
            get { return protocol; }
            set { protocol = value; }
        }

        /// <summary>
        /// message to display if the api key is not found
        /// </summary>
        public string NoApiKeyWarning
        {
            get { return noApiKeyWarning; }
            set { noApiKeyWarning = value; }
        }

        /// <summary>
        /// default is 13, lower number zooms out higher number zooms in
        /// </summary>
        public int ZoomLevel
        {
            get { return zoomLevel; }
            set { zoomLevel = value; }
        }
        

        public MapType GmapType
        {
            get { return gmapType; }
            set { gmapType = value; }
        }

        /// <summary>
        /// This should be the path to the folder containing mojoPortal custom javascript for working with google APIs
        /// i.e ~/ClientScript/google/
        /// This is NOT the path to the google server.
        /// </summary>
        public string GoogleJsBasePath
        {
            get { return googleJsBasePath; }
            set { googleJsBasePath = value; }
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
        /// Indicates whether the GMapApiKey was initialized.
        /// Initialization happens in on pre-render
        /// </summary>
        protected bool ApiKeyFound
        {
            get { return apiKeyFound; }
            
        }

        /// <summary>
        /// Hooks up the main GMap script.
        /// SubClasses should override but call the base method before doing anything else.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            LoadSettings();
            if (apiKeyFound)
            {
                IncludeGMapScript();
            }
            else
            {
                Literal litWarning = new Literal();
                litWarning.Text = noApiKeyWarning;
                this.Controls.Add(litWarning);
            }

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
        


        private void IncludeGMapScript()
        {
            Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "gmap", "\n<script src=\""
                + protocol + "://maps.google.com/maps?file=api&amp;v=2&amp;key="
                + gMapApiKey
                + "\" type=\"text/javascript\"></script>");

            Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "mojogmaputils", "\n<script src=\""
                + Page.ResolveUrl(googleJsBasePath + "mojogmaputils.js") + "\" type=\"text/javascript\"></script>");

        }

       
    }

}
