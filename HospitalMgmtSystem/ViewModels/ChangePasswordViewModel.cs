using HospitalMgmtSystem.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalMgmtSystem.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        public ICommand SavePasswordCommand { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        public readonly DBContext.HospitalDbContext db = new DBContext.HospitalDbContext();
        public ChangePasswordViewModel()
        {
            SavePasswordCommand = new RelayCommand(obj =>
            {
                ErrorMessage = string.Empty;
                if (string.IsNullOrEmpty(CurrentPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmNewPassword))
                {
                    ErrorMessage = "All fields are mandatory!";
                }
                else
                {
                    var user = db.User.Find(UserStore.User.Id);
                    if (!user.Password.Equals(CurrentPassword))
                        ErrorMessage = "Current password is incorrect!";
                    else if (!NewPassword.Equals(ConfirmNewPassword))
                        ErrorMessage = "New Password and Confirm new passwords do not match!";
                    else
                    {
                        user.Password = NewPassword;
                        db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        ErrorMessage = "Password changed successfully!";
                    }
                }

            });
        }
    }
}
