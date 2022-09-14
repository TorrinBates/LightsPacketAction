using Microsoft.Win32;
using System;
using System.Linq;
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

            ImportCommand = new RelayCommand(x => {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "XML-File | *.xml";
                ofd.Title = "Import Button Configuration";
                if (ofd.ShowDialog() == true) {
                    var result = configHandler.OpenConfig(ofd.FileName);
                    if (result.Item4.ReturnCode != ConfigHandlerReturnCodeType.Success)
                        DialogFactory.CreateErrorDialog("Export Configuration Failed", result.Item4.Reason);
                    else {
                        Rows = result.Item1;
                        Columns = result.Item2;
                        Buttons.Clear();
                        foreach (var message in result.Item3)
                            Buttons.Add(new ButtonViewModel(message)); 
                    }
                }
            });

            ExportCommand = new RelayCommand(x => {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "LPAButtonConfig.xml";
                sfd.Filter = "XML-File | *.xml";
                sfd.Title = "Export Button Configuration";
                if (sfd.ShowDialog() == true) {
                    var result = configHandler.SaveConfig(sfd.FileName, new Config(Rows, Columns, Buttons.Select(b => b.Message).ToList()));
                    if (result.ReturnCode != ConfigHandlerReturnCodeType.Success)
                        DialogFactory.CreateErrorDialog("Export Configuration Failed", result.Reason);
                }
            });

            CloseCommand = new RelayCommand(x => {
                var hasConfigChanged = HasConfigChanged();
                if (!hasConfigChanged || (hasConfigChanged && DialogFactory.CreateYesNoDialog("Exit Configuration", "Unsaved changes exist. Are you sure you want to exit?")))
                    base.CloseCommand.Execute(null);
            });

            ButtonClickCommand = new RelayCommand(x => {
                var button = (ButtonViewModel)x;
                CurrentButton = new EditButtonMessageViewModel(button.Message, (message) => { 
                    button.Message = message;
                    base.ToggleOverlay(false);
                });
                ToggleOverlay(true, true);
            });

            DisplayLines = true;
            DisplayWindow.Cursor = null;
            DisplayWindow.Show();
        }

        public override ICommand ButtonClickCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ExportCommand { get; }
        public override ICommand CloseCommand { get; }

        EditButtonMessageViewModel _currentButton;
        public EditButtonMessageViewModel CurrentButton {
            get => _currentButton;
            set {
                _currentButton = value;
                OnPropertyChanged("CurrentButton");
            }
        }

        bool _editingButton = false;
        public bool EditingButton {
            get => _editingButton;
            set {
                _editingButton = value;
                OnPropertyChanged("EditingButton");
            }
        }

        protected override void ToggleOverlay(object x, bool button=false) {
            EditingButton = button;
            base.ToggleOverlay(x);
        }

        private bool HasConfigChanged() {
            var currentConfig = _configHandler.GetActiveConfig();

            if (Rows != currentConfig.RowCount || Columns != currentConfig.ColumnCount || currentConfig.Buttons.Count != Buttons.Count) return true;

            for (int i = 0; i < currentConfig.Buttons.Count; i++)
                if (currentConfig.Buttons[i] != Buttons[i].Message) return true;

            return false;
        }
    }
}
