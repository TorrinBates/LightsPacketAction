using System.Windows.Input;

namespace LightsPacketAction
{
    class ErrorViewModel : ViewModelBase
    {
        public ErrorViewModel(string _error, RelayCommand close)
        {
            ErrorMessage = _error;
            CloseErrorCommand = close;
        }

        public ICommand CloseErrorCommand { get; private set; }

        public string ErrorMessage { get; private set; }
    }
}
