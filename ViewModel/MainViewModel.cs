using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using winetranet_application.Core;
using winetranet_application.View;

namespace winetranet_application.ViewModel
{
     class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }   
        public RelayCommand UserViewCommand { get; set; }
        public RelayCommand SiteViewCommand { get; set; }
        public RelayCommand ServiceViewCommand { get; set; }
        public RelayCommand LoginAdminCommand { get; set; }



        public HomeViewModel HomeVm { get; set; }
        public UserViewModel UserVm { get; set; }
        public SiteViewModel SiteVm { get; set; }
        public ServiceViewModel ServiceVm { get; set; }
        public AdminLoginViewModel AdminVm { get; set; }

        private ObservableObject _currentView;

        public ObservableObject CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        
        public MainViewModel()
        {
            HomeVm = new HomeViewModel();
            UserVm= new UserViewModel();
            SiteVm= new SiteViewModel();
            ServiceVm= new ServiceViewModel();
            AdminVm = new AdminLoginViewModel();

            CurrentView= AdminVm;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVm;
            });

            UserViewCommand = new RelayCommand(o =>
            {
                CurrentView = UserVm;
            });
            
            SiteViewCommand = new RelayCommand(o =>
            {
                CurrentView = SiteVm;
            });

            ServiceViewCommand = new RelayCommand(o =>
            {
                CurrentView = ServiceVm;
            });

            LoginAdminCommand = new RelayCommand(o =>
            {
                LoginView();
            });
        }

        public void LoginView()
        {
            if(CurrentView.AdminElements == "Hidden")
            {
                
                ModalWindow modal = new ModalWindow();
                modal.Content = new AdminLoginView();
                modal.DataContext = new AdminLoginViewModel();
                modal.ShowDialog();

                CurrentView.AdminElements = "Visible";
                UserVm.AdminElements = "Visible";
                SiteVm.AdminElements = "Visible";
                ServiceVm.AdminElements = "Visible";
                AdminElements = "Visible";
            }
            else if (CurrentView.AdminElements == "Visible")
            {
                CurrentView.AdminElements = "Hidden";
                AdminElements = "Hidden";
                CurrentView.AdminElements = "Hidden";
                UserVm.AdminElements = "Hidden";
                SiteVm.AdminElements = "Hidden";
                ServiceVm.AdminElements = "Hidden";
            }

        }

    }
}
