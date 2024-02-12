<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InprocessForm.ascx.cs" Inherits="UserControls_EditForms_IncomForm" %>
    <script type="text/javascript">
        //function AddTextBox() {
        //var div = document.createElement('DIV');
        //div.innerHTML = GetDynamicTextBox("");
        //document.getElementById("TextBoxContainer").appendChild(div);
        //}
        //function GetDynamicTextBox(value) {
        //    return '<input name = "DynamicTextBox" type="text" value = "' + value + '" />' +
        //        '<input type="button" value="Remove" onclick = "RemoveTextBox(this)" />'
        //}
        //function RemoveTextBox(div) {
        //    document.getElementById("TextBoxContainer").removeChild(div.parentNode);
        //}
        var focusedfieldName = false;
        function OnRowClick(s, e) {
            var key = s.GetRowKey(e.visibleIndex);
            gv.PerformCallback(['symbol',key].join('|'));
            var index = s.GetFocusedRowIndex();
            s.GetRowValues(e.visibleIndex, "SampleNo;Counts", RecieveGridValues);
            //s.StartEditRow(e.visibleIndex);
        }
        function RecieveGridValues(values) {
            var ar = values.join('|');
            
        }
        function OnApplid(s, e) {
            testgrid.ApplyFilter(e.filterExpression);
            testgrid.PerformCallback('filter');
        }
        function OnValueChanged(s, e) {
            ClientSpecies.SetValue(s.GetValue().substring(4, 5));
            //ClientSize.SetValue(s.GetValue().substring(5, 6));
            ClientStyle.SetValue(s.GetValue().substring(6, 7));
        }
        function onButtonClick(s, e) {
        }
        var lastValidationResult = false;
        function OnContactMethodChanged(s, e) {
            debugger;
            var selectedIndex = s.GetActiveTabIndex();
            UpdateTabListDecoration(s);
            tabbedGroupPageControl.SetActiveTabIndex(selectedIndex);
            //var arr = [4, 5, 6];
            //var b = arr.indexOf(selectedIndex);
            //formLayout.GetItemByName("gv").SetVisible(b==-1?false:true);
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
        function onDropDown(s, e) {
            var myDate = new Date(1970, 1, 1);
            s.SetDate(myDate);
            s.SetText("");
        }
        function testgrid_RowClick(s, e) {
            debugger;
            //TabList.SetActiveTabIndex(0);       
            var key = s.GetRowKey(e.visibleIndex);
            if (key != null)
                DevAV.ChangeDemoState("MailForm", "EditDraft", key);
            //alert(key);
            //debugger;
            //DevAV.showClearedPopup(countEditPopup);
            //countEditPopup.PerformCallback('read|' + key);
        }
        function UpdateRadioButtonListDecoration(radioButtonList) {

        }
        //function OnCustomButtonClick(s, e) {
        //    if (e.buttonID == "Edit") {
        //        var key = s.GetRowKey(e.visibleIndex);
        //        s.StartEditRow(e.visibleIndex);
        //        //alert(key);
        //        //
        //    }
        //}
        function OnSelectedIndexChanged(s, e) {
            debugger;
            //var selectedIndex = s.GetSelectedIndex();
            var selectedIndex = s.GetSelectedItem().value;
            //UpdateRadioButtonListDecoration(s);
            //TabbedGroup.SetActiveTabIndex(selectedIndex);
            gv.SetVisible(true);
            gv.PerformCallback('symbol|' + gv3.GetEditValue("Id"));
            f4.GetItemByName("gv").SetVisible(true);
            //gv.StartEditRow(0);
        }
        function OnContextMenuItemClick(sender, args) {
            if (args.objectType == "row") {
                if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                    var index = args.elementIndex;
                    var key = sender.GetRowKey(index);
                    if (key != null) {
                        var url = "./Test/Print.aspx?Id=" + key;
                        window.open(url, "_blank");
                    }
                    args.processOnServer = true;
                    args.usePostBack = true;
                } 
            }
        }
        function OnEndCallback(s, e) {
            if (s.cpKeyValue != undefined && s.cpKeyValue != "") {
                //alert(s.cpKeyValue);
                var value = s.cpKeyValue;
                gv.PerformCallback(value);
                delete s.cpKeyValue;
            }
        }
        var FocusedCellColumnIndex = 0;
        var FocusedCellRowIndex = 0;
        function OnInitGridBatch(s, e) {
            ASPxClientUtils.AttachEventToElement(s.GetMainElement(), "keydown", function (evt) {
                if (evt.keyCode == 40) {
                    ASPxClientUtils.PreventEventAndBubble(evt);
                    DownPressed(s);
                }
                if (evt.keyCode == 13) {
                    s.UpdateEdit();
                    //s.PerformCallback(['edit',0].join('|'));
                }
            });
        }
        function OnBatchEditStartAlimConstat(s, e) {
            FocusedCellColumnIndex = e.focusedColumn.index;
            FocusedCellRowIndex = e.visibleIndex;
        }
        function OnEndEditCell(s, e) {
            FocusedCellColumnIndex = 0;
            FocusedCellRowIndex = 0;
            var cellInfo = s.batchEditApi.GetEditCellInfo();
            debugger;
            setTimeout(function () {
                if (s.batchEditApi.HasChanges(cellInfo.rowVisibleIndex, cellInfo.column.index))
                    UpdateEdit(createObject(s, s.GetRowKey(e.visibleIndex), e.rowValues), cellInfo);
            }, 0);
            //window.setTimeout(function () {
            //    s.UpdateEdit();
            //}, 0);
        }
        function onEndUpdateCallback(s, e) {
            //lvwd.PerformCallback(['edit',0].join('|'));
        }
        function UpdateEdit(object, cellInfo) {
            debugger;
            //lvwd.PerformCallback('edit|'+[object].join('|'));
            callback.cpCellInfo = cellInfo;
            callback.PerformCallback(JSON.stringify(object));
        }
        function createObject(g, key, values) {
            var object = {};
            object["ID"] = key;
            Object.keys(values).map(function (k) {
                object[g.GetColumn(k).fieldName] = values[k].value;
            });
            return object;
        }
        function DownPressed(s) {
            var lastRecordIndex = s.GetTopVisibleIndex() + s.GetVisibleRowsOnPage() - 1;
            if (FocusedCellRowIndex < lastRecordIndex)
                s.batchEditApi.StartEdit(FocusedCellRowIndex + 1, FocusedCellColumnIndex);
            else
                s.batchEditApi.EndEdit();
        }

        function OnBatchEditEndEditing(s, e) {
            debugger;
            window.setTimeout(function () {
                s.UpdateEdit();
            }, 0);
        }
        var curentEditingIndex;
        function initNew(s, e) {
            gv3.AddNewRow();
            //e.VisibleIndex;
            //curentEditingIndex = s.GetEditValue("SampleNo");

            radioButtonList.SetSelectedIndex(-1);
        }
        var Countpersvalue;
        function OnTextChanged(s, e) {
            debugger;
            Countpersvalue = s.GetText();
            if (Countpersvalue != '' && Countpersvalue != 0) {
                var SampleNo = gv3.GetEditValue("SampleNo");
                gv.PerformCallback('reload|' + Countpersvalue + '|' + SampleNo);
                f4.GetItemByName("param").SetVisible(true);
            }
        }
        function OnGridBeginCallback(s, e) {
            if (e.command == "CANCELEDIT" || e.command == "UPDATEEDIT")
                GetBuildCalcu(false);
            if (e.command == "ADDNEWROW" || e.command == "STARTEDIT")
                GetBuildCalcu(true);
        }
        function GetBuildCalcu(b) {
            var arr = "param;gv";
            var g = arr.split(";"), s;
            for (s = 0; s < g.length; s++) {
                if (g[s] != "")
                    f4.GetItemByName(g[s]).SetVisible(b);
            }
            radioButtonList.SetSelectedIndex(-1);
        }
        function OnBatchEditEndEditing(s, e) {
            if (focusedfieldName)
                calculator(s);
            focusedfieldName = false;
        }
        function onFocusedCellChanging(s, e) {

            //var sapmat =s.batchEditApi.GetCellValue(s.GetFocusedRowIndex(), 'SAPMaterial');
            //if (e.cellInfo.column.fieldName == "PriceOfUnit" && s.batchEditApi.GetCellValue(s.GetFocusedRowIndex(), 'SAPMaterial') != null)
            //   e.cancel = true;
            if (e.cellInfo.column.fieldName == "ExchangeRate")
                e.cancel = true;
        }
        function OnBatchEditStartEditing(s, e) {
            FocusedCellColumnIndex = e.focusedColumn.index;
            curentEditingIndex = e.visibleIndex;
            var targetArray = ["Result"];
            if (targetArray.indexOf(e.focusedColumn.fieldName) > -1)
                focusedfieldName = true;
        }
        function calculator(s) {
            window.setTimeout(function () {
                var count_value = GetChangesCount(s.batchEditApi);
                if (count_value > 0)
                    s.UpdateEdit();
            }, 0);
        }
        function OnInitGridBatch(s, e) {
            ASPxClientUtils.AttachEventToElement(s.GetMainElement(), "keydown", function (evt) {
                if (evt.keyCode === ASPxClientUtils.StringToShortcutCode("Enter") || evt.keyCode === ASPxClientUtils.StringToShortcutCode("DOWN")) {
                    DownPressed(s);
                    ASPxClientUtils.PreventEventAndBubble(evt);
                }
            });
        }
        function DownPressed(s) {
            var lastRecordIndex = s.GetTopVisibleIndex() + s.GetVisibleRowsOnPage() - 1;
            if (curentEditingIndex < lastRecordIndex)
                s.batchEditApi.StartEdit(curentEditingIndex + 1, FocusedCellColumnIndex);
            else
                s.batchEditApi.EndEdit();
            //s.UpdateEdit();
        }
    </script>
<dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="hfEdits"/>
<dx:ASPxHiddenField ID="husername" runat="server" ClientInstanceName="husername"/>
<dx:ASPxHiddenField ID="hGeID" runat="server" ClientInstanceName="hGeID"/>
<dx:ASPxHiddenField ID="hKeyword" runat="server" ClientInstanceName="hKeyword" />
<dx:ASPxGridView ID="testgrid" ClientInstanceName="testgrid" runat="server" Width="100%" OnCustomCallback="testgrid_CustomCallback"
     OnFillContextMenuItems="testgrid_FillContextMenuItems" OnCustomDataCallback="testgrid_CustomDataCallback"
     DataSourceID="dsSample" KeyFieldName="Id">
    <Columns>
        <dx:GridViewDataColumn FieldName="Shifts" />
        <dx:GridViewDataColumn FieldName="SampleID" />
        <dx:GridViewDataColumn FieldName="Material" />
        <dx:GridViewDataColumn FieldName="Batch" />
        <dx:GridViewDataDateColumn FieldName="ReceivingDate" />
    </Columns>
    <ClientSideEvents Init="DevAV.Init" RowClick="testgrid_RowClick" EndCallback="DevAV.test" ContextMenuItemClick="function(s,e) { OnContextMenuItemClick(s, e); }"/>
    <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="500" />
    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
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
        <dx:ASPxSummaryItem FieldName="ReceivingDate" SummaryType="Count" />
    </TotalSummary>
    <SettingsBehavior allowfocusedrow="True" AutoExpandAllGroups="true" EnableRowHotTrack="True" ColumnResizeMode="Control" EnableCustomizationWindow="true" AllowSelectByRowClick="true"/>
    <settingspager pagesize="50" numericbuttoncount="6" />
    <SettingsContextMenu Enabled="true"/>
