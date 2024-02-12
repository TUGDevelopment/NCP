<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .dxmLite_DevEx.dxm-side-menu-mode .dxm-side-menu-button > div,
        .dxmLite_DevEx.dxm-side-menu-mode .dxm-side-menu-button > div:before,
        .dxmLite_DevEx.dxm-side-menu-mode .dxm-side-menu-button > div:after {
            background-color: white !important;
            content: none !important;
        }

        .dxm-side-menu-mode .dxm-side-menu-button, .dxm-side-menu-mode .dxm-side-menu-button > div {
            background-image: url('https://cdn4.iconfinder.com/data/icons/web-ui-color/128/Menu_green-512.png');
            background-size: 30px 30px;
        }

            .dxm-side-menu-mode .dxm-side-menu-button > div {
                top: initial;
                left: initial;
                margin-left: initial;
                margin-top: initial;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxMenu ID="theMenu" runat="server" AutoSeparators="RootOnly" ClientIDMode="AutoID" ItemWrap="false" Width="100%"
                ShowSubMenuShadow="True" EnableSubMenuScrolling="True" VerticalAlign="Middle" SeparatorHeight="24px">
                <SettingsAdaptivity Enabled="true" EnableAutoHideRootItems="true" EnableCollapseToSideMenu="true" CollapseToSideMenuAtWindowInnerWidth="1024" />
                <Items>
                    <dx:MenuItem Text="My Summary" NavigateUrl="Default.aspx" Name="myTasks">
                        <ItemStyle HorizontalAlign="Center" />
                    </dx:MenuItem>
                    <dx:MenuItem Text="Compliance" Name="complianceMenu" BeginGroup="True">
                    </dx:MenuItem>
                    <dx:MenuItem Text="Risk" Name="riskMenu" BeginGroup="True">
                    </dx:MenuItem>
                    <dx:MenuItem Text="KRI" Name="KRI">
                    </dx:MenuItem>
                    <dx:MenuItem Name="erTop" Text="Events">
                    </dx:MenuItem>
                    <dx:MenuItem Name="registersMenu" Text="Registers">
                    </dx:MenuItem>
                    <dx:MenuItem Name="ContractMenu" Text="Contracts">
                    </dx:MenuItem>
                    <dx:MenuItem Name="mnuControlInventory" Text="Control Inventory" BeginGroup="true">
                    </dx:MenuItem>
                    <dx:MenuItem Name="mnuDocuments" Text="Document Library" BeginGroup="true">
                    </dx:MenuItem>
                    <dx:MenuItem Name="ObligationMenu" Text="Obligations">
                    </dx:MenuItem>
                    <dx:MenuItem Text="Reports" Name="reportsMenu">
                    </dx:MenuItem>
                    <dx:MenuItem Text="Dashboard" NavigateUrl="dashboard.aspx"
                        Name="dashboard">
                    </dx:MenuItem>
                    <dx:MenuItem Text="Maintenance" Name="maintenanceMenu">
                    </dx:MenuItem>
                    <dx:MenuItem Text="Mobile Portal" Name="mobileMenu" BeginGroup="True"
                        NavigateUrl="default.aspx?MobileToWeb=false&dl=yes">
                        <ItemStyle HorizontalAlign="Center" />
                    </dx:MenuItem>
                </Items>
            </dx:ASPxMenu>

        </div>
    </form>
</body>
</html>
