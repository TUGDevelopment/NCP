using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Site : System.Web.UI.MasterPage {
    protected BasePage BasePage { get { return Page as BasePage; } }
    protected void Page_Load(object sender, EventArgs e) {

    }
    protected void Page_Init(object sender, EventArgs e)
    {
        var pageNameScript = string.Format("<script type='text/javascript'>DevAVPageName = '{0}';</script>", PageName);
        Page.Header.Controls.AddAt(0, new LiteralControl(pageNameScript));
    }
    protected string PageName
    {
        get
        {
            var page = Page as BasePage;
            NameValueCollection n = Request.QueryString;
            return (n.HasKeys()==false) ? "countEditForm" : Request.QueryString["viewMode"].ToString();
            //return page != null ? page.PageName : "countEditForm";// string.Empty;
            //return Id;
        }
    }
    protected void UserMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e)
    {

    }
}
