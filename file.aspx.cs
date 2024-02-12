using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class file : System.Web.UI.Page
{
    ServiceCS cs = new ServiceCS();
    protected void Page_Load(object sender, EventArgs e)
    {
        //using (WebClient webClient = new WebClient())
        //{
        //    webClient.DownloadFile("\\\\192.168.1.193\\ExcelFiles\\DataFile.xlsx", @"c:\temp\DataFile.xlsx");
        //}
        //cs.uploadfile("DataFile.xlsx", "", "");
        //TheDownload("\\\\192.168.1.193\\ExcelFiles\\DataFile.xlsx");
        string FileName = @"DataFile_"+  DateTime.Now.ToString("yyyyMMddHHmmss") +".xlsx"; // It's a file name displayed on downloaded file on client side.

        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.ClearContent();
        response.Clear();
        response.ContentType = "application/octet-stream";
        response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
        response.TransmitFile(Server.MapPath("~/ExcelFiles//DataFile.xlsx"));
        response.Flush();
        response.End();
        response.Write("load success");
    }
    public void TheDownload(string path)
    {
        var dir = @"c:\temp\DataFile.xlsx";
        CreateIfMissing(dir);
        System.IO.FileInfo toDownload = new System.IO.FileInfo("\\\\192.168.1.193\\ExcelFiles\\DataFile.xlsx");//;(HttpContext.Current.Server.MapPath(path));
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader("Content-Disposition",
                   "attachment; filename=" + toDownload.Name);
        HttpContext.Current.Response.AddHeader("Content-Length",
                   toDownload.Length.ToString());
        HttpContext.Current.Response.ContentType = "application/octet-stream";
        HttpContext.Current.Response.WriteFile(dir);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }
    private void CreateIfMissing(string dir)
    {
        bool folderExists = Directory.Exists(dir);
        if (!folderExists)
            Directory.CreateDirectory(dir);
    }
}