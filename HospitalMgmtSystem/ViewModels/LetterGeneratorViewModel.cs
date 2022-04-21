using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalMgmtSystem.ViewModels
{
    public class LetterGeneratorViewModel : BaseViewModel
    {
        public string LetterText { get; set; }
        public ICommand PrintLetterCommand { get; set; }
        public LetterGeneratorViewModel()
        {
            PrintLetterCommand = new RelayCommand(obj =>
            {
                PrintLetter();
            });
        }

        void PrintLetter()
        {
            //Create document  
            Document doc = new Document();
            //Create PDF Table  
            PdfPTable tableLayout = new PdfPTable(4);
            string currentPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Path.Combine(currentPath, "Letters")))
                Directory.CreateDirectory(Path.Combine(currentPath, "Letters"));
            var recordsPath = Path.Combine(currentPath, "Letters");
            //Create a PDF file in specific path  

            var filePath = $"{recordsPath}/{Guid.NewGuid()}.pdf";
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            //Open the PDF document  
            doc.Open();
            //Add Content to PDF  
            doc.Add(Add_Content_To_PDF(tableLayout));
            // Closing the document  
            doc.Close();

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                Verb = "print",
                FileName = filePath //put the correct path here
            };
            p.Start();
        }

        private PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {
            float[] headers = {
        20,
        20,
        30,
        30
    }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
                                              //Add Title to the PDF file at the top  
            tableLayout.AddCell(new PdfPCell(new Phrase("MedicAll Letter", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0))))
            {
                Colspan = 4,
                Border = 0,
                PaddingBottom = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            tableLayout.AddCell(new PdfPCell(new Phrase(LetterText, new Font(Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 4,
                Border = 0,
                PaddingBottom = 20,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            return tableLayout;
        }
        // Method to add single cell to the header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.NORMAL, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(0, 51, 102)
            });
        }
        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.NORMAL, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = iTextSharp.text.BaseColor.WHITE
            });
        }
    }
}
