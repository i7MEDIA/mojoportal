using System;
using System.Data;
using System.Web.UI;
using mojoPortal.Business;
using Resources;

namespace mojoPortal.Web.BlogUI;

public partial class BlogCategories : UserControl
{
	public int PageId { get; set; } = -1;
	public int ModuleId { get; set; } = -1;
	public string SiteRoot { get; set; } = string.Empty;
	public bool RenderAsTagCloud { get; set; } = true;
	public bool CanEdit { get; set; } = false;
	public string HeadingElement { get; set; } = "h3";


	protected void Page_Load(object sender, EventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(displaySettings.OverrideCategoryLabel))
		{
			litHeading.Text = displaySettings.OverrideCategoryLabel;
		}
		else
		{
			litHeading.Text = BlogResources.BlogCategoriesLabel;
		}

		HeadingElement = displaySettings.CategoryListHeadingElement;
	}


	protected override void OnPreRender(EventArgs e)
	{
		if (Visible)
		{
			BindList();
		}

		base.OnPreRender(e);
	}


	private void BindList()
	{
		if (PageId == -1 || ModuleId == -1)
		{
			return;
		}

		var catListCssClass = string.Empty;
		if (!string.IsNullOrWhiteSpace(displaySettings.CategoryListHeadingClass))
		{
			catListCssClass = $" class=\"{displaySettings.CategoryListHeadingClass}\"";
		}

		litHeadingOpenTag.Text = $"<{HeadingElement}{catListCssClass}>";
		litHeadingCloseTag.Text = $"</{HeadingElement}>";

		using IDataReader reader = Blog.GetCategories(ModuleId);
		if (RenderAsTagCloud)
		{
			dlCategories.Visible = false;
			cloud.DataHrefFormatString = "Blog/ViewCategory.aspx".ToLinkBuilder().PageId(PageId).ModuleId(ModuleId).AddParam("cat", "{0}").ToString();
			cloud.UseWeightInTextFormat = true;
			cloud.DataHrefField = "CategoryID";
			cloud.DataTextField = "Category";
			cloud.DataWeightField = "PostCount";
			cloud.DataSource = reader;
			cloud.DataBind();

			if (cloud.Items.Count == 0)
			{
				Visible = false;
			}
		}
		else
		{
			cloud.Visible = false;
			dlCategories.DataSource = reader;
			dlCategories.DataBind();
			dlCategories.Visible = (dlCategories.Items.Count > 0);

			if (dlCategories.Items.Count == 0)
			{
				Visible = false;
			}
		}
	}
}