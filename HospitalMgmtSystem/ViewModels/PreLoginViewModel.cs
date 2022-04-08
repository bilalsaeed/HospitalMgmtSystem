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
    public class PreLoginViewModel: BaseViewModel
    {
        public ICommand AdminLoginCommand { get; set; }
        public ICommand PatientLogin { get; set; }
        public ICommand DoctorLogin { get; set; }
        private readonly NavigationStore navigationStore;
        public PreLoginViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            AdminLoginCommand = new RelayCommand(obj => {
                GoToAdminLoginPage(obj);
            });
        }

        public void GoToAdminLoginPage(object obj)
        {
            UserTypeEnum userType = UserTypeEnum.Admin;
            if (obj != null)
            {
                switch (obj.ToString())
                {
                    case "Admin":
                        userType = UserTypeEnum.Admin;
                        break;
                    case "Doctor":
                        userType = UserTypeEnum.Doctor;
                        break;
                    case "Patient":
                        userType = UserTypeEnum.Patient;
                        break;

                    default:
                        userType = UserTypeEnum.Admin;
                        break;
                }
            }
            this.navigationStore.CurrentViewModel = new AdminLoginViewModel(this.navigationStore, userType);
        }

    }
}
