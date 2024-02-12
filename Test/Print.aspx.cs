using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using iTextSharp.text;
using iTextSharp.text.pdf;
//using Spire.Pdf;
//using Spire.Pdf.Graphics;

public partial class _Default : System.Web.UI.Page {
    QrCreator myQr = new QrCreator();
    MyDataModule cs = new MyDataModule();
    ServiceCS myclass = new ServiceCS();
    protected void Page_Load(object sender, EventArgs e)
    {
        int id;
        if (!int.TryParse(Request.QueryString["Id"], out id))
            return;
        var result = cs.ReadItems(string.Format(@"select SampleID from transSample where Id={0}", id));
        Button1_Click(new object(), new EventArgs());
        //pdfViewer.Text = CreatePdfObjectTag("LabelGen.ashx?dpi=" + DropDownList1.SelectedItem.ToString() + "&prodId=" + TextBox2.Text + "&prodName=" + TextBox1.Text + "&out=PDF");
        //string pdfpath= Server.MapPath("~/ExcelFiles/");
        //ImagesToPdf(myList ,pdfpath);
        //Main(myList);
    }
    //void Main(string[] args)
    //{
    //    string pdfpath = Server.MapPath("~/ExcelFiles/");
    //    PdfDocument doc = new PdfDocument();
    //    PdfSection section = doc.Sections.Add();
    //    PdfPageBase page = doc.Pages.Add();
    //    PdfImage image = PdfImage.FromFile(Server.MapPath("~/ExcelFiles/AAAAAA21C04.jpg"));

    //    float widthFitRate = image.PhysicalDimension.Width / page.Canvas.ClientSize.Width;
    //    float heightFitRate = image.PhysicalDimension.Height / page.Canvas.ClientSize.Height;
    //    float fitRate = Math.Max(widthFitRate, heightFitRate);
    //    float fitWidth = image.PhysicalDimension.Width / fitRate;
    //    float fitHeight = image.PhysicalDimension.Height / fitRate;

    //    page.Canvas.DrawImage(image, 0, 0, fitWidth, fitHeight);
    //    doc.SaveToFile(pdfpath + "test.pdf");
    //    doc.Close();
    //}
    //public void ImagesToPdf(string[] imagepaths, string pdfpath)
    //{
    //    iTextSharp.text.Rectangle pageSize = null;

    //    using (var srcImage = new Bitmap(imagepaths[0].ToString()))
    //    {
    //        pageSize = new iTextSharp.text.Rectangle(0, 0, srcImage.Width, srcImage.Height);
    //    }

    //    using (var ms = new MemoryStream())
    //    {
    //        var document = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
    //        iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
    //        document.Open();
    //        var image = iTextSharp.text.Image.GetInstance(imagepaths[0].ToString());
    //        document.Add(image);
    //        document.Close();

    //        File.WriteAllBytes(pdfpath + "cheque.pdf", ms.ToArray());
    //    }
    //}
    private string CreatePdfObjectTag(string pdfUrl)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<object type=\"application/pdf\" ");
        sb.Append("data=\"");
        sb.Append(pdfUrl);
        sb.Append("#toolbar=1&navpanes=0&scrollbar=1&page=1&zoom=100\" width=\"600px\" height=\"900px\" VIEWASTEXT><p>It appears you don't have a PDF plugin for your browser. <a target=\"_blank\" href=\"");
        sb.Append(pdfUrl);
        sb.Append("\">Click here to download the PDF file.</a></p></object>");
        return sb.ToString();
    }
    private string View(string pdfUrl)
    {
        string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"500px\" height=\"800px\">";
        embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
        embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
        embed += "</object>";
        return  string.Format(embed, ResolveUrl(pdfUrl));
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(TextBox2.Text))
        {
            //tst
            int id;
            if (!int.TryParse(Request.QueryString["Id"], out id))
                return;
            SqlParameter[] param = { new SqlParameter("@Id", id.ToString()),
                             new SqlParameter("@user", string.Format("{0}", cs.user_name)) };
            DataTable dt = cs.GetRelatedResources("spGetSample", param);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string strtext = TextBox2.Text;
                myQr.GetQrcode(strtext);
                string path = Server.MapPath("~/ExcelFiles/");
                PdfCreator Pdf = new PdfCreator();
                string[] myString = { "red", "blue", "green" };
                string input = path + "input.pdf";
                Document document = new Document();
                //string d = row["ReceivingDate"].ToString();
                //DateTime date = DateTime.ParseExact(d, "dd/MM/yyyy", null);
                PdfWriter.GetInstance(document, new FileStream(input, FileMode.Create));
                document.Open();
                document.Add(new Paragraph("\n\n\n"));
                document.Add(new Phrase("SUPPLIER : " + (char)3 + row["SupplierName"].ToString()));
                document.Add(new Paragraph("\n"));
                document.Add(new Phrase("CONTAINER No./TRUCK No. : " + (char)3 + row["ContainerNo"].ToString().Trim()));
                document.Add(new Paragraph("\n"));
                document.Add(new Phrase("MAT CODE : " + row["Material"].ToString().Trim()));
                document.Add(new Paragraph("\n"));
                document.Add(new Phrase("SAMPLEID : " + strtext.Trim()));
                document.Add(new Paragraph("\n"));
                document.Add(new Phrase("SIZE : " + (char)13));
                document.Add(new Paragraph("\n"));
                document.Add(new Phrase("RECEIVING DATE : " + (char)3 + row["date"].ToString()));
                document.Add(new Paragraph("\n"));
                document.Close();

                string image = path + strtext + ".png";
                string result = "result" + strtext + ".pdf";
                Pdf.Main(myString, input, image, path + result);

                //myQr.ConvertImageToPdf(strtext + ".png", strtext+".pdf");
                //string[] myList = { "~/ExcelFiles/"+ strtext  + ".png" };
                pdfViewer.Text = View("~/ExcelFiles/" + result);
            }
        }
    }
}