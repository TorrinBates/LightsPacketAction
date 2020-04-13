
namespace LightsPacketAction {
    public class ButtonViewModel : ViewModelBase {
        public ButtonViewModel(string message) {
            Message = message;
        }

        string _message;
        public string Message {
            get => _message;
            set {
                _message = value;
                OnPropertyChanged("Message");
            }
        }
    }
}
