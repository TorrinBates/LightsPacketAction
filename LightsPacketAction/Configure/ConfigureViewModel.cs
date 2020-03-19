using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LightsPacketAction
{
    public class ConfigureViewModel : ViewModelBase
    {
        public ConfigureViewModel()
        {
            NextCommand = new RelayCommand((p) => { CurrentButtonView = dual; });
            PreviousCommand = new RelayCommand((p) => { CurrentButtonView = next; });
            next = new NextViewModel(NextCommand);
            dual = new DualButtonViewModel(PreviousCommand, ApplyCommand);
            CurrentButtonView = next;
        }

        public ICommand PreviousCommand { get; private set; }
        public ICommand ApplyCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        private NextViewModel next;
        private DualButtonViewModel dual;

        private object _currentButtonView;
        public object CurrentButtonView
        {
            get { return _currentButtonView; }
            set
            {
                _currentButtonView = value;
                OnPropertyChanged("CurrentButtonView");
            }
        }
    }
}
