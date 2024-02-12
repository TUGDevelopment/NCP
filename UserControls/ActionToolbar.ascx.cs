using System;
using System.Web.UI;
using System.Xml;
using DevExpress.Utils;
using DevExpress.Web;
using System.Collections.Generic;

public partial class UserControls_ActionToolbar : System.Web.UI.UserControl {

    protected void Page_Load(object sender, EventArgs e) {
        ActionMenuDataSource.XPath = string.Format("Pages/{0}/Item", "form");// Utils.CurrentPageName);
    }

    protected void ActionMenu_ItemDataBound(object sender, MenuItemEventArgs e) {
        IHierarchyData itemHierarchyData = (IHierarchyData)e.Item.DataItem;
        var element = (XmlElement)itemHierarchyData.Item;

        var classAttr = element.Attributes["SpriteClassName"];
        if(classAttr != null)
            e.Item.Image.SpriteProperties.CssClass = classAttr.Value;
        if (e.Item.Parent == e.Item.Menu.RootItem)
            e.Item.ClientVisible = false;
    }
}
