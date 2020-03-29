using System;
using System.Windows.Input;

namespace LightsPacketAction
{
    public class ErrorViewModel : ViewModelBase
    {
        public ErrorViewModel(string error, Action close)
        {
            ErrorMessage = error;
            CloseErrorCommand = new RelayCommand(x => close());
        }

        public ICommand CloseErrorCommand { get; }
        public string ErrorMessage { get; }
    }
}
