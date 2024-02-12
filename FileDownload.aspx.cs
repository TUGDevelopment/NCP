using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    private string dbPath = "~/App_Data/SampleDB.mdb";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            Session.Clear();
    }
    protected void Button_Init(object sender, EventArgs e)
    {
        ASPxButton button = (ASPxButton)sender;
        GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)button.NamingContainer;
        ASPxUploadControl uploadControl = container.FindControl("UploadControl") as ASPxUploadControl;

        button.ClientInstanceName = button.ClientID;
        uploadControl.ClientInstanceName = uploadControl.ClientID;

        button.ClientSideEvents.Click = string.Format("function(s, e) {{" + 
            " if (s.isFileUploaded) {{ window.location = 'FileDownloadHandler.ashx?id={0}'; }} " + 
            "else {{ s.SetVisible(false); {1}.SetVisible(true); }} }}",
                container.KeyValue, uploadControl.ClientInstanceName);

        uploadControl.ClientSideEvents.FileUploadComplete = string.Format("function(s, e) {{ " + 
            "s.SetVisible(false); {0}.SetVisible(true); {0}.SetText('Download'); {0}.isFileUploaded = true; }}", 
            button.ClientInstanceName);

        button.ClientSideEvents.Init = FileExists(container.KeyValue) ? 
            "function(s, e) { s.isFileUploaded = true; s.SetText('Download'); }" : 
            "function(s, e) { s.isFileUploaded = false; s.SetText('Upload'); }";  
    }
    private bool FileExists(object key)
    {
        //SqlDataSource ds = new SqlDataSource();
        //ds.DataFile = dbPath;
        ds.SelectCommand = "SELECT [Data] FROM [FileSystem] WHERE [ID] = " + key.ToString();
        DataTable dt = ((DataView)ds.Select(DataSourceSelectArguments.Empty)).Table;
        byte[] file = dt.Rows[0]["Data"] as byte[];
        return file != null;
    }

    protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        ASPxUploadControl uploadControl = (ASPxUploadControl)sender;
        GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)uploadControl.NamingContainer;
        dsgv.UpdateParameters["ID"].DefaultValue = container.KeyValue.ToString();
        Session["File"] = e.UploadedFile.FileBytes;
        dsgv.Update();
    }

    protected void SqlDataSource1_Updating(object sender, SqlDataSourceCommandEventArgs e)
    {
        byte[] file = (byte[])Session["File"];
        e.Command.Parameters["File"].Value = file;
    }
}