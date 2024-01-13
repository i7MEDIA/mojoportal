//  Author:                     
//  Created:                    2014-02-15
//	Last Modified:              2018-03-28



using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.AdminUI
{
	public partial class PageManager : NonCmsBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(PageManager));

		private bool isAdmin = false;
		private bool isContentAdmin = false;
		private bool isSiteEditor = false;
		private bool canEditAnything = false;
		private int selectedPage = -1;
		//private ArrayList sitePages = new ArrayList();
		//private SiteMapDataSource siteMapDataSource;
		//private bool userCanAddPages = false;
		//protected string EditContentImage = WebConfigSettings.EditContentImage;
		//protected string EditPropertiesImage = WebConfigSettings.EditPropertiesImage;
		//protected string DeleteLinkImage = WebConfigSettings.DeleteLinkImage;

		private bool promptOnDelete = true;
		private bool promptOnMove = true;
		private bool promptOnSort = true;
		private bool showAltPageManagerLink = false;
		//private bool showDemoInfo = false;
		private string productUrl = string.Empty;
		private bool linkToViewPermissions = true;
		


		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}

			LoadSettings();

			//PopulatePageArray();
			PopulateLabels();
			PopulateControls();

			SetupScript();
			AddCss();
		}


		private void PopulateControls()
		{
			//BindListBox();

		}


		private void LoadSettings()
		{
			isAdmin = WebUser.IsAdmin;
			isContentAdmin = WebUser.IsContentAdmin;
			isSiteEditor = SiteUtils.UserIsSiteEditor();

			canEditAnything = (isAdmin || isContentAdmin || isSiteEditor);

			selectedPage = WebUtils.ParseInt32FromQueryString("selpage", -1);

			promptOnDelete = mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("PageManager:PromptOnDelete", promptOnDelete);
			promptOnMove = mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("PageManager:PromptOnMove", promptOnMove);
			promptOnSort = mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("PageManager:PromptOnSort", promptOnSort);
			showAltPageManagerLink = mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("PageManager:ShowAltPageManagerLink", showAltPageManagerLink);
			//showDemoInfo = mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("PageManager:ShowDemoInfo", showDemoInfo);
			productUrl = mojoPortal.Core.Configuration.ConfigHelper.GetStringProperty("PageManager:ProductUrl", productUrl);
			linkToViewPermissions = mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("PageManager:LinkToViewPermissions", linkToViewPermissions);


			AddClassToBody("administration");
			AddClassToBody("pagemanager");


		}

	   

		private void SetupScript()
		{
			//http://mbraak.github.io/jqTree/index.html#jqtree


			ScriptManager.RegisterClientScriptBlock(this, typeof(Page),
						"jqtreemain", "\n<script src=\""
						+ Page.ResolveUrl("~/ClientScript/jqmojo/tree.jquery.js") + "\" type=\"text/javascript\"></script>", false);

			string dataUrl = SiteRoot + "/Services/SiteMapJson.ashx?cmd=pm";

			StringBuilder script = new StringBuilder();

			script.Append("\n<script type=\"text/javascript\">\n");


			script.Append("$(function() {");


			script.Append("var $tree = $('#tree1'); ");
			script.Append("var $selPageId = $('#hdnSelPage'); ");
			script.Append("var $cmdBar = $('#ulCommands'); ");
			script.Append("var $pageLabel = $('#liInfo'); ");
			script.Append("var $liEdit = $('#liEdit'); ");
			script.Append("var $liSettings = $('#liSettings'); ");
			script.Append("var $liPermissions = $('#liPermissions'); ");
			script.Append("var $liView = $('#liView'); ");
			script.Append("var $liSort = $('#liSort'); ");
			script.Append("var $liNewChild = $('#liNewChild'); ");
			script.Append("var $liDeletePage = $('#liDeletePage'); ");

			script.Append("var $btnEdit = $('#lnkEdit'); ");
			script.Append("var $btnSettings = $('#lnkSettings'); ");
			script.Append("var $btnPermissions = $('#lnkPermissions'); ");
			script.Append("var $btnView = $('#lnkView'); ");
			script.Append("var $btnSort = $('#lnkSort'); ");
			script.Append("var $btnNewPage = $('#lnkNewChild'); ");
			script.Append("var $btnDelete = $('#lnkDeletePage'); ");
			script.Append("var $spnPermissions = $('#spnPermissions'); ");


			script.Append("var $showCommands = function(node) {");
			//script.Append("alert(node.name); ");
			script.Append("$selPageId.val(node.id);");
			script.Append("$pageLabel.html(node.name);");

			script.Append("var $url = '" + SiteRoot + "/Admin/PageLayout.aspx?pageid=' + node.id;");
			script.Append("$btnEdit.attr('href', $url);");
			script.Append("$url = '" + SiteRoot + "/Admin/PageSettings.aspx?pageid=' + node.id;");
			script.Append("$btnSettings.attr('href', $url);");
			script.Append("$btnView.attr('href', node.Url);");
			script.Append("$url = '" + SiteRoot + "/Admin/PageSettings.aspx?start=' + $selPageId.val();");
			script.Append("$btnNewPage.attr('href', $url);");

			if (linkToViewPermissions)
			{
				script.Append("$url = '" + SiteRoot + "/Admin/PagePermission.aspx?pageid=' + $selPageId.val() + '&p=v';");
				script.Append("$btnPermissions.attr('href', $url);");
			}
			else
			{
				script.Append("$url = '" + SiteRoot + "/Admin/PagePermissionsMenu.aspx?pageid=' + $selPageId.val();");
				script.Append("$btnPermissions.attr('href', $url);");
			}
			
			
			

			//PagePermissionsMenu.aspx

			script.Append("$cmdBar.show();");
			script.Append("var $liActive = $('li.jqtree-selected').last();");

			script.Append("$cmdBar.position({ my:\"left top\",at:\"right+10 top\",");
			script.Append("of: $liActive");
			script.Append(",collision:\"none\"");
			script.Append("});");

			script.Append("if(node.childcount > 1) {");
			script.Append("$liSort.show();");
			script.Append("} else {");
			script.Append("$liSort.hide();");
			script.Append("}; "); // end if(node.childcount > 1)

			script.Append("if(node.canEdit) {");
			script.Append("$btnEdit.show();");
			script.Append("$btnSettings.show();");
			if (canEditAnything)
			{
				script.Append("$liPermissions.show();");
				script.Append("$spnPermissions.html(node.protection);");
			}
			else
			{
				script.Append("$liPermissions.hide();");
			}
			script.Append("");
			script.Append("} else {");
			script.Append("$btnEdit.hide();");
			script.Append("$btnSettings.hide();");
			script.Append("$liPermissions.hide();");
			script.Append("}; "); // end CanEdit

			script.Append("if(node.canDelete) {");
			script.Append("$btnDelete.show();");
			script.Append("} else {");
			script.Append("$btnDelete.hide();");
			script.Append("}; "); // end CanEdit

			script.Append("if(node.canCreateChild) {");
			script.Append("$btnNewPage.show();");
			script.Append("} else {");
			script.Append("$btnNewPage.hide();");
			script.Append("}; "); // end CanEdit
			

			script.Append("}; "); // end showCommands

			script.Append("var $moveNode = function(movedNode, targetNode, previousParent, position) {");




			//script.Append("alert('you moved ' + movedNode.id);");

			//post to server and get result

			script.Append("var obj = {};");
			script.Append("obj['movedNode'] = movedNode.id; ");
			script.Append("obj['targetNode'] = targetNode.id; ");
			script.Append("obj['previousParent'] = previousParent.id; ");
			script.Append("obj['position'] = position; ");

			script.Append("var moveResult = false; ");

			script.Append("$.ajax({");
			script.Append("type:\"POST\",");
			script.Append("async:false,");
			script.Append("processData: true,");
			//script.Append("contentType: false,");
			script.Append("dataType: \"json\",");

			string moveUrl = SiteRoot + "/Services/SiteMapJson.ashx?cmd=move";

			script.Append("url:'" + moveUrl + "',");
			script.Append("data: obj,");
			script.Append("success: function(data, textStatus, jqXHR) { ");
			script.Append("if(data.Success) { moveResult = true; } else {");
			script.Append("alert(data.Message); }");

			script.Append("}, "); //end success
			script.Append("complete: function(jqXHR, textStatus) { ");

			//script.Append("alert('complete');");
			//script.Append("return false;");

			script.Append("}, "); //end complete

			script.Append("error:function(jqXHR, textStatus, errorThrown ) {");

			script.Append("alert(errorThrown);");

			script.Append("} "); //end error
			script.Append("});"); //end ajax

			script.Append("");
			script.Append("");
			script.Append("");
			script.Append("");
			script.Append("");

			script.Append("return moveResult;");
			script.Append("}; "); // end moveNode
			

			//script.Append("$btnEdit.click(function(e) {");
			//script.Append("e.preventDefault();");
			////script.Append("alert('edit page ' + $selPageId.val());");
			//script.Append("var url = '" + SiteRoot + "/Admin/PageLayout.aspx?pageid=' + $selPageId.val();");
			//script.Append("window.location.href = url;");
			//script.Append("});");

			//script.Append("$btnSettings.click(function(e) {");
			//script.Append("e.preventDefault();");
			////script.Append("alert('edit settings page ' + $selPageId.val());");
			//script.Append("var url = '" + SiteRoot + "/Admin/PageSettings.aspx?pageid=' + $selPageId.val();");
			//script.Append("window.location.href = url;");
			//script.Append("});");

			//script.Append("$btnView.click(function(e) {");
			//script.Append("e.preventDefault();");
			////script.Append("alert('view page ' + $selPageId.val());");
			//script.Append("var node = $tree.tree('getNodeById', $selPageId.val());");
			//script.Append("window.location.href = node.Url;");
			//script.Append("});");

			script.Append("$btnSort.click(function(e) {");


			script.Append("e.preventDefault();");

			if (promptOnSort)
			{
				script.Append("if (confirm('" + HttpUtility.HtmlAttributeEncode(PageManagerResources.SortAlphaPrompt) + "')) {");
			}
			

			//script.Append("alert('about to sort child pages ' + $selPageId.val());");
			script.Append("var node = $tree.tree('getNodeById', $selPageId.val());");
			script.Append("var objSort = {};");
			script.Append("objSort['selNode'] = node.id; ");

			script.Append("$.ajax({");
			script.Append("type:\"POST\",");
			script.Append("async:false,");
			script.Append("processData: true,");
			//script.Append("contentType: false,");
			script.Append("dataType: \"json\",");

			string sortUrl = SiteRoot + "/Services/SiteMapJson.ashx?cmd=sortalpha";

			script.Append("url:'" + sortUrl + "',");
			script.Append("data: objSort,");
			script.Append("success: function(data, textStatus, jqXHR) { ");
			script.Append("if(data.Success) { ");

			// reload the node
			script.Append("$('#tree1').tree('loadDataFromUrl', node); ");

			script.Append(" } else {");
			script.Append("alert(data.Message); }");

			script.Append("}, "); //end success
			script.Append("complete: function(jqXHR, textStatus) { ");

			//script.Append("alert('complete');");
			//script.Append("return false;");

			script.Append("}, "); //end complete

			script.Append("error:function(jqXHR, textStatus, errorThrown ) {");

			script.Append("alert(errorThrown);");

			script.Append("} "); //end error
			script.Append("});"); //end ajax



			if (promptOnSort)
			{
				script.Append("} "); //end if confirm
			}

			//

			script.Append("});");

			//script.Append("$btnNewPage.click(function(e) {");
			//script.Append("e.preventDefault();");
			////script.Append("alert('new child page ' + $selPageId.val());");
			//script.Append("var url = '" + SiteRoot + "/Admin/PageSettings.aspx?start=' + $selPageId.val();");
			//script.Append("window.location.href = url;");
			//script.Append("});");

			script.Append("$btnDelete.click(function(e) {");
			script.Append("e.preventDefault();");
			script.Append("var node = $tree.tree('getNodeById', $selPageId.val());");
			if (promptOnDelete)
			{
				script.Append("var doDelete = false; ");
				script.Append("if(node.childcount > 0) {");
				script.Append("if (confirm('" + HttpUtility.HtmlAttributeEncode(PageManagerResources.DeletePageWithChildrenWarning) + "')) {");
				script.Append("doDelete = true; ");
				script.Append("} ");

				//script.Append("alert('really delete page ' + node.id + ' and orphan the children?');");

				script.Append("} else {");

				script.Append("if (confirm('" + HttpUtility.HtmlAttributeEncode(PageManagerResources.DeletePageWarning) + "')) {");
				script.Append("doDelete = true; ");
				script.Append("} ");

				//script.Append("alert('really delete page ' + node.id);");
				script.Append("}"); //end else nodecount
			}
			else
			{
				script.Append("var doDelete = true; ");
			}
			

			script.Append("if(doDelete) {");
			//script.Append("alert('about to delete page ' + node.id);");

			script.Append("var objDel = {};");
			script.Append("objDel['delNode'] = node.id; ");

			script.Append("$.ajax({");
			script.Append("type:\"POST\",");
			script.Append("async:false,");
			script.Append("processData: true,");
			//script.Append("contentType: false,");
			script.Append("dataType: \"json\",");

			string delUrl = SiteRoot + "/Services/SiteMapJson.ashx?cmd=del";

			script.Append("url:'" + delUrl + "',");
			script.Append("data: objDel,");
			script.Append("success: function(data, textStatus, jqXHR) { ");
			script.Append("if(data.Success) { ");

			// remove the node from the tree
			script.Append("$('#tree1').tree('removeNode', node); ");

			script.Append(" } else {");
			script.Append("alert(data.Message); }");

			script.Append("}, "); //end success
			script.Append("complete: function(jqXHR, textStatus) { ");

			//script.Append("alert('complete');");
			//script.Append("return false;");

			script.Append("}, "); //end complete

			script.Append("error:function(jqXHR, textStatus, errorThrown ) {");

			script.Append("alert(errorThrown);");

			script.Append("} "); //end error
			script.Append("});"); //end ajax


			script.Append("} ");

			script.Append("});");


			script.Append(" $('#tree1').tree({");

			script.Append("dataUrl:'" + dataUrl + "'");
			script.Append(",dragAndDrop:true");
			script.Append(",onLoadFailed: function(response) {");
			script.Append("alert(response);");
			script.Append("}");

			//script.Append(",onCreateLi: function(node, $li) {");
			//script.Append("$li.find('.jqtree-element').append(");
			//script.Append("'<a href=\"#node-'+ node.id +'\" class=\"edit\" data-node-id=\"'+ node.id +'\">edit</a>'");
			//script.Append(");");
			//script.Append("}"); // end onCreateLi

			script.Append("});"); // end tree




			script.Append("$('#tree1').bind(");
			script.Append("'tree.click',");
			script.Append("function(event) {");
			//script.Append("event.preventDefault();");
			script.Append("var node = event.node;");
			//script.Append("alert(node.name + ' was clicked');");
		   
			script.Append("}");
			script.Append(");"); // end bind tree.click


			script.Append("$('#tree1').bind(");
			script.Append("'tree.dblclick',");
			script.Append("function(event) {");
			script.Append("console.log(e.node);");
			script.Append("}");
			script.Append(");"); // end bind tree.dblclick


			script.Append("$('#tree1').bind(");
			script.Append("'tree.select',");
			script.Append("function(event) {");
			script.Append("if (event.node) {");
			
			//script.Append("var node = event.node;");
			//script.Append("$selPageId.val(node.id);");
			//script.Append("alert(node.name + ' selected');");
			script.Append("$showCommands(event.node); ");

			//script.Append("if(event.previous_node) {");

			//script.Append("} ");

			script.Append("} else {");
			// event.node is null
			// a node was deselected
			// e.previous_node contains the deselected node
			script.Append("var node = event.previous_node;");
			script.Append("$selPageId.val(-1);");
			script.Append("$cmdBar.hide();");
			//script.Append("alert(node.name + ' deselected');");
			script.Append("}");
			script.Append("}");
			script.Append(");"); //end tree.select


			script.Append("$('#tree1').bind(");
			script.Append("'tree.contextmenu',");
			script.Append("function(event) {");
			script.Append("var node = event.node;");
		   // script.Append("alert(node.name + ' context');");
			script.Append("}");
			script.Append(");"); //end tree.contextmenu

			script.Append("$('#tree1').bind(");
			script.Append("'tree.move',");
			script.Append("function(event) {");
			script.Append("event.preventDefault();");
			if (promptOnMove)
			{
				script.Append("if (confirm('" + HttpUtility.HtmlAttributeEncode(PageManagerResources.MovePageConfirmPrompt) + "')) {");
			}
			

			// if did move it on the server
			script.Append("if($moveNode(event.move_info.moved_node, event.move_info.target_node, event.move_info.previous_parent,event.move_info.position)) {");
			script.Append("event.move_info.do_move();"); // this moves it in the ui

			//script.Append("console.log('moved_node', event.move_info.moved_node);");
			//script.Append("console.log('target_node', event.move_info.target_node);");
			//script.Append("console.log('position', event.move_info.position);");
			//script.Append("console.log('previous_parent', event.move_info.previous_parent);");
			
			script.Append("}"); //end if $moveNode

			if (promptOnMove)
			{
				script.Append("}"); //end if prompt
			}
			script.Append("}");
			script.Append(");"); //end bind tree.move

			script.Append("$('#tree1').bind(");
			script.Append("'tree.init',");
			script.Append("function() {");

			script.Append("}");
			script.Append(");"); //end bind tree.init

			script.Append("$('#tree1').bind(");
			script.Append("'tree.open',");
			script.Append("function(event) {");
			//script.Append("console.log(event.node);");
			//script.Append("var node = event.node;");
			//script.Append("if(node.children.length < node.childcount) {");
			//script.Append("alert(node.id);");

			//script.Append("$('#tree1').tree('loadDataFromUrl', '" + dataUrl + "?node=' + node.id, node);");

			//script.Append("}");

			//script.Append("alert('open');");
			//script.Append("$showCommands(); ");
			script.Append("$tree.tree('selectNode', null);"); //deselect
			script.Append("$tree.tree('selectNode', event.node);");

			script.Append("}");
			script.Append(");"); //end bind tree.open

			script.Append("$('#tree1').bind(");
			script.Append("'tree.close',");
			script.Append("function(event) {");
			script.Append("$tree.tree('selectNode', null);"); //deselect
			script.Append("$tree.tree('selectNode', event.node);");
			//script.Append("console.log(event.node);");
			script.Append("}");
			script.Append(");"); //end bind tree.close


			//script.Append("$tree.on(");
			//script.Append("'click', '.edit',");
			//script.Append("function(e) {");
			//script.Append("var node_id = $(e.target).data('node-id');");
			//script.Append("var node = $tree.tree('getNodeById', node_id);");
			//script.Append("if (node) {");
			//script.Append("alert(node.name + ' edit');");
			//script.Append("}");
			//script.Append("}");
			//script.Append(");"); //end click edit




			script.Append("});"); // end self exe function


			script.Append("\n</script>");

			ScriptManager.RegisterStartupScript(this, typeof(Page), "stspagemanager", script.ToString(), false);


		}

		private void AddCss()
		{
			if (IsPostBack) { return; }

			if (Page.Header.FindControl("jqtreecss") == null)
			{
				string pageManagerCss = mojoPortal.Core.Configuration.ConfigHelper.GetStringProperty("PageManager:TreeCss", "~/Data/style/jqtree.css");

				Literal cssLink = new Literal();
				cssLink.ID = "jqtreecss";
				cssLink.Text = "\n<link href='"
				+ Page.ResolveUrl(pageManagerCss)
				+ "' type='text/css' rel='stylesheet' media='screen' />";

				Page.Header.Controls.Add(cssLink);
			}

		}


		private void PopulateLabels()
		{
		   

			Title = SiteUtils.FormatPageTitle(siteSettings, PageManagerResources.PageManager);

			heading.Text = PageManagerResources.PageManager;
			if ((!isAdmin) && (!isSiteEditor) && (!isContentAdmin))
			{
				lnkNewPage.Visible = false;
				divAdminLinks.Visible = false;
			   

			}

			litInstructions.Text = PageManagerResources.PageManagerInstructions;


			lnkAdminMenu.Text = PageManagerResources.Administration;
			lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

			lnkPageTree.Text = PageManagerResources.PageManager;
			lnkPageTree.NavigateUrl = SiteRoot + "/Admin/PageManager.aspx";

			lnkAltPageManager.Text = PageManagerResources.StandardPageManager;
			lnkAltPageManager.NavigateUrl = SiteRoot + "/Admin/PageTree.aspx";
			if(showAltPageManagerLink)
			{
				lnkAltPageManager.Visible = true;
				altPmSeparator.Visible = true;
			}

			lnkNewPage.InnerText = PageManagerResources.NewRootPage;
			lnkNewPage.HRef = Page.ResolveUrl(SiteRoot + "/Admin/PageSettings.aspx");
			lnkNewPage.Visible = canEditAnything || WebUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages);

			
			litEdit.Text = PageManagerResources.EditPage;
			litSettings.Text = PageManagerResources.EditSettings;
			litPermissions.Text = PageManagerResources.EditPermissions;
			litView.Text = PageManagerResources.ViewPage;
			litSort.Text = PageManagerResources.SortPagesAlpha;
			litNewChild.Text = PageManagerResources.NewChildPage;
			litDeletePage.Text = PageManagerResources.DeletePage;

			//if(showDemoInfo)
			//{
			//    litDemoInfo.Visible = true;
			//    if(productUrl.Length > 0)
			//    {
			//        litDemoInfo.Text = "This is a demo of <a href='" + productUrl + "'>Page Manager Pro</a>, an add on product available in the mojoPortal Store.";
			//    }
			//    else
			//    {
			//        litDemoInfo.Text = "This is a demo of Page Manager Pro, an add on product available in the mojoPortal Store.";
			//    }
			//}
		}

		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(this.Page_Load);

			bool suppressMainMenu = mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("PageManager:SuppressMainMenu", false);
			bool suppressPageMenu = mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("PageManager:SuppressPageMenu", true);
		   
			SuppressMenuSelection();

			if (suppressMainMenu)
			{
				SuppressAllMenus();
			}
			if (suppressPageMenu)
			{
				SuppressPageMenu();
			}
			
		}







		#endregion
	}
}