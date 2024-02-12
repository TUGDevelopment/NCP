<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
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
</asp:Content>

