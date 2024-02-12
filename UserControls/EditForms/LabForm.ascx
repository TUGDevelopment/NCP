<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LabForm.ascx.cs" Inherits="UserControls_EditForms_LabForm" %>
<script type="text/javascript">
    function OnSelectedIndexChanged(s, e) {
        debugger;
        var b = s.GetText() == "Finish Goods" ? true : false;
        ClientShift.SetEnabled(b);
        ClientLines.SetEnabled(b);
        Clienttimes.SetEnabled(b);
        gv.PerformCallback('c|'+s.GetValue()); 
    }
    function testgrid_RowClick(s, e) {
        var key = s.GetRowKey(e.visibleIndex);
        if (key != null)
            DevAV.ChangeDemoState("MailForm", "EditDraft", key);
        //alert(key);
        //debugger;
        //DevAV.showClearedPopup(countEditPopup);
        //countEditPopup.PerformCallback('read|' + key);
    }
    var curentEditingIndex;
    var isSetTextRequired = false;
    var preventEndEditOnLostFocus = false;
    var text = "";

    function Grid_BatchEditStartEditing(s, e) {
              curentEditingIndex = e.visibleIndex;
              var productNameColumn = s.GetColumnByField("Material");
              if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                  return;
              var cellInfo = e.rowValues[productNameColumn.index];

              isSetTextRequired = true;
              text = cellInfo.text;
              gridLookup.GetInputElement().value = text;
              if (e.focusedColumn === productNameColumn)
                  gridLookup.SetFocus();
          }
          function Grid_BatchEditEndEditing(s, e) {
              var productNameColumn = s.GetColumnByField("Material");
              if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                  return;
              var cellInfo = e.rowValues[productNameColumn.index];
              cellInfo.value = gridLookup.GetInputElement().value;
              cellInfo.text = gridLookup.GetText();
              UpdateApi();
              //window.setTimeout(function () {
              //    gv.UpdateEdit();
              //}, 0);
          }

     function OnApplid(s, e) {
            testgrid.ApplyFilter(e.filterExpression);
            testgrid.PerformCallback('filter|0');
        }
    function onFileUploadComplete(s, e) {
        if (e.callbackData) {
            debugger;
            var fileData = e.callbackData.split('|');
            var fileName = fileData[0],
                fileUrl = fileData[1],
                fileSize = fileData[2];
            DevAV.AddFile(fileName, fileUrl, fileSize);
        }
    }
    function CloseGridLookup() {
        gridLookup.ConfirmCurrentSelection();
        gridLookup.HideDropDown();
        gridLookup.Focus();
    }
    function Combo_TextChanged(s, e) {
        debugger;
        //var g = s.GetGridView();
        //g.GetRowValues(g.GetFocusedRowIndex(), "Description", OnGetValues);  
        var para = s.GetSelectedItem().GetColumnText(1);
        //gv.GetEditor("Name").SetValue(para);
        gv.batchEditApi.SetCellValue(curentEditingIndex, "Name", para);
    }
    //function OnGetValues(array) {
    //    if (array && array.length) {
    //            gv.GetEditor("Name").SetValue(array);
    //    }
    //}
    function gvLookupChanged(s, e) {
        debugger;
        var param = s.GetValue();
        gv.SetEditValue("SalesOrder", param);
        var index = CmbMaterialType.GetValue();
        var g = s.GetGridView();
        g.GetValuesOnCustomCallback("GetOrder|" + index + "|" + param, function (r) {
            if (!r)
                return;
            gv.batchEditApi.SetCellValue(curentEditingIndex, "SalesItems", r["Item"]);
            gv.batchEditApi.SetCellValue(curentEditingIndex, "Material", r["Material"]);
        })
    }
    function OnLookupChanged(s, e) {
        var param = s.GetText();
        //var args = gv.GetEditor(1).GetValue();
        gv.SetEditValue("Material", param);
        //s.SetText(args[1]);
        //gv.GetEditor("Description").SetValue(args[2]);
        //var index = cbMaterialType.GetSelectedItem().value;
        var index = CmbMaterialType.GetValue();
        var g = s.GetGridView();
            //g.GetRowValues(g.GetFocusedRowIndex(), "Production;Description", OnGetSelectedFieldValues);
            //s.GetGridView().PerformCallback(arge);
            g.GetValuesOnCustomCallback("GetBatch|"+ index +"|" + param, function (r) {
                if (!r)
                    return;
                RecieveGridValues(r);
        })
    }
    function OnGetSelectedFieldValues(array) {
        if (array && array.length) {
            var a = ["ProductCode", "Description"];
            for (i = 0; i < a.length; i++) {
                //gv.GetEditor(a[i]).SetValue(array[i]);
                gv.batchEditApi.SetCellValue(curentEditingIndex, a[i], array[i]);
            }
        }
    }
    function RecieveGridValues(r) {
        debugger;
        //gv.GetEditor("BatchCode").SetValue(r["BatchCode"]);
        //gv.GetEditor("BatchSap").SetValue(r["BatchSap"]);
        if (r["index"] == '1') {
            gv.batchEditApi.SetCellValue(curentEditingIndex, "BatchSap", r["BatchSap"]);
            gv.batchEditApi.SetCellValue(curentEditingIndex, "ProductCode", r["ProductCode"]);
            gv.batchEditApi.SetCellValue(curentEditingIndex, "Description", r["Description"]);
        } else {
            //var value = s.GetGridView().SelectRows([1,2]);  
            gv.batchEditApi.SetCellValue(curentEditingIndex, "Description", r["Description"]);
        }
    }
    var lastPlant = null;
    function OnPlantChanged(r) {
        //var r = s.GetValue().toString();
        //alert(r);
        ClientTypes.PerformCallback(r.toString());
        ClientShift.PerformCallback(r.toString());
        ClientLines.PerformCallback(r.toString());
        CmbRecorder.PerformCallback(r.toString());
        if(hGeID.Get("GeID")=="0")
        OnGetApprove(r.toString());
    }
    function OnGetApprove(r) {
        cmbApprove1.PerformCallback(r);
        cmbApprove2.PerformCallback(r.toString());
    }
    function OnEndCallback(s, e) {
        debugger;
        var result =  ["Problem",cmbplant.GetText(), ClientTypes.GetText()].join("|");
                grid.GetEditor("Problem").PerformCallback(result);
        }
       var lastValidationResult = false;
        function OnContactMethodChanged(s, e) {
            var selectedIndex = s.GetActiveTabIndex();
            UpdateTabListDecoration(s);
            tabbedGroupPageControl.SetActiveTabIndex(selectedIndex);
            if (selectedIndex == 1) {
            var b = true;
            fileManager.PerformCallback(['load', b].join("|"));
            }
        }
        function UpdateTabListDecoration(s) {
            //debugger;
            var selectedIndex = s.GetActiveTabIndex();
            var b = selectedIndex == 4;
            //formLayout.GetItemByName("g4").SetVisible(b);
            //var selectedIndex = s.GetSelectedIndex();
            //for (var i = 0; i < s.GetItemCount(); i++)
            //    s.GetItemElement(i).parentNode.className = i === selectedIndex ? "selectedElement" : "";
        }
        function OnTabbedGroupPageControlInit(s, e) {
            s.SetActiveTabIndex(TabList.GetActiveTabIndex());
        }
        function OnSubmitButtonClick() {
            if (lastValidationResult)
                alert('Thank you!');
        }
    function OnValueChanged(s, e) {
        debugger;
        var res = s.GetText();
        var regex=/^[a-zA-Z]+$/;
        var result = res.match(regex);
        if (res != 'X') {
            cbShift.SetValue(result ? 'NS' : 'DS');
        }
    }
    function OnToolbarItemClick(s, e) {
        debugger;
        //if (e.item.name == "New")
        //    gv.AddNewRow();
        if (IsCustomExportToolbarCommand(e.item.name)) {
            e.processOnServer = true;
            e.usePostBack = true;
        }
    }
    function IsCustomExportToolbarCommand(command) {
        return command == "CustomExportToXLS" || command == "CustomExportToXLSX";
    }
    function OnBtnShowPopupClick(param) {
        debugger;
        //var height = Math.max(0, document.documentElement.clientHeight);
        //filterPopup.SetHeight(height);
        if (param == 'getsales') {
            PopupControl.RefreshContentUrl();
            PopupControl.SetContentUrl('src/selectedvalue.aspx?ID=1');
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
    function HidePopupAndShowInfo(closedBy, returnValue) {
        debugger;
        if (closedBy == 'Client') {
            PopupControl.Hide();
            //alert('Closed By: ' + closedBy + '\nReturn Value: ' + returnValue);
            var res = returnValue.split("|");
            gv.batchEditApi.SetCellValue(curentEditingIndex, "SalesOrder", res[0]);
            gv.batchEditApi.SetCellValue(curentEditingIndex, "SalesItems", res[1]);
            gv.batchEditApi.SetCellValue(curentEditingIndex, "ProductCode", res[2]);
            UpdateApi();
        }
    }
    function UpdateApi() {
        window.setTimeout(function () {
            gv.UpdateEdit();
        }, 0);
    }
    function OnButtonClick(s, e) {
        OnBtnShowPopupClick('getsales');
    }
</script>
<dx:ASPxHiddenField ID="hGeID" runat="server" ClientInstanceName="hGeID"/>
<dx:ASPxHiddenField ID="hKeyword" runat="server" ClientInstanceName="hKeyword" />
<dx:ASPxHiddenField ID="heditor" runat="server" ClientInstanceName="heditor" />
<%--<dx:ASPxHiddenField ID="hMaterialType" runat="server" ClientInstanceName="hMaterialType" />--%>
<dx:ASPxGridView ID="testgrid" ClientInstanceName="testgrid" runat="server" Width="100%" KeyFieldName="ID" DataSourceID="dsgv"
    OnCustomDataCallback="testgrid_CustomDataCallback"  OnHtmlDataCellPrepared="testgrid_HtmlDataCellPrepared" onCustomButtonCallback="testgrid_CustomButtonCallback"
    OnCustomCallback="testgrid_CustomCallback" CssClass="grid">
    <ClientSideEvents Init="DevAV.Init" RowClick="testgrid_RowClick" EndCallback="DevAV.test" CustomButtonClick="function(s, e) {  
        e.processOnServer = confirm('Do you really want to apply changes?'); }" />
    <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="500" />
    <SettingsEditing Mode="Inline"></SettingsEditing>
    <Columns>
    <dx:GridViewCommandColumn ShowNewButton="true" ShowEditButton="true" VisibleIndex="0" ButtonRenderMode="Image" Width="45" 
        CellStyle-CssClass="unitPriceColumn" HeaderStyle-CssClass="unitPriceColumn">
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="Clone">
                    <Image ToolTip="Clone Record" Url="~/Content/Images/Copy.png"/>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
        </dx:GridViewCommandColumn>
        <dx:GridViewDataColumn FieldName="ID" />
        <dx:GridViewDataColumn FieldName="NCPID" />
        <dx:GridViewDataDateColumn FieldName="KeyDate" />
        <dx:GridViewDataDateColumn FieldName="ProductionDate" />
        <dx:GridViewDataColumn FieldName="MaterialType"/>
        <dx:GridViewDataColumn FieldName="Location"/>
        <dx:GridViewDataColumn FieldName="Problem"/>
        <dx:GridViewDataComboBoxColumn FieldName="Approve">
            <PropertiesComboBox DataSourceID="dsuser" TextField="fn" ValueField="user_name" />
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataComboBoxColumn FieldName="Approvefinal">
            <PropertiesComboBox DataSourceID="dsuser" TextField="fn" ValueField="user_name" />
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataDateColumn FieldName="CreateOn" >  
            <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy HH:mm"></PropertiesDateEdit>  
        </dx:GridViewDataDateColumn> 
        <dx:GridViewDataColumn FieldName="Active"/>
    </Columns>
    <Styles>
        <Cell Wrap="False"/>
        <focusedrow BackColor="#f4dc7a" ForeColor="Black"/>
    </styles>
    <SettingsContextMenu Enabled="true"/>
    <SettingsPager PageSize="20" EnableAdaptivity="true">
        <PageSizeItemSettings Visible="true" ShowAllItem="true" AllItemText="All Records" />
    </SettingsPager>
    <TotalSummary>
        <dx:ASPxSummaryItem FieldName="ReceivingDate" SummaryType="Count" />
    </TotalSummary>
    <settingspager pagesize="50" numericbuttoncount="6" />
</dx:ASPxGridView>
<dx:ASPxGridViewExporter runat="server" ID="GridExporter" GridViewID="testgrid" />
<dx:ASPxButton ID="btn" runat="server" ClientInstanceName="btn" ClientVisible="false" OnClick="btn_Click" />
<dx:ASPxCallbackPanel ID="FormPanel" runat="server" RenderMode="Div" ClientVisible="false" ScrollBars="Auto" ClientInstanceName="ClientFormPanel">
        <PanelCollection>
            <dx:PanelContent>  
        <dx:ASPxFormLayout ID="formLayout" ClientInstanceName="formLayout" CssClass="formLayout" runat="server" AlignItemCaptionsInAllGroups="True" UseDefaultPaddings="false">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="800" />
        <Items>
        <dx:LayoutGroup Caption="" GroupBoxDecoration="None" ColCount="4">
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
                <dx:LayoutItem Caption="Material Type">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <%--<dx:ASPxRadioButtonList CaptionSettings-Position="Top" ID="cbMaterialType" runat="server" TextField="Name" RepeatColumns="6" RepeatDirection="Horizontal" ValueField="ID" 
                                DataSourceID="dsMaterialType" Paddings-Padding="0px" ClientInstanceName="cbMaterialType"  
                                RepeatLayout="Flow" Border-BorderStyle="None" Paddings-PaddingLeft="0px" FocusedStyle-Border-BorderStyle="None">   
                                <Paddings Padding="0px" PaddingBottom="1px" PaddingTop="10px" />
                                <ClientSideEvents SelectedIndexChanged="OnSelectedIndexChanged" />--%>
                                <dx:ASPxComboBox ID="CmbMaterialType" runat="server" ClientInstanceName="CmbMaterialType" DataSourceID="dsMaterialType" 
                                    OnCallback="CmbMaterialType_Callback" TextField="Name" ValueField="ID">
                                <ClientSideEvents SelectedIndexChanged="OnSelectedIndexChanged" />
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Plant">
                    <SpanRules>
                        <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                        <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                    </SpanRules>
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:aspxcombobox CaptionSettings-Position="Top" ID="cmbplant" runat="server" IncrementalFilteringMode="StartsWith"
                                TextField="value" ValueField="Id" EnableSynchronization="False"
                                ClientInstanceName="cmbplant" DataSourceID="dsPlant">      
                                <ClientSideEvents TextChanged="function(s, e) { OnPlantChanged(s.GetValue()); }" />
                                <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" ValidationGroup="group1"/>
                            </dx:aspxcombobox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Shift Option">
                    <SpanRules>
                        <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                        <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                    </SpanRules>
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:aspxcombobox CaptionSettings-Position="Top" ID="CmbShift" runat="server" OnCallback="CmbShift_Callback"
                                TextField="Name" ValueField="Id" 
                                ClientInstanceName="ClientShift">   
                                <ClientSideEvents ValueChanged="OnValueChanged" />
                            </dx:aspxcombobox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Line">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:aspxcombobox CaptionSettings-Position="Top" ID="cmbLine" runat="server" OnCallback="cmbLine_Callback"
                                TextField="Lname" ValueField="Id" 
                                ClientInstanceName="ClientLines">      
                            </dx:aspxcombobox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                    <dx:LayoutItem Caption="NCP Number">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtncpid" runat="server" ClientInstanceName="txtncpid" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Key Date" >
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxDateEdit ID="deKeyDate" CaptionSettings-Position="Top" runat="server" ClientInstanceName="deKeyDate" 
                                EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                DisplayFormatString="dd-MM-yyyy">
                                <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true">
                                    <RequiredField IsRequired="true" ErrorText="Any Date is required" />
                                </ValidationSettings>
                                <ClientSideEvents Init="function(s,e){ s.SetDate(new Date());}" />
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>  
                <dx:LayoutItem Caption="Time">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:aspxcombobox CaptionSettings-Position="Top" ID="Cmbtimes" runat="server" DataSourceID="dsTime"
                                TextField="Tname" ValueField="Id" 
                                ClientInstanceName="Clienttimes">      
                            </dx:aspxcombobox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="NCP Type">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:aspxcombobox CaptionSettings-Position="Top" ID="cmbncpType" DataSourceID="dsncptype" runat="server"  OnCallback="cmbncpType_Callback" 
                                TextField="Title" ValueField="Id" 
                                ClientInstanceName="ClientTypes">   
                                <ClientSideEvents SelectedIndexChanged="function(s, e) { var r = cmbplant.GetValue().toString();
                                    Cmblocation.PerformCallback(['Location',s.GetText(),r.toString()].join('|'));}" />
                                <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" ValidationGroup="group1"/>
                            </dx:aspxcombobox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Record By">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:aspxcombobox CaptionSettings-Position="Top" ID="CmbRecorder" runat="server" OnCallback="CmbRecorder_Callback"
                                TextField="Name" ValueField="Id" 
                                ClientInstanceName="CmbRecorder">      
                            </dx:aspxcombobox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem> 
                    <dx:LayoutItem Caption="Production Date" >
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxDateEdit ID="dePrddate" CaptionSettings-Position="Top" runat="server" ClientInstanceName="dePrddate" 
                                EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                DisplayFormatString="dd-MM-yyyy">
                                <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true">
                                    <RequiredField IsRequired="true" ErrorText="Any Date is required" />
                                </ValidationSettings>
                                <ClientSideEvents Init="function(s,e){ s.SetDate(new Date());}" />
                            </dx:ASPxDateEdit>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>  

                <dx:LayoutItem Caption="Shift" Paddings-PaddingTop="0px">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxRadioButtonList ID="cbShift" runat="server" DataSourceID="dsShift" ValueField="value" RepeatDirection="Horizontal" ClientInstanceName="cbShift"  
                                    RepeatLayout="Flow" Border-BorderStyle="None" Paddings-PaddingLeft="0px" FocusedStyle-Border-BorderStyle="None" TextField="value">
                                    <Paddings Padding="0px" PaddingBottom="1px" PaddingTop="10px"/>
                                </dx:ASPxRadioButtonList>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Location">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:aspxcombobox CaptionSettings-Position="Top" ID="Cmblocation" runat="server" OnCallback="Cmblocation_Callback"
                                TextField="value" ValueField="value" 
                                ClientInstanceName="Cmblocation">      
                            </dx:aspxcombobox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>                        
                </Items>
            </dx:LayoutGroup>
  
        <dx:LayoutItem Caption="" Width="100%">
        <LayoutItemNestedControlCollection>
            <dx:LayoutItemNestedControlContainer>
                <dx:ASPxTabControl ID="TabList" runat="server" NameField="Id" DataSourceID="XmlDataSource1" ActiveTabIndex="0" ClientInstanceName="TabList" 
                    EnableTabScrolling="false"
                    TabAlign="Left">
                    <ClientSideEvents ActiveTabChanged="OnContactMethodChanged" Init="UpdateTabListDecoration"/>
                </dx:ASPxTabControl>
                <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Platforms.xml"
                    XPath="//lab"></asp:XmlDataSource>
            </dx:LayoutItemNestedControlContainer>
        </LayoutItemNestedControlCollection>
    </dx:LayoutItem>
    <dx:TabbedLayoutGroup Caption="TabbedGroup" ClientInstanceName="tabbedGroupPageControl" ShowGroupDecoration="false" Width="100%">
        <ParentContainerStyle CssClass="tabbedGroupPageControlCell" />
        <ClientSideEvents Init="OnTabbedGroupPageControlInit" />
                <Items>
                <dx:LayoutGroup Caption="" GroupBoxDecoration="None" ColCount="2" SettingsItemCaptions-Location="Left">
                    <Items>                  
                   <dx:LayoutItem Caption="" Width="100%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxGridView runat="server" ID="gv" ClientInstanceName="gv" OnDataBinding="gv_DataBinding" KeyFieldName="Id" 
                                        OnInitNewRow="gv_InitNewRow" OnBatchUpdate="gv_BatchUpdate" 
                                        OnCustomCallback="gv_CustomCallback" 
                                        OnCustomButtonCallback="gv_CustomButtonCallback">
                                        <ClientSideEvents BatchEditStartEditing="Grid_BatchEditStartEditing" 
                                            BatchEditEndEditing="Grid_BatchEditEndEditing"/>
                                        <SettingsEditing Mode="Batch">
                                            <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="Cell"/>
                                        </SettingsEditing>
		                                <Styles>
			                                <Row Cursor="pointer" />
		                                </Styles>
                                        <SettingsSearchPanel ColumnNames="" Visible="false" />
		                                <Settings ShowGroupedColumns="True" GridLines="Vertical" ShowFooter="false" ShowStatusBar="Hidden"/>
		                                <SettingsBehavior AutoExpandAllGroups="true" EnableRowHotTrack="True" ColumnResizeMode="Control" EnableCustomizationWindow="true" AllowFocusedRow="true" />
		                                <SettingsPager PageSize="5">
                                        <PageSizeItemSettings Visible="false" ShowAllItem="true"  AllItemText="All Records" />
                                        </SettingsPager>
					                <%--<Toolbars>                                    
                                    <dx:GridViewToolbar>
                                        <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                                        <Items>
                                            <dx:GridViewToolbarItem Command="New" />
                                            <dx:GridViewToolbarItem Command="Edit" />
                                            <dx:GridViewToolbarItem Command="Delete" />
                                            <dx:GridViewToolbarItem Command="Refresh" BeginGroup="true" AdaptivePriority="2" />
                                            <dx:GridViewToolbarItem Text="Export to" Image-IconID="actions_download_16x16office2013" BeginGroup="true" AdaptivePriority="1">
                                                <Items>
                                                    <dx:GridViewToolbarItem Command="ExportToPdf" />
                                                    <dx:GridViewToolbarItem Command="ExportToDocx" />
                                                    <dx:GridViewToolbarItem Command="ExportToRtf" />
                                                    <dx:GridViewToolbarItem Command="ExportToCsv" />
                                                    <dx:GridViewToolbarItem Command="ExportToXls" Text="Export to XLS(DataAware)" />
                                                    <dx:GridViewToolbarItem Name="CustomExportToXLS" Text="Export to XLS(WYSIWYG)" Image-IconID="export_exporttoxls_16x16office2013" />
                                                    <dx:GridViewToolbarItem Command="ExportToXlsx" Text="Export to XLSX(DataAware)" />
                                                    <dx:GridViewToolbarItem Name="CustomExportToXLSX" Text="Export to XLSX(WYSIWYG)" Image-IconID="export_exporttoxlsx_16x16office2013" />
                                                </Items>
                                            </dx:GridViewToolbarItem>
                                            <dx:GridViewToolbarItem Alignment="Right">
                                                <Template>
                                                    <dx:ASPxButtonEdit ID="tbToolbarSearch" runat="server" NullText="Search..." Height="100%">
                                                        <Buttons>
                                                            <dx:SpinButtonExtended Image-IconID="find_find_16x16gray" />
                                                        </Buttons>
                                                    </dx:ASPxButtonEdit>
                                                </Template>
                                            </dx:GridViewToolbarItem>
                                        </Items>
                                    </dx:GridViewToolbar>
                                </Toolbars>
                                        <Columns>
                                             <dx:GridViewCommandColumn  ShowClearFilterButton="true" Width="42px" ButtonRenderMode="Image"
                                                FixedStyle="Left" ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true">   
                                                 <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="Delete">
                                                        <Image ToolTip="Remove" Url="~/Content/Images/Cancel.gif"/>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    <dx:ASPxButton runat="server" RenderMode="Link" AutoPostBack="false">
                                                        <Image ToolTip="Insert" Url="~/Content/Images/icons8-plus-math-filled-16.png" />
                                                        <ClientSideEvents Click="function(s,e){ gv.AddNewRow(); 
                                                            DevAV.SetUnitPriceColumnVisibility(); }" />
                                                    </dx:ASPxButton>
                                                </HeaderTemplate>
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataColumn FieldName="Material">
                                                <EditItemTemplate>
                                                    <dx:ASPxGridLookup ID="GridLookup" runat="server" SelectionMode="Single" AutoGenerateColumns="false" ClientInstanceName="gridLookup" 
                                                ClientSideEvents-ValueChanged="OnLookupChanged"
                                                DataSourceID="dsMaterial" IncrementalFilteringMode="Contains" MultiTextSeparator="; "  Value='<%# Bind("Material") %>' 
                                                KeyFieldName="Material" Width="400px" TextFormatString="{1}" Caption="">
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="Production" Settings-AutoFilterCondition="Contains" />
                                                    <dx:GridViewDataColumn FieldName="Material" Settings-AutoFilterCondition="Contains" />
                                                    <dx:GridViewDataColumn FieldName="Description" Settings-AutoFilterCondition="Contains"/>
                                                </Columns>
                                                <GridViewProperties>
                                                    <Templates>
                                                        <StatusBar>
                                                            <table class="OptionsTable" style="float: right">
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </StatusBar>
                                                    </Templates>

                                                    <SettingsBehavior AllowDragDrop="False" EnableRowHotTrack="True" />
                                                    <Settings ShowFilterRow="True" ShowStatusBar="Visible" />
                                                </GridViewProperties>
                                            </dx:ASPxGridLookup>
                                                </EditItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="ProductCode" />
                                            <dx:GridViewDataColumn FieldName="BatchCode" />                                                        
                                            <dx:GridViewDataColumn FieldName="BatchSap" />
                                            <dx:GridViewDataColumn FieldName="Description" />
                                            <dx:GridViewDataColumn FieldName="BatchPKG" CellStyle-CssClass="unitPriceColumn" HeaderStyle-CssClass="unitPriceColumn"/>
                                            <dx:GridViewDataComboBoxColumn FieldName="Supplier" CellStyle-CssClass="unitPriceColumn" HeaderStyle-CssClass="unitPriceColumn">
                                                <PropertiesComboBox DataSourceID="dsSupplier" TextField="Name" ValueField="Id" />
                                            </dx:GridViewDataComboBoxColumn>
                                        </Columns>--%>
                                        <SettingsCommandButton>
                                            <UpdateButton RenderMode="Image">
                                                <Image ToolTip="Save changes and close Edit Form" Url="~/Content/Images/update.png" />
                                            </UpdateButton>
                                            <CancelButton RenderMode="Image">
                                                <Image ToolTip="Close Edit Form without saving changes" Url="~/Content/Images/cancel.png" />
                                            </CancelButton>
                                            <NewButton RenderMode="Image">
                                                <Image ToolTip="Insert" Url="~/Content/Images/icons8-plus-math-filled-16.png" />
                                            </NewButton>
                                        </SettingsCommandButton>
                                    </dx:ASPxGridView>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                <dx:LayoutItem Caption="Option" Width="100%">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxRadioButtonList ID="cbOptfull" runat="server" DataSourceID="dsfull" ValueField="idx" RepeatColumns="4" RepeatDirection="Horizontal" ClientInstanceName="cbOptfull" 
                                RepeatLayout="Flow" Border-BorderStyle="None" Paddings-PaddingLeft="1px" FocusedStyle-Border-BorderStyle="None" TextField="value">
                            </dx:ASPxRadioButtonList>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
               
                <dx:LayoutItem Caption="Hold Quantity">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxTextBox CaptionSettings-Position="Top" ID="tbHoldQuantity" runat="server" ClientInstanceName="tbHoldQuantity"/>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>  
                <dx:EmptyLayoutItem />      
                <dx:LayoutItem Caption="Problem Quantity">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxTextBox CaptionSettings-Position="Top" ID="tbProblemqty" runat="server" ClientInstanceName="tbProblemqty"/>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>   
            <%--<dx:LayoutItem Caption="Quatity">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer>
                        <dx:ASPxTextBox CaptionSettings-Position="Top" ID="tbQuatity" runat="server" ClientInstanceName="tbQuatity"/>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>   --%>            
                 <dx:LayoutItem Caption="Final Decision">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:aspxcombobox CaptionSettings-Position="Top" ID="cmbdecision" runat="server" DataSourceID="dsdecision"
                                TextField="Title" ValueField="Id" 
                                ClientInstanceName="cmbdecision">      
                            </dx:aspxcombobox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
               <dx:LayoutItem Caption="FirstDecision">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:aspxcombobox CaptionSettings-Position="Top" ID="cmbFirstDecision" runat="server" DataSourceID="dsFirstDecision"
                                TextField="Title" ValueField="Id" 
                                ClientInstanceName="cmbFirstDecision">      
                            </dx:aspxcombobox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Action">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtAction" runat="server" ClientInstanceName="txtAction"/>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Result Decision">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxMemo CaptionSettings-Position="Top" runat="server" ID="mResultDecision" Rows="6" ClientInstanceName="mResultDecision"/>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
            <dx:LayoutItem Caption="Remark">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer>
                        <dx:ASPxMemo CaptionSettings-Position="Top" runat="server" ID="mRemark" Rows="6" ClientInstanceName="mRemark"/>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
               <dx:LayoutItem Caption="Approve step 1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox CaptionSettings-Position="Top" ID="cmbApprove1" runat="server" DataSourceID="dsApprove"  OnCallback="cmbApprove1_Callback"
                                    IncrementalFilteringMode="StartsWith" EnableSynchronization="False"
                                 ValueField="user_name" TextField="fn" ClientInstanceName="cmbApprove1">      
                            </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
               <dx:LayoutItem Caption="Approve step 2">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox CaptionSettings-Position="Top" ID="cmbApprove2" runat="server" DataSourceID="dsApprovefinal" OnCallback="cmbApprove2_Callback"
                                ValueField="user_name" TextField="fn" ClientInstanceName="cmbApprove2">
                            </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>  
                <dx:LayoutItem Caption="Problem" Width="100%">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxGridView runat="server" ID="grid" ClientInstanceName="grid" KeyFieldName="Id" OnDataBinding="grid_DataBinding" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                                        OnCellEditorInitialize="grid_CellEditorInitialize" OnCustomButtonCallback="grid_CustomButtonCallback"
                                        OnRowInserting="grid_RowInserting" 
                                        OnCustomCallback="grid_CustomCallback"
                                        OnRowUpdating="grid_RowUpdating">
                                        <ClientSideEvents RowClick="function(s,e){ s.StartEditRow(e.visibleIndex); }" />
                                            <Columns>
                                             <dx:GridViewCommandColumn  ShowClearFilterButton="true" Width="42px" ButtonRenderMode="Image"
                                                FixedStyle="Left" ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="Edit">
                                                        <Image ToolTip="Remove" Url="~/Content/Images/Cancel.gif"/>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    <dx:ASPxButton runat="server" RenderMode="Link" AutoPostBack="false">
                                                        <Image ToolTip="Insert" Url="~/Content/Images/icons8-plus-math-filled-16.png" />
                                                        <ClientSideEvents Click="function(s,e){ grid.AddNewRow();}" />
                                                    </dx:ASPxButton>
                                                </HeaderTemplate>
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataComboBoxColumn FieldName="Problem" Width="25%">
                                                <PropertiesComboBox ValueField="Title" TextFormatString="{0}" 
                                                    DataSourceID="dsProblem">
                                                    <ClientSideEvents EndCallback="OnEndCallback" />
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="Title"/>
                                                    <dx:ListBoxColumn FieldName="LongText"/>
                                                </Columns></PropertiesComboBox>
                                            </dx:GridViewDataComboBoxColumn>
                                            <dx:GridViewDataMemoColumn PropertiesMemoEdit-Rows="3" FieldName="Detail" />
                                        </Columns>
                                        <SettingsEditing UseFormLayout="True" Mode="EditForm" EditFormColumnCount="1" />
                                        <SettingsCommandButton>
                                            <UpdateButton RenderMode="Image">
                                                <Image ToolTip="Save changes and close Edit Form" Url="~/Content/Images/update.png" />
                                            </UpdateButton>
                                            <CancelButton RenderMode="Image">
                                                <Image ToolTip="Close Edit Form without saving changes" Url="~/Content/Images/cancel.png" />
                                            </CancelButton>
                                        </SettingsCommandButton>
                                    </dx:ASPxGridView>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutGroup SettingsItemCaptions-Location="Top">
                    <Items>
                        <dx:LayoutItem Caption="" Name="UploadControl">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <table>
                                    <tr>
                                        <td>
                                        <div class="uploadContainer">
                                            <dx:ASPxUploadControl ID="UploadControl" runat="server" ClientInstanceName="UploadControl" Width="240px"
                                                 NullText="Select multiple files..." UploadMode="Advanced" ShowUploadButton="false" ShowProgressPanel="True"
                                                 OnFileUploadComplete="UploadControl_FileUploadComplete">
                                                 <AdvancedModeSettings EnableMultiSelect="True" EnableFileList="True" EnableDragAndDrop="True" />
                                                 <ValidationSettings MaxFileSize="10485760" AllowedFileExtensions=".jpg,.jpeg,.gif,.png,.xls,.xlsx,.pdf">
                                                 </ValidationSettings>
                                                 <ClientSideEvents FilesUploadStart="function(s, e) { DevAV.Clear(); }"
                                                                  FileUploadComplete="onFileUploadComplete" />
                                            </dx:ASPxUploadControl>
                                        </div>
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td>
                                            <div class="filesContainer">
                                                  <dx:UploadedFilesContainer ID="FileContainer" runat="server" Width="380" Height="180" 
                                                NameColumnWidth="240" SizeColumnWidth="70" HeaderText="Uploaded files" /></div>
                                        </td>
                                    </tr>
                                </table>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="" Name="fileManager">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxFileManager ID="fileManager" runat="server" DataSourceID="dsFileSystem" OnCustomCallback="fileManager_CustomCallback" Height="240px" 
                                        OnFileUploading="fileManager_FileUploading"
                                        ClientInstanceName="fileManager"> 
                                        <Settings ThumbnailFolder="~/Content/FileManager" InitialFolder="Salvador Dali\1936 - 1945" 
                                            AllowedFileExtensions=".rtf, .pdf, .doc, .docx, .odt, .txt, .xls, .xlsx, .xlsb, .ods, .ppt, .pptx, .odp, .jpe, .jpeg, .jpg, .gif, .png , .msg"/>
                                        <SettingsFileList>
                                            <ThumbnailsViewSettings ThumbnailWidth="100" ThumbnailHeight="100" />
                                        </SettingsFileList>
                                        <SettingsToolbar ShowPath="false" ShowRefreshButton="false" />
                                        <SettingsDataSource KeyFieldName="ID" ParentKeyFieldName="ParentID" NameFieldName="Name" IsFolderFieldName="IsFolder" FileBinaryContentFieldName="Data" LastWriteTimeFieldName="LastWriteTime" />
                                        <SettingsEditing AllowCreate="false" AllowDelete="true" AllowMove="false" AllowRename="false" AllowDownload="true" />
                                        <SettingsBreadcrumbs Visible="true" ShowParentFolderButton="true" Position="Top" />
                                        <SettingsUpload UseAdvancedUploadMode="true" Enabled="false">
                                            <AdvancedModeSettings EnableMultiSelect="true"  />
                                            <ValidationSettings 
                                                MaxFileSize="10485760" 
                                                MaxFileSizeErrorText="The file you are trying to upload is larger than what is allowed (10 MB).">
                                            </ValidationSettings>
                                        </SettingsUpload>
					                <Settings EnableMultiSelect="true" />
                                    </dx:ASPxFileManager>
                                   <br />
                                    <p class="note">
                                        <dx:ASPxLabel ID="AllowedFileExtensionsLabel" runat="server" Text="Allowed file extensions: .jpg, .jpeg, .gif, .png, .xls, .xlsx, .pdf." Font-Size="8pt">
                                        </dx:ASPxLabel>
                                        <br />
                                        <dx:ASPxLabel ID="MaxFileSizeLabel" runat="server" Text="Maximum file size: 10 MB." Font-Size="8pt">
                                        </dx:ASPxLabel>
                                    </p>
                                   </dx:LayoutItemNestedControlContainer>
                               </LayoutItemNestedControlCollection>
                           </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutGroup>
                    <Items>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxGridView ID="gvlog" runat="server">
                                        <Settings ShowColumnHeaders="false" VerticalScrollBarMode="Visible" VerticalScrollableHeight="500"/>
                                    </dx:ASPxGridView>
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
<dx:ASPxPopupControl ID="PopupControl" runat="server" ClientInstanceName="PopupControl" CloseAction="CloseButton" CloseOnEscape="true" 
            Width="620px" Height="500px"
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
                    <dx:FilterControlDateColumn ColumnType="DateTime" PropertyName="KeyDate" DisplayName="KeyDate">
                        <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" />
                    </dx:FilterControlDateColumn>
                    <dx:FilterControlTextColumn ColumnType="String" PropertyName="NCPID" DisplayName="NCPID" />
                    <dx:FilterControlTextColumn ColumnType="String" PropertyName="TypeNCP" DisplayName="TypeNCP" />
                </Columns>
                <ClientSideEvents Applied="function(s, e) { testgrid.ApplyFilter(e.filterExpression);
                    testgrid.PerformCallback('filter|0');}" />
            </dx:ASPxFilterControl>
            <div style="text-align: right">
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
<asp:SqlDataSource ID="dsgv" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>" 
    SelectCommand="WebApp_SelectCmdV4" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:SessionParameter Name="Keyword" SessionField="Expr" Type="String" />
        <asp:SessionParameter Name="user" SessionField="user_name" Type="String" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsPlant" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>" 
    SelectCommand="spGetPlant" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:SessionParameter Name="user" SessionField="user_name" Type="String" />
    </SelectParameters>
    </asp:SqlDataSource>
