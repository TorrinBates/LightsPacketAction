using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LightsPacketAction
{
    class ConfigureViewModel : ViewModelBase
    {
        public ConfigureViewModel(RelayCommand close)
        {
            CancelCommand = close;
        }

        public ICommand CancelCommand { get; private set; }
    }
}
