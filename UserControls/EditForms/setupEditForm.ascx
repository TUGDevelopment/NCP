<%@ Control Language="C#" AutoEventWireup="true" CodeFile="setupEditForm.ascx.cs" Inherits="UserControls_EditForms_setupEditForm" %>
<script type="text/javascript">
    var lastarea = null;
    function OnInit(s, e) {
        AdjustSize();
        //document.getElementById("gridContainer").style.visibility = "";
        DevAV.ChangeDemoState("MailList");
    }
    function SynchronizeListBoxValues(dropDown, args, text) {
        //debugger;
        var texts = dropDown.GetText().split(";");
        var values = GetValuesByTexts(texts, text, checkListBox);
        checkListBox.SelectValues(values);
        UpdateText(text, checkListBox, dropDownEdit); // for remove non-existing texts
        if (values != "")
            dropDown.isValid = false;
    }
    function OnListBoxSelectionChanged(s, e, text) {
        UpdateText(text, checkListBox, dropDownEdit);
    }
    function UpdateText(text, ListBox, dropDown) {
        var selectedItems = null;
        selectedItems =  ListBox.GetSelectedItems();
        dropDown.SetText(GetSelectedItemsText(selectedItems));
    }
    function GetSelectedItemsText(items) {
        var texts = [];
        for (var i = 0; i < items.length; i++)
            //if (items[i].index != 0)
            texts.push(items[i].text);
        return texts.join(";");
    }
    function GetValuesByTexts(texts, text , ListBox) {
        var actualValues = [];
        var item;
        for (var i = 0; i < texts.length; i++) {
            item = ListBox.FindItemByText(texts[i]);
            if (item != null)
                actualValues.push(item.value);
        }
        return actualValues;
    }
    function OnEndCallback(s, e) {
        AdjustSize();
        debugger;
        var key = gvResult.cpKeyValue;
        gvResult.cpKeyValue = null;
        if (key == 0)
            DevAV.ChangeDemoState("MailList");
    }
    function OnControlsInitialized(s, e) {
        ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
            AdjustSize();
        });
    }
    function AdjustSize() {
        var height = Math.max(0, document.documentElement.clientHeight);
        gvResult.SetHeight(height - 120);
    }
    function UpdateGridHeight() {
        gvResult.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        gvResult.SetHeight(containerHeight - 120);
    }
    function OnareaChanged(s) {
        if (gvResult.GetEditor("InstallArea").InCallback())
            lastarea = s.GetValue().toString();
        else
            gvResult.GetEditor("InstallArea").PerformCallback(s.GetValue().toString());
    }
    function cb_OnEndCallback(s, e) {
        if (lastarea) {
            gvResult.GetEditor("InstallArea").PerformCallback(lastarea);
            lastarea = null;
        }
    }
    function OnContextMenuItemClick(sender, args) {
        debugger;
        if (args.objectType == "row") {
            if (args.item.name == "PDF" || args.item.name == "XLS" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            }
            //alert(args.item.name);
            //if (args.item.name == "DeleteRow")
            //    if (!window.confirm("Confirm Delete?"))
            //return;
        }
    }
    </script>
