// Author:
// Created:       2009-05-02
// Last Modified: 2017-06-20
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Data;
using System.Web.UI;

namespace mojoPortal.Web.BlogUI
{
	public partial class BlogCategories : UserControl
	{
		private int pageId = -1;

		public int PageId
		{
			get { return pageId; }
			set { pageId = value; }
		}

		private int moduleId = -1;

		public int ModuleId
		{
			get { return moduleId; }
			set { moduleId = value; }
		}

		private string siteRoot = string.Empty;

		public string SiteRoot
		{
			get { return siteRoot; }
			set { siteRoot = value; }
		}

		private bool renderAsTagCloud = true;

		public bool RenderAsTagCloud
		{
			get { return renderAsTagCloud; }
			set { renderAsTagCloud = value; }
		}

		private bool canEdit = false;

		public bool CanEdit
		{
			get { return canEdit; }
			set { canEdit = value; }
		}

		private string headingElement = "h3";

		public string HeadingElement
		{
			get { return headingElement; }
			set { headingElement = value; }
		}


		protected void Page_Load(object sender, EventArgs e)
		{
			if (displaySettings.OverrideCategoryLabel.Length > 0)
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
			if (this.Visible)
			{
				BindList();
			}

			base.OnPreRender(e);
		}


		private void BindList()
		{
			if (pageId == -1)
			{
				return;
			}

			if (moduleId == -1)
			{
				return;
			}

			litHeadingOpenTag.Text = "<";
			litHeadingOpenTag.Text += headingElement;
			litHeadingOpenTag.Text += !string.IsNullOrWhiteSpace(displaySettings.CategoryListHeadingClass) ? " class=\"" + displaySettings.CategoryListHeadingClass + "\"" : string.Empty;
			litHeadingOpenTag.Text += ">";

			litHeadingCloseTag.Text = "</" + headingElement + ">";

			using (IDataReader reader = Blog.GetCategories(ModuleId))
			{
				if (renderAsTagCloud)
				{
					dlCategories.Visible = false;
					cloud.DataHrefFormatString =
						SiteRoot +
						"/Blog/ViewCategory.aspx?cat={0}&amp;mid=" +
						ModuleId.ToInvariantString() +
						"&amp;pageid=" + PageId.ToInvariantString()
					;

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


		protected string GetCssClass(double catagoryCount)
		{
			return string.Empty;
		}
	}
}