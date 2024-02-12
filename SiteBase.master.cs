using System;
using System.Web.UI;
using DevExpress.Web;

public partial class SiteMasterBase : MasterPage {
    protected void DownloadButton_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e) {
        e.Properties["cpTrialUrl"] = AssemblyInfo.DXLinkTrial;
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
            string Id = (Request.QueryString["viewMode"] == null) ? "countEditForm" : Request.QueryString["viewMode"].ToString();
            //return page != null ? page.PageName : "countEditForm";// string.Empty;
            return Id;
        }
    }
}
