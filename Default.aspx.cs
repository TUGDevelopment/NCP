using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    ServiceCS cs = new ServiceCS();
    protected void Page_Init(object sender, EventArgs e){
        cs.GetDataReport();
        //LoadUserControls();
    }
    protected void cp_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e){
        LoadUserControls();
    }
    protected void LoadUserControls()
    {
        try
        {
            var viewMode = Request.QueryString["viewMode"];
            if (string.IsNullOrEmpty(viewMode))
                viewMode = "WebUserControl";
            MasterContainer.Controls.Clear();
            Control myControl = LoadControl(string.Format("~/UserControls/EditForms/{0}.ascx", viewMode));

            myControl.ID = "ucx_mapa";
            cp.Controls.Clear();
            cp.Controls.Add(myControl);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}