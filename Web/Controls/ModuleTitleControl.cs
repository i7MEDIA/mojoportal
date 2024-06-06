using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI;

public class ModuleTitleControl : WebControl, INamingContainer
{
	#region Constructors

	public ModuleTitleControl()
	{
		if (HttpContext.Current is null)
		{
			return;
		}

		EnsureChildControls();
	}

	#endregion

	#region Control Declarations

	protected Literal litModuleTitle;
	protected HyperLink lnkModuleSettings;
	protected HyperLink lnkModuleEdit;
	protected ImageButton ibPostDraftContentForApproval;
	protected ImageButton ibApproveContent;
	protected ImageButton ibPublishContent = null;
	protected HyperLink lnkRejectContent;
	protected ImageButton ibCancelChanges;
	protected WorkflowStatusIcon statusIcon;

	#endregion

	private bool forbidModuleSettings = false;
	private bool enableWorkflow = false;
	private SiteModuleControl siteModule = null;
	private string siteRoot = string.Empty;
	private string columnId = UIHelper.CenterColumnId;

	#region deprecated properties

	//private string artHeader = UIHelper.ArtisteerPostMetaHeader;
	//private string artHeadingCss = UIHelper.ArtPostHeader;


	public bool UseJQueryUI { get; set; } = false;

	public bool RenderArtisteer { get; set; } = false;

	public bool UseLowerCaseArtisteerClasses { get; set; } = false;

	public bool UseH3ForSideHeader { get; set; } = false;

	public bool UseArtisteer3 { get; set; } = false;

	#endregion

	private string headingTag = "h2";

	/// <summary>
	/// only used when UseModuleHeading is false
	/// </summary>
	public string Element { get; set; } = "h2";

	/// <summary>
	/// only used when UseModuleHeadingOnSideColumns is false
	/// </summary>
	public string SideColumnElement { get; set; } = "h2";



	private string topContent = string.Empty;
	private string bottomContent = string.Empty;
	private string cssClassToUse = string.Empty;

	public bool DetectSideColumn { get; set; } = false;

	/// <summary>
	/// if true (default is true) use the heading element defined on the module
	/// else use the themeable property on this control
	/// </summary>
	public bool UseModuleHeading { get; set; } = true;

	/// <summary>
	/// if true (default is true) use the heading element defined on the module
	/// else use the themeable property on this control
	/// </summary>
	public bool UseModuleHeadingOnSideColumns { get; set; } = true;

	public string LiteralExtraTopContent { get; set; } = string.Empty;

	public string LiteralExtraBottomContent { get; set; } = string.Empty;

	public string SideColumnLiteralExtraTopContent { get; set; } = string.Empty;

	public string SideColumnLiteralExtraBottomContent { get; set; } = string.Empty;

	public string ExtraCssClasses { get; set; } = string.Empty;

	public string SideColumnExtraCssClasses { get; set; } = string.Empty;

	public string LiteralHeadingTopWrap { get; set; } = string.Empty;

	public string LiteralHeadingBottomWrap { get; set; } = string.Empty;

	public string LiteraSideColumnHeadingTopWrap { get; set; } = string.Empty;

	public string LiteralSideColumnHeadingBottomWrap { get; set; } = string.Empty;

	public bool WrapLinksInSpan { get; set; } = true;

	public Module ModuleInstance { get; set; } = null;

	public string LiteralExtraMarkup { get; set; } = string.Empty;

	public string EditUrl { get; set; } = string.Empty;

	public string EditText { get; set; } = string.Empty;

	public bool UseHeading { get; set; } = true;

	public bool DisabledModuleSettingsLink { get; set; } = false;

	public bool CanEdit { get; set; } = false;

	public bool IsAdminEditor { get; set; } = false;

	public bool ShowEditLinkOverride { get; set; } = false;

	public ContentWorkflowStatus WorkflowStatus { get; set; } = ContentWorkflowStatus.None;

	public bool RenderEditLinksInsideHeading { get; set; } = true;

	/// <summary>
	/// by default extra markup is only shown when IsEditable is true
	/// setting this property to true will make it show the extra markup 
	/// if there is any when the user does not have edit permission on the module
	/// </summary>
	public bool ForceShowExtraMarkup { get; set; } = false;


