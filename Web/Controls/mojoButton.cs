//	Created:			    2010-02-20
//	Last Modified:		    2018-11-14
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// inherits button and adds extra markup for Artisteer if RenderArtisteer is true
    /// </summary>
    /// 
    [ParseChildren(false),PersistChildren(true)]
    public class mojoButton : Button, INamingContainer
    {
		public bool RenderArtisteer { get; set; } = false;
		public bool UsejQueryButton { get; set; } = false;
		public string ArtButtonLeftClass { get; set; } = "l"; // changed to art-button-l in Artisteer 3
		public string ArtButtonRightClass { get; set; } = "r"; // changed to art-button-r in Artisteer 3

		public string LiteralTopMarkup { get; set; } = string.Empty;

		public string LiteralBottomMarkup { get; set; } = string.Empty;

		public HtmlTextWriterTag Element { get; set; } = HtmlTextWriterTag.Input;

        public bool BlockPostBack { get; set; } = false;

		//public virtual ITemplate InsideMarkup { get; set; }

		public new string Text
		{
			get { return (string)ViewState["NewText"] ?? ""; }
			set { ViewState["NewText"] = value; }
		}

		private Dictionary<string,string> parsedSubObjects = new();

		protected override HtmlTextWriterTag TagKey { get { return Element; } }

		//protected override void AddParsedSubObject(object obj)
		//{
		//	//if (obj is LiteralControl literal)
		//	//{
		//	//	Text = literal.Text + base.Text;
		//	//}
		//	//else
		//	//{
		//		base.AddParsedSubObject(obj);
		//	//}

			
		//}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (BlockPostBack)
            {
                OnClientClick = "return false;";
            }

            if (RenderArtisteer)
            {
                writer.Write("<span class=\"art-button-wrapper\">");
                writer.Write("<span class=\"" + ArtButtonLeftClass + "\"> </span>\n");
                writer.Write("<span class=\"" + ArtButtonRightClass + "\"> </span>\n");
            }
            else if (LiteralTopMarkup.Length > 0)
            {
                writer.Write(LiteralTopMarkup);
            }

            base.Render(writer);

            if (RenderArtisteer)
            {
                writer.Write("\n</span>");
            }
            else if (LiteralBottomMarkup.Length > 0)
            {
                writer.Write(LiteralBottomMarkup);
            }
        }
		protected override void OnPreRender(System.EventArgs e)
		{
			base.OnPreRender(e);
			if (HttpContext.Current == null) { return; }
			if (!RenderArtisteer && UsejQueryButton)
			{
				CssClass += " jqbutton ui-button ui-widget ui-state-default ui-corner-all";
			}
			

			// I wasn't sure what the best way to handle 'Text' would
			// be. Text is treated as another control which gets added
			// to the end of the button's control collection in this 
			//implementation
			LiteralControl lc = new(this.Text);

			if (Element != HtmlTextWriterTag.Input)
			{
				Controls.Add(lc);

				// Add a value for base.Text for the parent class
				// If the following line is omitted, the 'value' 
				// attribute will be blank upon rendering
				if (string.IsNullOrWhiteSpace(base.Text))
				{
					if (!string.IsNullOrWhiteSpace(this.Text))
					{
						base.Text = this.Text;
						var foo = base.Text;
					}
					else
					{
						base.Text = this.UniqueID;
					}
				}
				else
				{
					Controls.Add(new LiteralControl(base.Text));
				}
				//else if(!string.IsNullOrWhiteSpace(this.Text))
				//{
				//	base.Text = this.Text;
				//}
			}
			else if (!string.IsNullOrWhiteSpace(this.Text))
			{
				//this.Parent.Controls.Add(lc);
				base.Text = this.Text;
			}


		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			RenderChildren(writer);
		}
	}
}
