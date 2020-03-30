using System.Windows.Input;

namespace LightsPacketAction {
    public class EditDisplayConfigurationViewModel : DisplayViewModel {
        public EditDisplayConfigurationViewModel(ConfigHandler configHandler) : base(configHandler.GetActiveConfig()) {



            DisplayWindow.InputBindings.Add(new KeyBinding(new RelayCommand(p => OverlayEnabled = !OverlayEnabled), Key.O, ModifierKeys.Control));

            DisplayWindow.Show();
        }

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
    }
}
