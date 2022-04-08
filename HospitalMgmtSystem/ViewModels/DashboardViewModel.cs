using HospitalMgmtSystem.Enums;
using HospitalMgmtSystem.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HospitalMgmtSystem.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly NavigationStore navigationStore;
        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged(nameof(WelcomeMessage));
            }
        }
        public DashboardViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this.navigationStore.NonAuthenticatedViews = Visibility.Collapsed;
            this.navigationStore.AuthenticatedViews = Visibility.Visible;
            setWelcomeText();
        }

        private void setWelcomeText()
        {
            var user = UserStore.User;
            if (user != null)
            {
                WelcomeMessage = $"Welcome {user.FirstName}!";
            }
        }
    }
}
