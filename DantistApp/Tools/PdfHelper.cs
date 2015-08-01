using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DantistApp.Tools
{
    public static class PdfHelper
    {
        public static String SavePdfToFile(PdfSharp.Pdf.PdfDocument pdfDoc)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.FileName = "Отчет"; // Default file name
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "PDF files (.pdf)|*.pdf"; // Filter files by extension

            string filePath = null;

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                filePath = dlg.FileName;

                pdfDoc.Save(filePath);
            }

            return filePath;
        }
    }
}
