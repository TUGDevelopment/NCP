using DevExpress.Pdf;
using DevExpress.Spreadsheet;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class ExportTo : System.Web.UI.Page
{
    Worksheet worksheet;
    MyDataModule cs = new MyDataModule();
    string strConn = ConfigurationManager.ConnectionStrings["ncpDbConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var _name = (Request.QueryString["Id"] == null) ? "FAQCSN11_7" : Request.QueryString["Id"].ToString();
            //Spreadsheet.Document.LoadDocument(Server.MapPath(@"~/ExcelFiles/" + _name + ".xlsx"));
            ////Spreadsheet.Open(Server.MapPath(@"~/ExcelFiles/" + _name + ".xlsx"));
            ////Spreadsheet.Document.DocumentSettings.Calculation.Iterative = true;
            ////worksheet = Spreadsheet.Document.Worksheets[0];
            ////IWorkbook workbook = Spreadsheet.Document;
            //Spreadsheet.WorkDirectory = "~/App_Data/WorkDirectory";
            //Spreadsheet.SettingsDocumentSelector.EditingSettings.AllowCopy = true;
            //Spreadsheet.SettingsDocumentSelector.EditingSettings.AllowCreate = true;
            //Spreadsheet.SettingsDocumentSelector.ToolbarSettings.Visible = true;
            //Spreadsheet.SettingsDocumentSelector.EditingSettings.AllowDelete = true;
            //Spreadsheet.SettingsDocumentSelector.EditingSettings.AllowMove = true;
            //Spreadsheet.SettingsDocumentSelector.EditingSettings.AllowRename = true;
            //Spreadsheet.SettingsDocumentSelector.UploadSettings.Enabled = true;

            //Spreadsheet.SettingsDocumentSelector.UploadSettings.UseAdvancedUploadMode = true;

            //Spreadsheet.CreateDefaultRibbonTabs(true);
        }
    }
}