using HospitalMgmtSystem.DTOs;
using HospitalMgmtSystem.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalMgmtSystem.ViewModels
{
    public class AccessCodeViewModel:BaseViewModel
    {
        public string TextBox1 { get; set; } = string.Empty;
        public string TextBox2 { get; set; } = string.Empty;
        public string TextBox3 { get; set; } = string.Empty;
        public string TextBox4 { get; set; } = string.Empty;
        public string TextBox5 { get; set; } = string.Empty;
        public string TextBox6 { get; set; } = string.Empty;
        public ICommand FileAccessCommand { get; set; }
        public readonly DBContext.HospitalDbContext db = new DBContext.HospitalDbContext();
        public AccessCodeViewModel(MedicalRecordsDTO dto)
        {
            FileAccessCommand = new RelayCommand(obj =>
            {
                OpenFile(dto);
            });
        }
        void OpenFile(MedicalRecordsDTO dto)
        {
            ErrorMessage = string.Empty;
            if(TextBox1.Equals("1") && TextBox2.Equals("2") && TextBox3.Equals("3") && TextBox4.Equals("4") && TextBox5.Equals("5") && TextBox6.Equals("6"))
            {
                string currentPath = Directory.GetCurrentDirectory();
                var recordsPath = Path.Combine(currentPath, "MedicalRecords");
                var file = $"{recordsPath}/{dto.Id}{dto.FileExtension}";
                FileInfo f1 = new FileInfo(file);
                if (f1.Exists)
                {
                    var pM = db.PatientMedicalRecords.Find(dto.Id);
                    pM.LastAccessedAt = DateTime.UtcNow;
                    pM.LastAccessedById = UserStore.User.Id;

                    db.Entry(pM).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    System.Diagnostics.Process.Start(file);
                    CloseRootDialog();
                    
                }
                else
                    ErrorMessage = "Invalid file!";
            }
            else
            {
                ErrorMessage = "Invalid access code!";
            }
        }
    }
}
