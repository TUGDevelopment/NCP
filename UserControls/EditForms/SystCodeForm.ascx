<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SystCodeForm.ascx.cs" Inherits="UserControls_EditForms_SystCodeForm" %>
    <script type="text/javascript">
        function OnSelectedIndexChanged(s, e) {
            //alert(s.GetValue());
            //hf.Set("Value", s.GetValue());
        }
        function OnInit(s, e) {
            //debugger;
            //var edit = editor.Get("Name");
            //grid.SetVisible(edit);
            AdjustSize();
            document.getElementById("gridContainer").style.visibility = "";
        }
        function OnEndCallback(s, e) {
            AdjustSize();
        }
        function OnControlsInitialized(s, e) {
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSize();
            });
        }
        function AdjustSize() {
            var height = Math.max(0, document.documentElement.clientHeight);
            grid.SetHeight(height - 120);
        }
        function ClearSelection() {
            TreeList.SetFocusedNodeKey("");
            UpdateControls(null, "");
        }
        function UpdateSelection() {
            var employeeName = "";
            var focusedNodeKey = TreeList.GetFocusedNodeKey();
            if (focusedNodeKey != "")
                employeeName = TreeList.cpEmployeeNames[focusedNodeKey];
            UpdateControls(focusedNodeKey, employeeName);
        }
        function UpdateControls(key, text) {
            grid.PerformCallback('reload|' + key);
            DropDownEdit.SetText(text);
            DropDownEdit.SetKeyValue(key);
            DropDownEdit.HideDropDown();
            UpdateButtons();
        }
        function UpdateButtons() {
            debugger;
            clearButton.SetEnabled(DropDownEdit.GetText() != "");
            selectButton.SetEnabled(TreeList.GetFocusedNodeKey() != "");
        }
        function OnDropDown() {
            TreeList.SetFocusedNodeKey(DropDownEdit.GetKeyValue());
            TreeList.MakeNodeVisible(TreeList.GetFocusedNodeKey());
        }
        function testgrid_RowClick(s, e) {
            debugger;
            //TabList.SetActiveTabIndex(0);
            var key = s.GetRowKey(e.visibleIndex);
            //if (key != null)
            //    DevAV.ChangeDemoState("MailForm", "EditDraft", key);
            //alert(key);
            //debugger;
            //DevAV.showClearedPopup(countEditPopup);
            //countEditPopup.PerformCallback('read|' + key);
        }
        function OnSubmitButtonClick(s, e) {

        }
        function OnTabbedGroupPageControlInit(s, e) {
            var param = DevAV.getparam();
                switch (param) {
                    case "RawMaterial":
                        s.SetActiveTabIndex(0);
                        break;
                    case "ProductStyle":
                        s.SetActiveTabIndex(1);
                        break;
                    case "MediaType":
                        s.SetActiveTabIndex(2);
                        break;
                    case "CanSize":
                        s.SetActiveTabIndex(3);
                        break;
                }
        }
        var visibleIndex = 0;
        function onStartEdit(s, e) {
            visibleIndex = e.visibleIndex;
        }
        function OnEndEdit(s, e) {
            var code = s.batchEditApi.GetCellValue(visibleIndex, 'Componant');
            if (code != null) {
                updategrid(s);
            }
        }
        function updategrid(s) {
            window.setTimeout(function () {
                var count_value = GetChangesCount(s.batchEditApi);
                if (count_value > 0)
                    s.UpdateEdit();
            }, 0);
        }
        function GetChangesCount(batchApi) {
            var updatedCount = batchApi.GetUpdatedRowIndices().length;
            var deletedCount = batchApi.GetDeletedRowIndices().length;
            var insertedCount = batchApi.GetInsertedRowIndices().length;

            return updatedCount + deletedCount + insertedCount;
        }
    </script>
