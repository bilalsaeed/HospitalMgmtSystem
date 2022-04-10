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
    public class AddEditAdminViewModel : BaseViewModel
    {
        public ICommand CloseAdminModal { get; set; }
        public ICommand SaveAdminButton { get; set; }
        private readonly HospitalDbContext db = new HospitalDbContext();
        private User _admin = new User();
        public User Admin { get => _admin; set { _admin = value; OnPropertyChanged(nameof(Admin)); } }
        private bool _isReadOnly = false;
        public bool IsReadOnly { get => _isReadOnly; set { _isReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); } }
        private string _message;
        public string Message { get => _message; set { _message = value; OnPropertyChanged(nameof(Message)); } }
        public AddEditAdminViewModel(User user)
        {
            if (user != null)
            {
                IsReadOnly = true;
                Message = "Edit Admin Details!";
                Admin = db.User.Find(user.Id);
            }
            else
            {
                Message = "Enter Admin Details!";
                Admin = new User();
            }
            CloseAdminModal = new RelayCommand(obj =>
            {
                CloseRootDialog();
            });

            SaveAdminButton = new RelayCommand(obj =>
            {
                SaveNewAdmin();
            });
        }
        private void SaveNewAdmin()
        {
            try
            {
                this.ErrorMessage = string.Empty;
                this.Admin.UserType = UserTypeEnum.Admin;
                if (IsReadOnly)
                    db.Entry(this.Admin).State = System.Data.Entity.EntityState.Modified;
                else
                    db.User.Add(this.Admin);
                db.SaveChanges();
                CloseRootDialog();
                NavigationStore.Instance.CurrentViewModel = new AdminsViewModel();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message + ex.InnerException?.Message;
            }
        }
    }
}
