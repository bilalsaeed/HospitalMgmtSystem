using HospitalMgmtSystem.Attributes;
using HospitalMgmtSystem.DBContext;
using HospitalMgmtSystem.DTOs;
using HospitalMgmtSystem.Enums;
using HospitalMgmtSystem.Models;
using HospitalMgmtSystem.Store;
using HospitalMgmtSystem.Views.Components;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace HospitalMgmtSystem.ViewModels
{
    public class PrescriptionViewModel : BaseViewModel
    {

        public ICommand SaveDiagnosisCommand { get; set; }
        public ICommand OpenFileSelector { get; set; }
        public ICommand RowActionCommand { get; set; }
        public ICommand PrintSingleMedicalNote { get; set; }
        public ICommand PrintAllMN { get; set; }
        public ICommand PrintPatient { get; set; }
        public ICommand SavePrescription { get; set; }

        public ICommand PrintSinglePrescription { get; set; }

        public ICommand PrintAllPrescriptions { get; set; }

        private Appointments appointment;
        private readonly HospitalDbContext db = new HospitalDbContext();

        public string _patientDiagnosisTextbox;
        public string PatientDiagnosisTextbox { get => _patientDiagnosisTextbox; set { _patientDiagnosisTextbox = value; OnPropertyChanged("PatientDiagnosisTextbox"); } }

        public User Patient { get; set; }

        private Prescriptions _prescription = new Prescriptions();

        public Prescriptions Prescription { get { return _prescription; } set { _prescription = value; OnPropertyChanged(nameof(Prescription)); } }

        #region Observeables
        private ObservableCollection<PatientDiagnosesDTO> _patientDiagnoses = new ObservableCollection<PatientDiagnosesDTO>();
        public ObservableCollection<PatientDiagnosesDTO> PatientDiagnoses { get => _patientDiagnoses; set { _patientDiagnoses = value; OnPropertyChanged(nameof(PatientDiagnoses)); } }

        private ObservableCollection<MedicalRecordsDTO> _medicalRecords = new ObservableCollection<MedicalRecordsDTO>();
        public ObservableCollection<MedicalRecordsDTO> MedicalRecords { get => _medicalRecords; set { _medicalRecords = value; OnPropertyChanged(nameof(MedicalRecords)); } }

        private ObservableCollection<MedicalRecordsDTO> _scanResults = new ObservableCollection<MedicalRecordsDTO>();
        public ObservableCollection<MedicalRecordsDTO> ScanResults { get => _scanResults; set { _scanResults = value; OnPropertyChanged(nameof(ScanResults)); } }

        private ObservableCollection<MedicalRecordsDTO> _labResults = new ObservableCollection<MedicalRecordsDTO>();
        public ObservableCollection<MedicalRecordsDTO> LabResults { get => _labResults; set { _labResults = value; OnPropertyChanged(nameof(LabResults)); } }

        private ObservableCollection<PrescriptionsDTO> _prescriptions = new ObservableCollection<PrescriptionsDTO>();
        public ObservableCollection<PrescriptionsDTO> PatientPrescriptions { get => _prescriptions; set { _prescriptions = value; OnPropertyChanged(nameof(PatientPrescriptions)); } }

        private Visibility _IsAdminOrDoctor;
        public Visibility IsAdminOrDoctor { get => _IsAdminOrDoctor; set { _IsAdminOrDoctor = value; OnPropertyChanged(nameof(IsAdminOrDoctor)); } }
        #endregion
        public PrescriptionViewModel(int patientId)
        {
            Patient = db.User.Find(patientId);
            SaveDiagnosisCommand = new RelayCommand(obj =>
            {
                var patientDiagnosis = new PatientDiagnosis()
                {
                    Diagnosis = this.PatientDiagnosisTextbox,
                    DoctorId = UserStore.User.Id,
                    PatientId = Patient.Id
                };
                db.PatientDiagnosis.Add(patientDiagnosis);
                db.SaveChanges();
                CloseRootDialog();
                PatientDiagnosisTextbox = string.Empty;
                LoadPatientDiagnoses();
            });

            SavePrescription = new RelayCommand(obj =>
            {
                var prescription = new Prescriptions()
                {
                    Dosage = Prescription.Dosage,
                    Quantity = Prescription.Dosage,
                    Medicine = Prescription.Medicine,
                    AddedById = UserStore.User.Id,
                    PatientId = Patient.Id
                };
                db.Prescriptions.Add(prescription);
                db.SaveChanges();
                CloseRootDialog();
                Prescription = new Prescriptions();
                LoadPatientPrescriptions();
            });

            OpenFileSelector = new RelayCommand(obj =>
            {
                var mR = Convert.ToInt32(obj);
                var recordType = (MedicalRecordType)mR;
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                fileDialog.Filter = "Pdf files (*.pdf)|*.pdf|Office Files|*.doc;*.xls;*.ppt|Txt files (*.txt)|*.txt|" + "All Images Files (*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif)|*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif" +
            "|PNG Portable Network Graphics (*.png)|*.png" +
            "|JPEG File Interchange Format (*.jpg *.jpeg *jfif)|*.jpg;*.jpeg;*.jfif" +
            "|BMP Windows Bitmap (*.bmp)|*.bmp" +
            "|TIF Tagged Imaged File Format (*.tif *.tiff)|*.tif;*.tiff" +
            "|GIF Graphics Interchange Format (*.gif)|*.gif"; ; // Optional file extensions

                DialogResult dr = fileDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    string currentPath = Directory.GetCurrentDirectory();
                    if (!Directory.Exists(Path.Combine(currentPath, "MedicalRecords")))
                        Directory.CreateDirectory(Path.Combine(currentPath, "MedicalRecords"));
                    var recordsPath = Path.Combine(currentPath, "MedicalRecords");
                    string directoryPath = System.IO.Path.GetDirectoryName(fileDialog.FileName);
                    foreach (var file in fileDialog.FileNames)
                    {
                        var newFileId = Guid.NewGuid();
                        FileInfo f1 = new FileInfo(file);
                        var patientMedicalRecords = new PatientMedicalRecords()
                        {
                            PatientId = Patient.Id,
                            LastAccessedById = UserStore.User.Id,
                            FileName = file,
                            Id = newFileId,
                            FileExtension = f1.Extension,
                            CreatedById = UserStore.User.Id,
                            RecordType = recordType
                        };
                        db.PatientMedicalRecords.Add(patientMedicalRecords);
                        File.Copy(file, $"{recordsPath}/{newFileId}{f1.Extension}");
                    }
                    db.SaveChanges();
                    LoadMedicalRecords();
                }
            });

            RowActionCommand = new RelayCommand(obj =>
            {
                AccessFile(obj);
            });

            LoadMedicalRecords();
            LoadPatientPrescriptions();

            LoadPatientDiagnoses();

            if (UserStore.User.UserType == Enums.UserTypeEnum.Admin)
                IsAdminOrDoctor = Visibility.Visible;
            else
                IsAdminOrDoctor = Visibility.Collapsed;

            PrintSingleMedicalNote = new RelayCommand(obj =>
            {
                var diagnosis = obj as PatientDiagnosesDTO;
                PrintOneMedicalNote(diagnosis);
            });

            PrintSinglePrescription = new RelayCommand(obj =>
            {
                var prescription = obj as PrescriptionsDTO;
                PrintOnePrescription(prescription);
            });

            PrintAllPrescriptions = new RelayCommand(obj =>
            {
                PrintPrescriptions();
            });



            PrintAllMN = new RelayCommand(obj =>
            {
                PrintAllMedicalNotes();
            });

            PrintPatient = new RelayCommand(obj =>
            {
                PrintPatientProfile();
            });
        }

        private void PrintPrescriptions()
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

            var table = new PdfPTable(2);
            table.SetWidths(new float[] { 400f, 400f });
            table.WidthPercentage = 80;
            var paragraph = new Paragraph();



            table.AddCell(new PdfPCell(new Phrase($"{Patient.FirstName + " " + Patient.LastName}'s All Prescription", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0))))
            {
                Colspan = 2,
                Border = 0,
                PaddingBottom = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            foreach (var prescription in PatientPrescriptions)
            {

                table.AddCell(new PdfPCell(new Phrase($"Prescription By: {prescription.AddedBy}", new Font(Font.NORMAL, 10, 1, new iTextSharp.text.BaseColor(0,0,0))))
                {
                    Colspan = 2,
                    Border = 0,
                    PaddingBottom = 20,
                    HorizontalAlignment = Element.ALIGN_LEFT
                });
                //Add Content to PDF  
                AddCellToBody(table, "Drug Name");
                AddCellToBody(table, prescription.Medicine);

                AddCellToBody(table, "Dosage");
                AddCellToBody(table, prescription.Dosage);

                AddCellToBody(table, "Quantity");
                AddCellToBody(table, prescription.Quantity);
            }


            doc.Add(table);
            // Closing the document  
            doc.Close();

            PrintDialog printDialog1 = new PrintDialog();
            
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Print Document";
            printDialog1.Document = printDoc;
            printDialog1.AllowSelection = true;
            printDialog1.AllowSomePages = true;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                Process p = new Process();
                p.StartInfo.FileName = filePath;
                p.StartInfo.Verb = "printto";
                p.StartInfo.Arguments = "\"" + printDialog1.PrinterSettings.PrinterName + "\"";
                p.Start();
            }
        }

        private void PrintOneMedicalNote(PatientDiagnosesDTO obj)
        {
            var pD = db.PatientDiagnosis.Find(obj.Id);
            var patient = db.User.Find(pD.PatientId);

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
            doc.Add(Add_Content_To_PDF(tableLayout, patient, pD.Diagnosis));
            // Closing the document  
            doc.Close();

            PrintDialog printDialog1 = new PrintDialog();
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                Process p = new Process();
                p.StartInfo.FileName = filePath;
                p.StartInfo.Verb = "printto";
                p.StartInfo.Arguments = "\"" + printDialog1.PrinterSettings.PrinterName + "\"";
                p.Start();
            }
        }

        private void PrintOnePrescription(PrescriptionsDTO obj)
        {
            var pR = db.Prescriptions.Find(obj.Id);
            var patient = db.User.Find(pR.PatientId);

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

            var table = new PdfPTable(2);
            table.SetWidths(new float[] { 400f, 400f });
            table.WidthPercentage = 80;
            var paragraph = new Paragraph();



            table.AddCell(new PdfPCell(new Phrase($"{Patient.FirstName + " " + Patient.LastName}'s Prescription", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0))))
            {
                Colspan = 2,
                Border = 0,
                PaddingBottom = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            //Add Content to PDF  
            AddCellToBody(table, "Drug Name");
            AddCellToBody(table, pR.Medicine);

            AddCellToBody(table, "Dosage");
            AddCellToBody(table, pR.Dosage);

            AddCellToBody(table, "Quantity");
            AddCellToBody(table, pR.Quantity);

            doc.Add(table);
            // Closing the document  
            doc.Close();

            PrintDialog printDialog1 = new PrintDialog();
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                Process p = new Process();
                p.StartInfo.FileName = filePath;
                p.StartInfo.Verb = "printto";
                p.StartInfo.Arguments = "\"" + printDialog1.PrinterSettings.PrinterName + "\"";
                p.Start();
            }
        }

        private PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, User patient, string message)
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
            tableLayout.AddCell(new PdfPCell(new Phrase($"{patient.FirstName + " " + patient.LastName}'s Medical Notes", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0))))
            {
                Colspan = 4,
                Border = 0,
                PaddingBottom = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            tableLayout.AddCell(new PdfPCell(new Phrase(message, new Font(Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
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
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = iTextSharp.text.BaseColor.WHITE
            });
        }

        private void AccessFile(object obj)
        {
            var dto = obj as MedicalRecordsDTO;
            var view = new AccessCodeModal()
            {
                DataContext = new AccessCodeViewModel(dto)
            };
            //show the dialog
            var result = DialogHost.Show(view, "RootDialog", null, null);

        }

        void LoadPatientDiagnoses()
        {
            var diagnoses = (from diagnosis in db.PatientDiagnosis
                             join doctor in db.User
                             on diagnosis.DoctorId equals doctor.Id
                             where diagnosis.PatientId == Patient.Id

                             select new PatientDiagnosesDTO()
                             {
                                 Id = diagnosis.Id,
                                 AddedAt = diagnosis.AddedAt,
                                 Diagnosis = diagnosis.Diagnosis,
                                 DoctorName = doctor.FirstName + " " + doctor.LastName
                             }).ToList();
            PatientDiagnoses = new ObservableCollection<PatientDiagnosesDTO>(diagnoses);
        }

        void LoadPatientPrescriptions()
        {
            var prescriptions = (from prescription in db.Prescriptions.ToList()
                                 join doctor in db.User.ToList()
                                 on prescription.AddedById equals doctor.Id
                                 where prescription.PatientId == Patient.Id

                                 select new PrescriptionsDTO()
                                 {
                                     Id = prescription.Id,
                                     AddedAt = prescription.AddedAt.ToString("f"),
                                     Medicine = prescription.Medicine,
                                     Dosage = prescription.Dosage,
                                     Quantity = prescription.Quantity,
                                     AddedBy = doctor.FirstName + " " + doctor.LastName
                                 }).ToList();
            PatientPrescriptions = new ObservableCollection<PrescriptionsDTO>(prescriptions);
        }

        void LoadMedicalRecords()
        {
            var medicaRecords = (from k in db.PatientMedicalRecords
                                 join u1 in db.User
                                 on k.CreatedById equals u1.Id
                                 join u2 in db.User
                                 on k.LastAccessedById equals u2.Id
                                 join u3 in db.User
                                 on k.PatientId equals u3.Id
                                 where k.PatientId == Patient.Id && k.RecordType == MedicalRecordType.OtherMedicalRecord


                                 select new MedicalRecordsDTO()
                                 {
                                     Id = k.Id,
                                     FileName = k.FileName,
                                     CreatedBy = u1.FirstName + " " + u1.LastName,
                                     LastAccessedBy = u2.FirstName + " " + u2.LastName,
                                     PatientName = u3.FirstName + " " + u3.LastName,
                                     CreatedDate = k.CreatedDate,
                                     LastAccessedAt = k.LastAccessedAt,
                                     FileExtension = k.FileExtension
                                 }).ToList();

            MedicalRecords = new ObservableCollection<MedicalRecordsDTO>(medicaRecords);
            var scanResults = (from k in db.PatientMedicalRecords
                               join u1 in db.User
                               on k.CreatedById equals u1.Id
                               join u2 in db.User
                               on k.LastAccessedById equals u2.Id
                               join u3 in db.User
                               on k.PatientId equals u3.Id
                               where k.PatientId == Patient.Id && k.RecordType == MedicalRecordType.ScanResult


                               select new MedicalRecordsDTO()
                               {
                                   Id = k.Id,
                                   FileName = k.FileName,
                                   CreatedBy = u1.FirstName + " " + u1.LastName,
                                   LastAccessedBy = u2.FirstName + " " + u2.LastName,
                                   PatientName = u3.FirstName + " " + u3.LastName,
                                   CreatedDate = k.CreatedDate,
                                   LastAccessedAt = k.LastAccessedAt,
                                   FileExtension = k.FileExtension
                               }).ToList();

            ScanResults = new ObservableCollection<MedicalRecordsDTO>(scanResults);
            var labResults = (from k in db.PatientMedicalRecords
                              join u1 in db.User
                              on k.CreatedById equals u1.Id
                              join u2 in db.User
                              on k.LastAccessedById equals u2.Id
                              join u3 in db.User
                              on k.PatientId equals u3.Id
                              where k.PatientId == Patient.Id && k.RecordType == MedicalRecordType.LabResult


                              select new MedicalRecordsDTO()
                              {
                                  Id = k.Id,
                                  FileName = k.FileName,
                                  CreatedBy = u1.FirstName + " " + u1.LastName,
                                  LastAccessedBy = u2.FirstName + " " + u2.LastName,
                                  PatientName = u3.FirstName + " " + u3.LastName,
                                  CreatedDate = k.CreatedDate,
                                  LastAccessedAt = k.LastAccessedAt,
                                  FileExtension = k.FileExtension
                              }).ToList();

            LabResults = new ObservableCollection<MedicalRecordsDTO>(labResults);
        }

        void PrintAllMedicalNotes()
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
            iTextSharp.text.List list = new iTextSharp.text.List(iTextSharp.text.List.ORDERED, 20f);
            list.IndentationLeft = 20f;//indented 20 points from left margin
            foreach (var i in PatientDiagnoses)
            {
                list.Add($"Diagnosis By ({i.DoctorName}): {i.Diagnosis}");
            }
            doc.Add(list);//add list to document
            // Closing the document  
            doc.Close();

            PrintDialog printDialog1 = new PrintDialog();
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                Process p = new Process();
                p.StartInfo.FileName = filePath;
                p.StartInfo.Verb = "printto";
                p.StartInfo.Arguments = "\"" + printDialog1.PrinterSettings.PrinterName + "\"";
                p.Start();
            }
        }

        void PrintPatientProfile()
        {
            //Create document  
            Document doc = new Document();
            string currentPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Path.Combine(currentPath, "Letters")))
                Directory.CreateDirectory(Path.Combine(currentPath, "Letters"));
            var recordsPath = Path.Combine(currentPath, "Letters");
            //Create a PDF file in specific path  

            var filePath = $"{recordsPath}/{Guid.NewGuid()}.pdf";
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            //Open the PDF document  
            doc.Open();

            PdfPTable table;
            PdfPCell cell;
            iTextSharp.text.Paragraph paragraph;

            table = new PdfPTable(2);
            table.SetWidths(new float[] { 400f, 400f });
            table.WidthPercentage = 80;
            paragraph = new Paragraph();

            PropertyInfo[] props = Patient.GetType().GetProperties();

            table.AddCell(new PdfPCell(new Phrase($"{Patient.FirstName + " " + Patient.LastName}'s Medical Profile", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0))))
            {
                Colspan = 2,
                Border = 0,
                PaddingBottom = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    ColumnNameAttribute authAttr = attr as ColumnNameAttribute;
                    if (authAttr != null)
                    {
                        object value = prop.GetValue(Patient, null);
                        if (value != null)
                        {

                            AddCellToBody(table, authAttr.Name);

                            var type = prop.GetType();
                            if (prop.PropertyType.Name == "Boolean")
                            {
                                AddCellToBody(table, Convert.ToBoolean(value) ? "Yes" : "No");
                            }
                            else
                            {
                                AddCellToBody(table, value.ToString());
                            }
                        }
                    }
                }
            }

            table.SetWidthPercentage(new float[2] { 400f, 400f }, PageSize.LETTER);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            doc.Add(table);
            // Closing the document  
            doc.Close();

            PrintDialog printDialog1 = new PrintDialog();
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                Process p = new Process();
                p.StartInfo.FileName = filePath;
                p.StartInfo.Verb = "printto";
                p.StartInfo.Arguments = "\"" + printDialog1.PrinterSettings.PrinterName + "\"";
                p.Start();
            }
        }
    }
}