<dx:ASPxHiddenField ID="hGeID" runat="server" ClientInstanceName="hGeID"/>
<dx:ASPxGridView ID="testgrid" ClientInstanceName="testgrid" runat="server" Width="100%" KeyFieldName="ID" 
    OnRowUpdating="testgrid_RowUpdating" OnRowInserting="testgrid_RowInserting" OnCustomDataCallback="testgrid_CustomDataCallback" OnCustomCallback="testgrid_CustomCallback"
    OnDataBinding="testgrid_DataBinding" AutoGenerateColumns="true" Border-BorderWidth="0px" CssClass="grid">
    <ClientSideEvents Init="DevAV.Init" RowClick="testgrid_RowClick" EndCallback="DevAV.test" />
    <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="500" />
    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
    <EditFormLayoutProperties>
        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
    </EditFormLayoutProperties>
    <SettingsEditing Mode="EditForm" EditFormColumnCount="1"></SettingsEditing>
    <Styles>
        <Cell Wrap="False"/>
        <focusedrow BackColor="#f4dc7a" ForeColor="Black"/>
    </styles>
    <SettingsPager PageSize="20" EnableAdaptivity="true">
        <PageSizeItemSettings Visible="true" ShowAllItem="true" AllItemText="All Records" />
    </SettingsPager>
    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="ReceivingDate" SummaryType="Count" />
    </TotalSummary>
    <SettingsBehavior allowfocusedrow="True" AutoExpandAllGroups="true" EnableRowHotTrack="True" ColumnResizeMode="Control" EnableCustomizationWindow="true" AllowSelectByRowClick="true"/>
    <settingspager pagesize="50" numericbuttoncount="6" />
    <SettingsContextMenu Enabled="true"/>
    <SettingsCommandButton EditButton-Image-Url="~/Content/images/icons8-edit-16.png" EditButton-RenderMode="Image"
                                    UpdateButton-RenderMode="Button"  CancelButton-RenderMode="Button"  />
