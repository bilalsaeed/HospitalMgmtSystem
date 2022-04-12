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
using System.Windows.Input;

namespace HospitalMgmtSystem.ViewModels
{
    public class AppointmentsViewModel : BaseViewModel
    {
        public ObservableCollection<ComboBoxPair> Patients { get; set; }
        public ObservableCollection<ComboBoxPair> Doctors { get; set; }
        private ObservableCollection<AppointmentsDto> _allAppointments = new ObservableCollection<AppointmentsDto>();
        public ObservableCollection<AppointmentsDto> AllAppointments { get { return _allAppointments; } set { _allAppointments = value; OnPropertyChanged(nameof(AllAppointments)); } }
        public ComboBoxPair PatientSelectedItem { get; set; }
        public ComboBoxPair DoctorSelectedItem { get; set; }

        private Appointments _appointment = new Appointments();
        public Appointments Appointments { get => _appointment; set { _appointment = value; OnPropertyChanged(nameof(Appointments)); } }
        private string _error;
        public string Error { get => _error; set { _error = value; OnPropertyChanged(nameof(Error)); } }
        public ICommand SaveCommand { get; set; }

        private readonly HospitalDbContext db = new HospitalDbContext();
        private readonly User LoggedInUser = UserStore.User;
        public AppointmentsViewModel()
        {
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

            LoadAppointments();
        }

        private void LoadAppointments()
        {
            List<AppointmentsDto> list = new List<AppointmentsDto>();
            switch (LoggedInUser.UserType)
            {
                case UserTypeEnum.Admin:
                    list = (from a in db.Appointments.ToList()
                            join d in db.User.ToList() on a.DoctorId equals d.Id
                            join p in db.User.ToList() on a.PatientId equals p.Id

                            select new AppointmentsDto()
                            {
                                Id = a.Id,
                                AppointmentDate = a.AppointmentDate.ToString("d"),
                                AppointmentTime = a.AppointmentTime.ToString("t"),
                                DoctorName = d.FirstName + " " + d.LastName,
                                PatientName = p.FirstName + " " + p.LastName,
                                Bill = a.Bill.Value,
                                Duration = a.Duration.Value
                            }).ToList();
                    break;
                case UserTypeEnum.Doctor:
                    list = (from a in db.Appointments.ToList()
                            join d in db.User.ToList() on a.DoctorId equals d.Id
                            join p in db.User.ToList() on a.PatientId equals p.Id
                            where a.DoctorId == LoggedInUser.Id

                            select new AppointmentsDto()
                            {
                                Id = a.Id,
                                AppointmentDate = a.AppointmentDate.ToString("d"),
                                AppointmentTime = a.AppointmentTime.ToString("t"),
                                DoctorName = d.FirstName + " " + d.LastName,
                                PatientName = p.FirstName + " " + p.LastName,
                                Bill = a.Bill.Value,
                                Duration = a.Duration.Value
                            }).ToList();
                    break;
                case UserTypeEnum.Patient:
                    list = (from a in db.Appointments.ToList()
                            join d in db.User.ToList() on a.DoctorId equals d.Id
                            join p in db.User.ToList() on a.PatientId equals p.Id
                            where a.PatientId == LoggedInUser.Id

                            select new AppointmentsDto()
                            {
                                Id = a.Id,
                                AppointmentDate = a.AppointmentDate.ToString("d"),
                                AppointmentTime = a.AppointmentTime.ToString("t"),
                                DoctorName = d.FirstName + " " + d.LastName,
                                PatientName = p.FirstName + " " + p.LastName,
                                Bill = a.Bill.Value,
                                Duration = a.Duration.Value
                            }).ToList();
                    break;
            }
            AllAppointments = new ObservableCollection<AppointmentsDto>(list);
        }

        private void SaveAppointment()
        {
            Error = string.Empty;
            if (Validate())
            {
                Appointments.DoctorId = int.Parse(this.DoctorSelectedItem.Key);
                Appointments.PatientId = int.Parse(this.PatientSelectedItem.Key);
                db.Appointments.Add(Appointments);
                db.SaveChanges();
                CloseRootDialog();
                LoadAppointments();
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
            return true;
        }

        private bool AlreadyHasAppointment()
        {
            return false;
        }
    }
}
