using ClosedXML.Excel;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_EditForms_countEditForm : System.Web.UI.UserControl
{
    MyDataModule cs = new MyDataModule();
    ServiceCS myclass = new ServiceCS();
    string user_name = HttpContext.Current.User.Identity.Name.Replace(@"THAIUNION\", @"");
    //String user_name = HttpContext.Current.Request.LogonUserIdentity.Name.Replace(@"THAIUNION\", @"");
    //string strConn = ConfigurationManager.ConnectionStrings["ncpDbConnectionString"].ConnectionString;
    private DataTable testTable
    {
        get { return Session["sessionKey"] == null ? null : (DataTable)Session["sessionKey"]; }
        set { Session["sessionKey"] = value; }
    }
    string fExpr
    {
        get { return Session["fExpr"] == null ? "Z" : Session["fExpr"].ToString(); }
        set { Session["fExpr"] = value; }
    }
    //string selectedDataSource
    //{
    //    get { return Session["selectedDataSource"] == null ? String.Empty : Session["selectedDataSource"].ToString(); }
    //    set { Session["selectedDataSource"] = value; }
    //}
    const string UploadDirectory = "~/Content/";
    string _fileForAttachment = "";
    List<string> _listOfFilesForAttachment = new List<string>();
    private void SaveFileToDB(string file, int Keys)
    {
        Objattachment or = new Objattachment();
        string filePath = string.Format("{0}{1}", MapPath("~/Captures/"), file);
        //or.ActiveBy = user_name.ToString();
        string filename = Path.GetFileName(filePath);
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        Byte[] bytes = br.ReadBytes((Int32)fs.Length);
        //or.Data = myclass.ReadFile(fileName);
        //or.Data = bytes;
        //or.Name = file;
        //or.MatDoc = Keys;
        //or.ContentType = "image/png";
        //myclass.savefilebyte(or);
        //Save to SQL server code
        var MyArray = ASPxHiddenField1.Get("MyArray");
        string strQuery = "insert into FileSystem values (@Name,@ContentType,@Data,@MatDoc,@ActiveBy,@SystemDate)";
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = filename;
        cmd.Parameters.Add("@ContentType", SqlDbType.VarChar).Value = "image/png";
        cmd.Parameters.Add("@Data", SqlDbType.Binary).Value = MyArray;
        cmd.Parameters.Add("@MatDoc", SqlDbType.VarChar).Value = Keys;
        cmd.Parameters.Add("@ActiveBy", SqlDbType.VarChar).Value = user_name.ToString(); 
        cmd.Parameters.Add("@SystemDate", SqlDbType.VarChar).Value = (object)DateTime.Now;
        InsertUpdateData(cmd);
    }
    protected void cp_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        LoadUserControls();
    }
    protected void LoadUserControls()
    {
        try
        {
            MasterContainer.Controls.Clear();
            Control myControl = LoadControl("~/src/WebUserControl.ascx");

            myControl.ID = "ucx_mapa";
            cp.Controls.Clear();
            cp.Controls.Add(myControl);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private Boolean InsertUpdateData(SqlCommand cmd)
    {
        SqlConnection con = new SqlConnection(cs.strConn);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;
        try
        {
            con.Open();
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            return false;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }
    private static byte[] ResizeImageFile(byte[] imageFile, int targetSize)
    {
        using (System.Drawing.Image oldImage = System.Drawing.Image.FromStream(new MemoryStream(imageFile)))
        {
            Size newSize = CalculateDimensions(oldImage.Size, targetSize);
            using (Bitmap newImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format24bppRgb))
            {
                using (Graphics canvas = Graphics.FromImage(newImage))
                {
                    canvas.SmoothingMode = SmoothingMode.AntiAlias;
                    canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), newSize));
                    MemoryStream m = new MemoryStream();
                    newImage.Save(m, ImageFormat.Jpeg);
                    return m.GetBuffer();
                }
            }
        }
    }
    private static Size CalculateDimensions(Size oldSize, int targetSize)
    {
        Size newSize = new Size();
        if (oldSize.Height > oldSize.Width)
        {
            newSize.Width = (int)(oldSize.Width * ((float)targetSize / (float)oldSize.Height));
            newSize.Height = targetSize;
        }
        else
        {
            newSize.Width = targetSize;
            newSize.Height = (int)(oldSize.Height * ((float)targetSize / (float)oldSize.Width));
        }
        return newSize;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session.Clear();
            //hGeID["GeID"] = string.Format("{0}", '0');
            var strNames = new List<string>();
            husername["user_name"] = user_name;
            //SqlParameter[] p = { new SqlParameter("@table", string.Format("ulogin where user_name = '{0}'", user_name))};
            //var result = cs.GetRelatedResources("usp_query", p);
            //foreach (DataRow dr in result.Rows) { 
            //    if (!string.IsNullOrEmpty(dr["ApprovedStep"].ToString())) { 
            //    strNames.Add(dr["ApprovedStep"].ToString());
            //        }
            //    }
            //ASPxHiddenField1.Get("MyArray");
            filter.FilterExpression = "[CreateOn] Between(#2019-09-01#, #2019-09-07#)";
            hKeyword["Expr"] = string.Format("{0}",fExpr);
        }
        //CreateGrid();
    }
    private void LoadFilterControl()
    {

    }
    //private void CreateGrid()
    //{
    //    ASPxGridView grid = new ASPxGridView();
    //    grid.ID = "grid";
    //    grid.Settings.ShowColumnHeaders = false;
    //    grid.EnableViewState = false;
    //    grid.DataBinding += grid_DataBinding;
    //    grid.RowUpdating += grid_RowUpdating;
    //    grid.CustomCallback += grid_CustomCallback;
    //    grid.DataBound += grid_DataBound;
    //    phGrid.Controls.Add(grid);
    //    grid.Columns.Clear();
    //    grid.AutoGenerateColumns = true;
    //    grid.KeyFieldName = "ID";
    //    grid.ClientInstanceName = "grid";
    //    grid.DataBind();
    //    AddColumns(grid);
    //}
    protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //object o = selectedDataSource;
        //if (o == null) return;
        if (string.IsNullOrEmpty(e.Parameters))
            return;
        var args = e.Parameters.Split('|');
        if (args[0]=="clear")
        {
            Session.Remove("sessionKey");
            g.DataSourceID = null;
            //g.DataBind();
        }
        if (args[0] == "reload")
        {
            dsgv.SelectParameters.Clear();
            var id = hGeID["GeID"];
            dsgv.SelectParameters.Add("Id", string.Format("{0}", string.Format("{0}", id)));
            dsgv.SelectParameters.Add("plant", string.Format("{0}", args[1]));
            dsgv.SelectParameters.Add("group", string.Format("{0}", args[2]));
            dsgv.DataBind();
            DataView dv = (DataView)dsgv.Select(DataSourceSelectArguments.Empty);
            if (dv != null){
                testTable = dv.Table;
                testTable.PrimaryKey = new DataColumn[] { testTable.Columns["ID"] };
            }
        }
        if (args[0]=="scan")
        {

        }
        //g.CancelEdit();
        //g.DataSource = null;
        //g.Columns.Clear();
        //g.KeyFieldName = "ID";
        g.CancelEdit();
        g.AutoGenerateColumns = false;
        g.DataBind();
        //AddColumns(g);
    }
	void Delete(string keys){
        using (SqlConnection con = new SqlConnection(cs.strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spDelPestControl";
            cmd.Parameters.AddWithValue("@ID", keys.ToString());
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            foreach (DataRow dr in testTable.Rows){
                dr.Delete();
            }
        }
	}
    protected void grid_DataBinding(object sender, EventArgs e)
    {
        //(sender as ASPxGridView).DataSource = GetDataSource();
        ASPxGridView g = sender as ASPxGridView;
        g.DataSource = LoadGrid;
        g.ForceDataRowType(typeof(DataRow));
    }
    private DataTable LoadGrid
    {
        get
        {
            if (testTable == null)
                return testTable;
            //DataTable dt = ((DataView)GetDataSource().Select(DataSourceSelectArguments.Empty)).Table; 
            var view = testTable.DefaultView;
            CreateGridColumns(view);
            return testTable;
        }
    }
    DataTable GetDataTable(string value)
    {
        return  myclass.builditems(string.Format(@"select value from dbo.FNC_SPLIT('{0}',';')", value));
    }
    void CreateGridColumns(DataView view)
    {
        grid.Columns.Clear();
        foreach (DataColumn c in view.Table.Columns)
        {
            var str = c.ColumnName;
            var args = str.Split('|');
            //GridViewDataColumn col = new GridViewDataTextColumn();
            if (c.ColumnName.Contains("RadioButton"))
            {
                GridViewDataColumn dc = new GridViewDataColumn();
                dc.Width = Unit.Percentage(90);
                dc.FieldName = c.ColumnName;
                dc.Caption = args[0];
                //DataTable dtDados = new DataTable();
                //dtDados.Columns.Add("chave");
                //dtDados.Columns.Add("valor");
                //for (int i = 0; i < 2; i++)
                //    dtDados.Rows.Add(i, "Valor " + i);
                //dc.EditItemTemplate = new MyRadioButtonList(dtDados);//
                dc.EditItemTemplate = new MyRadioButtonList(c.ColumnName);
                grid.Columns.Add(dc);
            }else if (c.ColumnName.Contains("Combobox"))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = c.ColumnName;
                cb.Caption = args[0];
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.TextField = "value";
                cb.PropertiesComboBox.ValueField = "value";
                cb.PropertiesComboBox.TextFormatString = "{0}";
                cb.PropertiesComboBox.DataSource = GetDataTable(args[2]);
                grid.Columns.Add(cb);
            }
            else if (c.ColumnName.Contains("CheckBox"))
            {
                GridViewDataCheckColumn cc = new GridViewDataCheckColumn();
                cc.FieldName = c.ColumnName;
                cc.Caption = args[0];
                grid.Columns.Add(cc);
            }
            else if (c.ColumnName.Contains("Number"))
            {
                GridViewDataSpinEditColumn se = new GridViewDataSpinEditColumn();
                se.FieldName = c.ColumnName;
                se.Caption = args[0];
                grid.Columns.Add(se);
            }
            else if (c.ColumnName.Contains("Name"))
            {
                GridViewDataTextColumn tb = new GridViewDataTextColumn();
                tb.FieldName = c.ColumnName;
                tb.EditFormSettings.Visible= DevExpress.Utils.DefaultBoolean.False;
                grid.Columns.Add(tb);
            }
            else
                AddTextColumn(c.ColumnName, grid);
        }
        //grid.KeyFieldName = "ID";
        int x = (Request.Browser.ScreenPixelsWidth) * 2 - 100;
        //grid.EditFormLayoutProperties.ColumnCount = 3;
        grid.KeyFieldName = view.Table.Columns[0].ColumnName;
        grid.Columns[0].Visible = false;
        //AddCommandColumn(grid);
    }
    private void AddTextColumn(string fieldName, ASPxGridView g)
    {
        var args = fieldName.Split('|');
        GridViewDataTextColumn c = new GridViewDataTextColumn();
        c.FieldName = fieldName;
        c.Caption = args[0];
        g.Columns.Add(c);
    }
    private void AddCommandColumn(ASPxGridView g)
    {
            GridViewCommandColumn c = new GridViewCommandColumn();
            g.Columns.Add(c);
            c.Width = 50;
            c.VisibleIndex = 0;
            c.ShowEditButton = true;
            c.ShowCancelButton = true;
            c.ShowUpdateButton = true;
    }
    //private SqlDataSource GetDataSource()
    //{
    //    //object o = selectedDataSource;
    //    //if (o == null)
    //    //dsgv.DataBind();
    //        return dsResult;
    //}
    protected void CmbArea_Callback(object sender, CallbackEventArgsBase e)
    {
        ASPxComboBox comb = sender as ASPxComboBox;
        comb.SelectedIndex = -1;
        if (string.IsNullOrEmpty(e.Parameter)) return;
        string[] param = e.Parameter.Split('|');
        if (param[0] == "Build" || param[0] == "reload")
        {
            DataTable dt = new DataTable();
            SqlParameter[] p = { new SqlParameter("@param", string.Format("{0}", param[1])),
            new SqlParameter("@plant", string.Format("{0}", param[2]))};
            dt = cs.GetRelatedResources("spGetArea", p);
            comb.DataSource = dt;
            comb.DataBind();
            if (param[3].ToString() != "0")
                //comb.Value = param[3];
            //else
               if (dt.Rows.Count > 0){
                    //string value = cs.ReadItems(
                       //string.Format(@"select Id from Mas_InstallArea where shortname='{0}'", param[3]));
                    comb.SelectedIndex = comb.Items.IndexOf(comb.Items.FindByValue(param[3]));
                }
                
        }
        if (param[0] == "Value")
        {
            comb.Value = param[1];
            CmbGrouping.SelectedIndex = 1;
        }
    }
    void buildparam(string e)
    {
        string[] param = e.Split('|');
    }
    //protected void btnListplant_DataBound(object sender, EventArgs e)
    //{
    //    ASPxRadioButtonList rbl = sender as ASPxRadioButtonList;
    //    rbl.SelectedIndex = 0;
    //}

    protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        var str = e.Column.FieldName;
        var args = str.Split('|');
            if (e.Editor is ASPxTextBox || e.Editor is ASPxComboBox || e.Editor is ASPxSpinEdit)
            e.Editor.Width = Unit.Pixel(230);
    }

    protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        DataTable table = (DataTable)g.DataSource;//LoadGrid;
        //GridViewRow row = g.Rows[e.RowIndex];
        var values = new[] { "ID", "Name" }; bool first = true;string myresult = "";
        foreach (DataColumn column in table.Columns)
        {
            string name = column.ToString();
            if (!values.Any(name.Contains))
            {
                var args = name.Split('|');
                //if (e.NewValues.Contains(name)) { }
                String CID = (String)e.NewValues[name]; string strvalue = "";
                if (!string.IsNullOrEmpty(CID))
                    strvalue = e.NewValues[name].ToString();
                else if (name.Contains("RadioButton"))
                {
                    GridViewDataColumn columnRftContent = g.Columns[args[0]] as GridViewDataColumn;
                    ASPxRadioButtonList objList = g.FindEditRowCellTemplateControl(columnRftContent, args[0] + g.VisibleStartIndex) as ASPxRadioButtonList;
                    //e.NewValues["RtfContent"] = objList.Value;
                    strvalue = objList.Value.ToString();
                    //    //test = Hf.Get("Value").ToString();
                    //    var param = name.Split('|');
                    //    GridViewDataColumn col1 = g.Columns[param[0]] as GridViewDataColumn;
                    //    ASPxRadioButtonList objList = (ASPxRadioButtonList)g.FindEditFormTemplateControl("ovRBL_Data_" + g.VisibleStartIndex);
                    //    test = objList.SelectedItem.Value.ToString();
                }

                using (SqlConnection con = new SqlConnection(cs.strConn))
                {
                    if (first)
                    {
                        first = false;
                        DataTable dt = new DataTable();
                        SqlParameter[] p = { new SqlParameter("@Id", string.Format("{0}", hGeID["GeID"])),
                             new SqlParameter("@user", string.Format("{0}", user_name)),
                             //new SqlParameter("@IssueDate", string.Format("{0}", deValidfrom.Value)),
                             new SqlParameter("@IssueDate",String.Format("{0:MM/dd/yyyy}", deValidfrom.Value)),
                             new SqlParameter("@Plant", string.Format("{0}", CmbPlant.Value)),
                             new SqlParameter("@Grouping", string.Format("{0}", CmbGrouping.Value)),
                             new SqlParameter("@InstallArea", string.Format("{0}", CmbArea.Value))};
                        dt = cs.GetRelatedResources("spinsertcontrol", p);
                        foreach (DataRow dr in dt.Rows)
                            myresult = string.Format("{0}", dr["ID"]);
                    }
                    using (SqlCommand cmd = new SqlCommand("spinsertcontroldetail"))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", string.Format("{0}", myresult));
                        cmd.Parameters.AddWithValue("@Param", args[0].ToString());
                        cmd.Parameters.AddWithValue("@Result", strvalue.ToString());
                        cmd.Parameters.AddWithValue("@useType", args[1].ToString());
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                }
            }
        }
        foreach (string fileName in tokenBox.Tokens)
        {
            if (!string.IsNullOrEmpty(myresult))
            SaveFileToDB(fileName, Convert.ToInt32(myresult));
        }
        g.JSProperties["cpKeyValue"] = myresult;
        e.Cancel = true;
        g.CancelEdit();
        testgrid.DataBind();
        Session.Remove("sessionKey");
    }

    protected void grid_DataBound(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //g.CancelEdit();
        if (g.VisibleRowCount > 0){
            DataTable table = (DataTable)g.DataSource;
            if (table != null)
                if (table.Rows.Count > 0) {
                    g.StartEdit(0);
                }
        }           
        //g.StartEdit(g.VisibleStartIndex);
    }

    protected void testgrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        if (string.IsNullOrEmpty(e.Parameters)) return;
        string[] args = e.Parameters.Split('|');
        if (args[0] == "Delete" && args.Length > 1) { 
            Delete(args[1]);
        }
        if (args[0] == "ExportToXLS")
            GridExporter.WriteXlsToResponse();

        if (args[0] == "filter")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(filter.GetFilterExpressionForMsSql());
            fExpr = sb.ToString();
            g.JSProperties["cpKeyValue"] = "filter|"+ fExpr;
        }
        if (args[0] == "Approve" || args[0] == "Reject")
        {
            for (int i = 0; i < g.VisibleRowCount; i++)
            {
                if (g.Selection.IsRowSelected(i)){
                    var id = g.GetRowValues(i, g.KeyFieldName).ToString();
                    string status = testgrid.GetRowValues(i, new string[] { "StatusApp" }).ToString();
                    SqlParameter[] p = { new SqlParameter("@ID", id.ToString()) ,
                        new SqlParameter("@statusapp",string.Format("{0}",args[0].ToString())),
                        new SqlParameter("@user",string.Format("{0}",user_name))};
                    DataTable dt = cs.GetRelatedResources("spapprovestep", p);
                }
            }
        }
        g.DataBind();
    }
    protected void testgrid_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
    {
        if (e.MenuType == GridViewContextMenuType.Rows)
        {
            e.Items.Clear();
            var item = e.CreateItem("ExportToXLS", "ExportToXLS");
            item.Image.Url = @"~/Content/Images/excel.gif";
            e.Items.Add(item);
            //item = e.CreateItem("Charts", "Charts");
            //item.Image.Url = @"~/Content/Images/colorful_chart.png";
            //e.Items.Add(item);
            item = e.CreateItem("ExportToLayout", "Export");
            item.BeginGroup = true;
            item.Image.Url = @"~/Content/Images/if_sign-out_59204.png";
            e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Custom), item);
            AddMenuSubItem(item, "PDF", "PDF", "~/Content/Images/pdf.gif", true);
            AddMenuSubItem(item, "XLS", "XLS", "~/Content/Images/excel.gif", true);
        }
    }
    private static void AddMenuSubItem(GridViewContextMenuItem parentItem, string text, string name, string iconID, bool isPostBack)
    {
        var exportToXlsItem = parentItem.Items.Add(text, name);
        exportToXlsItem.Image.Url = iconID;
    }
    protected void cb_Callback(object source, CallbackEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Parameter))
            return;
        var args = e.Parameter.Split('|');
        if (args[0] == "rebind")
        {
            SqlParameter[] p = { new SqlParameter("@Param", args[1].ToString()) };
            DataTable dttab = cs.GetRelatedResources("spGetData", p);
            foreach (DataRow row in dttab.Rows)
                e.Result =  row["data"].ToString();
                //grid.DataSource = null;
                //grid.DataBind();
            }
        if (args[0] == "read")
        {
            SqlParameter[] p = { new SqlParameter("@ID", args[1].ToString()) };
            DataTable dt = cs.GetRelatedResources("spSelectData", p);
            //spSelectData
        }
    }
    protected void grid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
    {
        //if (e.VisibleIndex == -1) return;
        switch (e.ButtonType){
            case ColumnCommandButtonType.Update:
                e.Visible = false;
                break;
            case ColumnCommandButtonType.Cancel:
                e.Visible = false;
                break;
        }
    }
    protected void grid_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
    {

    }
    protected void PreRender(object sender, EventArgs e)
    {
        SetItemCount();
    }
    protected void BeforeGetCallbackResult(object sender, EventArgs e)
    {
        SetItemCount();
    }
    void SetItemCount()
    {
        int itemCount = (int)testgrid.GetTotalSummaryValue(testgrid.TotalSummary["IssueDate"]);
        testgrid.SettingsPager.Summary.Text = "Page {0} of {1} (" + itemCount.ToString() + " items)";
    }
    //object GetDataSource(int count)
    //{
    //    List<object> result = new List<object>();
    //    for (int i = 0; i < count; i++)
    //        result.Add(new { ID = i, City = "City_" + i });
    //    return result;
    //}
    protected void testgrid_CustomDataCallback(object sender, ASPxGridViewCustomDataCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        var args = e.Parameters.Split('|');
        long id;
        if (!long.TryParse(args[2], out id))
            return;
        var result = new Dictionary<string, string>();
        if (args[1] == "EditDraft" || args[1] == "save")
        {
            SqlParameter[] param = {new SqlParameter("@Id",id.ToString()),
                        new SqlParameter("@user",user_name.ToString())};
            DataTable dt = cs.GetRelatedResources("spGetpestDetail", param);
            foreach(DataRow rw in dt.Rows){
                result["IssueDate"] = string.Format("{0}", rw["IssueDate"]);
                result["Plant"] = string.Format("{0}", rw["Plant"]);
                result["Grouping"] = string.Format("{0}", rw["Grouping"]);
                result["InstallArea"] = string.Format("{0}", rw["InstallArea"]);

                e.Result = result;
            }
        }
    }
    protected void grid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //g.CancelEdit();
        g.JSProperties["cpIsEdit"] = false;
    }

    protected void testgrid_ContextMenuItemClick(object sender, ASPxGridViewContextMenuItemClickEventArgs e)
    {
        switch (e.Item.Name)
        {
            case "ExportToXLS":
                GridExporter.WriteXlsToResponse();
                break;
            case "XLS":
                ExportToDel(e.Item.Name);
                break;
        }
    }
    public void ExportToDel(string Data) {
        string fileName = Server.MapPath("~/App_Data/Documents/FAQCSN11_7.xlsx");
        string _name = @"FAQCSN11_7_" + DateTime.Now.ToString("yyyyMMddHHmmss");
        using (var excelWorkbook = new XLWorkbook(fileName))
        {
            var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed();
            var ws = excelWorkbook.Worksheet(1);
            //DataTable dt = (DataTable)testgrid.DataSource;
            int row = 0;
            //foreach (GridViewColumn col in testgrid.VisibleColumns)
            //{
            //    GridViewDataColumn dataColumn = col as GridViewDataColumn;
            //    ws.Cell("C"+ i).Value = string.Format("{0}", dataColumn.FieldName);
            //    i += 1;
            //}
            string key = "";
            for (int i = 0; i < testgrid.VisibleRowCount; i++){
                if (key != testgrid.GetRowValues(i, "ID").ToString()){
                    int _row = 11 + row;
                    key = testgrid.GetRowValues(i, "ID").ToString();
                   // ws.Cell("B" + _row).Value = string.Format("{0}", key);
                    ws.Cell("C" + _row).Value = string.Format("{0}", testgrid.GetRowValues(i, "Name"));
                    row += 1;
                }
            }
            excelWorkbook.SaveAs(Server.MapPath("~/ExcelFiles/" + _name + ".xlsx"));
            //excelWorkbook.SaveAs(@"C:\temp\HelloWorld.xlsx");
        }
        string path = Server.MapPath("~/ExcelFiles/" + _name + ".xlsx");
        System.IO.FileInfo file = new System.IO.FileInfo(path);
        string Outgoingfile = _name+".xlsx"; //"FileName.xlsx";
        Context.Response.Write("<script>window.open('./ExportTo.aspx?Id="+ _name +"', '_blank');</script>");
        //if (file.Exists)
        //{
        //    Response.Clear();
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + Outgoingfile);
        //    Response.AddHeader("Content-Length", file.Length.ToString());
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.WriteFile(file.FullName);
        //    Response.Flush();
        //    Response.Close();

        //}
        //else
        //{
        //    Response.Write("This file does not exist.");
        //}
        //Context.Response.Write(Data);
    }

    protected void grid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        e.Cancel = true;
        g.CancelEdit();
        Session.Remove("sessionKey");
    }

    protected void testgrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        e.Properties["cpVisibleRowCount"] = g.VisibleRowCount;
        e.Properties["cpFilteredRowCountWithoutPage"] = GetFilteredRowCountWithoutPage();
    }
    protected int GetFilteredRowCountWithoutPage()
    {
        int selectedRowsOnPage = 0;
        foreach (var key in testgrid.GetCurrentPageRowValues("ID"))
        {
            if (testgrid.Selection.IsRowSelectedByKey(key))
                selectedRowsOnPage++;
        }
        return testgrid.Selection.FilteredCount - selectedRowsOnPage;
    }

    protected void testgrid_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //bool isOddRow = e.VisibleIndex % 2 == 0;
        //string Result = string.Format("{0}", g.GetRowValues(e.VisibleIndex, "StatusApp"));
        //bool isOddRow = string.Format("{0}", Result) != "0" && !string.IsNullOrEmpty(Result);
        //if (e.ButtonID == "EditCost" && isOddRow)
        //{

        //}
    }

    //protected void fileManager_CustomCallback(object sender, CallbackEventArgsBase e)
    //{
    //    fileManager.SettingsPermissions.Role = "Admin";
    //}


    protected void CmbPlant_Callback(object sender, CallbackEventArgsBase e)
    {

    }

    protected void testgrid_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
    {
        //ASPxGridView g = (sender as ASPxGridView);
        //if (e.CommandCellType == GridViewTableCommandCellType.Data)
        //{
        //    if (!System.Convert.IsDBNull(g.GetRowValuesByKeyValue(e.KeyValue, "StatusApp")) && g.GetRowValuesByKeyValue(e.KeyValue, "StatusApp").ToString() == "APPR")
        //    {
        //        ((WebControl)e.Cell.Controls[0]).Enabled = false;

        //    }
        //}
    }

    protected void testgrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
    {
        string[] strNamesArray = { "0", "vernie", "joel" };
        ASPxGridView g = (sender as ASPxGridView);
        if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox) { 
            string strStatus = g.GetRowValues(e.VisibleIndex, "StatusApp").ToString();
            if (!System.Convert.IsDBNull(g.GetRowValues(e.VisibleIndex, "StatusApp")) && g.GetRowValues(e.VisibleIndex, "StatusApp").ToString() == "0")
            {
                if (strNamesArray.Any(x => x == strStatus))
                    e.Enabled = false;
            }
        }
    }
    protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
        string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), resultExtension);
        string resultFileUrl = UploadDirectory + resultFileName;
        string resultFilePath = MapPath(resultFileUrl);
        //Save the uploaded file
        e.UploadedFile.SaveAs(resultFilePath);
        string name = e.UploadedFile.FileName;
        string url = ResolveClientUrl(resultFileUrl);
        long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
        string sizeText = sizeInKilobytes.ToString() + " KB";
        e.CallbackData = name + "|" + url + "|" + sizeText;
    }
    protected void UploadControl_FilesUploadComplete(object sender, FilesUploadCompleteEventArgs e)
    {

    }
    protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType == GridViewRowType.EditForm)
        {
            ASPxRadioButtonList ovRBL_Data = (ASPxRadioButtonList)grid.FindRowCellTemplateControl(e.VisibleIndex, null, "ovRBL_Data_" + e.VisibleIndex);
            if (ovRBL_Data != null)
            {
                object valor = e.GetValue(ovRBL_Data.Attributes["campo"].ToString());
                ovRBL_Data.SelectedIndex = -1;
                if (!String.IsNullOrEmpty(valor.ToString()))
                    ovRBL_Data.Items.FindByValue(valor.ToString()).Selected = true;
            }
        }
    }
    class MyRadioButtonList : ITemplate
    {
        //private DataTable dtDados;
        string name;
        public MyRadioButtonList(string name)
        {
            //this.dtDados = dtDados;
            this.name = name;
        }
        public void InstantiateIn(Control container)
        {
            var args = this.name.Split('|');
            int index = Array.IndexOf(args[2].Split(';'), args[3]);
            index = index == -1 ? 0 : index;
            DataTable dtDados = new DataTable();
            dtDados.Columns.Add("chave");
            dtDados.Columns.Add("value");
            var arr = args[2].Split(';');
            for (int i = 0; i < arr.Length; i++){
                string s = arr[i];
                dtDados.Rows.Add(i, arr[i]);
            }
            GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
            ASPxRadioButtonList ovRBL = new ASPxRadioButtonList();
            ovRBL.ID = args[0] + gridContainer.VisibleIndex;
            container.Controls.Add(ovRBL);
            ovRBL.ClientIDMode = ClientIDMode.Static;
            ovRBL.Width = Unit.Percentage(100);
            ovRBL.Border.BorderStyle = BorderStyle.None;
            ovRBL.Paddings.Padding = Unit.Pixel(0);
            ovRBL.RepeatColumns = dtDados.Rows.Count;
            ovRBL.ValueField = "value";
            ovRBL.TextField = "value";
            //ovRBL.ValueField = "chave";
            //ovRBL.TextField = "valor";
            //ovRBL.Attributes.Add("campo", "campo1");
            ovRBL.DataSource = dtDados;
            ovRBL.DataBind();
            ovRBL.ClientSideEvents.SelectedIndexChanged = "OnSelectedIndexChanged";
            ovRBL.ClientSideEvents.Init = "function(s,e){ s.SetSelectedIndex("+ index + "); }";
            //ovRBL.Value = "<%# Bind('"+ name +"') %>";
            ovRBL.Value = "<%# Eval('" + this.name + "')%>";
            //container.Controls.Add(ovRBL);
        }
    }
}