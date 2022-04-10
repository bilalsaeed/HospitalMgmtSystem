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
    public class DoctorsViewModel: BaseViewModel
    {
        private ObservableCollection<User> _doctors = new ObservableCollection<User>();
        private User _doctor = new User();
        public User Doctor
        {
            get => _doctor;
            set
            {
                _doctor = value;
                OnPropertyChanged(nameof(Doctor));
            }
        }

       
        public ICommand SaveButton { get; set; }
        public ICommand SearchAction { get; set; }
        public ICommand CloseModal { get; set; }
        public ICommand GoToDetails { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand AddModalCommand { get; set; }

        public string SearchQuery { get; set; }
        public ObservableCollection<User> Doctors
        {
            get => _doctors;
            set
            {
                _doctors = value;
                OnPropertyChanged(nameof(Doctors));
            }
        }
        private readonly DBContext.HospitalDbContext db = new DBContext.HospitalDbContext();

        private void LoadDoctors(string query)
        {
            List<User> adminsList = db.User
                .Where(a => a.UserType == Enums.UserTypeEnum.Doctor)
                .ToList()
                .Where(a => ((!string.IsNullOrEmpty(query) && $"{a.FirstName + a.LastName}".ToLower().IndexOf(query) > -1) || (string.IsNullOrEmpty(query))))
                .ToList();
            this.Doctors = new ObservableCollection<User>(adminsList);
        }

        private void SaveNewAdmin()
        {
            try
            {
                this.ErrorMessage = string.Empty;
                this.Doctor.UserType = UserTypeEnum.Admin;
                db.User.Add(this.Doctor);
                db.SaveChanges();
                CloseRootDialog();
                this.LoadDoctors(string.Empty);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message + ex.InnerException?.Message;
            }
        }

        private void DeleteAdmin(object obj)
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
                    this.LoadDoctors(this.SearchQuery);
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message + ex.InnerException?.Message;
                }
            }
        }
        public DoctorsViewModel()
        {

            SearchAction = new RelayCommand(obj =>
            {
                this.LoadDoctors(this.SearchQuery);
            });

            CloseModal = new RelayCommand(obj =>
            {
                this.CloseRootDialog();
                this.Doctor = new User();
                this.ErrorMessage = string.Empty;
            });

            GoToDetails = new RelayCommand(obj =>
            {
                Console.Out.WriteLine(obj);
            });

            DeleteCommand = new RelayCommand(obj =>
            {
                this.DeleteAdmin(obj);
            });

            AddModalCommand = new RelayCommand(obj =>
            {
                OpenModal(obj);
            });

            EditCommand = new RelayCommand(obj =>
            {
                OpenModal(obj);
            });

            LoadDoctors(string.Empty);
        }

        private void OpenModal(object obj)
        {
            var user = obj as User;
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new DoctorsModal
            {
                DataContext = new AddEditDoctorsViewModel(user)
            };

            //show the dialog
            var result = DialogHost.Show(view, "RootDialog", null, null);
        }
    }
}
