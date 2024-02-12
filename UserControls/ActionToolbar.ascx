<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActionToolbar.ascx.cs" Inherits="UserControls_ActionToolbar" %>
<table class="ActionToolbar"><tr>
<td class="Strut">
    <div style="float: left">
        <dx:ASPxMenu ID="ActionMenu" runat="server" DataSourceID="ActionMenuDataSource" BackColor="Transparent"
            ShowAsToolbar="true" ClientInstanceName="ClientActionMenu" CssClass="ActionMenu" SeparatorWidth="0" 
            OnItemDataBound="ActionMenu_ItemDataBound">
            <Border BorderWidth="0" />
            <SubMenuStyle CssClass="SubMenu" />
            <ClientSideEvents ItemClick="DevAV.ToolbarMenu_ItemClick" />
        </dx:ASPxMenu>
    </div>
    <b class="clear"></b>
</td>
</tr></table>
<asp:XmlDataSource ID="ActionMenuDataSource" runat="server" DataFile="~/App_Data/Actions.xml" />
