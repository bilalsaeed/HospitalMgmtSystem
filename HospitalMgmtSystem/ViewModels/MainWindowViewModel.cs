using HospitalMgmtSystem.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HospitalMgmtSystem.ViewModels
{
    public class MainWindowViewModel:BaseViewModel
    {
        private readonly NavigationStore navigationStore;
        public BaseViewModel CurrentViewModel => navigationStore.CurrentViewModel;
        public Visibility NonAuthenticatedViews => navigationStore.NonAuthenticatedViews;
        public Visibility AuthenticatedViews => navigationStore.AuthenticatedViews;

        public Visibility IsAdmin => navigationStore.IsAdmin;

        public ICommand LogoutCommand { get; set; }

        public MainWindowViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this.navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            this.navigationStore.CurrentViewModelChanged += OnAuthenticatedChanged;
            this.navigationStore.CurrentViewModelChanged += OnNonAuthenticatedChanged;
            this.navigationStore.CurrentViewModelChanged += OnIsAdminChanged;
            LogoutCommand = new RelayCommand(obj => {
                UserStore.User = null;
                this.navigationStore.CurrentViewModel = new PreLoginViewModel(this.navigationStore);
                this.navigationStore.AuthenticatedViews = Visibility.Collapsed;
                this.navigationStore.NonAuthenticatedViews = Visibility.Visible;
            });
        }

        private void OnIsAdminChanged()
        {
            OnPropertyChanged(nameof(this.IsAdmin));
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(this.CurrentViewModel));
        }

        private void OnAuthenticatedChanged()
        {
            OnPropertyChanged(nameof(this.AuthenticatedViews));
        }

        private void OnNonAuthenticatedChanged()
        {
            OnPropertyChanged(nameof(this.NonAuthenticatedViews));
        }
    }
}