</dx:ASPxGridView>
<dx:ASPxCallbackPanel ID="FormPanel" runat="server" RenderMode="Div" ClientVisible="false" ScrollBars="Vertical" ClientInstanceName="ClientFormPanel" >
    <PanelCollection>
        <dx:PanelContent>  
        <dx:ASPxFormLayout ID="formLayout" ClientInstanceName="formLayout" CssClass="formLayout" runat="server" AlignItemCaptionsInAllGroups="true">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="800" />
            <Items>
            <dx:TabbedLayoutGroup Caption="TabbedGroup" ClientInstanceName="tabbedGroupPageControl" ShowGroupDecoration="false" Width="100%">
                    <ParentContainerStyle CssClass="tabbedGroupPageControlCell" />
                    <ClientSideEvents Init="OnTabbedGroupPageControlInit" />
                    <Items>
                        <dx:LayoutGroup GroupBoxDecoration="None">
                            <Items>
                                <dx:LayoutItem Caption="Procedure" RequiredMarkDisplayMode="Required">
                                     <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxComboBox ID="cmbProductType" runat="server" DataSourceID="dsProductType" ValueField="Code">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="Code" />
                                                    <dx:ListBoxColumn FieldName="Description" />
                                                </Columns> 
                                            </dx:ASPxComboBox>
                                         </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Product Group" RequiredMarkDisplayMode="Required">
                                    <HelpTextSettings Position="Bottom" />
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxComboBox ID="cmbProdGroup" runat="server" DataSourceID="dsProdGroup" ValueField="ProductGroup">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="ProductGroup" />
                                                    <dx:ListBoxColumn FieldName="Name" />
                                                </Columns> 
                                            </dx:ASPxComboBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Code" RequiredMarkDisplayMode="Required" HelpTextSettings-Position="Bottom" >
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxTextBox ID="txtCode" runat="server" Width="100%"/>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Description" RequiredMarkDisplayMode="Required" HelpTextSettings-Position="Bottom" >
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxTextBox ID="txtDescription" runat="server" Width="100%"/>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                        </dx:LayoutGroup>
                        <dx:LayoutGroup GroupBoxDecoration="None">
                            <Items>
                            <dx:LayoutItem Caption="Product Group" RequiredMarkDisplayMode="Required">
                                <HelpTextSettings Position="Bottom" />
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxComboBox ID="cmbProductGroup" runat="server" DataSourceID="dsProdGroup" ValueField="ProductGroup">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="ProductGroup" />
                                                <dx:ListBoxColumn FieldName="Name" />
                                            </Columns> 
                                        </dx:ASPxComboBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Code" RequiredMarkDisplayMode="Required" HelpTextSettings-Position="Bottom" >
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="txtProductCode" runat="server" Width="100%"/>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Description" RequiredMarkDisplayMode="Required" HelpTextSettings-Position="Bottom" >
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="txtProductDesc" runat="server" Width="100%"/>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                        <dx:LayoutGroup GroupBoxDecoration="None">
                            <Items>
                            <dx:LayoutItem Caption="MediaType" RequiredMarkDisplayMode="Required">
                                <HelpTextSettings Position="Bottom" />
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxComboBox ID="cmbMediaType" runat="server" DataSourceID="dsMediaType" ValueField="Code">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="Code" />
                                                <dx:ListBoxColumn FieldName="Description" />
                                            </Columns> 
                                        </dx:ASPxComboBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Product Group" RequiredMarkDisplayMode="Required">
                                <HelpTextSettings Position="Bottom" />
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxComboBox ID="cmbMediaProd" runat="server" DataSourceID="dsProdGroup" ValueField="ProductGroup">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="ProductGroup" />
                                                <dx:ListBoxColumn FieldName="Name" />
                                            </Columns> 
                                        </dx:ASPxComboBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Select Type" RequiredMarkDisplayMode="Required" HelpTextSettings-Position="Bottom" >
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxGridView ID="gv" runat="server" ClientInstanceName="gv" OnCustomCallback="gv_CustomCallback"
                                                OnBatchUpdate="gv_BatchUpdate" OnDataBinding="gv_DataBinding" 
                                                KeyFieldName="ID">
                                                <ClientSideEvents BatchEditEndEditing="OnEndEdit" BatchEditStartEditing="onStartEdit" 
                                                    Init="function(s,e){ gv.PerformCallback('reload|0');}" />
                                                <Columns>
                                                    <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowEditButton="true" ButtonRenderMode="Image">
                                                        <CustomButtons>
                                                            <dx:GridViewCommandColumnCustomButton ID="Edit">
                                                                <Image ToolTip="Remove" Url="~/Content/Images/Cancel.gif"/>
                                                            </dx:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <HeaderTemplate>
                                                            <dx:ASPxButton runat="server" RenderMode="Link" AutoPostBack="false">
                                                                <Image ToolTip="Insert" Url="~/Content/Images/icons8-plus-math-filled-16.png" />
                                                                <ClientSideEvents Click="function(s,e){ gv.AddNewRow(); }" />
                                                            </dx:ASPxButton>
                                                        </HeaderTemplate>
                                                    </dx:GridViewCommandColumn>
                                                    <dx:GridViewDataComboBoxColumn FieldName="Componant">
                                                        <PropertiesComboBox DataSourceID="dsComponant" ValueField="value" TextField="value"/>
                                                    </dx:GridViewDataComboBoxColumn>
                                                    <dx:GridViewDataComboBoxColumn FieldName="Description">
                                                        <PropertiesComboBox DataSourceID="dsAnalysisItem" ValueField="ID" TextField="Name" ValueType="System.Int32"/>
                                                    </dx:GridViewDataComboBoxColumn>
                                                </Columns>
                                                <Settings  ShowFooter="true" GridLines="Both" ShowStatusBar="Hidden" VerticalScrollBarMode="Auto"/>
                                                <SettingsEditing Mode="Batch" />
                                            </dx:ASPxGridView>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
