using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_EditForms_SystCodeForm : MasterUserControl
{
    MyDataModule cs = new MyDataModule();
    public List<GridDataItem> listItems
    {
        get
        {
            var obj = this.Session["myList"];
            if (obj == null) { obj = this.Session["myList"] = new List<GridDataItem>(); }
            return (List<GridDataItem>)obj;
        }

        set
        {
            this.Session["myList"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session.Clear();
            hGeID["GeID"] = string.Format("{0}", 0);
            Session["SubID"] = 0;
        }
        Update();
    }
    public override void Update()
    {
        //gridData.DataSource = dsgv;
        //DataTable dt = ((DataView)dsgv.Select(DataSourceSelectArguments.Empty)).Table;
        testgrid.DataBind();
    }

    protected void TreeList_CustomJSProperties(object sender, TreeListCustomJSPropertiesEventArgs e)
    {
        ASPxTreeList treeList = sender as ASPxTreeList;
        Hashtable nameTable = new Hashtable();
        foreach (TreeListNode node in treeList.GetVisibleNodes())
            nameTable.Add(node.Key, string.Format("{0} {1}", node["FirstName"], node["LastName"]));
        e.Properties["cpEmployeeNames"] = nameTable;
    }

    protected void testgrid_DataBinding(object sender, EventArgs e)
    {
        (sender as ASPxGridView).DataSource = GetDataSource();
        AddColumns();
    }
    private SqlDataSource GetDataSource()
    {
        object o = selectedDataSource;
        switch (string.Format("{0}", Request.QueryString["param"]))
        {
            case "ContainerLidType"://ContainerLid
                ds.SelectCommand = "select Code,ContainerType,LidType from transContainerLid";
                return ds;
            case "Grade":
                ds.SelectCommand = "select Code,Description,IsActive from transGrade";
                return ds;
            case "CustBrand":
                ds.SelectCommand = "select Code,Description,IsActive from transCustomerBrand";
                return ds;
            case "Analysis":
                return dsAnalysisItem;
            case "ProductStyle":
                ds.SelectCommand = "SELECT ID,ProductGroup,Code,Description,IsActive from transProductStyle";
                return ds;
            case "RawMaterial":
                ds.SelectCommand = "SELECT ID,ProductGroup, ProductType,Code,Description,IsActive from transRawMaterial";
                return ds;
            case "MediaType": 
                ds.SelectCommand = "SELECT ID,MediaType,ProductGroup,Code,Description,IsActive from transMediaType";
                ds.UpdateCommand = "Update transMediaType set ProductGroup=@ProductGroup,Code=@Code where Id=@id";
                ds.InsertCommand = "insert into transMediaType values(@ProductGroup,@Code,@MediaType)";
                ds.DeleteCommand = "Delete transMediaType Where ID=@ID";
                return ds;
            case "CanSize":
                ds.SelectCommand = "select ID,ProductGroup,Code,CanSize,PouchWidth,Type,NW,DWeight,Packaging from transCanSize";
                return ds;
            default:
                return null;
        }
    }
    private List<GridDataItem> buildData()
    {
        switch (string.Format("{0}", Request.QueryString["param"]))
        {
            case "MediaType":
                int SubID = 0;
                DataTable dt = cs.build_Datatable ("SELECT * from transMediaItems where SubID='" + SubID + "'");
                listItems = (from DataRow dr in dt.Rows
                              select new GridDataItem()
                              {
                                  ID = Convert.ToInt32(dr["ID"]),
                                  Componant = dr["Componant"].ToString(),
                                  Description = dr["Description"].ToString(),
                                  SubID = dr["SubID"].ToString(),
                              }).ToList();

                return listItems;
            default:
                return null;
        }
    }

    //protected void testgrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    //{
    //    ASPxGridView g = sender as ASPxGridView;
    //    if (string.IsNullOrEmpty(e.Parameters))
    //        return;
    //    var args = e.Parameters.Split('|');
    //    long id;
    //    if (!long.TryParse(args[1], out id))
    //        return;
    //    var result = new Dictionary<string, string>();
    //    DataTable dt = new DataTable();

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

        }
    }
    string selectedDataSource
    {
        get { return Page.Session["selectedDataSource"] == null ? String.Empty : Page.Session["selectedDataSource"].ToString(); }
        set { Page.Session["selectedDataSource"] = value; }
    }

    private void AddColumns()
    {
        testgrid.Columns.Clear();
        if (GetDataSource() == null) return;
        DataView dw = (DataView)GetDataSource().Select(DataSourceSelectArguments.Empty);
        var values = new[] { "From", "To", "Validto" };
        foreach (DataColumn c in dw.Table.Columns)
        {
            var str = c.ColumnName;
            if (c.ColumnName.Contains("ProdGroup"))
            {
                GridViewDataColumn dc = new GridViewDataColumn();
                dc.Width = Unit.Percentage(90);
                dc.FieldName = c.ColumnName;
                dc.EditItemTemplate = new MyRadioButtonList(dsProdGroup);
                testgrid.Columns.Add(dc);
            }            
            else if (c.ColumnName.Contains("MediaType"))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = c.ColumnName;
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.Columns.Add(new ListBoxColumn("Code"));
                cb.PropertiesComboBox.Columns.Add(new ListBoxColumn("Description"));
                cb.PropertiesComboBox.ValueField = "Code";
                cb.PropertiesComboBox.TextFormatString = "{1}";
                cb.PropertiesComboBox.DataSource = dsMediaType;
                testgrid.Columns.Add(cb);
            }
            /*else if (c.ColumnName.Contains("ProductGroup"))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = c.ColumnName;
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.Columns.Add(new ListBoxColumn("ProductGroup"));
                cb.PropertiesComboBox.Columns.Add(new ListBoxColumn("Name"));
                cb.PropertiesComboBox.ValueField = "ProductGroup";
                cb.PropertiesComboBox.TextFormatString = "{1}";
                cb.PropertiesComboBox.DataSource = dsProdGroup;
                testgrid.Columns.Add(cb);
            }*/
            else if (c.ColumnName.Contains("MarginCode") || c.ColumnName.Contains("PercentMargin") || c.ColumnName.Contains("LBCode"))
            {
                GridViewDataTextColumn tc = new GridViewDataTextColumn();
                tc.FieldName = c.ColumnName;
                tc.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                testgrid.Columns.Add(tc);
            }
            else if (c.ColumnName.Contains("Description"))
            {
                GridViewDataTokenBoxColumn tb = new GridViewDataTokenBoxColumn();
                tb.FieldName = c.ColumnName;
                tb.PropertiesTokenBox.TextField = "Name";
                tb.PropertiesTokenBox.EnableCallbackMode = true;
                tb.PropertiesTokenBox.CallbackPageSize = 10;
                tb.PropertiesTokenBox.TextFormatString = "{0}";
                tb.PropertiesTokenBox.DataSource = dsAnalysisItem;
                testgrid.Columns.Add(tb);
            }
            else if (c.ColumnName.Contains("IsResign") || c.ColumnName.Contains("IsActive"))
            {
                GridViewDataCheckColumn cc = new GridViewDataCheckColumn();
                cc.FieldName = c.ColumnName;
                testgrid.Columns.Add(cc);
            }
            else
                AddTextColumn(c.ColumnName);
        }
        testgrid.KeyFieldName = dw.Table.Columns[0].ColumnName;
        testgrid.Columns[0].Visible = false;
        AddCommandColumn();
    }
    private void AddTextColumn(string fieldName)
    {
        GridViewDataTextColumn c = new GridViewDataTextColumn();
        c.FieldName = fieldName;
        testgrid.Columns.Add(c);
    }
    private void AddCommandColumn()
    {
        SqlDataSource ds = (SqlDataSource)testgrid.DataSource;
        bool showColumn = !(String.IsNullOrEmpty(ds.UpdateCommand) && String.IsNullOrEmpty(ds.InsertCommand) &&
            String.IsNullOrEmpty(ds.DeleteCommand));

        if (showColumn)
        {
            GridViewCommandColumn c = new GridViewCommandColumn();
            testgrid.Columns.Add(c);
            //c.Name = "Editar";
            //c.Caption = "Editar";
            c.VisibleIndex = 0;
            c.ShowEditButton = !String.IsNullOrEmpty(ds.UpdateCommand);
            c.Width = 50;
            //c.ShowNewButtonInHeader = !String.IsNullOrEmpty(ds.InsertCommand);
            //c.ShowDeleteButton = !String.IsNullOrEmpty(ds.DeleteCommand);
            c.ShowCancelButton = true;
            c.ShowUpdateButton = true;
            //c.ButtonRenderMode = GridCommandButtonRenderMode.Image;
        }
        //grid.SettingsCommandButton.NewButton.Image.Url = "~/Content/images/AddRecord.gif";
        //grid.SettingsCommandButton.EditButton.Image.Url = "~/Content/images/Edit.gif";
        //grid.SettingsCommandButton.UpdateButton.Image.Url = "~/Content/images/update.png";
        //grid.SettingsCommandButton.CancelButton.Image.Url = "~/Content/images/cancel.png";
        //grid.SettingsEditing.Mode = GridViewEditingMode.Batch;
    }

    protected void testgrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        DataRow dr = g.GetDataRow(g.FocusedRowIndex);
        if (string.IsNullOrEmpty(e.Parameters))
            return;
        var args = e.Parameters.Split('|');
        if (args[0] == "reload")
        {
            selectedDataSource = string.Format("{0}", args[1].ToString());
        }
        else if (args[0] == "SaveMail" || args[0] == "New")
        {
            using (SqlConnection con = new SqlConnection(cs.strConn))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                object o = selectedDataSource;
                switch (string.Format("{0}", Request.QueryString["param"]))
                {
                    case "RawMaterial":
                        cmd.CommandText = @"insert into transRawMaterial(ProductGroup,ProductType,Code,Description,IsActive" +
                            ")values (@ProductGroup, @ProductType,@Code,@Description,0)";
                        cmd.Parameters.AddWithValue("@ProductGroup", cmbProdGroup.Value);
                        cmd.Parameters.AddWithValue("@ProductType", string.Format("{0}", cmbProductType.Value));
                        cmd.Parameters.AddWithValue("@Code", string.Format("{0}", txtCode.Text));
                        cmd.Parameters.AddWithValue("@Description", string.Format("{0}", txtDescription.Text));
                        break;
                    case "ProductStyle":
                        cmd.CommandText = @"insert into transProductStyle(ProductGroup,Code,Description,IsActive" +
                            ")values (@ProductGroup,@Code,@Description,0)";
                        cmd.Parameters.AddWithValue("@ProductGroup", cmbProductGroup.Value);
                        cmd.Parameters.AddWithValue("@Code", string.Format("{0}", txtProductCode.Text));
                        cmd.Parameters.AddWithValue("@Description", string.Format("{0}", txtProductDesc.Text));
                        break;
                    case "MediaType":
                        cmd.CommandText = @"insert into transMediaType(MediaType,ProductGroup,Code,Description,IsActive" +
                            ")values (@MediaType,@ProductGroup,@Code,@Description,0)" +
                            "(SELECT CAST(scope_identity() AS int))";
                        cmd.Parameters.AddWithValue("@MediaType", cmbMediaType.Value);
                        cmd.Parameters.AddWithValue("@ProductGroup", cmbMediaProd.Value);
                        cmd.Parameters.AddWithValue("@Code", string.Format("{0}", ""));
                        cmd.Parameters.AddWithValue("@Description", string.Format("{0}", ""));
                        break;
                }
                cmd.Connection = con;
                con.Open();
                if(Request.QueryString["param"].ToString()== "MediaType") {
                var getValue = cmd.ExecuteScalar();
                    savelist(getValue.ToString());
                }
                else
                    cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        else
        {
            g.Columns.Clear();
            g.AutoGenerateColumns = false;
        }
        g.DataBind();
    }
    void savelist(string Id)
    {
        foreach (var items in listItems)
        {
            using (SqlConnection con = new SqlConnection(cs.strConn))
            {
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into transMediaItems values(@Componant,@Description,@SubID)";
                cmd.Parameters.Add("@Componant", SqlDbType.NVarChar).Value = items.Componant.ToString();
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = string.Format("{0}", items.Description);
                cmd.Parameters.Add("@SubID", SqlDbType.NVarChar).Value = string.Format("{0}", Id);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
    protected void testgrid_DataBound(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        if (g.VisibleRowCount > 0){
            //DataTable table = (DataTable)g.DataSource;
            //if (table != null)
            //    if (table.Rows.Count > 0)
            //    {
            //        g.StartEdit(0);
            //    }
            g.StartEdit(0);
        }
    }
    class MyRadioButtonList : ITemplate
    {
        SqlDataSource dsSqlData;
        public MyRadioButtonList(SqlDataSource dsSql)
        {
            this.dsSqlData = dsSql;
        }
        private DataTable data()
        {
            DataTable table = ((DataView)this.dsSqlData.Select(DataSourceSelectArguments.Empty)).Table;
            return table;
        }
        public void InstantiateIn(Control container)
        {
            //var args = this.name.Split('|');
            //int index = Array.IndexOf(args[2].Split(';'), args[3]);
            //index = index == -1 ? 0 : index;
            //DataTable dtDados = new DataTable();
            //dtDados.Columns.Add("chave");
            //dtDados.Columns.Add("value");
            //var arr = args[2].Split(';');
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    string s = arr[i];
            //    dtDados.Rows.Add(i, arr[i]);
            //}
            GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
            ASPxRadioButtonList ovRBL = new ASPxRadioButtonList();
            ovRBL.ID = "MyRadioButtonList1";
            container.Controls.Add(ovRBL);
            ovRBL.ClientIDMode = ClientIDMode.Static;
            ovRBL.Width = Unit.Percentage(100);
            ovRBL.Border.BorderStyle = BorderStyle.None;
            ovRBL.Paddings.Padding = Unit.Pixel(0);
            //ovRBL.RepeatColumns = dtDados.Rows.Count;
            ovRBL.ValueField = "ProductGroup";
            ovRBL.TextField = "Name";
            //ovRBL.ValueField = "chave";
            //ovRBL.TextField = "valor";
            //ovRBL.Attributes.Add("campo", "campo1");
            ovRBL.DataSource = data();
            ovRBL.DataBind();
            ovRBL.ClientSideEvents.SelectedIndexChanged = "OnSelectedIndexChanged";
            //ovRBL.ClientSideEvents.Init = "function(s,e){ s.SetSelectedIndex(" + index + "); }";
            //ovRBL.Value = "<%# Bind('"+ name +"') %>";
            //ovRBL.Value = "<%# Eval('" + this.name + "')%>";
            //container.Controls.Add(ovRBL);
        }
    }

    protected void testgrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {

    }

    protected void testgrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {

    }

    protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        DataRow dr = g.GetDataRow(g.FocusedRowIndex);
        if (string.IsNullOrEmpty(e.Parameters))
            return;
        var args = e.Parameters.Split('|');
        if (args[0] == "reload")
        {
            buildData();
            //    selectedDataSource = string.Format("{0}", args[1].ToString());
        }
        //else
        //{
        //    g.Columns.Clear();
        //    g.AutoGenerateColumns = false;
        //}
        g.DataBind();
    }

    protected void gv_DataBinding(object sender, EventArgs e)
    {
        (sender as ASPxGridView).DataSource = listItems;
    }
    public class GridDataItem
    {
        public int ID { get; set; }
        public string Componant { get; set; }
        public string Description { get; set; }
        public string SubID { get; set; }
    }
    protected void LoadNewValues(GridDataItem item, OrderedDictionary values)
    {
        item.Componant = Convert.ToString(values["Componant"]);
        item.Description = Convert.ToString(values["Description"]);
    }
    protected void gv_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
    {
        foreach (var args in e.InsertValues)
        {
            int NextRowID = 0;
            if (listItems.Count > 0)
                NextRowID = listItems.Max(t => t.ID);
            GridDataItem dr = new GridDataItem();
            NextRowID++;
            dr.ID = NextRowID;
            LoadNewValues(dr, args.NewValues);
            listItems.Add(dr);
        }
        foreach (var args in e.UpdateValues)
        {
            var dr = listItems.FirstOrDefault(x => x.ID == Convert.ToInt32(args.Keys["ID"]));
            LoadNewValues(dr, args.NewValues);
        }
            e.Handled = true;
    }

    class MyComboBoxTemplate : ITemplate
    {
        private List<string> data()
        {
            //MyDataModule mycla = new MyDataModule();
            //DataTable table = mycla.build_Datatable("select * from dbo.FNC_SPLIT('RawMaterial,ProductStyle,MediaType',',')");
            //List<string> list = table.AsEnumerable()
            //               .Select(r => r.Field<string>("value"))
            //               .ToList();
            //return list;
            return new List<string>(new string[] { "Mix","Topping", "None" });
        }
        public void InstantiateIn(Control container)
        {
            ASPxListBox lst = new ASPxListBox();
            //GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
            lst.ID = "combo";
            lst.DataSource = data();
            lst.Border.BorderWidth = 0;
            lst.SelectionMode = ListEditSelectionMode.CheckColumn;
            //combo.DropDownStyle = DropDownStyle.DropDown;
            //combo.IncrementalFilteringMode = IncrementalFilteringMode.StartsWith;
            container.Controls.Add(lst);
        }
    }
}