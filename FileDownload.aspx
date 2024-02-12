<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileDownload.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxGridView ID="Grid" runat="server" ClientInstanceName="grid" KeyFieldName="id" AutoGenerateColumns="False" 
            DataSourceID="dsgv">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="id" VisibleIndex="0" ReadOnly="True">
                    <EditFormSettings Visible="False" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="1">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="SystemDate" />
                <dx:GridViewDataColumn FieldName="Data" VisibleIndex="2">
                    <DataItemTemplate>
                        <dx:ASPxButton ID="Button" runat="server" OnInit="Button_Init" AutoPostBack="False" RenderMode="Link">
                        </dx:ASPxButton>
                        <dx:ASPxUploadControl ID="UploadControl" runat="server" ClientVisible="false" ShowProgressPanel="True" 
                            UploadMode="Auto" AutoStartUpload="true" Width="280px" OnFileUploadComplete="UploadControl_FileUploadComplete">
                            <ValidationSettings AllowedFileExtensions=".xls,.xlsx"></ValidationSettings>
                        </dx:ASPxUploadControl>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
            </Columns>
        </dx:ASPxGridView>
        <asp:SqlDataSource ID="dsgv" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"
            SelectCommand="spGetFileSystem"  SelectCommandType="StoredProcedure"
            UpdateCommand="UPDATE Sample SET File = ? WHERE (ID = ?)" OnUpdating="SqlDataSource1_Updating">
            <UpdateParameters>
                <asp:Parameter Name="File" />
                <asp:Parameter Name="ID" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="ds" runat="server" ConnectionString="<%$ ConnectionStrings:ncpDbConnectionString %>"/>
    </div>
    </form>
</body>
</html>
