// Author:					
// Created:				    2009-06-27
// Last Modified:			2009-06-27
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// The only purpose of this panel is to easily have a wrapper panel that will automatically have a CSS class
    /// based on the ModuleId so that it is possible to easily make a specific module instance have a different style
    /// </summary>
    public class ModulePanel : Panel
    {

        private int moduleId = -1;

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        private bool renderModulePanel = false;

        public bool RenderModulePanel
        {
            get { return renderModulePanel; }
            set { renderModulePanel = value; }

        }

        protected override void OnPreRender(EventArgs e)
        {
            if (moduleId > -1)
            {
                if (CssClass.Length == 0)
                {
                    CssClass = "module" + moduleId.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    CssClass += " module" + moduleId.ToString(CultureInfo.InvariantCulture);
                }
            }

            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }
            
            if ((renderModulePanel)||(WebConfigSettings.RenderModulePanel))
            {
                base.Render(writer);
            }
            else
            {
                base.RenderContents(writer);
            }
            
        }


    }
}
