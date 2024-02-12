using System;
using System.Web;
using System.Data;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Net.Mail;
using System.IO;
using System.Collections;
using System.ComponentModel;

/// <summary>
/// Summary description for MyDataModule
/// </summary>
public class MyDataModule
{
    public string user_name = HttpContext.Current.User.Identity.Name.Replace(@"THAIUNION\", @"");
    public string strConn = ConfigurationManager.ConnectionStrings["ncpDbConnectionString"].ConnectionString;
    public MyDataModule()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public DataTable build_Datatable(string data)
    {
        using (SqlConnection oConn = new SqlConnection(strConn))
        {
            oConn.Open();
            string strQuery = data;
            DataTable dt = new DataTable();
            SqlDataAdapter oAdapter = new SqlDataAdapter(strQuery, oConn);
            // Fill the dataset.
            oAdapter.Fill(dt);
            oConn.Close();
            oConn.Dispose();
            return dt;
        }
    }
    public DataTable GetRelatedResources(string StoredProcedure, object[] Parameters)
    {
        var Results = new DataTable();
        //String strConnString = System.Configuration.ConfigurationManager.
        //    ConnectionStrings["constr"].ConnectionString;
        try
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcedure, conn))
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(Parameters);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(Results);
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return Results;
    }
    public DataTable GetRelatedResourcesDb(string StoredProcedure, object[] Parameters)
    {
        var Results = new DataTable();
        String strConnString = System.Configuration.ConfigurationManager.
            ConnectionStrings["oeeDbConnectionString"].ConnectionString;
        try
        {
            using (SqlConnection conn = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand(StoredProcedure, conn))
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(Parameters);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(Results);
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return Results;
    }
    public string ReadItems(string strQuery)
    {
        string result = "";
        // (ByVal FieldName As String, ByVal TableName As String, ByVal Cur As String, ByVal Value As String) As String
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(strConn);
        SqlDataAdapter sda = new SqlDataAdapter();
        SqlCommand cmd = new SqlCommand(strQuery);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;
        con.Open();
        sda.SelectCommand = cmd;
        sda.Fill(dt);
        con.Close();
        con.Dispose();
        StringBuilder sb = new StringBuilder();
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                sb.Append(row[0] + ",");
            }
            if (result.Length < 2)
            {
                result = sb.ToString();
                result = result.Substring(0, (result.Length - 1));
            }
        }
        return result;
    }
    public DataTable GetData(SqlCommand cmd)
    {
        DataTable dt = new DataTable();
        //String strConnString = System.Configuration.ConfigurationManager.
        //     ConnectionStrings["ncpDbConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConn);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.CommandType = CommandType.Text;
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
 
    public bool HasColumn(DataTable data, string columnName)
    {
        if (data == null || string.IsNullOrEmpty(columnName))
        {
            return false;
        }

        foreach (DataColumn column in data.Columns)
            if (columnName.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase)) return true;
        return false;
    }
    public string GetNewID()
    {
        using (SqlConnection con = new SqlConnection(strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetNewIDRequest";
            cmd.Connection = con;
            con.Open();
            var getValue = cmd.ExecuteScalar();
            con.Close();
            con.Dispose();
            return (string)getValue;
        }
    }
}
