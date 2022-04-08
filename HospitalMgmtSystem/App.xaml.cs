using HospitalMgmtSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HospitalMgmtSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var navigationStore = NavigationStore.Instance;
            navigationStore.CurrentViewModel = new PreLoginViewModel(navigationStore);
            navigationStore.AuthenticatedViews = Visibility.Collapsed;
            navigationStore.NonAuthenticatedViews = Visibility.Visible;
            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
