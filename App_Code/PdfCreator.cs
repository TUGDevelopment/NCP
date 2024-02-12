using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

public class PdfCreator 
{
    public PdfCreator()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public void Main(string[] args,
        string input,
        string some_image,
        string result)
    {
        using (Stream inputPdfStream = new FileStream(input, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (Stream inputImageStream = new FileStream(some_image, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (Stream outputPdfStream = new FileStream(result, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            var reader = new PdfReader(inputPdfStream);
            var stamper = new PdfStamper(reader, outputPdfStream);
            var pdfContentByte = stamper.GetOverContent(1);

            Image im = Image.GetInstance(inputImageStream);
            im.SetAbsolutePosition(100, 200);
            pdfContentByte.AddImage(im);
            stamper.Close();
        }
    }
}