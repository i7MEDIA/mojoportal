<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="PageNotFound.aspx.cs" Inherits="mojoPortal.Web.PageNotFoundPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper pagenotfound ">
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
        	<div class="page-not-found-content-wrap">
        		<div class="page-not-found-intro">
		        	<h1>Page Not Found</h1>
	        		<p>
		        		We're sorry, the page you are looking for has moved or no longer exists.
		        	</p>
    			</div>
        		<div class="page-not-found-options">
        			<p class="lead">If you're still having trouble finding the content you're looking for, here's some things you can try:</p>
        			<ol>
        				<li>Check the spelling of your URL and ensure it is correct.</li>
        				<li>Try to find the page on the <portal:SiteMapLink id="smlink" runat="server" CssClass="sitemaplink" RenderAsListItem="false" />.</li>
        				<li>Search for the page below:</li>
        			</ol>
			        <div class="page-not-found-search"><input type="text" class="pnfsinput" placeholder="enter search query..." id="srchtext" onKeyPress="return submitenter(this,event)"/>
			        <button type="button" id="searchbutton" class="btn btn-success pnfsbtn" onClick="changeAction();" name="submitLogin"><i class="fal fa-search"></i></button></div>
		        </div>        		
        	</div>

        <asp:Panel ID="pnlGoogle404Enhancement" runat="server" CssClass="pnfgoogdiv" style="display: none;">
        
        <script type="text/javascript">
            var GOOG_FIXURL_LANG = '<%= CultureCode %>';
            var GOOG_FIXURL_SITE = '<%= SiteNavigationRoot %>';
        </script>
        <script type="text/javascript" src="https://linkhelp.clients.google.com/tbproxy/lh/wm/fixurl.js"></script>
        
        </asp:Panel>
                <asp:Literal ID="litErrorMessage" runat="server" EnableViewState="false" visible="false" />
    </portal:InnerBodyPanel>
    </portal:OuterBodyPanel>
    </portal:InnerWrapperPanel>
    </portal:OuterWrapperPanel>
<script type="text/javascript">

	$(document).ready(function(){
		$('#srchtext').val($('#goog-wm-qt').val());
		$('#searchbutton').click(function(){
			window.location.href='/SearchResults.aspx?q=' + $('#srchtext').val();
		});
	
	});
	
	function submitenter(myfield,e) {
		var keycode;
		if (window.event) keycode = window.event.keyCode;
		else if (e) keycode = e.which;
		else return true;
		
		if (keycode == 13) {
			window.location.href='/SearchResults.aspx?q=' + $('#srchtext').val();
			return false;
		}
		else {
		   return true;
		}
	}
	function tabIfEnter(myfield,e) {
		var keycode;
		if (window.event) keycode = window.event.keyCode;
		else if (e) keycode = e.which;
		else return true;
		
		if (keycode == 13)
		   {
		   document.forms[0].password.focus();
		   return false;
		   }
		else {
		   return true;
	   }
	}

</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
