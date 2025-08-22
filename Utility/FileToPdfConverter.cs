using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SkiaSharp;
using Xceed.Words.NET;

namespace Certitrack.Utility
{
    public static class FileToPdfConverter
    {
        public static byte[] ConvertImageToPdf(byte[] imageBytes)
        {
            using var imageStream = new MemoryStream(imageBytes);
            using var bitmap = SKBitmap.Decode(imageStream);

            using var outputStream = new MemoryStream();
            var document = new PdfDocument();
            var page = document.AddPage();
            page.Width = bitmap.Width;
            page.Height = bitmap.Height;

            var gfx = XGraphics.FromPdfPage(page);
            using var img = XImage.FromStream(() => new MemoryStream(imageBytes));
            gfx.DrawImage(img, 0, 0, page.Width, page.Height);

            document.Save(outputStream);
            return outputStream.ToArray();
        }

        public static byte[] ConvertDocxToPdf(byte[] docxBytes)
        {
            // Load the DOCX
            using var docxStream = new MemoryStream(docxBytes);
            using var document = DocX.Load(docxStream);

            using var outputStream = new MemoryStream();
            var pdf = new PdfDocument();

            // Create a single PDF page
            var page = pdf.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Simple text-only rendering (no formatting)
            var yPos = 20;
            foreach (var paragraph in document.Paragraphs)
            {
                gfx.DrawString(paragraph.Text, new XFont("Arial", 12), XBrushes.Black, new XRect(20, yPos, page.Width - 40, page.Height - 40), XStringFormats.TopLeft);
                yPos += 20;
            }

            pdf.Save(outputStream);
            return outputStream.ToArray();
        }
    }
}
