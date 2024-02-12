<%@ Control Language="C#" AutoEventWireup="true" CodeFile="transEditForm.ascx.cs" Inherits="UserControls_transEditForm" %>
<script type="text/javascript">
        var gridIsBound = false;
        function OnTabChanging(s, e) {
            var tabName = (pageControl.GetActiveTab()).name;
            e.cancel = !ASPxClientEdit.ValidateGroup('group' + tabName);
        }
        function OnButtonClick(s, e) {

            //var indexTab = (pageControl.GetActiveTab()).index;
            ////pageControl.SetActiveTab(pageControl.GetTab(indexTab + 1));
            if (!ASPxClientEdit.ValidateGroup('groupTabPersonal'))
                return alert('Field is required');
            transferEditFormLayout.GetItemByName("lyinput").SetVisible(false);
            hidelayout(true);
            myFunction();
            //alert("addRow");
            debugger;
            DevAV.test(e, s);
            //tokenBoxTo.SetFocus();
            //if (!gridIsBound) {
            //    transferEditPopup.PerformCallback('AddRow');
            //    gridIsBound = true;
            //}
            testgrid.PerformCallback('AddRow');
            tokenBoxTo.SetText("");
            txtWeight.SetText("");
        }
        function DoProcessEnterKey(htmlEvent, editName) {
            if (htmlEvent.keyCode == 13) {
                ASPxClientUtils.PreventEventAndBubble(htmlEvent);
                if (editName) {
                    ASPxClientControl.GetControlCollection().GetByName(editName).SetFocus();
                } else {
                    btn.DoClick();
                }
            }
        }
        function ConfirmAndExecute(s, e) {
            transferEditFormLayout.GetItemByName("lyinput").SetVisible(true);
            hidelayout(false);
            myFunction();
        }
        function hidelayout(identi) {
            transferEditFormLayout.GetItemByName("lytest").SetVisible(identi);
        }
        function myFunction() {
            var x = document.getElementById("transferButton");
            if (x.style.display === "none") {
                x.style.display = "block";
            } else {
                x.style.display = "none";
            }
        }
        function OnClose(s, e) {
            transferEditFormLayout.GetItemByName("lyinput").SetVisible(false);
            hidelayout(true);
            myFunction();
        }
    </script>
<dx:ASPxGridView runat="server" ID="gridData" KeyFieldName="Id" DataSourceID="dsgv" ClientInstanceName="gridData" Width="100%" EnableRowsCache="false" 
        OnCustomCallback="gridData_CustomCallback"   
        AutoGenerateColumns="true"
        Border-BorderWidth="0">
        <SettingsBehavior AutoExpandAllGroups="true" EnableRowHotTrack="True" ColumnResizeMode="NextColumn" AllowFocusedRow="true" EnableCustomizationWindow="true"/>
		<Styles>
			<Row Cursor="pointer" />
		</Styles>
        <ClientSideEvents RowDblClick="DevAV.gridData_RowDblClick" /> 
</dx:ASPxGridView>
<dx:ASPxGridViewExporter runat="server" ID="GridExporter" GridViewID="gridData" />
<div id="EditFormsContainer">
<dx:ASPxHiddenField ID="hfid" runat="server" ClientInstanceName="hfid" />
<dx:ASPxCallbackPanel ID="PreviewPanel" runat="server" RenderMode="Div" Height="100%" CssClass="PreviewPanel" ClientInstanceName="ClientPreviewPanel" 
    ClientVisible="false" OnCallback="PreviewPanel_Callback" >
</dx:ASPxCallbackPanel>
<dx:ASPxCallback ID="clbCallback" runat="server" 
            ClientInstanceName="callback" oncallback="OnCallback">
        </dx:ASPxCallback>
