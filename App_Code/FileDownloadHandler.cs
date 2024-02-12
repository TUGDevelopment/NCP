using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public class FileDownloadHandler : IHttpHandler
{
    //private string dbPath = "~/App_Data/SampleDB.mdb";
    public void ProcessRequest(HttpContext context)
    {
        string id = context.Request["id"];
        byte[] content = GetFileContentByKey(context, id);

        ExportToResponse(context, content, "file_" + id, "xlsx", false);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private byte[] GetFileContentByKey(HttpContext context, object key)
    {
        SqlDataSource ds = new SqlDataSource();
        //ds.DataFile = dbPath;
        string connectionString = WebConfigurationManager.ConnectionStrings["ncpDbConnectionString"].ConnectionString;
        ds.ConnectionString = connectionString;

        ds.SelectCommand = "SELECT [Data] FROM [FileSystem] WHERE [id] = " + key.ToString();
        DataTable dt = ((DataView)ds.Select(DataSourceSelectArguments.Empty)).Table;
        byte[] file = dt.Rows[0]["Data"] as byte[];
        return file;
    }

    public void ExportToResponse(HttpContext context, byte[] content, string fileName, string fileType, bool inline)
    {
        context.Response.Clear();
        context.Response.ContentType = "application/" + fileType;
        context.Response.AddHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", inline ? "Inline" : "Attachment", 
            fileName, fileType));
        context.Response.AddHeader("Content-Length", content.Length.ToString());
        context.Response.BinaryWrite(content);
        context.Response.Flush();
        context.Response.Close();
        context.Response.End();
    }
}