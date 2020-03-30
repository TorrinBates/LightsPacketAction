using System.Linq;
using System.Windows.Input;

namespace LightsPacketAction {
    public class EditDisplayConfigurationViewModel : DisplayViewModel {
        ConfigHandler _configHandler;
        public EditDisplayConfigurationViewModel(ConfigHandler configHandler) : base(configHandler.GetActiveConfig()) {
            _configHandler = configHandler;

            SaveCommand = new RelayCommand(x => HasConfigChanged(), x => {
                _configHandler.SetActiveConfig(Rows, Columns, Buttons.ToList());
                _configHandler.SaveConfig();
            });

            CloseCommand = new RelayCommand(x => {
                var result = DialogFactory.CreateYesNoDialog("Are you sure you would like to exit? You will lose any changes you have made from the last time you saved!");
                if(result) DisplayWindow.Close();
            });

            DisplayWindow.InputBindings.Add(new KeyBinding(new RelayCommand(p => OverlayEnabled = !OverlayEnabled), Key.O, ModifierKeys.Control));

            DisplayWindow.Show();
        }

        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }

        bool _overlayEnabled;
        public bool OverlayEnabled { 
            get { return _overlayEnabled; } 
            private set { 
                _overlayEnabled = value;
                if (value) DisplayWindow.Cursor = null;
                else DisplayWindow.Cursor = Cursors.None;
                OnPropertyChanged("OverlayEnabled");
            }
        }

        private bool HasConfigChanged() {
            var currentConfig = _configHandler.GetActiveConfig();

            if (Rows != currentConfig.RowCount || Columns != currentConfig.ColumnCount || currentConfig.Buttons.Count != Buttons.Count) return true;

            for (int i = 0; i < currentConfig.Buttons.Count; i++)
                if (currentConfig.Buttons[i] != Buttons[i]) return true;

            return false;
        }
    }
}
