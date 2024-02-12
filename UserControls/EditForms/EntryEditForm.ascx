<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EntryEditForm.ascx.cs" Inherits="UserControls_EditForms_EntryEditForm" %>
    <style>
        body, form
        {
            padding: 0;
            margin: 0;
            overflow: hidden;
            min-height: 200px;
            min-width: 400px;
        }
    </style>
    <script type="text/javascript">
        function UpdateGridHeight(){
            testgrid.SetHeight(0);
            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if(document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            testgrid.SetHeight(containerHeight);
        }
    </script>
<%--    <dx:ASPxGridView runat="server" Width="100%" ID="ASPxGridView1" ClientInstanceName="sampleGrid"
            AutoGenerateColumns="true" DataSourceID="ds" KeyFieldName="ID" EnableRowsCache="false">
            <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="500" />
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <EditFormLayoutProperties>
                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
            </EditFormLayoutProperties>
            <SettingsEditing Mode="EditForm"></SettingsEditing>
            <settingspager pagesize="30" numericbuttoncount="6" />
            <Styles>
                <Cell Wrap="False"></Cell>
            </Styles>
        </dx:ASPxGridView>--%>

<div id="gridContainer" style="visibility: hidden; padding-right: 50px;">
<dx:ASPxGridView ID="testgrid" ClientInstanceName="testgrid" runat="server" Width="100%" 
    DataSourceID="ds"
    KeyFieldName="ID" EnableRowsCache="false"
    CssClass="employeesGridView"
    Border-BorderWidth="0">
<styles>
    <focusedrow BackColor="#f4dc7a" ForeColor="Black">
    </focusedrow>
</styles>
<SettingsBehavior AllowFocusedRow="True" />
<%--<SettingsCustomizationWindow Enabled="True" /> --%>
    <SettingsPager PageSize="20"></SettingsPager>
<Settings VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" ShowFooter="True" ShowStatusBar="Hidden"/>
<SettingsBehavior AllowSort="false" AllowGroup="false" AllowFocusedRow="True" AutoExpandAllGroups="true" 
    EnableRowHotTrack="True" ColumnResizeMode="Control" />
<SettingsPager AlwaysShowPager="true" />
</dx:ASPxGridView>        
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
         <asp:SqlDataSource ID="ds" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from Mas_InstallArea">
</asp:SqlDataSource>
</div>
<div id="EditFormsContainer">
<dx:ASPxHiddenField ID="hfid" runat="server" ClientInstanceName="hfid" />
<dx:ASPxCallback ID="clbCallback" runat="server" 
    ClientInstanceName="callback" oncallback="OnCallback">
</dx:ASPxCallback>
<dx:ASPxPopupControl ID="EntryEditPopup" ClientInstanceName="EntryEditPopup" runat="server" PopupHorizontalAlign="WindowCenter" ShowCloseButton="true" 
    CloseOnEscape="true" Width="100%" PopupVerticalAlign="WindowCenter" 
    CloseAction="CloseButton" OnWindowCallback="EntryEditPopup_WindowCallback" Modal="true" PopupAnimationType="Fade" CssClass="emplEditFormPopup">
    <ClientSideEvents PopUp="function(s,e){callback.PerformCallback('@@@');rbShift.Focus();}" />
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
        <dx:ASPxFormLayout ID="formLayout" ClientInstanceName="formLayout" CssClass="formLayout" runat="server" UseDefaultPaddings="false">
                <Items>
                    <dx:LayoutGroup Caption="Group (ColCount=3)" ColCount="1">
                        <Items>
                            <dx:LayoutItem Caption="Date">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxDateEdit runat="server" ID="deDate" ClientInstanceName="deDate" Width="230px" Border-BorderWidth="0"
                                            DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" ValidationGroup="group1"/>
                                        </dx:ASPxDateEdit>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Shift" Width="100%">
                                <ParentContainerStyle Paddings-PaddingRight="2"/>
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxComboBox ID="rbShift" runat="server" RepeatColumns="2" RepeatLayout="Table"
                                                RepeatDirection="Horizontal"
                                                Border-BorderWidth="0" TextField="Name">
                                            <Items>
                                                <dx:ListEditItem Text="DS" Value="0" Selected="true" />
                                                <dx:ListEditItem Text="NS" Value="1" />
                                            </Items>
                                        </dx:ASPxComboBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:EmptyLayoutItem />
                            <dx:LayoutItem Caption="X-RAY No.">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="tbXray" runat="server" Width="230px" ReadOnly="true"/>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Production Name">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxComboBox ID="CmbCode" runat="server" Width="230px" TextFormatString="{0}" ValueField="Id"
                                            DropDownWidth="230px"                    
                                            ClientInstanceName="ClientCode">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="Code"/>
                                                <dx:ListBoxColumn FieldName="Name" Width="230px"/>
                                            </Columns>
                                        </dx:ASPxComboBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                </Items>
            </dx:ASPxFormLayout>
            <br />
            <dx:ASPxGridView runat="server" ID="grid" KeyFieldName="ID" ClientInstanceName="grid" Width="100%" EnableRowsCache="false" 
                    OnDataBound="grid_DataBound"  OnCellEditorInitialize="grid_CellEditorInitialize" 
                    OnDataBinding="grid_DataBinding" 
                    OnCustomCallback="grid_CustomCallback" CssClass="employeesGridView"
                    AutoGenerateColumns="true"
                    Border-BorderWidth="0">
                    <SettingsBehavior AutoExpandAllGroups="true" EnableRowHotTrack="True" ColumnResizeMode="NextColumn" AllowFocusedRow="true" EnableCustomizationWindow="true"/>
		            <Styles>
			            <Row Cursor="pointer" />
		            </Styles>
                    <SettingsEditing EditFormColumnCount="1" Mode="EditFormAndDisplayRow"  />
                    <EditFormLayoutProperties>
                        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="368" />
                    </EditFormLayoutProperties>
                    <SettingsPager Mode="ShowAllRecords"/>
                    <SettingsCommandButton>
                        <UpdateButton RenderMode="Image">
                            <Image ToolTip="Save changes and close Edit Form" Url="~/Content/Images/update.png" />
                        </UpdateButton>
                        <CancelButton RenderMode="Image">
                            <Image ToolTip="Close Edit Form without saving changes" Url="~/Content/Images/cancel.png" />
                        </CancelButton>
                    </SettingsCommandButton>
                    <ClientSideEvents RowDblClick="grid_RowDblClick" CustomButtonClick="OnCustomButtonClick"  /> 
            </dx:ASPxGridView>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
</div>
<asp:SqlDataSource ID="dsResult" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="spselectpestall" SelectCommandType="StoredProcedure">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dstest" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select value from dbo.FNC_SPLIT('X;/',';')">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsgv" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="WebApp_SelectCmdV3" SelectCommandType="StoredProcedure">
</asp:SqlDataSource>