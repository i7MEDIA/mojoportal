//	Author:                 
//  Created:			    2009-09-20
//	Last Modified:		    2011-09-05
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
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using mojoPortal.Business;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using Resources;
using static Lucene.Net.Store.Lock;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// a control for resizing images on the server based on resizing parmaters determined client side with javascript.
    /// uses the jCrop plugin http://deepliquid.com/content/Jcrop.html
    /// </summary>
    public partial class ImageCropper : UserControl
    {
        #region Properties

        private IFileSystem fileSystem = null;

        private string sourceImagePath = string.Empty;
        public string SourceImagePath
        {
            get { return sourceImagePath; }
            set { sourceImagePath = value; }
        }

        private string resultImagePath = string.Empty;
        public string ResultImagePath
        {
            get { return resultImagePath; }
            set { resultImagePath = value; }
        }



        private decimal aspectRatio = 0;
        public decimal AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value; }
        }

        private int minWidth = 0;
        public int MinWidth
        {
            get { return minWidth; }
            set { minWidth = value; }
        }

        private int minHeight = 0;
        public int MinHeight
        {
            get { return minHeight; }
            set { minHeight = value; }
        }

        private int maxWidth = 0;
        public int MaxWidth
        {
            get { return maxWidth; }
            set { maxWidth = value; }
        }

        private int maxHeight = 0;
        public int MaxHeight
        {
            get { return maxHeight; }
            set { maxHeight = value; }
        }

        private int finalMaxWidth = 0;
        public int FinalMaxWidth
        {
            get { return finalMaxWidth; }
            set { finalMaxWidth = value; }
        }

        private int finalMaxHeight = 0;
        public int FinalMaxHeight
        {
            get { return finalMaxHeight; }
            set { finalMaxHeight = value; }
        }

        private string bgColor = "black";
        public string BgColor
        {
            get { return bgColor; }
            set { bgColor = value; }
        }

        private decimal bgOpacity = .6m;
        public decimal BgOpacity
        {
            get { return bgOpacity; }
            set { bgOpacity = value; }
        }

        private int initialSelectionX = 0;
        public int InitialSelectionX
        {
            get { return initialSelectionX; }
            set { initialSelectionX = value; }
        }

        private int initialSelectionY = 0;
        public int InitialSelectionY
        {
            get { return initialSelectionY; }
            set { initialSelectionY = value; }
        }

        private int initialSelectionX2 = 0;
        public int InitialSelectionX2
        {
            get { return initialSelectionX2; }
            set { initialSelectionX2 = value; }
        }

        private int initialSelectionY2 = 0;
        public int InitialSelectionY2
        {
            get { return initialSelectionY2; }
            set { initialSelectionY2 = value; }
        }

        private bool allowUserToChooseCroppedFileName = false;
        public bool AllowUserToChooseCroppedFileName
        {
            get { return allowUserToChooseCroppedFileName; }
            set { allowUserToChooseCroppedFileName = value; }
        }

        private bool allowUserToSetFinalFileSize = false;
        public bool AllowUserToSetFinalFileSize
        {
            get { return allowUserToSetFinalFileSize; }
            set { allowUserToSetFinalFileSize = value; }
        }

        private string ext = string.Empty;
        private bool sourceExists = false;
        private bool targetExists = false;


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            if (!Page.IsPostBack) 
            {
                if (resultImagePath.Length > 0)
                {
                    txtCroppedFileName.Text = Path.GetFileName(resultImagePath);
                }
                PopulateControls();
            }
            
        }

        private void PopulateControls()
        {

            if (!sourceExists)
            {
                pnlCrop.Visible = false;

            }
            else
            {
                if (sourceImagePath.StartsWith("~"))
                {
                    imgToCrop.ImageUrl = Page.ResolveUrl(fileSystem.FileBaseUrl + sourceImagePath.Substring(1));
                }
                else
                {
                    imgToCrop.ImageUrl = Page.ResolveUrl(fileSystem.FileBaseUrl + sourceImagePath);
                }
            }

            if (targetExists)
            {
                pnlCropped.Visible = true;

                if (resultImagePath.StartsWith("~"))
                {
                    imgCropped.ImageUrl = Page.ResolveUrl(fileSystem.FileBaseUrl + resultImagePath.Substring(1) + "?g=" + Guid.NewGuid().ToString()); //prevent caching with a guid
                }
                else
                {
                    imgCropped.ImageUrl = Page.ResolveUrl(fileSystem.FileBaseUrl + resultImagePath + "?g=" + Guid.NewGuid().ToString()); //prevent caching with a guid
                }

            }
            else
            {
                pnlCropped.Visible = false;
            }
        }

        void btnCrop_Click(object sender, EventArgs e)
        {
            
            if (SiteUtils.IsImageFileExtension(ext))
            {
                decimal decx = 0;
                decimal decy = 0;
                decimal decw = 0;
                decimal dech = 0;

                decimal.TryParse(X.Value, out decx);
                decimal.TryParse(Y.Value, out decy);
                decimal.TryParse(W.Value, out decw);
                decimal.TryParse(H.Value, out dech);

                int x = Convert.ToInt32(decx);
                int y = Convert.ToInt32(decy);
                int w = Convert.ToInt32(decw);
                int h = Convert.ToInt32(dech);

				if (allowUserToSetFinalFileSize)
                {
                    int.TryParse(txtFinalWidth.Text, out finalMaxWidth);
                    int.TryParse(txtFinalHeight.Text, out finalMaxHeight);
                }

                if (fileSystem.FileExists(sourceImagePath))
                {
                    if ((allowUserToChooseCroppedFileName)&&(txtCroppedFileName.Text.Length > 0))
                    {
                        string newFileName = (Path.GetFileNameWithoutExtension(txtCroppedFileName.Text) + ext).ToCleanFileName();
                        resultImagePath = resultImagePath.Replace(VirtualPathUtility.GetFileName(resultImagePath), newFileName);
                    }

                    mojoPortal.Web.ImageHelper.CropImage(sourceImagePath, resultImagePath, IOHelper.GetMimeType(ext), w, h, x, y, WebConfigSettings.DefaultResizeBackgroundColor);

                    targetExists = true;
                    initialSelectionX = x;
                    initialSelectionY = y;
                    initialSelectionX2 = x + w;
                    initialSelectionY2 = y + h;

                    if (finalMaxWidth > 0)
                    {
                        if (aspectRatio == 1)
                        {
                            mojoPortal.Web.ImageHelper.ResizeAndSquareImage(resultImagePath, IOHelper.GetMimeType(ext), finalMaxWidth, WebConfigSettings.DefaultResizeBackgroundColor);
                        }
                        else
                        {
                            bool allowEnlargement = true;
                            mojoPortal.Web.ImageHelper.ResizeImage(resultImagePath, IOHelper.GetMimeType(ext), finalMaxWidth, finalMaxHeight, allowEnlargement, WebConfigSettings.DefaultResizeBackgroundColor);
                        }

                    }
                    
                }
            }
            else
            {
                sourceImagePath = string.Empty;
            }

            //WebUtils.SetupRedirect(this, Request.RawUrl);
            
            PopulateControls();
            //updPanel.Update();
            
        }

        private void PopulateLabels()
        {
            btnCrop.Text = Resource.CropImageButton;

        }

        private void LoadSettings()
        {
            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null) { return; }

            fileSystem = p.GetFileSystem();
            if (fileSystem == null) { return; }

            if (sourceImagePath.Length > 0)
            {
                sourceExists = fileSystem.FileExists(sourceImagePath);
                ext = VirtualPathUtility.GetExtension(sourceImagePath);
            }
            if (resultImagePath.Length > 0)
            {
                targetExists = fileSystem.FileExists(resultImagePath);
            }
            

            lblCroppedFileName.Visible = allowUserToChooseCroppedFileName;
            txtCroppedFileName.Visible = allowUserToChooseCroppedFileName;
            pnlFinalSize.Visible = allowUserToSetFinalFileSize;
            //if (WebConfigSettings.ImageCropperWrapperDivStyle.Length > 0)
            //{
            //}
            pnlImage.Attributes.Add("style", "margin: 20px 0; max-width: 180px; ");
            imgToCrop.Attributes.Add("style", "max-width: 100%;");

        }

        private void SetupInstanceScript()
        {
            var script = $@"<script data-loader=""ImageCropper"">
//import Cropper from 'cropperjs';

const image = document.getElementById('{imgToCrop.ClientID}');
const X = document.getElementById('{X.ClientID}');
const Y = document.getElementById('{Y.ClientID}');
const W = document.getElementById('{W.ClientID}');
const H = document.getElementById('{H.ClientID}');

const cropper = new Cropper(image,{{
    aspectRatio: 1,
    viewMode: 3,
    crop(event) {{
//        console.log(event.detail.x);
//        console.log(event.detail.y);
//        console.log(event.detail.width);
//        console.log(event.detail.height);
//        console.log(event.detail.rotate);
//        console.log(event.detail.scaleX);
//        console.log(event.detail.scaleY);
        X.value = event.detail.x;
        Y.value = event.detail.y;
        W.value = event.detail.width;
        H.value = event.detail.height;
    }},
}});
//cropper.setCropBoxData({{
//    height: 90,
//    width: 90
//}});
</script>
";

            //script.Append("function storeCoords" + imgToCrop.ClientID + "(c) {");
            //script.Append("jQuery('#" + X.ClientID + "').val(c.x);");
            //script.Append("jQuery('#" + Y.ClientID + "').val(c.y);");
            //script.Append("jQuery('#" + W.ClientID + "').val(c.w);");
            //script.Append("jQuery('#" + H.ClientID + "').val(c.h);");
            //script.Append("};");

            //script.Append("jQuery(document).ready(function() {");
            //script.Append("jQuery('#" + imgToCrop.ClientID + "').Jcrop({");
            //script.Append("onSelect: storeCoords" + imgToCrop.ClientID);
            ////script.Append(",onChange: storeCoords" + imgToCrop.ClientID);
            //script.Append(" ,bgColor:'" + bgColor + "'");
            //script.Append(",bgOpacity:" + bgOpacity.ToString(CultureInfo.InvariantCulture));
            //if (aspectRatio > 0) { script.Append(",aspectRatio:" + aspectRatio.ToString(CultureInfo.InvariantCulture)); }
            //if ((minWidth > 0) && (minHeight > 0))
            //{
            //    script.Append(",minSize:[" + minWidth.ToInvariantString() + "," + minHeight.ToInvariantString() + "]");
            //}

            //if ((maxWidth > 0) && (maxHeight > 0))
            //{
            //    script.Append(",maxSize:[" + maxWidth.ToInvariantString() + "," + maxHeight.ToInvariantString() + "]");
            //}

            //if ((initialSelectionX > 0) && (initialSelectionY > 0) && (initialSelectionX2 > 0) && (initialSelectionY2 > 0))
            //{
            //    script.Append(",setSelect:[" + initialSelectionX.ToInvariantString()
            //        + "," + initialSelectionY.ToInvariantString()
            //        + "," + initialSelectionX2.ToInvariantString()
            //        + "," + initialSelectionY2.ToInvariantString()
            //        + "]");
            //}

            //script.Append("});");
            //script.Append("});");

            //script.Append("</script>");

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                this.UniqueID,
                script.ToString());
        }

        private void SetupMainScript()
        {
            //we are assuming that jquery is alreadyloaded in the page via script loader

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "cropperjs", $"\n<script src=\"{Page.ResolveUrl("~/ClientScript/cropperjs/cropper.js")}\" data-loader=\"ImageCropper\" ></script>");
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "cropperjs-css", $"\n<link href=\"{Page.ResolveUrl("~/ClientScript/cropperjs/cropper.min.css")}\" rel=\"stylesheet\" data-loader=\"ImageCropper\"/>");
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Visible) { return; }

            SetupMainScript();
            SetupInstanceScript();
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnCrop.Click += new EventHandler(btnCrop_Click);
        }

        
    }
}