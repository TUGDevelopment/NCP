<%@ Page Language="C#" AutoEventWireup="true" CodeFile="upload.aspx.cs" Inherits="Test_upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function Uploader_OnUploadStart() {
            btnUpload.SetEnabled(false);
        }
        function Uploader_OnFileUploadComplete(args) {
            var imgSrc;
            if (args.isValid) {
                var date = new Date();
                imgSrc = "UploadImages/" + args.callbackData + "?dx=" + date.getTime();
            }
            getPreviewImageElement().src = imgSrc;
        }
        function Uploader_OnFilesUploadComplete(args) {
            UpdateUploadButton();
        }
        function UpdateUploadButton() {
            btnUpload.SetEnabled(uploader.GetText(0) != "");
        }
        function getPreviewImageElement() {
            return document.getElementById("previewImage");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        <table cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center" style="padding-right: 20px; vertical-align: top;">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <dx:ASPxLabel ID="lblSelectImage" runat="server" Text="Select Image:" AssociatedControlID="uplImage">
                                    </dx:ASPxLabel>
                                </td>
                                <td>
                                    <dx:ASPxUploadControl ID="uplImage" runat="server" ClientInstanceName="uploader"
                                        ShowProgressPanel="True" Size="35" OnFileUploadComplete="uplImage_FileUploadComplete">
                                        <ClientSideEvents FileUploadComplete="function(s, e) { 
                                                Uploader_OnFileUploadComplete(e); 
                                            }" FilesUploadComplete="function(s, e) { 
                                                Uploader_OnFilesUploadComplete(e); 
                                            }" FileUploadStart="function(s, e) { 
                                                Uploader_OnUploadStart(); 
                                            }" TextChanged="function(s, e) { 
                                                UpdateUploadButton();   
                                            }"></ClientSideEvents>
                                        <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".jpg,.jpeg,.jpe,.gif,.png">
                                        </ValidationSettings>
                                    </dx:ASPxUploadControl>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <dx:ASPxLabel ID="lblAllowebMimeType" runat="server" Text="Allowed image types: jpeg, gif"
                                        Font-Size="8pt">
                                    </dx:ASPxLabel>
                                    <br />
                                    <dx:ASPxLabel ID="lblMaxFileSize" runat="server" Text="Maximum file size: 4Mb" Font-Size="8pt">
                                    </dx:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Text="Upload" ClientInstanceName="btnUpload"
                                        Width="100px" ClientEnabled="False">
                                        <ClientSideEvents Click="function(s, e) { 
                                            uploader.Upload(); 
                                        }" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="center" class="imagePreviewCell">
                        <img src="../Content/ImagePreview.gif" id="previewImage" alt="" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
