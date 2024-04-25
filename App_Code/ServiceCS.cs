using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Services;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Globalization;
using System.Text;
using System.Net.Mail;
using System.Linq;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net;
using ClosedXML.Excel;
using System.Web;
using IWshRuntimeLibrary;
using System.Runtime.InteropServices;
using System.Security.Principal;

/// <summary>
/// Summary description for ServiceCS
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ServiceCS : System.Web.Services.WebService
{
    string strConn = ConfigurationManager.ConnectionStrings["ncpDbConnectionString"].ConnectionString;
    string CurUserName = HttpContext.Current.User.Identity.Name.Replace(@"THAIUNION\", @"");
    MyDataModule cs = new MyDataModule();
    [DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int WNetGetConnection(
    [MarshalAs(UnmanagedType.LPTStr)] string localName,
    [MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName,
    ref int length);
    public ServiceCS()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    [WebMethod]
    public void BuildCertEU()
    {
        string file = @"D:\SAPInterfaces\Outbound\ZTHWM_SL028_CERT_UPDT_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[] { new DataColumn(string.Format(@"{0}", "Material NumberT_CERT-MATNR")),
            new DataColumn(string.Format(@"{0}","Batch NumberT_CERT-CHARG")),
            new DataColumn(string.Format(@"{0}","PlantT_CERT-WERKS")),
            new DataColumn(string.Format(@"{0}","Invoice NO.T_CERT-INVNO")),
            new DataColumn(string.Format(@"{0}","Date of ManufactureT_CERT-HSDAT")),
            new DataColumn(string.Format(@"{0}","Production/inspection memoT_CERT-FERTH")),
            new DataColumn(string.Format(@"{0}","QuantityT_CERT-MENGE")),
            new DataColumn(string.Format(@"{0}","Base Unit of MeasureT_CERT-MEINS")),
            new DataColumn(string.Format(@"{0}","Vessel NO.T_CERT-VESSL")),
            new DataColumn(string.Format(@"{0}","FAOT_CERT-FAO")),
            new DataColumn(string.Format(@"{0}","AgingT_CERT-AGING")),
            new DataColumn(string.Format(@"{0}","StatusT_CERT-PSTAT")),
            });									

        var Results = new DataTable();//
        SqlParameter[] param = { new SqlParameter("@user", string.Format("{0}", CurUserName)) };
        Results = cs.GetRelatedResourcesDb("spSQLinterfaceCertEU", param);
        foreach (DataRow row in Results.Rows)
        {
            dt.Rows.Add(string.Format("{0}", row["Material"].ToString()),
            string.Format("{0}", row["Batch"].ToString()),
            string.Format("{0}", row["Plant"].ToString()),
            string.Format("{0}", row["InvoiceNo"].ToString()),
            string.Format("{0}", row["DateOfManuf"].ToString()),
            string.Format("{0}", row["Production"].ToString()),
            string.Format("{0}", row["Quantity"].ToString()),
            string.Format("{0}", row["BaseUoM"].ToString()),
            string.Format("{0}", row["Vessel"].ToString()),
            string.Format("{0}", row["FAO"].ToString()),
            row["Aging"].ToString(),
            row["StatusApp"].ToString());
        }
        
        cs.ToCSV(dt.Select("[Plant T_CERT-WERKS]='1021'").CopyToDataTable(), file);
        cs.ToCSV(dt.Select("[Plant T_CERT-WERKS]='1022'").CopyToDataTable(), @"D:\SAPInterfaces\Outbound\ZTHWM_SL028_CERT1022_UPDT_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv");
    }

    private void lineNotify(string msg)
    {
        string token = "9IBnp37LVHj0a6W5HLq2dF7sqIjGyEVn2DQtpQq7wYv";
        try
        {
            var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
            var postData = string.Format("message={0}", msg);
            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Headers.Add("Authorization", "Bearer " + token);

            using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
    [WebMethod]
    public void GetCurrentUserUid(soapActionObject soapAction)
    {
        HttpResponse Response = HttpContext.Current.Response;
        Response.ContentType = "application/x-javascript";
        Response.Charset = "utf-8";
        using (SqlConnection con = new SqlConnection(strConn))
        {
            //string strQuery = "select value from dbo.FNC_SPLIT('" + soapAction.FrDt + "', ',')";
            //DataTable dt = new DataTable();
            //SqlDataAdapter oAdapter = new SqlDataAdapter(strQuery, con);
            //// Fill the dataset.
            //oAdapter.Fill(dt);
            //con.Close();
            ////Context.Response.Write(JsonConvert.SerializeObject(dt));
            //Response.Write(JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented));
            //Response.End();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spselectsummaryall";
            cmd.Parameters.AddWithValue("@FrDt", string.Format("{0}", soapAction.FrDt));
            cmd.Parameters.AddWithValue("@ToDt", string.Format("{0}", soapAction.ToDt));
            cmd.Parameters.AddWithValue("@Plant", string.Format("{0}", soapAction.Plant));
            cmd.Parameters.AddWithValue("@where", string.Format("{0}", soapAction.where));
            cmd.Parameters.AddWithValue("@By", string.Format("{0}", soapAction.By));
            cmd.Parameters.AddWithValue("@Material", string.Format("{0}", soapAction.Material));
            cmd.Parameters.AddWithValue("@Batch", string.Format("{0}", soapAction.Batch));
            cmd.Parameters.AddWithValue("@username", string.Format("{0}", soapAction.username));
            cmd.Parameters.AddWithValue("@decision", string.Format("{0}", soapAction.decision));
            cmd.Parameters.AddWithValue("@FirstDecision", string.Format("{0}", soapAction.FirstDecision));
            cmd.Parameters.AddWithValue("@record", string.Format("{0}", soapAction.Record));
            cmd.Parameters.AddWithValue("@Packaging", string.Format("{0}", soapAction.Packaging));
            cmd.Parameters.AddWithValue("@Batch_Packaging", string.Format("{0}", soapAction.Batch_Packaging));
            cmd.Parameters.AddWithValue("@NCPType", string.Format("{0}", soapAction.NCPType));
            cmd.Parameters.AddWithValue("@Location", string.Format("{0}", soapAction.Location));
            cmd.Parameters.AddWithValue("@Line", string.Format("{0}", soapAction.LineNo));
            cmd.Parameters.AddWithValue("@Times", string.Format("{0}", soapAction.Times));
            cmd.Parameters.AddWithValue("@Shift", string.Format("{0}", soapAction.Shift));
            cmd.Parameters.AddWithValue("@Product", string.Format("{0}", soapAction.Product));
            cmd.Parameters.AddWithValue("@Problem", string.Format("{0}", soapAction.Problem));
            cmd.Connection = con;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
            oAdapter.Fill(dt);
            con.Close();
            //t= dt;
            Response.Write(JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented));
            Response.End();
        }
        //return "test :" + soapAction.userId;
    }
    private void CreateIfMissing(string path)
    {
        var dir = Server.MapPath(@"~/ConvertedFiles/" + path);
        bool folderExists = Directory.Exists(dir);
        if (!folderExists)
            Directory.CreateDirectory(dir);
    }
    [WebMethod]
    public void ExportToDel(string Data)
    {
        var workbook = new XLWorkbook(); int col = 2;
        //col = 4;
        //string[] array = { "zpm1", "", "X", "103","ex","203" };
        var worksheet = workbook.Worksheets.Add("ZTHWM_NCP_DELETE");
        //worksheet.Cell("D2").Value = "zpm1";
        //worksheet.Cell("F2").Value = "X";
        //worksheet.Cell("G2").Value = "103";
        //worksheet.Cell("H2").Value = "ex";
        //worksheet.Cell("I2").Value = "203";
        //for (int i = 3; i < 4; i++) {
        //worksheet.Range("C2:I2").CopyTo(worksheet.Range("C2:I2".Replace("2",i.ToString()))); 
        //worksheet.Cell("F"+i).Value = "X";
        //}
        var Results = new DataTable();//spGetHistory
        SqlParameter[] param = { new SqlParameter("@user", string.Format("{0}", CurUserName)) };
        Results = cs.GetRelatedResources("spSQLinterfaceWinDel", param);
        //string file = HttpContext.Current.Server.MapPath("~/ExcelFiles/ZTHWM_SL020_NCP_DELETE" + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv");
        string file = @"D:\SAPInterfaces\Outbound\ZTHWM_SL020_NCP_DEL" + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
        //HttpContext.Current.Server.MapPath("~/ExcelFiles/VK11_" ;
        //MyDataModule.ToCSV(Results, file);
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[] { new DataColumn(string.Format(@"{0}", "Material NumberT_NCP-MATNR")),
            new DataColumn(string.Format(@"{0}","PlantT_NCP-WERKS")),
            new DataColumn(string.Format(@"{0}","Batch NumberT_NCP-CHARG")),
            new DataColumn(string.Format(@"{0}","NCP CodeT_NCP-NCPCD")),
            });
        foreach (DataRow row in Results.Rows)
        {
            dt.Rows.Add(string.Format("{0}", row["material"].ToString()),
            string.Format("{0}", row["Plant"].ToString()),
            string.Format("{0}", row["Batchsap"].ToString()),
            row["NCPID"].ToString());
        }
        cs.ToCSV(dt, file);

        foreach (DataRow row in Results.Rows)
        {
            var i = col++;
            worksheet.Cell("B" + i).Value = string.Format("{0}", row["material"].ToString());
            worksheet.Cell("C" + i).Value = string.Format("{0}", row["Plant"].ToString());
            worksheet.Cell("D" + i).Value = string.Format("{0}", row["Batchsap"].ToString());
            worksheet.Cell("E" + i).Value = string.Format("{0}", row["NCPID"].ToString());
        }
        //workbook.SaveAs(@"C:\temp\HelloWorld.xlsx");
        //workbook.SaveAs(@"C:\\Users\fo5910155\Documents\Winshuttle\Studio\Data\ZTHWM_SL020_NCP_UPDATE_20180621_141014.xlsx");
        ExportToresult(Data);
        BuildCertEU();
        //Server.MapPath("~/App_Data/Documents/HelloWorld.xlsx"));
        Context.Response.Write(Data);
    }
    //public void ExportToDel(string Data)
    //{
    //    var workbook = new XLWorkbook(); int col = 2;
    //    //col = 4;
    //    //string[] array = { "zpm1", "", "X", "103","ex","203" };
    //    var worksheet = workbook.Worksheets.Add("ZTHWM_NCP_DELETE");
    //    //worksheet.Cell("D2").Value = "zpm1";
    //    //worksheet.Cell("F2").Value = "X";
    //    //worksheet.Cell("G2").Value = "103";
    //    //worksheet.Cell("H2").Value = "ex";
    //    //worksheet.Cell("I2").Value = "203";
    //    //for (int i = 3; i < 4; i++) {
    //    //worksheet.Range("C2:I2").CopyTo(worksheet.Range("C2:I2".Replace("2",i.ToString()))); 
    //    //worksheet.Cell("F"+i).Value = "X";
    //    //}
    //    var Results = new DataTable();//spGetHistory
    //    SqlParameter[] param = { new SqlParameter("@user", string.Format("{0}", CurUserName)) };
    //    Results = cs.GetRelatedResources("spSQLinterfaceWinDel", param);
    //    foreach (DataRow row in Results.Rows)
    //    {
    //        var i = col++;
    //        worksheet.Cell("B" + i).Value = string.Format("'{0}", row["material"].ToString());
    //        worksheet.Cell("C" + i).Value = string.Format("'{0}", row["Plant"].ToString());
    //        worksheet.Cell("D" + i).Value = string.Format("'{0}", row["Batchsap"].ToString());
    //        worksheet.Cell("E" + i).Value = string.Format("'{0}", row["NCPID"].ToString());
    //    }
    //    //workbook.SaveAs(@"C:\temp\HelloWorld.xlsx");
    //    workbook.SaveAs(@"C:\\Users\fo5910155\Documents\Winshuttle\Studio\Data\ZTHWM_SL020_NCP_UPDATE_20180621_141014.xlsx");
    //    ExportToresult(Data);
    //    //Server.MapPath("~/App_Data/Documents/HelloWorld.xlsx"));
    //    Context.Response.Write(Data);
    //}
    void ExportToresult(string Data)
    {
        var workbook = new XLWorkbook(); int col = 2;
        var worksheet = workbook.Worksheets.Add("ZTHWM_NCP_UPDATE");
        //MinPrice
        var Results = new DataTable();//spGetHistory
        SqlParameter[] param = { new SqlParameter("@user", string.Format("{0}", CurUserName)) };
        Results = cs.GetRelatedResources("spSQLinterfaceWinCreate", param);
        //string file = HttpContext.Current.Server.MapPath("~/ExcelFiles/ZTHWM_SL020_NCP_UPDATE" + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv");
        string file = @"D:\SAPInterfaces\Outbound\ZTHWM_SL020_NCP_UPDT" + "_" + DateTime.Now.ToString("yyyyMMddhhmm") + ".csv";
        //HttpContext.Current.Server.MapPath("~/ExcelFiles/VK11_" ;
        //MyDataModule.ToCSV(Results, file);
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[] { new DataColumn(string.Format(@"[{0}]","Material NumberT_NCP-MATNR")),
            new DataColumn(string.Format(@"{0}", "PlantT_NCP-WERKS")),
            new DataColumn(string.Format(@"{0}","Batch NumberT_NCP-CHARG")),
            new DataColumn(string.Format(@"{0}","NCP CodeT_NCP-NCPCD")),
            new DataColumn(string.Format(@"{0}","Result FlagT_NCP-RESLT")),

            new DataColumn(@"T_NCP-CRDAT"),
            new DataColumn(@"T_NCP-CRTIM"),
            new DataColumn(@"T_NCP-CHDAT"),
            new DataColumn(@"T_NCP-CHTIM"),
            new DataColumn(string.Format(@"{0}","NCP First DecisionT_NCP-TXFST")),

            new DataColumn(string.Format(@"{0}","NCP Result DecisionT_NCP-TXRES")),
            new DataColumn(string.Format(@"{0}","NCP ProblemT_NCP-TXPRB")),
            new DataColumn(string.Format(@"{0}","NCP RemarkT_NCP-TXRMK")),
            });
        foreach (DataRow row in Results.Rows)
        {
            dt.Rows.Add(string.Format("{0}", row["material"].ToString()),
            string.Format("{0}", row["Plant"].ToString()),
            string.Format("{0}", row["Batchsap"].ToString()),
            row["NCPID"].ToString(),
            row["Decision"].ToString() != "" || row["FirstDecision"].ToString() != "" ? "P" : "",
            string.Format("{0}", row["DateFirstDecision"].ToString()),
            row["TimeFirstDecision"].ToString(),
            string.Format("{0}", row["DateDecision"].ToString()).Replace('/', '.'),
            row["TimeDecision"].ToString(),
            row["FirstDecision"].ToString(),
            row["Decision"].ToString(),
            row["Problem"].ToString(),
            row["Remark"].ToString());
        }
        cs.ToCSV(dt, file);
        worksheet.Column(7).Width = 20;
        foreach (DataRow row in Results.Rows)
        {
            var i = col++;
            worksheet.Cell("B" + i).Value = string.Format("{0}", row["material"].ToString());
            worksheet.Cell("C" + i).Value = string.Format("{0}", row["Plant"].ToString());
            worksheet.Cell("D" + i).Value = string.Format("{0}", row["Batchsap"].ToString());
            worksheet.Cell("E" + i).Value = row["NCPID"].ToString();
            worksheet.Cell("F" + i).Value = row["Decision"].ToString() != "" || row["FirstDecision"].ToString() != "" ? "P" : "";
            worksheet.Cell("G" + i).Value = string.Format("{0}", row["DateFirstDecision"].ToString());
            worksheet.Cell("G" + i).Style.Font.FontSize = 8;
            worksheet.Cell("H" + i).Value = row["TimeFirstDecision"].ToString();
            worksheet.Cell("I" + i).Value = string.Format("{0}", row["DateDecision"].ToString()).Replace('/', '.');
            worksheet.Cell("J" + i).Value = row["TimeDecision"].ToString();
            worksheet.Cell("K" + i).Value = row["FirstDecision"].ToString();
            worksheet.Cell("L" + i).Value = row["Decision"].ToString();
            worksheet.Cell("M" + i).Value = row["Problem"].ToString();
            worksheet.Cell("N" + i).Value = row["Remark"].ToString();

            //worksheet.Cell("M" + i).Value = row["Currency"].ToString();
            //worksheet.Cell("N" + i).Value = row["PricingUnit"].ToString();
            //worksheet.Cell("O" + i).Value = row["SalesUnit"].ToString();
            //worksheet.Cell("P" + i).Value = string.Format("'{0}", row["RequestDate"].ToString());
            //worksheet.Cell("Q" + i).Value = string.Format("'{0}", row["RequireDate"].ToString());
            //worksheet.Cell("R" + i).Value = row["CostNo"].ToString();
        }
        //workbook.SaveAs(@"C:\temp\MinPrice.xlsx");
        //workbook.SaveAs(@"C:\\Users\fo5910155\Documents\Winshuttle\Studio\Data\ZTHWM_SL020_NCP_UPDATE_20180621_140330.xlsx");
    }
    //void ExportToresult(string Data)
    //{
    //    var workbook = new XLWorkbook(); int col = 2;
    //    var worksheet = workbook.Worksheets.Add("ZTHWM_NCP_UPDATE");
    //    //MinPrice
    //    var Results = new DataTable();//spGetHistory
    //    SqlParameter[] param = { new SqlParameter("@user", string.Format("{0}", CurUserName)) };
    //    Results = cs.GetRelatedResources("spSQLinterfaceWinCreate", param);
    //    worksheet.Column(7).Width = 20;
    //    foreach (DataRow row in Results.Rows)
    //    {
    //        var i = col++;
    //        worksheet.Cell("B" + i).Value = string.Format("'{0}", row["material"].ToString());
    //        worksheet.Cell("C" + i).Value = string.Format("'{0}", row["Plant"].ToString());
    //        worksheet.Cell("D" + i).Value = string.Format("'{0}", row["Batchsap"].ToString());
    //        worksheet.Cell("E" + i).Value = row["NCPID"].ToString();
    //        worksheet.Cell("F" + i).Value = row["Decision"].ToString() != "" || row["FirstDecision"].ToString() != "" ? "P" : "";
    //        worksheet.Cell("G" + i).Value = string.Format("'{0}", row["DateFirstDecision"].ToString());
    //        worksheet.Cell("G" + i).Style.Font.FontSize = 8;
    //        worksheet.Cell("H" + i).Value = row["TimeFirstDecision"].ToString();
    //        worksheet.Cell("I" + i).Value = string.Format("'{0}", row["DateDecision"].ToString()).Replace('/', '.');
    //        worksheet.Cell("J" + i).Value = row["TimeDecision"].ToString();
    //        worksheet.Cell("K" + i).Value = row["FirstDecision"].ToString();
    //        worksheet.Cell("L" + i).Value = row["Decision"].ToString();
    //        worksheet.Cell("M" + i).Value = row["Problem"].ToString();
    //        worksheet.Cell("N" + i).Value = row["Remark"].ToString();

    //        //worksheet.Cell("M" + i).Value = row["Currency"].ToString();
    //        //worksheet.Cell("N" + i).Value = row["PricingUnit"].ToString();
    //        //worksheet.Cell("O" + i).Value = row["SalesUnit"].ToString();
    //        //worksheet.Cell("P" + i).Value = string.Format("'{0}", row["RequestDate"].ToString());
    //        //worksheet.Cell("Q" + i).Value = string.Format("'{0}", row["RequireDate"].ToString());
    //        //worksheet.Cell("R" + i).Value = row["CostNo"].ToString();
    //    }
    //    //workbook.SaveAs(@"C:\temp\MinPrice.xlsx");
    //    workbook.SaveAs(@"C:\\Users\fo5910155\Documents\Winshuttle\Studio\Data\ZTHWM_SL020_NCP_UPDATE_20180621_140330.xlsx");
    //}
   
    [WebMethod]
    public void uploadfile(string file, string Doc, string username,
        string name)
    {
        //string AppLocation = "";
        //AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        //AppLocation = AppLocation.Replace("file:\\", "");
        //string file = AppLocation + "\\ExcelFiles\\DataFile.xlsx";

        //string file = Server.MapPath(@"~/ExcelFiles/" + Data);
        //string[] files = Directory.GetFiles(_p);
        //string[] files = Directory.GetFiles(Server.MapPath(@"~/ExcelFiles/" + Data));
        //foreach (string file in files)
        //{
        //Read File Bytes into a byte array
        byte[] FileData = ReadFile(file);

        //Initialize SQL Server Connection
        SqlConnection CN = new SqlConnection(strConn);

        string fileName = Path.GetFileName(file);
        string mimeType = MimeTypes.GetContentType(fileName);

        //Set insert query
        string qry = "insert into tblFiles values (@Name,@ContentType,@Data,@MatDoc,@ActiveBy,@SystemDate)";

        //Initialize SqlCommand object for insert.
        SqlCommand SqlCom = new SqlCommand(qry, CN);

        //We are passing Original File Path and file byte data as sql parameters.
        SqlCom.Parameters.Add(new SqlParameter("@Name", (object)name));
        SqlCom.Parameters.Add(new SqlParameter("@ContentType", (object)mimeType));
        SqlCom.Parameters.Add(new SqlParameter("@Data", (object)FileData));
        SqlCom.Parameters.Add(new SqlParameter("@MatDoc", (object)Doc));
        SqlCom.Parameters.Add(new SqlParameter("@ActiveBy", (object)username));
        SqlCom.Parameters.Add(new SqlParameter("@SystemDate", (object)DateTime.Now));
        //Open connection and execute insert query.
        CN.Open();
        SqlCom.ExecuteNonQuery();
        CN.Close();
        //}
        //string folderPath = Server.MapPath(@"~/ExcelFiles/" + Data);
        //Directory.Delete(folderPath, true);
    }
    [WebMethod]
    public void GetQRCodeGenerator(string strCode, string il, int wh)
    {
        var url = string.Format("http://chart.apis.google.com/chart?cht=qr&chs={1}x{2}&chl={0}", strCode, wh, il);
        WebResponse response = default(WebResponse);
        Stream remoteStream = default(Stream);
        StreamReader readStream = default(StreamReader);
        WebRequest request = WebRequest.Create(url);
        response = request.GetResponse();
        remoteStream = response.GetResponseStream();
        readStream = new StreamReader(remoteStream);
        System.Drawing.Image img = System.Drawing.Image.FromStream(remoteStream);
        img.Save(Server.MapPath("~/ExcelFiles/QRCode/" + strCode + ".png"));
        response.Close();
        remoteStream.Close();
        readStream.Close();
    }
    public byte[] ReadFile(string sPath)
    {
        //Initialize byte array with a null value initially.
        byte[] data = null;

        //Use FileInfo object to get file size.
        FileInfo fInfo = new FileInfo(sPath);
        long numBytes = fInfo.Length;

        //Open FileStream to read file
        FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);

        //Use BinaryReader to read file stream into byte array.
        BinaryReader br = new BinaryReader(fStream);

        //When you use BinaryReader, you need to supply number of bytes to read from file.
        //In this case we want to read entire file. So supplying total number of bytes.
        data = br.ReadBytes((int)numBytes);

        //Close BinaryReader
        br.Close();

        //Close FileStream
        fStream.Close();

        return data;
    }
    [WebMethod]
    public string GetUNCPath(string originalPath)
    {
        StringBuilder sb = new StringBuilder(512);
        int size = sb.Capacity;
        // look for the {LETTER}: combination ...
        if (originalPath.Length > 2 && originalPath[1] == ':')
        {
            // don't use char.IsLetter here - as that can be misleading
            // the only valid drive letters are a-z && A-Z.
            char c = originalPath[0];
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
            {
                int error = WNetGetConnection(originalPath.Substring(0, 2),
                    sb, ref size);
                if (error == 0)
                {
                    DirectoryInfo dir = new DirectoryInfo(originalPath);
                    string path = Path.GetFullPath(originalPath)
                        .Substring(Path.GetPathRoot(originalPath).Length);
                    return Path.Combine(sb.ToString().TrimEnd(), path);
                }
            }
        }
        return originalPath;
    }
    [WebMethod]
    public void moveto()
    {
        //AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
        //WindowsIdentity idnt = new WindowsIdentity(".\administrator", "P@ssw0rd");
        //WindowsImpersonationContext context = idnt.Impersonate();
        //System.IO.File.Copy(@"\\192.168.1.193\d$", @"~/ExcelFiles/DataFile.xlsx", true);
        //context.Undo();
        //IWshNetwork_Class network = new IWshNetwork_Class();
        //network.MapNetworkDrive("k:", @"\\192.168.1.193\d$", Type.Missing, ".\administrator", "P@ssw0rd");

        //string file = HttpContext.Current.Server.MapPath("~/ExcelFiles/DataFile.xlsx");
        //System.IO.File.Copy(file, @"\\192.168.1.193\d$");

        //var fileName = "DataFile.xlsx";
        //string file = HttpContext.Current.Server.MapPath("~/ExcelFiles");
        //var local = Path.Combine(file, fileName);
        //var remote = Path.Combine(@"D:\", fileName);
        //System.IO.File.Copy(local, remote);


        //AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
        //WindowsIdentity wid = new WindowsIdentity("FO5910155", "Tuf&12345");
        //WindowsImpersonationContext context = wid.Impersonate();
        //string pathToSourceFile = HttpContext.Current.Server.MapPath("~/ExcelFiles/DataFile.xlsx");
        //System.IO.File.Move(pathToSourceFile, @"D:\");

        //context.Undo();

        //string file = @"Z:\ExcelFiles\DataFile.xlsx";
        //string to = @"Z:/ExcelFiles/DataFile.xlsx";
        //System.IO.File.Move(file, to);
        //network.RemoveNetworkDrive("k:");
    }
    public void ExportDataSetToExcel(DataSet ds)
    {
        //string AppLocation = "";
        //AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        //AppLocation = AppLocation.Replace("file:\\", "");
        //string file = AppLocation + "\\ExcelFiles\\DataFile.xlsx";
        string file = HttpContext.Current.Server.MapPath("~/ExcelFiles/DataFile.xlsb");
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(ds.Tables[0]);
            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            wb.Style.Font.Bold = true;
            wb.SaveAs(file);
        }
    }
    [WebMethod]
    public void GetDataReport()
    {
        DataSet ds = null;
        using (SqlConnection con = new SqlConnection(strConn))
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetProblemReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                ExportDataSetToExcel(ds);
                smpcgmail("ExportDataSetToExcel");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ds.Dispose();
            }
        }
    }
    [WebMethod]
    public void smpcgmail(string MailSubject)
    {
        try
        {
            //string MailTo = "Jutharut.Suwankanoknark@thaiunion.com,Thipwal.Khao-orn@thaiunion.com,Tuenjai.Saelee @thaiunion.com";
            string MailTo = "voravut.somboornpong@thaiunion.com";
            //string AppLocation = "";  
            //AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);  
            //AppLocation = AppLocation.Replace("file:\\", "");  
            //string file = AppLocation + "\\ExcelFiles\\DataFile.xlsx";  
            string file = HttpContext.Current.Server.MapPath("~/ExcelFiles/DataFile.xlsx");
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.office365.com");
            mail.From = new MailAddress("Costing.WebBase@thaiunion.com");
            //mail.To.Add(MailTo); // Sending MailTo 
            if (string.IsNullOrEmpty(MailTo)) return;
            string[] words = MailTo.Split(',');
            foreach (string word in words)
            {
                if (!string.IsNullOrEmpty(word))
                    mail.To.Add(new MailAddress(word));
            }
            List<string> li = new List<string>();
            //li.Add("Wandee.Ardkhongharn@thaiunion.com");
            //li.Add("Wasana.Phuangkhajorn@thaiunion.com");  
            li.Add("voravut.somboornpong@thaiunion.com");
            //li.Add("saihacksoft@gmail.com");  
            //li.Add("saihacksoft@gmail.com");  
            mail.CC.Add(string.Join<string>(",", li)); // Sending CC  
            //mail.Bcc.Add(string.Join < string > (",", li)); // Sending Bcc   
            mail.Subject = MailSubject; // Mail Subject  
            string _link = "<br/> Click on the link to download the excel file --------><a href='http://192.168.1.193/lims/file.aspx'";
            _link += " style='color: rgb(0,255,0)'><font color='#0000FF'>click</font></a>";
            mail.Body = _link + "<br/> NCP Report *This is an automatically generated email, please do not reply*";
            mail.IsBodyHtml = true;
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(file); //Attaching File to Mail  
            //mail.Attachments.Add(attachment);
            SmtpServer.Port = 587; //PORT  
            SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential("Costing.WebBase@thaiunion.com", "AAaa123*");
            SmtpServer.Send(mail);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public void Delfilebyte(string Id)
    {
        using (SqlConnection con = new SqlConnection(strConn))
        {
            string query = "delete FileSystem where Id=@Id";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Id", Id.ToString());
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        Context.Response.Write("success");
    }
    [WebMethod]
    public void savefilebyte(Objattachment ro)
    {
        using (SqlConnection con = new SqlConnection(strConn))
        {
            string query = "spinsertFileSystem";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", ro.Name);
                cmd.Parameters.AddWithValue("@ContentType", ro.ContentType);
                cmd.Parameters.AddWithValue("@Data", ro.Data);
                cmd.Parameters.AddWithValue("@MatDoc", ro.MatDoc);
                cmd.Parameters.AddWithValue("@ActiveBy", ro.ActiveBy);
                cmd.Parameters.AddWithValue("@SystemDate", (object)DateTime.Now);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        Context.Response.Write("success");
    }

    [WebMethod]
    public void saveCertEU(string obj)
    {
        dynamic dynJson = JsonConvert.DeserializeObject(obj);

        List<ObjCertEU> item = new List<ObjCertEU>();
        using (SqlConnection con = new SqlConnection(strConn))
        {
            foreach (var ro in dynJson)
            {
                string query = @"insert into transCertEU values (@Material,@Batch,@Plant,@InvoiceNo,format(Getdate(),'dd-MMM-yyyy HH:mm:ss'),@Production,
                    @Quantity,@BaseUoM,@Vessel,@FAO,@Aging,@StatusApp);
                        ";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Material", ro.Material);
                    cmd.Parameters.AddWithValue("@Batch", ro.Batch);
                    cmd.Parameters.AddWithValue("@Plant", ro.Plant);
                    cmd.Parameters.AddWithValue("@InvoiceNo", ro.InvoiceNo);
                    cmd.Parameters.AddWithValue("@DateOfManuf", ro.DateOfManuf);
                    cmd.Parameters.AddWithValue("@Production", ro.Production);
                    cmd.Parameters.AddWithValue("@Quantity", ro.Quantity);
                    cmd.Parameters.AddWithValue("@BaseUoM", ro.BaseUoM);
                    cmd.Parameters.AddWithValue("@Vessel", ro.Vessel);
                    cmd.Parameters.AddWithValue("@FAO", ro.FAO);
                    cmd.Parameters.AddWithValue("@Aging", ro.Aging);
                    cmd.Parameters.AddWithValue("@StatusApp", ro.StatusApp);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        Context.Response.Write("success");
    }

    [WebMethod]
    public string LetterGradeFromNumber(int marks)
    {
        if (marks >= 5001)
            return "8";
        else if (marks >= 1001)
            return "7";
        else if (marks >= 501)
            return "6";
        else if (marks >= 101)
            return "5";
        else if (marks >= 51)
            return "4";
        else if (marks >= 11)
            return "3";
        else if (marks >= 1)
            return "2";
        else
            return "1"; // unclassified
    }
    [WebMethod]
    public string Getdeldocument(string data)
    {
        using (SqlConnection con = new SqlConnection(strConn))
        {
            string query = "delete DataResult where Id=@Id";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Id", data.ToString());
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        return "success";
    }
    [WebMethod]
    public void selectdetail(string data)
    {
        HttpResponse Response = HttpContext.Current.Response;
        Response.ContentType = "application/x-javascript";
        Response.Charset = "utf-8";
        SqlParameter[] param = { new SqlParameter("@data", data.ToString()) };
        DataTable table = new DataTable();
        table = GetRelatedResourcesDb("spGetDetail", param);
        Response.Write(JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented));
        Response.End();
    }
    [WebMethod]
    public void Getplant(string data)
    {
        SqlParameter[] param = { new SqlParameter("@user", data.ToString()) };
        DataTable table = new DataTable();
        table = GetRelatedResourcesDb("spGetPlant", param);
        Context.Response.Write(JsonConvert.SerializeObject(table));
    }
    [WebMethod]
    public void selectanalysisall(objanalysisall item)
    {
        SqlParameter[] param = { new SqlParameter("@data", item.where.ToString()),
                    new SqlParameter("@FrDt",item.FrDt.ToString()),
                    new SqlParameter("@ToDt",item.ToDt.ToString()),
                    new SqlParameter("@username", string.Format("{0}", item.username)),
                    new SqlParameter("@RetortNo",item.RetortNo.ToString()),
                    new SqlParameter("@StResult",item.StResult.ToString()),
                    new SqlParameter("@ToResult",item.ToResult.ToString()),
                    new SqlParameter("@Labanalysis", string.Format("{0}", item.Labanalysis)),
                    new SqlParameter("@TestReport",item.TestReport.ToString()),
                    new SqlParameter("@Record", string.Format("{0}", item.Record)),
                    new SqlParameter("@Plant",item.Plant.ToString()),
                    new SqlParameter("@By",item.By.ToString())};
        DataTable table = new DataTable();
        table = GetRelatedResourcesDb("spselectanalysisall", param);
        Context.Response.Write(JsonConvert.SerializeObject(table));
    }
    [WebMethod]
    public void selectResult(string user, string data)
    {
        SqlParameter[] param = {new SqlParameter("@ID",data),
                        new SqlParameter("@username",user)};
        DataTable table = new DataTable();
        table = GetRelatedResourcesDb("spselectresult", param);
        Context.Response.Write(JsonConvert.SerializeObject(table));
    }
    [WebMethod]
    public void GetinsertResult(objResult item)
    {
            SqlParameter[] param = {new SqlParameter("@ID",item.ID.ToString()),
                        new SqlParameter("@KeyDate",item.KeyDate.ToString()),
                        new SqlParameter("@SampleDate",item.SampleDate.ToString()),
                        new SqlParameter("@PositionID",item.PositionID.ToString()),
                        new SqlParameter("@ProductType",item.ProductType.ToString()),
                        new SqlParameter("@Plant",item.Plant.ToString()),
                        new SqlParameter("@GroupTPC",item.GroupTPC.ToString()),
                        new SqlParameter("@CoolTime",item.CoolTime.ToString()),
                        new SqlParameter("@RetortNo",item.RetortNo.ToString()),
                        new SqlParameter("@Temp",item.Temp.ToString()),
                        new SqlParameter("@ResidualChlorine",item.ResidualChlorine.ToString()),
                        new SqlParameter("@Cook",item.Cook.ToString()),
                        new SqlParameter("@Shift",item.Shift.ToString()),
                        new SqlParameter("@CreateBy",item.CreateBy.ToString()),
                        new SqlParameter("@Employee",item.Employee.ToString()),
                        new SqlParameter("@LabAnalysis",item.LabAnalysis.ToString()),
                        new SqlParameter("@ResultDate",item.ResultDate.ToString()),
                        new SqlParameter("@TestReport",item.TestReport.ToString()),
                        new SqlParameter("@SampleType",item.SampleType.ToString()),
                        new SqlParameter("@Remark",item.Remark.ToString())};
            DataTable table = new DataTable();
            table = GetRelatedResourcesDb("spinsertResult", param);
            Context.Response.Write(JsonConvert.SerializeObject(table));
    }
    [WebMethod]
    public void Insertitems(List<itemObject> data)
    {
        foreach (var item in data)
        {
            //if (first)
            //{
            //    first = false;
            //    SqlCommand sqlComm = new SqlCommand();
            //    sqlComm.CommandText = @"DELETE FROM tbPLCR WHERE RequestNo=@ID";
            //    sqlComm.Parameters.AddWithValue("@ID", item.ID.ToString());
            //    DataTable dt = GetData(sqlComm);
            //    Context.Response.Write(JsonConvert.SerializeObject(dt));
            //}
            SqlParameter[] param = {new SqlParameter("@Name",item.Name.ToString()),
                        new SqlParameter("@Result",item.Result.ToString()),
                        new SqlParameter("@StatusApp",item.StatusApp.ToString()),
                        new SqlParameter("@RequestNo",item.RequestNo.ToString())};
            DataTable table = new DataTable();
            table = GetRelatedResourcesDb("spInsertitems", param);
        }
        Context.Response.Write("sucess");
    }
    [WebMethod]
    public void InsertSaleitem(string data)
    {
        string datapath = "~/Content/" + data + ".json"; bool first = true;
        using (StreamReader sr = new StreamReader(Server.MapPath(datapath)))
        {
            string json = sr.ReadToEnd();
            dynamic dynJson = JsonConvert.DeserializeObject(json);
            foreach (var item in dynJson)
            {
                if (first)
                {
                    first = false;
                    SqlCommand sqlComm = new SqlCommand();
                    sqlComm.CommandText = @"DELETE FROM tbPLCR WHERE RequestNo=@ID";
                    sqlComm.Parameters.AddWithValue("@ID", item.ID.ToString());
                    DataTable dt = GetData(sqlComm);
                    Context.Response.Write(JsonConvert.SerializeObject(dt));
                }
                SqlParameter[] param = {new SqlParameter("@ID",item.ID.ToString()),
                        new SqlParameter("@CODES",item.CODES.ToString()),
                        new SqlParameter("@old_code",item.old_code.ToString()),
                        new SqlParameter("@BATCH",item.BATCH.ToString()),
                        new SqlParameter("@QTY",item.QTY.ToString()),
                        new SqlParameter("@BU",item.BU.ToString()),
                        new SqlParameter("@RK",item.RK.ToString()),
                        new SqlParameter("@Detail",item.Detail.ToString()),
                        new SqlParameter("@LC",item.LC.ToString()),
                        new SqlParameter("@DatePack",item.DatePack.ToString()),
                        new SqlParameter("@QC",item.QC.ToString()),
                        new SqlParameter("@PLANT",item.PLANT.ToString())};
                DataTable table = new DataTable();
                table = GetRelatedResources("spInsertSaleitem", param);
                Context.Response.Write(JsonConvert.SerializeObject(table));
            }
        }
    }
    [WebMethod]
    public void SaveDailyPlan(string data)
    {
        string datapath = "~/ExcelFiles/" + data + ".json"; 
        using (StreamReader sr = new StreamReader(Server.MapPath(datapath)))
        {
            string json = sr.ReadToEnd();
            dynamic dynJson = JsonConvert.DeserializeObject(json);
            foreach (var item in dynJson)
            {
                SqlParameter[] param = {new SqlParameter("@PlanDate",item.PlanDate.ToString()),
                        new SqlParameter("@PlanShift",item.PlanShift.ToString()),
                        new SqlParameter("@PlanNumber",item.PlanNumber.ToString()),
                        new SqlParameter("@ProductCode",item.ProductCode.ToString()),
                        new SqlParameter("@LinePack",item.LinePack.ToString()),
                        new SqlParameter("@LineFill",item.LineFill.ToString()),
                        new SqlParameter("@PrdOrder",item.PrdOrder.ToString()),
                        new SqlParameter("@Prded",item.PrdNoted.ToString()),
                        new SqlParameter("@PrdBatch",item.PrdBatch.ToString()),
                        new SqlParameter("@SubCode",item.SubCode.ToString()),
                        new SqlParameter("@LabelInk",item.LabelInk.ToString()),
                        new SqlParameter("@HourRM",decimal.Parse(item.HourRM.ToString())),

                        new SqlParameter("@TotalRM",decimal.Parse(item.TotalRM.ToString())),
                        new SqlParameter("@ed",item.Noted.ToString()),
                        new SqlParameter("@Quantity",decimal.Parse(item.Quantity.ToString())),
                        new SqlParameter("@PackingFW",decimal.Parse(item.PackingFW.ToString())),
                        new SqlParameter("@PackingNW",decimal.Parse(item.PackingNW.ToString())),
                        new SqlParameter("@RMGrade",item.RMGrade.ToString()),
                        new SqlParameter("@RMGradeType",item.RMGradeType.ToString()),
                        new SqlParameter("@PostingDate",item.PostingDate.ToString()),
                        new SqlParameter("@PostingShift",item.PostingShift.ToString())};
                DataTable table = new DataTable();
                table = GetRelatedResourcesConn("spinsertDailyPlan", param);
                //Context.Response.Write(JsonConvert.SerializeObject(table));
            }
            Context.Response.Write("Success");
        }
    }
    [WebMethod]
    public void savedataOrder(string data)
    {
        string datapath = "~/Content/" + data + ".json";
        using (StreamReader sr = new StreamReader(Server.MapPath(datapath)))
        {
            string json = sr.ReadToEnd();
            dynamic dynJson = JsonConvert.DeserializeObject(json);
            foreach (var item in dynJson)
            {
                SqlParameter[] param = {new SqlParameter("@ID",item.ID.ToString()),
                    new SqlParameter("@Salesorder", item.Salesorder.ToString()),
                new SqlParameter("@Item", item.Item.ToString()),
                new SqlParameter("@Agent", item.Agent.ToString()),
                new SqlParameter("@Brand", item.Brand.ToString()),
                new SqlParameter("@Port", item.Port.ToString()),
                new SqlParameter("@KeyDate", item.KeyDate.ToString()),
                new SqlParameter("@Country", item.Country.ToString()),
                new SqlParameter("@Plant", item.Plant.ToString()),
                new SqlParameter("@Quantity", item.Quantity.ToString()),
                new SqlParameter("@P", item.P.ToString()),

                new SqlParameter("@UserID", item.UserID.ToString()),
                new SqlParameter("@LastProduct", item.LastProduct.ToString()),
                new SqlParameter("@OrderOpen", item.OrderOpen.ToString()),
                new SqlParameter("@MaxCode", item.MaxCode.ToString()),
                new SqlParameter("@Incubation", item.Incubation.ToString()),
                new SqlParameter("@ShelfLife", item.ShelfLife.ToString()),
                new SqlParameter("@DelComplate", item.DelComplate.ToString())};
                DataTable table = new DataTable();
                table = GetRelatedResources("spinsertsaleorder", param);
                Context.Response.Write(JsonConvert.SerializeObject(table));
            }
        }
    }
    [WebMethod]
    public void selectdata(string data)
    {
        data = data.Replace("@", "%");
        SqlCommand cmd = new SqlCommand(data);
        DataTable dt = GetData(cmd);
        Context.Response.Write(JsonConvert.SerializeObject(dt));
    }
    [WebMethod]
    public void selectorder(string Salesdoc, string item)
    {
        SqlParameter[] param = {new SqlParameter("@saleorder",Salesdoc),
                        new SqlParameter("@item",item)};
        DataTable table = new DataTable();
        table = GetRelatedResources("spselectorder", param);
        Context.Response.Write(JsonConvert.SerializeObject(table));
    }
    [WebMethod]
    public void selectdeatil(string Id)
    {
        SqlParameter[] param = { new SqlParameter("@ID", Id) };
        DataTable table = new DataTable();
        table = GetRelatedResources("spselectdeatil", param);
        Context.Response.Write(JsonConvert.SerializeObject(table));
    }

    [WebMethod]
    public void getdeatil(string Id)
    {
        SqlParameter[] param = { new SqlParameter("@ID", Id) };
        DataTable table = new DataTable();
        table = GetRelatedResources("spgetdeatil", param);
        Context.Response.Write(JsonConvert.SerializeObject(table));
    }
    [WebMethod]
    public void selectbatch(string data, string batch)
    {
        data = data.Replace("@", "%");
        SqlParameter[] param = { new SqlParameter("@bc_code", data),
        new SqlParameter("@batch",batch)};
        DataTable table = new DataTable();
        table = GetRelatedResources("spselectbatch", param);
        Context.Response.Write(JsonConvert.SerializeObject(table));
    }
    [WebMethod]
    public void selectall(string data)
    {
        string datapath = "~/Content/" + data + ".json";//DataTable t=new DataTable();
        using (StreamReader sr = new StreamReader(Server.MapPath(datapath)))
        {
            string json = sr.ReadToEnd();
            dynamic dynJson = JsonConvert.DeserializeObject(json);
            foreach (var item in dynJson)
            {
                SqlParameter[] param = { new SqlParameter("@data", item.where.ToString()),
                    new SqlParameter("@FrDt",item.FrDt.ToString()),
                    new SqlParameter("@By",item.By.ToString()),
                    new SqlParameter("@Material",item.Material.ToString()),
                    new SqlParameter("@Batch",item.Batch.ToString()),
                    new SqlParameter("@username",item.username.ToString()),
                    new SqlParameter("@Salesorder",item.Salesorder.ToString()),
                    new SqlParameter("@Items",item.Items.ToString())};
                DataTable table = new DataTable();
                table = GetRelatedResources("spselectall", param);
                Context.Response.Write(JsonConvert.SerializeObject(table));
            }
        }
    }
    public DataTable GetRelatedResourcesDb(string StoredProcedure, object[] Parameters)
    {
        var Results = new DataTable();
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
            Context.Response.Write("Exception Executing Stored Procedure:" + ex.Message);
        }

        return Results;
    }
    public DataTable GetRelatedResourcesConn(string StoredProcedure, object[] Parameters)
    {
        var Results = new DataTable();
        String strConnString = System.Configuration.ConfigurationManager.
            ConnectionStrings["TunaRunConnectionString"].ConnectionString;
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
            Context.Response.Write("Exception Executing Stored Procedure:" + ex.Message);
        }

        return Results;
    }
    public DataTable GetRelatedResources(string StoredProcedure, object[] Parameters)
    {
        var Results = new DataTable();
        String strConnString = System.Configuration.ConfigurationManager.
            ConnectionStrings["WareHouseConnectionString"].ConnectionString;
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
            Context.Response.Write("Exception Executing Stored Procedure:" + ex.Message);
        }

        return Results;
    }
    public DataTable GetData(SqlCommand cmd)
    {
        DataTable dt = new DataTable();
        String strConnString = System.Configuration.ConfigurationManager.
             ConnectionStrings["WareHouseConnectionString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);
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
    [WebMethod]
    public void Getdelete(string data)
    {
        string datapath = "~/Content/" + data + ".json";
        using (StreamReader sr = new StreamReader(Server.MapPath(datapath)))
        {
            string json = sr.ReadToEnd();
            dynamic dynJson = JsonConvert.DeserializeObject(json);
            foreach (var item in dynJson)
            {
                using (SqlConnection con = new SqlConnection(strConn))
                {
                    string query = @"insert into tblChangeResult values (@Id,'Delete','',@user,format(Getdate(),'dd-MMM-yyyy HH:mm:ss'));
                        update tblncp set Active=1,modifyBy=@user,modifyOn=getdate() where Id=@Id";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Id", (int)item.Name);
                        cmd.Parameters.AddWithValue("@user", item.user.ToString());
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    //SqlCommand cmd = new SqlCommand();
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "spdelete";
                    //cmd.Parameters.AddWithValue("@Id", item.Name.ToString());
                    //cmd.Connection = con;
                    //con.Open();
                    //DataTable dt = new DataTable();
                    //SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
                    //oAdapter.Fill(dt);
                    //con.Close();
                }
                //SqlConnection sqlConn = new SqlConnection(strConn);
                //SqlCommand sqlComm = new SqlCommand();
                //sqlComm = sqlConn.CreateCommand();
                //sqlComm.CommandText = @"DELETE FROM tblncp WHERE ID='@Id'";
                //sqlComm.Parameters.AddWithValue("@Id", item.Name);
                //sqlConn.Open();
                //sqlComm.ExecuteNonQuery();
                //sqlConn.Close();

            }
        }
        Context.Response.Write("success");
    }
    [WebMethod]
    public void Getcopy(string sName, string suser, string num)
    {
        using (SqlConnection con = new SqlConnection(strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spcopy";
            cmd.Parameters.AddWithValue("@Id", sName);
            cmd.Parameters.AddWithValue("@user", suser);
            cmd.Parameters.AddWithValue("@num", Int32.Parse(num));
            cmd.Connection = con;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
            oAdapter.Fill(dt);
            con.Close();
            //Context.Response.Write(JsonConvert.SerializeObject(dt));
            Context.Response.Write("success");
        }
    }
    [WebMethod]
    public void GetHistory(string sName)
    {
        using (SqlConnection con = new SqlConnection(strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spHistory";
            cmd.Parameters.AddWithValue("@Id", sName);
            cmd.Connection = con;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
            oAdapter.Fill(dt);
            con.Close();
            Context.Response.Write(JsonConvert.SerializeObject(dt));
        }
    }
    //[WebMethod]
    //public int SaveDocument(Byte[] filebyte)
    //{
    //    int adInteger; 
    //    using (SqlConnection con = new SqlConnection(strConn))
    //    {
    //        using (SqlCommand cmd = new SqlCommand("INSERT INTO masmaterial(docbinaryarray)  VALUES(@docbinaryarray);SELECT SCOPE_IDENTITY();",con))
    //        {
    //            cmd.CommandType = CommandType.Text;
    //            cmd.Parameters.AddWithValue("@docbinaryarray", filebyte);
    //            con.Open();
    //            adInteger = (int)cmd.ExecuteScalar();

    //            if (con.State == System.Data.ConnectionState.Open) con.Close();
    //            return adInteger;
    //        }
    //    }
    //}
    //[WebMethod]
    //    public void Getselectsummaryall(string FrDt, string where,string By,string Batch,string Material,string username,string decision,string FirstDecision)
    //    {
    //        using (SqlConnection con = new SqlConnection(strConn))
    //        {
    //            SqlCommand cmd = new SqlCommand();
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.CommandText = "spselectsummaryall";
    //            cmd.Parameters.AddWithValue("@FrDt", FrDt);
    //            cmd.Parameters.AddWithValue("@where", where);
    //            cmd.Parameters.AddWithValue("@By", By);
    //            cmd.Parameters.AddWithValue("@Material", Material);
    //            cmd.Parameters.AddWithValue("@Batch", Batch);
    //            cmd.Parameters.AddWithValue("@username", username);
    //            cmd.Parameters.AddWithValue("@decision", decision);
    //            cmd.Parameters.AddWithValue("@FirstDecision", FirstDecision);
    //            cmd.Connection = con;
    //            con.Open();
    //            DataTable dt = new DataTable();
    //            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
    //            oAdapter.Fill(dt);
    //            con.Close();
    //            Context.Response.Write(JsonConvert.SerializeObject(dt));
    //        }
    //    }
  [WebMethod]
    public void Getselectsummaryall(string data)
    {
        HttpResponse Response = HttpContext.Current.Response;
        Response.ContentType = "application/x-javascript";
        Response.Charset = "utf-8";
        string datapath = "~/Content/" + data + ".json";//DataTable t=new DataTable();
        using (StreamReader sr = new StreamReader(Server.MapPath(datapath)))
        {
            string json = sr.ReadToEnd();
            dynamic dynJson = JsonConvert.DeserializeObject(json);
            foreach (var item in dynJson)
            {
                //Context.Response.Write(item.FrDt +','+ item.where+','+ item.By+','+ item.Material+','+ item.Batch+','+ item.username+','+ item.decision+','+ item.FirstDecision);
                using (SqlConnection con = new SqlConnection(strConn))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spselectsummaryall";
                    cmd.Parameters.AddWithValue("@FrDt", string.Format("{0}", item.FrDt));
                    cmd.Parameters.AddWithValue("@ToDt", string.Format("{0}", item.ToDt));
                    cmd.Parameters.AddWithValue("@Plant", string.Format("{0}", item.Plant));
                    cmd.Parameters.AddWithValue("@where", string.Format("{0}", item.where));
                    cmd.Parameters.AddWithValue("@By", string.Format("{0}", item.By));
                    cmd.Parameters.AddWithValue("@Material", string.Format("{0}", item.Material));
                    cmd.Parameters.AddWithValue("@Batch", string.Format("{0}", item.Batch));
                    cmd.Parameters.AddWithValue("@username", string.Format("{0}", item.username));
                    cmd.Parameters.AddWithValue("@decision", string.Format("{0}", item.decision));
                    cmd.Parameters.AddWithValue("@FirstDecision", string.Format("{0}", item.FirstDecision));
                    cmd.Parameters.AddWithValue("@record", string.Format("{0}", item.Record));
                    cmd.Parameters.AddWithValue("@Packaging", string.Format("{0}", item.Packaging));
                    cmd.Parameters.AddWithValue("@Batch_Packaging", string.Format("{0}", item.Batch_Packaging));
                    cmd.Parameters.AddWithValue("@NCPType", string.Format("{0}", item.NCPType));
                    cmd.Parameters.AddWithValue("@Location", string.Format("{0}", item.Location));
                    cmd.Parameters.AddWithValue("@Line", string.Format("{0}", item.LineNo));
                    cmd.Parameters.AddWithValue("@Times", string.Format("{0}", item.Times));
                    cmd.Parameters.AddWithValue("@Shift", string.Format("{0}", item.Shift));
                    cmd.Parameters.AddWithValue("@Product", string.Format("{0}", item.Product));
                    cmd.Parameters.AddWithValue("@Problem", string.Format("{0}", item.Problem));
                    cmd.Connection = con;
                    con.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
                    oAdapter.Fill(dt);
                    con.Close();
                    //t= dt;
                    Response.Write(JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented));
                    Response.End();
                }
            }
        }
        //return t;
    }
    [WebMethod]
    public void Getselectsummaryall_test(string data)
    {
        HttpResponse Response = HttpContext.Current.Response;
        Response.ContentType = "application/x-javascript";
        Response.Charset = "utf-8";
        string datapath = "~/Content/" + data + ".json";//DataTable t=new DataTable();
        using (StreamReader sr = new StreamReader(Server.MapPath(datapath)))
        {
            string json = sr.ReadToEnd();
            dynamic dynJson = JsonConvert.DeserializeObject(json);
            foreach (var item in dynJson)
            {
                //Context.Response.Write(item.FrDt +','+ item.where+','+ item.By+','+ item.Material+','+ item.Batch+','+ item.username+','+ item.decision+','+ item.FirstDecision);
                using (SqlConnection con = new SqlConnection(strConn))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "spselectsummaryall3";
                    cmd.Parameters.AddWithValue("@FrDt", string.Format("{0}", item.FrDt));
                    cmd.Parameters.AddWithValue("@ToDt", string.Format("{0}", item.ToDt));
                    cmd.Parameters.AddWithValue("@Plant", string.Format("{0}", item.Plant));
                    cmd.Parameters.AddWithValue("@where", string.Format("{0}", item.where));
                    cmd.Parameters.AddWithValue("@By", string.Format("{0}", item.By));
                    cmd.Parameters.AddWithValue("@Material", string.Format("{0}", item.Material));
                    cmd.Parameters.AddWithValue("@Batch", string.Format("{0}", item.Batch));
                    cmd.Parameters.AddWithValue("@username", string.Format("{0}", item.username));
                    cmd.Parameters.AddWithValue("@decision", string.Format("{0}", item.decision));
                    cmd.Parameters.AddWithValue("@FirstDecision", string.Format("{0}", item.FirstDecision));
                    cmd.Parameters.AddWithValue("@record", string.Format("{0}", item.Record));
                    cmd.Parameters.AddWithValue("@Packaging", string.Format("{0}", item.Packaging));
                    cmd.Parameters.AddWithValue("@Batch_Packaging", string.Format("{0}", item.Batch_Packaging));
                    cmd.Parameters.AddWithValue("@NCPType", string.Format("{0}", item.NCPType));
                    cmd.Parameters.AddWithValue("@Location", string.Format("{0}", item.Location));
                    cmd.Parameters.AddWithValue("@Line", string.Format("{0}", item.LineNo));
                    cmd.Parameters.AddWithValue("@Times", string.Format("{0}", item.Times));
                    cmd.Parameters.AddWithValue("@Shift", string.Format("{0}", item.Shift));
                    cmd.Parameters.AddWithValue("@Product", string.Format("{0}", item.Product));
                    cmd.Parameters.AddWithValue("@Problem", string.Format("{0}", item.Problem));
                    cmd.Connection = con;
                    con.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
                    oAdapter.Fill(dt);
                    con.Close();
                    //t= dt;
                    Response.Write(JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented));
                    Response.End();
                }
            }
        }
        //return t;
    }
    [WebMethod]
    public void Getselectsummary(string FrDt, string where, string By, string Batch, string Material)
    {
        using (SqlConnection con = new SqlConnection(strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spselectsummary";
            cmd.Parameters.AddWithValue("@FrDt", FrDt);
            cmd.Parameters.AddWithValue("@where", where);
            cmd.Parameters.AddWithValue("@By", By);
            cmd.Parameters.AddWithValue("@Material", Material);
            cmd.Parameters.AddWithValue("@Batch", Batch);
            cmd.Connection = con;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
            oAdapter.Fill(dt);
            con.Close();
            Context.Response.Write(JsonConvert.SerializeObject(dt));
        }
    }
    [WebMethod]
    public void GetselectMaterial(string n, string Keyword)
    {
        HttpResponse Response = HttpContext.Current.Response;
        Response.ContentType = "application/x-javascript";
        Response.Charset = "utf-8";
        using (SqlConnection con = new SqlConnection(strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spselectMaterial";
            cmd.Parameters.AddWithValue("@n", n);
            cmd.Parameters.AddWithValue("@Keyword", Keyword);
            cmd.Connection = con;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
            oAdapter.Fill(dt);
            con.Close();
            Response.Write(JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented));
            Response.End();
        }
    }
    [WebMethod]
    public void DelFile(string Id)
    {
        using (SqlConnection con = new SqlConnection(strConn))
        {
            string query = "delete tblFiles where Id=@Id";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Id", Id.ToString());
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        Context.Response.Write("success");
    }
    [WebMethod]
    public void savebyte(Objattachment ro)
    {
            using (SqlConnection con = new SqlConnection(strConn))
            {
                string query = @"insert into tblFiles values (@Name,@ContentType,@Data,@MatDoc,@ActiveBy)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Name", ro.Name);
                    cmd.Parameters.AddWithValue("@ContentType", ro.ContentType);
                    cmd.Parameters.AddWithValue("@Data", ro.Data);
                    cmd.Parameters.AddWithValue("@MatDoc", ro.MatDoc);
                    cmd.Parameters.AddWithValue("@ActiveBy", ro.ActiveBy);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        Context.Response.Write("success");
    }
    [WebMethod]
    public string formatbatchsap(string FrDt, string keydate, string Shift, string Line, string Time)
    {
        //YMDSLYMDN
        string name = ""; string b = "";
        string[] array = Regex.Split("4YMD;SLA;M(M);D;S", ";");
        foreach (string arr in array)
        {
            DateTime date;
            if (arr == "4YMD")
                date = DateTime.ParseExact(FrDt, "yyyyMMdd", null, DateTimeStyles.None);
            else
                date = DateTime.ParseExact(FrDt, "yyyyMMdd", null, DateTimeStyles.None);
            name = string.Format("{0}", arr);
            if (!string.IsNullOrEmpty(Time))
            {
                b += Structurename(name, date, Time, Line);
            }
            else
                b += Structurename(name, date, Shift, Line);
        }
        int number;
        bool isNumeric = int.TryParse(Shift, out number);
        if (isNumeric)
            b = ReplaceAt(b, 9, 1, "1");
        else
            b = ReplaceAt(b, 9, 1, "A");
      return b;
    }
    public static string ReplaceAt(string str, int index, int length, string replace)
    {
        if ((Convert.ToInt32(str.Length) - index) < 0) return "0";
        return str.Remove(index, Math.Min(length, str.Length - index))
                .Insert(index, replace);
 
    }
    [WebMethod]
    public string nameformat(nameformatObject items)
    {
        DataTable table = GetGridData();
        //spGetnameformat
        SqlParameter[] param = { new SqlParameter("@customer", items.customer.ToString()) };
        DataTable dt = cs.GetRelatedResources("spGetnameformat", param);
        string copy = "";
        DateTime date = DateTime.ParseExact(items.FrDt, "yyyyMMdd", null, DateTimeStyles.None);
        foreach (DataRow dr in dt.Rows)
        {
            copy = Structurename(dr["Name"].ToString(), date, items.ShiftOpt, items.Line);
        }
 
    if (items.ShiftOpt == "X") {
    string value = items.Shift =="DS"?"1":"A";
	copy = Getpositionstuff(copy,10,1,value);
	}
    //Replace("alphabet", "a", "e", 1, 1)
     return copy;
    }
    public string Structurename(string name, DateTime date, string Shift, string Line)
    {
        string strQuery = "";
        string copy = ""; string yourInt = ""; int pos = 1;
        copy = name.ToString();
        string s = "P|PP|PPP|PPPP|Y|YY|YYYY|M|MM|M(M)|MMM|D|D(D)|DD|S|L|LL|T|O|JJJ|L|LL|S";
        string[] words = s.Split('|');
        foreach (string word in words)
        {
            if (name.Contains(word))
            {
                int index = name.IndexOf(word);
                //Console.WriteLine(word);
                while (index != -1)
                {
                    switch (word)
                    {
                        case "Y":
                        case "YY":
                        case "YYYY":
                            //case when '" + word.ToString() + "'='Y' then Id " + 
                            strQuery = @"select case '" + word.ToString() + "' when 'Y' then convert(nvarchar,id)  when 'YY' then format(cast('" + date + "' as date),'yy') when 'YYYY' then [SName] end 'Id'" +
                                " from tblyear where sname='" + date.Year + "'";
                            foreach (DataRow row in builditems(strQuery).Rows)
                            {
                                copy = Getpositionstuff(copy, index + pos, (int)word.Length, row["Id"].ToString());
                            }
                            break;
                        case "M":
                        case "MMM":
                            foreach (DataRow row in builditems(@"select case when '" + word.ToString() + "' ='MMM' then Title else sname end 'sname' from tblmonth where Id='" + date.Month + "'").Rows)
                            {
                                copy = Getpositionstuff(copy, index + pos, (int)word.Length, row["sname"].ToString());
                            }
                            break;
                        case "MM":
                            yourInt = string.Format("{0:D2}", date.Month);
                            copy = Getpositionstuff(copy, index + pos, (int)word.Length, yourInt);
                            break;
                        case "M(M)":
                            foreach (DataRow row in builditems("select Mtext from tblmonth where Id='" + date.Month + "'").Rows)
                                copy = Getpositionstuff(copy, index + pos, (int)word.Length, row["Mtext"].ToString());
                            pos = (-3) + pos;
                            break;
                        case "D":
                            strQuery = "select case '" + word.ToString() + "' when 'D' then name when 'D(D)' then convert(nvarchar,id) when 'DD' then format(cast('" + date + "' as date),'dd') end 'name'" +
                                " from tblDate where Id='" + date.Day + "'";
                            foreach (DataRow row in builditems(strQuery).Rows)
                            {
                                copy = Getpositionstuff(copy, index + pos, (int)word.Length, row["name"].ToString());
                            }
                            break;
                        case "D(D)":
                            copy = Getpositionstuff(copy, index + pos, (int)word.Length, date.Day.ToString());
                            pos = (-3) + pos;
                            break;
                        case "DD":
                            yourInt = string.Format("{0:D2}", date.Day);
                            copy = Getpositionstuff(copy, index + pos, (int)word.Length, yourInt);
                            break;
                        case "JJJ":
                            strQuery = "select datepart(dy, '" + date + "') as 'Julian'";
                            foreach (DataRow row in builditems(strQuery).Rows)
                            {
                                yourInt = string.Format("{0:D3}", row["Julian"]);
                                copy = Getpositionstuff(copy, index + pos, (int)word.Length, yourInt);
                            }
                            break;
                        case "S":
                            copy = Getpositionstuff(copy, index + pos, (int)word.Length, Shift.ToString());
                            break;
                        //case "DD":
                        //    break;
                        case "L":
                        case "LL":
                            strQuery = "select case '" + word + "' when 'L' then Lname when 'LL' then L2 end 'name' from tblline where Lname='" + Line + "'";
                            foreach (DataRow row in builditems(strQuery).Rows)
                            {
                                copy = Getpositionstuff(copy, index + pos, (int)word.Length, row["name"].ToString());
                            }
                            break;
                    }
                    index = name.IndexOf(word, index + 1);
                }
            }
        }
        return copy;
    }
    private DataTable GetGridData()
    {
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[3] { new DataColumn("ID", typeof(int)),
                            new DataColumn("Component", typeof(string)),
                            new DataColumn("Code", typeof(string)) });
        dt.PrimaryKey = new DataColumn[] { dt.Columns["ID"] };
        return dt;
    }
    [WebMethod]
    public string Getpositionstuff(string oldData, int stuff, int n, string str)
    {
        using (SqlConnection con = new SqlConnection(strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sppositionstuff";
            cmd.Parameters.AddWithValue("@oldData", oldData);
            cmd.Parameters.AddWithValue("@n", n);
            cmd.Parameters.AddWithValue("@str", str);
            cmd.Parameters.AddWithValue("@stuff", stuff);
            cmd.Connection = con;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
            oAdapter.Fill(dt);
            string s = null;
            foreach (DataRow dr in dt.Rows)
            {
                s = dr["Name"].ToString();
            }
            con.Close();
            //Context.Response.Write(JsonConvert.SerializeObject(dt));
            return s;
        }
    }
    [WebMethod]
    public void Getulogin(string username, string password)
    {
        using (SqlConnection con = new SqlConnection(strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spulogin";
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Connection = con;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
            oAdapter.Fill(dt);
            con.Close();
            Context.Response.Write(JsonConvert.SerializeObject(dt));
        }
    }

    public DataTable builditems(string data)
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
            return dt;
        }
    }

    [WebMethod]
    public string GetDetails(string name, int age)
    {
        //return string.Format("Name: {0}{2}Age: {1}{2}TimeStamp: {3}", name, age, Environment.NewLine, DateTime.Now.ToString());
        //string NowPeriodDate = null;
        System.Globalization.CultureInfo USCulture = new System.Globalization.CultureInfo("en-US", true);

        return DateTime.Now.ToString("yyyyMMdd", USCulture);

    }
    [WebMethod()]
    public void GetExport(string Id)
    {
        using (SqlConnection con = new SqlConnection(strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spexport";
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Connection = con;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
            oAdapter.Fill(dt);
            con.Close();
            Context.Response.Write(JsonConvert.SerializeObject(dt));
        }
    }
    //[WebMethod()]
    //public string[] QueryData(string strtable)
    //{
    //    int i = 0;
    //    string sql = null;
    //    SqlConnection oConn = new SqlConnection(strConn);
    //    oConn.Open();
    //    sql = "SELECT * FROM " + strtable;
    //    System.Data.DataSet oDataset = new System.Data.DataSet();
    //    SqlDataAdapter oAdapter = new SqlDataAdapter(sql, oConn);
    //    oAdapter.Fill(oDataset);
    //    string[] s = new string[oDataset.Tables[0].Rows.Count];
    //    for (i = 0; i <= oDataset.Tables[0].Rows.Count - 1; i++)
    //    {
    //        s[i] = oDataset.Tables[0].Rows[i].ItemArray[0].ToString();
    //    }
    //    oConn.Close();
    //    return s;
    //}
    [WebMethod]
    public void InsertProblem(List<objectProblem> dynJson)
    {
        bool first = true;
        foreach (var item in dynJson)
        {
            using (SqlConnection con = new SqlConnection(strConn))
            {
                if (first)
                {
                    first = false;
                    SqlCommand sqlComm = new SqlCommand();
                    sqlComm = con.CreateCommand();
                    sqlComm.CommandText = @"DELETE FROM tblDetail WHERE RequestNo=@ID";
                    sqlComm.Parameters.AddWithValue("@ID", item.ID.ToString());
                    con.Open();
                    sqlComm.ExecuteNonQuery();
                    con.Close();
                }
                string query = "spinsertDetail";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", item.ID.ToString());
                    cmd.Parameters.AddWithValue("@Problem", item.Problem.ToString());
                    cmd.Parameters.AddWithValue("@Detail", item.Detail.ToString());
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    //con.Open();
                    //DataTable dt = new DataTable();
                    //SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
                    //oAdapter.Fill(dt);
                    //con.Close();
                    //Context.Response.Write(JsonConvert.SerializeObject(dt));
                    Context.Response.Write("success");
                }
            }
        }
    }
    [WebMethod()]
    public DataSet GetDataSet(string sName)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        var args = sName.ToString().Split('|');
 
                switch (args[0]){
                case "Location":
                    SqlParameter[] param = { new SqlParameter("@plant", args[2].ToString()),
                    new SqlParameter("@type",args[1].ToString())};
                    //DataTable table = new DataTable();
                    dt = GetRelatedResourcesDb("spGetLocation", param);
                    ds.Tables.Add(dt);       //break;
                     return ds;
                case "Problem":
                    sName = @"(select Title,LongText from tblProblem  a where a.Location like '%'+(select location from tbltype
                        where  substring(Title,1,3) =  substring('" + args[1] + "',1,3))+'%')#a";
                    break;
                case "Shift":
                    sName = @"(select Id, Name from tblShift where '" + args[1] + "' in (select value from dbo.FNC_SPLIT(plant, ';')))#a";
                    break;
                case "Line":
                    sName = @"(select * from tblLine where '" + args[1] + "' in (select value from dbo.FNC_SPLIT(plant,';')))#a";
                    break;
                case "Approve1":
                case "Approve2":
                    sName = @"(select [user_name],FirstName,LastName,FirstName + ' ' + LastName fn from ulogin where '" + args[1] + "' in (select value from dbo.FNC_SPLIT(plant,';'))" +
                            "and ApprovedStep like (case when '" + args[0] + "'='Approve1' then '%1%' else '%2%' end))#a";
                    break;
                case "Recorder":
                    sName = @"(select Id, Name from tblRecorder where '" + args[1] + "' in (select value from dbo.FNC_SPLIT(plant, ';')))#a";
                    break;
                case "employee":
                    sName = @"(select * from MasEmployee where '" + args[1] + "' in (select value from dbo.FNC_SPLIT(plant, ';')))#a";
                break;
            }
            using (SqlConnection oConn = new SqlConnection(strConn))
            {
                oConn.Open();
                sName = sName.Replace("@", "%");
                string strQuery = "select * from " + sName;
                //DataSet oDataset = new DataSet();
                //DataTable dt = new DataTable();
                SqlDataAdapter oAdapter = new SqlDataAdapter(strQuery, oConn);
                // Fill the dataset.
                //oAdapter.Fill(oDataset);
                oAdapter.Fill(dt);
                oConn.Close();
                ds.Tables.Add(dt);
                return ds;
            }
        }
    [WebMethod()]
    public void Getjson(string sName)
    {
        HttpResponse Response = HttpContext.Current.Response;
        Response.ContentType = "application/x-javascript";
        Response.Charset = "utf-8";
        using (SqlConnection oConn = new SqlConnection(strConn))
        {
            oConn.Open();
            sName = sName.Replace("@", "%");
            string strQuery = "select * from " + sName;
            DataTable dt = new DataTable();
            SqlDataAdapter oAdapter = new SqlDataAdapter(strQuery, oConn);
            // Fill the dataset.
            oAdapter.Fill(dt);
            oConn.Close();
            //Context.Response.Write(JsonConvert.SerializeObject(dt));
            Response.Write(JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented));
            Response.End();
        }
    }

    [WebMethod]
    public void selectncpid(string user, string data)
    {
        SqlParameter[] param = {new SqlParameter("@ID",data),
                        new SqlParameter("@username",user)};
        DataTable table = new DataTable();
        table = GetRelatedResourcesDb("spselectncpid", param);
        Context.Response.Write(JsonConvert.SerializeObject(table));
    }
    [WebMethod()]
    public void savedocument(RootObject ro)
    {
        DataTable dt = new DataTable();
        dt = savedata(ro);
        Context.Response.Write(JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented));
        Context.Response.End();
    }
    public DataTable savedata(RootObject ro)
    {
        string sp = "spsavedocument";
        HttpResponse Response = HttpContext.Current.Response;
        Response.ContentType = "application/x-javascript";
        Response.Charset = "utf-8";
        using (SqlConnection con = new SqlConnection(strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sp;
            cmd.Parameters.AddWithValue("@Id", string.Format("{0}", ro.ID));
            cmd.Parameters.AddWithValue("@ncptype", string.Format("{0}", ro.ncptype));
            cmd.Parameters.AddWithValue("@ncpid", string.Format("{0}", ro.ncpid));
            cmd.Parameters.AddWithValue("@Problem", string.Format("{0}", ro.Problem));
            cmd.Parameters.AddWithValue("@ProblemQty", string.Format("{0}", ro.ProblemQty));
            cmd.Parameters.AddWithValue("@FirstDecision", string.Format("{0}", ro.FirstDecision));
            cmd.Parameters.AddWithValue("@Decision", string.Format("{0}", ro.Decision));
            cmd.Parameters.AddWithValue("@KeyDate", string.Format("{0}", ro.KeyDate));
            cmd.Parameters.AddWithValue("@Location", string.Format("{0}", ro.Location));
            cmd.Parameters.AddWithValue("@Plant", string.Format("{0}", ro.Plant));
            cmd.Parameters.AddWithValue("@MaterialType", string.Format("{0}", ro.MaterialType));
            cmd.Parameters.AddWithValue("@BatchCode", string.Format("{0}", ro.BatchCode));
            cmd.Parameters.AddWithValue("@Product", string.Format("{0}", ro.Product));
            cmd.Parameters.AddWithValue("@Batchsap", string.Format("{0}", ro.Batchsap));
            cmd.Parameters.AddWithValue("@Active", string.Format("{0}", ro.Active));
            cmd.Parameters.AddWithValue("@Material", string.Format("{0}", ro.Material));
            cmd.Parameters.AddWithValue("@ProductionDate", string.Format("{0}", ro.ProductionDate));
            cmd.Parameters.AddWithValue("@Quantity", string.Format("{0}", ro.Quantity));
            cmd.Parameters.AddWithValue("@Shift", string.Format("{0}", ro.Shift));
            cmd.Parameters.AddWithValue("@HoldQuantity", string.Format("{0}", ro.HoldQuantity));
            cmd.Parameters.AddWithValue("@Action", string.Format("{0}", ro.Action));
            cmd.Parameters.AddWithValue("@Remark", string.Format("{0}", ro.Remark));
            cmd.Parameters.AddWithValue("@Approve", string.Format("{0}", ro.Approve));
            cmd.Parameters.AddWithValue("@Approvefinal", string.Format("{0}", ro.Approvefinal));
            cmd.Parameters.AddWithValue("@user", string.Format("{0}", ro.user));
            cmd.Parameters.AddWithValue("@LinesNo", string.Format("{0}", ro.LinesNo));
            cmd.Parameters.AddWithValue("@ShiftOption", string.Format("{0}", ro.ShiftOption));
            cmd.Parameters.AddWithValue("@Times", string.Format("{0}", ro.Times));
            cmd.Parameters.AddWithValue("@ResultDecision", string.Format("{0}", ro.ResultDecision));

            cmd.Parameters.AddWithValue("@Supplier", string.Format("{0}", ro.Supplier));
            cmd.Parameters.AddWithValue("@Packaging", string.Format("{0}", ro.Packaging));
            cmd.Parameters.AddWithValue("@Batch_Packaging", string.Format("{0}", ro.Batch_Packaging));
            cmd.Parameters.AddWithValue("@Recorder", string.Format("{0}", ro.Recorder));

            cmd.Connection = con;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
            oAdapter.Fill(dt);
            con.Close();
            return dt;
        }
    }
    [WebMethod]
    public void Getpersonal(string sName, string sCondition, string sCurrent)
    {
        using (SqlConnection con = new SqlConnection(strConn))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetpersonal";
            cmd.Parameters.AddWithValue("@name", sName);
            cmd.Parameters.AddWithValue("@current", sCurrent);
            cmd.Parameters.AddWithValue("@condition", sCondition);
            cmd.Connection = con;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter oAdapter = new SqlDataAdapter(cmd);
            oAdapter.Fill(dt);
            con.Close();
            Context.Response.Write(JsonConvert.SerializeObject(dt));
        }
    }
    [WebMethod()]
    public string saveCreateroot(string data)
    {
        string sp = "spCreateDocument";
        string datapath = "~/Content/" + data + ".json";
        using (StreamReader sr = new StreamReader(Server.MapPath(datapath)))
        {
            string json = sr.ReadToEnd();
            dynamic dynJson = JsonConvert.DeserializeObject(json);
            foreach (var item in dynJson)
            {
                //Console.WriteLine("{0} {1} {2} {3}\n", item.Id, item.Material,
                //    item.ProductName, item.user);
                sp = item.Material;
            }
            //List<CreateDocument> ro = JsonConvert.DeserializeObject<List<CreateDocument>>(json);
            ////Context.Response.Write(ro[0].Requester);
            //using (SqlConnection con = new SqlConnection(strConn))
            //{
            //    using (SqlCommand cmd = new SqlCommand(sp))
            //    {
            //        using (SqlDataAdapter sda = new SqlDataAdapter())
            //        {
            //            DataTable dt = new DataTable();
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.AddWithValue("@CreateBy", ro[0].CreateBy);
            //            cmd.Parameters.AddWithValue("@Code", ro[0].Code);
            //            cmd.Parameters.AddWithValue("@Condition", ro[0].Condition);
            //            cmd.Connection = con;
            //            sda.SelectCommand = cmd;
            //            sda.Fill(dt);
            //            Context.Response.Write(JsonConvert.SerializeObject(dt));
            //        }
            //    }
            //}
        }
        return sp;
    }
    [WebMethod]
    public void savechangeresult(List<savechangeObject> reObject)
    {
        foreach (var item in reObject)
        {
            using (SqlConnection con = new SqlConnection(strConn))
            {
                string query = "insert into tblChangeResult values (@MatDoc,@Name,@Result,@ActiveBy,format(Getdate(),'dd-MMM-yyyy HH:mm:ss'))";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Name", item.Name.ToString());
                    cmd.Parameters.AddWithValue("@Result", item.Result.ToString());
                    cmd.Parameters.AddWithValue("@MatDoc", item.Matdoc.ToString());
                    cmd.Parameters.AddWithValue("@ActiveBy", item.Activeby.ToString());
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        Context.Response.Write("success");
    }
}
public class objanalysisall
{
    public string where { get; set; }
    public string FrDt { get; set; }
    public string ToDt { get; set; }
    public string username { get; set; }
    public string RetortNo { get; set; }
    public string StResult { get; set; }
    public string ToResult { get; set; }
    public string Labanalysis { get; set; }
    public string TestReport { get; set; }
    public string Record { get; set; }
    public string Plant { get; set; }
    public string By { get; set; }
}
public class objResult
{
    public string ID { get; set; }
    public string KeyDate { get; set; }
    public string SampleDate { get; set; }
    public string PositionID { get; set; }
    public string ProductType { get; set; }
    public string Plant { get; set; }
    public string GroupTPC { get; set; }
    public string CoolTime { get; set; }
    public string RetortNo { get; set; }
    public string Temp { get; set; }
    public string ResidualChlorine { get; set; }
    public string Cook { get; set; }
    public string Shift { get; set; }
    public string CreateBy { get; set; }
    public string Employee { get; set; }
    public string LabAnalysis { get; set; }
    public string ResultDate { get; set; }
    public string TestReport { get; set; }
    public string SampleType { get; set; }
    public string Remark { get; set; }
}
public class objectProblem
{
    public string ID { get; set; }
    public string Problem { get; set; }
    public string Detail { get; set; }
}
public class savechangeObject
{
    public string Name { get; set; }
    public string Result { get; set; }
    public string Matdoc { get; set; }
    public string Activeby { get; set; }
}
public class itemObject
{
    public string Name { get; set; }
    public string Result { get; set; }
    public string RequestNo { get; set; }
    public string StatusApp { get; set; }
}
public class soapActionObject
{
    public string FrDt { get; set; }
    public string ToDt { get; set; }
    public string Plant { get; set; }

    public string where { get; set; }
    public string By { get; set; }
    public string Material { get; set; }
    public string Batch { get; set; }
    public string username { get; set; }
    public string decision { get; set; }
    public string FirstDecision { get; set; }
    public string Record { get; set; }
    public string Packaging { get; set; }
    public string Batch_Packaging { get; set; }
    public string NCPType { get; set; }
    public string Location { get; set; }
    public string LineNo { get; set; }
    public string Times { get; set; }
    public string Shift { get; set; }
    public string Product { get; set; }
    public string Problem { get; set; }
}
public class ObjCertEU
{
    public string Material { get; set; }
    public string Batch { get; set; }
    public string Plant { get; set; }
    public string InvoiceNo { get; set; }
    public string DateOfManuf { get; set; }
    public string Production { get; set; }
    public string Quantity { get; set; }

    public string BaseUoM { get; set; }
    public string Vessel { get; set; }
    public string FAO { get; set; }
    public string Aging { get; set; }
    public string StatusApp { get; set; }
}
public class Objattachment
{
    public int MatDoc { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Data { get; set; }
    //public string Data { get; set; }
    public string ActiveBy { get; set; }
}
public class nameformatObject
{
    public string customer { get; set; }
    public string FrDt { get; set; }
    public string ShiftOpt { get; set; }
    public string Line { get; set; }
    public string Shift { get; set; }
}
public class RootObject
{
	public string ID { get; set; }
    public string ncptype { get; set; }
    public string ncpid { get; set; }
    public string Problem { get; set; }
    public string FirstDecision { get; set; }
    public string Decision { get; set; }
    public string KeyDate { get; set; }
    public string Location { get; set; }
    public string Plant { get; set; }
    public string BatchCode { get; set; }
    public string Product { get; set; }
    public string Batchsap { get; set; }
    public string Active { get; set; }
    public string Material { get; set; }
	public string MaterialType {get; set;}
    public string ProductionDate { get; set; }
    public string Quantity { get; set; }
    public string Shift { get; set; }
    public string HoldQuantity { get; set; }
    public string ProblemQty { get; set; }
    public string Action { get; set; }
    public string Remark { get; set; }
    public string Approve { get; set; }
    public string Approvefinal { get; set; }
	public string user { get; set; }
    public string LinesNo { get; set; }
	public string ShiftOption { get; set; }
    public string Times { get; set; }
    public string ResultDecision { get; set; }

    public string Supplier { get; set; }
    public string Packaging { get; set; }
    public string Batch_Packaging { get; set; }
    public string Recorder { get; set; }       
}