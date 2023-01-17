
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using winetranet_application.Core;
using winetranet_application.Model;
using winetranet_application.View;

namespace winetranet_application.ViewModel
{
     class CreateUserViewModel : ObservableObject
    {
        // Fields 

        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private string _phoneMobile;
        private Service _service;
        private Site _site;
        
        private string _errorMessage;
        private bool _isViewVisible = true;

        // Property
        public User _user { get; set; }

        public string Action { get; set; }

        public string FirstName
        {
            get => _firstName; set{ _firstName = value; OnPropertyChanged(nameof(FirstName));}
        }
        public string LastName
        {
            get => _lastName; set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
        }
        public string Email
        {
            get => _email; set { _email = value; OnPropertyChanged(nameof(Email)); }
        }
        public string Phone
        {
            get => _phone; set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }
        public string PhoneMobile
        {
            get => _phoneMobile; set { _phoneMobile = value; OnPropertyChanged(nameof(PhoneMobile)); }
        }
        public Service Service
        {
            get => _service; set { _service = value; OnPropertyChanged(nameof(Service)); }
        }
        public Site Site
        {
            get => _site; set { _site = value; OnPropertyChanged(nameof(Site)); }
        }

        public List<Service> AllServices
        {
            get; set;
        }
        public List<Site> AllSites
        {
            get; set;
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
        public ICommand ActionCommand { get; }
        public CreateUserView Content { get; }

        // Constructor

        public CreateUserViewModel()
        {
            Content = new CreateUserView();
            Action = "Create";
            

            AllServices = GetAllServices();
            AllSites = GetAllSites();
            ActionCommand = new ViewModelCommand(ExecuteCreateCommand, CanExecuteCreateCommand);
        }

        public CreateUserViewModel(User user)
        {
            Content = new CreateUserView();
            Action = "Update";

            FirstName = user.Firstname;
            LastName = user.Lastname;
            Email = user.Email;
            Phone= user.Phone;
            PhoneMobile= user.PhoneMobile;
            Service = GetServiceById(user.Service);
            Site = GetSiteById(user.Site);

            AllServices = GetAllServices();
            AllSites = GetAllSites();
            _user = user;
            ActionCommand = new ViewModelCommand(ExecuteUpdateCommand, CanExecuteUpdateCommand);
        }

        // Methods


        private bool CanExecuteCreateCommand(object obj)
        {
            bool ValidData;

            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            {
                ValidData = false;
            }
            else
            {
                ValidData = true;
            }
            return ValidData;
        }

        private void ExecuteCreateCommand(object obj)
        {
            try
            {
                using (var context = new WinetranetContext())
                {
                    var user = new User();

                    user.Firstname = FirstName;
                    user.Lastname = LastName;
                    user.Email = Email;
                    user.Phone = Phone;
                    user.PhoneMobile = PhoneMobile;
                    user.Site = Site.Id;
                    user.Service = Service.Id;

                    context.Users.Add(user);
                    context.SaveChangesAsync();

                    Window.GetWindow(this.Content).Close();
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void ExecuteUpdateCommand(object obj)
        {
            try
            {
                using (var context = new WinetranetContext())
                {
                    var user = _user;

                    user.Firstname = FirstName;
                    user.Lastname = LastName;
                    user.Email = Email;
                    user.Phone = Phone;
                    user.PhoneMobile = PhoneMobile;
                    user.Site = Site.Id;
                    user.Service = Service.Id;

                    context.Users.Update(user);
                    context.SaveChangesAsync();

                    Window.GetWindow(this.Content).Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private bool CanExecuteUpdateCommand(object obj)
        {
            bool ValidData;

            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            {
                ValidData = false;
            }
            else
            {
                ValidData = true;
            }
            return ValidData;
        }

        private List<Site> GetAllSites()
        {
            using (var context = new WinetranetContext())
            {

                List<Site> sites = context.Sites.ToList();
                return sites;
            }
        }

        private Site GetSiteById(int? siteId)
        {
            using (var context = new WinetranetContext())
            {
               Site site = context.Sites.FirstOrDefault(x => x.Id == siteId);
                return site;
            }
        }

        private List<Service> GetAllServices()
        {
            using (var context = new WinetranetContext())
            {

                List<Service> services = context.Services.ToList();
                return services;
            }
        }

        private Service GetServiceById(int? serviceId)
        {
            using (var context = new WinetranetContext())
            {
                Service service = context.Services.FirstOrDefault(x => x.Id == serviceId);
                return service;
            }
        }
    }
}

