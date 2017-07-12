using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls
{
    /// <summary>
    ///	Author:				
    ///	Created:			2006-07-30
    ///	Last Modified:		2007-08-30
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    ///  		
    /// </summary>
    public class CollapseLinkButton : WebControl, INamingContainer
    {
        #region Control Declarations

        protected HtmlImage imgToggle;
        protected HtmlAnchor lnkToggle;

        #endregion

        #region Constructors

        public CollapseLinkButton()
		{
			EnsureChildControls();
		}

		#endregion

        #region Public Properties

        public string ControlToCollapse
        {
            get
            {
                string obj = ViewState["ControlToCollapse"] as string;
                return (obj != null) ? obj : string.Empty;
            }
            set
            {
                ViewState["ControlToCollapse"] = value;
            }
        }

        public string ExpandImageUrl
        {
            get
            {
                object obj = ViewState["ExpandImageUrl"];
                return (obj != null) ? (string)obj : string.Empty;
            }
            set
            {
                ViewState["ExpandImageUrl"] = value;
            }
        }

        public string CollapseImageUrl
        {
            get
            {
                object obj = ViewState["CollapseImageUrl"];
                return (obj != null) ? (string)obj : string.Empty;
            }
            set
            {
                ViewState["CollapseImageUrl"] = value;
            }
        }

        public string ExpandText
        {
            get
            {
                string obj = ViewState["ExpandText"] as string;
                return (obj != null) ? obj : string.Empty;
            }
            set
            {
                ViewState["ExpandText"] = value;
            }
        }

        public string CollapseText
        {
            get
            {
                string obj = ViewState["CollapseText"] as string;
                return (obj != null) ? obj : string.Empty;
            }
            set
            {
                ViewState["CollapseText"] = value;
            }
        }

        public bool StartCollapsed
        {
            get { return (ViewState["StartCollapsed"] != null ? (bool)ViewState["StartCollapsed"] : false); }
            set { ViewState["StartCollapsed"] = value; }
        }

        // Alexander's incorrect refactoring broke functionality here
        // I commented it out below and restored the original implementation
        // above
        //public bool StartCollapsed
        //{
        //    get
        //    {
        //        bool obj;
        //        return bool.TryParse(ViewState["StartCollapsed"] as string, out obj) ? obj : false;
        //    }
        //    set
        //    {
        //        ViewState["StartCollapsed"] = value;
        //    }
        //}

        #endregion


        private bool PropertiesValid
        {
            get
            {
                return (NamingContainer.FindControl(ControlToCollapse) != null);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // render to the designer
                //this.lnkToggle.RenderControl(writer);
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                base.Render(writer);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            
            SetupScripts();
            Initialize();
            base.OnPreRender(e);

        }

        private void Initialize()
        {
            if (this.CollapseImageUrl != string.Empty)
            {
                imgToggle.Visible = true;
                if (StartCollapsed)
                {
                    imgToggle.Src = ResolveUrl(this.ExpandImageUrl);
                    imgToggle.Alt = ExpandText;
                }
                else
                {
                    imgToggle.Src = ResolveUrl(this.CollapseImageUrl);
                    imgToggle.Alt = CollapseText;
                }
                
                imgToggle.Border = 0;
                
            }
            else
            {
                if (StartCollapsed)
                {
                    lnkToggle.InnerHtml = ExpandText;
                    lnkToggle.Title = ExpandText;
                }
                else
                {
                    lnkToggle.InnerHtml = CollapseText;
                    lnkToggle.Title = CollapseText;
                }
                lnkToggle.Visible = false;
            }

           
            if (PropertiesValid)
            {
                lnkToggle.Attributes["onclick"] = GetOnClick();
            }
            


        }

        protected override void CreateChildControls()
        {
            imgToggle = new HtmlImage();
            lnkToggle = new HtmlAnchor();
            this.Controls.Add(this.lnkToggle);
            lnkToggle.Controls.Add(this.imgToggle);

        }

        private String GetOnClick()
        {
            return string.Format("toggleCollapse(this,'{0}','{1}','{2}','{3}','{4}','{5}');",
                                 this.imgToggle.ClientID,
                                 NamingContainer.FindControl(ControlToCollapse).ClientID,
                                 this.ResolveUrl(this.CollapseImageUrl),
                                 this.ResolveUrl(this.ExpandImageUrl),
                                 this.CollapseText,
                                 this.ExpandText);
        }


       
        private void SetupScripts()
        {
            string script = @"
<script language=""javascript"" type=""text/javascript"">
<!--
	function toggleCollapse(lnk, img, objToHide, collapseUrl, expandUrl, collapseText, expandText) {
	var el = document.getElementById(objToHide);
    var toggleImage = document.getElementById(img);
	if ( el.style.display != 'none' ) 
    {
        DoCollapse(el, toggleImage,expandUrl,expandText);
	}
	else 
    {
        DoExpand(el,toggleImage,collapseUrl,collapseText);
	}
    
}

function DoCollapse(objToHide, image, expandUrl,expandText)
{
    objToHide.style.display = 'none';
    image.src = expandUrl;
    image.alt = expandText;
}

function DoExpand(objToHide, image, collapseUrl,collapseText)
{
    objToHide.style.display = '';
    image.src = collapseUrl;
    image.alt = collapseText;
}

//-->
</script>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "toggleCollapse", script);

            if (this.StartCollapsed)
            {
                StringBuilder collapseScript = new StringBuilder();
                collapseScript.Append("<script type=\"text/javascript\"> ");
                collapseScript.Append("\n<!-- ");
                collapseScript.Append("\n DoCollapse( ");
                collapseScript.Append("document.getElementById(\""
                    + NamingContainer.FindControl(ControlToCollapse).ClientID
                    + "\"), ");
                collapseScript.Append("document.getElementById(\"" 
                    + this.imgToggle.ClientID + "\"), ");
                collapseScript.Append("\"" 
                    + this.ResolveUrl(this.ExpandImageUrl) 
                    + "\", ");
                collapseScript.Append("\"" + this.ExpandText + "\" ");
                collapseScript.Append("); ");
                collapseScript.Append("\n//--> ");
                collapseScript.Append(" </script>");

                Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "s" + this.ClientID,
                    collapseScript.ToString());

            }


        }

    }
}
