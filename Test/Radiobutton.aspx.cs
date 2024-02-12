using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Test_Radiobutton : System.Web.UI.Page
{
    //protected void Page_Init(object sender, EventArgs e)
    //{
    //    if (!grid1.EnableViewState)
    //    {
    //        grid1.Columns.Clear();
    //        criaColunas();
    //    }
    //}
    protected void grid1_DataBinding(object sender, EventArgs e)
    {
        if (!grid1.EnableViewState)
        {
            grid1.Columns.Clear();
            criaColunas();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        grid1.KeyFieldName = "campo1";
        grid1.DataSource = criaDados();
        if (!IsPostBack)
            grid1.DataBind();
    }
    public DataTable criaDados()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("campo1");
        dt.Columns.Add("campo2");

        for (int i = 0; i < 10; i++)
            dt.Rows.Add(i, "Valor " + i);
        return dt;
    }

    public void criaColunas()
    {
        GridViewDataColumn coluna1 = new GridViewDataColumn("campo1");
        coluna1.Width = Unit.Percentage(10);
        grid1.Columns.Add(coluna1);

        GridViewDataColumn coluna2 = new GridViewDataColumn("campo2");
        coluna2.Width = Unit.Percentage(90);

        DataTable dtDados = new DataTable();
        dtDados.Columns.Add("chave");
        dtDados.Columns.Add("valor");
        for (int i = 0; i < 10; i++)
            dtDados.Rows.Add(i, "Valor " + i);
        coluna2.DataItemTemplate = new coluna2_DataItemTemplate(dtDados);
        grid1.Columns.Add(coluna2);
    }

    protected void grid1_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType == GridViewRowType.Data)
        {
            ASPxRadioButtonList ovRBL_Data = (ASPxRadioButtonList)grid1.FindRowCellTemplateControl(e.VisibleIndex, null, "ovRBL_Data_" + e.VisibleIndex);
            if (ovRBL_Data != null)
            {
                object valor = e.GetValue(ovRBL_Data.Attributes["campo"].ToString());
                ovRBL_Data.SelectedIndex = -1;
                if (!String.IsNullOrEmpty(valor.ToString()))
                    ovRBL_Data.Items.FindByValue(valor.ToString()).Selected = true;
            }
        }
    }
    class coluna2_DataItemTemplate : ITemplate
    {
        private DataTable dtDados;

        public coluna2_DataItemTemplate(DataTable dtDados)
        {
            this.dtDados = dtDados;
        }

        public void InstantiateIn(Control container)
        {
            ASPxRadioButtonList ovRBL = new ASPxRadioButtonList();
            var typedContainer = container as GridViewDataItemTemplateContainer;
            ovRBL.ID = "ovRBL_Data_" + typedContainer.VisibleIndex;
            container.Controls.Add(ovRBL);

            ovRBL.ClientIDMode = ClientIDMode.Static;
            ovRBL.Width = Unit.Percentage(100);
            ovRBL.Border.BorderStyle = BorderStyle.None;
            ovRBL.Paddings.Padding = Unit.Pixel(0);
            ovRBL.RepeatColumns = dtDados.Rows.Count;

            ovRBL.ValueField = "chave";
            ovRBL.TextField = "valor";

            ovRBL.Attributes.Add("campo", "campo1");

            ovRBL.DataSource = this.dtDados;
            ovRBL.DataBind();


        }
    }
}