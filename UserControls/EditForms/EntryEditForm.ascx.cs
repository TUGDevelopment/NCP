using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_EditForms_EntryEditForm : System.Web.UI.UserControl
{
    MyDataModule cs = new MyDataModule();
    string user_name = HttpContext.Current.User.Identity.Name.Replace(@"THAIUNION\", @"");
    //String user_name = HttpContext.Current.Request.LogonUserIdentity.Name.Replace(@"THAIUNION\", @"");
    string strConn = ConfigurationManager.ConnectionStrings["ncpDbConnectionString"].ConnectionString;
    private DataTable testTable
    {
        get { return Session["sessionKey"] == null ? null : (DataTable)Session["sessionKey"]; }
        set { Session["sessionKey"] = value; }
    }
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
            selectedDataSource = string.Format("{0}", "");
        }
        grid.DataBind();
    }
    protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        object o = selectedDataSource;
        if (o == null) return;
        if (string.IsNullOrEmpty(e.Parameters))
            return;
        var args = e.Parameters.Split('|');
        if (args[0] == "reload")
        {
            g.DataSource = null;
        }
        g.Columns.Clear();
        g.AutoGenerateColumns = false;
        g.DataBind();
    }
    protected void grid_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        g.KeyFieldName = "ID";
        g.DataSource = GetDataSource(); ;
        AddColumns();
        g.ForceDataRowType(typeof(DataRow));

    }
    private void AddColumns()
    {
        grid.Columns.Clear();
        DataView dw = (DataView)GetDataSource().Select(DataSourceSelectArguments.Empty);
        if (dw == null)
            return;
        foreach (DataColumn c in dw.Table.Columns)
        {
            var str = c.ColumnName;
            var args = str.Split('|');
            //GridViewDataColumn col = new GridViewDataTextColumn();
            if (c.ColumnName.Contains("Combobox"))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = c.ColumnName;
                cb.Caption = args[0];
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.TextField = "value";
                cb.PropertiesComboBox.ValueField = "value";
                cb.PropertiesComboBox.TextFormatString = "{0}";
                cb.PropertiesComboBox.DataSource = dstest;
                grid.Columns.Add(cb);
            }
            else if (c.ColumnName.Contains("CheckBox"))
            {
                GridViewDataCheckColumn cc = new GridViewDataCheckColumn();
                cc.FieldName = c.ColumnName;
                cc.Caption = args[0];
                grid.Columns.Add(cc);
            }
            else if (c.ColumnName.Contains("Name"))
            {
                GridViewDataTextColumn tc = new GridViewDataTextColumn();
                tc.FieldName = c.ColumnName;
                tc.Width = Unit.Pixel(230);
                tc.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                grid.Columns.Add(tc);
            }
            else
                AddTextColumn(c.ColumnName);
        }
        grid.KeyFieldName = dw.Table.Columns[0].ColumnName;
        grid.Columns[0].Visible = false;
        AddCommandColumn();
    }
    private void AddTextColumn(string fieldName)
    {
        var args = fieldName.Split('|');
        GridViewDataTextColumn c = new GridViewDataTextColumn();
        c.FieldName = fieldName;
        c.Caption = args[0];
        grid.Columns.Add(c);
    }
    private void AddCommandColumn()
    {
        SqlDataSource ds = (SqlDataSource)grid.DataSource;
        bool showColumn = !(String.IsNullOrEmpty(ds.UpdateCommand) && String.IsNullOrEmpty(ds.InsertCommand) &&
            String.IsNullOrEmpty(ds.DeleteCommand));
        //if (showColumn)
        //{
        GridViewCommandColumn c = new GridViewCommandColumn();
        grid.Columns.Add(c);
        c.Width = 50;
        c.VisibleIndex = 0;
        c.ShowEditButton = true;
        c.ShowCancelButton = true;
        c.ShowUpdateButton = true;
    }
    private SqlDataSource GetDataSource()
    {
        object o = selectedDataSource;
        return dsgv;
    }
    protected void EntryEditPopup_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
    {

    }
    protected void OnCallback(object source, CallbackEventArgs e)
    {
        Control resControl = null;
        if (!string.IsNullOrEmpty(e.Parameter)) return;
    }
    protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        var str = e.Column.FieldName;
        var args = str.Split('|');
        if (e.Editor is ASPxTextBox || e.Editor is ASPxComboBox)
        {
            e.Editor.Width = Unit.Pixel(230);
        }
    }
    protected void grid_DataBound(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        if (grid.VisibleRowCount > 0)
            g.StartEdit(0);
    }
}