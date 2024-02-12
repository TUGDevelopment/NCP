<%@ Control Language="C#" AutoEventWireup="true" CodeFile="countEditForm.ascx.cs" Inherits="UserControls_EditForms_countEditForm" %>
<%--<script type="text/javascript" src="Content/Scripts/webcam.min.js"></script>--%>
<script type="text/javascript">
    var lastCountry = null;
    function OnSelectedIndexChanged(s, e) {
        //alert(s.GetValue());
        //hf.Set("Value", s.GetValue());
    }
    function OnSelectedIndex(s, e) {
        debugger;
        DevAV.ClearForm("None");
    }
    function OnInit(s, e) {
        AdjustSize();
        document.getElementById("gridContainer").style.visibility = "";
    }
    function OnEndCallback(s, e) {
        //debugger;
        AdjustSize();
    }
    function OnControlsInitialized(s, e) {
        ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
            AdjustSize();
        });
    }
    function AdjustSize() {
        var height = Math.max(0, document.documentElement.clientHeight);
        testgrid.SetHeight(height - 220);
    }
    function grid_RowDblClick(s, e) {
        var key = s.GetRowKey(e.visibleIndex);
        //alert(key);
        //s.StartEditRow(e.visibleIndex);
    }
    function OnCustomButtonClick(s, e) {
        alert("test");
        if (s.cpIsEdit) {
            if (confirm('The grid is in the edit mode. Do you wish to cahcel this process?')) {
                s.CancelEdit();
                StartEditNewRow(s, e.visibleIndex);
            }
        }
        else {
            s.StartEditRow(e.visibleIndex);
        }
    }
        var grid;
        var index;
    function StartEditNewRow(grd, visibleIndex) {
        grid = grd;
        index = visibleIndex;
        window.setTimeout('grid.StartEditRow(index)', 500);
    }
    function Combo_SelectedIndexChanged(s) {
        var value = s.GetValue().toString();
        debugger;
        //countEditPopup.PerformCallback('rebind|0');
        //btnLinkImageAndText.SetEnabled(true);
        grid.CancelEdit();
        //ClientArea.SetValue("");
        //cb.PerformCallback('rebind|0');
        grid.PerformCallback('reload|'+CmbPlant.GetValue() + '|'+ CmbGrouping.GetValue());
        //grid.mainElement.dataSrc = null;
        //grid.Refresh();
        var strcom = s.GetValue().toString();
        if (ClientArea.InCallback())
            lastCountry = strcom.toString();
        else {
            var text = ["Build", strcom.toString(), CmbPlant.GetValue(), "0"];
            var param = text.join("|");
            ClientArea.PerformCallback(param);
        }
    }
    //function Combo_SelectedIndexChanged(Combo) {
    //    debugger;
    //    countEditPopup.PerformCallback('rebind|0');
    //    //grid.CancelEdit();
    //    //grid.mainElement.dataSrc = null;
    //    //grid.Refresh();
    //    var strcom = Combo.GetValue().toString();
    //    if (ClientArea.InCallback())
    //        lastCountry = strcom.toString();
    //    else {
    //        var param = "Build|" + strcom.toString();
    //        ClientArea.PerformCallback(param);
    //    }
    //}
    function Combo_EndCallback(s, e) {
        if (lastCountry) {
            var text = ["Build", lastCountry, CmbPlant.GetValue(), "0"];
            var value = text.join("|");
            ClientArea.PerformCallback(value);
            lastCountry = null;
        }
    }
    //function OnSelectedIndexChanged(s, e) {
        //debugger;
        //grid.CancelEdit();
        //grid.Refresh();
        //grid.PerformCallback('reload|0');
        //grid.SetVisible(true);
        //grid.StartEditRow(0);
        //alert("tst");
    //}
    function grid_EndCallback(s, e) {
        if (!grid.cpKeyValue)
            return;
        var key = grid.cpKeyValue;
        //debugger;
        //DevAV.ChangeDemoState("MailList");
        //testgrid.SetVisible(true);
        //ClientFormPanel.SetVisible(false);
        //alert('sucess +' + key);
        //countEditPopup.Hide();
        //testgrid.Refresh();
        //grid.PerformCallback('reload|0');
    }
    function testgrid_RowDblClick(s, e) {
        var key = s.GetRowKey(e.visibleIndex);
        if(key!=null)
        DevAV.ChangeDemoState("MailForm", "EditDraft", key);
        //alert(key);
        //debugger;
        //DevAV.showClearedPopup(countEditPopup);
        //countEditPopup.PerformCallback('read|' + key);
    }
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
    function HidePopupAndShowInfo(closedBy, returnValue) {
        debugger;
        if (closedBy == 'Client') {
            PopupControl.Hide();
            alert('Closed By: ' + closedBy + '\nReturn Value: ' + returnValue);
            var res = returnValue.split("@");
            cb.PerformCallback('rebind|' + returnValue);
            //CmbPlant.PerformCallback('Value|' + returnValue)
            //rbGrouping.PerformCallback('Value|' + returnValue)
            //CmbPlant.SetValue(returnValue.substring(0, 2));
            //rbGrouping.SetValue(returnValue.substring(2, 1));
            //ClientArea.SetValue(returnValue);
        }
        if (closedBy == 'capture') {
            PopupControl.Hide();
            //returnValue = "20190513012531.jpg";
            UploadedFilesTokenBox.AddToken(returnValue);
            updateTokenBoxVisibility();
        }
    }
    function build_data(group,plant,area) {
        var text = ["Build", group.toString(), plant.toString(), area];
            var param = text.join("|");
            ClientArea.PerformCallback(param);
    }
    function OnBtnShowPopupClick(param) {
        debugger;
        //var height = Math.max(0, document.documentElement.clientHeight);
        //filterPopup.SetHeight(height);
        if (param == 'scan') {
            PopupControl.RefreshContentUrl();
            PopupControl.SetContentUrl('src/jsqrcode.aspx?ID=1');
            //ClientFormPanel.SetVisible(false);
            //cp.PerformCallback();
        }
        if (param == 'Attach') {
            //UploadControl.AddFile('Uploading.jpg');//
            PopupControl.SetContentUrl('cs.aspx?ID=2');
            //UploadControl.Upload();
        }
        PopupControl.SetHeaderText(param);
        PopupControl.Show();
    }
    function onFileUploadComplete(s, e) {
        if (e.callbackData) {
            var fileData = e.callbackData.split('|');
            var fileName = fileData[0],
                fileUrl = fileData[1],
                fileSize = fileData[2];
            DevAV.AddFile(fileName, fileUrl, fileSize);
        }
    }
    function OnGridViewSelectionChanged(s, e) {
        //alert('XXX');
        debugger;
        var RowCount = testgrid.GetSelectedRowCount();
        var view = DemoState.View;
        //var hiddenSelectedRowCount = testgrid.GetSelectedRowCount() - GetSelectedFilteredRowCount();
        //alert(testgrid.GetSelectedRowCount());
        //ClientActionMenu.GetItemByName("Approve").SetVisible(hiddenSelectedRowCount > 0);
        ClientActionMenu.GetItemByName("Reject").SetVisible(RowCount > 0 && view =="MailList");
        ClientActionMenu.GetItemByName("Approve").SetVisible(RowCount > 0 && view =="MailList");
    }
    function GetSelectedFilteredRowCount() {
        return testgrid.cpFilteredRowCountWithoutPage + testgrid.GetSelectedKeysOnPage().length;
    }
    function OnActiveTabChanged(s, e) {
         var selectedIndex = s.GetActiveTabIndex();
            tabbed.SetActiveTabIndex(selectedIndex);     
    }
    function onTokenBoxValidation(s, e) {
        //var isValid = DocumentsUploadControl.GetText().length > 0 || UploadedFilesTokenBox.GetText().length > 0;
        //e.isValid = isValid;
        //if (!isValid) {
        //    e.errorText = "No files have been uploaded. Upload at least one file.";
        //}
    }
    function onTokenBoxValueChanged(s, e) {
        updateTokenBoxVisibility();
    }
    function updateTokenBoxVisibility() {
        var isTokenBoxVisible = UploadedFilesTokenBox.GetTokenCollection().length > 0;
        UploadedFilesTokenBox.SetVisible(isTokenBoxVisible);
    }
    function OnCallbackComplete(s, e) {
        var res = e.result.split('|');
        CmbPlant.SetValue(res[0]);
        CmbGrouping.SetValue(res[1]);
        build_data(CmbGrouping.GetValue(), res[0],res[2])
        grid.CancelEdit();
        grid.PerformCallback('reload|'+ res[0] +'|'+res[1]);
            //ClientArea.PerformCallback('Value|' + res)
    }
    </script>
