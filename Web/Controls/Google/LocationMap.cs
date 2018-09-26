// Author:					
// Created:				    2007-12-04
// Last Modified:			2018-09-26
//		
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	
// 2011-09-15 updates by Jamie Eubanks to support the newer v3 api which requires no api key and supports ssl
// the previous implementation was also retained because the newer one doesn't support local search
// so if local search is enabled and an api key is present and the request is not over ssl it will use the older api

using mojoPortal.Web.Controls.google;
using mojoPortal.Web.Framework;
using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	/// <summary>
	/// This a control for displaying a google map based on the location set in the location property.
	/// This is more simple than using latitude longitude, lat and long are derived automatically if possible
	/// from the location. Alternatively you can set the coordinates.
	/// 
	/// TODO: implement more of the optional features like overlays etc
	/// </summary>
	public class LocationMap : Panel
    {
		private string protocol = "http";

		#region Public Properties

		//public string Protocol
		//{
		//    get { return protocol; }
		//    set { protocol = value; }
		//}

		/// <summary>
		/// message to display if the api key is not found
		/// </summary>
		public string NoApiKeyWarning { get; set; } = "Could not find Google Maps API Key in Site Settings or Web.config";

		/// <summary>
		/// default is 13, lower number zooms out higher number zooms in
		/// </summary>
		public int ZoomLevel { get; set; } = 13;


		public MapType GmapType { get; set; } = MapType.G_NORMAL_MAP;

		/// <summary>
		/// This should be the path to the folder containing mojoPortal custom javascript for working with google APIs
		/// i.e ~/ClientScript/google/
		/// This is NOT the path to the google server.
		/// </summary>
		public string GoogleJsBasePath { get; set; } = "~/ClientScript/google/";

		/// <summary>
		/// To use google maps you need an API key.
		/// Get it here: http://code.google.com/apis/maps/signup.html
		/// put it in Web.config as <add key="GoogleMapsAPIKey" value="" />
		/// </summary>
		public string GMapApiKey { get; set; } = string.Empty;

		/// <summary>
		/// You can set this on the control if you need to use a different Web.config key for some reason.
		/// </summary>
		protected string GMapWebConfigKey { get; set; } = "GoogleMapsAPIKey";

		/// <summary>
		/// Indicates whether the GMapApiKey was initialized.
		/// Initialization happens in on pre-render
		/// </summary>
		protected bool ApiKeyFound { get; private set; } = false;

		/// <summary>
		/// gets or sets the text on the button used for requesting directions
		/// </summary>
		public string DirectionsButtonText { get; set; } = "Get Directions From";

		/// <summary>
		/// gets or sets the tooltip text on the button used for requesting directions
		/// </summary>
		public string DirectionsButtonToolTip { get; set; } = "Get Directions From";

		/// <summary>
		/// if true the options for satelite and hybrid maps are vailable
		/// </summary>
		public bool EnableMapType { get; set; } = false;

		/// <summary>
		/// if true uses the ma control with zoom tool
		/// </summary>
		public bool EnableZoom { get; set; } = false;

		/// <summary>
		/// if true shows a ballon with the location info
		/// </summary>
		public bool ShowInfoWindow { get; set; } = false;

		/// <summary>
		/// if true shows a search input and searches google for things close to the location
		/// </summary>
		public bool EnableLocalSearch { get; set; } = false;

		/// <summary>
		/// if true shows an input and a button for entering a start address and requesting directions.
		/// </summary>
		public bool EnableDrivingDirections { get; set; } = false;

		/// <summary>
		/// pixel height of rendered map
		/// </summary>
		public int MapHeight { get; set; } = 300;

		/// <summary>
		/// pixel width of rendered map
		/// </summary>
		public string MapWidth { get; set; } = "500px";

		public MapRatio MapRatio { get; set; } = MapRatio.r16by9;

		/// <summary>
		/// If set to true the map will be centered on the Latitude/Longitude coordinates.
		/// </summary>
		public bool UseLatLong { get; set; } = false;

		/// <summary>
		/// If UseLatLong is true, the map will be centered using this Latitude.
		/// </summary>
		public float Latitude { get; set; } = 0;

		/// <summary>
		/// If UseLatLong is true, the map will be centered using this Longitude.
		/// </summary>
		public float Longitude { get; set; } = 0;

		/// <summary>
		/// If UseLatLong is false and this string has a value of a location that can be looked up, the map will display the found location.
		/// </summary>
		public string Location { get; set; } = string.Empty;

		public bool ForceOldApi { get; set; } = false;

		private bool useOldApi = false;

        private string mapUtilsFile = "mojogmaputilsv3-v3-min.js";
        public string MapUtilsFile
        {
            get { return mapUtilsFile; }
            set { mapUtilsFile = value; }
        }

		private bool useIframe = true;
		public bool UseIframe
		{
			get => useIframe;
			set => useIframe = value;
		}

        #endregion


        #region Controls

        private Panel pnlMapCanvas;

        private Panel pnlDirections;
        private TextBox txtStartLocation;
        private mojoButton btnGetDirections;


        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (SiteUtils.IsSecureRequest()) { protocol = "https"; }
        }

        /// <summary>
        /// Sets up the javscript for this map instance to be created.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            LoadSettings();

			if (useIframe)
			{
				pnlMapCanvas = new BasePanel
				{
					CssClass = $"mp-responsive-maps mp-responsive-maps-{MapRatio}",
					RenderId = false
				};

				var iframe = new HtmlIframe
				{
					ID = "iframeMap",
					Src = $@"https://maps.google.com/maps?
						q={Uri.EscapeUriString(Location)}&amp;
						t={GmapType.ToString()}&amp;
						z={ZoomLevel.ToString()}&amp;
						output=embed"
				};

				iframe.Attributes.Add("frameborder", "0");
				iframe.Attributes.Add("marginheight", "0");
				iframe.Attributes.Add("marginwidth", "0");
				iframe.Attributes.Add("allowfullscreen", "allowfullscreen");
				iframe.Attributes.Add("scrolling", "no");


				var mapStyle = new HtmlGenericControl("style")
				{
					InnerHtml = @"
					.mp-responsive-maps {
						display: block;
						height: 0;
						overflow: hidden;
						padding: 0;
						position: relative;
					}

					.mp-responsive-maps > iframe {
						border: 0;
						bottom: 0;
						height: 100%;
						left: 0;
						position: absolute;
						top: 0;
						width: 100%;
					}

					.mp-responsive-maps-r16by9 {
						padding-bottom: 56.25%;
					}

					.mp-responsive-maps-r4by3 {
						padding-bottom: 75%;
					}".Replace(" ", string.Empty).Replace("\t", string.Empty).RemoveLineBreaks()
				};
				pnlMapCanvas.Controls.Add(iframe);
				this.Controls.Add(mapStyle);
				this.Controls.Add(pnlMapCanvas);
			}
			else
			{
				IncludeGMapScript();


				pnlMapCanvas = new Panel
				{
					CssClass = "gmap",
					ID = "pnlMapCanvas"
				};

				this.Controls.Add(pnlMapCanvas);

				if (EnableDrivingDirections)
				{
					btnGetDirections = new mojoButton();
					btnGetDirections.ID = "btnGetDirections";
					btnGetDirections.Text = DirectionsButtonText;
					btnGetDirections.ToolTip = DirectionsButtonToolTip;
					btnGetDirections.CssClass = "button gmapbutton";


					this.Controls.Add(btnGetDirections);

					Literal space = new Literal();
					space.Text = "&nbsp;";
					this.Controls.Add(space);

					txtStartLocation = new TextBox();
					txtStartLocation.ID = "txtStartLocation";
					txtStartLocation.CssClass = "gmaptextbox";
					this.Controls.Add(txtStartLocation);

					pnlDirections = new Panel();
					pnlDirections.ID = "pnlDirections";
					pnlDirections.CssClass = "gdirections";
					this.Controls.Add(pnlDirections);

					if (useOldApi)
					{
						btnGetDirections.Attributes.Add("onclick", "showMapAndDirections('" + pnlMapCanvas.ClientID
							+ "','" + pnlDirections.ClientID + "','" + txtStartLocation.ClientID + "','"
							+ Location + "','" + CultureInfo.CurrentUICulture.TwoLetterISOLanguageName + "');return false;");
					}
					else
					{
						string attribscr;
						attribscr = "showMapAndDirections('" + pnlMapCanvas.ClientID
							+ "','" + pnlDirections.ClientID + "','" + txtStartLocation.ClientID + "','"
							+ Location + "'";
						if (EnableZoom)
						{
							attribscr += ",true";
						}
						else
						{
							attribscr += ",false";
						}
						if (EnableMapType)
						{
							attribscr += ",true";
						}
						else
						{
							attribscr += ",false";
						}
						attribscr += ", '" + this.GmapType.ToString() + "'";

						attribscr += ");return false;";
						btnGetDirections.Attributes.Add("onclick", attribscr);

					}

					this.DefaultButton = btnGetDirections.ID;

				}

				if (
					(!MapWidth.EndsWith("px"))
					&& (!MapWidth.EndsWith("%"))
				)
				{
					MapWidth += "px";
				}

				if (SiteUtils.UseMobileSkin())
				{
					this.pnlMapCanvas.Attributes.Add("style", "width:98%;"
					+ "height:" + MapHeight.ToInvariantString() + "px;");
				}
				else
				{
					this.pnlMapCanvas.Attributes.Add("style", "width:"
						+ MapWidth
						+ ";height:" + MapHeight.ToInvariantString() + "px;");
				}

				SetupCss();
				SetupScript();
			}

        }

        private void SetupScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            //script.Append("\n<!-- \n");

            script.Append("\n  var gm" + this.pnlMapCanvas.ClientID + " = document.getElementById('" + this.pnlMapCanvas.ClientID + "'); ");
            script.Append("if(gm" + this.pnlMapCanvas.ClientID + "){");

            script.Append("showGMap(gm" + this.pnlMapCanvas.ClientID + ", '" + Location + "'");

            //script.Append("enableMapType, useMapControl, showInfoWindow, enableLocalSearch ");
            if (EnableMapType)
            {
                script.Append(",true");
            }
            else
            {
                script.Append(",false");
            }

            if (EnableZoom)
            {
                script.Append(",true");
            }
            else
            {
                script.Append(",false");
            }

            if (ShowInfoWindow)
            {
                script.Append(",true");
            }
            else
            {
                script.Append(",false");
            }

            if (EnableLocalSearch)
            {
                SetupSearchScript();
                script.Append(",true");
            }
            else
            {
                script.Append(",false");
            }

            if (useOldApi)
            {
                script.Append("," + this.GmapType.ToString() + "");
            }
            else
            {
                /* JGE Pass map type parameter as a string, to be translated in JS */
                script.Append(", '" + this.GmapType.ToString() + "'");
            }

            script.Append("," + this.ZoomLevel.ToString() + "");


            //script.Append(" ");

            script.Append(");");

            script.Append("}");

            //script.Append("\n//--> ");
            script.Append(" </script>");


            Page.ClientScript.RegisterStartupScript(
                typeof(Page),
                "setup" + this.ClientID,
                script.ToString());

        }

        private void SetupSearchScript()
        {
            

            Page.ClientScript.RegisterClientScriptBlock(
                typeof(GMapBasePanel),
                "guds", "\n<script type=\"text/javascript\" src=\""
                + protocol + "://www.google.com/uds/api?file=uds.js&amp;v=1.0"
                + "\" ></script>");

            Page.ClientScript.RegisterClientScriptBlock(
                typeof(GMapBasePanel),
                "gudslocal", "\n<script type=\"text/javascript\" src=\""
                + protocol + "://www.google.com/uds/solutions/localsearch/gmlocalsearch.js"
                + "\" ></script>");

        }

        private void SetupCss()
        {
            if (!EnableLocalSearch) { return; }
            if (!useOldApi) { return; }

            if (Page.Header == null) { return; }
            
           
            if (Page.Header.FindControl("gsearchcss") == null)
            {
                Literal cssLink = new Literal();
                cssLink.ID = "gsearchcss";
                cssLink.Text = "\n<link href='"
                + protocol
                + "://www.google.com/uds/css/gsearch.css' type='text/css' rel='stylesheet' media='screen' />";

                Page.Header.Controls.Add(cssLink);
            }

            if (Page.Header.FindControl("gmlocalsearchcss") == null)
            {
                Literal cssLink2 = new Literal();
                cssLink2.ID = "gmlocalsearchcss";
                cssLink2.Text = "\n<link href='"
                + protocol
                + "://www.google.com/uds/solutions/localsearch/gmlocalsearch.css' type='text/css' rel='stylesheet' media='screen' />";

                Page.Header.Controls.Add(cssLink2);
            }

        }


        private void LoadSettings()
        {
            

            if (GMapApiKey.Length > 0)
            {
                // don't look in Web.config if setting has been already made.
                ApiKeyFound = true;
                //return;

            }
            else if (ConfigurationManager.AppSettings[GMapWebConfigKey] != null)
            {
                GMapApiKey = ConfigurationManager.AppSettings[GMapWebConfigKey];
                if (GMapApiKey.Length > 0) ApiKeyFound = true;

            }

            if((EnableLocalSearch) && (ApiKeyFound) && (protocol == "http"))
            {
                useOldApi = true;
            }

        }


        


        private void IncludeGMapScript()
        {
            if (useOldApi)
            {
                Page.ClientScript.RegisterClientScriptBlock(
                    typeof(Page),
                    "gmap", "\n<script src=\""
                    + protocol + "://maps.google.com/maps?file=api&amp;v=2&amp;key="
                    + GMapApiKey
                    + "\" type=\"text/javascript\"></script>");

                Page.ClientScript.RegisterClientScriptBlock(
                    typeof(Page),
                    "mojogmaputils", "\n<script src=\""
                    + Page.ResolveUrl(GoogleJsBasePath + "mojogmaputils.js") + "\" type=\"text/javascript\"></script>");
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "gmap", "\n<script src=\""
                + protocol
                + "://maps.googleapis.com/maps/api/js?v=3&amp;key=" + GMapApiKey
                + "\" type=\"text/javascript\"></script>");

                Page.ClientScript.RegisterClientScriptBlock(
                    typeof(Page),
                    "mojogmaputils", "\n<script src=\""
                    + Page.ResolveUrl(GoogleJsBasePath + mapUtilsFile) + "\" type=\"text/javascript\"></script>");
            }

        }

    }
	public enum MapRatio
	{
		r16by9,
		r4by3

	}
}