<asp:SqlDataSource ID="dsShift" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from dbo.FNC_SPLIT('DS,NS',',')">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsfull" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from dbo.FNC_SPLIT('Hold full,Hold Not full',',')">
</asp:SqlDataSource>

<%--<asp:SqlDataSource ID="dsList" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select 1 idx,'' ProductCode,'' BatchCode,''Material,''Batch,''Description,''Supplier">
</asp:SqlDataSource>--%>
<asp:SqlDataSource ID="dsMaterialType" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spMaterialType" SelectCommandType="StoredProcedure">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsSupplier" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from(select * from tblSupplier union select '0' ,'-')#a ORDER BY 
 CASE WHEN Id='0' then '' else Id END ASC">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsncptype" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetncptype" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="FormPanel$formLayout$cmbplant" Name="Plant" PropertyName="Value" />
        <%--<asp:ControlParameter ControlID="FormPanel$formLayout$cpMaterialType$CmbMaterialType" Name="Id" PropertyName="Value" />--%>
        <asp:ControlParameter ControlID="FormPanel$formLayout$CmbMaterialType" Name="Id" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsTime" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from tblTime order by Tname">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsfirstdecision" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from tblfirstdecision order by Title">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsdecision" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from tbldecision order by Title">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsUser" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select *,concat(firstname ,' ' ,lastname)fn from ulogin order by firstname,lastname">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsProblem" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetProblem" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="FormPanel$formLayout$cmbncpType" Name="Type" PropertyName="Text" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsMaterial" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetMaterial" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:Parameter Name="Type" Type="String" />
        <%--<asp:ControlParameter ControlID="FormPanel$formLayout$cbMaterialType" Name="Type" PropertyName="Value" />--%>
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsPackaging" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetMaterial" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:Parameter Name="Type" DefaultValue="2" />
    </SelectParameters>
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
<asp:SqlDataSource ID="dsApprove" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetApprove" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="FormPanel$formLayout$cmbplant" Name="Plant" PropertyName="Value" />
        <asp:Parameter Name="args" DefaultValue="Approve1" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsApprovefinal" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetApprove" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="FormPanel$formLayout$cmbplant" Name="Plant" PropertyName="Value" />
        <asp:Parameter Name="args" DefaultValue="Approve2" />
    </SelectParameters>
</asp:SqlDataSource>
<%--<asp:SqlDataSource ID="dsOrder" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetSalesOrder" SelectCommandType="StoredProcedure">
</asp:SqlDataSource>--%>