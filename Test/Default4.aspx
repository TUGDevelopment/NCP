<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default4.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <script>
       var FocusedCellColumnIndex = 0;
var FocusedCellRowIndex = 0;

        function OnInitGridBatch(s, e) {
            ASPxClientUtils.AttachEventToElement(s.GetMainElement(), "keydown", function (evt) {
                if (evt.keyCode === ASPxClientUtils.StringToShortcutCode("ENTER"))
                    ASPxClientUtils.PreventEventAndBubble(evt);
                    DownPressed(s);
            });
        }

 
        function OnBatchEditStartAlimConstat(s, e) {

            FocusedCellColumnIndex = e.focusedColumn.index;
            FocusedCellRowIndex = e.visibleIndex;
}

       function OnEndEditCell(s, e) {
            FocusedCellColumnIndex = 0;
            FocusedCellRowIndex = 0;
        }

        function DownPressed(s) {
            var lastRecordIndex = s.GetTopVisibleIndex() + s.GetVisibleRowsOnPage() - 1;
            if (FocusedCellRowIndex < lastRecordIndex)
                s.batchEditApi.StartEdit(FocusedCellRowIndex + 1, FocusedCellColumnIndex);
            else
                s.batchEditApi.EndEdit();
        }
   </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" 
                SelectCommand="SELECT [ProductID], [ProductName], [SupplierID], [CategoryID], [QuantityPerUnit], [UnitPrice] FROM [Products]"></asp:SqlDataSource>
            <dx:ASPxGridView ID="ASPxGridViewListeAliConstat" runat="server" DataSourceID="SqlDataSource1" ClientInstanceName="gridAliConstat"  KeyFieldName="ProductID">
<ClientSideEvents  
Init="OnInitGridBatch" BatchEditStartEditing="OnBatchEditStartAlimConstat" BatchEditEndEditing="OnEndEditCell"/> 
<SettingsEditing Mode="Batch" BatchEditSettings-EditMode="Row" BatchEditSettings-StartEditAction="Click" BatchEditSettings-HighlightDeletedRows="False"></SettingsEditing>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="ProductID"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ProductName"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="QuantityPerUnit"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="UnitPrice"></dx:GridViewDataTextColumn>

                </Columns>
                </dx:ASPxGridView>
        </div>
    </form>
</body>
</html>