<dx:ASPxHiddenField runat="server" ID="Hf" ClientInstanceName="hf"/>
<dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="hfEdits"/>
<dx:ASPxHiddenField ID="husername" runat="server" ClientInstanceName="husername"/>
<dx:ASPxHiddenField ID="hGeID" runat="server" ClientInstanceName="hGeID" />
<dx:ASPxHiddenField ID="hKeyword" runat="server" ClientInstanceName="hKeyword" />
<%--<dx:ASPxGridView runat="server" Width="100%" ID="ASPxGridView1" ClientInstanceName="sampleGrid"
    AutoGenerateColumns="false" DataSourceID="dsResult" KeyFieldName="ID" CssClass="grid">
    <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="500" />
    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
    <Columns>
        <dx:GridViewCommandColumn ShowSelectCheckbox="True" ShowClearFilterButton="true" SelectAllCheckboxMode="Page" />
        <dx:GridViewDataColumn FieldName="rn" Width="5%"/>
        <dx:GridViewDataDateColumn FieldName="IssueDate" PropertiesDateEdit-DisplayFormatString="dd/MM/yyyy" GroupIndex="0" SortOrder="Descending"/>
        <dx:GridViewDataComboBoxColumn FieldName="Plant">
            <PropertiesComboBox DataSourceID="dsPlant" ValueField="Id" TextField="Title"/>
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataColumn FieldName="Name" Width="25%"/>
        <dx:GridViewDataColumn FieldName="GroupingName" Width="35%"/>
        <dx:GridViewDataComboBoxColumn FieldName="CreateBy" Caption="CreateBy" Width="20%"> 
            <PropertiesComboBox DataSourceID="dsulogin" ValueField="user_name" TextField="Name" />
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataDateColumn FieldName="CreateOn"> 
            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"/>
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataComboBoxColumn FieldName="StatusApp" Width="25%">
            <PropertiesComboBox DataSourceID="dsStaus" ValueField="Id" TextField="Name"/>
        </dx:GridViewDataComboBoxColumn>
    </Columns>
    <settingsbehavior allowfocusedrow="True" />
    <settingspager pagesize="50" numericbuttoncount="6" />
