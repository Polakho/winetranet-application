using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using winetranet_application.Core;
using winetranet_application.Model;
using winetranet_application.View;

namespace winetranet_application.ViewModel
{
    class UserViewModel : ObservableObject
    {

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set { _users = value; OnPropertyChanged(nameof(Users)); }
        }
        public ICollectionView UsersView
        {
            get
            {
                var UsersView = CollectionViewSource.GetDefaultView(Users);
                UsersView.Filter = new Predicate<object>(o => Filter(o as User));
                return UsersView;
            }
        }
        private bool Filter(User user)
        {
            return Search == null
                || user.Firstname.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1
                || user.Lastname.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1
                || user.Email.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1;
        }

        private string search;

        public string Search
        {
            get { return search; }
            set
            {
                search = value;
                OnPropertyChanged("Search");
                UsersView.Refresh();
            }
        }
        public RelayCommand CreateUserViewCommand { get; set; }
        public RelayCommand EditUserViewCommand { get; set; }
        public RelayCommand DeleteUserViewCommand { get; set; }

        public UserViewModel()
        {
            Users = GetUsers();

            CreateUserViewCommand = new RelayCommand(o =>
            {
                ModalWindow modal = new ModalWindow();
                modal.Content = new CreateUserView();
                modal.DataContext = new CreateUserViewModel();
                modal.ShowDialog();
                if (!modal.IsVisible)
                {
                    Users = GetUsers();
                    UsersView.Refresh();
                }
            });

            EditUserViewCommand = new RelayCommand(o =>
            {
                ModalWindow modal = new ModalWindow();
                modal.Content = new CreateUserView();
                modal.DataContext = new CreateUserViewModel((User) o);
                modal.ShowDialog();
                if (!modal.IsVisible)
                {
                    Users = GetUsers();
                    UsersView.Refresh();
                }
            });

            DeleteUserViewCommand = new RelayCommand(o =>
            {
                DeleteUser((User)o);
                Users = GetUsers();
            });
        }

        private void DeleteUser(User o)
        {
            using (var context = new WinetranetContext())
            {
                context.Users.Remove(o);
                context.SaveChanges();
            }
        }

        public static ObservableCollection<User> GetUsers()
        {
            using (var context = new WinetranetContext())
            {
                List<User> users = context.Users.ToList();

                if (users != null)
                {
                    return new ObservableCollection<User>(users);
                }
                else
                {
                    return new ObservableCollection<User>(new List<User> { });
                }
            }
        }
    }
}