<%--                            <dx:LayoutItem Caption="Code" RequiredMarkDisplayMode="Required" HelpTextSettings-Position="Bottom" >
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="txtMediaCode" runat="server" Width="100%"/>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Description" RequiredMarkDisplayMode="Required" HelpTextSettings-Position="Bottom" >
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="txtMediaDesc" runat="server" Width="100%"/>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>--%>
                        </Items>
                    </dx:LayoutGroup>
                    </Items>
                </dx:TabbedLayoutGroup>
                <%--<dx:LayoutItem Caption="" HorizontalAlign="Left" RequiredMarkDisplayMode="Hidden">
                    <ParentContainerStyle Paddings-PaddingTop="5" Paddings-PaddingRight="0" />
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxButton runat="server" ID="submitButton" Text="Submit" ValidateInvisibleEditors="false" Width="70"
                                ClientSideEvents-Click="OnSubmitButtonClick" />
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>--%>
		        <dx:LayoutGroup Caption="MainInfo" GroupBoxDecoration="None" UseDefaultPaddings="false" Paddings-PaddingTop="10">
                    <GroupBoxStyle>
                        <Caption Font-Bold="true" Font-Size="10"/>
                    </GroupBoxStyle>
                    <Items>
                    <%--<dx:LayoutItem Caption="Media Type" >
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                
                           <dx:ASPxComboBox runat="server" ID="CmbMediaType" ClientInstanceName="CmbMediaType" ValueField="Code" 
                              DataSourceID="dsMediaType"  TextFormatString="{0}, {1}" Width="240px">
                                <Columns>
                                    <dx:ListBoxColumn FieldName="Code" />
                                    <dx:ListBoxColumn FieldName="Description" />
                                </Columns>
                            </dx:ASPxComboBox>
                             <dx:ASPxDropDownEdit ID="DropDownEdit" runat="server" ClientInstanceName="DropDownEdit" 
                                Width="285px" AllowUserInput="False" AnimationType="None">
                                <DropDownWindowStyle>
                                    <Border BorderWidth="0px" />
                                </DropDownWindowStyle>
                                <ClientSideEvents Init="DevAV.Init" DropDown="OnDropDown" />
                                <DropDownWindowTemplate>
                                    <div>
                                        <dx:ASPxTreeList ID="TreeList" ClientInstanceName="TreeList" runat="server"
                                            Width="500px" DataSourceID="ods" OnCustomJSProperties="TreeList_CustomJSProperties"
                                            KeyFieldName="ID" ParentFieldName="EmployerId">
                                            <Settings VerticalScrollBarMode="Auto" ScrollableHeight="150" />
                                            <ClientSideEvents FocusedNodeChanged="function(s,e){ selectButton.SetEnabled(true); }" />
                                            <BorderBottom BorderStyle="Solid" />
                                            <SettingsBehavior AllowFocusedNode="true" AutoExpandAllNodes="true" FocusNodeOnLoad="false" />
                                            <SettingsPager Mode="ShowAllNodes">
                                            </SettingsPager>
                                            <Styles>
                                                <Node Cursor="pointer">
                                                </Node>
                                                <Indent Cursor="default">
                                                </Indent>
                                            </Styles>
                                            <Columns>
                                                <dx:TreeListTextColumn FieldName="FirstName" VisibleIndex="1" Caption="RowID"/>
                                                <dx:TreeListTextColumn FieldName="LastName" VisibleIndex="2" Caption="Title"/>
                                                <dx:TreeListDateTimeColumn FieldName="HireDate" VisibleIndex="3"/>
                                            </Columns>
                                        </dx:ASPxTreeList>
                                    </div>
                                    <table style="background-color: White; width: 100%;">
                                        <tr>
                                            <td style="padding: 10px;">
                                                <dx:ASPxButton ID="clearButton" ClientEnabled="false" ClientInstanceName="clearButton"
                                                    runat="server" AutoPostBack="false" Text="Clear">
                                                    <ClientSideEvents Click="ClearSelection" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td style="text-align: right; padding: 10px;">
                                                <dx:ASPxButton ID="selectButton" ClientEnabled="false" ClientInstanceName="selectButton"
                                                    runat="server" AutoPostBack="false" Text="Select">
                                                    <ClientSideEvents Click="UpdateSelection" />
                                                </dx:ASPxButton>
                                                <dx:ASPxButton ID="closeButton" runat="server" AutoPostBack="false" Text="Close">
                                                    <ClientSideEvents Click="function(s,e) { DropDownEdit.HideDropDown(); }" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </DropDownWindowTemplate>
                            </dx:ASPxDropDownEdit>
                            </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Product Group" >
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxRadioButtonList runat="server" ID="rdProdGroup" 
                                    DataSourceID="dsProdGroup" ValueField="ProductGroup" RepeatColumns="4" RepeatDirection="Horizontal" ClientInstanceName="rdProdGroup" 
                                                RepeatLayout="Flow" Border-BorderStyle="None" Paddings-PaddingLeft="0px" FocusedStyle-Border-BorderStyle="None" TextField="Name">
                                            </dx:ASPxRadioButtonList>
                            </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                            <hr />
                            <div id="gridContainer" style="visibility: hidden">
                            <dx:ASPxGridView runat="server" ID="grid" ClientInstanceName="grid" OnDataBinding="grid_DataBinding" KeyFieldName="ID" OnCustomCallback="grid_CustomCallback"
                                OnDataBound="grid_DataBound" Width="100%" EnableViewState="false" Border-BorderWidth="0">                                      
                                <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" />
                                <SettingsSearchPanel ColumnNames="" Visible="false" />
	                            <Settings ShowColumnHeaders="false" ShowGroupPanel="false" VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" ShowGroupedColumns="True" 
                                    GridLines="Vertical" ShowFooter="true" />
	                            <SettingsBehavior AutoExpandAllGroups="true" EnableRowHotTrack="True" ColumnResizeMode="Control" EnableCustomizationWindow="true" AllowFocusedRow="true"/>
	                            <SettingsPager PageSize="50">
                                <PageSizeItemSettings Visible="true" ShowAllItem="true"  AllItemText="All Records" />
                                </SettingsPager>
	                            <Styles>
		                            <Row Cursor="pointer" />
	                            </Styles>
                                <SettingsContextMenu Enabled="true" />
                                <SettingsEditing EditFormColumnCount="1" Mode="EditForm"/>
                                <SettingsPopup>
                                        <EditForm Modal="true" AllowResize="true"/>
                                </SettingsPopup>
                                <SettingsCommandButton EditButton-Image-Url="~/Content/images/icons8-edit-16.png" EditButton-RenderMode="Image"
                                    UpdateButton-RenderMode="Button"  CancelButton-RenderMode="Button"  />
                            </dx:ASPxGridView>
                            <dx:ASPxGridViewExporter runat="server" ID="GridExporter" GridViewID="grid" />
                            </div>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>--%>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>