</dx:ASPxGridView>--%>
<dx:ASPxGridView ID="testgrid" ClientInstanceName="testgrid" runat="server" Width="100%"
    OnCustomJSProperties="testgrid_CustomJSProperties" CssClass="grid"
    OnCustomCallback="testgrid_CustomCallback" OnFillContextMenuItems="testgrid_FillContextMenuItems"
    KeyFieldName="ID" EnableRowsCache="false" DataSourceID="dsResult" OnCustomButtonInitialize="testgrid_CustomButtonInitialize" onHtmlCommandCellPrepared="testgrid_HtmlCommandCellPrepared" 
    OnCommandButtonInitialize="testgrid_CommandButtonInitialize"
    OnCustomDataCallback="testgrid_CustomDataCallback"
    OnContextMenuItemClick="testgrid_ContextMenuItemClick"
    OnBeforeGetCallbackResult="BeforeGetCallbackResult" OnPreRender="PreRender"
    Border-BorderWidth="0">
    <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="500" />
    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
    <Columns>
        <dx:GridViewCommandColumn ShowSelectCheckbox="True" ShowClearFilterButton="true" SelectAllCheckboxMode="Page" Width="5%"/>
        <dx:GridViewDataColumn FieldName="rn" Width="5%" CellStyle-HorizontalAlign="Center"/>
        <dx:GridViewDataDateColumn FieldName="IssueDate" PropertiesDateEdit-DisplayFormatString="dd/MM/yyyy" GroupIndex="0" SortOrder="Descending"/>
        <dx:GridViewDataComboBoxColumn FieldName="Plant">
            <PropertiesComboBox DataSourceID="dsPlant" ValueField="Id" TextField="Title"/>
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataColumn FieldName="Name" Width="20%"/>
        <dx:GridViewDataColumn FieldName="GroupingName" Width="20%"/>
        <%--<dx:GridViewDataColumn FieldName="fullname" Caption="CreateBy" Width="20%" />--%>
        <dx:GridViewDataComboBoxColumn FieldName="CreateBy" Caption="CreateBy" Width="20%"> 
            <PropertiesComboBox DataSourceID="dsulogin" ValueField="user_name" TextField="Name" />
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataDateColumn FieldName="CreateOn"> 
            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"/>
        </dx:GridViewDataDateColumn>
<%--    <dx:GridViewDataColumn FieldName="StatusApp" Width="25%"/>--%>
        <dx:GridViewDataComboBoxColumn FieldName="StatusApp" Width="15%">
            <PropertiesComboBox DataSourceID="dsStaus" ValueField="Id" TextField="shortname"/>
        </dx:GridViewDataComboBoxColumn>
    </Columns>
    <ClientSideEvents Init="DevAV.Init" EndCallback="DevAV.test" RowDblClick="testgrid_RowDblClick" ContextMenuItemClick="function(s,e) { OnContextMenuItemClick(s, e); }" SelectionChanged="OnGridViewSelectionChanged" />
    <EditFormLayoutProperties>
        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
    </EditFormLayoutProperties>
    <SettingsEditing Mode="Inline"></SettingsEditing>
    <Styles>
        <Cell Wrap="False"/>
        <focusedrow BackColor="#f4dc7a" ForeColor="Black"/>
    </styles>
    <SettingsPager PageSize="20" EnableAdaptivity="true">
        <PageSizeItemSettings Visible="true" ShowAllItem="true" AllItemText="All Records" />
    </SettingsPager>
    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="IssueDate" SummaryType="Count" />
    </TotalSummary>
    <settingsbehavior allowfocusedrow="True" />
    <settingspager pagesize="50" numericbuttoncount="6" />
    <SettingsContextMenu Enabled="true"/>
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
<dx:ASPxCallback ID="cb" ClientInstanceName="cb" runat="server" OnCallback="cb_Callback">
    <ClientSideEvents CallbackComplete="function (s, e) { OnCallbackComplete(s, e); }" />
