<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OEEForm.ascx.cs" Inherits="UserControls_EditForms_OEEFrom" %>
<script type="text/javascript">
    function testgrid_RowClick (s, e) {

    }
    function OnRowClick(s, e) {
        //var key = s.GetRowKey(e.visibleIndex);
        //alert(key);
        s.StartEditRow(e.visibleIndex);
    }
        function OnApplid(s, e) {
            testgrid.ApplyFilter(e.filterExpression);
            testgrid.PerformCallback('filter');
        }
        function onButtonClick(s, e) {
        }
        var lastValidationResult = false;
        function OnContactMethodChanged(s, e) {
            var selectedIndex = s.GetActiveTabIndex();
            UpdateTabListDecoration(s);
            tabbedGroupPageControl.SetActiveTabIndex(selectedIndex);
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
    function OnBtnShowPopupClick(param) {

        //alert(param);
        var Keys = ClientPrdOrder.GetText();
        if (Keys == '') return alert('The Production order should not be empty...');
        cb.PerformCallback(['reload', Keys].join('|'))
        }
    function OnCallbackComplete(s, e) {
        debugger;
        var values = e.result.split('|');  
        txtOrder.SetValue(values['1']);
        txtItems.SetValue(values['2']);
        txtBrand.SetValue(values['3']);
        txtCustomer.SetValue(values['3']);
    }
    function OnInit(s, e) {
        debugger;
        var d = new Date(); // for now
        var localTime = new Date(0);
        s.SetDate(localTime);
    }
    function OnSelectedIndexChanged(s, e) {
        debugger;
        txtCustomSize.SetText(s.GetSelectedItem().GetColumnText(2));
        txtTarget.SetText(s.GetSelectedItem().GetColumnText(3));
    }
</script>
<dx:ASPxHiddenField ID="hGeID" runat="server" ClientInstanceName="hGeID"/>
<dx:ASPxHiddenField ID="hKeyword" runat="server" ClientInstanceName="hKeyword" />
<dx:ASPxGridView ID="testgrid" ClientInstanceName="testgrid" runat="server" Width="100%" KeyFieldName="Id" DataSourceID="dsLabelling"
     CssClass="grid">
    <ClientSideEvents Init="DevAV.Init" RowClick="testgrid_RowClick" EndCallback="DevAV.test" />
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
        <dx:ASPxFormLayout ID="formLayout" ClientInstanceName="formLayout" CssClass="formLayout" runat="server" AlignItemCaptionsInAllGroups="True" UseDefaultPaddings="false">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="800" />
        <Items>
        <dx:LayoutGroup GroupBoxDecoration="HeadingLine" Caption="XXXXX">
                <Items>
                <dx:LayoutItem Caption="Line no">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtGetID" runat="server" ClientInstanceName="txtGetID" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Number of employees" >
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxSpinEdit CaptionSettings-Position="Top" ID="seSampleID" runat="server" ClientInstanceName="seSampleID" ToolTip="จำนวนพนักงาน" >      
                                </dx:ASPxSpinEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Type">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxComboBox CaptionSettings-Position="Top" ID="cmbPkgType" runat="server" ClientInstanceName="ClientPkgType"
                                DataSourceID="dsPkgType" ValueField="ID" TextField="Name" >                                                
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>   
                <dx:LayoutItem Caption="Current Date" >
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer>
                        <dx:ASPxDateEdit ID="deAnyDate" CaptionSettings-Position="Top" runat="server" ClientInstanceName="deAnyDate" 
                            EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                            DisplayFormatString="dd-MM-yyyy">
                            <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true"/>
                        </dx:ASPxDateEdit>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>      
            <dx:LayoutItem Caption="Start Time">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer>
                        <dx:ASPxTimeEdit ID="dtStarttime" runat="server" Width="190" EditFormat="Custom" EditFormatString="HH:mm" DisplayFormatString="HH:mm" ClientInstanceName="dtStarttime">
                            <ClearButton DisplayMode="OnHover"></ClearButton>
                            <ValidationSettings ErrorDisplayMode="None" />
                            <ClientSideEvents Init="OnInit" />
                        </dx:ASPxTimeEdit>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>           
            <dx:LayoutItem Caption="Finish Time">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer>
                        <table>
                            <tr>
                                <td><dx:ASPxTimeEdit ID="dtFinishTime" runat="server" Width="190" EditFormat="Custom" EditFormatString="HH:mm" DisplayFormatString="HH:mm" ClientInstanceName="dtFinishTime">
                                    <ClearButton DisplayMode="OnHover"></ClearButton>
                                    <ValidationSettings ErrorDisplayMode="None" />
                                </dx:ASPxTimeEdit></td><td>&nbsp</td>
                                <td><dx:ASPxCheckBox ID="cbFinish" runat="server" Text="Finish" /></td>
                            </tr>
                        </table>
                        
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
        </dx:LayoutItem>                
                </Items>
        </dx:LayoutGroup>
        <dx:LayoutGroup Caption="Production Labelling" GroupBoxDecoration="HeadingLine" ColCount="4">
                <Items>                  
               <dx:LayoutItem Caption="Prod. order">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <table>
                                    <tr>
                                        <td>
                                        <dx:ASPxTextBox CaptionSettings-Position="Top" ID="tbPrdOrder" runat="server" ClientInstanceName="ClientPrdOrder"
                                             Width="100%">      
                                        </dx:ASPxTextBox>
                                        </td><td>&nbsp</td>
                                        <td><dx:ASPxButton ID="btnLinkImageAndText" runat="server" RenderMode="Link" AutoPostBack="false" ClientInstanceName="btnLinkImageAndText">
                                            <Image Url="~/Content/Images/icons8-ok-32.png"></Image>
                                            <ClientSideEvents Click="function(s, e) { OnBtnShowPopupClick('sele');
                                            }" />
                                        </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                                <dx:ASPxCallback ID="cb" ClientInstanceName="cb" runat="server" OnCallback="cb_Callback">
                                    <ClientSideEvents CallbackComplete="OnCallbackComplete" />
                                </dx:ASPxCallback>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Sales order">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtOrder" runat="server" ClientInstanceName="txtOrder" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Items">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtItems" runat="server" ClientInstanceName="txtItems" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Brand">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtBrand" runat="server" ClientInstanceName="txtBrand" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Customer">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtCustomer" runat="server" ClientInstanceName="txtCustomer" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Material Code">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox CaptionSettings-Position="Top" ID="CmbMaterial" runat="server" ClientInstanceName="CmbMaterial">      
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
            </Items>
        </dx:LayoutGroup>
        <%--<dx:LayoutGroup Caption="" ColCount="4">
            <Items>
            <dx:LayoutItem Caption="Material Code">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox2" runat="server" ClientInstanceName="txtGetID" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Description">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox5" runat="server" ClientInstanceName="txtGetID" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Unit">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtUnit" runat="server" ClientInstanceName="txtUnit" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Batch Raw Material">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox CaptionSettings-Position="Top" ID="CmbBatchRaw" ClientInstanceName="CmbBatchRaw" runat="server">      
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Tolal Qty">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtTotalQty" runat="server" ClientInstanceName="txtTotalQty" 
                                    ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Receipt Qty">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtReceiptQty" runat="server" ClientInstanceName="txtReceiptQty" 
                                    ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Remain Qty">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtRemainQty" runat="server" ClientInstanceName="txtRemainQty" 
                                    ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Quantity" >
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxSpinEdit CaptionSettings-Position="Top" ID="seQuantity" runat="server" 
                                    ClientInstanceName="seQuantity">      
                                </dx:ASPxSpinEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Batch">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtBatch" runat="server" 
                                ClientInstanceName="txtBatch">      
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
                <dx:LayoutItem Caption="Prod.Date" >
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer>
                        <dx:ASPxDateEdit ID="deProdDate" CaptionSettings-Position="Top" runat="server" ClientInstanceName="deProdDate" 
                            EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                            DisplayFormatString="dd-MM-yyyy">
                            <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true"/>
                        </dx:ASPxDateEdit>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>   
            <dx:LayoutItem Caption="Exp.Date" >
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer>
                        <dx:ASPxDateEdit ID="deExpDate" CaptionSettings-Position="Top" runat="server" ClientInstanceName="deExpDate" 
                            EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                            DisplayFormatString="dd-MM-yyyy">
                            <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true"/>
                        </dx:ASPxDateEdit>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem> 
            <dx:LayoutItem Caption="Pallet No">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtPalletNo" runat="server" 
                                ClientInstanceName="txtPalletNo">      
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Scrap No">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer>
                            <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtScrapNo" runat="server" 
                                ClientInstanceName="txtScrapNo">      
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
            </dx:LayoutItem>          
            </Items>
        </dx:LayoutGroup>--%>
        <dx:LayoutGroup Caption="Group Settings" GroupBoxDecoration="HeadingLine" ColCount="4">
                <Items>                    
                <dx:LayoutItem Caption="Code pack">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox CaptionSettings-Position="Top" ID="CmbCodePack" ClientInstanceName="CmbCodePack" runat="server"
                                    DataSourceID="dsCodePack" TextFormatString="{0};{1}" ValueField="Code">  
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="Code" />
                                        <dx:ListBoxColumn FieldName="Name" />
                                        <dx:ListBoxColumn FieldName="CustomSize" />
                                        <dx:ListBoxColumn FieldName="TargetCar" />
                                    </Columns>
                                    <ClientSideEvents SelectedIndexChanged="OnSelectedIndexChanged" />
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="(Size/ Customers)">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtCustomSize" runat="server" ClientInstanceName="txtCustomSize" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Target (Car/Man/HR.)">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="txtTarget" runat="server" ClientInstanceName="txtTarget" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
            <dx:LayoutItem Caption="Standard Machine Speed">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer>
                        <dx:ASPxSpinEdit runat="server" ID="seStdMachineSd" ClientInstanceName="seStdMachineSd" NumberType="Float" 
                            Number="0.00" >
                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" ValidationGroup="group1"/>
                        </dx:ASPxSpinEdit>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        <dx:LayoutItem Caption="">
        <LayoutItemNestedControlCollection>
            <dx:LayoutItemNestedControlContainer>
                <dx:ASPxTabControl ID="TabList" runat="server" NameField="Id" DataSourceID="XmlDataSource1" ActiveTabIndex="0" ClientInstanceName="TabList" EnableTabScrolling="true">
                    <ClientSideEvents ActiveTabChanged="OnContactMethodChanged" Init="UpdateTabListDecoration"/>
                </dx:ASPxTabControl>
                <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Platforms.xml"
                    XPath="//tab"></asp:XmlDataSource>
            </dx:LayoutItemNestedControlContainer>
        </LayoutItemNestedControlCollection>
    </dx:LayoutItem>
    <dx:TabbedLayoutGroup Caption="TabbedGroup" ClientInstanceName="tabbedGroupPageControl" ShowGroupDecoration="false" Width="100%">
        <ParentContainerStyle CssClass="tabbedGroupPageControlCell" />
        <ClientSideEvents Init="OnTabbedGroupPageControlInit" />
        <Items>
            <dx:LayoutGroup GroupBoxDecoration="None" SettingsItemCaptions-Location="Left">
                <Items>
                <dx:LayoutItem Caption="">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxGridView runat="server" ID="gvInk" KeyFieldName="ID" DataSourceID="dsInkjet" 
                                    ClientInstanceName="gvInk">
                                    <Columns>
                                        <dx:GridViewCommandColumn  ShowClearFilterButton="true" Width="42px" ButtonRenderMode="Image"
                                            FixedStyle="Left" ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <dx:ASPxButton runat="server" RenderMode="Link" AutoPostBack="false">
                                                    <Image ToolTip="Insert" Url="~/Content/Images/icons8-plus-math-filled-16.png" />
                                                    <ClientSideEvents Click="function(s,e){ gvInk.AddNewRow(); }" />
                                                </dx:ASPxButton>
                                            </HeaderTemplate>
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataDateColumn FieldName="CreateOn" Caption="Date"  VisibleIndex="1" />
                                        <dx:GridViewDataBinaryImageColumn FieldName="Data" Width="150">
                                            <PropertiesBinaryImage EnableServerResize="True">
                                                <EditingSettings Enabled="true" UploadSettings-UploadValidationSettings-MaxFileSize="4194304"/>
                                            </PropertiesBinaryImage>
                                        </dx:GridViewDataBinaryImageColumn>
                                        <dx:GridViewDataBinaryImageColumn FieldName="PicInkjet" PropertiesBinaryImage-EditingSettings-Enabled="true" />
                                        <dx:GridViewDataColumn FieldName="TotalInkjet" />
                                        <dx:GridViewDataComboBoxColumn FieldName="ReportBy">
                                            <PropertiesComboBox DataSourceID="dsUser" ValueField="user_name" TextField="fn" />
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataComboBoxColumn FieldName="ReviewedBy">
                                            <PropertiesComboBox DataSourceID="dsUser" ValueField="user_name" TextField="fn" />
                                        </dx:GridViewDataComboBoxColumn>
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
            <dx:LayoutGroup GroupBoxDecoration="None" SettingsItemCaptions-Location="Left">
                <Items>
                <dx:LayoutItem Caption="">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxGridView runat="server" ID="gvResult" KeyFieldName="ID" DataSourceID="dsResult" 
                                    ClientInstanceName="gvResult">
                                    <Columns>
                                        <dx:GridViewCommandColumn  ShowClearFilterButton="true" Width="42px" ButtonRenderMode="Image"
                                            FixedStyle="Left" ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true">
                                             
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <dx:ASPxButton runat="server" RenderMode="Link" AutoPostBack="false">
                                                    <Image ToolTip="Insert" Url="~/Content/Images/icons8-plus-math-filled-16.png" />
                                                    <ClientSideEvents Click="function(s,e){ gvResult.AddNewRow(); }" />
                                                </dx:ASPxButton>
                                            </HeaderTemplate>
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataComboBoxColumn FieldName="Components">
                                            <PropertiesComboBox DataSourceID="dsComponents" TextField="value" />
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataDateColumn FieldName="StartTime" />
                                        <dx:GridViewDataDateColumn FieldName="FinishTime" />
                                        <dx:GridViewDataColumn FieldName="Total" />
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
            <dx:LayoutGroup GroupBoxDecoration="None" SettingsItemCaptions-Location="Left">
                <Items>
                <dx:LayoutItem Caption="">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxGridView runat="server" ID="grid" KeyFieldName="ID" DataSourceID="dsComponents" ClientInstanceName="grid">
                                    <Columns>
                                        <dx:GridViewCommandColumn  ShowClearFilterButton="true" Width="42px" ButtonRenderMode="Image"
                                            FixedStyle="Left" ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true">
                                             
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <dx:ASPxButton runat="server" RenderMode="Link" AutoPostBack="false">
                                                    <Image ToolTip="Insert" Url="~/Content/Images/icons8-plus-math-filled-16.png" />
                                                    <ClientSideEvents Click="function(s,e){ grid.AddNewRow(); }" />
                                                </dx:ASPxButton>
                                            </HeaderTemplate>
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataComboBoxColumn FieldName="Components">
                                            <PropertiesComboBox DataSourceID="dsComponents" TextField="value" />
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataColumn FieldName="Material" />
                                        <dx:GridViewDataColumn FieldName="Description" />
                                        <dx:GridViewDataColumn FieldName="Unit" />
                                        <dx:GridViewDataColumn FieldName="Storageloc" Caption="Sloc"/>
                                        <dx:GridViewDataColumn FieldName="Batch" />
                                        <dx:GridViewDataColumn FieldName="QtyPackage" />
                                        <dx:GridViewDataColumn FieldName="Scraps" />
                                        <dx:GridViewDataColumn FieldName="Remark" />
                                        <dx:GridViewDataColumn FieldName="Mark" Width="20px" />
                                        <%--<dx:GridViewDataComboBoxColumn Caption="OEE Plan Halt time" FieldName="OEEPlan" />
                                        <dx:GridViewDataTextColumn FieldName="StartTime" />
                                        <dx:GridViewDataTextColumn FieldName="FinishTime" />--%>
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
            <dx:LayoutGroup GroupBoxDecoration="None" SettingsItemCaptions-Location="Left">
                <Items>
                <dx:LayoutItem Caption="">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxGridView runat="server" ID="gv" KeyFieldName="ID" DataSourceID="dsGReceipts" ClientInstanceName="gv">
                                    <Columns>
                                        <dx:GridViewCommandColumn  ShowClearFilterButton="true" Width="42px" ButtonRenderMode="Image"
                                            FixedStyle="Left" ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true">
                                             
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <dx:ASPxButton runat="server" RenderMode="Link" AutoPostBack="false">
                                                    <Image ToolTip="Insert" Url="~/Content/Images/icons8-plus-math-filled-16.png" />
                                                    <ClientSideEvents Click="function(s,e){ gv.AddNewRow(); }" />
                                                </dx:ASPxButton>
                                            </HeaderTemplate>
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataColumn FieldName="RawMaterial" />
                                        <dx:GridViewDataColumn FieldName="BatchRaW" />
                                        <dx:GridViewDataColumn FieldName="QtyReceipts" />
                                        <dx:GridViewDataColumn FieldName="Batch" />
                                        <dx:GridViewDataDateColumn FieldName="ProdDate" />
                                        <dx:GridViewDataDateColumn FieldName="ExpDate" />
                                        <dx:GridViewDataColumn FieldName="Mark" Width="20px" />
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
            <%--<dx:LayoutGroup GroupBoxDecoration="None" SettingsItemCaptions-Location="Left">
                <Items>
                <dx:LayoutItem Caption="Material Code">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox11" runat="server">      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Description">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox12" runat="server" ClientInstanceName="txtGetID" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Unit">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox13" runat="server" ClientInstanceName="txtGetID" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Batch Mat2">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox CaptionSettings-Position="Top" ID="ASPxomboBox14" runat="server" ClientInstanceName="txtGetID" ReadOnly="true" >      
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Total Qty">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox15" runat="server" ClientInstanceName="txtGetID" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Receipt Qty">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox16" runat="server" ClientInstanceName="txtGetID" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Remain Qty">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox17" runat="server" ClientInstanceName="txtGetID" ReadOnly="true" >      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>

                <dx:LayoutItem Caption="Qty">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox14" runat="server" ClientInstanceName="txtGetID">      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Batch">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox18" runat="server" ClientInstanceName="txtGetID">      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Prod. Date">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxDateEdit ID="ASPxDateEdit1" CaptionSettings-Position="Top" runat="server" ClientInstanceName="deAnyDate" 
                            EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                            DisplayFormatString="dd-MM-yyyy">
                            <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true"/>
                        </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Exp. Date">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxDateEdit ID="ASPxDateEdit2" CaptionSettings-Position="Top" runat="server" ClientInstanceName="deAnyDate" 
                            EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                            DisplayFormatString="dd-MM-yyyy">
                            <ValidationSettings ValidationGroup="groupTabDate" ValidateOnLeave="true" SetFocusOnError="true"/>
                        </dx:ASPxDateEdit>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="pallet number">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox20" runat="server" ClientInstanceName="txtGetID">      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Number of scraps (cans)">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxTextBox CaptionSettings-Position="Top" ID="ASPxTextBox21" runat="server" ClientInstanceName="txtGetID">      
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
            </Items>
        </dx:LayoutGroup>--%>
        </Items>
        </dx:TabbedLayoutGroup>
        </Items>
        </dx:ASPxFormLayout>
            </dx:PanelContent>
        </PanelCollection>
