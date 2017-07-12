// Author:					
// Created:				2007-09-10
// Last Modified:		    2009-07-20

using System;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace mojoPortal.Web.Controls
{
    /// <summary>
    /// If DoRounding is true renders top elements for
    /// Corner Rounding Based on Nifty Corners: rounded corners 
    /// without images By Alessandro Fulciniti
    /// http://www.html.it/articoli/nifty/index.html
    /// 
    /// Tip: this property can be set in theme.skin file for a skin
    /// to turn rounded corners on or off. The rounding is actually 
    /// done by css, this control only adds markup (extra unsemantic divs)
    /// that the css is applied to for the rounding effect.
    /// 
    /// Example css
    ///.rtop, .rbottom{display: block;}
    ///.rtop, .rbottom{background: #414141;}
    ///.rtop .r1, .rtop .r2, .rtop .r3, .rtop .r4,
    ///.rbottom .r1, .rbottom .r2, .rbottom .r3, .rbottom .r4{background: #545454;}
    ///.rtop .r1, .rtop .r2, .rtop .r3, .rtop .r4,
    ///.rbottom .r1, .rbottom .r2, .rbottom .r3, .rbottom .r4
    ///{ display: block;height: 1px;overflow: hidden; }
    ///.r1{margin: 0 5px}
    ///.r2{margin: 0 3px}
    ///.r3{margin: 0 2px}
    ///.rtop .r4, .rbottom .r4{ margin: 0 1px; height: 2px }
    ///.rbottom { margin-bottom:10px;  }
    /// </summary>
    public class CornerRounderTop : WebControl
    {
        #region Constructors

        public CornerRounderTop()
		{
			EnsureChildControls();
		}

		#endregion

        private bool doRounding = false;
        private string roundingMarkup = "<div class='rtop'><div class='r1'></div><div class='r2'></div><div class='r3'></div><div class='r4'></div></div>";

        [Bindable(true), Category("Behavior"), DefaultValue(false)]
        public bool DoRounding
        {
            get { return doRounding; }
            set { doRounding = value; }
        }

        public string RoundingMarkup
        {
            get { return roundingMarkup; }
            set { roundingMarkup = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }

        protected override void Render(HtmlTextWriter writer)
        {
            
            if (HttpContext.Current == null)
            {
                // render to the designer
                writer.Write("[" + this.ID + "]");
                return;
            }

            DoRender(writer);
        }

        private void DoRender(HtmlTextWriter writer)
        {
            if (DoRounding)
            {
                writer.Write(roundingMarkup);
            }

        }

    }
}
