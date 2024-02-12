using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_transEditForm : System.Web.UI.UserControl
{
    MyDataModule cs = new MyDataModule();
    string user_name = HttpContext.Current.User.Identity.Name.Replace(@"THAIUNION\", @"");
    private DataTable testdata
    {
        get { return Session["testdata"] == null ? null : (DataTable)Session["testdata"]; }
        set { Session["testdata"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        hfid["hidden_value"] ="";
        //if (!IsPostBack)
    }
    protected void showTabs_Init(object sender, EventArgs e)
    {
        // if (!IsCallback && !IsPostBack)
        if (!IsPostBack)
        {
            ASPxCheckBox cb = (ASPxCheckBox)sender;
            cb.Checked = pageControl.ShowTabs;
        }
    }
    protected void showTabs_CheckedChanged(object sender, EventArgs e)
    {
        ASPxCheckBox cb = (ASPxCheckBox)sender;
        pageControl.ShowTabs = cb.Checked;
        popupControl.ShowOnPageLoad = false;
    }

    protected void transferEditPopup_WindowCallback(object source, PopupWindowCallbackArgs e)
    {
        ASPxPopupControl popupControl = (ASPxPopupControl)source;
        if (string.IsNullOrEmpty(e.Parameter)) return;
        string[] param = e.Parameter.Split('|');
        if (param[0] == "read")
        {
            SqlParameter[] p = { new SqlParameter("@Id", param[1].ToString()) };
            DataTable dt = cs.GetRelatedResources("spselectResult", p);
            foreach (DataRow dr in dt.Rows)
            {
                txtBooksNo.Text = string.Format("{0}", dr["BookNumber"]);
                CmbCarNumber.Text= string.Format("{0}", dr["CarID"]);
                CmbDestination.Value = string.Format("{0}", dr["Destination"]);
                int index = Convert.ToInt32(dr["Shift"].ToString()=="O"?1:0);
                radioButtonList.SelectedIndex = index;

            }
        }
    }
    protected void OnCallback(object source, CallbackEventArgs e)
    {
        Control resControl = null;
        if (!string.IsNullOrEmpty(e.Parameter)) return; }
    protected void testgrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //DataTable table = (DataTable)Session["GetMyData"];
        if (string.IsNullOrEmpty(e.Parameters)) return;
        string[] param = e.Parameters.Split('|');
        if (param[0] == "AddRow")
            testdata = insertsGrid();
        if (param[0] == "reload" || param[0] == "priority")
            testgrid_reload(param[1]);
        g.DataBind();
    }
    private DataTable insertsGrid()
    {
        DataTable dt = testdata;
        int NextRowID = Convert.ToInt32(testdata.AsEnumerable()
                .Max(row => row["ID"]));
        NextRowID++;
        int ID = NextRowID;
        var values = TokenBoxTo.Text;
        dt.Rows.Add(ID,
                        values.ToString(),
                        txtWeight.Text,
                        "I", 0, "X");
        return dt;
    }
    void testgrid_reload(string Key)
    {
        Session.Remove("testdata");
        if (Key == "0")
            testdata = new DataTable();
        SqlParameter[] p = { new SqlParameter("@param", Key.ToString()) };
        testdata = cs.GetRelatedResources("spselectDataitem", p);
        testdata.PrimaryKey = new DataColumn[] { testdata.Columns["ID"] };
    }

    protected void testgrid_DataBound(object sender, EventArgs e)
    {

    }

    protected void testgrid_DataBinding(object sender, EventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        g.KeyFieldName = "ID";
        (sender as ASPxGridView).DataSource = GetTable;
        g.ForceDataRowType(typeof(DataRow));
    }
    private DataTable GetTable
    {
        get
        {
            if (testdata == null)
            {
                testdata = new DataTable();
                if (string.IsNullOrEmpty(string.Format("{0}", hfid["hidden_value"])))
                    testdata = GetGridData();
            }
            return testdata;
        }
    }
    private DataTable GetGridData()
    {
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[6] { new DataColumn("ID", typeof(int)),
                            new DataColumn("CarrierID", typeof(string)),
                            new DataColumn("Weight", typeof(string)),
                            new DataColumn("CheckType", typeof(string)),
                            new DataColumn("MatDoc", typeof(string)),
                            new DataColumn("Mark",typeof(int))});
        dt.PrimaryKey = new DataColumn[] { dt.Columns["ID"] };
        return dt;
    }

    protected void PreviewPanel_Callback(object sender, CallbackEventArgsBase e)
    {
        Control resControl = null;
        if (!string.IsNullOrEmpty(e.Parameter)) return;
    }
    protected void gridData_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        ASPxGridView g = sender as ASPxGridView;
        //DataTable table = (DataTable)Session["GetMyData"];
        if (string.IsNullOrEmpty(e.Parameters)) return;
        string[] param = e.Parameters.Split('|');
        if (param[0] == "save")
        {
            string rbOptionSV = radioButtonList.SelectedItem.ToString();
            foreach (ListEditItem item in radioButtonList.Items)
            {
                if (item.Selected == true)
                    rbOptionSV = item.Text;
            }

            SqlParameter[] p = { new SqlParameter("@Id", param[1].ToString()),
            new SqlParameter("@BookNumber", string.Format("{0}", txtBooksNo.Text)),
            new SqlParameter("@StatusApp", string.Format("{0}", 0)),
            new SqlParameter("@CarID", string.Format("{0}", CmbCarNumber.Text)),
            //new SqlParameter("@StForm", string.Format("{0}", DateTime.Now.ToString("dd-MM-yyyy"))),
            new SqlParameter("@StForm", string.Format("{0}", "")),
            new SqlParameter("@Destination", string.Format("{0}", CmbDestination.Text)),
            new SqlParameter("@user", string.Format("{0}", user_name.ToString())),
            new SqlParameter("@shift", string.Format("{0}",rbOptionSV ))};
            DataTable dt = cs.GetRelatedResources("spinsertDataResult", p);
            foreach (DataRow dr in dt.Rows)
                savetestgrid(string.Format("{0}", dr["Id"]));
        }
        g.DataBind();
    }
        void savetestgrid(string keys){
                foreach (DataRow dr in testdata.Rows)
                {
                    SqlParameter[] p = { new SqlParameter("@matDoc", keys.ToString()),
                        new SqlParameter("@Weight", string.Format("{0}",dr["Weight"] )),
                        new SqlParameter("@Mark", string.Format("{0}",dr["Mark"] )),
                        new SqlParameter("@CarrierID", string.Format("{0}",dr["CarrierID"] ))};
            DataTable dt = cs.GetRelatedResources("spinsertCarrier", p);
        }
        }
}