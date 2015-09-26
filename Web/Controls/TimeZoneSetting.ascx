<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimeZoneSetting.ascx.cs" Inherits="mojoPortal.Web.UI.TimeZoneSetting" %>

<asp:DropDownList ID="ddTimeZone" runat="server" >
    <asp:ListItem Value="-12.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelEniwetokKwajalein %>" />
    <asp:ListItem Value="-11.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelMidwayIslandSamoa %>" />
    <asp:ListItem Value="-10.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelHawaii %>" />
    <asp:ListItem Value="-9.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelAlaska %>" />
    <asp:ListItem Value="-8.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelPacific %>" />
    <asp:ListItem Value="-7.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelMountain %>" />
    <asp:ListItem Value="-6.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelCentral %>" />
    <asp:ListItem Value="-5.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelEastern %>" />
    <asp:ListItem Value="-4.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelAtlantic %>" />
    <asp:ListItem Value="-3.50" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelNewfoundland %>" />
    <asp:ListItem Value="-3.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelBrazilBuenosAiresGeorgetown %>" />
    <asp:ListItem Value="-2.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelMidAtlantic %>" />
    <asp:ListItem Value="-1.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelAzoresCapeVerdeIslands %>" />
    <asp:ListItem Value="0.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelGMT %>" />
    <asp:ListItem Value="1.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelCentralEurope %>" />
    <asp:ListItem Value="2.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelEasternEurope %>" />
    <asp:ListItem Value="3.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelBaghdadKuwaitRiyadhMoscowStPetersburgVolgogradNairobi %>" />
    <asp:ListItem Value="3.50" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelTehran %>" />
    <asp:ListItem Value="4.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelAbuDhabiMuscatBakuTbilisi %>" />
    <asp:ListItem Value="4.50" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelKabul %>" />
    <asp:ListItem Value="5.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelEkaterinburgIslamabadKarachiTashkent %>" />
    <asp:ListItem Value="5.50" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelBombayCalcuttaMadrasNewDelhi %>" />
    <asp:ListItem Value="6.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelAlmatyDhakaColombo %>" />
    <asp:ListItem Value="7.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelBangkokHanoiJakarta %>" />
    <asp:ListItem Value="8.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelBeijingPerthSingaporeHongKongChongqingUrumqiTaipei %>" />
    <asp:ListItem Value="9.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelTokyoSeoulOsakaSapporoYakutsk %>" />
    <asp:ListItem Value="9.50" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelAdelaideDarwin %>" />
    <asp:ListItem Value="10.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelEASTEastAustralianStandardGuamPapuaNewGuineaVladivostok %>" />
    <asp:ListItem Value="11.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelMagadanSolomonIslandsNewCaledonia %>" />
    <asp:ListItem Value="12.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelAucklandWellingtonFijiKamchatkaMarshallIsland %>" />
    <asp:ListItem Value="13.00" Text="<%$ Resources:TimeZoneResources, TimeZoneLabelNukualofa %>" />
</asp:DropDownList>