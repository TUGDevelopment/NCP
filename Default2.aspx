<%@ Page Title="" Language="C#" MasterPageFile="~/SiteBase.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="_Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitlePartPlaceHolder" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HelpMenuDescriptionPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <script type="text/javascript">
        function OnEndCallback(s, e) {
            //alert("test");
     }
    </script>
<dx:ASPxCallbackPanel ID="cp" runat="server" 
    ClientInstanceName="CallbackPanel" OnCallback="cp_Callback">
    <ClientSideEvents EndCallback="OnEndCallback" />
    <PanelCollection>
        <dx:PanelContent ID="pc" runat="server">
        <div id="MasterContainer" runat="server"/>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>
<%--<div id="EditFormsContainer" style="display:normal">
    <dx:countEditForm ID="countEditForm" runat="server" />
</div>--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterRangeControlPlaceHolder" Runat="Server">
</asp:Content>