</dx:ASPxCallback>
<dx:ASPxCallbackPanel ID="cp" runat="server" ClientInstanceName="cp" OnCallback="cp_Callback">
    <PanelCollection>
        <dx:PanelContent ID="pc" runat="server">
        <div id="MasterContainer" runat="server"/>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>
<dx:ASPxCallbackPanel ID="FormPanel" runat="server" RenderMode="Div" ClientVisible="false" ScrollBars="Auto" ClientInstanceName="ClientFormPanel">
        <PanelCollection>
            <dx:PanelContent>  
        <dx:ASPxFormLayout ID="formLayout" ClientInstanceName="formLayout" CssClass="formLayout" runat="server" UseDefaultPaddings="false" AlignItemCaptionsInAllGroups="true">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="800" />
        <Items>
        <dx:LayoutItem Caption="">
            <LayoutItemNestedControlCollection>
                <dx:LayoutItemNestedControlContainer>
                    <dx:ASPxTabControl ID="tcDemos" runat="server" NameField="Id" DataSourceID="XmlDataSource1" ActiveTabIndex="0" ClientInstanceName="tcDemos">
                        <ClientSideEvents ActiveTabChanged="function(s, e) { OnActiveTabChanged(s, e); }"/>
                    </dx:ASPxTabControl>
                    <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Platforms.xml"
                        XPath="//product"></asp:XmlDataSource>
                </dx:LayoutItemNestedControlContainer>
            </LayoutItemNestedControlCollection>
        </dx:LayoutItem>              
        <dx:TabbedLayoutGroup Caption="" ActiveTabIndex="0" ClientInstanceName="tabbed" Width="100%" ShowGroupDecoration="false">   
            <Items>
            <dx:LayoutGroup Caption="Details" ColCount="1">
                <Paddings PaddingTop="10px"></Paddings>
                    <GroupBoxStyle>
                        <Caption Font-Bold="true" Font-Size="10"/>
                    </GroupBoxStyle>
                    <GridSettings StretchLastItem="true" WrapCaptionAtWidth="660">
                       <Breakpoints>
                            <dx:LayoutBreakpoint MaxWidth="500" ColumnCount="1" Name="S" />
                            <dx:LayoutBreakpoint MaxWidth="800" ColumnCount="2" Name="M" />
                       </Breakpoints>
                       </GridSettings>
                <Items>
                    <dx:LayoutItem Caption="Plant">
                        <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="CmbPlant" runat="server" DataSourceID="dsPlant" ValueField="Id" Width="230px" OnCallback="CmbPlant_Callback"
                                    ClientInstanceName="CmbPlant" TextField="Title">
                                    <ClientSideEvents Init="function(s, e) { s.SetSelectedIndex(0); }" 
                                        SelectedIndexChanged="OnSelectedIndex"/>
                                </dx:ASPxComboBox>
                                <%--<dx:ASPxRadioButtonList ID="btnListplant" runat="server" RepeatDirection="Horizontal" RepeatColumns="4"
                                            ClientInstanceName="btnListplant" DataSourceID="dsPlant" ValueField="Id" TextField="Title"
                                            Border-BorderStyle="None" Paddings-PaddingLeft="0px" FocusedStyle-Border-BorderStyle="None"
                                            SelectedIndex="0" RepeatLayout="Flow">
                                    <CaptionSettings Position="Top" />
                                    <ClientSideEvents Init="function(s,e){ s.SetSelectedIndex(0);}" />
                                </dx:ASPxRadioButtonList>--%> 
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Date">
                        <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxDateEdit runat="server" ID="deValidfrom" ClientInstanceName="deValidfrom" Width="130px"
                                    DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">    
                                <ClientSideEvents Init="function(s,e){ s.SetDate(new Date());}" />
                            </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Grouping">
                        <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                               <dx:ASPxComboBox ID="CmbGrouping" runat="server" DataSourceID="dsGrouping" ValueField="Id" Width="230px"
                                    ClientInstanceName="CmbGrouping" TextField="Name">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { Combo_SelectedIndexChanged(s); }"/>
                                </dx:ASPxComboBox>
                                 <%--<dx:ASPxRadioButtonList ID="rbGrouping" runat="server" DataSourceID="dsGrouping" ValueField="Id" RepeatColumns="4" RepeatDirection="Horizontal" ClientInstanceName="rbGrouping" 
                                    RepeatLayout="Flow" Border-BorderStyle="None" Paddings-PaddingLeft="0px" FocusedStyle-Border-BorderStyle="None" TextField="Name">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { rb_SelectedIndexChanged(s); }"/>
                                </dx:ASPxRadioButtonList>--%>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Area">
                        <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="CmbArea" runat="server" Width="230px" TextFormatString="{0} | {1}" ValueField="Id" DropDownWidth="350px"           
                                            OnCallback="CmbArea_Callback" ClientInstanceName="ClientArea">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="shortname"/>
                                                <dx:ListBoxColumn FieldName="Name" Width="250px"/>
                                            </Columns>
                                            <ClientSideEvents EndCallback="Combo_EndCallback"/>
                                        </dx:ASPxComboBox>
                                    <%--<table>
                                    <tr>
                                        <td>
                                        
                                        </td><td>&nbsp</td>
                                        <td><dx:ASPxButton ID="btnLinkImageAndText" runat="server" RenderMode="Link" AutoPostBack="false" ClientInstanceName="btnLinkImageAndText"
                                            Width="90px" Height="25px" Text="scan">
                                            <Image Url="~/Content/Images/icons8-barcode-scanner-32.png"></Image>
                                            <ClientSideEvents Click="function(s, e) { OnBtnShowPopupClick('scan');
                                            }" />
                                        </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                                <dx:ASPxButtonEdit ID="ASPxButtonEdit1" Text="Andy McKee - Rylynn" Width="100%" runat="server">
                                <Buttons>
                                    <dx:EditButton>
                                    </dx:EditButton>
                                </Buttons>
                                    <ClientSideEvents ButtonClick="function(s, e) {
	                            filterPopup.Show();
                            }" />
                            </dx:ASPxButtonEdit>--%>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                     <dx:LayoutItem CaptionSettings-Location="Top" Caption="" Width="100%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxGridView runat="server" ID="grid" KeyFieldName="ID" ClientInstanceName="grid" Width="100%" EnableRowsCache="false" 
                                            OnCustomCallback="grid_CustomCallback" 
                                            OnDataBinding="grid_DataBinding" OnCellEditorInitialize="grid_CellEditorInitialize" 
                                            OnRowUpdating="grid_RowUpdating"
                                            OnRowDeleting="grid_RowDeleting"
                                            OnDataBound="grid_DataBound" 
                                            OnHtmlRowCreated="grid_HtmlRowCreated" 
                                            AutoGenerateColumns="true"
                                            Border-BorderWidth="0" OnCancelRowEditing="grid_CancelRowEditing"
                                            OnCommandButtonInitialize="grid_CommandButtonInitialize"
                                            OnCustomButtonInitialize="grid_CustomButtonInitialize">
                                            <Settings ShowColumnHeaders="false" VerticalScrollBarMode="Visible" VerticalScrollableHeight="500"/>
                                            <SettingsBehavior AutoExpandAllGroups="true" EnableRowHotTrack="True" ColumnResizeMode="NextColumn" AllowFocusedRow="true" EnableCustomizationWindow="true"/>
		                                    <Styles>
			                                    <Row Cursor="pointer" />
		                                    </Styles>
                                            <SettingsEditing Mode="EditForm" EditFormColumnCount="4"/>
                                            <SettingsPager Mode="ShowAllRecords"/>
                                            <EditFormLayoutProperties>
                                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="700" />
                                            </EditFormLayoutProperties>
                                            <%--<Templates>
                                                <EditForm>
                                                    <div style="padding: 4px 3px 4px">
                                                        <dx:ASPxPageControl runat="server" ID="pageControl" Width="100%">
                                                            <TabPages>
                                                                <dx:TabPage Text="Info" Visible="true">
                                                                    <ContentCollection>
                                                                        <dx:ContentControl runat="server">
                                                                            <dx:ASPxGridViewTemplateReplacement ID="Editors" ReplacementType="EditFormEditors"
                                                                                runat="server">
                                                                            </dx:ASPxGridViewTemplateReplacement>
                                                                        </dx:ContentControl>
                                                                    </ContentCollection>
                                                                </dx:TabPage>
                                                            </TabPages>
                                                        </dx:ASPxPageControl>
                                                    </div>
                                                    <div style="text-align: left; padding: 2px; display:none";>
                                                        <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                            runat="server">
                                                        </dx:ASPxGridViewTemplateReplacement>
                                                        <dx:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                            runat="server">
                                                        </dx:ASPxGridViewTemplateReplacement>
                                                    </div>
                                                </EditForm>
                                            </Templates>--%>
                                            <ClientSideEvents RowDblClick="grid_RowDblClick" CustomButtonClick="OnCustomButtonClick" EndCallback="grid_EndCallback" /> 
                                    </dx:ASPxGridView>
                                    <br />

                                    <%--<dx:ASPxGridViewExporter runat="server" ID="GridExporter" GridViewID="gridData" />
                                    <asp:PlaceHolder ID="phGrid" runat="server"></asp:PlaceHolder>--%>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup Caption="Attached file">
             <Paddings PaddingTop="10px"></Paddings>
                    <GroupBoxStyle>
                        <Caption Font-Bold="true" Font-Size="10"/>
                    </GroupBoxStyle>
                    <GridSettings StretchLastItem="true" WrapCaptionAtWidth="660">
                       <Breakpoints>
                            <dx:LayoutBreakpoint MaxWidth="500" ColumnCount="1" Name="S" />
                            <dx:LayoutBreakpoint MaxWidth="800" ColumnCount="2" Name="M" />
                       </Breakpoints>
                       </GridSettings>
                     <Items>
                    <dx:LayoutItem Caption="">
                        <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <%--<div id="my_camera"></div>
	                            <!-- Configure a few settings and attach camera -->
	                            <script type="text/javascript">
		                            Webcam.set({
			                            width: 320,
			                            height: 240,
			                            image_format: 'jpeg',
			                            jpeg_quality: 90
		                            });
		                            Webcam.attach( '#my_camera' );
	                            </script>
                                    <br />
		                            <input type=button value="Take Snapshot" onClick="take_snapshot()">	
	                            <!-- Code to handle taking the snapshot and displaying it locally -->
	                            <script type="text/javascript">
		                            function take_snapshot() {
                                        debugger;
			                            // take snapshot and get image data
			                            Webcam.snap( function(data_uri) {
				                            // display results in page
                                            var arr = new Array();
                                                arr["test"] = [data_uri];
                                                hfEdits.Set("MyArray", arr);
       				                            demo.innerHTML = 
					                            '<h2>Here is your image:</h2>' + 
                                                '<img src="' + data_uri + '"/>';
			                            } );
		                            }
	                            </script>--%> 
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxUploadControl ID="UploadControl" runat="server" ClientInstanceName="UploadControl" Width="320" UploadMode="Advanced" 
                                                NullText="Select multiple files..." ShowUploadButton="false" ShowProgressPanel="True" OnFilesUploadComplete="UploadControl_FilesUploadComplete"
                                                OnFileUploadComplete="UploadControl_FileUploadComplete">
                                                <AdvancedModeSettings EnableMultiSelect="True" EnableFileList="True" EnableDragAndDrop="True" />
                                                <ClientSideEvents FilesUploadStart="function(s, e) { DevAV.Clear(); }"
                                                                  FileUploadComplete="onFileUploadComplete" />
                                            </dx:ASPxUploadControl>
                                            <%--<dx:ASPxUploadControl ID="uploader" runat="server" ClientInstanceName="uploader" Width="100%"
                                            NullText="Select multiple files..." UploadMode="Advanced" ShowUploadButton="true" ShowProgressPanel="True"
                                            OnFileUploadComplete="UploadControl_FileUploadComplete">
                                            <AdvancedModeSettings EnableMultiSelect="True" EnableFileList="True" EnableDragAndDrop="True" />
                                            <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".jpg,.jpeg,.gif,.png">
                                            </ValidationSettings>
                                            <ClientSideEvents  FileUploadComplete="onFileUploadComplete"/></dx:ASPxUploadControl>--%>

                                        </td>
                                        <td style="vertical-align:top;">
                                            <dx:ASPxButton ID="btnAttach" runat="server" RenderMode="Link" AutoPostBack="false" ClientInstanceName="btnAttach"
                                            Width="90px" Height="25px" Text="Attach" ClientVisible="false">
                                            <Image Url="~/Content/Images/icons8-unsplash-32.png"></Image>
                                            <ClientSideEvents Click="function(s, e) { OnBtnShowPopupClick('Attach');
                                            }" />
                                        </dx:ASPxButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><br />
                                                <dx:ASPxTokenBox runat="server" Width="100%" ID="tokenBox" ClientInstanceName="UploadedFilesTokenBox"
                                                NullText="Select the documents to submit" AllowCustomTokens="false" ClientVisible="false">
                                                <ClientSideEvents Init="updateTokenBoxVisibility" ValueChanged="onTokenBoxValueChanged" Validation="onTokenBoxValidation" />
                                                <ValidationSettings EnableCustomValidation="true"></ValidationSettings>
                                            </dx:ASPxTokenBox>
                                            <div style="display:none">
                                                  <dx:UploadedFilesContainer ID="FileContainer" runat="server" Width="380" Height="180" 
                                                NameColumnWidth="240" SizeColumnWidth="70" HeaderText="Uploaded files" /></div>
                                        </td>
                                    </tr>
                                </table>
                                <%--<dx:ASPxFileManager ID="fileManager" runat="server" DataSourceID="dsfileManager" OnCustomCallback="fileManager_CustomCallback"
                                      ClientInstanceName="fileManager">
                                        <Settings ThumbnailFolder="~/Content/FileManager" InitialFolder="Salvador Dali\1936 - 1945" 
                                            AllowedFileExtensions=".rtf, .pdf, .doc, .docx, .odt, .txt, .xls, .xlsx, .xlsb, .ods, .ppt, .pptx, .odp, .jpe, .jpeg, .jpg, .gif, .png , .msg"/>
                                        <SettingsFileList>
                                            <ThumbnailsViewSettings ThumbnailWidth="100" ThumbnailHeight="100" />
                                        </SettingsFileList>
                                        <SettingsDataSource KeyFieldName="ID" ParentKeyFieldName="ParentID" NameFieldName="Name" IsFolderFieldName="IsFolder" FileBinaryContentFieldName="Data" LastWriteTimeFieldName="LastWriteTime" />
                                        <SettingsEditing AllowCreate="true" AllowDelete="true" AllowMove="true" AllowRename="true" AllowCopy="true" AllowDownload="true" />
                                         <SettingsPermissions>
                                          <AccessRules>
					                    <dx:FileManagerFolderAccessRule Path="" Edit="Deny" />
                                               <dx:FileManagerFileAccessRule Path="*.xml" Edit="Deny" />
                                               <dx:FileManagerFolderAccessRule Path="System" Browse="Deny" />

                                               <dx:FileManagerFolderAccessRule Role="Guest" Path="" Browse="Deny" />
                                               <dx:FileManagerFolderAccessRule Path="MemBersOnly" Browse="Deny" />
                                               <dx:FileManagerFolderAccessRule Path="TestFolder" Role="TestUser" Edit="Deny" Browse="Deny" EditContents="Deny" />                
                                               <dx:FileManagerFolderAccessRule Role="Admin" Browse="Allow" Path="" Edit="Allow" EditContents="Allow" Upload="Allow" />
                                          </AccessRules>
                                       </SettingsPermissions>
                                        <SettingsBreadcrumbs Visible="true" ShowParentFolderButton="true" Position="Top" />
                                        <SettingsUpload UseAdvancedUploadMode="true">
                                            <AdvancedModeSettings EnableMultiSelect="true" />
                                            <ValidationSettings 
                                                MaxFileSize="10000000" 
                                                MaxFileSizeErrorText="The file you are trying to upload is larger than what is allowed (10 MB).">
                                            </ValidationSettings>
                                        </SettingsUpload>
					                <Settings EnableMultiSelect="true" />
                                    <ClientSideEvents Init="function(s, e) { s.PerformCallback() ;}" />
                                    </dx:ASPxFileManager>
                                    <br />
                                    <p class="Note">
                                        <b>Allowed Extensions</b>: .jpg, .jpeg, .gif, .rtf, .txt, .avi, .png, .mp3, .xml,
                                        .doc, .pdf , .xls, .xlsx, .xlsb, .msg
                                    </p>--%>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
   </dx:TabbedLayoutGroup>
        </Items>
    </dx:ASPxFormLayout>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <%--<dx:ASPxFormLayout runat="server" ID="formLayout" ClientInstanceName="formLayout" CssClass="formLayout">
        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="800" />
        <Items>
            <dx:LayoutGroup Caption="XXX" ColCount="3" GroupBoxDecoration="None" UseDefaultPaddings="false" Paddings-PaddingTop="10">
                <Paddings PaddingTop="10px"></Paddings>
                <GroupBoxStyle>
                    <Caption Font-Bold="true" Font-Size="10"/>
                </GroupBoxStyle>
                <Items>
                    <dx:LayoutItem Caption="Plant" ColSpan="3" HelpText="ColSpan=3">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxComboBox ID="CmbPlant" runat="server" DataSourceID="dsPlant" ValueField="Id" Width="230px"
                                    ClientInstanceName="ClientGrouping" TextField="Title">
                                        <ClientSideEvents Init="function(s,e){ s.SetSelectedIndex(0);}" />
                                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" ValidationGroup="group1"/>
                                </dx:ASPxComboBox>
                                
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    <dx:LayoutItem Caption="Date">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                           
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="select Type">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                            
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Area">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                            
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    </items>
                </dx:LayoutGroup>
        </Items>
        </dx:ASPxFormLayout>--%>
