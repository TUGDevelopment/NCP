using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;

public partial class Test_upload : System.Web.UI.Page
{
    const string UploadDirectory = "~/UploadControl/UploadImages/";
    const string ThumbnailFileName = "ThumbnailImage.jpg";

    protected void uplImage_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        if (!e.IsValid) e.CallbackData = string.Empty;

        string fileName = string.Format("~/UploadImages/{0}", e.UploadedFile.FileName);
        e.UploadedFile.SaveAs(MapPath(fileName), true);

        e.CallbackData = e.UploadedFile.FileName;
    }
}