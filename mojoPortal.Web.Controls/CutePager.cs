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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls;

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

	private double _itemCount;

	public string ContainerElement { get; set; } = "div";

	public string ContainerElementCssClass { get; set; } = "modulepager";

	public string PageInfoCssClass { get; set; } = "PageInfo";

	public string CurrentPageCssClass { get; set; } = "SelectedPage";

	public string OtherPageCssClass { get; set; } = "ModulePager";

	public string OtherPageCellCssClass { get; set; } = "PagerOtherPageCells";

	public string CurrentPageCellCssClass { get; set; } = "PagerCurrentPageCell";

	public string SSCCellCssClass { get; set; } = "PagerSSCCells";

	public string PageURLFormat { get; set; } = string.Empty;

	public bool RenderAsTable { get; set; } = false;

	public bool LeaveOutSpans { get; set; } = false;

	public bool RenderAsList { get; set; } = false;

	/// <summary>
	/// if true an html 5 nav element is wrapped arround the pager
	/// </summary>
	public bool RenderNavElement { get; set; } = true;

	public bool WrapPageInfoInAnchor { get; set; } = false;


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
	public int PageSize { get; set; } = 15;

	[Browsable(false)]
	public int PageCount { get; set; }

	[Category("behaviouralSettings")]
	public bool ShowFirstLast { get; set; } = false;

	[Category("behaviouralSettings")]
	public bool EnableSmartShortCuts { get; set; } = true;

	[Category("behaviouralSettings")]
	public double SmartShortCutRatio { get; set; } = 3.0D;

	[Category("behaviouralSettings")]
	public int CompactedPageCount { get; set; } = 10;

	[Category("behaviouralSettings")]
	public int NotCompactedPageCount { get; set; } = 15;

	[Category("behaviouralSettings")]
	public int MaxSmartShortCutCount { get; set; } = 6;

	[Category("behaviouralSettings")]
	public int SmartShortCutThreshold { get; set; } = 30;

	[Category("behaviouralSettings")]
	public bool AlternativeTextEnabled { get; set; } = true;

	#endregion

	#region // Globalized Section


	[Category("GlobalizaionSettings")]
	public string NavigateToPageText { get; set; } = "Navigate to Page";

	[Category("GlobalizaionSettings")]
	public string GoClause { get; set; } = "go";

	[Category("GlobalizaionSettings")]
	public string OfClause { get; set; } = "of";

	[Category("GlobalizaionSettings")]
	public string FromClause { get; set; } = "From";

	[Category("GlobalizaionSettings")]
	public string PageClause { get; set; } = "Page";

	[Category("GlobalizaionSettings")]
	public string ToClause { get; set; } = "to";

	[Category("GlobalizaionSettings")]
	public string ShowingResultClause { get; set; } = "Showing Results";

	[Category("GlobalizaionSettings")]
	public string ShowResultClause { get; set; } = "Show Result";

	[Category("GlobalizaionSettings")]
	public string BackToFirstClause { get; set; } = "Navigate to First Page";

	[Category("GlobalizaionSettings")]
	public string GoToLastClause { get; set; } = "Navigate to Last Page";

	[Category("GlobalizaionSettings")]
	public string BackToPageClause { get; set; } = "Back to Page";

	[Category("GlobalizaionSettings")]
	public string NextToPageClause { get; set; } = "Next to Page";

	[Category("GlobalizaionSettings")]
	public string LastClause { get; set; } = "&gt;&gt;";

	[Category("GlobalizaionSettings")]
	public string FirstClause { get; set; } = "&lt;&lt;";

	[Category("GlobalizaionSettings")]
	public string PreviousClause { get; set; } = "&lt;";

	[Category("GlobalizaionSettings")]
	public string NextClause { get; set; } = "&gt;";

	[Category("GlobalizaionSettings")]
	public bool RTL { get; set; } = false;

	[Themeable(false)]
	public string ViewAllUrl { get; set; } = string.Empty;

	public string ViewAllText { get; set; } = "View All";


	#endregion

	#region // Render Utilities

	private string GenerateClassAttribute(string cssClass)
	{
		if (string.IsNullOrEmpty(cssClass)) { return string.Empty; }
		return $" class='{cssClass}' ";
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
			altGen.Append(NavigateToPageText + " " + desiredPage.ToString());

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
		string template = "<a" + GenerateClassAttribute(OtherPageCssClass)
			+ " href=\"{0}\" title=\""
			+ BackToFirstClause
			+ "\">" + FirstClause + "</a> ";

		if (RenderAsTable)
		{
			template = "<td class=\"" + OtherPageCellCssClass + "\">"
				+ template + "</td>";
		}
		else if (RenderAsList)
		{
			template = "<li" + GenerateClassAttribute(OtherPageCellCssClass) + ">"
				+ template + "</li>";
		}

		if (PageURLFormat.Length > 0)
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
			+ GenerateClassAttribute(OtherPageCssClass) + " href=\"{0}\" title=\""
			+ GoToLastClause + "\">"
			+ LastClause + "</a> ";

		if (RenderAsTable)
		{
			template = "<td class=\"" + OtherPageCellCssClass + "\">"
				+ template + "</td>";
		}
		else if (RenderAsList)
		{
			template = "<li" + GenerateClassAttribute(OtherPageCellCssClass) + ">"
				+ template + "</li>";
		}

		if (PageURLFormat.Length > 0)
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
			+ GenerateClassAttribute(OtherPageCssClass) + " href=\"{0}\" title=\""
			+ BackToPageClause + " "
			+ (CurrentIndex - 1).ToString() + "\">"
			+ PreviousClause + "</a> ";

		if (RenderAsTable)
		{
			template = "<td class=\"" + OtherPageCellCssClass + "\">"
				+ template + "</td>";
		}
		else if (RenderAsList)
		{
			template = "<li" + GenerateClassAttribute(OtherPageCellCssClass) + ">"
				+ template + "</li>";
		}

		if (PageURLFormat.Length > 0)
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
			+ GenerateClassAttribute(OtherPageCssClass) + " href=\"{0}\" title=\""
			+ NextToPageClause + " "
			+ (CurrentIndex + 1).ToString() + "\">"
			+ NextClause + "</a> ";

		if (RenderAsTable)
		{
			template = "<td class=\"" + OtherPageCellCssClass + "\">"
				+ template + "</td>";
		}
		else if (RenderAsList)
		{
			template = "<li" + GenerateClassAttribute(OtherPageCellCssClass) + ">"
				+ template + "</li>";
		}

		if (PageURLFormat.Length > 0)
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
		if (LeaveOutSpans)
		{
			result = "<a"
			+ GenerateClassAttribute(CurrentPageCssClass) + " href=\"{0}\" "
			+ GetAlternativeText(CurrentIndex) + " >"
			+ CurrentIndex.ToString() + "</a> ";
			//result = CurrentIndex.ToString();
		}
		else
		{
			result = "<span class=\"" + CurrentPageCssClass + "\""
				+ GetAlternativeText(CurrentIndex)
				+ ">" + CurrentIndex.ToString()
				+ "</span> ";
		}

		if (RenderAsTable)
		{
			result = "<td class=\"" + CurrentPageCellCssClass + "\">"
				+ result
			+ "</td>";

		}
		else if (RenderAsList)
		{
			result = "<li" + GenerateClassAttribute(CurrentPageCellCssClass) + ">"
				+ result
			+ "</li>";
		}

		if (PageURLFormat.Length > 0)
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
			+ GenerateClassAttribute(OtherPageCssClass) + " href=\"{0}\" "
			+ GetAlternativeText(index) + " >"
			+ index.ToString() + "</a> ";

		if (RenderAsTable)
		{
			template = "<td class=\"" + OtherPageCellCssClass + "\">"
				+ template + "</td>";
		}
		else if (RenderAsList)
		{
			template = "<li" + GenerateClassAttribute(OtherPageCellCssClass) + ">"
				+ template + "</li>";
		}


		if (PageURLFormat.Length > 0)
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
			+ GenerateClassAttribute(OtherPageCssClass) + " href=\"{0}\" "
			+ GetAlternativeText(index) + " >"
			+ index.ToString() + "</a> ";

		if (RenderAsTable)
		{
			template = "<td class=\"" + SSCCellCssClass + "\">"
				+ template + "</td>";
		}
		else if (RenderAsList)
		{
			template = "<li" + GenerateClassAttribute(SSCCellCssClass) + ">"
				+ template + "</li>";
		}


		if (PageURLFormat.Length > 0)
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

	private List<int> SmartShortCutList { get; set; }

	private void CalculateSmartShortcutAndFillList()
	{
		SmartShortCutList = new List<int>();
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

		if (ContainerElement.Length > 0)
		{
			writer.Write("<" + ContainerElement);
			if (ContainerElementCssClass.Length > 0)
			{
				writer.Write(" class='" + ContainerElementCssClass + "'");
			}


			writer.Write(">");

		}

		if (RenderNavElement)
		{
			writer.Write("<nav role='navigation'>");
		}

		if (RenderAsTable)
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
		else if (RenderAsList)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "pagination");
			writer.RenderBeginTag(HtmlTextWriterTag.Ul);

		}

		if (RenderAsList)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, PageInfoCssClass);
			writer.RenderBeginTag(HtmlTextWriterTag.Li);
		}

		if (CurrentIndex > -1)
		{
			// begin page info
			if (!LeaveOutSpans)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, PageInfoCssClass);
				writer.RenderBeginTag(HtmlTextWriterTag.Span);
			}

			if (WrapPageInfoInAnchor)
			{
				writer.Write("<a name='pageinfo'>");
			}
			writer.Write(PageClause + " " + CurrentIndex.ToString() + " " + OfClause + " " + PageCount.ToString());
			if (WrapPageInfoInAnchor)
			{
				writer.Write("</a>");
			}

			if (!LeaveOutSpans)
			{
				writer.RenderEndTag(); //span
			}
			writer.Write(" ");

			// end pageinfo

		}


		//start view all link
		if (ViewAllUrl.Length > 0)
		{
			if (!LeaveOutSpans)
			{
				if (CurrentIndex == -1)
				{

					writer.AddAttribute(HtmlTextWriterAttribute.Class, CurrentPageCssClass + " viewall");
					writer.RenderBeginTag(HtmlTextWriterTag.Span);
				}
				else
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Class, PageInfoCssClass + " viewall");
					writer.RenderBeginTag(HtmlTextWriterTag.Span);
				}

			}

			writer.Write("<a class='pagerall' href='" + ViewAllUrl + "'>");
			writer.Write(ViewAllText);
			writer.Write("</a>");


			if (!LeaveOutSpans)
			{
				writer.RenderEndTag(); //span
			}
			writer.Write(" ");

		}


		//end view all link

		if (RenderAsList)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "unavailable");
			writer.RenderEndTag(); //li
		}

		if (RenderAsTable)
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

		if (RenderAsTable)
		{
			writer.RenderEndTag();

			writer.RenderEndTag();
		}
		else if (RenderAsList)
		{
			writer.RenderEndTag(); //ul
		}

		if (RenderNavElement)
		{
			writer.Write("</nav>");
		}

		if (ContainerElement.Length > 0)
		{
			writer.Write("</" + ContainerElement + ">");
		}

		base.Render(writer);
		//base.RenderEndTag(writer);

	}


	#endregion
}