<asp:SqlDataSource ID="ods" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>" 
    SelectCommand="spGetSubjectType" SelectCommandType="StoredProcedure">
</asp:SqlDataSource>
<asp:SqlDataSource ID="ds" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"/>
<asp:SqlDataSource ID="dsProdGroup" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>" 
    SelectCommand="spGetProdGroup" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:QueryStringParameter Name="param" QueryStringField="param" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsMediaType" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>" 
    SelectCommand="select * from tblMediaType">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsAnalysisItem" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>" 
    SelectCommand="select * from tblAnalysisItem"
    InsertCommand = "insert into tblAnalysisItem values(@Name)"
    UpdateCommand = "update tblAnalysisItem set Name=@Name where ID=@ID">
    <UpdateParameters>
        <asp:Parameter Name="Name" />
        <asp:Parameter Name="ID" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Name" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsResult" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spselectsysall" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:QueryStringParameter Name="param" QueryStringField="param" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsProductType" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>" 
    SelectCommand="select * from tblProductType">
</asp:SqlDataSource>
  <asp:SqlDataSource ID="dsComponant" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
       SelectCommand="select * from dbo.FNC_SPLIT('Mix,Topping,None',',')" />
<asp:SqlDataSource ID="dsitems" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>" 
    SelectCommand="select * from transMediaItems where SubID=@SubID">
    <SelectParameters>
        <asp:SessionParameter Name="SubID" SessionField="SubID" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>