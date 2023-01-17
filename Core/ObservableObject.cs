using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace winetranet_application.Core
{
     public abstract class ObservableObject : INotifyPropertyChanged
    {
        private string _adminElements = "Hidden";

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string AdminElements
        {
            get { return _adminElements; }
            set
            {
                _adminElements = value;
                OnPropertyChanged(nameof(AdminElements));
            }
        }
    }
}
