using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LightsPacketAction {
    public class EditDisplayConfigurationViewModel : DisplayViewModel {
        ConfigHandler _configHandler;
        public EditDisplayConfigurationViewModel(ConfigHandler configHandler) : base(configHandler.GetActiveConfig()) {
            _configHandler = configHandler;

            SaveCommand = new RelayCommand(x => HasConfigChanged(), x => {
                _configHandler.SetActiveConfig(Rows, Columns, Buttons.Select(y => y.Message).ToList());
                _configHandler.SaveConfig();
            });

            CloseCommand = new RelayCommand(x => {
                var hasConfigChanged = HasConfigChanged();
                if(!hasConfigChanged || (hasConfigChanged && DialogFactory.CreateYesNoDialog("Exit Configuration", "Are you sure you would like to exit? You will lose any changes you have made since the last time you saved!")))
                    base.CloseCommand.Execute(null);
            });

            ButtonClickCommand = new RelayCommand(x => {
                CustomWindow window = null;
                var button = (ButtonViewModel)x;
                var vm = new EditButtonMessageViewModel(button.Message, () => window.Close());
                window = new CustomWindow(vm, "Edit Button Message");
                window.Owner = Application.Current.MainWindow;
                window.MinimizeVisibility = Visibility.Collapsed;
                window.XVisibility = Visibility.Collapsed;
                window.Height = 160;

                window.ShowDialog();

                if (vm.Result) {
                    button.Message = vm.ButtonMessage;
                    if(SaveCommand.CanExecute(null)) SaveCommand.Execute(null);
                }
            });

            DisplayLines = true;
            DisplayWindow.Cursor = null;
            DisplayWindow.Show();
        }

        public override ICommand ButtonClickCommand { get; }
        public ICommand SaveCommand { get; }
        public override ICommand CloseCommand { get; }

        private bool HasConfigChanged() {
            var currentConfig = _configHandler.GetActiveConfig();

            if (Rows != currentConfig.RowCount || Columns != currentConfig.ColumnCount || currentConfig.Buttons.Count != Buttons.Count) return true;

            for (int i = 0; i < currentConfig.Buttons.Count; i++)
                if (currentConfig.Buttons[i] != Buttons[i].Message) return true;

            return false;
        }
    }
}
