// Author:					
// Created:				    2010-06-10
// Last Modified:			2010-06-22
// FeatureGuid:		        59ec610a-afab-4c26-8859-fbb2659ab2cb
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.

using System;
using mojoPortal.Web.UI;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.MapUI
{
    public partial class BingMapModule : SiteModuleControl
    {
        private BingMapConfiguration config = new BingMapConfiguration();

       
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
            bmap.Location = config.Location;
            bmap.LocationAlias = config.Caption;
            bmap.ShowLocationPin = config.ShowLocationPin;
            bmap.Height = config.MapHeight;
            bmap.MapWidth = config.MapWidth;
            bmap.Zoom = config.ZoomSetting;
            bmap.MapStyle = config.MapStyle;
            bmap.ShowMapControls = config.ShowMapControls;
            bmap.ShowGetDirections = config.EnableDrivingDirections;
            divDirections.Visible = config.EnableDrivingDirections;
            bmap.DistanceUnit = config.DistanceUnit;
            if (config.DistanceUnit == "VERouteDistanceUnit.Kilometer")
            {
                bmap.DistanceUnitLabel = BingResources.Kilometers;
            }
            else
            {
                bmap.DistanceUnitLabel = BingResources.Miles;
            }

            bmap.FromTextBoxClientId = txtFromLocation.ClientID;
            bmap.GetDirectionsButtonClientId = btnGetDirections.ClientID;
            bmap.ShowDirectionsPanelClientId = pnlDirections.ClientID;
            bmap.ResetSearchLinkUrl = SiteUtils.GetCurrentPageUrl();

            
        }



        private void PopulateLabels()
        {
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            btnGetDirections.Text = BingResources.GetDirectionsButton;

            bmap.DistanceLabel = BingResources.Distance;
            bmap.ResetSearchLinkLabel = BingResources.Reset;
            
        }

        private void LoadSettings()
        {
            config = new BingMapConfiguration(Settings);

            if (config.InstanceCssClass.Length > 0)
            {
                pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass);
            }
            
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