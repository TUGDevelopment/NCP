using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_EditForms_login : System.Web.UI.UserControl
{
    string constr = ConfigurationManager.ConnectionStrings["ncpDbConnectionString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btOK_Click(object sender, EventArgs e)
    {
        int userId = 0;string userData = "XX";

        //if (HttpContext.Current.User.Identity.IsAuthenticated)
        //{
        //string UserName = HttpContext.Current.User.Identity.Name;
        //    string cookiestr;
        //    FormsAuthenticationTicket tkt = new FormsAuthenticationTicket(1, UserName, DateTime.Now,
        //        DateTime.Now.AddMinutes(1), false, userData);
        //    cookiestr = FormsAuthentication.Encrypt(tkt);
        //    HttpCookie ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
        //    ck.Expires = tkt.Expiration;
        //    ck.Path = FormsAuthentication.FormsCookiePath;
        //    FormsAuthentication.SetAuthCookie(UserName, false);
        //    string strRedirect = Request.QueryString["CustomReturnUrl"];
        //    if (strRedirect == null)
        //        strRedirect = "http://localhost/LIMS_DEV/Default.aspx";
        //    Response.Redirect(strRedirect + "?UserData=" + userData, true);
        //}
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("Validate_User"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", tbLogin.Text);
                cmd.Parameters.AddWithValue("@Password", tbPassword.Text);
                cmd.Connection = con;
                con.Open();
                userId = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
        }
        switch (userId)
        {
            case -1:
                tbLogin.Text = "Username and/or password is incorrect.";
                break;
            case -2:
                tbLogin.Text = "Account has not been activated.";
                break;
            default:
                FormsAuthentication.RedirectFromLoginPage(tbLogin.Text, true);
                break;
        }
    }
}