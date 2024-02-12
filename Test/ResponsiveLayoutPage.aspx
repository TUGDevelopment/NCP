<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResponsiveLayoutPage.aspx.cs" Inherits="Panel_ResponsiveLayoutPage" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, user-scalable=no, maximum-scale=1.0, minimum-scale=1.0" />
    <title>Responsive Layout - ASP.NET Panel Control</title>
    <style>
        body, form
        {
            padding: 0;
            margin: 0;
            overflow: hidden;
            min-height: 240px;
            min-width: 340px;
        }
        .title
        {
            float: left;
            padding: 1px 4px 2px;
            font-size: 2.2em;
        }
        .expandedPanel .title
        {
            display: none;
        }
        .main-menu
        {
            float: right!important;
            margin: 8px 0 4px;
        }
        .expandedPanel .main-menu
        {
            width: 100%;
        }
        .grid,
        .grid .dxgvHSDC,
        .grid .dxgvCSD
        {
            border-left: 0!important;
            border-right: 0!important;
            border-bottom: 0!important;
        }
    </style>
    <script type="text/javascript">
        function OnListBoxValueChanged() {
            sampleGrid.Refresh();
        }
        function UpdateHeaderMenuOrientation() {
            var orientation = topPanel.IsExpandable() ? 'Vertical' : 'Horizontal';
            if(orientation !== headerMenu.GetOrientation())
                headerMenu.SetOrientation(orientation);
        }
        function UpdateGridHeight(){
            sampleGrid.SetHeight(0);
            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if(document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            sampleGrid.SetHeight(containerHeight - topPanel.GetHeight() - employeeSelectorPanel.GetHeight());
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <dx:ASPxPanel ID="TopPanel" runat="server" FixedPosition="WindowTop" ClientInstanceName="topPanel" CssClass="topPanel" Collapsible="true">
            <Styles>
                <ExpandedPanel CssClass = "expandedPanel" />
            </Styles>
            <SettingsAdaptivity CollapseAtWindowInnerWidth="580" />
            <PanelCollection>
                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                    <div class="title">Orders</div>
                    <dx:ASPxMenu ID="mMain" ClientInstanceName="headerMenu" runat="server" CssClass="main-menu">
                        <Items>
                            <dx:MenuItem Text="Data" NavigateUrl="javascript:void(0)"></dx:MenuItem>
                            <dx:MenuItem Text="Settings" NavigateUrl="javascript:void(1)"></dx:MenuItem>
                            <dx:MenuItem Text="Profile" NavigateUrl="javascript:void(2)"></dx:MenuItem>
                            <dx:MenuItem Text="Help" NavigateUrl="javascript:void(3)"></dx:MenuItem>
                        </Items>
                    </dx:ASPxMenu>
                </dx:PanelContent>
            </PanelCollection>
            <ExpandBarTemplate>
                <div class="title">Orders</div>
            </ExpandBarTemplate>
        </dx:ASPxPanel>
        <dx:ASPxPanel ID="LeftPanel" runat="server" FixedPosition="WindowLeft" Collapsible="true">
            <SettingsAdaptivity CollapseAtWindowInnerWidth="900" />
            <PanelCollection>
                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                    <dx:ASPxNavBar ID="nbMain" runat="server">
                        <Groups>
                            <dx:NavBarGroup Text="Main">
                                <Items>
                                    <dx:NavBarItem Text="Overview" NavigateUrl="javascript:void(4)"></dx:NavBarItem>
                                    <dx:NavBarItem Text="Reports" NavigateUrl="javascript:void(5)"></dx:NavBarItem>
                                    <dx:NavBarItem Text="Analitics" NavigateUrl="javascript:void(6)"></dx:NavBarItem>
                                </Items>
                            </dx:NavBarGroup>
                            <dx:NavBarGroup Text="Export">
                                <Items>
                                    <dx:NavBarItem Text="Rtf" NavigateUrl="javascript:void(7)"></dx:NavBarItem>
                                    <dx:NavBarItem Text="Xls" NavigateUrl="javascript:void(8)"></dx:NavBarItem>
                                    <dx:NavBarItem Text="Pdf" NavigateUrl="javascript:void(9)"></dx:NavBarItem>
                                </Items>
                            </dx:NavBarGroup>
                        </Groups>
                        <Paddings Padding="0px" />
                    </dx:ASPxNavBar>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxPanel>
        <dx:ASPxPanel ID="EmployeeSelectorPanel" runat="server" ClientInstanceName="employeeSelectorPanel">
            <PanelCollection>
                <dx:PanelContent>
                    <dx:ASPxComboBox runat="server" ID="ASPxComboBox1" ClientInstanceName="sampleListBox"
                        DataSourceID="EmployeesDataSource" TextField="Name" ValueField="EmployeeID" ValueType="System.Int32"
                        SelectedIndex="0" Height="100%" Caption="Employee">
                        <ClientSideEvents ValueChanged="OnListBoxValueChanged" />
                    </dx:ASPxComboBox>
                </dx:PanelContent>
            </PanelCollection>
            <Paddings Padding="8px" />
        </dx:ASPxPanel>
        <dx:ASPxGridView runat="server" Width="100%" ID="ASPxGridView1" ClientInstanceName="sampleGrid"
            AutoGenerateColumns="False" DataSourceID="OrdersDataSource" KeyFieldName="OrderID" CssClass="grid">
            <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="500" />
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <columns>
                <dx:GridViewDataTextColumn FieldName="OrderID" VisibleIndex="0" AdaptivePriority="2"  Width="100px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="OrderDate" VisibleIndex="1">
                    <PropertiesTextEdit DisplayFormatString="d" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ShipName" VisibleIndex="2" AdaptivePriority="1">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ShipCountry" VisibleIndex="3" AdaptivePriority="1">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ShipCity" VisibleIndex="4" AdaptivePriority="1">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ShippedDate" VisibleIndex="6" AdaptivePriority="1">
                    <PropertiesTextEdit DisplayFormatString="d" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Quantity" VisibleIndex="7" Width="100px">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="UnitPrice" VisibleIndex="8" Width="100px">
                    <PropertiesTextEdit DisplayFormatString="c" />
                </dx:GridViewDataTextColumn>
            </columns>
            <settingsbehavior allowfocusedrow="True" />
            <settingspager pagesize="50" numericbuttoncount="6" />
        </dx:ASPxGridView>
        <script type="text/javascript">
        // <![CDATA[
            ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function(s, e){
                UpdateHeaderMenuOrientation();
                UpdateGridHeight();
            });
            ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function(s, e){
                UpdateHeaderMenuOrientation();
                UpdateGridHeight();
            });
        // ]]> 
        </script>
        <asp:SqlDataSource ID="OrdersDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:constr %>"
            SelectCommand="SELECT top 10 O.[OrderID], O.[OrderDate], O.[ShipName], O.[ShipCountry], O.[ShipCity], O.[ShippedDate], OD.[Quantity], OD.[UnitPrice], C.[CompanyName], C.[ContactName], C.[ContactTitle], C.[Country], C.[City], C.[Address], C.[Phone], C.[Fax] FROM [Orders] AS O INNER JOIN [OrderDetails] AS OD ON O.OrderID = OD.OrderID INNER JOIN [Customers] AS C ON O.[CustomerID] = C.[CustomerID]">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="EmployeesDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:constr %>"  
            SelectCommand="select it.[EmployeeID], (it.[LastName] + ' ' + it.[FirstName]) AS Name from Employees it" />
    </form>
</body>
</html>
