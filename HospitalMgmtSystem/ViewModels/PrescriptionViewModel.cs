using HospitalMgmtSystem.DBContext;
using HospitalMgmtSystem.DTOs;
using HospitalMgmtSystem.Models;
using HospitalMgmtSystem.Store;
using HospitalMgmtSystem.Views.Components;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        private Appointments appointment;
        private readonly HospitalDbContext db = new HospitalDbContext();

        public string _patientDiagnosisTextbox;
        public string PatientDiagnosisTextbox { get => _patientDiagnosisTextbox; set { _patientDiagnosisTextbox = value; OnPropertyChanged("PatientDiagnosisTextbox"); } }

        private readonly User Patient;
        private readonly User Doctor;

        #region Observeables
        private ObservableCollection<PatientDiagnosesDTO> _patientDiagnoses = new ObservableCollection<PatientDiagnosesDTO>();
        public ObservableCollection<PatientDiagnosesDTO> PatientDiagnoses { get => _patientDiagnoses; set { _patientDiagnoses = value; OnPropertyChanged(nameof(PatientDiagnoses)); } }

        private ObservableCollection<MedicalRecordsDTO> _medicalRecords = new ObservableCollection<MedicalRecordsDTO>();
        public ObservableCollection<MedicalRecordsDTO> MedicalRecords { get => _medicalRecords; set { _medicalRecords = value; OnPropertyChanged(nameof(MedicalRecords)); } }

        public Visibility IsAdminOrDoctor { get; set; }
        #endregion
        public PrescriptionViewModel(AppointmentsDto appointmentDto)
        {
            appointment = db.Appointments.Find(appointmentDto.Id);
            Patient = db.User.Find(appointment.PatientId);
            Doctor = db.User.Find(appointment.DoctorId);
            SaveDiagnosisCommand = new RelayCommand(obj =>
            {
                var patientDiagnosis = new PatientDiagnosis()
                {
                    Diagnosis = this.PatientDiagnosisTextbox,
                    DoctorId = Doctor.Id,
                    PatientId = Patient.Id
                };
                db.PatientDiagnosis.Add(patientDiagnosis);
                db.SaveChanges();
                CloseRootDialog();
                PatientDiagnosisTextbox = string.Empty;
                LoadPatientDiagnoses();
            });

            OpenFileSelector = new RelayCommand(obj =>
            {
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
                            DoctorId = Doctor.Id,
                            PatientId = Patient.Id,
                            LastAccessedById = UserStore.User.Id,
                            FileName = file,
                            Id = newFileId,
                            FileExtension = f1.Extension,
                            CreatedById = UserStore.User.Id
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

            LoadPatientDiagnoses();

            if (UserStore.User.UserType == Enums.UserTypeEnum.Admin)
                IsAdminOrDoctor = Visibility.Visible;
            else
                IsAdminOrDoctor = Visibility.Collapsed;
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

        void LoadMedicalRecords()
        {
            var records = (from k in db.PatientMedicalRecords
                           join u1 in db.User
                           on k.CreatedById equals u1.Id
                           join u2 in db.User
                           on k.LastAccessedById equals u2.Id
                           join u3 in db.User
                           on k.PatientId equals u3.Id
                           join u4 in db.User
                           on k.DoctorId equals u4.Id
                           where k.PatientId == Patient.Id


                           select new MedicalRecordsDTO()
                           {
                               Id = k.Id,
                               FileName = k.FileName,
                               CreatedBy = u1.FirstName + " " + u1.LastName,
                               LastAccessedBy = u2.FirstName + " " + u2.LastName,
                               PatientName = u3.FirstName + " " + u3.LastName,
                               DoctorName = u4.FirstName + " " + u4.LastName,
                               CreatedDate = k.CreatedDate,
                               LastAccessedAt = k.LastAccessedAt,
                               FileExtension = k.FileExtension
                           }).ToList() ;

            MedicalRecords = new ObservableCollection<MedicalRecordsDTO>(records);
        }
    }
}
