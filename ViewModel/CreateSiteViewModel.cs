
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using winetranet_application.Core;
using winetranet_application.Model;
using winetranet_application.View;

namespace winetranet_application.ViewModel
{
    class CreateSiteViewModel : ObservableObject
    {
        // Fields 

        private string _ville;

        private string _errorMessage;
        private bool _isViewVisible = true;

        // Property
        public Model.Site _site { get; set; }

        public string Action { get; set; }

        public string Ville
        {
            get => _ville; set { _ville = value; OnPropertyChanged(nameof(Ville)); }
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
        public CreateSiteView Content { get; }

        // Constructor

        public CreateSiteViewModel()
        {
            Content = new CreateSiteView();
            Action = "Create";
            ActionCommand = new ViewModelCommand(ExecuteCreateCommand, CanExecuteCreateCommand);
        }

        public CreateSiteViewModel(Model.Site site)
        {
            Content = new CreateSiteView();
            Action = "Update";

            Ville = site.Ville;

            _site = site;
            ActionCommand = new ViewModelCommand(ExecuteUpdateCommand, CanExecuteUpdateCommand);
        }

        // Methods


        private bool CanExecuteCreateCommand(object obj)
        {
            bool ValidData;

            if (string.IsNullOrWhiteSpace(Ville))
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
                    var site = new Model.Site();

                     site.Ville = Ville;

                    context.Sites.Add(site);
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
                    var site = _site;

                    site.Ville = Ville;

                    context.Sites.Update(site);
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

            if (string.IsNullOrWhiteSpace(Ville))
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

