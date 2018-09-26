/// Author:					
/// Created:				2007-12-05
/// Last Modified:			2010-06-11
/// ApplicationGuid:		5219a554-d176-481e-abc0-9ea2ee4c8efc
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.

using System;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.MapUI
{
    
    public partial class GoogleMapModule : SiteModuleControl
    {
        private GoogleMapConfiguration config = new GoogleMapConfiguration();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            gmap.Location = config.Location;

            if (config.GoogleApiKey.Length > 0)
            {
                gmap.GMapApiKey = config.GoogleApiKey;
            }
            else
            {
                gmap.GMapApiKey = SiteUtils.GetGmapApiKey();
            }

            gmap.EnableMapType = config.EnableMapType;
            gmap.EnableZoom = config.EnableZoom;
            gmap.ShowInfoWindow = config.ShowInfoWindow;
            gmap.EnableLocalSearch = config.EnableLocalSearch;
            gmap.MapHeight = config.MapHeight;
            gmap.MapWidth = config.MapWidth;
			gmap.UseIframe = config.UseIframe;
			gmap.MapRatio = config.MapRatio;
            gmap.EnableDrivingDirections = config.EnableDrivingDirections;
            gmap.GmapType = config.GoogleMapType;
            gmap.ZoomLevel = config.ZoomSetting;
            
            if (config.Caption.Length > 0) { litCaption.Text = config.Caption; }
  
            if (config.UseLocationAsCaption) { litCaption.Text = Server.HtmlEncode(gmap.Location); }

        }


        private void PopulateLabels()
        {
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                if (config.UseLocationAsTitle)
                {
                    this.ModuleConfiguration.ModuleTitle = config.Location;
                }

                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;

                

                
            }

            gmap.DirectionsButtonText = GMapResources.GoogleMapGetDirectionsFromButton;
            gmap.DirectionsButtonToolTip = GMapResources.GoogleMapGetDirectionsFromButtonToolTip;
            gmap.NoApiKeyWarning = GMapResources.NoApiKeyWarning;
        }

        private void LoadSettings()
        {
            config = new GoogleMapConfiguration(Settings);

            if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

           
        }

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

        }

        #endregion


    }
}