using ClosedXML.Excel;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Web.ASPxSpreadsheet;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_EditForms_LabForm : System.Web.UI.UserControl
{
    MyDataModule cs = new MyDataModule();
    ServiceCS myclass = new ServiceCS();
    HttpContext c = HttpContext.Current;
    EdiFormTemplate template = new EdiFormTemplate();
    string user_name = HttpContext.Current.User.Identity.Name.Replace(@"THAIUNION\", @"");
    private DataTable _dataTable
    {
        get { return Page.Session["CustomTable"] == null ? null : (DataTable)Page.Session["CustomTable"]; }
        set { Page.Session["CustomTable"] = value; }
    }
    private DataTable _buildcolumn
    {
        get { return Page.Session["buildcolumn"] == null ? null : (DataTable)Page.Session["buildcolumn"]; }
        set { Page.Session["buildcolumn"] = value; }
    }
    private DataTable _testTable
    {
        get { return Page.Session["sessionKey"] == null ? null : (DataTable)Page.Session["sessionKey"]; }
        set { Page.Session["sessionKey"] = value; }
    }
    string fExpr
    {
        get { return Page.Session["fExpr"] == null ? "X" : Page.Session["fExpr"].ToString(); }
        set { Page.Session["fExpr"] = value; }
    }
    string matType
    {
        get { return this.Session["matType"] == null ? "0" : this.Session["matType"].ToString(); }
        set { this.Session["matType"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //hMaterialType["MaterialType"] = "";
            Session.Clear();
            DateTime today = DateTime.Today;
            DateTime b = DateTime.Now.AddDays(-30);
            filter.FilterExpression = string.Format("[KeyDate] Between(#{0}#, #{1}#)",
                    b.ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd"));
            hGeID["GeID"] = string.Format("{0}", 0);
            heditor["editor"] = string.Format("{0}", 1);
            Page.Session["user_name"] = user_name;
            Page.Session["Expr"] = string.Format("{0}", fExpr);
            //matType = "0";
        }
    }

    protected void CmbShift_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] arr = { "Shift", e.Parameter };
        FillCombo(sender, arr);
    }
    protected void FillCombo(object sender,string[] arr)
    {
        ASPxComboBox comb = sender as ASPxComboBox;
        DataSet ds = myclass.GetDataSet(string.Join("|", arr));
        comb.DataSource = ds;
        comb.DataBind();
    }
        protected void cmbLine_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] arr = { "Line", e.Parameter };
        FillCombo(sender, arr);
    }

    protected void CmbRecorder_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] arr = { "Recorder", e.Parameter };
        FillCombo(sender, arr);
    }

    protected void cmbncpType_Callback(object sender, CallbackEventArgsBase e)
    {
        ASPxComboBox comb = sender as ASPxComboBox;
        comb.DataBind();
    }

    protected void Cmblocation_Callback(object sender, CallbackEventArgsBase e)
    {
        ASPxComboBox comb = sender as ASPxComboBox;
        DataSet ds = myclass.GetDataSet(e.Parameter);
        comb.DataSource = ds;
        comb.DataBind();
        if (hGeID["GeID"].ToString() == "0")
            comb.SelectedIndex = 0;
    }

    protected void cmbApprove1_Callback(object sender, CallbackEventArgsBase e)
    {
        ASPxComboBox comb = (ASPxComboBox)sender;
        comb.DataBind();
        //string[] arr = { "Approve1", e.Parameter };
        //FillCombo(sender, arr);
        if (string.IsNullOrEmpty(e.Parameter))
            return;
        GetApprove(comb, e);
    }
    void GetApprove(ASPxComboBox comb, CallbackEventArgsBase e)
    {
        var args = e.Parameter.Split('|');
        if (args[0].ToString() == "reload")
        {
            foreach (ListEditItem item in comb.Items)
            {
               if(args[1] == item.Value.ToString()) { 
                    comb.SelectedIndex = item.Index;
                    break;
                }
            }
        }
    }
    protected void cmbApprove2_Callback(object sender, CallbackEventArgsBase e)
    {
        //string[] arr = { "Approve2", e.Parameter };
        //FillCombo(sender, arr);
        ASPxComboBox comb = (ASPxComboBox)sender;
        comb.DataBind();
        if (string.IsNullOrEmpty(e.Parameter))
            return;
        GetApprove(comb, e);
    }

    protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (!grid.IsEditing || e.Column.FieldName != "Problem") return;
        if (e.KeyValue == DBNull.Value || e.KeyValue == null) return;
        //object val = grid.GetRowValuesByKeyValue(e.KeyValue, "Country");
        //if (val == DBNull.Value) return;
        //string country = (string)val;
        ASPxComboBox combo = e.Editor as ASPxComboBox;
        //FillCityCombo(combo, country);
        combo.Callback += new CallbackEventHandlerBase(cmbProblem_OnCallback);
    }

    void cmbProblem_OnCallback(object source, CallbackEventArgsBase e)
    {
        //string[] arr = { e.Parameter };
        //FillCombo(source as ASPxComboBox, arr);
    }
    protected void grid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        ASPxGridView g = (ASPxGridView)sender;
        //DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
        DataRow row = _dataTable.NewRow();
        e.NewValues["Id"] = GetNewId(_dataTable);
        e.NewValues["StatusTemp"] = "Add";
        IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
            if (enumerator.Key.ToString() != "Count")
                row[enumerator.Key.ToString()] = enumerator.Value;
        g.CancelEdit();
        e.Cancel = true;
        _dataTable.Rows.Add(row);
        //Page.Session["Id"] = row["Id"];
    }
    int GetNewId(DataTable dt)
    {
        //ds = (DataSet)Session["DataSet"];
        //DataTable table = ds.Tables[0];
        if (dt.Rows.Count == 0) return 1;
        int max = Convert.ToInt32(dt.Rows[0]["Id"]);
        for (int i = 1; i < dt.Rows.Count; i++)
        {
            if (Convert.ToInt32(dt.Rows[i]["Id"]) > max)
                max = Convert.ToInt32(dt.Rows[i]["Id"]);
        }
        return max + 1;
    }
    protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        DataTable dt = (DataTable)g.DataSource;//LoadGrid;
        //Byte[] image = e.NewValues["Picture"] as byte[];
        if (dt != null)
        {
            var values = new[] { "Id", "RequestNo"};
            DataRow dr = dt.Rows.Find(e.Keys[0]);
            foreach (DataColumn column in _dataTable.Columns)
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
        if (args[0] == "SaveMail" || args[0] == "New") 
        {
            RootObject ro = new RootObject();
            ro.ID = string.Format("{0}", id);
            ro.ncptype = string.Format("{0}", cmbncpType.Text);
            ro.ncpid = string.Format("{0}", txtncpid.Text);
            ro.Problem = string.Format("{0}", "");
            ro.FirstDecision = string.Format("{0}", cmbFirstDecision.Value);
            ro.Decision = string.Format("{0}", cmbdecision.Value);
            ro.KeyDate = string.Format("{0}", deKeyDate.Value);
            ro.Location = string.Format("{0}", Cmblocation.Text);
            ro.Plant = string.Format("{0}", cmbplant.Text);
            ro.MaterialType = string.Format("{0}", CmbMaterialType.SelectedItem.Text);
            ro.BatchCode = string.Format("{0}", "");
            ro.Product  = string.Format("{0}", 0);
            ro.Batchsap  = string.Format("{0}", 0);
            ro.Active = string.Format("{0}", 0);
            ro.Material= string.Format("{0}", 0);
            ro.ProductionDate= string.Format("{0}", dePrddate.Value);
            ro.Quantity = string.Format("{0}", tbHoldQuantity.Text);
            ro.Shift = string.Format("{0}", cbShift.Value);
            ro.HoldQuantity = string.Format("{0}", cbOptfull.Value);
            ro.ProblemQty = string.Format("{0}", tbProblemqty.Text);
            ro.Action = string.Format("{0}", 0);
            ro.Remark = string.Format("{0}", mRemark.Text);
            ro.Approve=  string.Format("{0}", cmbApprove1.Value);
            ro.Approvefinal = string.Format("{0}", cmbApprove2.Value);
            ro.user = string.Format("{0}", user_name);
            ro.LinesNo = string.Format("{0}", cmbLine.Text);
            ro.ShiftOption= string.Format("{0}", CmbShift.Text);
            ro.Times = string.Format("{0}", Cmbtimes.Text);
            ro.ResultDecision = string.Format("{0}", cmbFirstDecision.Value);

            ro.Supplier=string.Format("{0}", "");
            ro.Packaging=string.Format("{0}", "");
            ro.Batch_Packaging=string.Format("{0}", "");
            ro.Recorder=string.Format("{0}", CmbRecorder.Text);
            DataTable r = myclass.savedata(ro);
            foreach (DataRow dr in r.Rows)
            {
                string Keys = dr["ID"].ToString();
                foreach (DataRow row in _dataTable.Rows)
                    using (SqlConnection con = new SqlConnection(cs.strConn))
                    {
                        using (SqlCommand cmd = new SqlCommand("spinsertDetail"))
                        {
                            cmd.Connection = con;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", row["ID"].ToString());
                            cmd.Parameters.AddWithValue("@Problem", row["Problem"].ToString());
                            cmd.Parameters.AddWithValue("@Detail", row["Detail"].ToString());
                            cmd.Parameters.AddWithValue("@RequestNo", Keys.ToString());
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                g.JSProperties["cpSelectedRowKey"] = string.Format("{0}|{1}", args[0],dr["ID"]);
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
        if (args[0] == "Delete" && args.Length > 1)
        {
            using (SqlConnection con = new SqlConnection(cs.strConn))
            {
                string query = @"insert into tblChangeResult values (@Id,'Delete','',@user,format(Getdate(),'dd-MMM-yyyy HH:mm:ss'));
                    update tblncp set Active=1,modifyBy=@user,modifyOn=getdate() where Id=@Id";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Id", id.ToString());
                    cmd.Parameters.AddWithValue("@user", cs.user_name.ToString());
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        if(args[0]== "ExportToXLS")
        {
            export();
        }
        g.DataBind();
    }
    void export()
    {
        string name = "1";
            ASPxSpreadsheet spreadsheet = new ASPxSpreadsheet();
            XLWorkbook wb = new XLWorkbook();
            string path = Server.MapPath(@"~/App_Data/Documents/Book1.xlsx");//F10B1907
        spreadsheet.Document.LoadDocument(path);
        Worksheet wsheet = spreadsheet.Document.Worksheets[0];

        var st = new MemoryStream();
        spreadsheet.Document.SaveDocument(st, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
        Response.Clear();
        Response.ContentType = "application/force-download";
        String fileName = String.Format(name + "_{0}.xlsx", DateTime.Now.ToString("yyyyMMddhhmmss"));
        Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        Response.BinaryWrite(st.ToArray());
        Response.End();
    }
    public string DataTableToJSONWithJSONNet(DataTable table)
    {
        string JSONString = string.Empty;
        JSONString = JsonConvert.SerializeObject(table);
        return JSONString;
    }

    protected void gv_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        (sender as ASPxGridView).DataSource = LoadGrid;
        g.KeyFieldName = "Id";
        //g.DataSource = _testTable;
        g.ForceDataRowType(typeof(DataRow));
    }
     private DataTable LoadGrid
    {
        get
        {
            if (_testTable == null)
                return _testTable;
            initGrid();
            return _testTable;
        }
    }
    private void initGrid()
    {
        gv.Columns.Clear();
        AddCommandColumn();
        foreach (DataRow c in _buildcolumn.Rows)
        {
            string[] words = c["colname"].ToString().Split(';');
            foreach (string word in words)
            {
            var strname = word.ToString();
            if (strname.Equals("Packaging"))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = strname;
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.Columns.Add(new ListBoxColumn("Material"));
                cb.PropertiesComboBox.Columns.Add(new ListBoxColumn("Description"));
                cb.PropertiesComboBox.ValueField = "Material";
                cb.PropertiesComboBox.TextFormatString = "{0}";
                cb.PropertiesComboBox.CallbackPageSize = 10;
                cb.PropertiesComboBox.EnableCallbackMode = true;
                cb.PropertiesComboBox.IncrementalFilteringDelay = 500;
                //cb.PropertiesComboBox.ClientInstanceName = "Material";
                cb.PropertiesComboBox.DataSource = dsPackaging;
                cb.PropertiesComboBox.ClientSideEvents.TextChanged = "Combo_TextChanged";
                gv.Columns.Add(cb);
            }
            else if (strname.Equals("ProductionDate") || strname.Equals("ReceivedDate")) {
                    GridViewDataDateColumn dt = new GridViewDataDateColumn();
                    dt.FieldName = strname;
                    dt.PropertiesDateEdit.EditFormatString = "dd-MM-yyyy";
                    dt.PropertiesDateEdit.DisplayFormatString = "dd-MM-yyyy";
                    gv.Columns.Add(dt);
                }
            else if (strname.Equals("Supplier"))
            {
                GridViewDataComboBoxColumn CBCol = new GridViewDataComboBoxColumn();
                CBCol.FieldName = strname;
                CBCol.PropertiesComboBox.Columns.Clear();
                CBCol.PropertiesComboBox.TextField = "Name";
                CBCol.PropertiesComboBox.ValueField = "Id";
                CBCol.PropertiesComboBox.TextFormatString = "{0}";
                CBCol.PropertiesComboBox.EnableCallbackMode = true;
                CBCol.PropertiesComboBox.CallbackPageSize = 10;
                CBCol.PropertiesComboBox.DataSource = dsSupplier;
                //cb.Width = Unit.Percentage(15);
                gv.Columns.Add(CBCol);
            }
            else if (strname.Equals("Material"))
            {
                GridViewDataColumn col = new GridViewDataColumn {
                    //Caption = "XX",
                    //Width = Unit.Pixel(150),
                    //Name = strname,
                    FieldName = strname,
                };
                ASPxGridLookup lookup = new ASPxGridLookup {
                    SelectionMode = GridLookupSelectionMode.Single,
                    KeyFieldName = "RowID",
                    AutoGenerateColumns = false,
                    ClientInstanceName = "gridLookup",
                    ID = "gridLookup"
                };
                lookup.Columns.Add(new GridViewDataTextColumn() { FieldName = "Production", VisibleIndex = 0 });
                lookup.Columns.Add(new GridViewDataTextColumn() { FieldName = "Material", VisibleIndex = 1 });
                lookup.Columns.Add(new GridViewDataTextColumn() { FieldName = "Description", VisibleIndex = 2 });
                //lookup.ClientSideEvents.ValueChanged = "OnLookupChanged";
                ASPxGridView gridView = lookup.GridView;
                gridView.CustomDataCallback += new ASPxGridViewCustomDataCallbackEventHandler(lookup_CustomDataCallback);
                lookup.DataSource = GetDataTable();
                lookup.GridViewProperties.Settings.ShowFilterRow = true;
                lookup.GridViewProperties.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                if (!(lookup.Columns[0] is GridViewCommandColumn))
                {
                    GridViewCommandColumn commandCol = new GridViewCommandColumn();
                    commandCol.ShowSelectCheckbox = true;
                    commandCol.VisibleIndex = 0;
                    commandCol.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
                    commandCol.ShowClearFilterButton = true;
                    lookup.Columns.Insert(0, commandCol);
                }                 
                lookup.TextFormatString = "{1}";
                    //lookup.Value = string.Format("Value='<%# Bind(\"{0}\") %>'", strname);
                    lookup.ClientSideEvents.GotFocus = "function(s,e) { s.ShowDropDown(); }";
                    lookup.Width = Unit.Percentage(100);
                LookupTemplate template = new LookupTemplate(lookup);
                col.EditItemTemplate = template;
                gv.Columns.Add(col);

            
            }else if (strname.Equals("SalesOrder"))
                {
                    GridViewDataButtonEditColumn ddc = new GridViewDataButtonEditColumn();
                    ddc.PropertiesButtonEdit.Buttons.Add(new EditButton("..."));
                    ddc.PropertiesButtonEdit.ClientSideEvents.ButtonClick = "OnButtonClick";
                    ddc.FieldName = strname;
                    gv.Columns.Add(ddc);
                }
                else if (strname.Equals("XXXXXX"))
                {
                    GridViewDataColumn col = new GridViewDataColumn
                    {
                        FieldName = strname,
                    };
                    ASPxGridLookup lookup = new ASPxGridLookup
                    {
                        SelectionMode = GridLookupSelectionMode.Single,
                        KeyFieldName = "Id",
                        AutoGenerateColumns = false,
                        ClientInstanceName = "gvLookup",
                        ID = "gvLookup"
                    };
                    lookup.Columns.Add(new GridViewDataTextColumn() { FieldName = "SaleDoc", VisibleIndex = 0 });
                    lookup.Columns.Add(new GridViewDataTextColumn() { FieldName = "Item", VisibleIndex = 1 });
                    lookup.Columns.Add(new GridViewDataTextColumn() { FieldName = "Material", VisibleIndex = 2 });
                    ASPxGridView gvLookup = lookup.GridView;
                    gvLookup.CustomDataCallback += new ASPxGridViewCustomDataCallbackEventHandler(lookup_CustomDataCallback);
                    //lookup.DataSource = dsOrder;
                    lookup.GridViewProperties.Settings.ShowFilterRow = true;
                    lookup.GridViewProperties.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                    if (!(lookup.Columns[0] is GridViewCommandColumn))
                    {
                        GridViewCommandColumn commandCol = new GridViewCommandColumn();
                        commandCol.ShowSelectCheckbox = true;
                        commandCol.VisibleIndex = 0;
                        commandCol.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
                        commandCol.ShowClearFilterButton = true;
                        lookup.Columns.Insert(0, commandCol);
                    }
                    lookup.TextFormatString = "{0}";
                    lookup.ClientSideEvents.GotFocus = "function(s,e) { s.ShowDropDown(); }";
                    lookup.Width = Unit.Percentage(100);
                    gvLookup template = new gvLookup(lookup);
                    col.EditItemTemplate = template;
                    gv.Columns.Add(col);
                }
            else
                AddTextColumn(strname);
            }
        }

        gv.KeyFieldName = _testTable.Columns["Id"].ColumnName;
        //gv.Columns[0].Visible = false;
    }
    void lookup_CustomDataCallback(object sender, ASPxGridViewCustomDataCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        var result = new Dictionary<string, string>();
        string[] param = e.Parameters.Split('|'); //bool selected = true;
        int id;
        if (!int.TryParse(param[1], out id))
            return;
        switch (param[0])
        {
            case "GetOrder":
                e.Result = result;
                break;
            case "GetBatch":
                string strSQL = string.Format(@"select * from tblMaterial where Material='{0}'", param[2]);
                var _dt = cs.build_Datatable(strSQL);
                DataRow rw = _dt.Select().FirstOrDefault();
                if (param[1] == "1")
                {
                    if (CmbShift.Text == "" || cmbLine.Text == "") return;
                    nameformatObject ro = new nameformatObject();
                    ro.customer = param[2];
                    ro.FrDt = Convert.ToDateTime(deKeyDate.Value).ToString("yyyyMMdd");
                    ro.ShiftOpt = CmbShift.Text;
                    ro.Line = cmbLine.Text;
                    ro.Shift = cbShift.Value.ToString();
                    result["index"] = param[1].ToString();
                    result["ProductCode"] = rw["Production"].ToString();
                    result["Description"] = rw["Description"].ToString();
                    result["BatchCode"] = myclass.nameformat(ro);
                    result["BatchSap"] = myclass.formatbatchsap(
                    Convert.ToDateTime(dePrddate.Value).ToString("yyyyMMdd"),
                    Convert.ToDateTime(deKeyDate.Value).ToString("yyyyMMdd"),
                    CmbShift.Text,
                    cmbLine.Text,
                    Cmbtimes.Text.ToString());
                    e.Result = result;
                }
                else if (param[1] == "2" || param[1] == "6")
                {
                    result["index"] = param[1].ToString();
                    result["Description"] = rw["Description"].ToString();
                    e.Result = result;
                }
                else if (param[1] == "5")
                {
                    result["index"] = param[1].ToString();
                    result["Description"] = rw["Description"].ToString();
                    e.Result = result;
                }
                else if (param[1] == "4")
                {
                    result["index"] = param[1].ToString();
                    result["Description"] = rw["Description"].ToString();
                    e.Result = result;
                }
                break;
            case "Material":
                e.Result = param[2].ToString();
                break;
        }
    }
    DataTable GetDataTable()
    {
        DataTable _Table = new DataTable();
        //Build Material
        dsMaterial.SelectParameters.Clear();
        dsMaterial.SelectParameters.Add("Type", TypeCode.String, string.Format("{0}", matType.ToString()));
        dsMaterial.DataBind();
        DataView dv = (DataView)dsMaterial.Select(DataSourceSelectArguments.Empty);
        if (dv != null)
            _Table = dv.Table;
        _Table.PrimaryKey = new DataColumn[] { _Table.Columns["Material"] };
        return _Table;
    }
    private void AddTextColumn(string fieldName)
    {
        var values = new[] { "Id","ProductCode", "Description", "BatchCode", "BatchSap", "Name", "BatchPKG","SupplierLot",
            "SupplierName", "PurchaseOrder","SalesItems","Weight","Mark" };        
        if (values.Any(fieldName.Contains)) { 
            GridViewDataTextColumn c = new GridViewDataTextColumn();
            c.FieldName = fieldName;
            if (fieldName == "Mark")
                c.Width = Unit.Pixel(0);
            gv.Columns.Add(c);
        }
    }
    void AddCommandColumn()
    {
        GridViewCommandColumn col = new GridViewCommandColumn();
        gv.Columns.Add(col);
        col.ShowEditButton = true;
        col.ShowUpdateButton = true;
        col.ShowDeleteButton = true;
        col.ShowNewButtonInHeader = true;
        col.Width = Unit.Pixel(42);
        col.FixedStyle = GridViewColumnFixedStyle.Left;
        col.ButtonRenderMode = GridCommandButtonRenderMode.Image;
        GridViewCommandColumnCustomButton but = new GridViewCommandColumnCustomButton();
        but.ID = "EditCost";
        //but.Text = "Upload";
        but.Image.ToolTip = "Subir Documentos al Contrato";
        but.Image.Url = "~/Content/Images/Cancel.gif";
        col.CustomButtons.Add(but);
        gv.Columns.Add(col);


    }
        DataTable BuildTable()
    {
        if (_dataTable != null)
            return _dataTable;
        _dataTable = new DataTable();
        int Id = Convert.ToInt32(hGeID["GeID"]);
        SqlParameter[] param = { new SqlParameter("@Id", Id.ToString()) };
        _dataTable = cs.GetRelatedResources("spSelectProblem", param);
        //_dataTable = ((DataView)dsgv.Select(DataSourceSelectArguments.Empty)).Table;
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = _dataTable.Columns["Id"];
        _dataTable.PrimaryKey = keyColumns;
        return _dataTable;
    }
    protected void grid_DataBinding(object sender, EventArgs e)
    {
        //DataSourceID="dsDetail"
        ASPxGridView g = sender as ASPxGridView;
        //(sender as ASPxGridView).DataSource = LoadGrid;
        g.KeyFieldName = "Id";
        g.DataSource = _dataTable;
        g.ForceDataRowType(typeof(DataRow));
    }
    protected void gv_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
    {
        ASPxGridView g = (ASPxGridView)sender;
        if (_testTable != null)
        {
            //int NextRowID = _testTable.Rows.Count;
            int NextRowID = Convert.ToInt32(_testTable.AsEnumerable()
            .Max(row => row["Id"]));
            NextRowID++;
            e.NewValues["Id"] = NextRowID;
            e.NewValues["Material"] = 0;
            e.NewValues["ReceivedDate"] = DateTime.Today; 
            e.NewValues["ProductionDate"] = DateTime.Today;
        }
    }
    protected void gv_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        ASPxGridView g = (ASPxGridView)sender;
        //DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
        if (_testTable != null)
        {
            DataRow row = _testTable.NewRow();
            e.NewValues["Id"] = GetNewId(_testTable);
            e.NewValues["StatusTemp"] = "Add";
            e.NewValues["RequestNo"] = Convert.ToInt32(hGeID["GeID"]);
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                if (enumerator.Key.ToString() != "Count")
                    row[enumerator.Key.ToString()] = enumerator.Value;
            _testTable.Rows.Add(row);
        }
        g.CancelEdit();
        e.Cancel = true;

    }

    protected void gv_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //DataTable dt = (DataTable)g.DataSource;//LoadGrid;
                                               //Byte[] image = e.NewValues["Picture"] as byte[];
        var values = new[] { "Id", "RequestNo","ActiveDate"};
        if (_testTable != null)
        {
            DataRow dr = _testTable.Rows.Find(e.Keys[0]);
            foreach (DataColumn column in _testTable.Columns)
            {
                if (!values.Any(column.ColumnName.Equals))
                {
                    if(e.NewValues["ProductionDate"]==null && column.ColumnName == "ProductionDate")
                        dr["ProductionDate"] = DateTime.Today;
                    else if(e.NewValues["ReceivedDate"] == null && column.ColumnName == "ReceivedDate")
                        dr["ReceivedDate"] = DateTime.Today;
                    else
                        dr[column.ColumnName] = e.NewValues[column.ColumnName];
                }
            }
            //dr["Data"] = e.NewValues["Data"];
        }
        g.CancelEdit();
        e.Cancel = true;
    }
    protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        var result = new Dictionary<string, string>();
        string[] args = e.Parameters.Split('|'); //bool selected = true;
        int id;
        if (!int.TryParse(args[1], out id))
            return;
        //g.Columns.Clear();
        switch (args[0])
        {
            case "SaveMail":
                if (_testTable != null)
                {
                    foreach (DataRow rw in _testTable.Rows)
                    {
                        using (SqlConnection con = new SqlConnection(cs.strConn))
                        {
                            using (SqlCommand cmd = new SqlCommand("spinsertProduct"))
                            {
                                cmd.Connection = con;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@ID", rw["ID"].ToString());
                                cmd.Parameters.AddWithValue("@ProductCode", rw["ProductCode"].ToString());
                                cmd.Parameters.AddWithValue("@Material", rw["Material"].ToString());
                                cmd.Parameters.AddWithValue("@BatchSap", rw["BatchSap"].ToString());
                                cmd.Parameters.AddWithValue("@Supplier", rw["Supplier"].ToString());
                                cmd.Parameters.AddWithValue("@user", cs.user_name.ToString());
                                cmd.Parameters.AddWithValue("@Packaging", rw["Packaging"].ToString());
                                cmd.Parameters.AddWithValue("@BatchCode", rw["BatchCode"].ToString());
                                cmd.Parameters.AddWithValue("@BatchPKG", rw["BatchPKG"].ToString());
                                cmd.Parameters.AddWithValue("@RequestNo", id.ToString());
                                cmd.Parameters.AddWithValue("@Mark", rw["Mark"].ToString());
                                cmd.Parameters.AddWithValue("@PurchaseOrder", rw["PurchaseOrder"].ToString());
                                cmd.Parameters.AddWithValue("@SupplierLot", rw["SupplierLot"].ToString());
                                cmd.Parameters.AddWithValue("@ProductionDate", rw["ProductionDate"].ToString());
                                cmd.Parameters.AddWithValue("@ReceivedDate", rw["ReceivedDate"].ToString());
                                cmd.Parameters.AddWithValue("@SalesOrder", rw["SalesOrder"].ToString());
                                cmd.Parameters.AddWithValue("@SalesItems", rw["SalesItems"].ToString());
                                cmd.Parameters.AddWithValue("@Weight", rw["Weight"].ToString());
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }
                }
                break;
            case "load": case "c":
                matType = args[1].ToString();
                Page.Session.Remove("sessionKey");
                int Id = Convert.ToInt32(hGeID["GeID"]);
                _testTable = new DataTable();
                SqlParameter[] param = { new SqlParameter("@Id", Id),
                new SqlParameter("@Type", args[1].ToString())};
                _testTable = cs.GetRelatedResources("spGetProduct", param);
                DataColumn[] keyColumns = new DataColumn[1];
                keyColumns[0] = _testTable.Columns["Id"];
                _testTable.PrimaryKey = keyColumns;

                string strSQL = string.Format(@"select concat(colname,';','Mark')colname from Unlockcolumn where fn like '%{0}%'", matType);
                _buildcolumn = cs.build_Datatable(strSQL);
                //initGrid();
                break;
            //case "Delete":
            //    DataRow dr = _testTable.Rows.Find(id);
            //        dr["Mark"] = dr["Mark"].ToString() == "D" ? "" : "D";
            //        break;
        }
        g.DataBind();
    }
    //protected void Page_Init(object sender, EventArgs e)
    //{
    //    gv.DataSource = LoadGrid;
    //    gv.KeyFieldName = "Id";
    //}
    protected void gv_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        ASPxGridView g = (ASPxGridView)sender;
        if (e.ButtonID != "EditCost") return;
        object keyValue = g.GetRowValues(e.VisibleIndex, g.KeyFieldName);
        DataRow found = _testTable.Rows.Find(keyValue);
        //t.Rows.Remove(found);
        found["Mark"] = found["Mark"].ToString() == "D" ? "" : "D";
        _testTable.AcceptChanges();
        g.DataBind();
    }
    const string UploadDirectory = "~/Content/UploadControl/";
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
        string Id = string.Format("{0}", hGeID["GeID"]);
        myclass.uploadfile(resultFilePath, Id, user_name,name);
        e.CallbackData = name + "|" + url + "|" + sizeText;

    }
    protected void UploadControl_FilesUploadComplete(object sender, FilesUploadCompleteEventArgs e)
    {

    }
    private DataTable GetDataSource()
    {
        if (Session["DataSource"] == null)
        {
            DataTable table = new DataTable();

            DataColumn column;
            DataRow row;

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ImageID";
            column.ReadOnly = true;
            column.Unique = true;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ParentKey";
            column.AutoIncrement = false;
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ImageName";
            column.AutoIncrement = false;
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Boolean");
            column.ColumnName = "IsFolder";
            column.AutoIncrement = false;
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns["ImageID"];
            table.PrimaryKey = PrimaryKeyColumns;

            // Instantiate the DataSet variable.
            //dataSet = new DataSet();
            // Add the new DataTable to the DataSet.
            //dataSet.Tables.Add(table);

            for (int i = 0; i < 4; i++)
            {
                row = table.NewRow();
                row["ImageID"] = i;
                row["ParentKey"] = 0;
                row["ImageName"] = (i == 0) ? "Images" : "Image" + i;
                row["IsFolder"] = i == 0;
                table.Rows.Add(row);
            }

            Session["DataSource"] = table;
        }
        return (DataTable)Session["DataSource"];
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
                _dataTable = BuildTable();
                break;
        }
        g.DataBind();
    }
    protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "StatusTemp")
        {
            var g = sender as ASPxGridView;
            DataRow row = g.GetDataRow(e.VisibleIndex);
            int index = e.VisibleIndex;
            if (g.VisibleRowCount != 0 && row != null)
            {
                byte[] data = g.GetRowValues(e.VisibleIndex, "StatusTemp") as byte[];
                int r = string.Format("{0}", data).Length;
                if (data != null && data.Length > 0)
                    e.Cell.BackColor = Color.LightGreen;
                else
                    e.Cell.BackColor = Color.Coral;
            }
        }
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
     
            SqlParameter[] param = {new SqlParameter("@ID",id.ToString()),
                        new SqlParameter("@username",user_name.ToString())};
            DataTable dt = cs.GetRelatedResources("spselectncpid", param);
            foreach (DataRow rw in dt.Rows)
            {
                result["TypeNCP"] = string.Format("{0}", rw["TypeNCP"]);
                result["FirstDecision"] = string.Format("{0}", rw["FirstDecision"]);
                result["ResultDecision"] = string.Format("{0}", rw["ResultDecision"]);
                result["Decision"] = string.Format("{0}", rw["Decision"]);
                result["KeyDate"] = string.Format("{0}", rw["KeyDate"]);
                result["NCPID"] = string.Format("{0}", rw["NCPID"]);
                result["Location"] = string.Format("{0}", rw["Location"]);
                result["Plant"] = string.Format("{0}", rw["Plant"]);
                result["ProductionDate"] = string.Format("{0}", rw["ProductionDate"]);
                result["Quantity"] = string.Format("{0}", rw["Quantity"]);
                result["Shift"] = string.Format("{0}", rw["Shift"]);
                result["HoldQuantity"] = string.Format("{0}", rw["HoldQuantity"]);
                result["Remark"] = string.Format("{0}", rw["Remark"]);
                result["Approve"] = string.Format("{0}", rw["Approve"]);
                result["Approvefinal"] = string.Format("{0}", rw["Approvefinal"]);
                result["LinesNo"] = string.Format("{0}", rw["LinesNo"]);
                result["ShiftOption"] = string.Format("{0}", rw["ShiftOption"]);
                result["Times"] = string.Format("{0}", rw["Times"]);
                result["MaterialType"] = string.Format("{0}", rw["MaterialType"]);
                result["PlantID"] = string.Format("{0}", rw["PlantID"]);
                result["Recorder"] = string.Format("{0}", rw["Recorder"]);
                result["ProblemQty"] = string.Format("{0}", rw["ProblemQty"]);
                result["Action"] = string.Format("{0}", rw["Action"]);
                result["editor"] = string.Format("{0}", rw["editor"]);
                result["matType"] = string.Format("{0}", rw["MaterialType"]);
                e.Result = result;
            }

        }
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
                Page.Session["GeID"] = string.Format("{0}", hGeID["GeID"]);
                break;
        }
        fm.DataBind();
    }
    protected void fileManager_FileUploading(object source, FileManagerFileUploadEventArgs e)
    {
        var leaveNo = string.Format("{0}", hGeID["GeID"]);
        var newleavepath = @"~/Content/UploadControl/" + leaveNo + @"/";
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
        using (var stream = new FileStream(newDocPath, FileMode.Open, FileAccess.Read)) {
            using (var read = new BinaryReader(stream)) { file = read.ReadBytes((int)stream.Length); } }
        Objattachment ro = new Objattachment();
        ro.Name = name;
        ro.ContentType = mimeType;
        ro.Data = file;
        ro.MatDoc = Convert.ToInt32(leaveNo);
        ro.ActiveBy = user_name;
        myclass.savebyte(ro);
        e.Cancel = true; //cancelling the upload, prevents duplicate uploads
        e.ErrorText = "Success"; //shown when the upload is cancelled
                                 //fmLeaveDocs.Refresh(); //does not work. Causes an error.
    }

    protected void grid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        ASPxGridView g = (ASPxGridView)sender;
        if (e.ButtonID != "remove") return;
        object keyValue = g.GetRowValues(e.VisibleIndex, g.KeyFieldName);
        DataRow found = _dataTable.Rows.Find(keyValue);
        //t.Rows.Remove(found);
        found["Mark"] = found["Mark"].ToString() == "D" ? "" : "D";
        _dataTable.AcceptChanges();
        g.DataBind();
    }

    protected void testgrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        string data = g.GetRowValues(Convert.ToInt32(e.VisibleIndex), "Active").ToString();
        if (data.ToString() == "1")
            e.Cell.BackColor = Color.Coral;
        //if (e.DataColumn.FieldName == "Active")
        //{
        //    var g = sender as ASPxGridView;
        //    DataRow row = g.GetDataRow(e.VisibleIndex);
        //    int index = e.VisibleIndex;
        //    if (g.VisibleRowCount != 0 && row != null)
        //    {
        //        var data = g.GetRowValues(e.VisibleIndex, "Active");
        //        if (data.ToString() == "1")
        //            e.Cell.BackColor = Color.Coral;
        //    }
        //}
    }
    protected void gv_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        //    if (e.Column.FieldName == "Material")
        //{
        //    if (_testTable != null)
        //    {
        //        var myResult = _testTable.AsEnumerable()
        //        .Select(s => new
        //        {
        //            id = s.Field<string>("Material"),
        //        })
        //        .Distinct().ToList();
        //    }
        //ASPxGridLookup GridLookup = (ASPxGridLookup)e.Editor;
        //string[] indexes = dropDownEdit.Text.Split(',');

        //ASPxListBox listBox = (ASPxListBox)dropDownEdit.FindControl("ASPxListBox1");
        //listBox.ClientInstanceName = "checkListBox";
        //listBox.ClientSideEvents.SelectedIndexChanged = String.Format("function(s,e){{OnListBoxSelectionChanged(s,e,{0});}}", dropDownEdit.ClientID);
        //dropDownEdit.ClientSideEvents.TextChanged = String.Format("function(s,e){{ SynchronizeListBoxValues(s,e,{0});}}", listBox.ClientID);
        //dropDownEdit.ClientSideEvents.DropDown = String.Format("function(s,e){{ SynchronizeListBoxValues(s,e,{0});}}", listBox.ClientID);
        //if (listBox == null) return;
        //foreach (ListEditItem item in listBox.Items)
        //{
        //    if (indexes.Contains<string>(item.Value.ToString()))
        //        item.Selected = true;
        //}
        //}
        if (e.Column.FieldName == "SalesOrder")
        {
            (e.Editor as ASPxButtonEdit).ClientSideEvents.ButtonClick = "OnBtnShowPopupClick('getsales')";
        }
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        export();
    }
    class MyComboBoxTemplate : ITemplate
    {
        private List<string> data()
        {
            //MyDataModule mycla = new MyDataModule();
            //DataTable table = mycla.builditems("select Name from MasUserType");
            //List<string> list = table.AsEnumerable()
            //               .Select(r => r.Field<string>("Name"))
            //               .ToList();
            //return list;
            
            return new List<string>(new string[] { "101", "102", "103", "901" });
        }
        public void InstantiateIn(Control container)
        {
            ASPxGridLookup combo = new ASPxGridLookup();
            //GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
            combo.ID = "combo";
            combo.DataSource = data();
            combo.Init += combo_Init;
            combo.DataBound += combo_DataBound;
            //combo.EnableTheming = false;
            combo.Border.BorderWidth = 0;
 
            //combo.SelectionMode = ListEditSelectionMode.CheckColumn;
            //combo.DropDownStyle = DropDownStyle.DropDown;
            combo.IncrementalFilteringMode = IncrementalFilteringMode.StartsWith;
            container.Controls.Add(combo);
        }
 
        void combo_DataBound(object sender, EventArgs e)
        {
        }

        void combo_Init(object sender, EventArgs e)
        {

        }
    }
    public class LookupTemplate : ITemplate
    {
        public ASPxGridLookup _lookup { get; set; }

        public LookupTemplate(ASPxGridLookup lookup)
        {
            this._lookup = lookup;
        }
        #region Implementation of ITemplate
        public void InstantiateIn(Control container)
        {
            _lookup.ClientSideEvents.ValueChanged = "OnLookupChanged";
            container.Controls.Add(_lookup);
        }
        #endregion
    }
    public class gvLookup : ITemplate
    {
        public ASPxGridLookup _lookup { get; set; }

        public gvLookup(ASPxGridLookup lookup)
        {
            this._lookup = lookup;
        }
        #region Implementation of ITemplate
        public void InstantiateIn(Control container)
        {
            _lookup.ClientSideEvents.ValueChanged = "gvLookupChanged";
            container.Controls.Add(_lookup);
        }
        #endregion
    }
    //public class LookupTemplate : ITemplate
    //{
    //    //private ASPxGridView _gridView;
    //    private SqlDataSource _SqlData;
    //    //public ASPxGridView Grid {
    //    //    get {
    //    //        return _gridView;
    //    //    }

    //    //    set {
    //    //        _gridView = value;
    //    //    }
    //    //}
    //    public LookupTemplate(SqlDataSource dataSource)
    //    {
    //        //_gridView = grid;
    //        _SqlData = dataSource;
    //    }

    //    public void InstantiateIn(Control container)
    //    {
    //        ASPxGridLookup gridLookupOptions = new ASPxGridLookup { ID = "GridLookup" };
    //        gridLookupOptions.ClientInstanceName = "gridLookup";
    //        container.Controls.Add(gridLookupOptions);
    //        gridLookupOptions.IncrementalFilteringMode= IncrementalFilteringMode.Contains;
    //        gridLookupOptions.SelectionMode = GridLookupSelectionMode.Single;
    //        gridLookupOptions.TextFormatString = "{0}";
    //        gridLookupOptions.Value = "Value='<%# Bind(\"Material\") %>'";

    //        gridLookupOptions.MultiTextSeparator = " ";
    //        gridLookupOptions.Caption = "";
    //        //gridLookupOptions.GridViewProperties.SettingsBehavior.AllowFocusedRow = false;
    //        //gridLookupOptions.GridViewProperties.SettingsBehavior.AllowSelectByRowClick = true;
    //        //gridLookupOptions.GridViewProperties.SettingsBehavior.AllowSelectSingleRowOnly = true;
    //        gridLookupOptions.GridViewProperties.Settings.ShowFilterRow = true;
    //        gridLookupOptions.GridViewProperties.Settings.ShowStatusBar = GridViewStatusBarMode.Visible;
    //        //gridLookupOptions.GridViewProperties.Settings.ShowFooter = false;
    //        gridLookupOptions.ClientSideEvents.ValueChanged = "OnChanged";
    //        //gridLookupOptions.ClientSideEvents.GotFocus = "OnGotFocus";
    //        //gridLookupOptions.ClientSideEvents.KeyDown = "OnKeyDown";
    //        //gridLookupOptions.ClientSideEvents.CloseUp = "OnCloseUp";
    //        //gridLookupOptions.ClientSideEvents.EndCallback = "OnEndCallback";
    //        //gridLookupOptions.ClientSideEvents.DropDown = "OnDropDown";
    //        //gridLookupOptions.DataSource = _SqlData;
    //        gridLookupOptions.DataSource = _SqlData;
    //        //gridLookupOptions.DataBind();
    //        gridLookupOptions.KeyFieldName = "Material";
    //        //if (!(gridLookupOptions.Columns[0] is GridViewCommandColumn))
    //        //{
    //        //    GridViewCommandColumn commandCol = new GridViewCommandColumn();
    //        //    commandCol.ShowSelectCheckbox = true;
    //        //    commandCol.VisibleIndex = 0;
    //        //    commandCol.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
    //        //    commandCol.ShowClearFilterButton = true;
    //        //    gridLookupOptions.Columns.Insert(0, commandCol);
    //        //}
    //        //int index = (container as GridViewBatchEditItemTemplateContainer).VisibleIndex;
    //        //if (index != -1)
    //        //    gridLookupOptions.Value = _gridView.GetRowValues(index, "Options").ToString();

    //    }
    //}

    protected void testgrid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        if (e.ButtonID != "Clone") return;
        ASPxGridView g = (ASPxGridView)sender;
        object keyValue = g.GetRowValues(e.VisibleIndex, g.KeyFieldName);
        using (SqlConnection con = new SqlConnection(cs.strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spcopy";
            cmd.Parameters.AddWithValue("@Id", keyValue);
            cmd.Parameters.AddWithValue("@user", Page.Session["user_name"].ToString());
            cmd.Parameters.AddWithValue("@num", 1);
            cmd.Connection = con;
            con.Open();
            var getValue = cmd.ExecuteScalar();
            con.Close();
            g.JSProperties["cpSelectedRowKey"] = (getValue == null) ? string.Empty : getValue.ToString();
        }
    }

    protected void CmbMaterialType_Callback(object sender, CallbackEventArgsBase e)
    {
        ASPxComboBox combobox = (ASPxComboBox)sender;
        var args = e.Parameter.Split('|');
        switch (args[0])
        {
            case "c":case "reload":
                //cbCustomer.DataSource = dt;
                CmbMaterialType.DataBind();
                if (args[1].ToString() != "")
                {
                    combobox.SelectedIndex = combobox.Items.IndexOf(combobox.Items.FindByValue(args[1]));
                    //for (int i = 0; i < cbMaterialType.Items.Count; i++)
                    //{
                    //    if (hMaterialType["MaterialType"].ToString().Contains(cbMaterialType.Items[i].Value.ToString()))
                    //        cbMaterialType.Items[i].Selected = true;
                    //}
                }
                break;
        }
    }

    protected void gv_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
    {
        foreach (var args in e.InsertValues)
        {
            if (_testTable != null)
            {
                DataRow row = _testTable.NewRow();
                args.NewValues["Id"] = GetNewId(_testTable);
                args.NewValues["StatusTemp"] = "Add";
                args.NewValues["RequestNo"] = Convert.ToInt32(hGeID["GeID"]);
                IDictionaryEnumerator enumerator = args.NewValues.GetEnumerator();
                enumerator.Reset();
                while (enumerator.MoveNext())
                    if (enumerator.Key.ToString() != "Count")
                        row[enumerator.Key.ToString()] = enumerator.Value;
                _testTable.Rows.Add(row);
            }
        }
        foreach (var args in e.UpdateValues)
        {
            //Byte[] image = e.NewValues["Picture"] as byte[];
            var values = new[] { "Id", "RequestNo", "ActiveDate" };
            if (_testTable != null)
            {
                DataRow dr = _testTable.Rows.Find(args.Keys["Id"]);
                foreach (DataColumn column in _testTable.Columns)
                {
                    if (!values.Any(column.ColumnName.Equals))
                    {
                        if (args.NewValues["ProductionDate"] == null && column.ColumnName == "ProductionDate")
                            dr["ProductionDate"] = DateTime.Today;
                        else if (args.NewValues["ReceivedDate"] == null && column.ColumnName == "ReceivedDate")
                            dr["ReceivedDate"] = DateTime.Today;
                        else
                            dr[column.ColumnName] = args.NewValues[column.ColumnName];
                    }
                }
            }
        }
        e.Handled = true;
    }
}