using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using winetranet_application.Core;

namespace winetranet_application.ViewModel
{
    class ModalViewModel : ObservableObject
    {
        private object? _content;

        public object Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        
        public ModalViewModel(UserControl _content)
        {
            Content = _content;
        }


    }
}
