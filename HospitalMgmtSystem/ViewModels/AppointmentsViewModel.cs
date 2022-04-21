using HospitalMgmtSystem.DBContext;
using HospitalMgmtSystem.DTOs;
using HospitalMgmtSystem.Enums;
using HospitalMgmtSystem.Models;
using HospitalMgmtSystem.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HospitalMgmtSystem.ViewModels
{
    public class AppointmentsViewModel : BaseViewModel
    {
        public ObservableCollection<ComboBoxPair> Patients { get; set; }
        public ObservableCollection<ComboBoxPair> Doctors { get; set; }
        private ObservableCollection<AppointmentsView> _allAppointments = new ObservableCollection<AppointmentsView>();
        public ObservableCollection<AppointmentsView> AllAppointments { get { return _allAppointments; } set { _allAppointments = value; OnPropertyChanged(nameof(AllAppointments)); } }
        public ComboBoxPair PatientSelectedItem { get; set; }
        public ComboBoxPair DoctorSelectedItem { get; set; }

        private Appointments _appointment = new Appointments();
        public Appointments Appointments { get => _appointment; set { _appointment = value; OnPropertyChanged(nameof(Appointments)); } }
        private string _error;
        public string Error { get => _error; set { _error = value; OnPropertyChanged(nameof(Error)); } }
        public ICommand SaveCommand { get; set; }
        public ICommand NavigateToDetailsAction { get; set; }
        public ICommand deleteAppointment { get; set; }
        public ICommand ToggleDoctorAvailability { get; set; }
        public ICommand SearchAction { get; set; }

        private readonly HospitalDbContext db = new HospitalDbContext();
        private readonly User LoggedInUser = UserStore.User;
        public Visibility IsAdmin { get; set; }
        public string SearchQuery { get; set; }

        public AppointmentsViewModel()
        {
            NavigationStore.Instance.NonAuthenticatedViews = Visibility.Collapsed;
            NavigationStore.Instance.AuthenticatedViews = Visibility.Visible;
            if (UserStore.User.UserType == UserTypeEnum.Admin)
                NavigationStore.Instance.IsAdmin = Visibility.Visible;
            else
                NavigationStore.Instance.IsAdmin = Visibility.Collapsed;
            Patients = new ObservableCollection<ComboBoxPair>();
            Doctors = new ObservableCollection<ComboBoxPair>();
            foreach (var patient in db.User.Where(a => a.UserType == UserTypeEnum.Patient).ToList())
            {
                Patients.Add(new ComboBoxPair(patient.Id.ToString(), patient.FirstName + " " + patient.LastName));
            }
            foreach (var patient in db.User.Where(a => a.UserType == UserTypeEnum.Doctor).ToList())
            {
                Doctors.Add(new ComboBoxPair(patient.Id.ToString(), patient.FirstName + " " + patient.LastName));
            }

            SaveCommand = new RelayCommand(obj =>
            {
                SaveAppointment();
            });

            NavigateToDetailsAction = new RelayCommand(obj =>
            {
                //var appointment = obj as AppointmentsDto;
                //NavigationStore.Instance.CurrentViewModel = new PrescriptionViewModel(appointment);
            });

            deleteAppointment = new RelayCommand(obj =>
            {
                var appointment = obj as AppointmentsDto;
                if (appointment != null)
                {
                    db.Appointments.Remove(db.Appointments.Find(appointment.Id));
                    db.SaveChanges();
                    LoadAppointments(this.SearchQuery);
                }
            });

            ToggleDoctorAvailability = new RelayCommand(obj =>
            {
                var app = obj as AppointmentsDto;
                var appointmentDb = db.Appointments.Find(app.Id);
                appointmentDb.IsAvailable = !appointmentDb.IsAvailable;
                db.Entry(appointmentDb).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                LoadAppointments(this.SearchQuery);
            });

            SearchAction = new RelayCommand(obj =>
            {
                LoadAppointments(this.SearchQuery);
            });

            LoadAppointments(this.SearchQuery);

            if (UserStore.User.UserType == UserTypeEnum.Admin)
                IsAdmin = Visibility.Visible;
            else
                IsAdmin = Visibility.Collapsed;
        }

        private void LoadAppointments(string query)
        {
            List<AppointmentsDto> list = new List<AppointmentsDto>();
            switch (LoggedInUser.UserType)
            {
                case UserTypeEnum.Admin:
                    list = (from a in db.Appointments.ToList()
                            join d in db.User.ToList() on a.DoctorId equals d.Id
                            join p in db.User.ToList() on a.PatientId equals p.Id

                            where (string.IsNullOrEmpty(query) ||
                            (!string.IsNullOrEmpty(query) && $"{d.FirstName + d.LastName}".ToLower().IndexOf(query.ToLower()) > -1) ||
                            (!string.IsNullOrEmpty(query) && $"{p.FirstName + p.LastName}".ToLower().IndexOf(query.ToLower()) > -1))

                            && a.AppointmentDate.ToString("d") == DateTime.Now.ToString("d")

                            select new AppointmentsDto()
                            {
                                Id = a.Id,
                                AppointmentDate = a.AppointmentDate.ToString("D"),
                                AppointmentTime = a.AppointmentTime.ToString("t"),
                                DoctorName = d.FirstName + " " + d.LastName,
                                PatientName = p.FirstName + " " + p.LastName,
                                Bill = a.Bill.Value,
                                Duration = a.Duration.Value,
                                IsAvailable = a.IsAvailable,
                                Room = a.Room
                            }).ToList();
                    break;
                case UserTypeEnum.Doctor:
                    list = (from a in db.Appointments.ToList()
                            join d in db.User.ToList() on a.DoctorId equals d.Id
                            join p in db.User.ToList() on a.PatientId equals p.Id
                            where a.DoctorId == LoggedInUser.Id

                            && (string.IsNullOrEmpty(query) ||
                            (!string.IsNullOrEmpty(query) && $"{d.FirstName + d.LastName}".ToLower().IndexOf(query.ToLower()) > -1) ||
                            (!string.IsNullOrEmpty(query) && $"{p.FirstName + p.LastName}".ToLower().IndexOf(query.ToLower()) > -1))

                            && a.AppointmentDate.ToString("d") == DateTime.Now.ToString("d")

                            select new AppointmentsDto()
                            {
                                Id = a.Id,
                                AppointmentDate = a.AppointmentDate.ToString("D"),
                                AppointmentTime = a.AppointmentTime.ToString("t"),
                                DoctorName = d.FirstName + " " + d.LastName,
                                PatientName = p.FirstName + " " + p.LastName,
                                Bill = a.Bill.Value,
                                Duration = a.Duration.Value,
                                IsAvailable = a.IsAvailable,
                                Room = a.Room
                            }).ToList();
                    break;
                case UserTypeEnum.Patient:
                    list = (from a in db.Appointments.ToList()
                            join d in db.User.ToList() on a.DoctorId equals d.Id
                            join p in db.User.ToList() on a.PatientId equals p.Id
                            where a.PatientId == LoggedInUser.Id

                            && (string.IsNullOrEmpty(query) ||
                            (!string.IsNullOrEmpty(query) && $"{d.FirstName + d.LastName}".ToLower().IndexOf(query.ToLower()) > -1) ||
                            (!string.IsNullOrEmpty(query) && $"{p.FirstName + p.LastName}".ToLower().IndexOf(query.ToLower()) > -1))

                            && a.AppointmentDate.ToString("d") == DateTime.Now.ToString("d")

                            select new AppointmentsDto()
                            {
                                Id = a.Id,
                                AppointmentDate = a.AppointmentDate.ToString("D"),
                                AppointmentTime = a.AppointmentTime.ToString("t"),
                                DoctorName = d.FirstName + " " + d.LastName,
                                PatientName = p.FirstName + " " + p.LastName,
                                Bill = a.Bill.Value,
                                Duration = a.Duration.Value,
                                IsAvailable = a.IsAvailable,
                                Room = a.Room
                            }).ToList();
                    break;
            }

            var doctors = list.Select(a => a.DoctorName).Distinct();
            AllAppointments = new ObservableCollection<AppointmentsView>();

            foreach (var doc in doctors)
            {
                var appointmentView = new AppointmentsView();
                appointmentView.DoctorName = doc;
                appointmentView.Appointments = new ObservableCollection<AppointmentsDto>(list.Where(a => a.DoctorName == doc));
                AllAppointments.Add(appointmentView);

            }
        }

        private void SaveAppointment()
        {
            Error = string.Empty;
            Appointments.DoctorId = int.Parse(this.DoctorSelectedItem.Key);
            Appointments.PatientId = int.Parse(this.PatientSelectedItem.Key);
            if (Validate())
            {
                db.Appointments.Add(Appointments);
                db.SaveChanges();
                CloseRootDialog();
                LoadAppointments(this.SearchQuery);
            }
        }

        public bool Validate()
        {

            if (Appointments.AppointmentDate == DateTime.MinValue)
            {
                Error = "Appointment Date Can't Have empty Values";
                return false;
            }
            if (PatientSelectedItem == null)
            {
                Error = "Patient Can't Have empty Values";

                return false;
            }
            if (DoctorSelectedItem == null)
            {
                Error = "Doctor Can't Have empty Values";

                return false;
            }

            if (Appointments.Duration == null)
            {
                Error = "Duration Can't Have empty Values";
                return false;
            }
            if (Appointments.Bill == null)
            {
                Error = "Bill Can't Have empty Values";
                return false;
            }

            if (DrAlreadyHasAnAppointment())
            {
                Error = "Doctor already has an appointment at this time!";
                return false;
            }
            return true;
        }

        private bool DrAlreadyHasAnAppointment()
        {
            var alreadyExists = db.Appointments.
                ToList().
                Where(
                a => !a.IsAvailable &&
                a.AppointmentDate.ToString("d") == this.Appointments.AppointmentDate.ToString("d") &&
                a.AppointmentTime.ToString("t") == this.Appointments.AppointmentTime.ToString("t") &&
                a.DoctorId == this.Appointments.DoctorId).
                FirstOrDefault();
            return alreadyExists != null;
        }
    }
}
