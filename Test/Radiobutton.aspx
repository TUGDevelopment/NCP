<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Radiobutton.aspx.cs" Inherits="Test_Radiobutton" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <dx:ASPxGridView ID="grid1" runat="server" ClientInstanceName="grid1" AutoGenerateColumns="False" OnDataBinding="grid1_DataBinding" 
                OnHtmlRowCreated="grid1_HtmlRowCreated" Width="100%" EnableViewState="False" ViewStateMode="Disabled" Theme="DevEx">
                <SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="Control" />
                <Settings ShowVerticalScrollBar="True" VerticalScrollableHeight="500" />
            </dx:ASPxGridView>
<%--    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" KeyFieldName="ID" ClientVisible="false">
                    <Columns>
                        <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowEditButton="true" VisibleIndex="0" />
                        <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="0" Visible="false">
                            <EditFormSettings Visible="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ID" VisibleIndex="1" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="GroupingID" VisibleIndex="2"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="3">
                            <DataItemTemplate>
                                <asp:RadioButtonList ID="rblYesNoNA" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                    <asp:ListItem Value="No">No</asp:ListItem>
                                    <asp:ListItem Value="NA">N/A</asp:ListItem>
                                </asp:RadioButtonList>
                            </DataItemTemplate>
                            <EditItemTemplate>
                                <asp:RadioButtonList ID="rblYesNoNA" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                    <asp:ListItem Value="No">No</asp:ListItem>
                                    <asp:ListItem Value="NA">N/A</asp:ListItem>
                                </asp:RadioButtonList>
                            </EditItemTemplate>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsPager Mode="ShowAllRecords" />
                    <SettingsEditing Mode="EditForm" />
                </dx:ASPxGridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
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
                </asp:SqlDataSource>--%>
    </form>
</body>
</html>
