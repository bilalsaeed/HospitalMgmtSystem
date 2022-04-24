using HospitalMgmtSystem.DBContext;
using HospitalMgmtSystem.Enums;
using HospitalMgmtSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace HospitalMgmtSystem.ViewModels
{
    public class UserRegistrationViewModel : BaseViewModel
    {
        private readonly NavigationStore navigationStore;
        private User _user = new User();
        private UserTypeEnum _userTypeEnum;
        public ICommand RegisterButton { get; set; }
        public ICommand LoginButton { get; set; }
        private string _topMessage;
        public string TopMessage
        {
            get => _topMessage;
            set
            {
                _topMessage = value;
                OnPropertyChanged(nameof(TopMessage));
            }
        }
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;

            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        private readonly HospitalDbContext db = new HospitalDbContext();
        public UserRegistrationViewModel(NavigationStore navigationStore, UserTypeEnum userType)
        {
            _userTypeEnum = userType;
            switch (userType)
            {
                case UserTypeEnum.Admin:
                    TopMessage = "Admin Registration";
                    break;
                case UserTypeEnum.Doctor:
                    TopMessage = "Doctor Registration";
                    break;
                case UserTypeEnum.Patient:
                    TopMessage = "Patient Registration";
                    break;

                default:
                    TopMessage = "Admin Registration";
                    break;
            }
            this.navigationStore = navigationStore;
            RegisterButton = new RelayCommand(obj =>
            {
                DoRegistration(obj);
            });

            LoginButton = new RelayCommand(obj =>
            {
                GoToLoginPage();
            });
        }

        private void GoToLoginPage()
        {
            this.navigationStore.CurrentViewModel = new AdminLoginViewModel(this.navigationStore, _userTypeEnum);
        }

        private void DoRegistration(object obj)
        {
            var passwordBox = obj as PasswordBox;
            var password = passwordBox.Password;

            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(User.LastName)
                || string.IsNullOrEmpty(User.FirstName)
                || string.IsNullOrEmpty(User.UserName))
            {
                ErrorMessage = "All Fields are mandatory";
                return;
            }

            try
            {
                var alreadyExists = db.User.Where(a => a.UserName == User.UserName).FirstOrDefault();

                if (alreadyExists != null)
                    throw new Exception("User already exists!");

                User.Password = password;
                User.UserType = _userTypeEnum;
                db.User.Add(User);
                db.SaveChanges();

                Task.Run(async () =>
                {
                    int i = 1, counter = 0;
                    while (counter < 4)
                    {
                        ErrorMessage = "Creating User" + new String('.', i) + new string(' ', 3 - i);
                        i = (i % 3) + 1;
                        counter++;
                        await Task.Delay(1000);
                    }
                    this.navigationStore.CurrentViewModel = new AdminLoginViewModel(this.navigationStore, _userTypeEnum);
                });

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

        }
    }
}
