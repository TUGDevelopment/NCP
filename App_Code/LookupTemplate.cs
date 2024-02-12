using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public class LookupTemplate : ITemplate {
    public LookupTemplate() {   
    }

    public void InstantiateIn(Control container) {
        ASPxGridLookup gridLookupOptions = new ASPxGridLookup { ID = "lookup1" };
        gridLookupOptions.KeyFieldName = "Options";
        container.Controls.Add(gridLookupOptions);
        gridLookupOptions.DataSource = GetDataTable();
        gridLookupOptions.AutoGenerateColumns = true;
        gridLookupOptions.SelectionMode = GridLookupSelectionMode.Multiple;
        gridLookupOptions.TextFormatString = "{0}";
        gridLookupOptions.MultiTextSeparator = " ";
      
        gridLookupOptions.GridViewProperties.SettingsBehavior.AllowFocusedRow = true;
        gridLookupOptions.GridViewProperties.SettingsBehavior.AllowSelectByRowClick = true;
        gridLookupOptions.GridViewProperties.SettingsBehavior.AllowSelectSingleRowOnly = true;
        gridLookupOptions.Caption = "Select options";
        gridLookupOptions.ClientSideEvents.GotFocus = "function(s,e) { s.ShowDropDown(); }";
        gridLookupOptions.Width = Unit.Pixel(100);
        gridLookupOptions.DataBind();
        GridViewEditItemTemplateContainer gridContainer = (GridViewEditItemTemplateContainer)container;
        if (!(gridLookupOptions.Columns[0] is GridViewCommandColumn)) {
            GridViewCommandColumn commandCol = new GridViewCommandColumn();
            commandCol.ShowSelectCheckbox = true;
            commandCol.VisibleIndex = 0;
            commandCol.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
            commandCol.ShowClearFilterButton = true;
            gridLookupOptions.Columns.Insert(0, commandCol);
        }
     
    }

    DataTable GetDataTable() {
        DataTable Table = new DataTable();
        Table.Columns.Add("Options", typeof(string));
        Table.Rows.Add("Option1");
        Table.Rows.Add("Option2");
        Table.Rows.Add("Option3");
        return Table;
    }
}