using DevExpress.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_EditForms_ResultForm : System.Web.UI.UserControl
{
    MyDataModule cs = new MyDataModule();
    ServiceCS myclass = new ServiceCS();
    string user_name = HttpContext.Current.User.Identity.Name.Replace(@"THAIUNION\", @"");
    //HttpContext c = HttpContext.Current;
    string fExpr
    {
        get { return Page.Session["fExpr"] == null ? "X" : Page.Session["fExpr"].ToString(); }
        set { Page.Session["fExpr"] = value; }
    }
    private DataTable _testTable
    {
        get { return Page.Session["sessionKey"] == null ? null : (DataTable)Page.Session["sessionKey"]; }
        set { Page.Session["sessionKey"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session.Clear();
            DateTime today = DateTime.Today;
            DateTime b = DateTime.Now.AddDays(-30);
            filter.FilterExpression = string.Format("[KeyDate] Between(#{0}#, #{1}#)",
                    b.ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd"));
            hGeID["GeID"] = string.Format("{0}", 0);
            this.Session["user_name"] = user_name;
            this.Session["Expr"] = string.Format("{0}", fExpr);
            heditor["editor"] = string.Format("{0}", 1);
        }
    }

    protected void testgrid_CustomDataCallback(object sender, DevExpress.Web.ASPxGridViewCustomDataCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        var args = e.Parameters.Split('|');
        long id;
        if (!long.TryParse(args[2], out id))
            return;
        var result = new Dictionary<string, string>();
        if (args[1] == "EditDraft" || args[1] == "save")
        {

            SqlParameter[] param = {new SqlParameter("@ID",id.ToString()),
                        new SqlParameter("@username",user_name.ToString())};
            DataTable dt = cs.GetRelatedResources("spselectresult", param);
            foreach (DataRow rw in dt.Rows)
            {
                result["Plant"] = string.Format("{0}", rw["Plant"]);
                result["SampleDate"] = string.Format("{0}", rw["SampleDate"]);
                result["CoolTime"] = string.Format("{0}", rw["CoolTime"]);
                result["RetortNo"] = string.Format("{0}", rw["RetortNo"]);
                result["KeyDate"] = string.Format("{0}", rw["KeyDate"]);
                result["Temp"] = string.Format("{0}", rw["Temp"]);
                result["ResidualChlorine"] = string.Format("{0}", rw["ResidualChlorine"]);
                result["Cook"] = string.Format("{0}", rw["Cook"]);
                result["LabAnalysis"] = string.Format("{0}", rw["LabAnalysis"]);
                result["TestReport"] = string.Format("{0}", rw["TestReport"]);
                result["Remark"] = string.Format("{0}", rw["Remark"]);
                result["EmployeeName"] = string.Format("{0}", rw["EmployeeName"]);
                result["SampleType"] = string.Format("{0}", rw["SampleType"]);
                result["PositionID"] = string.Format("{0}", rw["PositionID"]);
                result["ResultDate"] = string.Format("{0}", rw["ResultDate"]);
                result["Shift"] = string.Format("{0}", rw["Shift"]);
                result["ProductType"] = string.Format("{0}", rw["ProductType"]);
                e.Result = result;
            }

        }
    }

    protected void testgrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        var args = e.Parameters.Split('|');
        long id;
        if (!long.TryParse(args[1], out id))
            return;
        var result = new Dictionary<string, string>();
        DataTable dt = new DataTable();
        if (args[0] == "SaveMail" || args[0] == "New")
        {
            SqlParameter[] param = {new SqlParameter("@ID",string.Format("{0}", hGeID["GeID"])),
                        new SqlParameter("@KeyDate",string.Format("{0}", deKeyDate.Value)),
                        new SqlParameter("@SampleDate",string.Format("{0}", deSampleDate.Value)),
                        new SqlParameter("@PositionID",string.Format("{0}", CmbPosition.Value)),
                        new SqlParameter("@ProductType",string.Format("{0}", Getrb(OptType))),
                        new SqlParameter("@Plant",string.Format("{0}", cmbplant.Text)),
                        new SqlParameter("@GroupTPC",""),
                        new SqlParameter("@CoolTime",string.Format("{0}", txtTime.Text)),
                        new SqlParameter("@RetortNo",string.Format("{0}", txtRetortNo.Text)),
                        new SqlParameter("@Temp",string.Format("{0}", txtTemp.Text)),
                        new SqlParameter("@ResidualChlorine",string.Format("{0}", txtResidualchlorine.Text)),
                        new SqlParameter("@Cook",string.Format("{0}", txtCook.Text)),
                        new SqlParameter("@Shift",string.Format("{0}", Getrb(cbShift))),
                        new SqlParameter("@CreateBy",user_name.ToString()),
                        new SqlParameter("@Employee", string.Format("{0}", CmbRecorder.Value)),
                        new SqlParameter("@LabAnalysis", string.Format("{0}", cmbLabanalysis.Text)),
                        new SqlParameter("@ResultDate", string.Format("{0}", deResultDate.Value)),
                        new SqlParameter("@TestReport", string.Format("{0}", txtTestReport.Text)),
                        new SqlParameter("@SampleType", string.Format("{0}", CmbType.Value)),
                        new SqlParameter("@Remark",string.Format("{0}", mNotes.Value))};
            DataTable table = new DataTable();
            table = myclass.GetRelatedResourcesDb("spinsertResult", param);
            foreach (DataRow dr in table.Rows)
            {
                string Keys = dr["ID"].ToString();
                foreach (DataRow rw in _testTable.Rows)
                    using (SqlConnection con = new SqlConnection(cs.strConn))
                    {
                        using (SqlCommand cmd = new SqlCommand("spInsertitems"))
                        {
                            cmd.Connection = con;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Name", rw["Name"].ToString());
                            cmd.Parameters.AddWithValue("@Result", rw["Result"].ToString());
                            cmd.Parameters.AddWithValue("@StatusApp", rw["StatusApp"].ToString());
                            cmd.Parameters.AddWithValue("@RequestNo", Keys.ToString());
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                g.JSProperties["cpSelectedRowKey"] = string.Format("{0}", dr["ID"]);
            }
            //}
        }
        if (args[0] == "filter")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(filter.GetFilterExpressionForMsSql());
            fExpr = sb.ToString();
            g.JSProperties["cpKeyValue"] = "filter|" + fExpr;
        }
        g.DataBind();
    }
    public string Getrb(ASPxRadioButtonList rb)
    {
        string result = "";
        StringBuilder sb = new StringBuilder();
        foreach (ListEditItem item in rb.Items)
        {
            if (item.Selected)
                sb.Append(item.Value + ";");
            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                result = sb.ToString();
                result = result.Substring(0, (result.Length - 1));
            }
        }
        return result;
    }
    const string UploadDirectory = "~/Content/UploadControl/";
    protected void UploadControl_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
    {
        string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
        string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), resultExtension);
        string resultFileUrl = UploadDirectory + resultFileName;
        string resultFilePath = MapPath(resultFileUrl);
        //Save the uploaded file
        e.UploadedFile.SaveAs(resultFilePath);
        string name = e.UploadedFile.FileName;
        string mimeType = MimeTypes.GetContentType(name);
        string url = ResolveClientUrl(resultFileUrl);
        long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
        string sizeText = sizeInKilobytes.ToString() + " KB";
        string Id = string.Format("{0}", hGeID["GeID"]);
        byte[] FileData = myclass.ReadFile(resultFilePath);
        //ArtsDataSource.InsertParameters["Name"].DefaultValue = name.ToString();
        //ArtsDataSource.InsertParameters["IsFolder"].DefaultValue = "true";
        //ArtsDataSource.InsertParameters["ParentID"].DefaultValue ="1";
        //ArtsDataSource.InsertParameters["Data"]. = myservice.ReadFile(resultFilePath);
        //ArtsDataSource.InsertParameters["LastWriteTime"].DefaultValue = DateTime.Now.ToString();
        //ArtsDataSource.InsertParameters["GCRecord"].DefaultValue = String.Format("{0}", hfgetvalue["NewID"]);
        //ArtsDataSource.InsertParameters["LastUpdateBy"].DefaultValue = hfuser["user_name"].ToString();
        //ArtsDataSource.Insert();
        using (SqlConnection con = new SqlConnection(cs.strConn))
        {
            string query = "spinsertFileSystem";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@ContentType", mimeType);
                cmd.Parameters.AddWithValue("@Data", FileData);
                cmd.Parameters.AddWithValue("@MatDoc", Id);
                cmd.Parameters.AddWithValue("@ActiveBy",user_name);
                cmd.Parameters.AddWithValue("@SystemDate", (object)DateTime.Now);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        e.CallbackData = name + "|" + url + "|" + sizeText;
    }
    DataTable GetTable()
    {
        // Here we create a DataTable with four columns.
        var data = Convert.ToInt32(hGeID["GeID"]);
        DataTable table = new DataTable();
        SqlParameter[] param = { new SqlParameter("@data", data.ToString()) };
        table = myclass.GetRelatedResourcesDb("spGetDetail", param);
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = table.Columns["ID"];
        table.PrimaryKey = keyColumns;
        return table;
    }

    protected void lvwd_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        g.KeyFieldName = "ID";
        g.DataSource = _testTable;
        g.ForceDataRowType(typeof(DataRow));
    }

    protected void lvwd_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        var args = e.Parameters.Split('|');
        long id;
        //if (args[0] == "edit")
        //{
        //    string result = args[1];
        //    dynamic dynJson = JsonConvert.DeserializeObject(result);
        //    UpdateData(JsonConvert.DeserializeObject<GridDataItem>(result));
        //}
        if (!long.TryParse(args[1], out id))
            return;
        if (args[0]=="load")
        {
            Session.Remove("sessionKey");
            _testTable = new DataTable();
            _testTable = GetTable();
        }

        g.DataBind();
    }
    public void UpdateData(GridDataItem model)
    {
        DataRow dr = _testTable.Rows.Find(model.ID);
        var values = new[] { "Result" };
        foreach (DataColumn column in _testTable.Columns)
        {
            if (values.Any(column.ColumnName.Contains))
                dr[column.ColumnName] = model.Result;
        }
    }
    protected void lvwd_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        var values = new[] { "Result" };

        DataRow dr = _testTable.Rows.Find(e.Keys[0]);
        foreach (DataColumn column in _testTable.Columns)
        {
            if (values.Any(column.ColumnName.Contains))
                dr[column.ColumnName] = e.NewValues[column.ColumnName];
        }
        g.CancelEdit();
        e.Cancel = true;
    }

    protected void ASPxCallback_Callback(object source, CallbackEventArgs e)
    {
        try
        {
            UpdateData(JsonConvert.DeserializeObject<GridDataItem>(e.Parameter));
            e.Result = "OK";
        }
        catch (Exception ex)
        {
            e.Result = ex.Message;
        }
    }

    protected void lvwd_DataBound(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        DataTable table = (DataTable)g.DataSource;
        if (table != null){
            foreach (DataRow row in table.Rows)
            {
                String myVar = "";int myNum = 0;
                string c = string.Format("{0}", row["Name"]);
                if (c == "TVC" && row["Result"].ToString() != "")
                {
                    myVar = row["Result"].ToString();
                    if (myVar == "<1")
                        myVar = "1";
                    if (Int32.TryParse(myVar, out myNum)){
                        int mark = Convert.ToInt32(myVar);
                        row["StatusApp"] = string.Format("{0}", myclass.LetterGradeFromNumber(mark));
                    }
                }
                else if (c == "Samonella" && row["Result"].ToString() != "")
                {
                    myVar = row["Result"].ToString();
                    if (myVar == "not detect" || myVar == "ND")
                        row["StatusApp"] = "Pass";
                    if (myVar == "-")
                        row["StatusApp"] = "NT";
                }
                else if (c == "Coliform" || c == "E.Coli")
                {
                    myVar = row["Result"].ToString();
                    if (Int32.TryParse(myVar, out myNum)){
                        if (myNum <= 1.1)
                            row["StatusApp"] = "Pass";
                        else if (myNum > 1.1)
                            row["StatusApp"] = "NT";
                    }
                    else if (myVar == "not detect" || myVar == "NT" || myVar == "<1.1")
                            row["StatusApp"] = "Pass";
                        else if (myVar == "-" || myVar == ">1.1")
                            row["StatusApp"] = "NT";
                }else if (row["Result"].ToString() == "")
                    row["StatusApp"] = "";
            }
        }
    }
    protected void CmbPosition_Callback(object sender, CallbackEventArgsBase e)
    {
        ASPxComboBox comb = sender as ASPxComboBox;
        comb.DataBind();
    }

    protected void CmbRecorder_Callback(object sender, CallbackEventArgsBase e)
    {
        ASPxComboBox comb = sender as ASPxComboBox;
        comb.DataBind();
    }

    protected void fileManager_CustomCallback(object sender, CallbackEventArgsBase e)
    {
        ASPxFileManager fm = (ASPxFileManager)sender;
        string[] param = e.Parameter.Split('|'); //bool selected = true;
        //int id;
        //if (!int.TryParse(param[1], out id))
        //    return;
        switch (param[0])
        {
            case "load":
                //ArtsDataSource.SelectParameters.Clear();
                //.SelectParameters.Add("GCRecord", string.Format("{0}", hfgetvalue["NewID"]));
                //ArtsDataSource.SelectParameters.Add("username", hfuser["user_name"].ToString());
                this.Session["GeID"] = string.Format("{0}", hGeID["GeID"]);
                break;
        }
        fm.DataBind();
    }
    protected void fileManager_FileUploading(object source, FileManagerFileUploadEventArgs e)
    {
        var leaveNo = string.Format("{0}", hGeID["GeID"]); 
        var newleavepath = @"~/Content/UploadControl/" + leaveNo+ @"/";
        if (!Directory.Exists(Server.MapPath(newleavepath)))
        {
            Directory.CreateDirectory(Server.MapPath(newleavepath));
        }
        string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), e.File.Name);
        var newDocPath = Server.MapPath(newleavepath) + resultFileName;
    
            FileStream fs = new FileStream(newDocPath, FileMode.CreateNew);
            e.InputStream.CopyTo(fs);
            fs.Close(); //close the new file created by the FileStream
        byte[] file;
        String name = e.FileName;
        string mimeType = MimeTypes.GetContentType(name);
        //String path = MapPath(newleavepath);
        using (var stream = new FileStream(newDocPath, FileMode.Open, FileAccess.Read)) { using (var read = new BinaryReader(stream)) { file = read.ReadBytes((int)stream.Length); } }
        Objattachment ro = new Objattachment();
        ro.Name = name;
        ro.ContentType = mimeType;
        ro.Data = file;
        ro.MatDoc = Convert.ToInt32(leaveNo);
        ro.ActiveBy = user_name;
        myclass.savefilebyte(ro);
        e.Cancel = true; //cancelling the upload, prevents duplicate uploads
        e.ErrorText = "Success"; //shown when the upload is cancelled
                                 //fmLeaveDocs.Refresh(); //does not work. Causes an error.
    }
    //{
    //    //string name = e.FileName;

    //    //byte[] FileData = myclass.ReadFile(resultFilePath);
    //    //FileStream fs = new FileStream(MapPath(RequiredPath), FileMode.CreateNew);
    //    //e.InputStream.CopyTo(fs);
    //    //fs.Close();


    //    e.Cancel = true;
    //}
}
public class GridDataItem
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Result { get; set; }
    public string StatusApp { get; set; }
}