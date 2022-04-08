using HospitalMgmtSystem.Enums;
using HospitalMgmtSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalMgmtSystem.ViewModels
{
    public class PatientsViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private ObservableCollection<User> _patients = new ObservableCollection<User>();
        private User _patient = new User();
        public User Patient
        {
            get => _patient;
            set
            {
                _patient = value;
                OnPropertyChanged(nameof(Patient));
            }
        }
        private string _textValidation;
        public string textValidation { get => _textValidation; set { _textValidation = value; OnPropertyChanged(nameof(textValidation)); } }
        public ICommand SavePatientsButton { get; set; }
        public ICommand SearchAction { get; set; }
        public ICommand CloseAddPatientModal { get; set; }
        public string SearchQuery { get; set; }
        public ObservableCollection<User> Patients
        {
            get => _patients;
            set
            {
                _patients = value;
                OnPropertyChanged(nameof(Patients));
            }
        }
        private readonly DBContext.HospitalDbContext db = new DBContext.HospitalDbContext();
        public PatientsViewModel(NavigationStore navigationStore)
        {
            this._navigationStore = navigationStore;
            SavePatientsButton = new RelayCommand(obj =>
            {
                saveNewPatient();
            });

            SearchAction = new RelayCommand(obj =>
            {
                this.LoadPatients(this.SearchQuery);
            });

            CloseAddPatientModal = new RelayCommand(obj => {
                this.CloseRootDialog();
                this.Patient = new User();
            });

            LoadPatients(string.Empty);
        }

        private void LoadPatients(string query)
        {
            List<User> patientsList = db.User
                .Where(a => a.UserType == Enums.UserTypeEnum.Patient)
                .ToList()
                .Where(a => ((query != string.Empty && $"{a.FirstName + a.LastName}".ToLower().IndexOf(query) > -1) || (query == string.Empty)))
                .ToList();
            this.Patients = new ObservableCollection<User>(patientsList);
        }

        private void saveNewPatient()
        {
            this.Patient.UserType = UserTypeEnum.Patient;
        }
    }
}
