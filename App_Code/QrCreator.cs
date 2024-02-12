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
using System.Linq;
using System.Drawing.Imaging;
using BarcodeLib;
using QRCoder;
using System.Web.UI.WebControls;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Net;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class QrCreator : System.Web.Services.WebService
{
    public QrCreator()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public void ConvertImageToPdf(string srcFilename, string dstFilename)
    {
        iTextSharp.text.Rectangle pageSize = null;
        string pdfpath = Server.MapPath("~/ExcelFiles/");
        srcFilename = pdfpath + srcFilename;
        using (var srcImage = new System.Drawing.Bitmap(srcFilename))
        {
            pageSize = new iTextSharp.text.Rectangle(0, 0, srcImage.Width, srcImage.Height);
        }
        using (var ms = new MemoryStream())
        {
            var document = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
            iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
            document.Open();
            var image = iTextSharp.text.Image.GetInstance(srcFilename);
            document.Add(image);
            document.Close();

            File.WriteAllBytes(pdfpath + dstFilename, ms.ToArray());
        }
    }

    [WebMethod]
    public void GetQrcode(string code)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
        Image imgBarCode = new Image();
        imgBarCode.Height = 90;
        imgBarCode.Width = 90;

        using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(10))
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitMap.Save(Server.MapPath("~/ExcelFiles/") + code + ".png", System.Drawing.Imaging.ImageFormat.Png);
                Byte[] byteImage = ms.ToArray();
                imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
            }
        }
    }
    [WebMethod]
    public void GenerateBarcodeImage(string d, string il, int wh)
    {
        string t = "Code 128";
        if (d != null)
        {
            //Read in the parameters
            string strData = d;
            int imageHeight = Convert.ToInt32("40");
            int imageWidth = Convert.ToInt32(wh);
            string Forecolor = "";
            string Backcolor = "";
            bool bIncludeLabel = il == "true";
            //string strImageFormat = "GIF";
            string strAlignment = "c";

            BarcodeLib.TYPE type = BarcodeLib.TYPE.UNSPECIFIED;
            switch (t)
            {
                case "UPC-A": type = BarcodeLib.TYPE.UPCA; break;
                case "UPC-E": type = BarcodeLib.TYPE.UPCE; break;
                case "UPC 2 Digit Ext": type = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_2DIGIT; break;
                case "UPC 5 Digit Ext": type = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_5DIGIT; break;
                case "EAN-13": type = BarcodeLib.TYPE.EAN13; break;
                case "JAN-13": type = BarcodeLib.TYPE.JAN13; break;
                case "EAN-8": type = BarcodeLib.TYPE.EAN8; break;
                case "ITF-14": type = BarcodeLib.TYPE.ITF14; break;
                case "Codabar": type = BarcodeLib.TYPE.Codabar; break;
                case "PostNet": type = BarcodeLib.TYPE.PostNet; break;
                case "Bookland-ISBN": type = BarcodeLib.TYPE.BOOKLAND; break;
                case "Code 11": type = BarcodeLib.TYPE.CODE11; break;
                case "Code 39": type = BarcodeLib.TYPE.CODE39; break;
                case "Code 39 Extended": type = BarcodeLib.TYPE.CODE39Extended; break;
                case "Code 93": type = BarcodeLib.TYPE.CODE93; break;
                case "LOGMARS": type = BarcodeLib.TYPE.LOGMARS; break;
                case "MSI": type = BarcodeLib.TYPE.MSI_Mod10; break;
                case "Interleaved 2 of 5": type = BarcodeLib.TYPE.Interleaved2of5; break;
                case "Standard 2 of 5": type = BarcodeLib.TYPE.Standard2of5; break;
                case "Code 128": type = BarcodeLib.TYPE.CODE128; break;
                case "Code 128-A": type = BarcodeLib.TYPE.CODE128A; break;
                case "Code 128-B": type = BarcodeLib.TYPE.CODE128B; break;
                case "Code 128-C": type = BarcodeLib.TYPE.CODE128C; break;
                case "Telepen": type = BarcodeLib.TYPE.TELEPEN; break;
                case "FIM (Facing Identification Mark)": type = BarcodeLib.TYPE.FIM; break;
                case "Pharmacode": type = BarcodeLib.TYPE.PHARMACODE; break;
                default: break;
            }//switch

            System.Drawing.Image barcodeImage = null;
            try
            {
                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                if (type != BarcodeLib.TYPE.UNSPECIFIED)
                {
                    b.IncludeLabel = bIncludeLabel;

                    //alignment
                    switch (strAlignment.ToLower().Trim())
                    {
                        case "c":
                            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                            break;
                        case "r":
                            b.Alignment = BarcodeLib.AlignmentPositions.RIGHT;
                            break;
                        case "l":
                            b.Alignment = BarcodeLib.AlignmentPositions.LEFT;
                            break;
                        default:
                            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                            break;
                    }//switch

                    if (Forecolor.Trim() == "" && Forecolor.Trim().Length != 6)
                        Forecolor = "000000";
                    if (Backcolor.Trim() == "" && Backcolor.Trim().Length != 6)
                        Backcolor = "FFFFFF";

                    //===== Encoding performed here =====
                    barcodeImage = b.Encode(type, strData.Trim(), System.Drawing.ColorTranslator.FromHtml("#" + Forecolor), System.Drawing.ColorTranslator.FromHtml("#" + Backcolor), imageWidth, imageHeight);
                    //===================================

                    //===== Static Encoding performed here =====
                    //barcodeImage = BarcodeLib.Barcode.DoEncode(type, this.txtData.Text.Trim(), this.chkGenerateLabel.Checked, this.btnForeColor.BackColor, this.btnBackColor.BackColor);
                    //==========================================
                    BarcodeLib.SaveTypes y = SaveTypes.JPG;
                    b.SaveImage(Server.MapPath("~/FileTest/" + strData + ".jpg"), y);
                    //Response.ContentType = "~/FileTest/" + strImageFormat;
                    //System.IO.MemoryStream MemStream = new System.IO.MemoryStream();

                    //switch (strImageFormat)
                    //{
                    //    case "gif": barcodeImage.Save(MemStream, ImageFormat.Gif); break;
                    //    case "jpeg": barcodeImage.Save(MemStream, ImageFormat.Jpeg); break;
                    //    case "png": barcodeImage.Save(MemStream, ImageFormat.Png); break;
                    //    case "bmp": barcodeImage.Save(MemStream, ImageFormat.Bmp); break;
                    //    case "tiff": barcodeImage.Save(MemStream, ImageFormat.Tiff); break;
                    //    default: break;
                    //}//switch
                    //MemStream.WriteTo(Response.OutputStream);
                }//if
            }//try
            catch (Exception ex)
            {
                Context.Response.Write(ex);
                //TODO: find a way to return this to display the encoding error message
            }//catch
            finally
            {
                if (barcodeImage != null)
                {
                    //Clean up / Dispose...
                    barcodeImage.Dispose();
                }
            }//finally
        }//if
    }
}