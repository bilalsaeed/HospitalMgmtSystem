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
    public class AdminsViewModel: BaseViewModel
    {
        private ObservableCollection<User> _admins = new ObservableCollection<User>();
        private User _admin = new User();
        public User Admin
        {
            get => _admin;
            set
            {
                _admin = value;
                OnPropertyChanged(nameof(Admin));
            }
        }
        public ICommand SearchAction { get; set; }
        public ICommand CloseAddAdminModal { get; set; }
        public ICommand GoToAdminDetails { get; set; }
        public ICommand DeleteAdminCommand { get; set; }
        public ICommand EditAdminCommand { get; set; }
        public ICommand AddPatientModalCommand { get; set; }

        public string SearchQuery { get; set; }
        public ObservableCollection<User> Admins
        {
            get => _admins;
            set
            {
                _admins = value;
                OnPropertyChanged(nameof(Admins));
            }
        }
        private readonly DBContext.HospitalDbContext db = new DBContext.HospitalDbContext();

        private void LoadAdmins(string query)
        {
            List<User> adminsList = db.User
                .Where(a => a.UserType == Enums.UserTypeEnum.Admin)
                .ToList()
                .Where(a => ((!string.IsNullOrEmpty(query) && $"{a.FirstName + a.LastName}".ToLower().IndexOf(query) > -1) || (string.IsNullOrEmpty(query))))
                .ToList();
            this.Admins = new ObservableCollection<User>(adminsList);
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
                    this.LoadAdmins(this.SearchQuery);
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message + ex.InnerException?.Message;
                }
            }
        }
        public AdminsViewModel()
        {
            SearchAction = new RelayCommand(obj =>
            {
                this.LoadAdmins(this.SearchQuery);
            });

            CloseAddAdminModal = new RelayCommand(obj =>
            {
                this.CloseRootDialog();
                this.Admin = new User();
                this.ErrorMessage = string.Empty;
            });

            GoToAdminDetails = new RelayCommand(obj =>
            {
                Console.Out.WriteLine(obj);
            });

            DeleteAdminCommand = new RelayCommand(obj =>
            {
                this.DeleteAdmin(obj);
            });

            AddPatientModalCommand = new RelayCommand(obj =>
            {
                OpenAddEditAdminModal(obj);
            });

            EditAdminCommand = new RelayCommand(obj =>
            {
                OpenAddEditAdminModal(obj);
            });

            LoadAdmins(string.Empty);
        }

        private void OpenAddEditAdminModal(object obj)
        {
            var user = obj as User;
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new AddEditAdminModal
            {
                DataContext = new AddEditAdminViewModel(user)
            };

            //show the dialog
            var result = DialogHost.Show(view, "RootDialog", null, null);
        }
    }
}