<%--<div id="gridContainer" style="visibility: hidden; padding-right: 50px;"></div>--%>
<dx:ASPxGridView runat="server" Width="100%" ID="gvResult" ClientInstanceName="gvResult" OnCustomCallback="gvResult_CustomCallback" 
    OnFillContextMenuItems="gvResult_FillContextMenuItems" OnContextMenuItemClick="gvResult_ContextMenuItemClick"
    OnCellEditorInitialize="gvResult_CellEditorInitialize" KeyFieldName="ID" EnableViewState="false"
    OnDataBinding="gvResult_DataBinding" 
    OnDataBound="gvResult_DataBound" 
    OnRowUpdating="gvResult_RowUpdating"
    OnRowDeleting="gvResult_RowDeleting"
    OnRowInserting="gvResult_RowInserting" 
    EnableRowsCache="false">
    <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" ContextMenuItemClick="function(s,e) { OnContextMenuItemClick(s, e); }"/>
    <Settings VerticalScrollBarMode="Visible" VerticalScrollableHeight="100" />
    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
    <EditFormLayoutProperties>
        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
    </EditFormLayoutProperties>
    <SettingsEditing Mode="EditForm"></SettingsEditing>
    <settingspager pagesize="30" numericbuttoncount="6" />
    <Styles>
        <Cell Wrap="False"/>
        <focusedrow BackColor="#f4dc7a" ForeColor="Black"/>
    </styles>
    <SettingsPager PageSize="20" EnableAdaptivity="true">
        <PageSizeItemSettings Visible="true" ShowAllItem="true" AllItemText="All Records" />
    </SettingsPager>
    <SettingsBehavior ConfirmDelete="true" AllowFocusedRow="True" />
    <SettingsContextMenu Enabled="true"/>
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
<%--<dx:ASPxGridView runat="server" ID="gvResult" ClientInstanceName="gvResult" OnCustomCallback="gvResult_CustomCallback" 
    OnFillContextMenuItems="gvResult_FillContextMenuItems" OnContextMenuItemClick="gvResult_ContextMenuItemClick"
    OnCellEditorInitialize="gvResult_CellEditorInitialize" KeyFieldName="ID" EnableViewState="false"
    OnDataBinding="gvResult_DataBinding" Width="100%"
    OnDataBound="gvResult_DataBound" 
    OnRowUpdating="gvResult_RowUpdating"
    OnRowDeleting="gvResult_RowDeleting"
    OnRowInserting="gvResult_RowInserting"    
    Border-BorderWidth="0">
    <ClientSideEvents Init="OnInit" EndCallback="OnEndCallback" ContextMenuItemClick="function(s,e) { OnContextMenuItemClick(s, e); }"/>
    <SettingsSearchPanel ColumnNames="" Visible="false" />
    <styles>
        <focusedrow BackColor="#f4dc7a" ForeColor="Black">
        </focusedrow>
    </styles>
    <SettingsPager PageSize="20" EnableAdaptivity="true">
            <PageSizeItemSettings Visible="true" ShowAllItem="true" />
        </SettingsPager>
    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
    <EditFormLayoutProperties>
        <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
    </EditFormLayoutProperties>
    <SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="Control" AutoExpandAllGroups="True"/>
    <SettingsCustomizationWindow Enabled="True" /> 
    <Settings VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" ShowFooter="true" GridLines="None" ShowGroupPanel="True"/>
    <SettingsEditing EditFormColumnCount="1" Mode="EditFormAndDisplayRow" />
    <SettingsContextMenu Enabled="true" RowMenuItemVisibility-Refresh="true" />
    <SettingsCommandButton EditButton-Image-Url="~/Content/images/EditCustomerButton_Gray.png" EditButton-RenderMode="Image"
        UpdateButton-Image-Url="~/Content/images/update.png" UpdateButton-RenderMode="Image"
        CancelButton-Image-Url="~/Content/images/cancel.png" CancelButton-RenderMode="Image"  />
    <SettingsPopup>
            <EditForm Modal="true" AllowResize="true" />
    </SettingsPopup>
</dx:ASPxGridView>--%>
<dx:ASPxGridViewExporter runat="server" ID="GridExporter" GridViewID="grid" />
<div id="EditFormsContainer">
<dx:ASPxCallbackPanel ID="FormPanel" runat="server" RenderMode="Div" ClientVisible="false" ScrollBars="Vertical" ClientInstanceName="ClientCostFormPanel">
        <PanelCollection>
            <dx:PanelContent>  
            <dx:ASPxFormLayout runat="server" ID="formLayout" ClientInstanceName="formLayout" CssClass="formLayout">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="800" />
            <Items>
                <dx:LayoutGroup Caption="XXX" ColCount="3" GroupBoxDecoration="Box" UseDefaultPaddings="false" Paddings-PaddingTop="10">
                    <Paddings PaddingTop="10px"></Paddings>
                    <GroupBoxStyle>
                        <Caption Font-Bold="true" Font-Size="10"/>
                    </GroupBoxStyle>
                    <Items>
                        <dx:LayoutItem Caption="calendar">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxCalendar ID="calendar" runat="server" />
                                <%--<dx:ASPxComboBox ID="CmbCompany" runat="server" DataSourceID="dsCompany" ValueField="Code" Width="230px"
                                        ClientInstanceName="ClientCompany" TextFormatString="{0}" OnCallback="CmbCompany_Callback">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Code"/>
                                            <dx:ListBoxColumn FieldName="Name"/>
                                        </Columns>
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }"/>
                                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" ValidationGroup="group1"/>
                                    </dx:ASPxComboBox>--%>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
            </dx:ASPxFormLayout>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
