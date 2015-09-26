using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;



namespace mojoPortal.Web.Controls
{
    /// <summary>
    /// Added 2007/04/26
    /// Adapted from Code Project Article
    /// http://www.codeproject.com/aspnet/swfobject_server_control.asp
    /// Last Modified:  2007/05/01
    /// 
    /// </summary>
    [ParseChildren(ChildrenAsProperties = false)]
    public class FlashMovie : Control
    {

        string containerID = null;
        string movie = string.Empty;
        string width = string.Empty;
        string height = string.Empty;
        string flashVersion = "";

        List<FlashParameter> parameters = new List<FlashParameter>();
        List<FlashVariable> variables = new List<FlashVariable>();

        public FlashMovie() 
        { }

        public string ContainerID 
        { 
            get { return containerID; } 
            set { containerID = value; } 
        }

        public string Movie 
        { 
            get { return movie; } 
            set { movie = value; } 
        }

        public string Width 
        { 
            get { return width; } 
            set { width = value; } 
        }

        public string Height 
        { 
            get { return height; } 
            set { height = value; } 
        }

        public string FlashVersion 
        { 
            get { return flashVersion; } 
            set { flashVersion = value; } 
        }
        public List<FlashParameter> Parameters 
        { 
            get { return parameters; } 
            set { parameters = value; } 
        }

        public List<FlashVariable> Variables 
        { 
            get { return variables; } 
            set { variables = value; } 
        }

        [Bindable(true), Category("Behavior"), DefaultValue("~/ClientScript")]
        public string ScriptDirectory
        {
            get { return (ViewState["ScriptDirectory"] != null ? (string)ViewState["ScriptDirectory"] : "~/ClientScript"); }
            set { ViewState["ScriptDirectory"] = value; }
        }

        protected override void AddParsedSubObject(object obj)
        {

            if (obj is FlashParameter) parameters.Add((FlashParameter)obj);
            if (obj is FlashVariable) variables.Add((FlashVariable)obj);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupScripts();
        }

        private void SetupScripts()
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "swfobjectscript", "<script type=\"text/javascript\" src=\""
                + ResolveUrl(this.ScriptDirectory + "/swfobject.js") + "\"></script>");

        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {

                base.DataBind();

                string id = (containerID == null) ? this.Parent.ClientID : containerID;

                string jsVar = id + "_SWFObject";

                writer.WriteLine(String.Format("<!-- {0} with SWFObject START -->", movie));

                writer.WriteBeginTag("script");
                writer.WriteAttribute("type", "text/javascript");
                writer.WriteLine(HtmlTextWriter.TagRightChar);

                writer.WriteLine(String.Format("var {0} = new SWFObject('{1}', '', '{2}', '{3}', '{4}', '');", new object[] { jsVar, this.Page.ResolveUrl(movie), width, height, flashVersion }));

                writer.WriteLine(String.Format("with ({0}) {{", jsVar));

                foreach (FlashParameter p in parameters)
                {
                    p.DataBind();
                    writer.WriteLine(String.Format("addParam('{0}', '{1}');", p.Name, p.Value));
                }

                foreach (FlashVariable v in variables)
                {
                    v.DataBind();
                    writer.WriteLine(String.Format("addVariable('{0}', '{1}');", v.Name, v.Value));
                }

                writer.WriteLine(String.Format("write('{0}');", id));
                writer.WriteLine("}");
                writer.WriteEndTag("script");
                writer.WriteLine();

                writer.Write(String.Format("<!-- {0} with SWFObject END -->", movie));
            }
        }
    }


    public class FlashParameter : FlashInput { }


    public class FlashVariable : FlashInput { }


    public abstract class FlashInput : Control
    {
        string name = "";
        string _value = "";

        public string Name 
        { 
            get { return name; } 
            set { name = value; } 
        }

        public string Value 
        { 
            get { return _value; } 
            set { _value = value; } 
        }
    }

}