</dx:ASPxGridView>
<dx:ASPxCallbackPanel ID="FormPanel" runat="server" RenderMode="Div" ClientVisible="false" ScrollBars="Auto" ClientInstanceName="ClientFormPanel">
        <PanelCollection>
            <dx:PanelContent>  
            <dx:ASPxPageControl ID="citiesTabPage" Width="100%" runat="server" TabAlign="Left" TabPosition="Left" ActiveTabIndex="0" EnableHierarchyRecreation="True" >
            <TabPages>
                <dx:TabPage Text="Raw Material Description">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl1" runat="server">
                            <dx:ASPxFormLayout runat="server" ID="f1">
                                <Items>
                        <dx:LayoutGroup GroupBoxDecoration="None" ColCount="3" SettingsItemCaptions-Location="Left">
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
                            <dx:LayoutItem Caption="ID" ClientVisible="false">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtGetID" runat="server" ClientInstanceName="txtGetID" ReadOnly="true" Width="20%">      
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Request No" Width="100%" ClientVisible="false">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxLabel runat="server" ID="lbRequestNo" ClientInstanceName="ClientRequestNo" ReadOnly="true" Width="240px">
                                        </dx:ASPxLabel>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="SampleID"  Width="100%">
                                <SpanRules>
                                    <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                    <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                                </SpanRules>
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtSampleID" runat="server" ClientInstanceName="ClientSampleID" ReadOnly="true" Width="20%">      
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Shift"  Width="100%">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxRadioButtonList ID="cbShift" runat="server" DataSourceID="dsShift" ValueField="value" RepeatColumns="4" RepeatDirection="Horizontal" ClientInstanceName="cbShift" 
                                                RepeatLayout="Flow" Border-BorderStyle="None" Paddings-PaddingLeft="0px" FocusedStyle-Border-BorderStyle="None" TextField="value">
                                            </dx:ASPxRadioButtonList>
                                            <%--<dx:ASPxComboBox ID="cbShift" runat="server" CaptionSettings-Position="Top" ValueType="System.String" 
                                                DataSourceID="dsShift" ValueField="value" TextField="value" ClientInstanceName="cbShift">
                                                <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true">
                                                    <RequiredField IsRequired="true" ErrorText="Shift is required" />
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>--%>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Date Receiving">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxDateEdit ID="deAnyDate" CaptionSettings-Position="Top" runat="server" ClientInstanceName="deAnyDate" 
                                                EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                                DisplayFormatString="dd-MM-yyyy">
                                                <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true">
                                                    <RequiredField IsRequired="true" ErrorText="Any Date is required" />
                                                </ValidationSettings>
                                            </dx:ASPxDateEdit>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>      
                                <dx:LayoutItem Caption="Material">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxComboBox ID="CmbMaterial" runat="server" CaptionSettings-Position="Top" ValueType="System.String" ValueField="Material"
                                                DropDownWidth="600px" EnableCallbackMode="true" TextFormatString="{0}" 
                                                IncrementalFilteringMode="Contains" DropDownStyle="DropDown"
                                                CallbackPageSize="10" 
                                                DataSourceID="dsMaterial" ClientInstanceName="CmbMaterial">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="Material" />
                                                    <dx:ListBoxColumn FieldName="Description" />
                                                </Columns>
                                                <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true">
                                                    <RequiredField IsRequired="true" ErrorText="Material is required" />
                                                </ValidationSettings>
                                                <ClientSideEvents ValueChanged="OnValueChanged" />
                                            </dx:ASPxComboBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Batch">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtBatch" runat="server" ClientInstanceName="txtBatch">
                                                <ValidationSettings SetFocusOnError="True" ValidationGroup="groupTabPersonal">
                                                    <RequiredField IsRequired="True" ErrorText="Batch is required" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Species">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxComboBox CaptionSettings-Position="Top" ID="cmbSpecies" runat="server" ClientInstanceName="ClientSpecies"
                                                DataSourceID="dsSpecies" ValueField="Id" TextField="Name" Width="20%">                                                
                                            </dx:ASPxComboBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>  
                                <%--<dx:LayoutItem Caption="Size">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxComboBox CaptionSettings-Position="Top" ID="cmbSize" runat="server" 
                                                ClientInstanceName="ClientSize" Width="20%">
                                                <Items>
                                                    <dx:ListEditItem Text="None" Value="0" />
                                                    <dx:ListEditItem Text="Large" Value="L" />
                                                    <dx:ListEditItem Text="Small" Value="S" />
                                                </Items>
                                                </dx:ASPxComboBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>--%>
                                <dx:LayoutItem Caption="Style">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxComboBox CaptionSettings-Position="Top" ID="cmbStyle" runat="server" 
                                                ClientInstanceName="ClientStyle" Width="20%">
                                                <Items>
                                                    <dx:ListEditItem Text="WR" Value="4" />
                                                    <dx:ListEditItem Text="HGT" Value="3" />
                                                </Items>
                                                </dx:ASPxComboBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Supplier">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxComboBox ID="cmbSupplier" runat="server" CaptionSettings-Position="Top" ValueType="System.String" TextField="Name" ValueField="Id"
                                                DropDownWidth="600px" EnableCallbackMode="true" TextFormatString="{0}" 
                                                IncrementalFilteringMode="Contains" DropDownStyle="DropDown"
                                                CallbackPageSize="10" 
                                                DataSourceID="dsSupplier" ClientInstanceName="cmbSupplier">
                                                <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true">
                                                    <RequiredField IsRequired="true" ErrorText="Supplier is required" />
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Vessel">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="txtVessel" CaptionSettings-Position="Top" runat="server" ClientInstanceName="txtVessel">
                                                <ValidationSettings SetFocusOnError="true" ValidationGroup="groupTabPersonal">
                                                    <RequiredField IsRequired="true" ErrorText="Vessel is required" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Invoice No.">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="txtInvoice" CaptionSettings-Position="Top" runat="server" ClientInstanceName="txtInvoice">
                                                <ValidationSettings SetFocusOnError="true" ValidationGroup="groupTabPersonal">
                                                    <RequiredField IsRequired="true" ErrorText="Invoice is required" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Container No./Truck No">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="txtContainer" CaptionSettings-Position="Top" runat="server" ClientInstanceName="txtContainer">
                                                <ValidationSettings SetFocusOnError="true" ValidationGroup="groupTabPersonal">
                                                    <RequiredField IsRequired="true" ErrorText="Container is required" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Net Weight (Kgs.)">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxTextBox ID="txtNetWeight" CaptionSettings-Position="Top" runat="server" ClientInstanceName="txtNetWeight">
                                                <ValidationSettings SetFocusOnError="true" ValidationGroup="groupTabPersonal">
                                                    <RequiredField IsRequired="true" ErrorText="Net Weight is required" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Packaging">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxComboBox ID="cmbPackaging" runat="server" CaptionSettings-Position="Top" ValueType="System.String"  ValueField="Name" 
                                                DropDownWidth="600px" EnableCallbackMode="true" TextFormatString="{0}" 
                                                IncrementalFilteringMode="Contains" DropDownStyle="DropDown"
                                                CallbackPageSize="10"
                                                DataSourceID="dsPackaging" ClientInstanceName="cmbPackaging">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="Name" />
                                                    <dx:ListBoxColumn FieldName="Description" />
                                                </Columns>
                                                <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true">
                                                    <RequiredField IsRequired="true" ErrorText="Packaging is required" />
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Notes" Width="100%">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxMemo runat="server" ID="mNotes" Rows="2" Width="100%" ClientInstanceName="mNotes"/>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                        </dx:LayoutGroup>
                                </Items>
                            </dx:ASPxFormLayout>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Picture of container">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl2" runat="server">
                            <dx:ASPxFormLayout runat="server" ID="f2">
                                <Items>
                                <dx:LayoutGroup GroupBoxDecoration="None" SettingsItemCaptions-Location="Left">
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
                                    <dx:LayoutItem Caption="PICTURE OF CONTAINER">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer>
                                            <dx:ASPxGridView runat="server" ID="grid" KeyFieldName="idx" OnRowUpdating="grid_RowUpdating"
                                                OnDataBinding="grid_DataBinding" CssClass="myGrid"
                                                OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                                                OnCustomCallback="grid_CustomCallback" ClientInstanceName="grid">
                                                <ClientSideEvents CustomButtonClick="function (s, e) { if (e.buttonID == 'EditForm') {
                                                    var key = s.GetRowKey(e.visibleIndex);
                                                    s.StartEditRow(e.visibleIndex);} 
                                                    }" />
                                                <Settings ShowColumnHeaders="false" VerticalScrollBarMode="Auto" GridLines="None" />
                                                <Border BorderStyle="None" />
                                                <SettingsBehavior AutoExpandAllGroups="true" EnableRowHotTrack="True" ColumnResizeMode="NextColumn" AllowFocusedRow="true" 
                                                    EnableCustomizationWindow="true"/>
		                                        <Styles>
			                                        <Row Cursor="pointer" />
		                                        </Styles>
                                                <SettingsAdaptivity AdaptivityMode="HideDataCells" HideDataCellsAtWindowInnerWidth="780" AllowOnlyOneAdaptiveDetailExpanded="true" AdaptiveDetailColumnCount="2">
                                                </SettingsAdaptivity>
                                                <EditFormLayoutProperties>
                                                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                    <Items>
                                                        <dx:GridViewColumnLayoutItem ColumnName="value" RequiredMarkDisplayMode="Hidden" VerticalAlign="Top"  />
                                                        <dx:GridViewColumnLayoutItem ColumnName="Data" HorizontalAlign="Left" HelpText="You can upload JPG, GIF or PNG file. Maximum fils size is 4MB." />
                                                        <dx:GridViewColumnLayoutItem ColumnName="ActiveDate"/>
                                                        <dx:EditModeCommandLayoutItem Colspan="1" ShowCancelButton="true" ShowUpdateButton="true" HorizontalAlign="Right"/>
                                                    </Items>
                                                </EditFormLayoutProperties>
                                                <SettingsEditing UseFormLayout="True" Mode="EditForm" EditFormColumnCount="1" />
                                                <SettingsPager Mode="ShowAllRecords"/>
                                                <Columns>
                                                    <dx:GridViewCommandColumn ShowNewButtonInHeader="false" ShowEditButton="true" VisibleIndex="1" Width="10%" ButtonRenderMode="Image">
                                                        <CustomButtons>
                                                        <dx:GridViewCommandColumnCustomButton ID="EditForm">
                                                        <Image ToolTip="Edit" Url="~/Content/Images/icons8-edit-16.png"/>
                                                        </dx:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dx:GridViewCommandColumn>
						                            <dx:GridViewDataBinaryImageColumn FieldName="Data" VisibleIndex="2" Caption="Image" Visible="false">
							                            <PropertiesBinaryImage EnableClientSideAPI="True" ShowLoadingImage="True" ImageHeight="150" ImageWidth="225" EnableServerResize="True">
								                        <EditingSettings Enabled="True">
								                            <ButtonPanelSettings Visibility="OnMouseOver" />
								                        </EditingSettings>
							                            </PropertiesBinaryImage>
							                        </dx:GridViewDataBinaryImageColumn>
                                                    <%--<dx:GridViewDataBinaryImageColumn FieldName="Picture" VisibleIndex="2" Width="150">
                                                        <PropertiesBinaryImage ImageHeight="150" ImageWidth="225" EnableServerResize="True">
                                                            <EditingSettings Enabled="true" UploadSettings-UploadValidationSettings-MaxFileSize="4194304"/>
                                                        </PropertiesBinaryImage>
                                                    </dx:GridViewDataBinaryImageColumn>--%>
                                                    <dx:GridViewDataTextColumn ReadOnly="true" FieldName="value" VisibleIndex="3" EditFormSettings-Visible="False" Caption="step"/>
                                                    <dx:GridViewDataTimeEditColumn FieldName="ActiveDate" VisibleIndex="4" Caption="Time"/>
                                                </Columns>  
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
                                    <dx:LayoutItem Caption="Transportation Condition">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxCheckBoxList ID="cbtraCondition" runat="server" DataSourceID="dstraCondition" ValueField="value" TextField="value" 
                                                    RepeatColumns="4" RepeatDirection="Horizontal" ClientInstanceName="cbtraCondition" 
                                                    RepeatLayout="Flow" Border-BorderStyle="None" Paddings-PaddingLeft="0px" FocusedStyle-Border-BorderStyle="None">
                                                </dx:ASPxCheckBoxList>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                   </Items>
                                  </dx:LayoutGroup>
                                </Items>
                            </dx:ASPxFormLayout>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Fish temperature(0°C)">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl3" runat="server">
                            <dx:ASPxFormLayout runat="server" ID="f3">
                                <Items>
                                <dx:LayoutGroup GroupBoxDecoration="None" SettingsItemCaptions-Location="Left">
                                    <Items>
                                        <dx:LayoutItem Caption="Thermometer No.">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox CaptionSettings-Position="Top" ID="tbThermometer" runat="server" ClientInstanceName="tbThermometer"/>

				                            </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Ice (Fresh Fish Only)">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxRadioButtonList ID="rbIceId" runat="server" CaptionSettings-Position="left" ValueType="System.String" Width="20%" 
                                                         DataSourceID="dsIceId" ValueField="value" TextField="value" RepeatColumns="1" Border-BorderStyle="None" Paddings-Padding="0px"
                                                        ClientInstanceName="rbIceId"/>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Sample">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox CaptionSettings-Position="Top" ID="tbSampling" runat="server"  ClientInstanceName="tbSampling"/>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxGridView runat="server" ID="gvTemp" KeyFieldName="Id" ClientInstanceName="gvTemp"
                                                    OnDataBinding="gvTemp_DataBinding" OnCustomCallback="gvTemp_CustomCallback" 
                                                    OnBatchUpdate="gvTemp_BatchUpdate" OnCustomButtonCallback="gvTemp_CustomButtonCallback">
                                                    <ClientSideEvents RowClick="function(s,e){ s.StartEditRow(e.visibleIndex); }" />
                                                    <Columns>
                                                        <dx:GridViewCommandColumn  ShowClearFilterButton="true" Width="42px" ButtonRenderMode="Image"
                                                            FixedStyle="Left" ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true">
                                                            <CustomButtons>
                                                                <dx:GridViewCommandColumnCustomButton ID="EditCost">
                                                                    <Image ToolTip="Remove" Url="~/Content/Images/Cancel.gif"/>
                                                                </dx:GridViewCommandColumnCustomButton>
                                                            </CustomButtons>
                                                    
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <HeaderTemplate>
                                                                <dx:ASPxButton runat="server" RenderMode="Link" AutoPostBack="false">
                                                                    <Image ToolTip="Insert" Url="~/Content/Images/icons8-plus-math-filled-16.png" />
                                                                    <ClientSideEvents Click="function(s,e){ gvTemp.AddNewRow(); }" />
                                                                </dx:ASPxButton>
                                                            </HeaderTemplate>
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewDataBinaryImageColumn FieldName="Data" VisibleIndex="1" Visible="false">
                                                            <PropertiesBinaryImage ImageHeight="150" ImageWidth="225" EnableServerResize="True">
                                                                <EditingSettings Enabled="true" UploadSettings-UploadValidationSettings-MaxFileSize="4194304"/>
                                                            </PropertiesBinaryImage>
                                                        </dx:GridViewDataBinaryImageColumn>
                                                        <dx:GridViewDataTextColumn FieldName="valueTemp" Caption="Temp" VisibleIndex="2"/>
                                                        <dx:GridViewDataColumn EditFormSettings-Visible="False" FieldName="StatusTemp" VisibleIndex="3"/>
                                                    </Columns>
                                                    <SettingsEditing UseFormLayout="True" Mode="Batch" EditFormColumnCount="1" />
                                                       <Settings ShowFooter="false" ShowStatusBar="Hidden"/>
                                                    <Styles>
			                                            <Row Cursor="pointer" />
		                                            </Styles>
                                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" HideDataCellsAtWindowInnerWidth="780" AllowOnlyOneAdaptiveDetailExpanded="true" AdaptiveDetailColumnCount="2">
                                                    </SettingsAdaptivity>
                                                    <EditFormLayoutProperties>
                                                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                                                    </EditFormLayoutProperties>
                                                    <TotalSummary>
                                                        <dx:ASPxSummaryItem FieldName="valueTemp" SummaryType="Average" ShowInColumn="valueTemp" />
                                                    </TotalSummary>
                                                </dx:ASPxGridView>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        </Items>
                                    </dx:LayoutGroup>
                                </Items>
                            </dx:ASPxFormLayout>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Weight of fish(Gram;G.)">
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl4" runat="server">
                            <dx:ASPxFormLayout runat="server" ID="f4" ClientInstanceName="f4">
                                <Items>
                                    <dx:LayoutGroup GroupBoxDecoration="None">
                                        <Items>
                                              <dx:LayoutItem Caption="" Width="100%">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer>
                                                        <dx:ASPxGridView runat="server" ID="gv3" AutoGenerateColumns="true" KeyFieldName="Id" 
                                                            OnRowValidating="gv3_RowValidating" OnCustomButtonInitialize="gv3_CustomButtonInitialize"
                                                            OnRowUpdating="gv3_RowUpdating"
                                                            OnCustomCallback="gv3_CustomCallback" OnInitNewRow="gv3_InitNewRow"
                                                            OnRowInserting="gv3_RowInserting"
                                                            OnDataBinding="gv3_DataBinding" OnCustomButtonCallback="gv3_CustomButtonCallback"
                                                            ClientInstanceName="gv3">
                                                            <ClientSideEvents BeginCallback="OnGridBeginCallback"/>
                                                            <SettingsCommandButton>
                                                            <EditButton>
                                                                <Image ToolTip="Edit" Url="~/Content/Images/icons8-edit-16.png" />
                                                            </EditButton></SettingsCommandButton>
                                                            <Columns>
                                                                <dx:GridViewCommandColumn  ShowClearFilterButton="true" Width="42px" ButtonRenderMode="Image"
                                                                FixedStyle="Left" ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true">                                                   
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <HeaderTemplate>
                                                                    <dx:ASPxButton runat="server" RenderMode="Link" AutoPostBack="false">
                                                                        <Image ToolTip="Insert" Url="~/Content/Images/icons8-plus-math-filled-16.png" />
                                                                        <ClientSideEvents Click="initNew" />
                                                                    </dx:ASPxButton>
                                                                </HeaderTemplate>
                                                                <CustomButtons>
                                                                <%--<dx:GridViewCommandColumnCustomButton ID="Edit">
                                                                    <Image ToolTip="Edit" Url="~/Content/Images/icons8-edit-16.png"/>
                                                                </dx:GridViewCommandColumnCustomButton>--%>
                                                                <dx:GridViewCommandColumnCustomButton ID="remove">
                                                                        <Image ToolTip="Remove" Url="~/Content/Images/Cancel.gif"/>
                                                                    </dx:GridViewCommandColumnCustomButton>
                                                            </CustomButtons>
                                                            </dx:GridViewCommandColumn>
                                                            <dx:GridViewDataBinaryImageColumn FieldName="Data">
                                                                    <PropertiesBinaryImage EnableClientSideAPI="True" ShowLoadingImage="True" ImageHeight="150" ImageWidth="225" EnableServerResize="True">
								                                    <EditingSettings Enabled="True">
								                                        <ButtonPanelSettings Visibility="OnMouseOver" />
								                                    </EditingSettings>
							                                        </PropertiesBinaryImage>
                                                                    <HeaderStyle CssClass="hide" />
                                                                    <EditCellStyle CssClass="hide" />
                                                                    <CellStyle CssClass="hide" />
                                                                    <FilterCellStyle CssClass="hide" />
                                                                    <FooterCellStyle CssClass="hide" />
                                                                    <GroupFooterCellStyle CssClass="hide" />
                                                                </dx:GridViewDataBinaryImageColumn>
                                                                <dx:GridViewDataBinaryImageColumn FieldName="Attachment">
                                                                    <PropertiesBinaryImage EnableClientSideAPI="True" ShowLoadingImage="True" ImageHeight="150" ImageWidth="225" EnableServerResize="True">
								                                    <EditingSettings Enabled="True">
								                                        <ButtonPanelSettings Visibility="OnMouseOver" />
								                                    </EditingSettings>
							                                        </PropertiesBinaryImage>
                                                                    <HeaderStyle CssClass="hide" />
                                                                    <EditCellStyle CssClass="hide" />
                                                                    <CellStyle CssClass="hide" />
                                                                    <FilterCellStyle CssClass="hide" />
                                                                    <FooterCellStyle CssClass="hide" />
                                                                    <GroupFooterCellStyle CssClass="hide" />
                                                                </dx:GridViewDataBinaryImageColumn>
                                                                <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="true" />
                                                                <dx:GridViewDataTextColumn FieldName="SampleNo" ReadOnly="true" />
                                                                <dx:GridViewDataTextColumn FieldName="FrozenPacked" Caption="FrozenPacked Date/Lot"/>
                                                                <dx:GridViewDataTextColumn FieldName="FrozenWeight" />
                                                                <dx:GridViewDataTextColumn FieldName="NetWeight" />
                                                                <dx:GridViewDataTextColumn FieldName="Counts">
                                                                    <PropertiesTextEdit>
                                                                        <ClientSideEvents TextChanged="OnTextChanged" />
                                                                    </PropertiesTextEdit>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataComboBoxColumn FieldName="ForeignMatter">
                                                                    <PropertiesComboBox DataSourceID="dsparamForeignMatter" ValueField="idx" TextField="value"/>
                                                                </dx:GridViewDataComboBoxColumn>
                                                                <dx:GridViewDataTextColumn FieldName="Notes"/>
                                                                <dx:GridViewDataTextColumn FieldName="Mark" EditFormSettings-Visible="False"/>
                                                            </Columns>   
                                                            <EditFormLayoutProperties ColCount="2">
                                                                <Items>
                                                                    <dx:GridViewLayoutGroup ColCount="2" GroupBoxDecoration="None">
                                                                        <Items>
                                                                            <dx:GridViewColumnLayoutItem ColumnName="Id" />
                                                                            <dx:GridViewColumnLayoutItem ColumnName="SampleNo" />
                                                                            <dx:GridViewColumnLayoutItem ColumnName="FrozenPacked" Caption="FrozenPacked Date/Lot" />
                                                                            <dx:GridViewColumnLayoutItem ColumnName="FrozenWeight" Width="100%" />
                                                                            <dx:GridViewColumnLayoutItem ColumnName="Attachment" HorizontalAlign="Left" HelpText="You can upload JPG, GIF or PNG file. Maximum fils size is 4MB." />
                                                                            <dx:GridViewColumnLayoutItem ColumnName="Counts"  Caption="Count pers" Width="100%"/>
                                                                            <dx:GridViewColumnLayoutItem ColumnName="ForeignMatter"  Caption="Foreign Matter" Width="100%"/>
                                                                        </Items>
                                                                    </dx:GridViewLayoutGroup>
                                                                    <dx:GridViewLayoutGroup GroupBoxDecoration="None">
                                                                        <Items>
                                                                            <dx:EmptyLayoutItem/>
                                                                            <dx:GridViewColumnLayoutItem ColumnName="NetWeight"/>
                                                                            <dx:GridViewColumnLayoutItem ColumnName="Data" HorizontalAlign="Left" HelpText="You can upload JPG, GIF or PNG file. Maximum fils size is 4MB."
                                                                                Caption="Attachment" />
                                                                            <%--<dx:GridViewColumnLayoutItem Caption="Hire Date" VerticalAlign="Top">
                                                                                <Template>
                                                                                    <div>
                                                                                        <dx:ASPxCalendar ID="hireDateCalendar" runat="server" Value='<%# Bind("HireDate") %>' CssClass="hireDateCalendar" />
                                                                                    </div>
                                                                                </Template>
                                                                            </dx:GridViewColumnLayoutItem>--%>
                                                                        </Items>
                                                                    </dx:GridViewLayoutGroup>
                                                                    <dx:EditModeCommandLayoutItem Width="100%" HorizontalAlign="Right" />
                                                                </Items>
                                                                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="700" />
                                                            </EditFormLayoutProperties>
                                                            <Templates>
                                                                <EditForm>
                                                                    <div style="padding: 4px 3px 4px">
                                                                    <dx:ASPxGridViewTemplateReplacement ID="Editors" ReplacementType="EditFormEditors" runat="server"/>
                                                                    </div>
                                                                <div style="text-align: right; padding: 2px">
                                                                    <dx:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                        runat="server">
                                                                    </dx:ASPxGridViewTemplateReplacement>
                                                                    <dx:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                        runat="server">
                                                                    </dx:ASPxGridViewTemplateReplacement>
                                                                </div>
                                                                </EditForm>
                                                            </Templates>
                                                            <Settings GridLines="None" />
                                                            <Border BorderStyle="None" />
                                                            <SettingsEditing UseFormLayout="True" Mode="EditForm" EditFormColumnCount="1" />
                                                            <SettingsBehavior AllowSelectByRowClick="true"/>
                                                            <SettingsCommandButton>
                                                             <%--<EditButton>
                                                                <Image ToolTip="Edit" Url="~/Content/Images/icons8-edit-16.png" />
                                                            </EditButton>--%>
                                                            <UpdateButton RenderMode="Image">
                                                                <Image ToolTip="Save changes and close Edit Form" Url="~/Content/Images/update.png" />
                                                            </UpdateButton>
                                                            <CancelButton RenderMode="Image">
                                                                <Image ToolTip="Close Edit Form without saving changes" Url="~/Content/Images/cancel.png" />
                                                            </CancelButton>
                                                            </SettingsCommandButton>
                                                            <TotalSummary>
                                                                <dx:ASPxSummaryItem FieldName="NetWeight" SummaryType="Average" ShowInColumn="NetWeight" />
                                                                <dx:ASPxSummaryItem FieldName="NetWeight" SummaryType="Sum" ShowInColumn="NetWeight" />
                                                                <dx:ASPxSummaryItem FieldName="FrozenWeight" SummaryType="Average" ShowInColumn="FrozenWeight" />
                                                                <dx:ASPxSummaryItem FieldName="FrozenWeight" SummaryType="Sum" ShowInColumn="FrozenWeight" />
                                                            </TotalSummary>
                                                        </dx:ASPxGridView>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Selection Option" Name="param" CaptionSettings-Location="Top" ClientVisible="false">
                                            <ParentContainerStyle Paddings-Padding="0" />
                                            <CaptionCellStyle Paddings-PaddingLeft="10px" />
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxRadioButtonList runat="server" ID="radioButtonList" ClientSideEvents-SelectedIndexChanged="OnSelectedIndexChanged" 
                                                        CssClass="radioButtonList" RepeatColumns="1"
                                                        Border-BorderWidth="0" ClientSideEvents-Init="UpdateRadioButtonListDecoration" SelectedIndex="0" 
                                                        ClientInstanceName="radioButtonList" Width="30%">
                                                        <Items>
                                                            <dx:ListEditItem Text="Length * cm" Value="0" /> 
                                                            <dx:ListEditItem Text="Weight * G" Value="1" />
                                                            <dx:ListEditItem Text="Defect of Fish " Value="2" />
                                                            <dx:ListEditItem Text="Foreign Matter" Value="3" />
                                                            <%--<dx:ListEditItem Text="Appearance" Value="4" />--%>
                                                        </Items>
                                                    </dx:ASPxRadioButtonList>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    <dx:LayoutItem Caption="" Name="gv" ClientVisible="false" CaptionSettings-Location="Top" Width="100%">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxGridView runat="server" ID="gv" OnDataBound="gv_DataBound" 
                                                KeyFieldName="Name" OnBatchUpdate="gv_BatchUpdate"
                                                OnCustomCallback="gv_CustomCallback" 
                                                OnDataBinding="gv_DataBinding" 
                                                OnCellEditorInitialize="gv_CellEditorInitialize" OnCommandButtonInitialize="gv_CommandButtonInitialize" ClientInstanceName="gv">
                                                <SettingsPager Mode="ShowAllRecords"/>
                                                <Settings VerticalScrollableHeight="594" ShowFooter="false" ShowStatusBar="Hidden" GridLines="Both" />
                                                <Border BorderStyle="None" />
                                                <SettingsEditing Mode="Batch" EditFormColumnCount="1"/> 
                                                <ClientSideEvents BatchEditEndEditing="OnBatchEditEndEditing" BatchEditStartEditing="OnBatchEditStartEditing" 
					                    Init="OnInitGridBatch" FocusedCellChanging="onFocusedCellChanging"/>  
                                            </dx:ASPxGridView>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                            </Items>
                            </dx:LayoutGroup>
                                </Items>
                            </dx:ASPxFormLayout>                     
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Sensory">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl5" runat="server">
                        <dx:ASPxFormLayout ID="f5" runat="server">
                            <Items>
                            <dx:LayoutGroup GroupBoxDecoration="None" ColCount="2">
                                <Items>
                                    <dx:LayoutItem Caption="Odor">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxComboBox ID="CmbOdor" runat="server" ClientInstanceName="ClientOdor" DataSourceID="dsSensory"
                                                     ValueField="idx" TextField="value" />
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Texture">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxComboBox ID="CmbTexture" runat="server" ClientInstanceName="ClientTexture" DataSourceID="dsSensory"
                                                     ValueField="idx" TextField="value" />
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Formalin">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl6" runat="server">
                        <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server">
                            <Items>
                            <dx:LayoutGroup GroupBoxDecoration="None">
                                <Items>
                                    <dx:LayoutItem Caption="Formalin">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxComboBox ID="CmbFormalin" runat="server" ClientInstanceName="ClientFormalin" DataSourceID="dsFormalin"
                                                     ValueField="idx" TextField="value" />
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Appearance">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl7" runat="server">
                        <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server">
                            <Items>
                            <dx:LayoutGroup GroupBoxDecoration="None">
                                <Items>
                                    <dx:LayoutItem Caption="Appearance" Name="gv" CaptionSettings-Location="Left">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                            <div id="divContainer" runat="server"/>
                                            <dx:ASPxGridView ID="gvApp" runat="server" KeyFieldName="Id" AutoGenerateColumns="false" ClientInstanceName="gvApp"
                                                OnCellEditorInitialize="gvApp_CellEditorInitialize" OnCustomCallback="gvApp_CustomCallback" 
                                                OnDataBinding="gvApp_DataBinding">
                                                <SettingsPager Mode="ShowAllRecords"/>
                                                <Settings VerticalScrollableHeight="594" ShowFooter="false" ShowStatusBar="Hidden" GridLines="Both" />
                                                <Border BorderStyle="None" />
                                                <SettingsEditing Mode="Batch" EditFormColumnCount="1"/> 
                                            </dx:ASPxGridView>
                                            <dx:ASPxCallback ID="ASPxCallback" runat="server" ClientInstanceName="callback" OnCallback="ASPxCallback_Callback">
                                            <ClientSideEvents CallbackComplete="onEndUpdateCallback" />
                                        </dx:ASPxCallback>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem Caption="Grand">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox1" runat="server" 
                                                    ClientInstanceName="txtUniformity" ReadOnly="true" Width="10%">      
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Uniformity">
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl8" runat="server">
                        <dx:ASPxFormLayout ID="ASPxFormLayout3" runat="server">
                            <Items>
                            <dx:LayoutGroup GroupBoxDecoration="None">
                                <Items>
                                    <dx:LayoutItem Caption="Input">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer>
                                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtUniformity" runat="server" 
                                                    ClientInstanceName="txtUniformity" Width="20%">      
                                                </dx:ASPxTextBox>
                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            </TabPages>
        </dx:ASPxPageControl>
        </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
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
<asp:SqlDataSource ID="dsMaterial" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetMaterial" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:Parameter Name="Type" DefaultValue="0" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsSupplier" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from tblSupplier">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsShift" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from dbo.FNC_SPLIT('NS,DS',',')">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dstraCondition" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from dbo.FNC_SPLIT('CLEAN,CLOSED DOOR',',')">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsIceId" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from dbo.FNC_SPLIT('Little,Moderate,Much,Not Test',',')">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsSensory" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from dbo.FNC_SPLIT('Normal|AB Normal','|')">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsFormalin" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from dbo.FNC_SPLIT('NOT DETECT|DETECT|NotTest','|')">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsPackaging" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from tblPackaging">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsparamForeignMatter" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from dbo.FNC_SPLIT('NOT DETECT|DETECT','|')">
</asp:SqlDataSource>
<%--<asp:SqlDataSource ID="dsgv" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetTask" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="hGeID" Name="Id" PropertyName="['GeID']"/>
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsTemp" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetTemp" SelectCommandType="StoredProcedure">
</asp:SqlDataSource> 

<asp:SqlDataSource ID="dsAppearance" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGenAppearance" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="hGeID" Name="Id" PropertyName="['GeID']"/>
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsWeight" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetWeight" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="hGeID" Name="Id" PropertyName="['GeID']"/>
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsAnalysis" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGetAnalysis" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:Parameter Name="group" DefaultValue="6" />
    </SelectParameters>
</asp:SqlDataSource>--%>
<asp:SqlDataSource ID="dsSample" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from transSample">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsSpecies" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from MasSpecies">
</asp:SqlDataSource>
<%--<asp:SqlDataSource ID="dsGenCount" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGenCount" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="FormPanel$formLayout$PC_1$txtCount" Name="strCount" PropertyName="Text" />
        <asp:ControlParameter ControlID="hGeID" Name="SampId" PropertyName="['GeID']"/>
    </SelectParameters>
</asp:SqlDataSource>--%>
<asp:SqlDataSource ID="dsForeignMatter" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spGenForeignMatter" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:ControlParameter ControlID="hGeID" Name="Id" PropertyName="['GeID']"/>
    </SelectParameters>
</asp:SqlDataSource>