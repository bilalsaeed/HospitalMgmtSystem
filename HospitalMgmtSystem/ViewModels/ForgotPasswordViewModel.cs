using HospitalMgmtSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HospitalMgmtSystem.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        public string SaveButtonText { get => _saveButtonText; set { _saveButtonText = value; OnPropertyChanged(nameof(SaveButtonText)); } }
        private string _saveButtonText = "Verify Access Code!";

        public string TextBox1 { get; set; } = string.Empty;
        public string TextBox2 { get; set; } = string.Empty;
        public string TextBox3 { get; set; } = string.Empty;
        public string TextBox4 { get; set; } = string.Empty;
        public string TextBox5 { get; set; } = string.Empty;
        public string TextBox6 { get; set; } = string.Empty;

        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string UserName { get; set; }


        private Visibility _accessCodeVerified = Visibility.Collapsed;
        public Visibility AccessCodeVerified { get => _accessCodeVerified; set { _accessCodeVerified = value; OnPropertyChanged(nameof(AccessCodeVerified)); } }

        public ICommand SavePasswordCommand { get; set; }
        public ICommand GoToLoginPage { get; set; }
        public readonly DBContext.HospitalDbContext db = new DBContext.HospitalDbContext();
        private bool _DisableAfterAccessCodeVerified = false;
        public bool DisableAfterAccessCodeVerified { get => _DisableAfterAccessCodeVerified; set { _DisableAfterAccessCodeVerified = value; OnPropertyChanged(nameof(DisableAfterAccessCodeVerified)); } }
        public ForgotPasswordViewModel(UserTypeEnum userType)
        {
            SavePasswordCommand = new RelayCommand(obj =>
            {
                var user = db.User.Where(a => a.UserName == UserName).FirstOrDefault();
                ErrorMessage = string.Empty;
                if (user != null)
                {
                    if (AccessCodeVerified == Visibility.Collapsed)
                    {
                        if (TextBox1.Equals("1") && TextBox2.Equals("2") && TextBox3.Equals("3") && TextBox4.Equals("4") && TextBox5.Equals("5") && TextBox6.Equals("6"))
                        {
                            AccessCodeVerified = Visibility.Visible;
                            DisableAfterAccessCodeVerified = true;
                            SaveButtonText = "Update Password";
                        }
                        else
                        {
                            ErrorMessage = "Invalid Access Code";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmNewPassword))
                        {
                            ErrorMessage = "Password fields are mandatory!";
                        }
                        else
                        {
                            if (!NewPassword.Equals(ConfirmNewPassword))
                                ErrorMessage = "New Password and Confirm new passwords do not match!";
                            else
                            {
                                user.Password = NewPassword;
                                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                                ErrorMessage = "Password changed successfully!";
                            }
                        }
                    }
                }
                else
                {
                    ErrorMessage = "User does not exists!";
                }
            });

            GoToLoginPage = new RelayCommand(obj => {
                NavigationStore.Instance.CurrentViewModel = new AdminLoginViewModel(NavigationStore.Instance, userType);
            });
        }
    }
}
