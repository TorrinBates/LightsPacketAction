using System;
using System.Windows.Input;

namespace LightsPacketAction {
    public class EditButtonMessageViewModel : ViewModelBase {
        public EditButtonMessageViewModel(string defaultValue, Action<string> okay) {
            _initialMessage = defaultValue;
            ButtonMessage = defaultValue;

            OkayCommand = new RelayCommand(x => _initialMessage != _buttonMessage, x => okay(ButtonMessage));
        }

        public ICommand OkayCommand { get; }

        string _initialMessage;
        string _buttonMessage;
        public string ButtonMessage { 
            get => _buttonMessage;
            set {
                _buttonMessage = value;
                OnPropertyChanged("ButtonMessage");
            } 
        }
    }
}
