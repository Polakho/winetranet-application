using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using winetranet_application.Core;
using winetranet_application.Model;
using winetranet_application.View;

namespace winetranet_application.ViewModel
{
    class ServiceViewModel : ObservableObject
    {

        private ObservableCollection<Service> _services;
        public ObservableCollection<Service> Services
        {
            get { return _services; }
            set { _services = value; OnPropertyChanged(nameof(Services)); }
        }
        public ICollectionView ServicesView
        {
            get
            {
                var ServicesView = CollectionViewSource.GetDefaultView(Services);
                ServicesView.Filter = new Predicate<object>(o => Filter(o as Service));
                return ServicesView;
            }
        }
        private bool Filter(Service service)
        {
            return Search == null
                || service.Name.IndexOf(Search, StringComparison.OrdinalIgnoreCase) != -1;
        }

        private string search;

        public string Search
        {
            get { return search; }
            set
            {
                search = value;
                OnPropertyChanged("Search");
                ServicesView.Refresh();
            }
        }
        public RelayCommand CreateServiceViewCommand { get; set; }
        public RelayCommand EditServiceViewCommand { get; set; }
        public RelayCommand DeleteServiceViewCommand { get; set; }

        public ServiceViewModel()
        {
            Services = GetServices();

            CreateServiceViewCommand = new RelayCommand(o =>
            {
                ModalWindow modal = new ModalWindow();
                modal.Content = new CreateServiceView();
                modal.DataContext = new CreateServiceViewModel();
                modal.ShowDialog();
                if (!modal.IsVisible)
                {
                    Services = GetServices();
                    ServicesView.Refresh();
                }
            });

            EditServiceViewCommand = new RelayCommand(o =>
            {
                ModalWindow modal = new ModalWindow();
                modal.Content = new CreateServiceView();
                modal.DataContext = new CreateServiceViewModel((Service)o);
                modal.ShowDialog();
                if (!modal.IsVisible)
                {
                    Services = GetServices();
                    ServicesView.Refresh();
                }
            });

            DeleteServiceViewCommand = new RelayCommand(o =>
            {
                DeleteService((Service)o);
                Services = GetServices();
            });
        }

        private void DeleteService(Service o)
        {
            using (var context = new WinetranetContext())
            {
                var Users = context.Users.Where(users => users.Service == o.Id);
                if (!Users.Any())
                {
                    context.Services.Remove(o);
                    context.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Some users are affected to this service");
                }
            }
        }

        public static ObservableCollection<Service> GetServices()
        {
            using (var context = new WinetranetContext())
            {
                List<Service> services = context.Services.ToList();

                if (services != null)
                {
                    return new ObservableCollection<Service>(services);
                }
                else
                {
                    return new ObservableCollection<Service>(new List<Service> { });
                }
            }
        }
    }
}
