using HospitalMgmtSystem.DBContext;
using HospitalMgmtSystem.Enums;
using HospitalMgmtSystem.Models;
using HospitalMgmtSystem.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HospitalMgmtSystem.ViewModels
{
    public class AdminLoginViewModel : BaseViewModel
    {
        private readonly NavigationStore navigationStore;

        public ICommand CloseApp { get; set; }
        public ICommand RegistrationPage { get; set; }
        public ICommand LoginButton { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }

        private string _userType;
        private string _errorMessage;
        private UserTypeEnum _userTypeEnum;
        private User _user = new User();
        public string UserType
        {
            get => _userType;
            set
            {
                _userType = value;
                OnPropertyChanged(nameof(UserType));
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
        private readonly HospitalDbContext db = new HospitalDbContext();
        public AdminLoginViewModel(NavigationStore navigationStore, UserTypeEnum userType)
        {
            _userTypeEnum = userType;
            switch (userType)
            {
                case UserTypeEnum.Admin:
                    UserType = "Welcome Back Admin";
                    break;
                case UserTypeEnum.Doctor:
                    UserType = "Welcome Back Doctor";
                    break;
                case UserTypeEnum.Patient:
                    UserType = "Welcome Back Patient";
                    break;

                default:
                    UserType = "Welcome Back Admin";
                    break;
            }
            this.navigationStore = navigationStore;
            CloseApp = new RelayCommand(obj =>
            {
                GoBackToPreLogin(obj);
            });

            RegistrationPage = new RelayCommand(obj =>
            {
                GoToRegistrationPage();
            });

            LoginButton = new RelayCommand(obj =>
            {
                LoginUser(obj);
            });

            ForgotPasswordCommand = new RelayCommand(obj => { this.navigationStore.CurrentViewModel = new ForgotPasswordViewModel(userType); });
        }

        private void GoToRegistrationPage()
        {
            this.navigationStore.CurrentViewModel = new UserRegistrationViewModel(this.navigationStore, _userTypeEnum);
        }

        private void GoBackToPreLogin(object obj)
        {
            this.navigationStore.CurrentViewModel = new PreLoginViewModel(this.navigationStore);
        }

        private void LoginUser(object obj)
        {
            var passwordBox = obj as PasswordBox;
            var password = passwordBox.Password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(User.UserName))
            {
                ErrorMessage = "You must enter username or password!";
            }
            else
            {
                var user = db.User.Where(a => a.UserName == User.UserName && a.Password == password && a.UserType == _userTypeEnum).FirstOrDefault();

                if (user == null)
                {
                    ErrorMessage = "User does not exists!";
                }
                else
                {
                    Task.Run(async () =>
                    {
                        int i = 1, counter = 0;
                        while (counter < 4)
                        {
                            ErrorMessage = "Loggin in" + new String('.', i) + new string(' ', 3 - i);
                            i = (i % 3) + 1;
                            counter++;
                            await Task.Delay(1000);
                        }
                        UserStore.User = user;
                        this.navigationStore.CurrentViewModel = new DashboardViewModel(this.navigationStore);
                    });
                }
            }
        }
    }
}
