using DevExpress.Web;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_EditForms_IncomForm : System.Web.UI.UserControl
{
    MyDataModule cs = new MyDataModule();
    ServiceCS myclass = new ServiceCS();
    HttpContext ct = HttpContext.Current;
    string user_name = HttpContext.Current.User.Identity.Name.Replace(@"THAIUNION\", @"");
    private DataTable _dataTable
    {
        get { return ct.Session["CustomTable"] == null ? null : (DataTable)ct.Session["CustomTable"]; }
        set { ct.Session["CustomTable"] = value; }
    }
    private DataTable _TempApp
    {
        get { return ct.Session["TempApp"] == null ? null : (DataTable)ct.Session["TempApp"]; }
        set { ct.Session["TempApp"] = value; }
    }
    private DataTable _Temp
    {
        get { return ct.Session["Temp"] == null ? null : (DataTable)ct.Session["Temp"]; }
        set { ct.Session["Temp"] = value; }
    }
    private DataTable _testTable
    {
        get { return ct.Session["sessionKey"] == null ? null : (DataTable)ct.Session["sessionKey"]; }
        set { ct.Session["sessionKey"] = value; }
    }
    private DataTable TableData
    {
        get { return ct.Session["TableData"] == null ? null : (DataTable)ct.Session["TableData"]; }
        set { ct.Session["TableData"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session.Clear();
            hGeID["GeID"] = string.Format("{0}", 0);
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
            this.ct.Session["SubID"] = 0;
            DateTime today = DateTime.Today;
            DateTime b = DateTime.Now.AddDays(-30);
            filter.FilterExpression = string.Format("[CreateOn] Between(#{0}#, #{1}#)",
                b.ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd"));

            //hKeyword["Expr"] = string.Format("{0}", fExpr);
            //ASPxGridView ASPxGridView1 = new ASPxGridView();
            //ASPxGridView1.ID = "ASPxGridView1";
            //divContainer.Controls.Add(ASPxGridView1);
            //ASPxGridLookup1.DataSourceID = "AccessDataSource1";
            //ASPxGridLookup1.KeyFieldName = "ProductID";
            //ASPxGridLookup1.AutoGenerateColumns = false;
            //ASPxGridLookup1.SelectionMode = GridLookupSelectionMode.Multiple;
            //ASPxGridLookup1.TextFormatString = "{0}";
            //ASPxGridLookup1.MultiTextSeparator = ", ";
            //ASPxGridLookup1.Width = 250;
            //ASPxGridLookup1.Columns.Add(new GridViewDataTextColumn() { FieldName = "ProductName", VisibleIndex = 0 });
            //ASPxGridLookup1.Columns.Add(new GridViewDataTextColumn() { FieldName = "UnitPrice", VisibleIndex = 1 });
            //ASPxGridLookup1.GridViewProperties.Settings.ShowFilterRow = true;
        }
    }

    protected void ASPxUploadControl1_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
    {

    }

    protected void gvw_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView g = (sender) as ASPxGridView;
        g.DataSource = (DataTable)ct.Session["dt"];
    }

    protected void gvw_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        if (string.IsNullOrEmpty(e.Parameters))
            return;
        var args = e.Parameters.Split('|');
        long id;
        if (!long.TryParse(args[1], out id))
            return;
        if (args[0] == "load")
        {

            //dsGenCount.SelectParameters.Clear();
            //dsGenCount.SelectParameters.Add("strCount", string.Format("{0}", id.ToString()));
            //dsGenCount.DataBind();
            int Id = Convert.ToInt32(hGeID["GeID"]);
            SqlParameter[] param = { new SqlParameter("@strCount", args[1].ToString()),
                new SqlParameter("@SubID", ""),
                new SqlParameter("@SampId", Id.ToString())};
            DataTable dt = cs.GetRelatedResources("spGenCount", param);
            ct.Session["dt"] = dt;
            //if (result.Rows.Count > 0)
            //    foreach (DataRow dr in result.Rows) {
            //        g.JSProperties["cpResult"] = dr["TermOfPayment"].ToString();
            //        }

        }
        g.DataBind();
    }
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
            DataTable dt = cs.GetRelatedResources("spGetSample", param);
            foreach (DataRow rw in dt.Rows)
            {
                result["SampleID"] = string.Format("{0}", rw["SampleID"]);
                result["Shifts"] = string.Format("{0}", rw["Shifts"]);
                result["Material"] = string.Format("{0}", rw["Material"]);
                result["ReceivingDate"] = string.Format("{0}", rw["ReceivingDate"]);
                result["Batch"] = string.Format("{0}", rw["Batch"]);
                result["Species"] = string.Format("{0}", rw["Species"]);
                result["Style"] = string.Format("{0}", rw["Style"]);
                result["Supplier"] = string.Format("{0}", rw["Supplier"]);
                result["Vessel"] = string.Format("{0}", rw["Vessel"]);
                result["InvoiceNo"] = string.Format("{0}", rw["InvoiceNo"]);
                result["ContainerNo"] = string.Format("{0}", rw["ContainerNo"]);
                result["NetWeight"] = string.Format("{0}", rw["NetWeight"]);
                result["Packaging"] = string.Format("{0}", rw["Packaging"]);
                result["Notes"] = string.Format("{0}", rw["Notes"]);
                result["traCondition"] = string.Format("{0}", rw["traCondition"]);
                result["Thermometer"] = string.Format("{0}", rw["Thermometer"]);
                result["IceId"] = string.Format("{0}", rw["IceId"]);
                result["Sampling"] = string.Format("{0}", rw["Sampling"]);

                result["Countper"] = string.Format("{0}", rw["Countper"]);
                result["Others"] = string.Format("{0}", rw["Others"]);
                result["Odor"] = string.Format("{0}", rw["Odor"]);
                result["Texture"] = string.Format("{0}", rw["Texture"]);
                result["Formalin"] = string.Format("{0}", rw["Formalin"]);
                result["valWeight"] = string.Format("{0}", rw["valWeight"]);
                e.Result = result;
            }

        }
    }
    void Delete(string keys)
    {
        using (SqlConnection con = new SqlConnection(cs.strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spDelSample";
            cmd.Parameters.AddWithValue("@ID", keys.ToString());
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
    protected void testgrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        if (string.IsNullOrEmpty(e.Parameters))
            return;
        var args = e.Parameters.Split('|');
        long id;
        if (!long.TryParse(args[1], out id))
            return;
        var result = new Dictionary<string, string>();
        DataTable dt = new DataTable();
        if (args[0] == "Delete" && args.Length > 1)
        {
            Delete(args[1]);
        }
        if (args[0] == "SaveMail")
        {
            using (SqlConnection con = new SqlConnection(cs.strConn))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spInsertSample";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@SampleID", string.Format("{0}", txtSampleID.Text));
                cmd.Parameters.AddWithValue("@Shifts", string.Format("{0}", cbShift.Value));
                cmd.Parameters.AddWithValue("@ReceivingDate", string.Format("{0}", deAnyDate.Value));
                cmd.Parameters.AddWithValue("@Material", string.Format("{0}", CmbMaterial.Value));
                cmd.Parameters.AddWithValue("@Batch", string.Format("{0}", txtBatch.Text));
                cmd.Parameters.AddWithValue("@Species", string.Format("{0}", cmbSpecies.Text));
                cmd.Parameters.AddWithValue("@Style", string.Format("{0}", cmbStyle.Text));

                cmd.Parameters.AddWithValue("@Supplier", string.Format("{0}", cmbSupplier.Value));
                cmd.Parameters.AddWithValue("@Vessel", string.Format("{0}", txtVessel.Text));
                cmd.Parameters.AddWithValue("@InvoiceNo", string.Format("{0}", txtInvoice.Text));
                cmd.Parameters.AddWithValue("@ContainerNo", string.Format("{0}", txtContainer.Text));
                cmd.Parameters.AddWithValue("@NetWeight", string.Format("{0}", txtNetWeight.Text));
                cmd.Parameters.AddWithValue("@Packaging", string.Format("{0}", cmbPackaging.Value));
                cmd.Parameters.AddWithValue("@Notes", string.Format("{0}", mNotes.Text));
                cmd.Parameters.AddWithValue("@user", string.Format("{0}", user_name));
                cmd.Parameters.AddWithValue("@traCondition", string.Format("{0}", GetCheckedItems(cbtraCondition)));
                cmd.Parameters.AddWithValue("@Thermometer", string.Format("{0}", tbThermometer.Text));
                cmd.Parameters.AddWithValue("@IceId", string.Format("{0}", Getrb(rbIceId)));
                cmd.Parameters.AddWithValue("@Sampling", string.Format("{0}", tbSampling.Text));
                //cmd.Parameters.AddWithValue("@Countper",string.Format("{0}", txtCount.Text));
                //cmd.Parameters.AddWithValue("@Others",string.Format("{0}", mOthers.Text));

                //cmd.Parameters.AddWithValue("@Odor", string.Format("{0}", rbOdor.Value));
                //cmd.Parameters.AddWithValue("@Texture", string.Format("{0}", rbTexture.Value));
                //cmd.Parameters.AddWithValue("@Formalin", string.Format("{0}", CmbFormalin.Value));
                cmd.Connection = con;
                con.Open();
                var getValue = cmd.ExecuteScalar();
                con.Close();
                savedata(Int32.Parse(getValue.ToString()));
                //(sender as ASPxGridView).JSProperties["cpKeyValue"] = string.Format("{0}|{1}",args[0],getValue);
            }
            //Task

        }
        g.DataBind();
    }
    void savedata(int Id)
    {
        DataTable table = (DataTable)grid.DataSource;//LoadGrid;
        if (table != null)
            foreach (DataRow dr in table.Rows)
            {
                using (SqlConnection con = new SqlConnection(cs.strConn))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateTask";
                    cmd.Parameters.Add("@idx", SqlDbType.Int).Value = dr["idx"].ToString();
                    cmd.Parameters.Add("@SampId", SqlDbType.Int).Value = string.Format("{0}", Id);
                    cmd.Parameters.Add("@value", SqlDbType.NVarChar).Value = string.Format("{0}", dr["value"]);
                    byte[] dtask = new byte[0];
                    if (!Convert.IsDBNull(dr["Data"]))
                        dtask = (byte[])dr["Data"];
                    cmd.Parameters.Add("@Data", SqlDbType.VarBinary).Value = dtask;
                    cmd.Parameters.Add("@ActiveDate", SqlDbType.DateTime).Value = (DateTime)dr["ActiveDate"];
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        //spUpdateTemp
        if (_Temp != null)
            foreach (DataRow dr in _Temp.Rows)
            {
                using (SqlConnection con = new SqlConnection(cs.strConn))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spUpdateTemp";
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = dr["Id"].ToString();
                    cmd.Parameters.Add("@SampId", SqlDbType.Int).Value = string.Format("{0}", Id);
                    cmd.Parameters.Add("@valueTemp", SqlDbType.NVarChar).Value = string.Format("{0}", dr["valueTemp"]);
                    byte[] Data = new byte[0];
                    if (!Convert.IsDBNull(dr["Data"]))
                        Data = (byte[])dr["Data"];
                    cmd.Parameters.Add("@Data", SqlDbType.VarBinary).Value = Data;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        //sample
        if (_testTable != null)
            foreach (DataRow dr in _testTable.Rows)
            {
                int SampId = 0;
                using (SqlConnection con = new SqlConnection(cs.strConn))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spInsertWeight";
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = dr["Id"].ToString();
                    cmd.Parameters.Add("@SampId", SqlDbType.Int).Value = string.Format("{0}", Id);
                    cmd.Parameters.Add("@SampleNo", SqlDbType.NVarChar).Value = string.Format("{0}", dr["SampleNo"]);
                    cmd.Parameters.Add("@FrozenPacked", SqlDbType.NVarChar).Value = string.Format("{0}", dr["FrozenPacked"]);
                    cmd.Parameters.Add("@FrozenWeight", SqlDbType.NVarChar).Value = string.Format("{0}", dr["FrozenWeight"]);
                    byte[] bytes = new byte[0];
                    if (!Convert.IsDBNull(dr["Data"]))
                        bytes = (byte[])dr["Data"];
                    cmd.Parameters.Add("@Data", SqlDbType.VarBinary).Value = bytes;
                    byte[] Att = new byte[0];
                    if (!Convert.IsDBNull(dr["Attachment"]))
                        Att = (byte[])dr["Attachment"];
                    cmd.Parameters.Add("@Att", SqlDbType.VarBinary).Value = Att;
                    cmd.Parameters.Add("@NetWeight", SqlDbType.NVarChar).Value = string.Format("{0}", dr["NetWeight"]);
                    cmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = string.Format("{0}", dr["Notes"]);
                    cmd.Parameters.Add("@Counts", SqlDbType.NVarChar).Value = string.Format("{0}", dr["Counts"]);
                    cmd.Parameters.Add("@Mark", SqlDbType.NVarChar).Value = string.Format("{0}", dr["Mark"]);
                    cmd.Parameters.Add("@ForeignMatter", SqlDbType.NVarChar).Value = string.Format("{0}", dr["ForeignMatter"]);
                    cmd.Connection = con;
                    con.Open();
                    var getValue = cmd.ExecuteScalar();
                    SampId = Int32.Parse(getValue.ToString());
                    con.Close();
                }
                if (SampId.ToString() != "0")
                {
                    DataRow[] arr = TableData.Select(string.Format("SubID='{0}'", dr["Id"]));
                    foreach (DataRow r in arr)
                    {
                        using (SqlConnection con = new SqlConnection(cs.strConn))
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "spInsertDefect";
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = r["Id"].ToString();
                            cmd.Parameters.Add("@SampId", SqlDbType.Int).Value = string.Format("{0}", Id);
                            cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = string.Format("{0}", r["Title"]);
                            cmd.Parameters.Add("@value", SqlDbType.NVarChar).Value = string.Format("{0}", r["value"]);
                            cmd.Parameters.Add("@SubID", SqlDbType.NVarChar).Value = string.Format("{0}", SampId);
                            cmd.Parameters.Add("@AnalysisTypes", SqlDbType.NVarChar).Value = string.Format("{0}", r["AnalysisTypes"]);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
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
    public string GetCheckedItems(ASPxCheckBoxList cb)
    {
        string result = "";
        StringBuilder sb = new StringBuilder();
        foreach (ListEditItem item in cb.Items)
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
    protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        DataTable dt = (DataTable)g.DataSource;//LoadGrid;
        //Byte[] image = e.NewValues["Picture"] as byte[];
        if (dt != null)
        {
            DataRow dr = dt.Rows.Find(e.Keys[0]);
            foreach (DataColumn column in _dataTable.Columns)
            {
                if (!column.ColumnName.Contains("idx"))
                {
                    dr[column.ColumnName] = e.NewValues[column.ColumnName];
                }
            }
            //dr["Data"] = e.NewValues["Data"];
        }
        g.CancelEdit();
        e.Cancel = true;
    }
    protected void grid_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //(sender as ASPxGridView).DataSource = LoadGrid;
        g.KeyFieldName = "idx";
        g.DataSource = _dataTable;

        g.ForceDataRowType(typeof(DataRow));
    }
    DataTable BuildTable(string Id)
    {
        if (_dataTable != null)
            return _dataTable;
        _dataTable = new DataTable();
        SqlParameter[] param = { new SqlParameter("@Id", Id.ToString()) };
        _dataTable = cs.GetRelatedResources("spGetTask", param);
        //_dataTable = ((DataView)dsgv.Select(DataSourceSelectArguments.Empty)).Table;
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = _dataTable.Columns["idx"];
        _dataTable.PrimaryKey = keyColumns;
        return _dataTable;
    }

    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        string[] param = e.Parameters.Split('|'); //bool selected = true;
        int id;
        if (!int.TryParse(param[1], out id))
            return;
        switch (param[0])
        {
            case "load":
                Session.Remove("CustomTable");
                _dataTable = BuildTable(string.Format("{0}", id));
                break;
        }
        g.DataBind();
    }

    protected void gvTemp_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        g.KeyFieldName = "Id";
        g.DataSource = _Temp;
        g.ForceDataRowType(typeof(DataRow));
    }

    protected void gvTemp_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        string[] param = e.Parameters.Split('|'); //bool selected = true;
        int id;
        if (!int.TryParse(param[1], out id))
            return;
        switch (param[0])
        {
            case "load":
                Session.Remove("Temp");
                _Temp = BuildTemp(string.Format("{0}", id));
                break;
        }
        g.DataBind();
    }
    DataTable BuildTemp(string Id)
    {
        if (_Temp != null)
            return _Temp;
        _Temp = new DataTable();
        SqlParameter[] param = { new SqlParameter("@Id", Id.ToString()) };
        _Temp = cs.GetRelatedResources("spGetTemp", param);
        //_dataTable = ((DataView)dsgv.Select(DataSourceSelectArguments.Empty)).Table;
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = _Temp.Columns["Id"];
        _Temp.PrimaryKey = keyColumns;
        return _Temp;
    }
    private string GetFieldsCollectionDx()
    {
        string fieldCollection = string.Empty;
        List<object> fieldvalues = grid.GetSelectedFieldValues(new string[] { "Id" });
        int count = grid.Selection.Count;
        return fieldCollection;
    }
    protected void gvTemp_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        //DataRow dr = _Temp.NewRow();
        //ds = (DataSet)ct.Session["DataSet"];
        ASPxGridView gridView = (ASPxGridView)sender;
        //DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
        DataRow row = _Temp.NewRow();
        e.NewValues["Id"] = GetNewId(_Temp);
        e.NewValues["StatusTemp"] = "Add";
        IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
            if (enumerator.Key.ToString() != "Count")
                row[enumerator.Key.ToString()] = enumerator.Value;
        gridView.CancelEdit();
        e.Cancel = true;
        _Temp.Rows.Add(row);
    }
    private int GetNewId(DataTable dt)
    {
        //ds = (DataSet)ct.Session["DataSet"];
        //DataTable table = ds.Tables[0];
        if (dt.Rows.Count == 0) return 0;
        int max = Convert.ToInt32(dt.Rows[0]["Id"]);
        for (int i = 1; i < dt.Rows.Count; i++)
        {
            if (Convert.ToInt32(dt.Rows[i]["Id"]) > max)
                max = Convert.ToInt32(dt.Rows[i]["Id"]);
        }
        return max + 1;
    }
    protected void gvTemp_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //DataTable dt = (DataTable)g.DataSource;//LoadGrid;
        //Byte[] image = e.NewValues["Picture"] as byte[];
        var values = new[] { "Id", "ActiveDate" };
        if (_Temp != null)
        {
            DataRow dr = _Temp.Rows.Find(e.Keys[0]);
            foreach (DataColumn column in _Temp.Columns)
            {
                if (!values.Any(column.ColumnName.Contains))
                {
                    dr[column.ColumnName] = e.NewValues[column.ColumnName];
                }
            }
            //dr["Data"] = e.NewValues["Data"];
        }
        g.CancelEdit();
        e.Cancel = true;
    }
    protected void gvTemp_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        ASPxGridView g = (ASPxGridView)sender;
        if (e.ButtonID != "EditCost") return;
        object keyValue = g.GetRowValues(e.VisibleIndex, g.KeyFieldName);
        DataRow found = _Temp.Rows.Find(keyValue);
        found["StatusTemp"] = "Del";
        _Temp.AcceptChanges();
        g.DataBind();
    }

    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        //var values = new[] { "Component", "SubType", "Description", "SAPMaterial" };
        //if (values.Any(e.DataColumn.FieldName.Contains))
        //    e.Cell.ForeColor = Color.Black;
        //if (e.DataColumn.FieldName == "aValidate")
        //    IsValidate= string.Format("{0}",e.CellValue);
        if (e.DataColumn.FieldName == "value")
        {
            var g = sender as ASPxGridView;
            DataRow row = g.GetDataRow(e.VisibleIndex);
            int index = e.VisibleIndex;
            if (g.VisibleRowCount != 0 && row != null)
            {
                byte[] data = g.GetRowValues(e.VisibleIndex, "Data") as byte[];
                int r = string.Format("{0}", data).Length;
                //if (string.Format("{0}",FieldBytes).Length>14)
                if (data != null && data.Length > 0)
                    //    e.Cell.BackColor = Color.Orange;
                    //if (FieldBytes.Length > 0)
                    e.Cell.BackColor = Color.LightGreen;
                else
                    e.Cell.BackColor = Color.Coral;
            }
        }
    }

    protected void gv3_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        g.DataSource = LoadGrid;
        g.ForceDataRowType(typeof(DataRow));
    }
    private DataTable LoadGrid
    {
        get
        {
            if (_testTable != null)
                return _testTable;
            int Id = Convert.ToInt32(hGeID["GeID"]);
            _testTable = new DataTable();
            SqlParameter[] param = { new SqlParameter("@Id", Id) };
            _testTable = cs.GetRelatedResources("spGetWeight", param);
            //_dataTable = ((DataView)dsgv.Select(DataSourceSelectArguments.Empty)).Table;
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = _testTable.Columns["Id"];
            _testTable.PrimaryKey = keyColumns;
            //return testTable;
            //var view = _testTable.DefaultView;
            //CreateGridColumns(view);
            return _testTable;
        }
    }
    void CreateGridColumns(DataView view)
    {
        gv3.Columns.Clear();

    }

    protected void gv_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //if (g.IsEditing && e.Column.FieldName == "Id")
        //    e.Editor.ReadOnly = true;
        //else
        //string ana = Getrb(radioButtonList);
        //if (string.Format("{0}", ana == "" ? "0" : ana)=="4" && e.Column.FieldName == "a"){
            //ASPxComboBox combo = e.Editor as ASPxComboBox;
            //combo.Callback += combo_Callback;
        //} 
    }
    void combo_Callback(object sender, CallbackEventArgsBase e)
    {
        ASPxComboBox combo = sender as ASPxComboBox;
        combo.DataSource = GetDataTable("|A|B|C|D"); ;
        combo.ValueField = "idx";
        combo.TextField = "value";
        combo.TextFormatString = "{0}";
        combo.DataBind();
    }
    protected void gv_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
    {
        if (e.ButtonType == ColumnCommandButtonType.Update) {
            e.Visible = false;
        }
    }
    //protected void gv_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
    //{
    //    if (e.ButtonType == ColumnCommandButtonType.Update || e.ButtonType == ColumnCommandButtonType.Cancel) {
    //        e.Visible = false;
    //    }
    //}

    protected void gv3_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //DataTable dt = (DataTable)g.DataSource;//LoadGrid;
        //Byte[] image = e.NewValues["Picture"] as byte[];
        var values = new[] { "Id", "ActiveDate" };
        if (_testTable != null)
        {
            DataRow dr = _testTable.Rows.Find(e.Keys[0]);
            foreach (DataColumn column in _testTable.Columns)
            {
                if (!values.Any(column.ColumnName.Contains))
                {
                    dr[column.ColumnName] = e.NewValues[column.ColumnName];
                }
            }
            //dr["Data"] = e.NewValues["Data"];
        }
        g.CancelEdit();
        e.Cancel = true;
    }

    protected void gv3_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        ASPxGridView g = (ASPxGridView)sender;
        //DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
        DataRow row = _testTable.NewRow();
        //int max = Convert.ToInt32(_testTable.AsEnumerable()
        //        .Max(rw => rw["SampleNo"]));
        //e.NewValues["Id"] = GetNewId(_testTable);
        e.NewValues["StatusTemp"] = "Add";
        IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
            if (enumerator.Key.ToString() != "Count")
                row[enumerator.Key.ToString()] = enumerator.Value;
        g.CancelEdit();
        e.Cancel = true;
        _testTable.Rows.Add(row);
        string[] arr = { "load", row["Id"].ToString()};
        string result =string.Format("{0}", String.Join("|", arr));
        g.JSProperties["cpKeyValue"] = result;
    }

    protected void gv_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //dataTable
        var values = new[] { "a" };

        DataRow dr = TableData.Rows.Find(e.Keys[0]);
        foreach (DataColumn column in TableData.Columns)
        {
            if (column.ColumnName=="a")
                dr[column.ColumnName] = e.NewValues[column.ColumnName];
        }
            //update
            //buildchange(dr);
        g.CancelEdit();
        e.Cancel = true;
        //ASPxGridView g = sender as ASPxGridView;
        //var values = new[] { "Id" };
        ////DataTable table = (DataTable)g.DataSource;//LoadGrid;
        ////GridViewRow row = g.Rows[e.RowIndex];
        ////DataView dv = (DataView)dsGenDefect.Select(DataSourceSelectArguments.Empty);
        ////if (dv != null)
        ////{
        ////    DataTable table = dv.Table;
        //DataTable table = (DataTable)this.ct.Session["myDatatable"];
        //foreach (DataColumn column in table.Columns)
        //    {
        //        string name = column.ToString();
        //        var result = e.NewValues[name];
        //        //spInsertForeignMatter
        //        if (!values.Any(name.Contains))
        //        {
        //            using (SqlConnection con = new SqlConnection(cs.strConn))
        //            {
        //                using (SqlCommand cmd = new SqlCommand("spInsertDefect"))
        //                {
        //                    cmd.Connection = con;
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.AddWithValue("@SampId", string.Format("{0}", hGeID["GeID"]));
        //                    cmd.Parameters.AddWithValue("@Title", name.ToString());
        //                    cmd.Parameters.AddWithValue("@value", string.Format("{0}", result));
        //                    con.Open();
        //                    cmd.ExecuteNonQuery();
        //                    con.Close();
        //                }
        //            }
        //        }
        //    }
        ////}
        //g.CancelEdit();
        //e.Cancel = true;
    }

    protected void testgrid_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
    {
        if (e.MenuType == GridViewContextMenuType.Rows)
        {
            e.Items.Clear();
            //var item = e.CreateItem("Export", "Export");
            //item.BeginGroup = true;
            //item.Image.IconID = "export_export_16x16";
            //e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), item);
            //e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Custom), item);
            //AddMenuSubItem(item, "qrcode", "XLS", "~/Content/Images/icons8-qr-code-26.png", true);
            //AddMenuSubItem(item, "pdf", "XLS", "~/Content/Images/pdf.gif", true);
            //e.Items.Clear();
            var item = e.CreateItem("QR-Code", "ExportToPDF");
            item.Image.Url = @"~/Content/Images/icons8-qr-code-26.png";
            item.Image.Width = Unit.Percentage(18);
            e.Items.Add(item);

            item = e.CreateItem("Export", "Export");
            item.BeginGroup = true;
            item.Image.Url = @"~/Content/Images/if_sign-out_59204.png";
            e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Custom), item);
            AddMenuSubItem(item, "Del", "Del", "~/Content/Images/Cancel.gif", true);
            AddMenuSubItem(item, "XLS", "ExportToXLS", "~/Content/Images/excel.gif", true);
        }
    }
    private static void AddMenuSubItem(GridViewContextMenuItem parentItem, string text, string name, string iconID, bool isPostBack)
    {
        var exportToXlsItem = parentItem.Items.Add(text, name);
        exportToXlsItem.Image.Url = iconID;
    }

    protected void gv3_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        string[] param = e.Parameters.Split('|'); //bool selected = true;
        int id;
        if (!int.TryParse(param[1], out id))
            return;
        switch (param[0])
        {
            case "load":
                Session.Remove("sessionKey");
                _testTable =null;
                break;
        }
        g.DataBind();
    }

    protected void gv_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        g.KeyFieldName = "Id";
        g.DataSource = TableData;
        g.ForceDataRowType(typeof(DataRow));
        AddColumns();
    }
    private void AddColumns()
    {
        gv.Columns.Clear();
        if (TableData == null) return;
        var values = new[] { "Eye", "Skin", "Odor", "Texture","Gill" };
        var arr = new[] { "Odor", "Formalin", "Texture"};
        foreach (DataColumn c in TableData.Columns)
        {              
            string str = c.ColumnName.ToString();
                DataTable data = new DataTable();
            string ana = Getrb(radioButtonList);
            var args = str.Split('|');
            if ((values.Any(str.Contains)) && (radioButtonList.SelectedItem.Text == "Appearance"))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = str;
                cb.Caption = args[0];
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.ValueField = "idx";
                cb.PropertiesComboBox.TextField = "Value";
                cb.PropertiesComboBox.DataSource = GetDataTable("|A|B|C|D");
                gv.Columns.Add(cb);
            }
            else if ((arr.Any(str.Contains)) && (radioButtonList.SelectedItem.Text == "Sensory"))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = str;
                cb.Caption = args[0];
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.ValueField = "idx";
                cb.PropertiesComboBox.TextField = "Value";
                cb.PropertiesComboBox.DataSource = GetDataTable("Normal|AB Normal");
                gv.Columns.Add(cb);
            }
            else if (str.Contains("CheckBox"))
            {
                GridViewDataCheckColumn cc = new GridViewDataCheckColumn();
                cc.FieldName = str;
                cc.Caption = args[0];
                gv.Columns.Add(cc);
            }
            else if (str == "Title") {
                GridViewDataColumn d = new GridViewDataColumn();
                d.FieldName = args[0]; 
                gv.Columns.Add(d);
            }
            else if (str == "SubID" || str == "AnalysisTypes")
            {
                GridViewDataColumn d = new GridViewDataColumn();
                d.HeaderStyle.CssClass = "hide";
                d.EditCellStyle.CssClass = "hide";
                d.CellStyle.CssClass = "hide";
                d.FilterCellStyle.CssClass = "hide";
                d.FooterCellStyle.CssClass = "hide";
                d.GroupFooterCellStyle.CssClass = "hide";
                d.FieldName = args[0];
                gv.Columns.Add(d);
            }
            else
                AddTextColumn(str);
        }
        gv.KeyFieldName = TableData.Columns[0].ColumnName;
        gv.Columns[0].Visible = false;
    }
    private void AddTextColumn(string fieldName)
    {
        var values = new[] { "Id", "ActiveDate", "SampId" };
        if (!values.Any(fieldName.Contains)) { 
            var args = fieldName.Split('|');
            GridViewDataTextColumn c = new GridViewDataTextColumn();
            if (IsNumeric(fieldName)) {
                int a = Convert.ToInt32(fieldName) + 1;
                c.Caption = a.ToString();
            } else
                c.Caption = args[0];
            c.FieldName = fieldName;
            gv.Columns.Add(c);
        }
    }
    public bool IsNumeric(string input)
    {
        int test;
        return int.TryParse(input, out test);
    }
    DataTable buildData(string Keys)
    {
        DataTable _t = new DataTable();
                    SqlParameter[] param = { new SqlParameter("@Id", Keys.ToString())};
                    _t = cs.GetRelatedResources("spGenDefect", param);
        return _t;
    }

    protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        if (string.IsNullOrEmpty(e.Parameters))
            return;
        var args = e.Parameters.Split('|');
        long id;
        if (!long.TryParse(args[1], out id))
            return;
        if (args[0] == "load")
        {
            TableData = new DataTable();
            //TableData.Columns.Add("Id", typeof(int));
            //TableData.Columns.Add("AnalysisTypes", typeof(string));
            //TableData.Columns.Add("Title", typeof(string));
            //TableData.Columns.Add("value", typeof(string));
            //TableData.Columns.Add("SubID", typeof(string));
            //TableData.Columns.Add("SampId", typeof(string));
            //    if (TableData == null){
            //    TableData = myclass.builditems(
            //        @"select top 0 ''a,Id,Name as ColumnName,''SubID,AnalysisTypes from MasAnalysis");
            //        DataColumn[] keyColumns = new DataColumn[1];
            //        keyColumns[0] = TableData.Columns["Id"];
            //        TableData.PrimaryKey = keyColumns;
            //    }
            //    if (TableData != null) {
            //        DataRow _row = _testTable.Select(string.Format("Id={0}", id.ToString())).FirstOrDefault();
            //        if (_row != null){
            //            int j = Convert.ToInt32(_row["Counts"]);
            //            ActivePageSymbol = id.ToString();
            int Keys = Convert.ToInt32(hGeID["GeID"]);
            TableData = buildData(string.Format("{0}", Keys.ToString()));
            //            foreach (DataRow _r in _t.Rows)
            //            {
            //                int max = Convert.ToInt32(TableData.AsEnumerable()
            //                .Max(row => row["Id"]));
            //                _r["Id"] = max + 1;
            //                TableData.ImportRow(_r);
            //            }
            //        }
            //    }
        }
            if (args[0] == "symbol")
        {
            ActivePageSymbol = id.ToString();
        }
        if (args[0] == "reload")
        {
            if (!string.IsNullOrEmpty(args[2]))
            {
                Object sumObject = TableData.Compute("Count(Result)", "SubID=" + args[2]);
                if (Convert.ToDecimal(sumObject) == 0)
                {
                    for (int i = 0; i < Convert.ToInt32(args[1]); i++)
                    {
                        int o = 0;
                        do
                        {
                            DataRow dr = TableData.NewRow();
                            int max = Convert.ToInt32(TableData.AsEnumerable()
                                            .Max(row => row["Id"]));
                            max++;
                            int name = i;
                            name++;
                            dr["Id"] = max;
                            dr["Title"] = name;
                            dr["SubID"] = args[2].ToString();
                            dr["AnalysisTypes"] = o.ToString();
                            TableData.Rows.Add(dr);
                            o++;
                        } while (o < 2);
                    }
                    SqlParameter[] p = { new SqlParameter("@group", args[2]) };
                    var dt = cs.GetRelatedResources("spGetAnalysisGroup", p);
                    TableData.Merge(dt);
                    //foreach (DataRow _r in dt.Rows)
                    //{
                    //    DataRow xrow = TableData.NewRow();
                    //    int newmax = Convert.ToInt32(TableData.AsEnumerable()
                    //    .Max(row => row["Id"]));
                    //    newmax++;
                    //    xrow = _r;
                    //    xrow["Id"] = newmax;
                    //    TableData.Rows.Add(xrow);
                    //}
                    DataColumn[] keyColumns = new DataColumn[1];
                    keyColumns[0] = TableData.Columns["Id"];
                    TableData.PrimaryKey = keyColumns;
                }
            }
        }
        g.DataBind();
    }
    const string sessionKey = "CE6907BD-E867-4cbf-97E2-F1EB702F433";
    public string ActivePageSymbol
    {
        get
        {
            if (ct.Session[sessionKey] == null)
                ct.Session[sessionKey] = string.Format("{0}", 1);
            return (string)ct.Session[sessionKey];
        }
        set { ct.Session[sessionKey] = value; }
    }
    private DataTable LoadData(ASPxGridView g)
    {
        if (TableData != null)
            return TableData;
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = TableData.Columns["Id"];
        TableData.PrimaryKey = keyColumns;
        //return testTable;
        var view = TableData.DefaultView;
        //CreateGridColumnsData(view, g);
        return TableData;
    }
    DataTable GetDataTable(string value)
    {
        return myclass.builditems(string.Format(@"select idx,value from dbo.FNC_SPLIT('{0}','|')", value));
    }
    void CreateGridColumnsData(DataView view, ASPxGridView g)
    {
        g.Columns.Clear();
        AddCommandColumn(g);
        foreach (DataColumn c in view.Table.Columns)
        {
            if (c.ColumnName == "Eye")
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = c.ColumnName;
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.TextField = "value";
                cb.PropertiesComboBox.ValueField = "idx";
                cb.PropertiesComboBox.TextFormatString = "{0}";
                cb.PropertiesComboBox.DataSource = GetDataTable("|A|B|C|D");
                cb.Width = Unit.Percentage(15);
                g.Columns.Add(cb);
            }
            else
                AddTextColumn(c.ColumnName, g);
        }
        g.KeyFieldName = view.Table.Columns[0].ColumnName;
        g.Columns[0].Visible = false;
    }
    void AddCommandColumn(ASPxGridView g)
    {
        GridViewCommandColumn command = new GridViewCommandColumn();
        g.Columns.Add(command);
        command.ShowEditButton = true;
        command.Width = Unit.Percentage(10);
    }
    private void AddTextColumn(string fieldName, ASPxGridView g)
    {
        var args = fieldName.Split('|');
        GridViewDataTextColumn c = new GridViewDataTextColumn();
        c.FieldName = fieldName;
        c.Caption = args[0];
        g.Columns.Add(c);
    }


    protected void gv3_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        if (e.NewValues["Counts"]==null){
            e.Errors[g.Columns["Counts"]] = "Value can't be null.";
        }
        if (string.IsNullOrEmpty(e.RowError) && e.Errors.Count > 0) e.RowError = "Please, correct all errors.";
    }

    protected void gv3_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        ASPxGridView g = (ASPxGridView)sender;
        if (e.ButtonID != "remove") return;
        object keyValue = g.GetRowValues(e.VisibleIndex, g.KeyFieldName);
        DataRow found = _testTable.Rows.Find(keyValue);
        //t.Rows.Remove(found);
        found["Mark"] = found["Mark"].ToString() == "D" ? "" : "D";
        _testTable.AcceptChanges();
        g.DataBind();
    }

    protected void gv3_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        string Result = string.Format("{0}", g.GetRowValues(e.VisibleIndex, "Mark"));
        bool isOddRow = string.Format("{0}", Result) != "" && !string.IsNullOrEmpty(Result);
        if (e.ButtonID == "remove" && isOddRow)
        {
            e.Image.Url = "~/Content/Images/Refresh.png";
            e.Image.ToolTip = "Return";
        }
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
    public void UpdateData(GridDataItem model)
    {
        DataRow dr = TableData.Rows.Find(model.Id);
        var values = new[] { "a" };
        foreach (DataColumn column in TableData.Columns)
        {
            if (column.ColumnName=="a")
                dr[column.ColumnName] = model.a;
        }
    }

    protected void gv3_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
    {
        ASPxGridView g = (ASPxGridView)sender;
        if (_testTable != null)
        {
            //int NextRowID = _testTable.Rows.Count;
            int NextRowID = Convert.ToInt32(_testTable.AsEnumerable()
            .Max(row => row["Id"]));
            NextRowID++;
            e.NewValues["Id"] = NextRowID;
            e.NewValues["SampleNo"] = NextRowID;
        }
    }

    protected void gvTemp_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
    {
        foreach (var args in e.InsertValues)
        {
            DataRow row = _Temp.NewRow();
            int NextRowID = _Temp.Rows.Count;
            NextRowID++;
            row["Id"] = GetNewId(_Temp);
            row["StatusTemp"] = "Add";
            foreach (DataColumn column in _Temp.Columns)
            {
                if (!column.ColumnName.Contains("Id"))
                {
                    switch (column.ColumnName)
                    {
                        case "Mark":
                            row[column.ColumnName] = "X";
                            break;
                        default:
                            row[column.ColumnName] = args.NewValues[column.ColumnName];
                            break;
                    }
                }
            }
            _Temp.Rows.Add(row);
        }

        foreach (var args in e.UpdateValues)
        {
            DataRow dr = _Temp.Rows.Find(args.Keys["Id"]);
            foreach (DataColumn column in _Temp.Columns)
            {
                if (!column.ColumnName.Contains("Id"))
                {
                    dr[column.ColumnName] = args.NewValues[column.ColumnName];
                }
            }
        }
        foreach (var args in e.DeleteValues)
        {
            DataRow found = _Temp.Rows.Find(args.Keys["Id"]);
            _Temp.Rows.Remove(found);
        }

        e.Handled = true;
    }

    protected void gvApp_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        g.KeyFieldName = "Id";
        g.DataSource = _TempApp;
        g.ForceDataRowType(typeof(DataRow));
        ReAddColumns(g);
    }

    protected void gvApp_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        var values = new[] { "Eye", "Skin", "Odor", "Texture", "Gill" };
        ASPxGridView grid = (ASPxGridView)sender;
        if (values.Any(e.Column.FieldName.Contains))
        {
            ASPxComboBox cb = (ASPxComboBox)e.Editor;
            //cb.Columns.Add(new ListBoxColumn("value"));
            cb.ValueField = "idx";
            cb.TextField = "Value";
            cb.DataSource = GetDataTable("|A|B|C|D");
            cb.DataBindItems();
        }
    }
    private void ReAddColumns(ASPxGridView g)
    {
        g.Columns.Clear();
        if (_TempApp == null) return;
        var values = new[] { "Eye", "Skin", "Odor", "Texture", "Gill" };
        foreach (DataColumn c in _TempApp.Columns)
        {
            string str = c.ColumnName.ToString();
            DataTable data = new DataTable();
            string ana = Getrb(radioButtonList);
            var args = str.Split('|');
            if ((values.Any(str.Contains)))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = str;
                cb.Caption = args[0];
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.ValueField = "idx";
                cb.PropertiesComboBox.TextField = "Value";
                cb.PropertiesComboBox.DataSource = GetDataTable("|A|B|C|D");
                g.Columns.Add(cb);
            }
        }
        g.KeyFieldName = _TempApp.Columns[0].ColumnName;
        //g.Columns[0].Visible = false;
    }
    protected void gvApp_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        if (string.IsNullOrEmpty(e.Parameters))
            return;
        var args = e.Parameters.Split('|');
        long id;
        if (!long.TryParse(args[1], out id))
            return;
        if (args[0] == "reload")
        {
            SqlParameter[] p = { new SqlParameter("@group", args[2]) };
            _TempApp = cs.GetRelatedResources("spGetAnalysis", p);
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = _TempApp.Columns["Id"];
            _TempApp.PrimaryKey = keyColumns;
        }
        g.DataBind();
    }

    protected void gv_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //dataTable
        var values = new[] { "a" };
        foreach (var args in e.UpdateValues)
        {
            if (TableData != null)
            {
                DataRow dr = TableData.Rows.Find(args.Keys["Id"]);
                foreach (DataColumn column in TableData.Columns)
                {
                    if (column.ColumnName == "value")
                        dr[column.ColumnName] = args.NewValues[column.ColumnName];
                }
            }
        }
        e.Handled = true;
    }

    protected void gv_DataBound(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        if (TableData != null)
        {
            DataTable _dt = (DataTable)g.DataSource;
            if (_dt != null)
                if (_dt.Rows.Count > 0)
                {
                    string ana = Getrb(radioButtonList);
                    g.AutoFilterByColumn(g.Columns["SubID"], ActivePageSymbol);
                    g.AutoFilterByColumn(g.Columns["AnalysisTypes"], string.Format("{0}", ana == "" ? "0" : ana));
                }
        }
    }
}
public class GridDataItem
{
    public int Id { get; set; }
    public string ColumnName { get; set; }
    public string a { get; set; }
    public string SubID { get; set; }
    public string AnalysisTypes { get; set; }
}