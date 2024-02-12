<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HeaderMenu.ascx.cs" Inherits="HeaderMenu" %>
<style>
       .menuContainer
       {
           margin: 20px auto;
       }
       .mainMenu,
       .mainMenuPopup,
       .mainMenuPopup a
       {
           font-size: 14px;
       }
       .mainMenuPopup
       {
           background-color: white;
       }
       .mainMenuPopup .SubMenuContent
       {
           margin: 0 auto;
       }
       .mainMenuPopup .SubMenuTextContent
       {
           padding: 20px;
           text-align: center;
       }
       .mainMenuPopup .SubMenuContent a
       {
           display: block;
           padding: 2px 0;
       }
       .mainMenuPopup .Group
       {
           float: left;
           padding: 20px 8px;
       }
       .mainMenuPopup .GroupContainer
       {
           float: left;
           padding: 0 8px;
       }
       .mainMenuPopup .GroupTitle
       {
           font-size: 1.2em;
           color: #333333;
           padding: 0 20px 20px 0;
       }
       .mainMenuPopup .GroupColumn
       {
           float: left;
           padding: 0 20px 0 0;
       }
       .mainMenuPopup .CategoryTitle
       {
           color: #AAAAAA;
           padding: 0 0 6px;
       }
       .mainMenuPopup .CategoryBreak
       {
           padding: 8px 0;
       }
       .clear
       {
           clear: both;
       }
        .body-content
        {
            margin: 0 auto;
            padding: 0 10px 40px;
            width: 80%;
            min-width: 200px;
            line-height: 150%;
            text-align: center;
        }
        .body-content h3
        {
            font-size: 1.2em;
            color: #333333;
            padding: 40px 0 20px;
        }
    </style>
