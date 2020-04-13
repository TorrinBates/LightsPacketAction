using System;
using System.Windows.Input;

namespace LightsPacketAction {
    public class EditButtonMessageViewModel : ViewModelBase {
        public EditButtonMessageViewModel(string defaultValue, Action close) {
            ButtonMessage = defaultValue;

            CloseCommand = new RelayCommand(x => {
                if (bool.TryParse(x as string, out bool resultVal)) Result = resultVal;
                close();
            });
        }

        public ICommand CloseCommand { get; }

        string _buttonMessage;
        public string ButtonMessage { 
            get => _buttonMessage;
            set {
                _buttonMessage = value;
                OnPropertyChanged("ButtonMessage");
            } 
        }

        public bool Result { get; private set; } = false;
    }
}
