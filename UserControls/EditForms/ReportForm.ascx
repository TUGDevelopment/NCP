<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReportForm.ascx.cs" Inherits="UserControls_EditForms_ReportForm" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v19.2" %>
<script type="text/javascript">
    //function OnInit(s, e) {
    //    AdjustSize();
    //    //document.getElementById("gridContainer").style.visibility = "";
    //    DevAV.ChangeDemoState("MailList");
    //}
    function OnEndCallback(s, e) {
    //    AdjustSize();
        var key = testgrid.cpKeyValue;
        if (key != undefined || key != null) {
            var args = key.split("|");
            if (args[0] == 'filter') {
                hKeyword.Set("Expr", args[1] == '' ? 'Z' : args[1]);
                s.PerformCallback('rebind');
            }
        }
        testgrid.cpKeyValue = null;
    }
    //function OnControlsInitialized(s, e) {
    //    ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
    //        AdjustSize();
    //    });
    //}
    //function AdjustSize() {
    //    var height = Math.max(0, document.documentElement.clientHeight);
    //    testgrid.SetHeight(height - 140);
    //}
    function UpdateGridHeight() {
        testgrid.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        testgrid.SetHeight(containerHeight - 140);
    }
    function OnContextMenuItemClick(sender, args) {
        if (args.objectType == "row") {
            if (args.item.name == "PDF" || args.item.name == "XLS" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } 
        }
    }
    function OnApplid(s, e) {
        testgrid.ApplyFilter(e.filterExpression);
        testgrid.PerformCallback('filter');
    }
    function gv_RowDblClick(s, e) {
        //debugger;
        s.GetRowValues(e.visibleIndex, 'Id;Text', OnGetRowId);
    }
    function OnGetRowId(values) {
        //gv.PerformCallback('build|'+ values[1]);
        filter.FilterExpression = values[1];
        cpfilter.PerformCallback('build|'+ values[1]);
    }
    var grid;
    var index;
    function StartEditNewRow(grd, visibleIndex) {
        grid = grd;
        index = visibleIndex;
        window.setTimeout('grid.StartEditRow(index)', 500);
    }
</script>
<%--<div id="gridContainer" style="visibility: hidden; padding-right: 50px;"></div>--%>
<dx:ASPxHiddenField ID="hGeID" runat="server" ClientInstanceName="hGeID"/>
<dx:ASPxHiddenField ID="hKeyword" runat="server" ClientInstanceName="hKeyword" />
<dx:ASPxGridView ID="testgrid" ClientInstanceName="testgrid" runat="server" Width="100%" CssClass="grid"
    OnDataBinding="testgrid_DataBinding" OnCustomCallback="testgrid_CustomCallback"
    OnFillContextMenuItems="testgrid_FillContextMenuItems"
    OnContextMenuItemClick="testgrid_ContextMenuItemClick"
    OnBeforeGetCallbackResult="BeforeGetCallbackResult" OnPreRender="PreRender"
    KeyFieldName="ID" EnableRowsCache="false"
    Border-BorderWidth="0">
    <styles>
        <focusedrow BackColor="#f4dc7a" ForeColor="Black">
        </focusedrow>
    </styles>
    <ClientSideEvents Init="DevAV.Init" EndCallback="DevAV.test" 
    ContextMenuItemClick="OnContextMenuItemClick"/>
<SettingsPager PageSize="20"></SettingsPager>
<Settings VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" ShowFooter="True" 
    ShowStatusBar="Hidden" ShowFilterRowMenu="true" AutoFilterCondition="Contains"/>
<SettingsBehavior AllowFocusedRow="True" AutoExpandAllGroups="true" 
    EnableRowHotTrack="True" ColumnResizeMode="Control" />
    <SettingsPager PageSize="20" EnableAdaptivity="true">
        <PageSizeItemSettings Visible="true" ShowAllItem="true" AllItemText="All Records" />
    </SettingsPager>
    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="IssueDate" SummaryType="Count" />
    </TotalSummary>
<SettingsContextMenu Enabled="true">
    <RowMenuItemVisibility DeleteRow="false" NewRow="false" EditRow="false" />
