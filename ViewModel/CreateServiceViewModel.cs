
using System;
using System.Windows;
using System.Windows.Input;
using winetranet_application.Core;
using winetranet_application.Model;
using winetranet_application.View;

namespace winetranet_application.ViewModel
{
    class CreateServiceViewModel : ObservableObject
    {
        // Fields 

        private string _name;

        private string _errorMessage;
        private bool _isViewVisible = true;

        // Property
        public Service _service { get; set; }

        public string Action { get; set; }

        public string Name
        {
            get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); }
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
        public CreateServiceView Content { get; }

        // Constructor

        public CreateServiceViewModel()
        {
            Content = new CreateServiceView();
            Action = "Create";
            ActionCommand = new ViewModelCommand(ExecuteCreateCommand, CanExecuteCreateCommand);
        }

        public CreateServiceViewModel(Service service)
        {
            Content = new CreateServiceView();
            Action = "Update";

            Name = service.Name;

            _service = service;
            ActionCommand = new ViewModelCommand(ExecuteUpdateCommand, CanExecuteUpdateCommand);
        }

        // Methods


        private bool CanExecuteCreateCommand(object obj)
        {
            bool ValidData;

            if (string.IsNullOrWhiteSpace(Name))
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
                    var service = new Service();

                    service.Name = Name;

                    context.Services.Add(service);
                    context.SaveChangesAsync();

                    Window.GetWindow(this.Content).Close();
                }
            }
            catch (Exception ex)
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
                    var service = _service;

                    service.Name = Name;

                    context.Services.Update(service);
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

            if (string.IsNullOrWhiteSpace(Name))
            {
                ValidData = false;
            }
            else
            {
                ValidData = true;
            }
            return ValidData;
        }
    }
}

