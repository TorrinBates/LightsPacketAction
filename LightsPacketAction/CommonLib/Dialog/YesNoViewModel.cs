using System;
using System.Windows.Input;

namespace LightsPacketAction {
    public class YesNoViewModel : ViewModelBase {
        public YesNoViewModel(string message, Action close) {

            Message = message;

            NoCommand = new RelayCommand(x => {
                Result = false;
                close();
            });

            YesCommand = new RelayCommand(x => {
                Result = true;
                close();
            });
        }

        public ICommand NoCommand { get; }
        public ICommand YesCommand { get; }
        public string Message { get; }
        public bool Result = false;
    }
}
