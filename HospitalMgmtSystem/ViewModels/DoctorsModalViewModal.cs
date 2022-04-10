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
    public class AddEditDoctorsViewModel : BaseViewModel
    {
        public ICommand CloseModal { get; set; }
        public ICommand SaveButton { get; set; }
        private readonly HospitalDbContext db = new HospitalDbContext();
        private User _doctor = new User();
        public User Doctor { get => _doctor; set { _doctor = value; OnPropertyChanged(nameof(Doctor)); } }
        private bool _isReadOnly = false;
        public bool IsReadOnly { get => _isReadOnly; set { _isReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); } }
        private string _message;
        public string Message { get => _message; set { _message = value; OnPropertyChanged(nameof(Message)); } }
        public AddEditDoctorsViewModel(User user)
        {
            if (user != null)
            {
                IsReadOnly = true;
                Message = "Edit Doctor Details!";
                Doctor = db.User.Find(user.Id);
            }
            else
            {
                Message = "Enter Doctor Details!";
                Doctor = new User();
            }
            CloseModal = new RelayCommand(obj =>
            {
                CloseRootDialog();
            });

            SaveButton = new RelayCommand(obj =>
            {
                Save();
            });
        }
        private void Save()
        {
            try
            {
                this.ErrorMessage = string.Empty;
                this.Doctor.UserType = UserTypeEnum.Doctor;
                if (IsReadOnly)
                    db.Entry(this.Doctor).State = System.Data.Entity.EntityState.Modified;
                else
                    db.User.Add(this.Doctor);
                db.SaveChanges();
                CloseRootDialog();
                NavigationStore.Instance.CurrentViewModel = new DoctorsViewModel();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message + ex.InnerException?.Message;
            }
        }
    }
}
