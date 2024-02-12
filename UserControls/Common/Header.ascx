<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Header" %>
<%@ Register Src="~/UserControls/Common/HeaderMenu.ascx" TagPrefix="uc" TagName="HeaderMenu" %>
<%@ Register Src="~/UserControls/ActionToolbar.ascx" TagPrefix="uc" TagName="ActionToolbar" %>
<div style="float: left;">
    <uc:ActionToolbar runat="server" />
    <%--<a href="<%= ResolveClientUrl("~/") %>" class="top-logo"></a>--%>
</div>
<div style="float: right; padding-right: 50px;">
    <uc:HeaderMenu runat="server" ID="HeaderMenu" />
</div>
<div class="clear"></div>
