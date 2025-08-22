using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf.IO;

namespace Certitrack.Utility
{
    public static class PdfWatermarkHelper
    {
        public static byte[] AddWatermark(byte[] pdfBytes, string watermarkText)
        {
            using var inputStream = new MemoryStream(pdfBytes);
            using var outputStream = new MemoryStream();

            var document = PdfReader.Open(inputStream, PdfDocumentOpenMode.Modify);

            foreach (var page in document.Pages)
            {
                var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append);
                var font = new XFont("Arial", 40, XFontStyle.BoldItalic);
                var brush = new XSolidBrush(XColor.FromArgb(128, 255, 0, 0));
                var format = new XStringFormat
                {
                    Alignment = XStringAlignment.Center,
                    LineAlignment = XLineAlignment.Center
                };

                gfx.DrawString(watermarkText, font, brush, new XRect(0, 0, page.Width, page.Height), format);
            }

            document.Save(outputStream);
            return outputStream.ToArray();
        }
    }
}
