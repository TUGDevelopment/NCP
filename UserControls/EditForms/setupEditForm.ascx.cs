using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_EditForms_setupEditForm : System.Web.UI.UserControl
{
    MyDataModule cs = new MyDataModule();
    //string user_name = HttpContext.Current.User.Identity.Name.Replace(@"THAIUNION\", @"");
    string strConn = ConfigurationManager.ConnectionStrings["ncpDbConnectionString"].ConnectionString;
    string selectedDataSource
    {
        get { return Session["selectedDataSource"] == null ? String.Empty : Session["selectedDataSource"].ToString(); }
        set { Session["selectedDataSource"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Session.Clear();
            selectedDataSource = string.Format("{0}", Request.QueryString["viewMode"]);
        }
        gvResult.DataBind();
    }

    protected void gvResult_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        DataRow dr = g.GetDataRow(g.FocusedRowIndex);
        //Session["selectedDataSource"] = Int32.Parse(e.Parameters);
        if (string.IsNullOrEmpty(e.Parameters))
            return;
        g.Columns.Clear();
        g.AutoGenerateColumns = false;
        g.DataBind();
    }

    protected void gvResult_FillContextMenuItems(object sender, DevExpress.Web.ASPxGridViewContextMenuEventArgs e)
    {

    }

    protected void gvResult_ContextMenuItemClick(object sender, DevExpress.Web.ASPxGridViewContextMenuItemClickEventArgs e)
    {
        //switch (e.Item.Name)
        //{
        //    case "ExportToXLS":
        //        GridExporter.WriteXlsToResponse();
        //        break;
        //    case "XLS":
        //        ExportToDel(e.Item.Name);
        //        break;
        //}
    }

    protected void gvResult_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
    {
        if (e.Column.FieldName == "Plant")
        {
            ASPxDropDownEdit dropDownEdit = (ASPxDropDownEdit)e.Editor;
            dropDownEdit.ClientInstanceName = "dropDownEdit";
            string[] indexes = dropDownEdit.Text.Split(',');

            ASPxListBox listBox = (ASPxListBox)dropDownEdit.FindControl("ASPxListBox1");
            listBox.ClientInstanceName = "checkListBox";
            listBox.ClientSideEvents.SelectedIndexChanged = String.Format("function(s,e){{OnListBoxSelectionChanged(s,e,{0});}}", dropDownEdit.ClientID);
            dropDownEdit.ClientSideEvents.TextChanged = String.Format("function(s,e){{ SynchronizeListBoxValues(s,e,{0});}}", listBox.ClientID);
            dropDownEdit.ClientSideEvents.DropDown = String.Format("function(s,e){{ SynchronizeListBoxValues(s,e,{0});}}", listBox.ClientID);
            if (listBox == null) return;
            foreach (ListEditItem item in listBox.Items)
            {
                if (indexes.Contains<string>(item.Value.ToString()))
                    item.Selected = true;
            }
        }
    }

    protected void gvResult_DataBinding(object sender, EventArgs e)
    {
        (sender as ASPxGridView).DataSource = GetDataSource();
        AddColumns();
    }
    private void AddColumns()
    {
        gvResult.Columns.Clear();
        DataView dw = (DataView)GetDataSource().Select(DataSourceSelectArguments.Empty);
        if (dw == null)
            return;
        foreach (DataColumn c in dw.Table.Columns)
        {
            var str = c.ColumnName;
            //GridViewDataColumn col = new GridViewDataTextColumn();
            if (c.ColumnName.Contains("IsActive"))
            {
                GridViewDataCheckColumn cc = new GridViewDataCheckColumn();
                cc.FieldName = c.ColumnName;
                cc.Width = Unit.Percentage(20);
                gvResult.Columns.Add(cc);
            }
            else if (c.ColumnName.Contains("GroupingID") || c.ColumnName.Contains("InstallArea"))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = c.ColumnName;
                cb.Width = 330;
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.Columns.Add(new ListBoxColumn("shortname"));
                cb.PropertiesComboBox.Columns.Add(new ListBoxColumn("Name"));
                cb.PropertiesComboBox.ClientSideEvents.EndCallback = "cb_OnEndCallback";
                cb.PropertiesComboBox.ValueField = "ID";
                cb.PropertiesComboBox.TextField = "Name";
                cb.PropertiesComboBox.TextFormatString = "{0}: {1}";

                cb.PropertiesComboBox.DataSource =(DataTable)buildData(c.ColumnName);
                gvResult.Columns.Add(cb);
            }
            else if (c.ColumnName.Contains("Area"))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = c.ColumnName;
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.TextField = "value";
                cb.PropertiesComboBox.TextFormatString = "{0}";
                cb.PropertiesComboBox.DataSource = dsZone;
                gvResult.Columns.Add(cb);
            }
            else if (c.ColumnName.Contains("useType"))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = c.ColumnName;
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.TextField = "value";
                cb.PropertiesComboBox.TextFormatString = "{0}";
                cb.PropertiesComboBox.DataSource = dscontest;
                gvResult.Columns.Add(cb);
            }
            else if (c.ColumnName.Equals("Plant"))
            {
                GridViewDataDropDownEditColumn col = new GridViewDataDropDownEditColumn();
                col = new GridViewDataDropDownEditColumn();
                ((DropDownEditProperties)col.PropertiesEdit).DropDownWindowTemplate = new MyDropDownWindow();
                col.FieldName = c.ColumnName;
                gvResult.Columns.Add(col);

            }
            else if (c.ColumnName.Equals("PlantID"))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = c.ColumnName;
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.Columns.Add(new ListBoxColumn("shortname"));
                cb.PropertiesComboBox.Columns.Add(new ListBoxColumn("Name"));
                cb.PropertiesComboBox.ValueField = "Id";
                cb.PropertiesComboBox.TextField = "Name";
                cb.PropertiesComboBox.TextFormatString = "{0}: {1}";
                cb.PropertiesComboBox.ClientSideEvents.SelectedIndexChanged += "function(s, e) { OnareaChanged(s); }";
                DataTable _t= ((DataView)dsPlant.Select(DataSourceSelectArguments.Empty)).Table;
                _t.PrimaryKey = new DataColumn[] { _t.Columns["Id"] };
                cb.PropertiesComboBox.DataSource = _t;
                gvResult.Columns.Add(cb);
            }
            else
                AddTextColumn(c.ColumnName);
        }
        gvResult.KeyFieldName = dw.Table.Columns[0].ColumnName;
        gvResult.Columns[0].Visible = false;
        AddCommandColumn();
    }
    private void AddTextColumn(string fieldName)
    {
        GridViewDataTextColumn c = new GridViewDataTextColumn();
        c.FieldName = fieldName;
        c.Width = Unit.Percentage(fieldName == "Name" ? 50 : 20);
        gvResult.Columns.Add(c);
    }
    private void AddCommandColumn()
    {
        SqlDataSource ds = (SqlDataSource)gvResult.DataSource;
        bool showColumn = !(String.IsNullOrEmpty(ds.UpdateCommand) && String.IsNullOrEmpty(ds.InsertCommand) &&
            String.IsNullOrEmpty(ds.DeleteCommand));

        if (showColumn)
        {
            GridViewCommandColumn c = new GridViewCommandColumn();
            gvResult.Columns.Add(c);
            c.Width = 50;
            c.VisibleIndex = 0;
            c.ShowEditButton = !String.IsNullOrEmpty(ds.UpdateCommand);
            //c.ShowNewButtonInHeader = !String.IsNullOrEmpty(ds.InsertCommand);
            c.ShowDeleteButton = !String.IsNullOrEmpty(ds.DeleteCommand);
            c.ShowCancelButton = true;
            c.ShowUpdateButton = true;
            //c.ButtonRenderMode = GridCommandButtonRenderMode.Image;
        }
        //grid.SettingsCommandButton.EditButton.Image.Url = "~/Content/images/Edit.gif";
        //grid.SettingsCommandButton.UpdateButton.Image.Url = "~/Content/images/update.png";
        //grid.SettingsCommandButton.CancelButton.Image.Url = "~/Content/images/cancel.png";
    }
    DataTable buildData(string _name)
    {
        DataTable table = new DataTable(); DataView dv;
        if (_name== "InstallArea")
            dv = (DataView)dsArea.Select(DataSourceSelectArguments.Empty);
        else
            dv = (DataView)dsGrouping.Select(DataSourceSelectArguments.Empty);

        if (dv != null)
        {
            dv.RowFilter = "IsActive=0";
            table = dv.ToTable();
            table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };
        }
        return table; 
    }
    private SqlDataSource GetDataSource()
    {
        object o = selectedDataSource;
        if (o == null)
            return dsGrouping;
        switch (string.Format("{0}", o))
        {
            case "area":
                return dsArea;
            case "param":
                return dsparam;
            case "InstallArea":
                return dsInstallArea;
            case "user":
                return dsUser;
            case "Plant":
                return dsPlant;
            case "Supplier":
                return dsSupplier;
            case "Packaging":
                return dsPackaging;
			 case "group":
                return dsGrouping;
            case "Problem":
                return dsProblem;
            case "location":
                return dslocation;
            case "NCPType":
                return dsNCPtype;
            default: 
                return SqlDataSource1;
            
        }
    }
    protected void gvResult_DataBound(object sender, EventArgs e)
    {

    }

    protected void gvResult_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {

    }

    protected void gvResult_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {

    }
    protected void gvResult_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {

    }

    public class MyDropDownWindow : ITemplate
    {
        void ITemplate.InstantiateIn(Control container)
        {
            ASPxListBox lb = new ASPxListBox();
            lb.ID = "ASPxListBox1";
            container.Controls.Add(lb);

            lb.Width = Unit.Percentage(50);
            lb.SelectionMode = ListEditSelectionMode.CheckColumn;
            //lb.ClientSideEvents.SelectedIndexChanged = "function(s,e) { OnSelectedIndexChanged(s,e); }";
            lb.DataBinding += lb_DataBinding;
        }
        void lb_DataBinding(object sender, EventArgs e)
        {
            ASPxListBox lb = (ASPxListBox)sender;
            lb.DataSource = GetDataSource();
        }
        private List<string> GetDataSource()
        {
            MyDataModule mycla = new MyDataModule();
            DataTable table = mycla.build_Datatable("select Title from tblplant");
            List<string> list = table.AsEnumerable()
                           .Select(r => r.Field<string>("Title"))
                           .ToList();
            return list;

        }
    }
}