<dx:ASPxPopupControl ID="transferEditPopup" ClientInstanceName="transferEditPopup" runat="server" PopupHorizontalAlign="WindowCenter" ShowCloseButton="false" CloseOnEscape="true"
    PopupVerticalAlign="WindowCenter" CloseAction="None" OnWindowCallback="transferEditPopup_WindowCallback" Modal="true" PopupAnimationType="Fade" CssClass="emplEditFormPopup">
    <ClientSideEvents PopUp="function(s,e){callback.PerformCallback('@@@');}" />
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <dx:ASPxFormLayout ID="transferEditFormLayout" runat="server" AlignItemCaptionsInAllGroups="True">
                <Styles>
                    <LayoutGroup CssClass="group"></LayoutGroup>
                </Styles>
                <Items>
                    <dx:LayoutGroup ColCount="2" GroupBoxDecoration="None">
                        <Items>
                        <dx:LayoutItem Caption="Type">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxRadioButtonList ID="radioButtonList" runat="server" RepeatDirection="Horizontal"
                                             ClientInstanceName="radioButtonList" DataSourceID="dsShift" TextField="value" ValueField="value"
                                             Border-BorderStyle="None" Paddings-PaddingLeft="0px" FocusedStyle-Border-BorderStyle="None"
                                             SelectedIndex="0" RepeatLayout="Flow">
                                        <CaptionSettings Position="Top" />
                                    </dx:ASPxRadioButtonList>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Books No">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="txtBooksNo" runat="server" Width="170px" ClientInstanceName="txtBooksNo">
                                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" ValidationGroup="group1"/>
                                        <ClientSideEvents KeyDown="function(s, e) { DoProcessEnterKey(e.htmlEvent, 'ClientDestination'); }" />
                                    </dx:ASPxTextBox>            
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Destination">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxComboBox ID="CmbDestination" runat="server" ValueField="Id" DataSourceID="dsDestination"
                                            ClientInstanceName="ClientDestination" TextFormatString="{0}">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="Id"/>
                                                <dx:ListBoxColumn FieldName="Name"/>
                                            </Columns>
                                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" ValidationGroup="group1"/>
                                            <ClientSideEvents KeyDown="function(s, e) { DoProcessEnterKey(e.htmlEvent, 'txtCarNumber'); }" />
                                        </dx:ASPxComboBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Car Number">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxComboBox ID="CmbCarNumber" runat="server" Width="250px" DataSourceID="dsCarNumber"
                                            TextFormatString="{0}" ClientInstanceName="CmbCarNumber" >
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="Name"/>
                                                <dx:ListBoxColumn FieldName="Responsible"/>
                                            </Columns>
                                        <ClientSideEvents KeyDown="function(s, e) { DoProcessEnterKey(e.htmlEvent, 'radioButtonList'); }" />
                                        </dx:ASPxComboBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                     <dx:LayoutGroup ColCount="2" GroupBoxDecoration="None" Name="lytest">
                        <Items>
                            <dx:LayoutItem Caption="" ColSpan="2">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxMenu ID="ASPxMenu1" runat="server" CssClass="ActionMenu" 
                                            ClientInstanceName="ASPxMenu1" SeparatorWidth="0" BackColor="White">
                                        <Items>
                                            <dx:MenuItem Text="Add Row" Name="new"  Image-Url="~/Content/Images/icons8-plus-math-filled-16.png"/>
                                            <dx:MenuItem Text="Clear" Name="clear" Image-Url="~/Content/Images/Delete.gif" />
                                            <dx:MenuItem Text="Export to" BeginGroup="true" Image-Url="~/Content/Images/if_sign-out_59204.png">
                                                <Items>
                                                    <dx:MenuItem Name="ExportToPDF" Text="PDF" Image-Url="~/Content/Images/excel.gif"/>
                                                </Items>
                                            </dx:MenuItem>
                                        </Items>
                                        <Border BorderWidth="0" />
                                        <ClientSideEvents ItemClick="ConfirmAndExecute" />
                                        </dx:ASPxMenu>
                                        <dx:ASPxGridView ID="testgrid" ClientInstanceName="testgrid" runat="server" Width="100%" 
                                            KeyFieldName="ID" EnableRowsCache="false"
                                            OnDataBinding="testgrid_DataBinding"
                                            OnDataBound="testgrid_DataBound"
                                            
                                            OnCustomCallback="testgrid_CustomCallback" >
                                        </dx:ASPxGridView>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                    <dx:LayoutGroup Caption="" ColCount="2" GroupBoxDecoration="None" Name="lyinput" ClientVisible="false">
                        <Items>
                            <dx:LayoutItem Caption="">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxCheckBox ID="showTabs" runat="server" AutoPostBack="true" Text="Show Tabs" 
                                            ClientVisible="false"
                                            OnCheckedChanged="showTabs_CheckedChanged" OnInit="showTabs_Init">
                                            </dx:ASPxCheckBox><br />
                                            <dx:ASPxPageControl ID="pageControl" ClientInstanceName="pageControl" runat="server"
                                                ActiveTabIndex="0" EnableHierarchyRecreation="true" >
                                                <ClientSideEvents ActiveTabChanging="OnTabChanging" />
                                                <TabPages>
                                                    <dx:TabPage Name="TabPersonal" Text="input">
                                                        <ContentCollection>
                                                            <dx:ContentControl runat="server">
                                                                <dx:ASPxLabel ID="lblFirstName" runat="server" Text="bin">
                                                                </dx:ASPxLabel>
                                                                <dx:ASPxTokenBox runat="server" ID="TokenBoxTo" ClientInstanceName="tokenBoxTo" DataSourceID="TokenBoxDataSource" 
                                                                    TextField="CarrierID" ShowDropDownOnFocus="Never">
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="group1">
                                                                        <RequiredField IsRequired="True" ErrorText="Name is required"/>
                                                                    </ValidationSettings>
                                                                    <ClientSideEvents KeyDown="function(s, e) { DoProcessEnterKey(e.htmlEvent, 'txtWeight'); }" />
                                                                </dx:ASPxTokenBox>
                                                                <hr />
                                                                <dx:ASPxLabel ID="lblLastName" runat="server" Text="Weight">
                                                                </dx:ASPxLabel>
                                                                <dx:ASPxTextBox ID="txtWeight" runat="server" Width="170px" ClientInstanceName="txtWeight">
                                                                    <ValidationSettings SetFocusOnError="true" ValidationGroup="groupTabPersonal">
                                                                        <RequiredField IsRequired="true" ErrorText="Weigh is required" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                                <hr />                           
                                                                <%--<dx:ASPxCheckBox ID="checkBox" runat="server" ClientInstanceName="checkBox" Text="I agree">
                                                                    <ValidationSettings SetFocusOnError="True" ValidationGroup="groupTabPersonal">
                                                                        <RequiredField IsRequired="True" ErrorText="I agree is required" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxCheckBox>--%>
                                                                <hr />
                                                                <dx:ASPxButton ID="btnNextPersonal" runat="server" Text="Next" ClientInstanceName="btnNextPersonal"
                                                                   AutoPostBack="false" ValidationGroup="groupTabPersonal">
                                                                    <ClientSideEvents Click="OnButtonClick" />
                                                                </dx:ASPxButton>
                                                                <dx:ASPxButton ID="btnClose" runat="server" AutoPostBack="False" UseSubmitBehavior="False" Text="Close">
                                                                    <ClientSideEvents Click="OnClose" />
                                                                </dx:ASPxButton>
                                                                <dx:ASPxValidationSummary ID="validSummaryPersonal" runat="server" ValidationGroup="groupTabPersonal">
                                                                </dx:ASPxValidationSummary>
                                                            </dx:ContentControl>
                                                        </ContentCollection>
                                                    </dx:TabPage>
                                                </TabPages>
                                            </dx:ASPxPageControl>
                                        <dx:ASPxPopupControl ID="popupControl" runat="server" CloseAction="CloseButton" ClientInstanceName="popupControl"
                                            HeaderText="Summary" PopupHorizontalAlign="OutsideRight" PopupHorizontalOffset="10">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl runat="server">
                                                </dx:PopupControlContentControl>
                                            </ContentCollection>
                                        </dx:ASPxPopupControl>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
            <div class="buttonsContainer" id="transferButton">
                <dx:ASPxButton ID="TaskSaveButton" runat="server" AutoPostBack="false" Text="Save" Width="100px">
                    <ClientSideEvents Click="DevAV.xxxSaveButton_Click" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="TaskCancelButton" runat="server" AutoPostBack="False" UseSubmitBehavior="False" Text="Cancel" Width="100px">
                    <ClientSideEvents Click="function(s, e) { transferEditPopup.Hide(); }" />
                </dx:ASPxButton>
            </div>
            <div style="clear: both">
           </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
   <%-- <dx:ASPxCallbackPanel ID="FormPanel" runat="server" RenderMode="Div" ClientVisible="true" ScrollBars="Vertical" ClientInstanceName="ClientCostFormPanel">
        <PanelCollection>
            <dx:PanelContent>  
            <dx:ASPxFormLayout runat="server" ID="formLayout" ClientInstanceName="formLayout" CssClass="formLayout">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="800" />
            <Items>
                <dx:LayoutGroup Caption="outxxx" ColCount="1" GroupBoxDecoration="Box" UseDefaultPaddings="false" Paddings-PaddingTop="10">
                    <SettingsItemCaptions Location="Left" HorizontalAlign="Left"/>
                    <Paddings PaddingTop="10px"></Paddings>
                    <GroupBoxStyle>
                        <Caption Font-Bold="true" Font-Size="10"/>
                    </GroupBoxStyle>
                    <Items>
                        <dx:LayoutItem Caption="Books No.">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox runat="server" ID="tbBooksNo" 
                                        ClientInstanceName="ClientBooksNo"/>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Date">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox runat="server" ID="tbDate" 
                                        ClientInstanceName="ClientDate"/>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Shift">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxRadioButtonList ID="radioButtonList" runat="server" DataSourceID="dsShift" RepeatDirection="Horizontal"
                                             Border-BorderStyle="None" Paddings-PaddingLeft="0px" FocusedStyle-Border-BorderStyle="None"
                                            TextField="Value" RepeatLayout="Flow">
                                        <CaptionSettings Position="Top" />
                                    </dx:ASPxRadioButtonList>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Destination">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="CmbDestination" runat="server" ValueField="Id"
                                        ClientInstanceName="ClientDestination" TextFormatString="{0}">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Id"/>
                                            <dx:ListBoxColumn FieldName="Name"/>
                                        </Columns>
                                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" ValidationGroup="group1"/>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Car Number">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                <dx:ASPxComboBox ID="Cmblicense" runat="server" ValueField="Id"
                                        ClientInstanceName="Clientlicense" TextFormatString="{0}">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="Id"/>
                                            <dx:ListBoxColumn FieldName="Name"/>
                                        </Columns>
                                        <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" ValidationGroup="group1"/>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
            </dx:ASPxFormLayout>
        </dx:PanelContent>
    </PanelCollection>
  </dx:ASPxCallbackPanel>--%>
</div>
<asp:SqlDataSource ID="dsgv" runat="server" ConnectionString="<%$ ConnectionStrings:constr %>"
    SelectCommand="spselectall" SelectCommandType="StoredProcedure">
</asp:SqlDataSource>
  <asp:SqlDataSource ID="dsDestination" runat="server" ConnectionString="<%$ ConnectionStrings:constr %>"
        SelectCommand="select * from tbDestination">
  </asp:SqlDataSource>
  <asp:SqlDataSource ID="dsShift" runat="server" ConnectionString="<%$ ConnectionStrings:constr %>"
        SelectCommand="select value from dbo.FNC_SPLIT('I;O',';')">
  </asp:SqlDataSource>
  <asp:SqlDataSource ID="dsCarNumber" runat="server" ConnectionString="<%$ ConnectionStrings:constr %>"
        SelectCommand="select * from tbCarNumber">
  </asp:SqlDataSource>
  <asp:SqlDataSource ID="TokenBoxDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:constr %>"
        SelectCommand="select * from tbCarrier">
  </asp:SqlDataSource>