<%--<dx:ASPxGridView ID="gvResult" runat="server" ClientInstanceName="gvResult" Width="100%" EnableRowsCache="false"
    Border-BorderWidth="0">
    <Columns>
        <dx:GridViewDataCheckColumn FieldName="" />
    </Columns>
</dx:ASPxGridView>--%>
</div>
<asp:SqlDataSource ID="dsGrouping" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select ID,shortname,Name,isnull(IsActive,'0')IsActive from Mas_Grouping" 
    UpdateCommand="update Mas_Grouping set Name=@Name,IsActive=@IsActive,shortname=@shortname where ID=@ID"
    InsertCommand="Insert into Mas_Grouping values(@Name,@shortname,@IsActive)" 
    DeleteCommand="Delete Mas_Grouping where ID=@ID">
    <UpdateParameters>
        <asp:Parameter Name="shortname" Type="String" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="IsActive" Type="Boolean" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="shortname" Type="String" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="IsActive" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsArea" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select ID,Name,shortname,Area ,case when IsActive is null then 0 else IsActive end IsActive from Mas_InstallArea" 
    UpdateCommand="update Mas_InstallArea set Name=@Name,shortname=@shortname,IsActive=@IsActive,Area=@Area where ID=@ID"
    InsertCommand="Insert into Mas_InstallArea values(@Name,@shortname,@Area,@IsActive)" 
    DeleteCommand="Delete Mas_InstallArea where ID=@ID">
    <UpdateParameters>
        <asp:Parameter Name="Area" Type="String" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="shortname" Type="String" />
        <asp:Parameter Name="IsActive" Type="Boolean" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Area" Type="String" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="shortname" Type="String" />
        <asp:Parameter Name="IsActive" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsparam" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select ID,GroupingID,Name,useType,DataValue,case when IsActive is null then 0 else IsActive end IsActive from Mas_param" 
    UpdateCommand="update Mas_param set GroupingID=@GroupingID,Name=@Name,useType=@useType,DataValue=@DataValue,IsActive=@IsActive where ID=@ID"
    InsertCommand="Insert into Mas_param values(@GroupingID,@Name,@useType,@DataValue,@IsActive)" 
    DeleteCommand="Delete Mas_param where ID=@ID">
    <UpdateParameters>
        <asp:Parameter Name="GroupingID" Type="Int32" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="useType" Type="String" />
        <asp:Parameter Name="DataValue" Type="String" />
        <asp:Parameter Name="IsActive" Type="Boolean" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="GroupingID" Type="Int32" />
        <asp:Parameter Name="Name" Type="String" />
        <asp:Parameter Name="useType" Type="String" />
        <asp:Parameter Name="DataValue" Type="String" />
        <asp:Parameter Name="IsActive" Type="Boolean" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dscontest" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select value from dbo.FNC_SPLIT('Number;TextBox;Combobox;CheckBox;RadioButton',';')">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsInstallArea" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select ID,GroupingID,InstallArea,PlantID,shortname as Equipment_No,case when IsActive is null then 0 else IsActive end IsActive from Mas_SamplingName" 
    UpdateCommand="Update Mas_SamplingName set GroupingID=@GroupingID,InstallArea=@InstallArea,PlantID=@PlantID ,shortname=@Equipment_No,IsActive=@IsActive where ID=@ID"
    InsertCommand="spInsertSaplingName" InsertCommandType="StoredProcedure" 
    DeleteCommand="Delete Mas_SamplingName where ID=@ID">
    <UpdateParameters>
        <asp:Parameter Name="GroupingID" Type="Int32" />
        <asp:Parameter Name="InstallArea" Type="Int32" />
        <asp:Parameter Name="PlantID" Type="Int32" />
        <asp:Parameter Name="Equipment_No" Type="String" />
        <asp:Parameter Name="IsActive" Type="Boolean" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="GroupingID" Type="Int32" />
        <asp:Parameter Name="InstallArea" Type="Int32" />
        <asp:Parameter Name="PlantID" Type="Int32" />
        <asp:Parameter Name="Equipment_No" Type="String" />
        <asp:Parameter Name="IsActive" Type="Boolean"/>
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsPlant" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select ID,Title as 'Name',shortname,0 IsActive from tblplant"
    UpdateCommand="update tblplant set Title=@Name,shortname=@shortname,IsActive=@IsActive where Id=@Id"
    InsertCommand="insert into tblplant (Title,shortname)values(@Title,@shortname)" >
        <UpdateParameters>
        <asp:Parameter Name="ID" />
        <asp:Parameter Name="Name" />
        <asp:Parameter Name="shortname" />
        <asp:Parameter Name="IsActive" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="Title" />
        <asp:Parameter Name="shortname" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsZone" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from dbo.FNC_SPLIT('ENCLOSED,INTERNAL',',')">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsUser" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select au_id,user_name,Email,FirstName,LastName,userlevel,ApprovedStep,Plant from ulogin" 
    UpdateCommand="update ulogin set Email=@Email,Userlevel=@Userlevel,Plant=@Plant,ApprovedStep=@ApprovedStep where au_id=@au_id"
    InsertCommand="insert into ulogin (user_name,Email,FirstName,LastName,Userlevel,ApprovedStep,Plant)values(@user_name,@Email,@FirstName,@LastName,@Userlevel,@ApprovedStep,@Plant)"
    DeleteCommand="Delete ulogin where au_id=@au_id">
    <UpdateParameters>
        <asp:Parameter Name="au_id" />
        <asp:Parameter Name="Email" />
        <asp:Parameter Name="Userlevel" />
        <asp:Parameter Name="Plant" />
	<asp:Parameter Name="ApprovedStep" />
    </UpdateParameters>
    <InsertParameters>
	<asp:Parameter Name="user_name" />
        <asp:Parameter Name="Email" />
	<asp:Parameter Name="FirstName" />
	<asp:Parameter Name="LastName" />
        <asp:Parameter Name="Userlevel" />
	<asp:Parameter Name="ApprovedStep" />
	<asp:Parameter Name="Plant" />
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsSupplier" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from tblSupplier" UpdateCommand="update tblSupplier set Name=@Name where Id=@Id">
    <UpdateParameters>
        <asp:Parameter Name="Id" />
        <asp:Parameter Name="Name" />
    </UpdateParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsPackaging" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from tblPackaging" 
    UpdateCommand="update tblPackaging set Name=@Name,Description=@Description,DataType=@DataType where Id=@Id">
    <UpdateParameters>
        <asp:Parameter Name="Id" />
        <asp:Parameter Name="Name" />
        <asp:Parameter Name="Description" />
        <asp:Parameter Name="DataType" />
    </UpdateParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from dbo.FNC_SPLIT('',',')">
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsProblem" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select * from tblProblem" InsertCommand="insert into tblProblem values(@Title,@LongText,@Location)"
    UpdateCommand="update tblProblem set Location=@Location,Title=@Title,LongText=@LongText where Id=@Id">
    <InsertParameters>
        <asp:Parameter Name="Location" />
        <asp:Parameter Name="Title" />
        <asp:Parameter Name="LongText" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" />
        <asp:Parameter Name="Location" />
        <asp:Parameter Name="Title" />
        <asp:Parameter Name="LongText" />
    </UpdateParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dslocation" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select Id,Name,Title,dbo.fnc_gettype(Plant) as Plant from tbllocation" InsertCommand="insert into tbllocation values(@Name,@Title,@Plant)"
    UpdateCommand="update tbllocation set Name=@Name,Title=@Title,Plant=dbo.fnc_settype(@Plant) where Id=@Id">
    <InsertParameters>
        <asp:Parameter Name="Plant" />
        <asp:Parameter Name="Title" />
        <asp:Parameter Name="Name" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" />
        <asp:Parameter Name="Plant" />
        <asp:Parameter Name="Title" />
        <asp:Parameter Name="Name" />
    </UpdateParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="dsNCPtype" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
    SelectCommand="select Id,Location,Title,dbo.fnc_gettype(Plant) as Plant from tbltype" InsertCommand="insert into tbltype values(@Title,@Plant,@Location,@Origin)"
    UpdateCommand="update tbltype set Location=@Location,Title=@Title,Plant=dbo.fnc_settype(@Plant),Origin=@Origin where Id=@Id">
    <InsertParameters>
        <asp:Parameter Name="Plant" />
        <asp:Parameter Name="Title" />
        <asp:Parameter Name="Location" />
        <asp:Parameter Name="Origin" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" />
        <asp:Parameter Name="Plant" />
        <asp:Parameter Name="Title" />
        <asp:Parameter Name="Location" />
        <asp:Parameter Name="Origin" />
    </UpdateParameters>
</asp:SqlDataSource>
