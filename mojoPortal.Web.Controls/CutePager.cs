////////////////////////////////////////////////////////////////////////////////////////////
//             				 Pager Control For ASP.NET 2.0			                      //
//								Created By Bidel.Akbari									  //
//									November 2006									  	  //
//								bidel.akbari@gmail.com							          //
////////////////////////////////////////////////////////////////////////////////////////////
// from: http://www.codeproject.com/aspnet/ASPNETPagerControl.asp
// with Modifications by 
// 2007-09-01 added support for paging with links instead of postback
// added option to render without html tables and made it default
// added ability to set CssClasses
// 2012-04-27  added support for RenderAsList
//2013-01-30 added container element
// 2014-01-08 added support for view all url

#region // using Directives
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using System.Collections;
#endregion

namespace mojoPortal.Web.Controls
{
    [ToolboxData("<{0}:Pager runat=\"server\"></{0}:Pager>")]
    public class CutePager : WebControl, IPostBackEventHandler, INamingContainer
    {

        #region // PostBack Stuff
        private static readonly object EventCommand = new object();


        public event CommandEventHandler Command
        {
            add { Events.AddHandler(EventCommand, value); }
            remove { Events.RemoveHandler(EventCommand, value); }
        }



        protected virtual void OnCommand(CommandEventArgs e)
        {
            CommandEventHandler clickHandler = (CommandEventHandler)Events[EventCommand];
            if (clickHandler != null) clickHandler(this, e);
        }



        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            OnCommand(new CommandEventArgs(this.UniqueID, Convert.ToInt32(eventArgument)));
        }
        #endregion

        

        #region // Accessors

        private double _itemCount; // total count of rows
        private int _pageSize = 15; // the Size of items that can be displayed on page
        private int _pageCount; // Total No of Pages
        private bool _showFirstLast = false; // to determine wheter show first and last link or not

        // added by 
        private string _PageURLFormat = string.Empty;
        private bool _RenderAsTable = false;
        private string _PageInfoCssClass = "PageInfo";
        private string _CurrentPageCssClass = "SelectedPage";
        private string _OtherPageCssClass = "ModulePager";
        private string _OtherPageCellCssClass = "PagerOtherPageCells";
        private string _CurrentPageCellCssClass = "PagerCurrentPageCell";
        private string _SSCCellCssClass = "PagerSSCCells";

        private string containerElement = "div";

        public string ContainerElement
        {
            get { return containerElement; }
            set { containerElement = value; }
        }

        private string containerElementCssClass = "modulepager";

        public string ContainerElementCssClass
        {
            get { return containerElementCssClass; }
            set { containerElementCssClass = value; }
        }

        public string PageInfoCssClass
        {
            get { return _PageInfoCssClass; }
            set { _PageInfoCssClass = value; }
        }

        public string CurrentPageCssClass
        {
            get { return _CurrentPageCssClass; }
            set { _CurrentPageCssClass = value; }
        }

        public string OtherPageCssClass
        {
            get { return _OtherPageCssClass; }
            set { _OtherPageCssClass = value; }
        }

        public string OtherPageCellCssClass
        {
            get { return _OtherPageCellCssClass; }
            set { _OtherPageCellCssClass = value; }
        }

        public string CurrentPageCellCssClass
        {
            get { return _CurrentPageCellCssClass; }
            set { _CurrentPageCellCssClass = value; }
        }

        public string SSCCellCssClass
        {
            get { return _SSCCellCssClass; }
            set { _SSCCellCssClass = value; }
        }

        

        public string PageURLFormat
        {
            get { return _PageURLFormat; }
            set { _PageURLFormat = value; }
        }

        public bool RenderAsTable
        {
            get { return _RenderAsTable; }
            set { _RenderAsTable = value; }
        }

        private bool leaveOutSpans = false;
        public bool LeaveOutSpans
        {
            get { return leaveOutSpans; }
            set { leaveOutSpans = value; }
        }

        private bool renderAsList = false;