<%--    <dx:ASPxPopupControl ID="countEditPopup" ClientInstanceName="countEditPopup" runat="server" PopupHorizontalAlign="WindowCenter" ShowCloseButton="true" 
    CloseOnEscape="true" Width="450px" PopupVerticalAlign="WindowCenter" AllowResize="true" AllowDragging="True" ScrollBars="Auto"
    CloseAction="CloseButton" OnWindowCallback="countEditPopup_WindowCallback" Modal="true" PopupAnimationType="Fade">
    <ClientSideEvents PopUp="function(s,e){callback.PerformCallback('@@@');rbGrouping.Focus();}" />
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">

        </dx:PopupControlContentControl>
            </ContentCollection>
    </dx:ASPxPopupControl>--%>
<dx:ASPxPopupControl ID="PopupControl" runat="server" ClientInstanceName="PopupControl" CloseAction="CloseButton" CloseOnEscape="true" Maximized="true"
            HeaderText="Results" AllowDragging="True" Modal="true" PopupAnimationType="Fade"
            EnableViewState="False">
        <ClientSideEvents PopUp="function(s, e) { ASPxClientEdit.ClearGroup('createAccountGroup'); }" />
        <SizeGripImage Width="11px" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="Panel2" runat="server" DefaultButton="btCreate">
                    <PanelCollection>
                        <dx:PanelContent runat="server">
                     </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="FilterPopup" runat="server" ClientInstanceName="filterPopup" Width="620px" Height="500px" CloseAction="CloseButton" CloseOnEscape="true" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
        HeaderText="FilterPopup" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AutoUpdatePosition="true">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
            <div style="max-width: 400px">
            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="Filter" Width="100%" View="GroupBox">
            <ContentPaddings PaddingLeft="0" PaddingRight="0" />
            <PanelCollection>
                <dx:PanelContent runat="server">
            <dx:ASPxFilterControl ID="filter" runat="server" 
            ClientInstanceName="filter">
            <Columns>
                <dx:FilterControlDateColumn ColumnType="DateTime" PropertyName="CreateOn" DisplayName="CreateOn">
                    <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" />
                </dx:FilterControlDateColumn>
                <dx:FilterControlComboBoxColumn ColumnType="String" PropertyName="Grouping" DisplayName="Grouping">
                    <PropertiesComboBox DataSourceID="dsGrouping" ValueField="ID" TextField="Name" />
                </dx:FilterControlComboBoxColumn>
                <dx:FilterControlComboBoxColumn ColumnType="String" PropertyName="StatusApp" DisplayName="StatusApp">
                    <PropertiesComboBox DataSourceID="dsStaus" ValueField="Id" TextField="shortname"/>
                </dx:FilterControlComboBoxColumn>
            </Columns>
            <ClientSideEvents Applied="function(s, e) { OnApplid(s, e);}" />
            </dx:ASPxFilterControl>
            <div style="text-align: right;">
            <dx:ASPxButton runat="server" ID="btnApply" Text="Apply" AutoPostBack="false" UseSubmitBehavior="false" Width="80px" Style="margin: 12px 1em auto auto;">
                <ClientSideEvents Click="function() { filter.Apply(); 
                    filterPopup.Hide();}" />
            </dx:ASPxButton>
            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <%--<ClientSideEvents Closing="function(s) {s.SetContentHtml(null); }" />--%>
    </dx:ASPxPopupControl>
    <asp:SqlDataSource ID="dsfileManager" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
        SelectCommand="spGetFileSystem2" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
    </asp:SqlDataSource>
