// Author:					
// Created:				2007-12-04
// Last Modified:			2009-01-22
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
    /// <summary>
    /// This a control for displaying a google map based on the location set in the location property.
    /// This is more simple than using latitude longitude, lat and long are derived automatically if possible
    /// from the location. Alternatively you can set the coordinates.
    /// 
    /// TODO: implement more of the optional features like overlays etc
    /// </summary>
    public class LocationMap : GMapBasePanel
    {

        #region Private Properties

        private float latitude = 0;
        private float longitude = 0;
        private bool useLatLong = false;
        private string location = string.Empty;

        private int mapHeight = 300;
        private int mapWidth = 500;

        private bool enableMapType = false;
        private bool enableZoom = false;
        private bool showInfoWindow = false;
        private bool enableLocalSearch = false;
        private bool enableDrivingDirections = false;
        private string directionsButtonText = "Get Directions From";
        private string directionsButtonToolTip = "Get Directions From";

        #endregion

        #region Public Properties

        /// <summary>
        /// gets or sets the text on the button used for requesting directions
        /// </summary>
        public string DirectionsButtonText
        {
            get { return directionsButtonText; }
            set { directionsButtonText = value; }
        }

        /// <summary>
        /// gets or sets the tooltip text on the button used for requesting directions
        /// </summary>
        public string DirectionsButtonToolTip
        {
            get { return directionsButtonToolTip; }
            set { directionsButtonToolTip = value; }
        }

        /// <summary>
        /// if true the options for satelite and hybrid maps are vailable
        /// </summary>
        public bool EnableMapType
        {
            get { return enableMapType; }
            set { enableMapType = value; }
        }

        /// <summary>
        /// if true uses the ma control with zoom tool
        /// </summary>
        public bool EnableZoom
        {
            get { return enableZoom; }
            set { enableZoom = value; }
        }

        /// <summary>
        /// if true shows a ballon with the location info
        /// </summary>
        public bool ShowInfoWindow
        {
            get { return showInfoWindow; }
            set { showInfoWindow = value; }
        }

        /// <summary>
        /// if true shows a search input and searches google for things close to the location
        /// </summary>
        public bool EnableLocalSearch
        {
            get { return enableLocalSearch; }
            set { enableLocalSearch = value; }
        }

        /// <summary>
        /// if true shows an input and a button for entering a start address and requesting directions.
        /// </summary>
        public bool EnableDrivingDirections
        {
            get { return enableDrivingDirections; }
            set { enableDrivingDirections = value; }
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
        /// If set to true the map will be centered on the Latitude/Longitude coordinates.
        /// </summary>
        public bool UseLatLong
        {
            get { return useLatLong; }
            set { useLatLong = value; }
        }

        /// <summary>
        /// If UseLatLong is true, the map will be centered using this Latitude.
        /// </summary>
        public float Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        /// <summary>
        /// If UseLatLong is true, the map will be centered using this Longitude.
        /// </summary>
        public float Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        /// <summary>
        /// If UseLatLong is false and this string has a value of a location that can be looked up, the map will display the found location.
        /// </summary>
        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        #endregion


        #region Controls

        private Panel pnlMapCanvas;

        private Panel pnlDirections;
        private TextBox txtStartLocation;
        private Button btnGetDirections;


        #endregion

        /// <summary>
        /// Sets up the javscript for this map instance to be created.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            pnlMapCanvas = new Panel();
            pnlMapCanvas.CssClass = "gmap";
            pnlMapCanvas.ID = "pnlMapCanvas";

            this.Controls.Add(pnlMapCanvas);

            if (enableDrivingDirections)
            {
                btnGetDirections = new Button();
                btnGetDirections.ID = "btnGetDirections";
                btnGetDirections.Text = directionsButtonText;
                btnGetDirections.ToolTip = directionsButtonToolTip;
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

                btnGetDirections.Attributes.Add("onclick", "showMapAndDirections('" + pnlMapCanvas.ClientID
                    + "','" + pnlDirections.ClientID + "','" + txtStartLocation.ClientID + "','"
                    + location + "','" + CultureInfo.CurrentUICulture.TwoLetterISOLanguageName + "');return false;");

                this.DefaultButton = btnGetDirections.ID;

            }


            this.pnlMapCanvas.Attributes.Add("style", "width:" 
                + mapWidth.ToString(CultureInfo.InvariantCulture) 
                + "px;height:" + mapHeight.ToString(CultureInfo.InvariantCulture) + "px;");

            SetupCss();
            SetupScript();

        }

        private void SetupScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            //script.Append("\n<!-- \n");

            script.Append("\n  var gm" + this.pnlMapCanvas.ClientID + " = document.getElementById('" + this.pnlMapCanvas.ClientID + "'); ");
            script.Append("if(gm" + this.pnlMapCanvas.ClientID + "){");

            script.Append("showGMap(gm" + this.pnlMapCanvas.ClientID + ", '" + location + "'");

            //script.Append("enableMapType, useMapControl, showInfoWindow, enableLocalSearch ");
            if (enableMapType)
            {
                script.Append(",true");
            }
            else
            {
                script.Append(",false");
            }

            if (enableZoom)
            {
                script.Append(",true");
            }
            else
            {
                script.Append(",false");
            }

            if (showInfoWindow)
            {
                script.Append(",true");
            }
            else
            {
                script.Append(",false");
            }

            if (enableLocalSearch)
            {
                SetupSearchScript();
                script.Append(",true");
            }
            else
            {
                script.Append(",false");
            }

            script.Append("," + this.GmapType.ToString() + "");

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
            string protocol = "http";
            if (Page.Request.IsSecureConnection) { protocol = "https"; }

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
            if (Page.Header == null) { return; }
            
            string protocol = "http";
            if (Page.Request.IsSecureConnection) { protocol = "https"; }

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


    }
}
