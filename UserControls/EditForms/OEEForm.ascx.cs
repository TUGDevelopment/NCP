using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_EditForms_OEEFrom : System.Web.UI.UserControl
{
    MyDataModule cs = new MyDataModule();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void cb_Callback(object source, DevExpress.Web.CallbackEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Parameter))
            return;
        var args = e.Parameter.Split('|');
        var result = new Dictionary<string, string>();
        if (args[0] == "reload")
        {
            SqlParameter[] Param = { new SqlParameter("@Param", args[1].ToString()) };
            DataTable dttab = cs.GetRelatedResourcesDb("spGetData", Param);
            foreach (DataRow row in dttab.Rows)
            {
                string[] arr = { row["ProOrder"].ToString(),
                string.Format("{0}",row["SalesOrder"]),
                string.Format("{0}",row["Items"]),
                string.Format("{0}",row["Brand"]),
                string.Format("{0}",row["Customer"])};
                e.Result = string.Join("|", arr);
            }

        }
    }
}