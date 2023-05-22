<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="mojoRating.ascx.cs" Inherits="mojoPortal.Web.UI.mojoRating" %>
<%--    <asp:UpdatePanel ID="upRating" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <span id="spnPrompt" runat="server" class="ratingprompt">
                <asp:Literal ID="litPrompt" runat="server" />
            </span>
            <span class="rating" style="display: none;"><asp:Literal ID="litRating" runat="server" EnableViewState="false" /></span>
                <portal:AjaxRating runat="server" ID="UserRating" EnableViewState="true" MaxRating="5"
                    CurrentRating="0" CssClass="ratingStar" StarCssClass="ratingItem" WaitingStarCssClass="Saved"
                    FilledStarCssClass="Filled" EmptyStarCssClass="Empty" />
                <asp:HiddenField ID="hdnContentGuid" runat="server" EnableViewState="false" />
            <span id="spnTotal" runat="server" class="voteswrap">
                <asp:Label ID="lblRatingVotes" runat="server" CssClass="ratingvotes" EnableViewState="false" />
                <asp:HyperLink id="lnkResults" runat="server" CssClass="cblink" Visible="false" EnableViewState="false" />
            </span>
            <asp:Panel ID="pnlComments" runat="server" CssClass="ratingcomments">
                <asp:Label ID="lblComments" runat="server" AssociatedControlID="txtComments" CssClass="rcommentprompt" />
                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" CssClass="ratingcommentbox" />
                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="rcommentprompt rpemail" />
                <asp:TextBox ID="txtEmail" runat="server" CssClass="normaltextbox ratingemail" />
                <div class="ratingbuttons">
                    <asp:Button ID="btnComments" runat="server" CausesValidation="false" CommandArgument="Comment" />
                    <asp:Button ID="btnNoThanks" runat="server" CausesValidation="false" CommandArgument="NoThanks" />
                    <asp:HiddenField ID="hdnRating" runat="server" Value='' />
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>--%>


