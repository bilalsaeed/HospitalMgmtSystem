using HospitalMgmtSystem.Enums;
using HospitalMgmtSystem.Models;
using HospitalMgmtSystem.Views.Components;
using MaterialDesignThemes.Wpf;
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

        private User _editPatient = new User();
        public User EditPatient { get => _editPatient; set { _editPatient = value; OnPropertyChanged(nameof(EditPatient)); } }
        private string _textValidation;
        public string textValidation { get => _textValidation; set { _textValidation = value; OnPropertyChanged(nameof(textValidation)); } }
        public ICommand SavePatientsButton { get; set; }
        public ICommand SearchAction { get; set; }
        public ICommand CloseAddPatientModal { get; set; }
        public ICommand GoToPatientsDetails { get; set; }
        public ICommand DeletePatientCommand { get; set; }
        public ICommand EditPatientCommand { get; set; }

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
                SaveNewPatient();
            });

            SearchAction = new RelayCommand(obj =>
            {
                this.LoadPatients(this.SearchQuery);
            });

            CloseAddPatientModal = new RelayCommand(obj =>
            {
                this.CloseRootDialog();
                this.Patient = new User();
                this.ErrorMessage = string.Empty;
            });

            GoToPatientsDetails = new RelayCommand(obj =>
            {
                Console.Out.WriteLine(obj);
            });

            DeletePatientCommand = new RelayCommand(obj =>
            {
                this.DeleltePatient(obj);
            });

            EditPatientCommand = new RelayCommand(obj =>
            {
                var user = obj as User;
                //let's set up a little MVVM, cos that's what the cool kids are doing:
                var view = new EditPatientModal
                {
                    DataContext = new EditPatientViewModel(user)
                };

                //show the dialog
                var result = DialogHost.Show(view, "RootDialog", null, null);
            });

            LoadPatients(string.Empty);
        }

        private void LoadPatients(string query)
        {
            List<User> patientsList = db.User
                .Where(a => a.UserType == Enums.UserTypeEnum.Patient)
                .ToList()
                .Where(a => ((!string.IsNullOrEmpty(query) && $"{a.FirstName + a.LastName}".ToLower().IndexOf(query) > -1) || (string.IsNullOrEmpty(query))))
                .ToList();
            this.Patients = new ObservableCollection<User>(patientsList);
        }

        private void SaveNewPatient()
        {
            try
            {
                this.ErrorMessage = string.Empty;
                this.Patient.UserType = UserTypeEnum.Patient;
                db.User.Add(this.Patient);
                db.SaveChanges();
                CloseRootDialog();
                this.LoadPatients(string.Empty);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message + ex.InnerException?.Message;
            }
        }

        private void DeleltePatient(object obj)
        {
            var user = obj as User;

            if (user == null)
            {
                ErrorMessage = "Something went wrong!";
            }
            else
            {
                try
                {
                    var dbUser = db.User.Find(user.Id);
                    db.User.Remove(dbUser);
                    db.SaveChanges();
                    this.LoadPatients(this.SearchQuery);
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message + ex.InnerException?.Message;
                }
            }
        }
    }
}
