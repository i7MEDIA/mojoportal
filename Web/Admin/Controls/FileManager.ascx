<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="FileManager.ascx.cs"
    Inherits="mojoPortal.Web.AdminUI.FileManagerControl" %>
<portal:mojoLabel ID="lblDisabledMessage" runat="server" CssClass="txterror info" />
<asp:Panel ID="pnlFile" runat="server" DefaultButton="btnUpload">
    <asp:PlaceHolder ID="myPlaceHolder" runat="server"></asp:PlaceHolder>
    <input id="hdnUploadID" type="hidden" name="hdnUploadID" />
    <asp:HiddenField ID="hdnCurDir" runat="server" />
    <table cellspacing="1" width="99%">
        <tr>
            <td class="">
                <asp:ImageButton ID="btnGoUp" runat="server" ImageUrl="/images/btnUp.jpg" AlternateText="" />
                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="/images/btnDelete.jpg" AlternateText="Delete" />
                &nbsp;&nbsp;<asp:Label ID="lblCurrentDirectory" runat="server" CssClass="foldername"></asp:Label>
                &nbsp;&nbsp;<portal:mojoLabel ID="lblError" runat="server" CssClass="txterror info" />
            </td>
        </tr>
        <tr>
            <td>
                <mp:mojoGridView ID="dgFile" runat="server" DataKeyNames="type"
                    SkinID="FileManager" TableCssClass="jqtable" AutoGenerateColumns="False" AllowSorting="True">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkChecked" runat="server"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="fileName">
                            <ItemTemplate>
                                &nbsp;
                                <asp:PlaceHolder ID="plhImgEdit" runat="server"></asp:PlaceHolder>
                                <asp:Image ID="imgType" runat="server" AlternateText=" "></asp:Image>
                                <asp:Button ID="lnkName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"filename") %>'
                                    CommandName="ItemClicked" CommandArgument='<%# Eval("path").ToString() + "#" + Eval("type").ToString()  %>'
                                    CausesValidation="false" CssClass="buttonlink"></asp:Button>
                            </ItemTemplate>
                            <EditItemTemplate>
                                &nbsp;
                                <asp:PlaceHolder ID="Placeholder1" runat="server"></asp:PlaceHolder>
                                <asp:Image ID="imgEditType" runat="server"></asp:Image>
                                <asp:TextBox ID="txtEditName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"filename") %>'>
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="fileName">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem,"size") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField SortExpression="fileName">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem,"modified") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Button ID="LinkButton1" runat="server" CssClass="buttonlink" CommandName="Edit"
                                    CausesValidation="false" Text="<%# Resources.Resource.FileManagerRename %>">
                                </asp:Button>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="LinkButton3" runat="server" CommandName="Update" Text='<%# Resources.Resource.FileManagerUpdateButton %>'>
                                </asp:Button>&nbsp;
                                <asp:Button ID="LinkButton2" runat="server" CommandName="Cancel" CausesValidation="false"
                                    Text="<%# Resources.Resource.FileManagerCancelButton %>"></asp:Button>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </mp:mojoGridView>
            </td>
        </tr>
        <tr>
            <td>
                <div class="moduletitle">
                    <asp:Label ID="lblCounter" runat="server"></asp:Label>
                </div>
            </td>
        </tr>
    </table>
    <table id="tblUpload" runat="server" class="fileupload" cellspacing="1" width="99%">
        <tr>
            <th colspan="2">
            </th>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="pnlNewFolder" runat="server" DefaultButton="btnNewFolder">
                    <asp:TextBox ID="txtNewDirectory" runat="server" CssClass="mediumtextbox"></asp:TextBox>
                    <portal:mojoButton ID="btnNewFolder" runat="server" Text="" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="moduletitle">
                    <mp:SiteLabel ID="lblGalleryEditImageLabel" runat="server" ConfigKey="FileManagerUploadLabel">
                    </mp:SiteLabel>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="pnlUpload" runat="server" DefaultButton="btnUpload">
                    <portal:jQueryFileUpload ID="uploader" runat="server" />
                    <asp:HiddenField ID="hdnState" Value="images" runat="server" />
                    <asp:ImageButton ID="btnRefresh" runat="server" TabIndex="-1"  />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="fileupload" colspan="2">
                <portal:mojoButton ID="btnUpload" runat="server" Text="Upload" />
            </td>
        </tr>
    </table>
</asp:Panel>
