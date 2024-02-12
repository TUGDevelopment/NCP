using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Linq;

public partial class _Default : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["ncpDbConnectionString"].ConnectionString;
	MyDataModule cs = new MyDataModule();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsCallback && !IsPostBack)
        {
            //ASPxGridView1.DataBind();
            if (Request.QueryString["Id"]=="0")
            ASPxGridViewExporter1.WriteXlsxToResponse();
            else
                BindGrid();

        }
    }
    protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
    {
        string strQuery = "spExportanalysisall";
        SqlCommand cmd = new SqlCommand(strQuery);
        DataTable dt = spGetData(cmd);
        ASPxGridView1.DataSource = dt;

    }
    public DataTable spGetData(SqlCommand cmd)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(constr);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = con;
        try
        {
            con.Open();
            sda.SelectCommand = cmd;
            sda.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            con.Close();
            sda.Dispose();
            con.Dispose();
        }
    }
//    protected void ASPxGridView1_DataBinding(object sender, EventArgs e)
//    {
//        string strQuery = "select *" +
//             " from tblncp";
//        SqlCommand cmd = new SqlCommand(strQuery);
//        DataTable dt = cs.GetData(cmd);
//        ASPxGridView1.DataSource = dt;
//    }
    private void BindGrid()
       {
        int id = int.Parse(Request.QueryString["Id"]);
        byte[] bytes;
        string fileName, contentType;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select Name, Data, ContentType from FileSystem where Id=@Id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    sdr.Read();
                    bytes = (byte[])sdr["Data"];
                    contentType = sdr["ContentType"].ToString();
                    fileName = sdr["Name"].ToString();
                }
                con.Close();
            }
        }
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = contentType;
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
        Response.BinaryWrite(bytes);
        Response.Flush(); 
        Response.End();
    }
}
