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
            NextCommand = new RelayCommand((p) => { CurrentButtonView = dual; CurrentFormView = messages; });
            PreviousCommand = new RelayCommand((p) => { CurrentButtonView = next; CurrentFormView = layout; });
            next = new NextViewModel(NextCommand);
            dual = new DualButtonViewModel(PreviousCommand, ApplyCommand);
            layout = new LayoutViewModel();
            messages = new MessagesViewModel();
            CurrentButtonView = next;
            CurrentFormView = layout;
        }

        public ICommand PreviousCommand { get; private set; }
        public ICommand ApplyCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        private NextViewModel next;
        private DualButtonViewModel dual;
        private LayoutViewModel layout;
        private MessagesViewModel messages;

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

        private object _currentFormView;
        public object CurrentFormView
        {
            get { return _currentFormView; }
            set
            {
                _currentFormView = value;
                OnPropertyChanged("CurrentFormView");
            }
        }
    }
}