</dx:ASPxCallbackPanel>
<dx:ASPxPopupControl ID="PopupControl" runat="server" Width="680px" Height="380px" CloseAction="CloseButton" CloseOnEscape="true"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="PopupControl"
        HeaderText="popup" AllowDragging="True" Modal="True" PopupAnimationType="Fade"
        EnableViewState="False" PopupHorizontalOffset="40" PopupVerticalOffset="40" AutoUpdatePosition="true">
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
<asp:SqlDataSource ID="dsLabelling" runat="server" ConnectionString="<%$ ConnectionStrings:oeeDbConnectionString %>" 
    SelectCommand="select * from transLabelling">
</asp:SqlDataSource>
<%--<asp:SqlDataSource ID="dsgv" runat="server" ConnectionString="<%$ ConnectionStrings:oeeDbConnectionString %>" 
    SelectCommand="select 1 idx,'' OEEPlan,'' StartTime,''FinishTime">
</asp:SqlDataSource>--%>
<asp:SqlDataSource ID="dsPkgType" runat="server" ConnectionString="<%$ ConnectionStrings:oeeDbConnectionString %>" 
    SelectCommand="select * from MasPkgType">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsCodePack" runat="server" ConnectionString="<%$ ConnectionStrings:oeeDbConnectionString %>" 
    SelectCommand="select * from MasCodePack">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsInkjet" runat="server" ConnectionString="<%$ ConnectionStrings:oeeDbConnectionString %>" 
    SelectCommand="select * from TransAttInkjet">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsComponents" runat="server" ConnectionString="<%$ ConnectionStrings:oeeDbConnectionString %>" 
    SelectCommand="select *,''Mark from TransComponents">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsResult" runat="server" ConnectionString="<%$ ConnectionStrings:oeeDbConnectionString %>" 
    SelectCommand="select *,''Mark from TransResult">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsGReceipts" runat="server" ConnectionString="<%$ ConnectionStrings:oeeDbConnectionString %>" 
    SelectCommand="select *,''Mark from TransGReceipts">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsUser" runat="server" ConnectionString="<%$ ConnectionStrings:oeeDbConnectionString %>" 
    SelectCommand="select * from ulogin">
</asp:SqlDataSource>