        public bool RenderAsList
        {
            get { return renderAsList; }
            set { renderAsList = value; }
        }

        private bool renderNavElement = false;
        /// <summary>
        /// if true an html 5 nav element is wrapped arround the pager
        /// </summary>
        public bool RenderNavElement
        {
            get { return renderNavElement; }
            set { renderNavElement = value; }
        }

        private bool wrapPageInfoInAnchor = false;
        public bool WrapPageInfoInAnchor
        {
            get { return wrapPageInfoInAnchor; }
            set { wrapPageInfoInAnchor = value; }
        }


        [Browsable(false)]
        public double ItemCount
        {
            get { return _itemCount; }
            set
            {
                _itemCount = value;

                double divide = ItemCount / PageSize;
                double ceiled = System.Math.Ceiling(divide);
                PageCount = Convert.ToInt32(ceiled);
            }
        }

        [Browsable(false)]
        public int CurrentIndex
        {
            get
            {
                if (ViewState["aspnetPagerCurrentIndex"] == null)
                {
                    ViewState["aspnetPagerCurrentIndex"] = 1;
                    return 1;
                }
                else
                {
                    return Convert.ToInt32(ViewState["aspnetPagerCurrentIndex"]);
                }
                // return _currentIndex;
            }
            // set { _currentIndex = value; }
            set { ViewState["aspnetPagerCurrentIndex"] = value; }
        }

