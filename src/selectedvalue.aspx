<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectedvalue.aspx.cs" Inherits="src_selectedvalue" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function OnRowDblClick(s, e) {
            debugger;
            //var result = s.GetRowKey(e.visibleIndex);
            s.GetRowValues(e.visibleIndex, 'Id;SaleDoc;Item;Material', OnGetRowId); 
            //window.opener.OnCloseSelector(key);
            //window.close();
        }
        function OnGetRowId(values) {
            var result = [values[1], values[2], values[3]].join('|');
            window.parent.HidePopupAndShowInfo('Client', result);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxGridView runat="server" ID="gv" Width="100%" KeyFieldName="Id;SaleDoc;Item;Material" 
                ClientInstanceName="gv" DataSourceID="dsOrder">
                <ClientSideEvents RowDblClick="OnRowDblClick" />
                <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="500" />
                <SettingsEditing Mode="Inline"></SettingsEditing>
                <Columns>
                    <dx:GridViewDataColumn FieldName="SaleDoc" />
                    <dx:GridViewDataColumn FieldName="Item" />
                    <dx:GridViewDataColumn FieldName="Material" />
                </Columns>
                <SettingsDataSecurity AllowReadUnlistedFieldsFromClientApi="True" />
                <Styles>
                    <Cell Wrap="False"/>
                    <focusedrow BackColor="#f4dc7a" ForeColor="Black"/>
                </Styles>
                <SettingsContextMenu Enabled="true"/>
                <SettingsPager PageSize="20" EnableAdaptivity="true">
                    <PageSizeItemSettings Visible="true" ShowAllItem="true" AllItemText="All Records" />
                </SettingsPager>
                <settingspager pagesize="50" numericbuttoncount="6" />
                <SettingsBehavior AllowSelectByRowClick="true" />
            </dx:ASPxGridView>
        </div>
    </form>
    <asp:SqlDataSource ID="dsOrder" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetSalesOrder" SelectCommandType="StoredProcedure">
</asp:SqlDataSource>
</body>
</html>
