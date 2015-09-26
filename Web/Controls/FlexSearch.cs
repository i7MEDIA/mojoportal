//	Author:         Jamie Eubanks
//	Created:        2014-05-09
//	Last Modified:  2014-05-16
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.		

using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
   /// <summary>
   /// a lightweight, flexible container alternative to mojoPortal SearchInput control
   /// </summary>
   public class FlexSearch : WebControl
   {
      protected TextBox tb;
      protected mojoButton mb;
      protected LinkButton lb;

      private string actJSVarname = "$mpfs_act";
      private string actionSubmitCssClass = "mpfs_act";

      private string srchContJSVarname = "$mpfs_frm";
      private string searchContainerSubmitCssClass = "mpfs_frm";

      private bool linkOnly = true;
      /// <summary>
      /// Set false to render a search text input
      /// </summary>
      public bool LinkOnly
      {
         get { return linkOnly; }
         set { linkOnly = value; }
      }

      private string outerContainerElement = string.Empty;
      /// <summary>
      /// Overall outer container element
      /// </summary>
      public string OuterContainerElement
      {
         get { return outerContainerElement; }
         set { outerContainerElement = value; }
      }

      private string outerContainerCssClass = string.Empty;
      /// <summary>
      /// CSS class for outer container
      /// </summary>
      public string OuterContainerCssClass
      {
         get { return outerContainerCssClass; }
         set { outerContainerCssClass = value; }
      }

      private string searchContainerElement = string.Empty;
      /// <summary>
      /// Inner container element, enclosing link/button and/or input
      /// This is required to create JavaScript search action when pressing enter
      /// </summary>
      public string SearchContainerElement
      {
         get { return searchContainerElement; }
         set { searchContainerElement = value; }
      }

      private string searchContainerCssClass = string.Empty;
      /// <summary>
      /// CSS class for inner enclosing container
      /// </summary>
      public string SearchContainerCssClass
      {
         get { return searchContainerCssClass; }
         set { searchContainerCssClass = value; }
      }

      private string inputContainerElement = string.Empty;
      /// <summary>
      /// Container element for input
      /// </summary>
      public string InputContainerElement
      {
         get { return inputContainerElement; }
         set { inputContainerElement = value; }
      }

      private string inputContainerCssClass = string.Empty;
      /// <summary>
      /// CSS class for input's container
      /// </summary>
      public string InputContainerCssClass
      {
         get { return inputContainerCssClass; }
         set { inputContainerCssClass = value; }
      }

      private string inputPlaceHolderText = string.Empty;
      /// <summary>
      /// HTML5 placeholder text for input element
      /// </summary>
      public string InputPlaceHolderText
      {
         get { return inputPlaceHolderText; }
         set { inputPlaceHolderText = value; }
      }

      private string actionContainerElement = string.Empty;
      /// <summary>
      /// Link/button containing element
      /// </summary>
      public string ActionContainerElement
      {
         get { return actionContainerElement; }
         set { actionContainerElement = value; }
      }

      private string actionContainerCssClass = string.Empty;
      /// <summary>
      /// CSS class for Link/button container
      /// </summary>
      public string ActionContainerCssClass
      {
         get { return actionContainerCssClass; }
         set { actionContainerCssClass = value; }
      }

      private string actionText = string.Empty;
      /// <summary>
      /// Link/button text (overrides value from Resources)
      /// </summary>
      public string ActionText
      {
         get { return actionText; }
         set { actionText = value; }
      }

      private string actionCssClass = string.Empty;
      /// <summary>
      /// Link/button CSS class
      /// </summary>
      public string ActionCssClass
      {
         get { return actionCssClass; }
         set { actionCssClass = value; }
      }

      private bool renderAsButton = false;
      /// <summary>
      /// Set true to render the output as a button, rather than a link
      /// </summary>
      public bool RenderAsButton
      {
         get { return renderAsButton; }
         set { renderAsButton = value; }
      }

      private bool hideOnSearchResultsPage = true;
      /// <summary>
      /// Set false to show this control on the Search Results page. Can also be overridden by web.config setting ShowSkinSearchInputOnSearchResults
      /// </summary>
      public bool HideOnSearchResultsPage
      {
         get { return hideOnSearchResultsPage; }
         set { hideOnSearchResultsPage = value; }
      }

      private bool hideOnSiteSettingsPage = true;
      /// <summary>
      /// Set false to show this control on the Site Settings page. Can also be overridden by web.config setting ShowSearchInputOnSiteSettings
      /// </summary>
      public bool HideOnSiteSettingsPage
      {
         get { return hideOnSiteSettingsPage; }
         set { hideOnSiteSettingsPage = value; }
      }

      private bool hideOnLoginPage = true;
      /// <summary>
      /// Set false to show this control on the Login page.
      /// </summary>
      public bool HideOnLoginPage
      {
         get { return hideOnLoginPage; }
         set { hideOnLoginPage = value; }
      }

      private bool hideOnRegistrationPage = true;
      /// <summary>
      /// Set false to show this control on the Registration page.
      /// </summary>
      public bool HideOnRegistrationPage
      {
         get { return hideOnRegistrationPage; }
         set { hideOnRegistrationPage = value; }
      }

      private bool hideOnPasswordRecoveryPage = true;
      /// <summary>
      /// Set false to show this control on the Password Recovery and/or Password Reset pages.
      /// </summary>
      public bool HideOnPasswordRecoveryPage
      {
         get { return hideOnPasswordRecoveryPage; }
         set { hideOnPasswordRecoveryPage = value; }
      }

      private bool renderAsListItem = false;
      /// <summary>
      /// Set true to render in backward compatibility mode for use in "topnav" links
      /// </summary>
      public bool RenderAsListItem
      {
         get { return renderAsListItem; }
         set { renderAsListItem = value; }
      }

      private Guid featureGuid = Guid.Empty;
      /// <summary>
      /// used when you want to make a search input that only searches a single feature
      /// </summary>
      public Guid FeatureGuid
      {
         get { return featureGuid; }
         set { featureGuid = value; }
      }

      protected override void Render(HtmlTextWriter writer)
      {
         if (!ShouldRender())
         { 
            return; 
         }
         else
         {
             if (renderAsListItem)
             {
                 outerContainerElement = "li";
                 outerContainerCssClass = "topnavitem";
                 actionCssClass = "sitelink";
             }
             writer.Write(OpenContainer(outerContainerElement, outerContainerCssClass));
             writer.Write(OpenContainer(searchContainerElement, searchContainerCssClass));
             if (!linkOnly)
             {
                 writer.Write(OpenContainer(inputContainerElement, inputContainerCssClass));
                 if (tb != null && tb.Visible)
                 {
                     tb.RenderControl(writer);
                 }
                 writer.Write(CloseContainer(inputContainerElement));
             }
             writer.Write(OpenContainer(actionContainerElement, actionContainerCssClass));
             if (renderAsButton)
             {
                 if (mb != null && mb.Visible)
                 {
                     mb.RenderControl(writer);
                 }
             }
             else
             {
                 if (lb != null && lb.Visible)
                 {
                     lb.RenderControl(writer);
                 }
             }
             writer.Write(CloseContainer(actionContainerElement));
             writer.Write(CloseContainer(searchContainerElement));
             writer.Write(CloseContainer(outerContainerElement));
         }
      }

      protected string OpenContainer(string element, string cssClass)
      {
         if (string.IsNullOrEmpty(element)) return string.Empty;

         string tagText = "<" + element;

         if (cssClass != string.Empty) tagText += " class='" + cssClass + "'";
         tagText += ">";
         return tagText;
      }

      protected string CloseContainer(string element)
      {
          if (string.IsNullOrEmpty(element)) return string.Empty;
          
          return "</" + element + ">";
      }

      private bool ShouldRender()
      {
         if (renderAsListItem) return true;

         if (hideOnSearchResultsPage 
             && (Page.Request.CurrentExecutionFilePath.IndexOf("SearchResults.aspx", StringComparison.InvariantCultureIgnoreCase) >= 0)
             && (!WebConfigSettings.ShowSkinSearchInputOnSearchResults))
            return false;

         if (hideOnSiteSettingsPage
             && (Page.Request.CurrentExecutionFilePath.IndexOf("SiteSettings.aspx", StringComparison.InvariantCultureIgnoreCase) >= 0)
             && (!WebConfigSettings.ShowSearchInputOnSiteSettings))
            return false;

         if (hideOnLoginPage
             && (Page.Request.CurrentExecutionFilePath.IndexOf("Login.aspx", StringComparison.InvariantCultureIgnoreCase) >= 0))
            return false;

         if (hideOnRegistrationPage
             && (Page.Request.CurrentExecutionFilePath.IndexOf("Register.aspx", StringComparison.InvariantCultureIgnoreCase) >= 0))
            return false;

         if (hideOnPasswordRecoveryPage
             && ((Page.Request.CurrentExecutionFilePath.IndexOf("RecoverPassword.aspx", StringComparison.InvariantCultureIgnoreCase) >= 0)
                  || (Page.Request.CurrentExecutionFilePath.IndexOf("PasswordReset.aspx", StringComparison.InvariantCultureIgnoreCase) >= 0)))
            return false;

         return true;
      }

      protected void mbFS_Click(object sender, EventArgs e)
      {
         DoRedirectToSearchResults();
      }

      protected void lbFS_Click(object sender, EventArgs e)
      {
         DoRedirectToSearchResults();
      }

      private void SetUpDefaultClick()
      {
          // Add form and submit CSS classes
          if (String.IsNullOrEmpty(actionCssClass))
          {
              actionCssClass = actionSubmitCssClass;
          }
          else
          {
              actionCssClass += " " + actionSubmitCssClass;
          }

          if (String.IsNullOrEmpty(searchContainerCssClass))
          {
              searchContainerCssClass = searchContainerSubmitCssClass;
          }
          else
          {
              searchContainerCssClass += " " + searchContainerSubmitCssClass;
          }

          // Register JQuery function to handle enter key triggering the action
          // http://www.sentia.com.au/blog/fixing-the-enter-key-in-aspnet-with-jquery
          mojoBasePage basePage = Page as mojoBasePage;
          if (basePage != null)
          {
              basePage.ScriptConfig.IncludeColorBox = true;

              StringBuilder script = new StringBuilder();

//      private string actionJavaScriptVarname = "mpfs_act";
//      private string searchContainerJavaScriptVarname = "mpfs_frm";
              script.Append("$(document).ready(function() {");
              script.Append(" var " + actJSVarname + " = $('." + actionSubmitCssClass + "');");
              script.Append(" var " + srchContJSVarname + " = " + actJSVarname + ".parents('." + searchContainerSubmitCssClass + "');");
              script.Append(" " + srchContJSVarname + ".keypress(function(e) {");
              script.Append(" if (e.which == 13 && e.target.type != 'textarea') {");
              script.Append(" if (" + actJSVarname + "[0].type == 'submit')");
              script.Append(" " + actJSVarname + "[0].click();");
              script.Append(" else");
              script.Append(" eval(" + actJSVarname + "[0].href);");
              script.Append(" return false;");
              script.Append(" }");
              script.Append(" });");
              script.Append(" });");

              ScriptManager.RegisterStartupScript(this, typeof(Page),
                 "info-init", "\n<script type=\"text/javascript\" >"
                 + script.ToString() + "</script>", false);
          }
      }




      private void DoRedirectToSearchResults()
      {
         string featureFilter = string.Empty;
         string searchText = string.Empty;
         if (tb.Text.Length > 0)
         {
            searchText = "?q=" + Page.Server.UrlEncode(tb.Text);
            if (featureGuid != Guid.Empty)
            {
               featureFilter = "&f=" + featureGuid.ToString(); ;
            }
         }
         string redirectUrl = SiteUtils.GetNavigationSiteRoot() + "/SearchResults.aspx" + searchText + featureFilter;
         Page.Response.Redirect(redirectUrl, true);
      }

      protected override void OnInit(EventArgs e)
      {
          base.OnInit(e);

          Controls.Clear();

          string searchText = Resources.Resource.SearchButtonText;
          if (actionText != string.Empty) searchText = actionText;

          if (!linkOnly)
          {
              if (!String.IsNullOrEmpty(searchContainerElement))
              {
                  SetUpDefaultClick();
              }

              tb = new TextBox();
              tb.ID = this.ID + "_tbFS";
              if (inputPlaceHolderText != string.Empty) tb.Attributes.Add("Placeholder", inputPlaceHolderText);
              this.Controls.Add(tb);
          }

          if (renderAsButton)
          {
              mb = new mojoButton();
              mb.ID = this.ID + "_mbFS";
              mb.Text = searchText;
              if (actionCssClass != string.Empty) mb.CssClass = actionCssClass;
              mb.Click += new EventHandler(mbFS_Click);
              this.Controls.Add(mb);
          }
          else
          {
              lb = new LinkButton();
              lb.ID = this.ID + "_lbFS";
              lb.Text = searchText;
              if (actionCssClass != string.Empty) lb.CssClass = actionCssClass;
              lb.Click += new EventHandler(lbFS_Click);
              this.Controls.Add(lb);
          }
      }
   }
}