	private SiteModuleControl GetParentAsSiteModelControl(Control child)
	{
		if (HttpContext.Current is null || child.Parent is null)
		{
			return null;
		}
		else if (child.Parent is SiteModuleControl)
		{
			return child.Parent as SiteModuleControl;
		}
		else
		{
			return GetParentAsSiteModelControl(child.Parent);
		}
	}

	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current is null)
		{
			writer.Write($"[{ID}]");
			return;
		}

		if (UseHeading && !string.IsNullOrWhiteSpace(topContent))
		{
			writer.Write(topContent);
		}

		if (!UseHeading && ModuleInstance is not null)
		{
			// only need this when not rendering a heading element
			writer.Write(Invariant($"<a id=\"module{ModuleInstance.ModuleId}\" class=\"moduleanchor\"></a>"));
		}

		if (UseHeading && !string.IsNullOrWhiteSpace(headingTag))
		{
			writer.WriteBeginTag(headingTag);

			if (ModuleInstance is not null)
			{
				writer.WriteAttribute("id", Invariant($"module{ModuleInstance.ModuleId}"));
			}

			writer.WriteAttribute("class", $"{cssClassToUse} moduletitle");
			writer.Write(HtmlTextWriter.TagRightChar);
		}

		if (UseHeading && !string.IsNullOrWhiteSpace(LiteralHeadingTopWrap))
		{
			writer.Write(LiteralHeadingTopWrap);
		}

		litModuleTitle.RenderControl(writer);

		if (UseHeading && !string.IsNullOrWhiteSpace(LiteralHeadingBottomWrap))
		{
			writer.Write(LiteralHeadingBottomWrap);
		}

		if (RenderEditLinksInsideHeading)
		{
			if (CanEdit)
			{
				if (WrapLinksInSpan)
				{
					writer.Write("<span class=\"modulelinks\">");
				}
				if (!forbidModuleSettings)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					lnkModuleSettings.RenderControl(writer);
				}

				if (ibCancelChanges is not null && ibCancelChanges.Visible)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					ibCancelChanges.RenderControl(writer);
				}

				if (ibPostDraftContentForApproval is not null && ibPostDraftContentForApproval.Visible)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					ibPostDraftContentForApproval.RenderControl(writer);
				}

				if (lnkRejectContent is not null && lnkRejectContent.Visible)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					lnkRejectContent.RenderControl(writer);
				}

				if (ibApproveContent is not null && ibApproveContent.Visible)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					ibApproveContent.RenderControl(writer);
				}

				if (ibPublishContent is not null && ibPublishContent.Visible)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					ibPublishContent.RenderControl(writer);
				}

				if (!string.IsNullOrWhiteSpace(statusIcon.ToolTip))
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					statusIcon.RenderControl(writer);
				}

				if (
					lnkModuleEdit is not null
					&& !string.IsNullOrWhiteSpace(EditUrl)
					&& !string.IsNullOrWhiteSpace(EditText)
				)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					lnkModuleEdit.RenderControl(writer);
				}

				if (!string.IsNullOrWhiteSpace(LiteralExtraMarkup))
				{
					writer.Write(LiteralExtraMarkup);
				}

				if (WrapLinksInSpan)
				{
					writer.Write("</span>");
				}
			}
			else
			{
				//can't edit
				if (ForceShowExtraMarkup)
				{
					if (!string.IsNullOrWhiteSpace(LiteralExtraMarkup))
					{
						writer.Write(LiteralExtraMarkup);
					}
				}
			}
		}

		if (UseHeading && !string.IsNullOrWhiteSpace(headingTag))
		{
			writer.WriteEndTag(headingTag);
		}

		if (UseHeading && !string.IsNullOrWhiteSpace(bottomContent))
		{
			writer.Write(bottomContent);
		}

		if (!RenderEditLinksInsideHeading)
		{
			if (CanEdit)
			{
				writer.Write("<div class=\"edlinks\">");

				if (!forbidModuleSettings)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					lnkModuleSettings.RenderControl(writer);
				}

				if (ibCancelChanges is not null && ibCancelChanges.Visible)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					ibCancelChanges.RenderControl(writer);
				}

				if (ibPostDraftContentForApproval is not null && ibPostDraftContentForApproval.Visible)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					ibPostDraftContentForApproval.RenderControl(writer);
				}

				if (lnkRejectContent is not null && lnkRejectContent.Visible)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					lnkRejectContent.RenderControl(writer);
				}

				if (ibApproveContent is not null && ibApproveContent.Visible)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					ibApproveContent.RenderControl(writer);
				}

				if (ibPublishContent is not null && ibPublishContent.Visible)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					ibPublishContent.RenderControl(writer);
				}

				if (!string.IsNullOrWhiteSpace(statusIcon.ToolTip))
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					statusIcon.RenderControl(writer);
				}

				if (lnkModuleEdit is not null
					&& !string.IsNullOrEmpty(EditUrl)
					&& !string.IsNullOrEmpty(EditText)
				)
				{
					writer.Write(HtmlTextWriter.SpaceChar);
					lnkModuleEdit.RenderControl(writer);
				}

				writer.Write("</div>");

			} //can edit

			if (!string.IsNullOrWhiteSpace(LiteralExtraMarkup))
			{
				writer.Write(LiteralExtraMarkup);
			}
		}
	}


	void ibApproveContent_Click(object sender, ImageClickEventArgs e)
	{
		if (GetParentAsSiteModelControl(this) is IWorkflow workflow)
		{
			workflow.Approve();
		}
	}


	protected void ibPostDraftContentForApproval_Click(object sender, ImageClickEventArgs e)
	{
		if (GetParentAsSiteModelControl(this) is IWorkflow workflow)
		{
			workflow.SubmitForApproval();
		}
	}


	protected void ibCancelChanges_Click(object sender, ImageClickEventArgs e)
	{
		if (GetParentAsSiteModelControl(this) is IWorkflow workflow)
		{
			workflow.CancelChanges();
		}
	}


	protected override void OnPreRender(EventArgs e)
	{

		base.OnPreRender(e);
		if (HttpContext.Current is null)
		{
			return;
		}

		headingTag = WebConfigSettings.ModuleTitleTag;

		Initialize();

		if (DetectSideColumn)
		{
			columnId = this.GetColumnId();

			switch (columnId)
			{
				case UIHelper.LeftColumnId:
				case UIHelper.RightColumnId:
					topContent = SideColumnLiteralExtraTopContent;
					bottomContent = SideColumnLiteralExtraBottomContent;
					cssClassToUse = SideColumnExtraCssClasses;

					LiteralHeadingTopWrap = LiteraSideColumnHeadingTopWrap;
					LiteralHeadingBottomWrap = LiteralSideColumnHeadingBottomWrap;

					if (!UseModuleHeadingOnSideColumns)
					{
						headingTag = SideColumnElement;
					}
					break;

				case UIHelper.CenterColumnId:
				default:

					topContent = LiteralExtraTopContent;
					bottomContent = LiteralExtraBottomContent;
					cssClassToUse = ExtraCssClasses;

					if (!UseModuleHeading)
					{
						headingTag = Element;
					}
					break;
			}
		}
		else
		{
			topContent = LiteralExtraTopContent;
			bottomContent = LiteralExtraBottomContent;
			cssClassToUse = ExtraCssClasses;
			if (!UseModuleHeading)
			{
				headingTag = Element;
			}
		}
	}

	private void Initialize()
	{
		if (HttpContext.Current is null)
		{
			return;
		}

		siteModule = GetParentAsSiteModelControl(this);
		var useTextLinksForFeatureSettings = true;

		if (Page is mojoBasePage basePage)
		{
			useTextLinksForFeatureSettings = basePage.UseTextLinksForFeatureSettings;
		}

		if (siteModule is not null)
		{
			ModuleInstance = siteModule.ModuleConfiguration;
			CanEdit = siteModule.IsEditable;
			enableWorkflow = siteModule.EnableWorkflow;
			forbidModuleSettings = siteModule.ForbidModuleSettings;
		}

		if (ModuleInstance is not null)
		{
			headingTag = ModuleInstance.HeadElement;
			if (ModuleInstance.ShowTitle)
			{
				litModuleTitle.Text = Page.Server.HtmlEncode(ModuleInstance.ModuleTitle);
			}
			else
			{
				UseHeading = false;
			}

			if (CanEdit)
			{
				if (!DisabledModuleSettingsLink)
				{
					lnkModuleSettings.Visible = true;
					lnkModuleSettings.Text = Resource.SettingsLink;
					lnkModuleSettings.ToolTip = Resource.ModuleEditSettings;

					if (!useTextLinksForFeatureSettings)
					{
						lnkModuleSettings.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/" + WebConfigSettings.EditPropertiesImage);
					}
					else
					{
						// if its a text link make it small like the edit link
						lnkModuleSettings.CssClass = "ModuleEditLink";
					}

					siteRoot = SiteUtils.GetNavigationSiteRoot();

					lnkModuleSettings.NavigateUrl = "Admin/ModuleSettings.aspx".ToLinkBuilder().PageId(ModuleInstance.PageId).ModuleId(ModuleInstance.ModuleId).ToString();

					if ((enableWorkflow) && (siteModule is not null) && (siteModule is IWorkflow))
					{
						SetupWorkflowControls();
					}
				}
			}

			if (
				(CanEdit || ShowEditLinkOverride)
				&& EditText is not null && !string.IsNullOrWhiteSpace(EditUrl))
			{
				lnkModuleEdit.Text = EditText;
				if (!string.IsNullOrWhiteSpace(ToolTip))
				{
					lnkModuleEdit.ToolTip = ToolTip;
				}
				else
				{
					lnkModuleEdit.ToolTip = EditText;
				}
				lnkModuleEdit.NavigateUrl = EditUrl.ToLinkBuilder().PageId(ModuleInstance.PageId).ModuleId(ModuleInstance.ModuleId).ToString();

				if (!useTextLinksForFeatureSettings)
				{
					lnkModuleEdit.ImageUrl = Page.ResolveUrl($"~/Data/SiteImages/{WebConfigSettings.EditContentImage}");
				}
			}
		}
	}


	private void SetupWorkflowControls()
	{
		if (HttpContext.Current is null || siteModule == null || ModuleInstance == null)
		{
			return;
		}

		if ((Page is CmsPage cmsPage) && (cmsPage.ViewMode == PageViewMode.WorkInProgress))
		{
			switch (WorkflowStatus)
			{
				case ContentWorkflowStatus.Draft:

					ibPostDraftContentForApproval.ImageUrl = Page.ResolveUrl(WebConfigSettings.RequestApprovalImage);
					ibPostDraftContentForApproval.ToolTip = Resource.RequestApprovalToolTip;
					ibPostDraftContentForApproval.Visible = true;

					statusIcon.ToolTip = Resource.WorkflowDraft;

					if (WebConfigSettings.WorkflowShowPublishForUnSubmittedDraft)
					{
						if (
						(cmsPage.CurrentPage is not null)
						&& (IsAdminEditor || WebUser.IsInRoles(cmsPage.CurrentPage.EditRoles) || WebUser.IsInRoles(ModuleInstance.AuthorizedEditRoles)
						|| (WebConfigSettings.Use3LevelContentWorkflow && (WebUser.IsInRoles(cmsPage.CurrentPage.DraftApprovalRoles) || WebUser.IsInRoles(ModuleInstance.DraftApprovalRoles)))
						)
						)
						{
							ibApproveContent.ImageUrl = Page.ResolveUrl(WebConfigSettings.ApproveContentImage);
							ibApproveContent.Visible = true;
							ibApproveContent.ToolTip = Resource.ApproveContentToolTip;
						}
					}
					break;

				case ContentWorkflowStatus.AwaitingApproval:

					if (WebConfigSettings.Use3LevelContentWorkflow)
					{
						//disable edit link because draft is awaiting approval
						lnkModuleEdit.Visible = false;
					}

					if (
						(cmsPage.CurrentPage is not null)
						&& (IsAdminEditor || WebUser.IsInRoles(cmsPage.CurrentPage.EditRoles) || WebUser.IsInRoles(ModuleInstance.AuthorizedEditRoles)
						|| (WebConfigSettings.Use3LevelContentWorkflow && (WebUser.IsInRoles(cmsPage.CurrentPage.DraftApprovalRoles) || WebUser.IsInRoles(ModuleInstance.DraftApprovalRoles)))
						)
						)
					{
						if (WebConfigSettings.Use3LevelContentWorkflow)
						{
							//user can edit current draft awaiting approval
							lnkModuleEdit.Visible = true;
						}

						//add in the reject and approve links:                                            
						ibApproveContent.ImageUrl = Page.ResolveUrl(WebConfigSettings.ApproveContentImage);
						ibApproveContent.Visible = true;
						ibApproveContent.ToolTip = Resource.ApproveContentToolTip;

						lnkRejectContent.NavigateUrl = "Admin/RejectContent.aspx".ToLinkBuilder().ModuleId(ModuleInstance.ModuleId).PageId(ModuleInstance.PageId).ToString();
						lnkRejectContent.ImageUrl = Page.ResolveUrl(WebConfigSettings.RejectContentImage);
						lnkRejectContent.ToolTip = Resource.RejectContentToolTip;
						lnkRejectContent.Visible = true;
					}

					statusIcon.ToolTip = WebConfigSettings.Use3LevelContentWorkflow ? Resource.WorkflowAwaitingApproval3Level : Resource.WorkflowAwaitingApproval;
					break;

				case ContentWorkflowStatus.AwaitingPublishing:
					if (
						(cmsPage.CurrentPage is not null)
						&& (IsAdminEditor || WebUser.IsInRoles(cmsPage.CurrentPage.EditRoles) || WebUser.IsInRoles(ModuleInstance.AuthorizedEditRoles))
						)
					{
						ibPublishContent.ImageUrl = Page.ResolveUrl(WebConfigSettings.PublishContentImage);
						ibPublishContent.Visible = true;
						ibPublishContent.ToolTip = Resource.PublishContentToolTip;

						lnkRejectContent.NavigateUrl = "Admin/RejectContent.aspx".ToLinkBuilder().ModuleId(ModuleInstance.ModuleId).PageId(ModuleInstance.PageId).ToString();
						lnkRejectContent.ImageUrl = Page.ResolveUrl(WebConfigSettings.RejectContentImage);
						lnkRejectContent.ToolTip = Resource.RejectContentToolTip;
						lnkRejectContent.Visible = true;
					}

					statusIcon.ToolTip = Resource.WorkflowAwaitingPublishing;

					break;

				case ContentWorkflowStatus.ApprovalRejected:
					statusIcon.ToolTip = Resource.WorkflowRejected;
					break;
			}

			if (
				WorkflowStatus != ContentWorkflowStatus.Cancelled
				&& WorkflowStatus != ContentWorkflowStatus.Approved
				&& WorkflowStatus != ContentWorkflowStatus.None
				)
			{
				//allow changes to be cancelled:
				ibCancelChanges.ImageUrl = Page.ResolveUrl(WebConfigSettings.CancelContentChangesImage);
				ibCancelChanges.ToolTip = Resource.CancelChangesToolTip;
				ibCancelChanges.Visible = true;
			}
		}
	}


	protected override void CreateChildControls()
	{
		if (HttpContext.Current is null)
		{
			return;
		}

		litModuleTitle = new Literal();

		lnkModuleSettings = new HyperLink
		{
			CssClass = "modulesettingslink"
		};

		lnkModuleEdit = new HyperLink
		{
			CssClass = "ModuleEditLink",
			SkinID = "plain"
		};

		ibPostDraftContentForApproval = new ImageButton
		{
			ID = "lbPostDraftContentForApproval",
			CssClass = "jqtt ModulePostDraftForApprovalLink",
			SkinID = "plain",
			Visible = false
		};
		ibPostDraftContentForApproval.Click += new ImageClickEventHandler(ibPostDraftContentForApproval_Click);
		Controls.Add(ibPostDraftContentForApproval);

		ibApproveContent = new ImageButton
		{
			ID = "ibApproveContent",
			CssClass = "jqtt ModuleApproveContentLink",
			SkinID = "plain",
			Visible = false
		};
		ibApproveContent.Click += new ImageClickEventHandler(ibApproveContent_Click);
		Controls.Add(ibApproveContent);

		if (WebConfigSettings.Use3LevelContentWorkflow)
		{
			ibPublishContent = new ImageButton
			{
				ID = "ibPublishContent",
				CssClass = "jqtt ModulePublishContentLink",
				SkinID = "plain",
				Visible = false
			};
			ibPublishContent.Click += new ImageClickEventHandler(ibApproveContent_Click); //approve and publish are the same at this point so we have only one method
			Controls.Add(ibPublishContent);
		}

		lnkRejectContent = new HyperLink
		{
			ID = "ibRejectContent",
			CssClass = "jqtt ModuleRejectContentLink",
			SkinID = "plain",
			Visible = false
		};

		ibCancelChanges = new ImageButton
		{
			ID = "ibCancelChanges",
			CssClass = "jqtt ModuleCancelChangesLink",
			SkinID = "plain",
			Visible = false
		};
		UIHelper.AddConfirmationDialog(ibCancelChanges, Resource.CancelContentChangesButtonWarning);
		ibCancelChanges.Click += new ImageClickEventHandler(ibCancelChanges_Click);
		Controls.Add(ibCancelChanges);

		statusIcon = new WorkflowStatusIcon();
		Controls.Add(statusIcon);
	}
}
