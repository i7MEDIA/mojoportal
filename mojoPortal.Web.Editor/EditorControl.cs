// Author:		        
// Created:            2007-05-25
// Last Modified:      2011-02-26
// 
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;


namespace mojoPortal.Web.Editor
{
    
    public class EditorControl : Panel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EditorControl));
        private EditorProvider editorProvider = null;
        private IWebEditor editor;
        private string providerName = "CKEditorProvider";
        private string failsafeProviderName = "CKEditorProvider";
        private string providerNameFromViewState = string.Empty;
        private string scriptBaseUrl = "~/ClientScript";

        public IWebEditor WebEditor
        { 
            get { return editor; } 
        }

        public string Text
        {
            get { return editor.Text; }
            set { editor.Text = value; }
        }

        public string ScriptBaseUrl
        {
            get { return scriptBaseUrl; }
            set { scriptBaseUrl = value; }
        }

      
        /// <summary>
        /// This should be set in Page PreInit event
        /// </summary>
        public string ProviderName
        {
            get { return providerName; }
            set 
            {
                if (HttpContext.Current == null) { return; }
                if (this.Site != null && this.Site.DesignMode)
                {
                    
                }
                else
                {
                    if (
                    (value != providerName)
                    ||(editorProvider == null)
                    )
                    {
                        providerName = value;
                        SetupProvider();
                     
                    }
                    
                }
                
            }
        }

        public EditorProvider Provider
        {
            get { return editorProvider; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (HttpContext.Current == null) { return; }

            base.OnInit(e);

            DoInit();  
               
            
        }

        private void DoInit()
        {
#if NET35
            if (WebConfigSettings.DisablePageViewStateByDefault) {Page.EnableViewState = true; }
#endif
            Page.RegisterRequiresControlState(this);
            // an exception always happens here in design mode
            // this try is just to fix the display in design view in VS
            if (editorProvider == null)
            {
                SetupProvider();
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (providerName != editorProvider.Name)
            {
                SetupProvider();
            }
        }

        

        private void SetupProvider()
        {
            
            try
            {
                if (EditorManager.Providers[providerName] != null)
                {
                    editorProvider = EditorManager.Providers[providerName];
                }
                else
                {
                    editorProvider = EditorManager.Providers[failsafeProviderName];
                }
                editor = editorProvider.GetEditor();
                editor.ControlID = this.ID + "innerEditor";

                editor.SiteRoot = Page.ResolveUrl("~/");
                editor.ScriptBaseUrl = Page.ResolveUrl(scriptBaseUrl);
                

                this.Controls.Clear();
                this.Controls.Add(editor.GetEditorControl());
            }
            catch (TypeInitializationException ex)
            {
                log.Error(ex);
            }


        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
                return;
            }
            
            base.Render(writer);
            
        }

        
    }
}
