using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using winetranet_application.Model;
using winetranet_application.View;
using System.Security;
using winetranet_application.Core;

namespace winetranet_application.ViewModel
{
    public class AdminLoginViewModel : ObservableObject
    {
        // Fields 

        private SecureString _password;

        private string _errorMessage;
        private bool _isViewVisible = true;

        // Property


        public SecureString Password
        {
            get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }
        public bool IsViewVisible
        {
            get => _isViewVisible;
            set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); }
        }

        // Commands
        public ICommand LoginAdminCommand { get; }
        public AdminLoginView Content { get; }

        // Constructor

        public AdminLoginViewModel()
        {
            Content = new AdminLoginView();

            LoginAdminCommand = new RelayCommand(o =>
            {
                MessageBox.Show("hello");
               // Login();
            });
        }

        // Methods





    }


}

