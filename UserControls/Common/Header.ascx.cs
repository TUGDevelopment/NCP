using System;
using System.Web.Security;
using System.Web.UI;

public partial class Header : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.User.Identity.IsAuthenticated)
            FormsAuthentication.RedirectToLoginPage();
    }

}