<asp:SqlDataSource ID="dsInstallArea" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from Mas_InstallArea">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsResult" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spselectpestall" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="hKeyword" Name="Keyword" PropertyName="['Expr']"/>
        <asp:ControlParameter ControlID="husername" Name="user_name" PropertyName="['user_name']"/>
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dstest" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select value from dbo.FNC_SPLIT('X;/',';')">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsPlant" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spselectPlant" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="husername" Name="user_name" PropertyName="['user_name']"/>
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsGrouping" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from Mas_Grouping where isnull(IsActive,0)=0">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsStaus" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from Mas_Status">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsgv" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="WebApp_SelectCmdV2" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:Parameter Name="plant" Type="String" />
        <asp:Parameter Name="group" Type="String" />
        <asp:Parameter Name="Id" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsulogin" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select *,firstname + ' ' + lastname as 'Name'  from ulogin">
</asp:SqlDataSource>
<%--<asp:SqlDataSource ID="dsgv" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="WebApp_SelectCmdV2" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="FormPanel$formLayout$CmbPlant" Name="plant" PropertyName="Value" />
        <asp:ControlParameter ControlID="FormPanel$formLayout$rbGrouping" Name="group" PropertyName="Value" />
        <asp:ControlParameter ControlID="hGeID" Name="Id" PropertyName="['GeID']"/></SelectParameters>
</asp:SqlDataSource>--%>