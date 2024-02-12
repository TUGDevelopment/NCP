using ClosedXML.Excel;
using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Data;
using DevExpress.Web.ASPxSpreadsheet;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data.SqlClient;
using DevExpress.Data.Filtering;
using DevExpress.CodeParser;

public partial class UserControls_EditForms_ReportForm : System.Web.UI.UserControl
{
    MyDataModule mycs = new MyDataModule();
    ServiceCS cs = new ServiceCS();
    string user_name = HttpContext.Current.User.Identity.Name.Replace(@"THAIUNION\", @"");
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Update();
            DateTime today = DateTime.Today;
            DateTime b = DateTime.Now.AddDays(-30);
            filter.FilterExpression = string.Format("[IssueDate] Between(#{0}#, #{1}#)",
                    b.ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd"));
        }

    }
    public void Update()
    {
        //gridData.DataSource = dsgv;
        //DataTable dt = ((DataView)dsgv.Select(DataSourceSelectArguments.Empty)).Table;
        hKeyword["Expr"] = string.Format("{0}", fExpr);
        hGeID["GeID"] = string.Format("{0}", 0);
        testgrid.DataBind();
        ApplyLayout(0);
    }
    void ApplyLayout(int layoutIndex)
    {
        testgrid.BeginUpdate();
        try
        {
            testgrid.ClearSort();
            switch (layoutIndex)
            {
                case 0:
                    //testgrid.GroupBy(testgrid.Columns["Grouping"]);
                    //testgrid.GroupBy(testgrid.Columns["IssueDate"]);
                    break;
                //case 1:
                //    testgrid.GroupBy(testgrid.Columns["Country"]);
                //    testgrid.GroupBy(testgrid.Columns["City"]);
                //    break;
                //case 2:
                //    testgrid.GroupBy(testgrid.Columns["CompanyName"]);
                //    break;
            }
        }
        finally
        {
            testgrid.EndUpdate();
        }
        testgrid.ExpandAll();
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        string SavedExpression =  filter.GetFilterExpressionForDataSet();
        using (SqlConnection con = new SqlConnection(mycs.strConn))
        {
            using (SqlCommand cmd = new SqlCommand("spUpdatefilter"))
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@text", SavedExpression.ToString());
                cmd.Parameters.AddWithValue("@user", user_name.ToString());
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
    protected void testgrid_DataBinding(object sender, EventArgs e)
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
            //if (testTable == null)
            //return testTable;
                DataView dv = (DataView)ds.Select(DataSourceSelectArguments.Empty);
                if (dv == null)
                return testTable;
            CreateGridColumns(dv);
                testTable = dv.Table;
            return testTable;
        }
    }
    void CreateGridColumns(DataView view)
    {
        testgrid.Columns.Clear();
        foreach (DataColumn c in view.Table.Columns)
        {
            var str = c.ColumnName;
            var args = str.Split('|');
            //GridViewDataColumn col = new GridViewDataTextColumn();
            //if (c.ColumnName.Contains("Combobox"))
            //{
            //    GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
            //    cb.FieldName = c.ColumnName;
            //    cb.Caption = args[0];
            //    cb.PropertiesComboBox.Columns.Clear();
            //    cb.PropertiesComboBox.TextField = "value";
            //    cb.PropertiesComboBox.ValueField = "value";
            //    cb.PropertiesComboBox.TextFormatString = "{0}";
            //    cb.PropertiesComboBox.DataSource = cs.builditems(string.Format("select value from dbo.FNC_SPLIT('{0}',';')", args[2])); 
            //    testgrid.Columns.Add(cb);
            //}
            if (c.ColumnName.Contains("Grouping")|| c.ColumnName.Contains("Plant") || c.ColumnName.Contains("InstallArea"))
            {
                GridViewDataComboBoxColumn cb = new GridViewDataComboBoxColumn();
                cb.FieldName = c.ColumnName;
                cb.Caption = args[0];
                cb.PropertiesComboBox.Columns.Clear();
                cb.PropertiesComboBox.TextField = "Name";
                cb.PropertiesComboBox.ValueField = "ID";
                cb.PropertiesComboBox.TextFormatString = "{0}";
                if (c.ColumnName == "Grouping") {
                    cb.Width = Unit.Percentage(20);
                    cb.PropertiesComboBox.DataSource = dsGrouping;}
                if (c.ColumnName == "Plant") {
                    cb.Width = Unit.Percentage(20);
                    cb.PropertiesComboBox.DataSource = dsPlant;}
                if (c.ColumnName == "InstallArea") {
                    cb.Width = Unit.Percentage(20);
                    cb.PropertiesComboBox.DataSource = dsArea;}
                //cb.PropertiesComboBox.DataSource = (DataTable)buildData(c.ColumnName); 
                testgrid.Columns.Add(cb);
            }
            else if (c.ColumnName.Contains("CheckBox"))
            {
                GridViewDataCheckColumn cc = new GridViewDataCheckColumn();
                cc.FieldName = c.ColumnName;
                cc.Caption = args[0];
                testgrid.Columns.Add(cc);
            }
            else if (c.ColumnName.Contains("Number"))
            {
                GridViewDataSpinEditColumn se = new GridViewDataSpinEditColumn();
                se.FieldName = c.ColumnName;
                se.Caption = args[0];
                se.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                testgrid.Columns.Add(se);
            }
            else if (c.ColumnName.Contains("Name"))
            {
                GridViewDataTextColumn tb = new GridViewDataTextColumn();
                tb.FieldName = c.ColumnName;
                tb.EditFormSettings.Visible = DevExpress.Utils.DefaultBoolean.False;
                testgrid.Columns.Add(tb);
            }
            else
                AddTextColumn(c.ColumnName, testgrid);
        }
        //grid.KeyFieldName = "ID";
        testgrid.KeyFieldName = view.Table.Columns[0].ColumnName;
        testgrid.Columns[0].Visible = false;
        //AddCommandColumn(grid);
    }
    private void AddTextColumn(string fieldName, ASPxGridView g)
    {
        var args = fieldName.Split('|');
        GridViewDataTextColumn c = new GridViewDataTextColumn();
        c.FieldName = fieldName;
        c.Width = Unit.Percentage(fieldName == "CreateOn" ? 20 : 10);
        c.Caption = args[0];
        c.CellStyle.HorizontalAlign = HorizontalAlign.Center;
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
    //SqlDataSource buildData(string _name)
    //{
    //    DataTable table = new DataTable();
    //    DataView dv;
    //    switch (_name)
    //    {
    //        case "InstallArea":
    //            dv = (DataView)dsArea.Select(DataSourceSelectArguments.Empty);
    //            break;
    //        case "Grouping":
    //            dv = (DataView)dsGrouping.Select(DataSourceSelectArguments.Empty);
    //            break;
    //        case "Plant":
    //            dv = (DataView)dsPlant.Select(DataSourceSelectArguments.Empty);
    //            break;
    //    }
    //    /if (dv != null)
    //    {
    //        dv.RowFilter = "IsActive=0";
    //        table = dv.ToTable();
    //        table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };
    //    }
    //    return table;
    //}

    protected void testgrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        if (string.IsNullOrEmpty(e.Parameters)) return;
        string[] args = e.Parameters.Split('|');
        //switch (args[0])
        //{
        //    case "ExportToXLS":
        //        export();
        //        break;
        //}
        if (args[0] == "filter")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(filter.GetFilterExpressionForMsSql());
            fExpr = sb.ToString();
            g.JSProperties["cpKeyValue"] = "filter|" + fExpr;
        }
        g.DataBind();
    }

    protected void testgrid_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
    {
        if (e.MenuType == GridViewContextMenuType.Rows)
        {
            var item = e.CreateItem("Export", "Export");
            item.BeginGroup = true;
            item.Image.IconID = "export_export_16x16";
            e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), item);
            e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Custom), item);
            AddMenuSubItem(item, "FAQCNA รายงานการตรวจนับจำนวน และเครื่องดักแมลง", "XLS",  "~/Content/Images/excel.gif", true);
            AddMenuSubItem(item, "FAQCNA  รายงานการตรวจสอบหนู 110362_Tuf", "XLS", "~/Content/Images/excel.gif", true);
            AddMenuSubItem(item, "F3QCPC17 Insectocutor Inspection Record", "XLS", "~/Content/Images/excel.gif", true);
        }
    }
    private static void AddMenuSubItem(GridViewContextMenuItem parentItem, string text, string name, string iconID, bool isPostBack)
    {
        var exportToXlsItem = parentItem.Items.Add(text, name);
        exportToXlsItem.Image.Url = iconID;
    }
    protected void testgrid_ContextMenuItemClick(object sender, ASPxGridViewContextMenuItemClickEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        switch (e.Item.Name)
        {
            case "ExportToXLS":
                export();
                break;
            case "XLS":
                exportto(e.Item.Text,g);
                break;
        }
    }
    void exportto(string name, ASPxGridView g)
    {

        ASPxSpreadsheet spreadsheet =  new ASPxSpreadsheet();
        XLWorkbook wb = new XLWorkbook();
        
        string path = "";
        DataTable dtdate = new DataTable("Product");
        dtdate.Columns.Add("id", typeof(int));
        dtdate.Columns.Add("IssueDate", typeof(string));
        dtdate.Columns.Add("coloumn", typeof(string));
        if (name == "FAQCNA รายงานการตรวจนับจำนวน และเครื่องดักแมลง")
        {
            path = Server.MapPath(@"~/ExcelFiles/F10B1907.xlsx");//F10B1907
            spreadsheet.Document.LoadDocument(path);
            Worksheet wsheet = spreadsheet.Document.Worksheets[0];
            string[] ar = { "J", "O", "T", "Y", "AD" };

            int irow = 10, o = 0, copyirow = irow, col = 0, erow = 26;
            string IssueDate = "";
            DataView dv = (DataView)dsGrouping.Select(DataSourceSelectArguments.Empty);
            g.SortBy(g.Columns["IssueDate"], ColumnSortOrder.Ascending);
            DataTable table = g.DataSource as DataTable;
            DataView view = new DataView(table);
            DataTable distinctValues = view.ToTable(true, "InstallArea");
            List<string> termsList = new List<string>();
            foreach (DataRow r in distinctValues.Rows)
            {
                var shortname = _result((string)r["InstallArea"], "shortname");
                var tarea = mycs.ReadItems(@"select Area from Mas_InstallArea where shortname='" + shortname.ToString() + "'");
                DataRow[] result = table.Select("InstallArea = '" + r["InstallArea"].ToString() + "' and Grouping=1");
                foreach (DataRow row in result)
                {
                    int x = distinctValues.Rows.IndexOf(r);
                    if (distinctValues.Rows.IndexOf(r) == 0)
                    {
                        foreach (DataColumn column in row.Table.Columns)
                        {
                            termsList.Add(column.ColumnName);
                        }
                        wsheet.Cells["D6"].Value = mycs.ReadItems(@"select Title from tblplant where Id='"
                        + row["Plant"].ToString() + "'");
                        DateTime myDate;
                        if (DateTime.TryParse(row["IssueDate"].ToString(), out myDate))
                        {
                            int monthNumber = myDate.Month;
                            int y = myDate.Year;
                            string monthName = new DateTimeFormatInfo().GetMonthName(monthNumber);
                            wsheet.Cells["AG6"].Value = monthName;
                            wsheet.Cells["AK6"].Value = y;
                        }
                    }
                    if ((string)row["IssueDate"] != IssueDate)
                    {
                        int max = dtdate.Rows.Count;
                        max++; DataRow dr = null;
                        if (dtdate.Rows.Count > 0)
                            dr = dtdate.Select("IssueDate='" + (string)row["IssueDate"] + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                        if (dr != null)
                        {
                            col = ColumnNameToNumber(dr["coloumn"].ToString());
                        }
                        else
                        {
                            dtdate.Rows.Add(max, (string)row["IssueDate"], ar[o]);
                            IssueDate = (string)row["IssueDate"];
                            if (o < ar.Length)
                            {
                                wsheet.Cells[ar[o] + "8"].Value = IssueDate;
                                col = ColumnNameToNumber(ar[o]);
                            }
                            else goto jumptoexit;
                            o++;
                        }

                    }
                    switch (tarea)
                    {
                        case "ENCLOSED":
                            wsheet.Cells["B" + irow].Value = shortname;
                            wsheet.Cells["C" + irow].Value = _result((string)row["InstallArea"], "Name");
                            if (col > 0)
                            {
                                wsheet.Cells[GetExcelColumnName(col + 0) + irow].Formula = string.Format("{0}", row["แมลงหวี่ (Moth Fly)|TextBox"]);
                                wsheet.Cells[GetExcelColumnName(col + 1) + irow].Formula = string.Format("{0}", row["ยุง (Mosquitoes)|TextBox"]);
                                wsheet.Cells[GetExcelColumnName(col + 2) + irow].Formula = string.Format("{0}", row["แมลงวัน (Fly)|TextBox"]);
                                wsheet.Cells[GetExcelColumnName(col + 3) + irow].Formula = string.Format("{0}", row["แมลงอื่น ๆ (Other)|TextBox"]);
                            }

                            break;
                        case "INTERNAL":
                            wsheet.Cells["B" + erow].Value = shortname;
                            wsheet.Cells["C" + erow].Value = _result((string)row["InstallArea"], "Name");
                            if (col > 0)
                            {
                                wsheet.Cells[GetExcelColumnName(col + 0) + erow].Formula = string.Format("{0}", row["แมลงหวี่ (Moth Fly)|TextBox"]);
                                wsheet.Cells[GetExcelColumnName(col + 1) + erow].Formula = string.Format("{0}", row["ยุง (Mosquitoes)|TextBox"]);
                                wsheet.Cells[GetExcelColumnName(col + 2) + erow].Formula = string.Format("{0}", row["แมลงวัน (Fly)|TextBox"]);
                                wsheet.Cells[GetExcelColumnName(col + 3) + erow].Formula = string.Format("{0}", row["แมลงอื่น ๆ (Other)|TextBox"]);
                            }

                            break;
                    }
                }
                if (tarea == "ENCLOSED")
                    irow++;
                if (tarea == "INTERNAL")
                    erow++;
                IssueDate = "";
                o = 0;
            }
        }
        else if (name == "FAQCNA  รายงานการตรวจสอบหนู 110362_Tuf")
        {
            path = Server.MapPath(@"~/ExcelFiles/FAQCNA_รายงานการตรวจสอบหนู_150762_Tuf.xlsx");
            //var _name = (Request.QueryString["Id"] == null) ? "FAQCSN11_7" : Request.QueryString["Id"].ToString();
            spreadsheet.Document.LoadDocument(path);
            //Spreadsheet.Open(Server.MapPath(@"~/ExcelFiles/" + _name + ".xlsx"));
            //Spreadsheet.Document.DocumentSettings.Calculation.Iterative = true;
            //worksheet = Spreadsheet.Document.Worksheets[0];
            //IWorkbook workbook = Spreadsheet.Document;
            //spreadsheet.WorkDirectory = "~/App_Data/WorkDirectory";
            //Get(Server.MapPath(@"~/ExcelFiles/" + name + ".xlsx"));

            //wb.Worksheets.Add(testTable, "WorksheetName");
            //wb.SaveAs(path);
            Worksheet ws;
            ws = spreadsheet.Document.Worksheets[0];
            int irow = 10, o = 0, copyirow = irow, col = 0;
            //string IssueDate = "";
            //foreach(DataRow r in testTable.Rows) {
            //}
            DataView dv = (DataView)dsGrouping.Select(DataSourceSelectArguments.Empty);
            g.SortBy(g.Columns["IssueDate"], ColumnSortOrder.Ascending);
            string[] ar = { "I", "L", "O", "R", "U" };
            DataTable table = g.DataSource as DataTable;
            DataView view = new DataView(table);
            DataTable distIssueDate = view.ToTable(true, "IssueDate");
            distIssueDate.DefaultView.Sort = "IssueDate ASC";
            var orderedRows = from row in distIssueDate.AsEnumerable()
                              orderby row.Field<DateTime>("IssueDate")
                              select row;
            DataTable tblOrdered = orderedRows.CopyToDataTable();
            //distIssueDate = distIssueDate.DefaultView.ToTable();
            foreach (DataRow r in tblOrdered.Rows)
            {
                if (o < ar.Length)
                    ws.Cells[ar[o] + "7"].Value = Convert.ToDateTime(r["IssueDate"]).ToString("dd-MM-yyyy");
                DateTime myDate;
                if (o == 0)
                    if (DateTime.TryParse(r["IssueDate"].ToString(), out myDate))
                    {
                        int monthNumber = myDate.Month;
                        string monthName = new DateTimeFormatInfo().GetMonthName(monthNumber);
                        ws.Cells["D5"].Value = monthName;
                    }
                o++;
            }
            DataTable distinctValues = view.ToTable(true, "InstallArea");
            List<string> termsList = new List<string>();
            foreach (DataRow r in distinctValues.Rows)
            {
                DataRow[] result = table.Select("InstallArea = '" + r["InstallArea"].ToString() + "' and Grouping=2");
                foreach (DataRow row in result)
                {
                    int x = distinctValues.Rows.IndexOf(r);
                    if (distinctValues.Rows.IndexOf(r) == 0)
                    {
                        foreach (DataColumn column in row.Table.Columns)
                        {
                            termsList.Add(column.ColumnName);
                        }
                        ws.Cells["D4"].Value = mycs.ReadItems(@"select Title from tblplant where Id='"
                        + row["Plant"].ToString() + "'");
                    }
                    //    if ((string)row["IssueDate"] != IssueDate)
                    //    {
                    //        int max = dtdate.Rows.Count;
                    //        max++; DataRow dr = null;
                    foreach (string value in ar)
                    {
                        var _d = ws.Cells[value + "7"].Value;
                        DateTime myDate;
                        if (DateTime.TryParse(_d.ToString(), out myDate))
                            if (myDate.ToString("dd-MM-yyyy") == Convert.ToDateTime(row["IssueDate"]).ToString("dd-MM-yyyy"))
                            {
                                col = ColumnNameToNumber(value.ToString());
                                break;
                            }
                    }
                    //        else
                    //        {
                    //            if (o == ar.Length) goto jumptoexit;
                    //            dtdate.Rows.Add(max, (string)row["IssueDate"], ar[o]);
                    //            IssueDate = (string)row["IssueDate"];
                    //            if (o < ar.Length)
                    //            {
                    //                ws.Cells[ar[o] + "7"].Value = IssueDate;
                    //                col = ColumnNameToNumber(ar[o]);
                    //            }
                    //            else goto jumptoexit;
                    //            o++;
                    //        }
                    //        irow = 10;
                    //    }
                    //    if (irow == copyirow)
                    //    {
                    //        ws.Rows[irow].Insert();
                    //        ws.Rows[irow].CopyFrom(ws.Rows[copyirow + 1]);
                    //        copyirow++;
                    //    }
                    ws.Cells["A" + irow].Value = _result((string)row["InstallArea"], "shortname");
                    ws.Cells["B" + irow].Value = _result((string)row["InstallArea"], "Name");
                    //ws.Cells["X" + irow].Formula = "=SUM(I"+ irow + ",L"+ irow + ",O"+ irow + ",R"+ irow + ",U"+ irow + ")";
                    string strsum = "SUM(I10,L10,O10,R10,U10)".ToString(new CultureInfo("en-US")).Replace("10", irow.ToString());
                    ws.Cells["X" + irow].Formula = string.Format("={0}", strsum, new CultureInfo("en-US"));

                    if (col > 0)
                    {
                        //foreach (DataColumn column in row.Table.Columns)
                        //{
                        //ColumnName = column.ColumnName;
                        //ColumnData = row[column].ToString();
                        var result1 = termsList.Where(item => item == "กิน/ติด (Grawing/Catch)|TextBox");
                        foreach (string value in result1)
                        {
                            if (!string.IsNullOrEmpty(row[value].ToString()))
                                ws.Cells[GetExcelColumnName(col) + irow].Formula = "=" + row[value].ToString();
                        }
                        char c = '✓';
                        //var column = "ไม่กิน/ไม่ติด (Non Grawing/No Catch)|TextBox";
                        //    if(row[column].ToString()!="0")
                        //    ws.Cells[GetExcelColumnName(col) + irow].Value = row[column].ToString();
                        if (row["สภาพกล่อง|RadioButton"].ToString() == "Normal")
                        {
                            ws.Cells[GetExcelColumnName(col - 1) + irow].Value = string.Format("{0}", c);
                        }
                        if (row["สภาพกล่อง|RadioButton"].ToString() == "Abnormal")
                        {
                            ws.Cells[GetExcelColumnName(col - 1) + irow].Value = "X";
                            ws.Cells["W" + irow].Value = "X";
                        }
                        else
                        {
                            ws.Cells[GetExcelColumnName(col + 1) + irow].Value = c.ToString();
                            ws.Cells["W" + irow].Value = c.ToString();
                        }
                        if (row["เปลี่ยน (Changed)|CheckBox"].ToString() == "True")
                        {
                            ws.Cells[GetExcelColumnName(col + 1) + irow].Value = c.ToString();
                            ws.Cells["Y" + irow].Value = c.ToString();
                        }
                        //ws.Cells[irow, col--].Value = row[7].ToString();
                        //ws.Cells[irow, col].Value = row[8].ToString();
                        //ws.Cells[irow, col++].Value = row[9].ToString();
                        //}
                    }
                }
                irow++;
            }
            //for (int i = 0; i < g.VisibleRowCount; i++)
            //{
            //    if ((string)g.GetRowValues(i, "IssueDate") != IssueDate)
            //    {
            //        IssueDate = (string)g.GetRowValues(i, "IssueDate");
            //        if (o < 4)
            //            ws.Cells[array[o] + "8"].Value = IssueDate;
            //        irow = 12;
            //        copyirow = 60;
            //        o++;
            //    }
            //    if (irow > 59)
            //    {
            //        ws.Rows[irow].Insert();
            //        ws.Rows[irow].CopyFrom(ws.Rows[copyirow]);
            //        copyirow++;
            //    }


            //    ws.Cells["A" + irow].Value = _result((string)g.GetRowValues(i, "InstallArea"));
            //    irow++;
            //}
            //}
        }
        else if (name.Equals("F3QCPC17 Insectocutor Inspection Record"))
        {
            path = Server.MapPath(@"~/ExcelFiles/F3QCPC17-0-01-10-21.xlsx");
            spreadsheet.Document.LoadDocument(path);
            Worksheet ws;
            int o2 = 0;
             ws = spreadsheet.Document.Worksheets[0];
            DataView dv2 = (DataView)dsGrouping.Select(DataSourceSelectArguments.Empty);
            g.SortBy(g.Columns["IssueDate"], ColumnSortOrder.Ascending);
            string[] ar2 = { "B", "D", "F", "H", "J","L","N","P","R","S","T","V","X","Z","AB","AD" };
            DataTable table2 = g.DataSource as DataTable;
            DataView view2 = new DataView(table2);
            DataTable distIssueDate2 = view2.ToTable(true, "IssueDate");
            distIssueDate2.DefaultView.Sort = "IssueDate ASC";
            var orderedRows2 = from row in distIssueDate2.AsEnumerable()
                              orderby row.Field<DateTime>("IssueDate")
                              select row;
            DataTable tblOrdered2 = orderedRows2.CopyToDataTable();
            //distIssueDate = distIssueDate.DefaultView.ToTable();
            foreach (DataRow r in tblOrdered2.Rows)
            {
                if(o2 <= 15)
                ws.Cells[ar2[o2] + "6"].Value = Convert.ToDateTime(r["IssueDate"]).ToString("dd-MM-yyyy");
  
                //DataTable distinctValues2 = view2.ToTable(true, "InstallArea");
                //List<string> termsList2 = new List<string>();
                //foreach (DataRow r in distinctValues2.Rows){
                int irow2 = 8;
                DataRow[] result2 = table2.Select("IssueDate = '" + r["IssueDate"].ToString() + "' and Grouping=8");
                foreach (DataRow row in result2)
                {
                    ws.Cells["A" + irow2].Value = _result((string)row["InstallArea"], "Name");
                    ws.Cells[ar2[o2] + irow2].Value = string.Format("{0}", row["Fly|TextBox"]);
                    var num = ColumnNameToNumber(ar2[o2]);
                    ws.Cells[GetExcelColumnName(num+1) + irow2].Value = string.Format("{0}", row["Other|TextBox"]);
                    irow2++;
                }
              o2++;
                //}
            }
        }
        jumptoexit:
            var st = new MemoryStream();
            spreadsheet.Document.SaveDocument(st, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            Response.Clear();
            Response.ContentType = "application/force-download";
            String fileName = String.Format(name + "_{0}.xlsx", DateTime.Now.ToString("yyyyMMddhhmmss"));
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(st.ToArray());
            Response.End();
        //System.IO.FileInfo file = new System.IO.FileInfo(path);
        //string Outgoingfile = name+".xlsx";
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
    }
    private int ColumnNameToNumber(string col_name)
    {
        int result = 0;

        // Process each letter.
        for (int i = 0; i < col_name.Length; i++)
        {
            result *= 26;
            char letter = col_name[i];

            // See if it's out of bounds.
            if (letter < 'A') letter = 'A';
            if (letter > 'Z') letter = 'Z';

            // Add in the value of this letter.
            result += (int)letter - (int)'A' + 1;
        }
        return result;
    }
    private string GetExcelColumnName(int columnNumber)
    {
        int dividend = columnNumber;
        string columnName = String.Empty;
        int modulo;

        while (dividend > 0)
        {
            modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
            dividend = (int)((dividend - modulo) / 26);
        }

        return columnName;
    }
    string _result(string keys,string value)
    {
        string r = "";
        if (string.IsNullOrEmpty(keys))
            return r;
        DataView dvsqllist = (DataView)dsArea.Select(DataSourceSelectArguments.Empty);
        if (dvsqllist != null)
        {
            DataTable dt = dvsqllist.Table;
            DataRow[] mytable = dt.Select("ID='" + keys.ToString() + "'");
            foreach (DataRow dr in mytable)
                r= dr[value].ToString(); 
                    
        }
        return r;
    }
    void export()
    {
        GridExporter.WriteXlsToResponse();
       
    }
    public static string Get(string uri)
    {
        string results = "N/A";

        try
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            results = sr.ReadToEnd();
            sr.Close();
        }
        catch (Exception ex)
        {
            results = ex.Message;
        }
        return results;
    }
    protected void btn_Click(object sender, EventArgs e)
    {
        //export();
        PrintingSystemBase ps = new PrintingSystemBase();
        PrintableComponentLinkBase lnk = new PrintableComponentLinkBase(ps);
        lnk.Component = GridExporter;

        CompositeLinkBase compositeLink = new CompositeLinkBase(ps);
        compositeLink.Links.AddRange(new object[] { lnk });
        compositeLink.CreateDocument();

        MemoryStream stream = new MemoryStream();
        compositeLink.PrintingSystemBase.ExportToXls(stream);
        WriteToResponse(testgrid.ID, true, "xls", stream);
    }
    protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
    {
        if (Page == null || Page.Response == null) return;
        string disposition = saveAsFile ? "attachment" : "inline";
        Page.Response.Clear();
        Page.Response.Buffer = false;
        Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
        Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
        Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, HttpUtility.UrlEncode(fileName).Replace("+", "%20"), fileFormat));
        Page.Response.BinaryWrite(stream.ToArray());
        Page.Response.End();
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

    protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        var result = new Dictionary<string, string>();
        string[] args = e.Parameters.Split('|');
        switch (args[0])
        {
            case "build":
                //filter.FilterExpression = args[1].ToString();
                //filter.DataBind();
                CriteriaOperator co = CriteriaOperator.Parse(args[1].ToString());
                break;
        }
        g.DataBind();
    }

    protected void cpfilter_Callback(object sender, CallbackEventArgsBase e)
    {
        string[] args = e.Parameter.Split('|');
        filter.FilterExpression = args[1].ToString();
        filter.DataBind();
    }
}