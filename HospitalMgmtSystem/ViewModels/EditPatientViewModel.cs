using HospitalMgmtSystem.DBContext;
using HospitalMgmtSystem.Enums;
using HospitalMgmtSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalMgmtSystem.ViewModels
{
    public class EditPatientViewModel: BaseViewModel
    {
        public ICommand CloseEditPatientModal { get; set; }
        public ICommand SaveEidtPatientsButton { get; set; }
        private readonly HospitalDbContext db = new HospitalDbContext();
        private User _patient = new User();
        public User Patient { get => _patient; set { _patient = value; OnPropertyChanged(nameof(Patient)); } }
        public EditPatientViewModel(User user)
        {
            if(user != null)
            {                
                Patient = db.User.Find(user.Id);
            }
            else
            {
                ErrorMessage = "Something went wrong!";
            }
            CloseEditPatientModal = new RelayCommand(obj => {
                CloseRootDialog();
            });

            SaveEidtPatientsButton = new RelayCommand(obj => {
                SaveNewPatient();
            });
        }
        private void SaveNewPatient()
        {
            try
            {
                this.ErrorMessage = string.Empty;
                this.Patient.UserType = UserTypeEnum.Patient;
                db.Entry(this.Patient).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                CloseRootDialog();
                NavigationStore.Instance.CurrentViewModel = new PatientsViewModel(NavigationStore.Instance);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message + ex.InnerException?.Message;
            }
        }
    }
}
