﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NavigationToolbar.ascx.cs" Inherits="UserControls_NavigationToolbar" %>
<table class="NavigationToolbar"><tr><td>
    <dx:ASPxMenu ID="NavigationMenu" runat="server" CssClass="NavigationMenu" ToolTip="Menu" OnInit="NavigationMenu_Init" BackColor="Transparent"
        ShowAsToolbar="true" AppearAfter="5000">
        <HorizontalPopOutImage Url="~/Content/Images/MathIcons/PopOutIcon.png"></HorizontalPopOutImage>
        <ItemStyle Font-Size="28px" />
        <SubMenuStyle CssClass="SubMenu" />
        <SubMenuItemStyle Font-Size="15px" />
        <Border BorderWidth="0" />
    </dx:ASPxMenu>
</td><td>
    <dx:ASPxImage ID="CollapsePaneImage" runat="server" Cursor="pointer" SpriteCssClass="Sprite_CollapsePane" ToolTip="Collapse" AlternateText="Collapse" 
        ClientInstanceName="ClientCollapsePaneImage" ClientVisible="false">
    </dx:ASPxImage>
</td></tr></table>
