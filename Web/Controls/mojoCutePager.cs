using mojoPortal.Web.Controls;
using Resources;
using System;
using System.Web;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// This control inherits form CutePager and applies localization from Resource.resx
    /// </summary>
    public class mojoCutePager : CutePager
    {

        protected override void OnInit(EventArgs e)
        {
            
            if (HttpContext.Current == null) { return; }
            base.OnInit(e);
            DoInit();
            
        }

        private void DoInit()
        {
            this.NavigateToPageText = Resource.CutePagerNavigateToPageText;
            this.GoClause = Resource.CutePagerGoClause;
            this.OfClause = Resource.CutePagerOfClause;
            this.FromClause = Resource.CutePagerFromClause;
            this.PageClause = Resource.CutePagerPageClause;
            this.ToClause = Resource.CutePagerToClause;
            this.ShowingResultClause = Resource.CutePagerShowingResultClause;
            this.ShowResultClause = Resource.CutePagerShowResultClause;
            this.BackToFirstClause = Resource.CutePagerBackToFirstClause;
            this.GoToLastClause = Resource.CutePagerGoToLastClause;
            this.BackToPageClause = Resource.CutePagerBackToPageClause;
            this.NextToPageClause = Resource.CutePagerNextToPageClause;
            this.ViewAllText = Resource.PagerViewAll;

            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft)
                {
                    this.RTL = true;
                }
            }
            catch { }

        }

        //protected override void Render(HtmlTextWriter writer)
        //{
        //    if (HttpContext.Current == null)
        //    {
        //        writer.Write("[" + this.ID + "]");
        //        return;
        //    }

        //    base.Render(writer);


        //}

    }
}