<%--         <dx:ASPxPanel ID="TopPanel" runat="server" FixedPosition="WindowTop" ClientInstanceName="topPanel" CssClass="topPanel" Collapsible="true">
            <Styles>
                <ExpandedPanel CssClass = "expandedPanel" />
            </Styles>
            <SettingsAdaptivity CollapseAtWindowInnerWidth="580" />
            <PanelCollection>
                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                    <table style="width: 100%">
                        <tr>
                            <td><div class="title">Pest Control</div></td>
                            <td><dx:ASPxMenu ID="mMain" ClientInstanceName="headerMenu" runat="server" CssClass="main-menu" 
                                Theme="Moderno" DataSourceID="siteMapDataSource"/></td>
                            <td><asp:LoginStatus ID="LoginStatus1" runat="server" /></td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
            <ExpandBarTemplate>
                <div class="title">Pest Control</div>
            </ExpandBarTemplate>
        </dx:ASPxPanel>--%>
        <%--<dx:ASPxMenu ID="ASPxMenu2" runat="server" BackColor="Transparent" ItemAutoWidth="true"  
            ShowAsToolbar="true" CssClass="ActionMenu" SeparatorWidth="0" Theme="Moderno">
            <Border BorderWidth="0" />
            <SubMenuStyle CssClass="SubMenu" />
            
            <SettingsAdaptivity Enabled="true" EnableAutoHideRootItems="true"
                    EnableCollapseToSideMenu="true" CollapseToSideMenuAtWindowInnerWidth="300"
                    EnableCollapseRootItemsToIcons="true" CollapseRootItemsToIconsAtWindowInnerWidth="700" />
        <Items>
            <dx:MenuItem Text="SysCode">
                <Image IconID="navigation_backward_32x32"></Image>
            </dx:MenuItem>
            <dx:MenuItem>
                <Image IconID="navigation_forward_32x32"></Image>
            </dx:MenuItem>
            <dx:MenuItem BeginGroup="true">
                <Image IconID="save_save_32x32"></Image>
            </dx:MenuItem>
            <dx:MenuItem BeginGroup="true">
                <Image IconID="actions_refresh_32x32"></Image>
            </dx:MenuItem>
            <dx:MenuItem>
                <Image IconID="navigation_home_32x32"></Image>
            </dx:MenuItem>
            <dx:MenuItem BeginGroup="true" DropDownMode="true">
                <Image IconID="find_find_32x32"></Image>
                <Items>
                    <dx:MenuItem Text="Find customer">
                        <Image IconID="find_findcustomers_32x32"></Image>
                    </dx:MenuItem>
                    <dx:MenuItem Text="Find by ID">
                        <Image IconID="find_findbyid_32x32"></Image>
                    </dx:MenuItem>
                </Items>
            </dx:MenuItem>
            <dx:MenuItem BeginGroup="true">
                <Image IconID="setup_properties_32x32"></Image>
            </dx:MenuItem>
        </Items>
    </dx:ASPxMenu>
    <dx:ASPxMenu ID="Menu" runat="server" Width="100%" ItemAutoWidth="true" ShowPopOutImages="True" AllowSelectItem="true">
                <RootItemSubMenuOffset LastItemX="8" />
                <SettingsAdaptivity Enabled="true" EnableAutoHideRootItems="true"
                    EnableCollapseToSideMenu="true" CollapseToSideMenuAtWindowInnerWidth="300"
                    EnableCollapseRootItemsToIcons="true" CollapseRootItemsToIconsAtWindowInnerWidth="700" />
                <ItemStyle CssClass="dxm-menuItem" />
                <SubMenuStyle CssClass="subMenu" />
                <Items>
                    <dx:MenuItem Text="Meat & Poultry" ToolTip="Meat & Poultry">
                        <Image>
                            <SpriteProperties CssClass="MeatAndPoultry" HottrackedCssClass="MeatAndPoultry_Hovered" />
                        </Image>
                    </dx:MenuItem>
                    <dx:MenuItem Text="Fast Food" ToolTip="Fast Food">
                        <Image>
                            <SpriteProperties CssClass="FastFood" HottrackedCssClass="FastFood_Hovered" />
                        </Image>
                    </dx:MenuItem>
                    <dx:MenuItem Text="Healthy Food" ToolTip="Healthy Food">
                        <Image>
                            <SpriteProperties CssClass="HealthyFood" HottrackedCssClass="HealthyFood_Hovered" />
                        </Image>
                    </dx:MenuItem>
                    <dx:MenuItem Text="Baking" ToolTip="Baking">
                        <Image>
                            <SpriteProperties CssClass="Baking" HottrackedCssClass="Baking_Hovered" />
                        </Image>
                    </dx:MenuItem>
                    <dx:MenuItem Text="Fruits" ToolTip="Fruits" Selected="true">
                        <Image>
                            <SpriteProperties CssClass="Fruit" HottrackedCssClass="Fruit_Hovered" />
                        </Image>
                        <Items>
                            <dx:MenuItem Text="Apples" ToolTip="Apples" />
                            <dx:MenuItem Text="Bananas" ToolTip="Bananas" />
                            <dx:MenuItem Text="Kiwis" ToolTip="Kiwis" />
                            <dx:MenuItem Text="Oranges" ToolTip="Oranges" />
                        </Items>
                    </dx:MenuItem>
                    <dx:MenuItem Text="Beverages" ToolTip="Beverages">
                        <Image>
                            <SpriteProperties CssClass="Beverages" HottrackedCssClass="Beverages_Hovered" />
                        </Image>
                    </dx:MenuItem>
                    <dx:MenuItem Text="Juice" ToolTip="Juice">
                        <Image>
                            <SpriteProperties CssClass="Juice" HottrackedCssClass="Juice_Hovered" />
                        </Image>
                    </dx:MenuItem>
                    <dx:MenuItem Text="Wine" ToolTip="Wine">
                        <Image>
                            <SpriteProperties CssClass="Wine" HottrackedCssClass="Wine_Hovered" />
                        </Image>
                    </dx:MenuItem>
                </Items>
            </dx:ASPxMenu>--%>
    <div class="mainMenu">
    <table style="width: 100%">
    <tr>
        <td>
     <dx:ASPxMenu ID="mainMenu" runat="server" Theme="Moderno" DataSourceID="siteMapDataSource" ClientVisible="false" 
         AllowSelectItem="true" SelectParentItem="true" ClientInstanceName="mainMenu" SubMenuItemStyle-Width="230px">
        <RootItemSubMenuOffset FirstItemY="1" />
        <ClientSideEvents ItemClick="OnMenuItemClick" />
        <ItemStyle>
        <SelectedStyle BackColor="Yellow">
        </SelectedStyle>
    </ItemStyle>
    </dx:ASPxMenu></td>
    <td>
    <h2 style="color: #EFC849;">
        <asp:LoginStatus ID="LoginStatus1" runat="server" ForeColor="Yellow" /> 
    </h2></td>
        </tr>
    </table>   
    </div>
<dx:ASPxSiteMapDataSource ID="siteMapDataSource" runat="server" SiteMapFileName="~/web.sitemap"/>
