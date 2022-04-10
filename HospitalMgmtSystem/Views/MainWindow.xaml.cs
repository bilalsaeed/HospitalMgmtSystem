using HospitalMgmtSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HospitalMgmtSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private NavigationStore _navigationStore = NavigationStore.Instance;

        private void goToPatientsView(object sender, MouseButtonEventArgs e)
        {
            _navigationStore.CurrentViewModel = new PatientsViewModel(this._navigationStore);
        }

        private void goToAdminsView(object sender, MouseButtonEventArgs e)//goToDoctorsView
        {
            _navigationStore.CurrentViewModel = new AdminsViewModel();
        }

        private void goToDoctorsView(object sender, MouseButtonEventArgs e)
        {
            _navigationStore.CurrentViewModel = new DoctorsViewModel();
        }
    }
}