        [Category("behaviouralSettings")]
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        [Browsable(false)]
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }


        [Category("behaviouralSettings")]
        public bool ShowFirstLast
        {
            get { return _showFirstLast; }
            set { _showFirstLast = value; }
        }

        // to enable/disable smart shortcuts
        private bool _enableSSC = true;
        [Category("behaviouralSettings")]
        public bool EnableSmartShortCuts
        {
            get { return _enableSSC; }
            set { _enableSSC = value; }
        }

        // the ration to count the space whithin the smartshortcut pages
        private double _sscRatio = 3.0D;
        [Category("behaviouralSettings")]
        public double SmartShortCutRatio
        {
            get { return _sscRatio; }
            set { _sscRatio = value; }
        }

        // first compacted group of visible page numbers
        private int _firstCompactedPageCount = 10;
        [Category("behaviouralSettings")]
        public int CompactedPageCount
        {
            get { return _firstCompactedPageCount; }
            set { _firstCompactedPageCount = value; }
        }

        // ordinary not compacted visible page numbers count
        private int _notCompactedPageCount = 15;
        [Category("behaviouralSettings")]
        public int NotCompactedPageCount
        {
            get { return _notCompactedPageCount; }
            set { _notCompactedPageCount = value; }
        }

        // maximum number of smart shortcuts
        private int _maxSmartShortCutCount = 6;
        [Category("behaviouralSettings")]
        public int MaxSmartShortCutCount
        {
            get { return _maxSmartShortCutCount; }
            set { _maxSmartShortCutCount = value; }
        }

        // the number which determines that the smart short cuts must be rendered if pagecount is morethatn specific number
        private int _sscThreshold = 30;
        [Category("behaviouralSettings")]
        public int SmartShortCutThreshold
        {
            get { return _sscThreshold; }
            set { _sscThreshold = value; }
        }

        // generate alt title for page indeces
        private bool _altEnabled = true;
        [Category("behaviouralSettings")]
        public bool AlternativeTextEnabled
        {
            get { return _altEnabled; }
            set { _altEnabled = value; }
        }

        #endregion

        #region // Globalized Section

        private string _GO = "go";
        private string _OF = "of";
        private string _FROM = "From";
        private string _PAGE = "Page";
        private string _TO = "to";
        private string _SHOWING_RESULT = "Showing Results";
        private string _SHOW_RESULT = "Show Result";
        private string _BACK_TO_FIRST = "Navigate to First Page";
        private string _GO_TO_LAST = "Navigate to Last Page";
        private string _BACK_TO_PAGE = "Back to Page";
        private string _NEXT_TO_PAGE = "Next to Page";
        private string _NAVIGATE_TO_PAGE = "Navigate to Page";
        private string _LAST = "&gt;&gt;";
        private string _FIRST = "&lt;&lt;";
        private string _previous = "&lt;";
        private string _next = "&gt;";

        [Category("GlobalizaionSettings")]
        public string NavigateToPageText
        {
            get { return _NAVIGATE_TO_PAGE; }
            set { _NAVIGATE_TO_PAGE = value; }
        }

        [Category("GlobalizaionSettings")]
        public string GoClause
        {
            get { return _GO; }
            set { _GO = value; }
        }

        [Category("GlobalizaionSettings")]
        public string OfClause
        {
            get { return _OF; }
            set { _OF = value; }
        }

        [Category("GlobalizaionSettings")]
        public string FromClause
        {
            get { return _FROM; }
            set { _FROM = value; }
        }

        [Category("GlobalizaionSettings")]
        public string PageClause
        {
            get { return _PAGE; }
            set { _PAGE = value; }
        }

        [Category("GlobalizaionSettings")]
        public string ToClause
        {
            get { return _TO; }
            set { _TO = value; }
        }

        [Category("GlobalizaionSettings")]
        public string ShowingResultClause
        {
            get { return _SHOWING_RESULT; }
            set { _SHOWING_RESULT = value; }
        }

        [Category("GlobalizaionSettings")]
        public string ShowResultClause
        {
            get { return _SHOW_RESULT; }
            set { _SHOW_RESULT = value; }
        }

        [Category("GlobalizaionSettings")]
        public string BackToFirstClause
        {
            get { return _BACK_TO_FIRST; }
            set { _BACK_TO_FIRST = value; }
        }

        [Category("GlobalizaionSettings")]
        public string GoToLastClause
        {
            get { return _GO_TO_LAST; }
            set { _GO_TO_LAST = value; }
        }

        [Category("GlobalizaionSettings")]
        public string BackToPageClause
        {
            get { return _BACK_TO_PAGE; }
            set { _BACK_TO_PAGE = value; }
        }

        [Category("GlobalizaionSettings")]
        public string NextToPageClause
        {
            get { return _NEXT_TO_PAGE; }
            set { _NEXT_TO_PAGE = value; }
        }

        [Category("GlobalizaionSettings")]
        public string LastClause
        {
            get { return _LAST; }
            set { _LAST = value; }
        }

        [Category("GlobalizaionSettings")]
        public string FirstClause
        {
            get { return _FIRST; }
            set { _FIRST = value; }
        }

        [Category("GlobalizaionSettings")]
        public string PreviousClause
        {
            get { return _previous; }
            set { _previous = value; }
        }

        [Category("GlobalizaionSettings")]
        public string NextClause
        {
            get { return _next; }
            set { _next = value; }
        }

        private bool _rightToLeft = false;
        [Category("GlobalizaionSettings")]
        public bool RTL
        {
            get { return _rightToLeft; }
            set { _rightToLeft = value; }
        }

        private string viewAllUrl = string.Empty;

        [Themeable(false)]
        public string ViewAllUrl
        {
            get { return viewAllUrl; }
            set { viewAllUrl = value; }
        }

        private string viewAllText = "View All";

        public string ViewAllText
        {
            get { return viewAllText; }
            set { viewAllText = value; }
        }


        #endregion

        #region // Render Utilities

        private string GenerateClassAttribute(string cssClass)
        {
            if(string.IsNullOrEmpty(cssClass)) { return string.Empty;}
            return " class='" + cssClass + "' ";
        }

        private string GenerateAltMessage(int desiredPage)
        {
            StringBuilder altGen = new StringBuilder();
            if (ItemCount > 0)
            {
                altGen.Append(desiredPage == CurrentIndex ? ShowingResultClause : ShowResultClause);
                altGen.Append(" ");
                altGen.Append(((desiredPage - 1) * PageSize) + 1);
                altGen.Append(" ");
                altGen.Append(ToClause);
                altGen.Append(" ");
                altGen.Append(desiredPage == PageCount ? ItemCount : desiredPage * PageSize);
                altGen.Append(" ");
                altGen.Append(OfClause);
                altGen.Append(" ");
                altGen.Append(ItemCount);
            }
            else
            {
                // just use page number
                altGen.Append(_NAVIGATE_TO_PAGE + " " + desiredPage.ToString());

            }

            return altGen.ToString();
        }

        private string GetAlternativeText(int index)
        {
            return AlternativeTextEnabled ? string.Format(" title=\"{0}\"", 
                GenerateAltMessage(index)) : "";
        }

        private string RenderFirst()
        {
            string template = "<a" + GenerateClassAttribute(_OtherPageCssClass)
                + " href=\"{0}\" title=\"" 
                + BackToFirstClause
                + "\">" + FirstClause + "</a> ";

            if (_RenderAsTable)
            {
                template = "<td class=\"" + _OtherPageCellCssClass + "\">"
                    + template + "</td>";
            }
            else if (renderAsList)
            {
                template = "<li" + GenerateClassAttribute(_OtherPageCellCssClass) + ">"
                    + template + "</li>";
            }

            if (_PageURLFormat.Length > 0)
            {
                string templateURL = String.Format(PageURLFormat, "1");
                return String.Format(
                    template,
                    templateURL);
            }
            else
            {

                return String.Format(
                    template,
                    Page.ClientScript.GetPostBackClientHyperlink(this, "1"));
            }
        }

        private string RenderLast()
        {
            string template = "<a" 
                + GenerateClassAttribute(_OtherPageCssClass) + " href=\"{0}\" title=\""
                + GoToLastClause + "\">" 
                + LastClause + "</a> ";

            if (_RenderAsTable)
            {
                template = "<td class=\"" + _OtherPageCellCssClass + "\">"
                    + template + "</td>";
            }
            else if (renderAsList)
            {
                template = "<li" + GenerateClassAttribute(_OtherPageCellCssClass) + ">"
                    + template + "</li>";
            }

            if (_PageURLFormat.Length > 0)
            {
                string templateURL = String.Format(
                    PageURLFormat, PageCount.ToString());
                
                return String.Format(
                    template,
                    templateURL);
            }
            else
            {
                return String.Format(
                    template, 
                    Page.ClientScript.GetPostBackClientHyperlink(
                    this, PageCount.ToString()));
            }
        }

        private string RenderBack()
        {
            string template = "<a" 
                + GenerateClassAttribute(_OtherPageCssClass) + " href=\"{0}\" title=\"" 
                + BackToPageClause + " " 
                + (CurrentIndex - 1).ToString() + "\">" 
                + PreviousClause + "</a> ";

            if (_RenderAsTable)
            {
                template = "<td class=\"" + _OtherPageCellCssClass + "\">"
                    + template + "</td>";
            }
            else if (renderAsList)
            {
                template = "<li" + GenerateClassAttribute(_OtherPageCellCssClass) + ">"
                    + template + "</li>";
            }

            if (_PageURLFormat.Length > 0)
            {
                string templateURL = String.Format(
                    PageURLFormat, (CurrentIndex - 1).ToString());
                return String.Format(
                    template,
                    templateURL);
            }
            else
            {
                return String.Format(
                    template,
                    Page.ClientScript.GetPostBackClientHyperlink(
                    this, (CurrentIndex - 1).ToString()));
            }
        
        }

        private string RenderNext()
        {
            string template = "<a" 
                + GenerateClassAttribute(_OtherPageCssClass) + " href=\"{0}\" title=\"" 
                + NextToPageClause + " " 
                + (CurrentIndex + 1).ToString() + "\">" 
                + NextClause + "</a> ";

            if (_RenderAsTable)
            {
                template = "<td class=\"" + _OtherPageCellCssClass + "\">"
                    + template + "</td>";
            }
            else if (renderAsList)
            {
                template = "<li" + GenerateClassAttribute(_OtherPageCellCssClass) + ">"
                    + template + "</li>";
            }

            if (_PageURLFormat.Length > 0)
            {
                string templateURL = String.Format(PageURLFormat, (CurrentIndex + 1).ToString());
                return String.Format(
                    template,
                    templateURL);
            }
            else
            {
                return String.Format(
                    template,
                    Page.ClientScript.GetPostBackClientHyperlink(
                    this, (CurrentIndex + 1).ToString()));
            }
        
        }

        private string RenderCurrent()
        {
            //string result = "<a class=\""
            //    + _CurrentPageCssClass + "\" href=\"{0}\" "
            //    + GetAlternativeText(CurrentIndex) + " >"
            //    + CurrentIndex.ToString() + "</a>&nbsp;";
            string result;
            if (leaveOutSpans)
            {
                result = "<a"
                + GenerateClassAttribute(_CurrentPageCssClass) + " href=\"{0}\" "
                + GetAlternativeText(CurrentIndex) + " >"
                + CurrentIndex.ToString() + "</a> ";
                //result = CurrentIndex.ToString();
            }
            else
            {
                result = "<span class=\"" + _CurrentPageCssClass + "\""
                    + GetAlternativeText(CurrentIndex)
                    + ">" + CurrentIndex.ToString()
                    + "</span> ";
            }

            if (_RenderAsTable)
            {
                result = "<td class=\"" + _CurrentPageCellCssClass + "\">"
                    + result
                + "</td>";

            }
            else if (renderAsList)
            {
                result = "<li" + GenerateClassAttribute(_CurrentPageCellCssClass) + ">"
                    + result
                + "</li>";
            }

            if (_PageURLFormat.Length > 0)
            {
                string templateURL = String.Format(PageURLFormat, (CurrentIndex).ToString());
                return String.Format(
                    result,
                    templateURL);
            }
            


            return result;
        }

        private string RenderOther(int index)
        {
            string template = "<a" 
                + GenerateClassAttribute(_OtherPageCssClass) + " href=\"{0}\" " 
                + GetAlternativeText(index) + " >" 
                + index.ToString() + "</a> ";
            
            if (_RenderAsTable)
            {
                template = "<td class=\"" + _OtherPageCellCssClass + "\">"
                    + template + "</td>";
            }
            else if (renderAsList)
            {
                template = "<li" + GenerateClassAttribute(_OtherPageCellCssClass) + ">"
                    + template + "</li>";
            }


            if (_PageURLFormat.Length > 0)
            {
                string templateURL = String.Format(
                    PageURLFormat, index.ToString());

                return String.Format(
                    template,
                    templateURL);
            }
            else
            {
                return String.Format(
                    template,
                    Page.ClientScript.GetPostBackClientHyperlink(
                    this, index.ToString()));
            }
        
        }

        private string RenderSSC(int index)
        {
            string template = "<a" 
                + GenerateClassAttribute(_OtherPageCssClass) + " href=\"{0}\" " 
                + GetAlternativeText(index) + " >" 
                + index.ToString() + "</a> ";

            if (_RenderAsTable)
            {
                template = "<td class=\"" + _SSCCellCssClass + "\">"
                    + template + "</td>";
            }
            else if (renderAsList)
            {
                template = "<li" + GenerateClassAttribute(_SSCCellCssClass) + ">"
                    + template + "</li>";
            }


            if (_PageURLFormat.Length > 0)
            {
                string templateURL = String.Format(PageURLFormat, index.ToString());
                return String.Format(
                    template,
                    templateURL);
            }
            else
            {
                return String.Format(
                    template,
                    Page.ClientScript.GetPostBackClientHyperlink(
                    this, index.ToString()));
            }
        
        }
        #endregion

        #region // Smart ShortCut Stuff

        /* smart shortcut list calculator and list */

        private List<int> _smartShortCutList;
        private List<int> SmartShortCutList
        {
            get { return _smartShortCutList; }
            set { _smartShortCutList = value; }
        }

        private void CalculateSmartShortcutAndFillList()
        {
            _smartShortCutList = new List<int>();
            double shortCutCount = this.PageCount * SmartShortCutRatio / 100;
            double shortCutCountRounded = System.Math.Round(shortCutCount, 0);
            if (shortCutCountRounded > MaxSmartShortCutCount) shortCutCountRounded = MaxSmartShortCutCount;
            if (shortCutCountRounded == 1) shortCutCountRounded++;

            for (int i = 1; i < shortCutCountRounded + 1; i++)
            {
                int calculatedValue = (int)(System.Math.Round((this.PageCount * (100 / shortCutCountRounded) * i / 100) * 0.1, 0) * 10);
                if (calculatedValue >= this.PageCount) break;
                SmartShortCutList.Add(calculatedValue);
            }
        }

        /* smart shortcut list calculator and list */


        private void RenderSmartShortCutByCriteria(int basePageNumber, bool getRightBand, HtmlTextWriter writer)
        {
            if (IsSmartShortCutAvailable())
            {

                List<int> lstSSC = this.SmartShortCutList;

                int rVal = -1;
                
                if (getRightBand)
                {
                    for (int i = 0; i < lstSSC.Count; i++)
                    {
                        if (lstSSC[i] > basePageNumber)
                        {
                            rVal = i;
                            // sometimes we dont reach here and inappropriate ssc show's up
                            
                            break;
                        }
                    }

                    if (rVal >= 0)
                    {
                        for (int i = rVal; i < lstSSC.Count; i++)
                        {
                            if (lstSSC[i] != basePageNumber)
                            {
                                writer.Write(RenderSSC(lstSSC[i]));
                            }
                            
                        }
                    }


                }
                else if (!getRightBand)
                {

                    for (int i = 0; i < lstSSC.Count; i++)
                    {
                        if (basePageNumber > lstSSC[i])
                        {
                            rVal = i;
                            // sometimes we dont reach here and inappropriate ssc show's up
                            // allowRender = true;
                            // break;
                        }
                    }

                    if (rVal >= 0)
                    {
                        for (int i = 0; i < rVal + 1; i++)
                        {
                            if (lstSSC[i] != basePageNumber)
                            {
                                writer.Write(RenderSSC(lstSSC[i]));
                            }
                            //System.Web.HttpContext.Current.Response.Write(lstSSC.Count.ToString() +"<br/>");
                            //System.Web.HttpContext.Current.Response.Write("left " + i.ToString() + "<br/>");
                            // System.Web.HttpContext.Current.Response.Write(lstSSC[i].ToString());
                        }
                    }



                }

            }

        }


        bool IsSmartShortCutAvailable()
        {
            return this.EnableSmartShortCuts && this.SmartShortCutList != null && this.SmartShortCutList.Count != 0;
        }
        #endregion

        #region // Override Control's Render operation
        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            DoRender(writer);
        }

        private void DoRender(HtmlTextWriter writer)
        {
            if (Page != null) Page.VerifyRenderingInServerForm(this);

            //base.RenderBeginTag(writer);

            if (this.PageCount > this.SmartShortCutThreshold)
            {

                if (EnableSmartShortCuts)
                {

                    CalculateSmartShortcutAndFillList();

                }
            }

            if (containerElement.Length > 0)
            {
                writer.Write("<" + containerElement);
                if (containerElementCssClass.Length > 0)
                {
                    writer.Write(" class='" + containerElementCssClass + "'");
                }
                

                writer.Write(">");

            }

            if (renderNavElement)
            {
                writer.Write("<nav role='navigation'>");
            }

            if (_RenderAsTable)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "3");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "1");
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "PagerContainerTable");
                if (RTL) writer.AddAttribute(HtmlTextWriterAttribute.Dir, "rtl");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);

                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "PagerInfoCell");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
            }
            else if (renderAsList)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "pagination");
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            }

            if (renderAsList)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, _PageInfoCssClass);
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
            }

            if(CurrentIndex > -1)
            {
                // begin page info
                if (!leaveOutSpans)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, _PageInfoCssClass);
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                }

                if (wrapPageInfoInAnchor)
                {
                    writer.Write("<a name='pageinfo'>");
                }
                writer.Write(PageClause + " " + CurrentIndex.ToString() + " " + OfClause + " " + PageCount.ToString());
                if (wrapPageInfoInAnchor)
                {
                    writer.Write("</a>");
                }

                if (!leaveOutSpans)
                {
                    writer.RenderEndTag(); //span
                }
                writer.Write(" ");

                // end pageinfo

            }
            

            //start view all link
            if (viewAllUrl.Length > 0)
            {
                if (!leaveOutSpans)
                {
                    if(CurrentIndex == -1)
                    {
                        
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, _CurrentPageCssClass + " viewall");
                        writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, _PageInfoCssClass + " viewall");
                        writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    }
                    
                }

                writer.Write("<a class='pagerall' href='" + viewAllUrl + "'>");
                writer.Write(viewAllText);
                writer.Write("</a>");
                

                if (!leaveOutSpans)
                {
                    writer.RenderEndTag(); //span
                }
                writer.Write(" ");

            }


            //end view all link

            if (renderAsList)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "unavailable");
                writer.RenderEndTag(); //li
            }

            if (_RenderAsTable)
            {
                writer.RenderEndTag();

            }

            if (CurrentIndex > -1)
            {
                if (ShowFirstLast && CurrentIndex != 1)
                    writer.Write(RenderFirst());

                if (CurrentIndex != 1)
                    writer.Write(RenderBack());
            }

            if (CurrentIndex < CompactedPageCount)
            {

                if (CompactedPageCount > PageCount) CompactedPageCount = PageCount;

                for (int i = 1; i < CompactedPageCount + 1; i++)
                {
                    if (i == CurrentIndex)
                    {
                        writer.Write(RenderCurrent());
                    }
                    else
                    {
                        writer.Write(RenderOther(i));
                    }
                }

                RenderSmartShortCutByCriteria(CompactedPageCount, true, writer);

            }
            else if (CurrentIndex >= CompactedPageCount && CurrentIndex < NotCompactedPageCount)
            {

                if (NotCompactedPageCount > PageCount) NotCompactedPageCount = PageCount;

                for (int i = 1; i < NotCompactedPageCount + 1; i++)
                {
                    if (i == CurrentIndex)
                    {
                        writer.Write(RenderCurrent());
                    }
                    else
                    {
                        writer.Write(RenderOther(i));
                    }
                }

                RenderSmartShortCutByCriteria(NotCompactedPageCount, true, writer);

            }
            else if (CurrentIndex >= NotCompactedPageCount)
            {
                int gapValue = NotCompactedPageCount / 2;
                int leftBand = CurrentIndex - gapValue;
                int rightBand = CurrentIndex + gapValue;


                RenderSmartShortCutByCriteria(leftBand, false, writer);

                for (int i = leftBand; (i < rightBand + 1) && i < PageCount + 1; i++)
                {
                    if (i == CurrentIndex)
                    {
                        writer.Write(RenderCurrent());
                    }
                    else
                    {
                        writer.Write(RenderOther(i));
                    }
                }

                if (rightBand < this.PageCount)
                {

                    RenderSmartShortCutByCriteria(rightBand, true, writer);
                }
            }

            if (CurrentIndex != PageCount)
                writer.Write(RenderNext());

            if (ShowFirstLast && CurrentIndex != PageCount)
                writer.Write(RenderLast());

            if (_RenderAsTable)
            {
                writer.RenderEndTag();

                writer.RenderEndTag();
            }
            else if (renderAsList)
            {
                writer.RenderEndTag(); //ul
            }

            if (renderNavElement)
            {
                writer.Write("</nav>");
            }

            if (containerElement.Length > 0)
            {
                writer.Write("</" + containerElement + ">");
            }

            base.Render(writer);
            //base.RenderEndTag(writer);

        }


        #endregion
    }
}