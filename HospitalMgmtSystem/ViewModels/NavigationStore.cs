using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HospitalMgmtSystem.ViewModels
{
    public sealed class NavigationStore
    {
        private NavigationStore() { }
        private static NavigationStore instance = null;
        public static NavigationStore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NavigationStore();
                }
                return instance;
            }
        }
        private BaseViewModel _currentViewModel;

        public event Action CurrentViewModelChanged;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();

            }
        }
        private Visibility _authenticatedViews;
        private Visibility _nonAuthenticatedViews;
        public Visibility NonAuthenticatedViews
        {
            get => _nonAuthenticatedViews;
            set
            {
                _nonAuthenticatedViews = value;
                OnCurrentViewModelChanged();
            }
        }
        public Visibility AuthenticatedViews
        {
            get => _authenticatedViews;
            set
            {
                _authenticatedViews = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
