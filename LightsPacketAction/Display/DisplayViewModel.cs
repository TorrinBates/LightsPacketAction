using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LightsPacketAction
{
    public abstract class DisplayViewModel : ViewModelBase {
        Config _config;
        public DisplayViewModel(Config activeConfig) {
            _config = activeConfig;

            Buttons = new ObservableCollection<ButtonViewModel>(_config.Buttons.Select(x => new ButtonViewModel(x)));

            Rows = _config.RowCount;
            Columns = _config.ColumnCount;

            DisplayWindow = new Window() {
                Owner = Application.Current.MainWindow,
                ResizeMode = ResizeMode.NoResize,
                WindowState = WindowState.Maximized,
                WindowStyle = WindowStyle.None,
                Background = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                Content = this
            };

            ToggleOverlayCommand = new RelayCommand(x => IsOverlayEnabled = x is bool ? (bool)x : !IsOverlayEnabled);
            CloseCommand = new RelayCommand(x => DisplayWindow.Close());

            DisplayWindow.InputBindings.Add(new KeyBinding(ToggleOverlayCommand, Key.O, ModifierKeys.Control));
            DisplayWindow.InputBindings.Add(new KeyBinding(new RelayCommand(x => {
                if (IsOverlayEnabled)
                    ToggleOverlayCommand.Execute(false);
                else CloseCommand.Execute(null); 
            }), Key.Escape, ModifierKeys.None));

            //EditDisplay:
            //Change message on double click
        }

        public virtual ICommand ButtonClickCommand { get; }
        public ICommand ToggleOverlayCommand { get; }
        public ICommand ExitOverlayCommand { get; }
        public virtual ICommand CloseCommand { get; }

        int _rows;
        public int Rows { 
            get => _rows;
            set {
                _rows = value;
                AdjustButtonList();
                OnPropertyChanged("Rows");
            } 
        }

        int _columns;
        public int Columns {
            get => _columns;
            set {
                _columns = value;
                AdjustButtonList();
                OnPropertyChanged("Columns");
            }
        }


        public ObservableCollection<ButtonViewModel> Buttons { get; }

        bool _isOverlayEnabled;
        public virtual bool IsOverlayEnabled { 
            get => _isOverlayEnabled; 
            protected set { 
                _isOverlayEnabled = value; 
                OnPropertyChanged("IsOverlayEnabled"); 
            } 
        }

        bool _displayLines = false;
        public bool DisplayLines { 
            get => _displayLines;
            protected set {
                _displayLines = value;
                OnPropertyChanged("DisplayLines");
            } 
        }

        protected Window DisplayWindow { get; }

        private void AdjustButtonList() {
            var total = Rows * Columns;
            var count = Buttons.Count;
            if (total <= 0) Buttons.Clear();
            else if (total > count) for (var i = 0; i < total - count; i++) Buttons.Add(new ButtonViewModel("Button" + (i + count + 1)));
            else if (total < count) for (int i = count-1; i > total - 1; i--) Buttons.RemoveAt(i);
        }
    }
}