</SettingsContextMenu>
</dx:ASPxGridView>   
 <dx:ASPxGridViewExporter runat="server" ID="GridExporter" GridViewID="testgrid" />   
    <script type="text/javascript">
        // <![CDATA[
            ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function(s,e){
                UpdateGridHeight();
            });
            ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function(s,e){
                UpdateGridHeight();
            });
        // ]]> 
        </script>
    <dx:ASPxButton ID="btn" runat="server" ClientInstanceName="btn" ClientVisible="false"
                OnClick="btn_Click" />
    <dx:ASPxPopupControl ID="FilterPopup" runat="server" ClientInstanceName="filterPopup" Width="450px" PopupHorizontalAlign="WindowCenter" PopupAnimationType="Fade" 
        PopupVerticalAlign="WindowCenter" AllowDragging="True" CssClass="filterPopup" ShowCloseButton="true" HeaderText="Create Custom Filter" CloseAction="OuterMouseClick" CloseOnEscape="true" Modal="true">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
            <div style="padding: 4px 3px 4px">
                    <dx:ASPxPageControl runat="server" ID="pageControl" Width="100%">
                        <TabPages>
                            <dx:TabPage Text="Info" Visible="true">
                                <ContentCollection>
                                    <dx:ContentControl runat="server">
                                        <dx:ASPxCallbackPanel ID="cpfilter" runat="server" Width="100%" ClientInstanceName="cpfilter" OnCallback="cpfilter_Callback" >
                                          <PanelCollection>
                                          <dx:PanelContent runat="server">
                                                <dx:ASPxFilterControl ID="filter" runat="server"  
                                                ClientInstanceName="filter">
                                                <Columns>
                                                    <dx:FilterControlColumn ColumnType="Integer" DisplayName="matdoc" PropertyName="ID" />
                                                    <dx:FilterControlDateColumn ColumnType="DateTime" PropertyName="IssueDate" DisplayName="IssueDate">
                                                        <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" />
                                                    </dx:FilterControlDateColumn>
                                                    <dx:FilterControlComboBoxColumn ColumnType="String" PropertyName="Grouping" DisplayName="Grouping">
                                                        <PropertiesComboBox DataSourceID="dsGrouping" ValueField="ID" TextField="Name" />
                                                    </dx:FilterControlComboBoxColumn>
                                                    <dx:FilterControlComboBoxColumn ColumnType="String" PropertyName="Plant" DisplayName="Plant">
                                                        <PropertiesComboBox DataSourceID="dsPlant" ValueField="ID" TextField="Name" />
                                                    </dx:FilterControlComboBoxColumn>
                                                    <dx:FilterControlComboBoxColumn ColumnType="String" PropertyName="InstallArea" DisplayName="InstallArea">
                                                        <PropertiesComboBox DataSourceID="dsArea" ValueField="ID" TextField="Name" />
                                                    </dx:FilterControlComboBoxColumn>
                                                    <dx:FilterControlTextColumn ColumnType="String" PropertyName="Area" />
                                                </Columns>
                                                <ClientSideEvents Applied="function(s, e) { OnApplid(s, e);}" />
                                            </dx:ASPxFilterControl>
                                            </dx:PanelContent>
                                            </PanelCollection>
                                    </dx:ASPxCallbackPanel>
                                    <div style="text-align: right">
                                        <table>
                                            <tr>
                                                <td><dx:ASPxButton runat="server" ID="btnApply" Text="Apply" AutoPostBack="false" UseSubmitBehavior="false">
                                                <ClientSideEvents Click="function() { filter.Apply(); 
                                                filterPopup.Hide();}" />
                                                </dx:ASPxButton></td><td>&nbsp</td>
                                                <td> <dx:ASPxButton ID="btnSave" runat="server" text="Save Settings" OnClick="btnSave_OnClick"></dx:ASPxButton></td>
                                            </tr>
                                        </table></div>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                            <dx:TabPage Text="favorite" Visible="true">
                                <ContentCollection>
                                    <dx:ContentControl runat="server">
                                        <dx:ASPxGridView ID="gv" runat="server" DataSourceID="dsFilterControl" KeyFieldName="Id" EnableRowsCache="false" OnCustomCallback="gv_CustomCallback"
                                            ClientInstanceName="gv">
                                            <ClientSideEvents RowDblClick="gv_RowDblClick" />
                                            <Columns>
                                                <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowDeleteButton="true" ShowCancelButton="true" Width="10" />
                                                <dx:GridViewDataColumn FieldName="Text" Width="200" />
                                            </Columns>
                                            <SettingsAdaptivity AdaptivityMode="HideDataCells" />
                                            <SettingsBehavior AllowFocusedRow="true" />
                                        </dx:ASPxGridView>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                        </TabPages>
                    </dx:ASPxPageControl>
                </div>
            
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    
    <asp:SqlDataSource ID="ds" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="WebAPP_SelectCrossColumn2" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="hKeyword" Name="Keyword" PropertyName="['Expr']"/>
    </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dstest" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
        SelectCommand="select value from dbo.FNC_SPLIT('X;/',';')">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsPlant" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
        SelectCommand="select Id,Title as 'Name',0 IsActive from tblplant">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsGrouping" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
        SelectCommand="select ID,Name,case when IsActive is null then 0 else IsActive end IsActive from Mas_Grouping">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsArea" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
        SelectCommand="select ID,Name,shortname,case when IsActive is null then 0 else IsActive end IsActive from Mas_InstallArea">
    </asp:SqlDataSource>
<asp:SqlDataSource ID="dsFileSystem" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetFiles" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False"
    DeleteCommand="DELETE FROM tblFile WHERE ID = @Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="hGeID" Name="Id" 
            PropertyName="['GeID']"/>
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsFilterControl" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select Id,Text from MasFilterControl" DeleteCommand="delete MasFilterControl where Id=@Id">
    <DeleteParameters>
        <asp:Parameter Name="Id" />
    </DeleteParameters>
</asp:SqlDataSource>
