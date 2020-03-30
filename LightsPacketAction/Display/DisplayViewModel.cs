using System;
using System.Collections.Generic;
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

            Buttons = new ObservableCollection<string>(_config.Buttons);

            var closeCommand = new RelayCommand((param) => DisplayWindow.Close());
            DisplayWindow = new Window() {
                Owner = Application.Current.MainWindow,
                ResizeMode = ResizeMode.NoResize,
                WindowState = WindowState.Maximized,
                WindowStyle = WindowStyle.None,
                Cursor = Cursors.None,
                Background = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                Content = this
            };

            //EditDisplay:
            //Change message on double click
            //Overlay
            //Cancel/save config changes
        }
        public ICommand ButtonClickCommand { get; protected set; }

        public int Rows { 
            get { return _config.RowCount; }
            set {
                _config.RowCount = value;
                AdjustButtonList();
                OnPropertyChanged("Rows");
                OnPropertyChanged("Buttons");
            } 
        }

        public int Columns {
            get { return _config.ColumnCount; }
            set {
                _config.ColumnCount = value;
                AdjustButtonList();
                OnPropertyChanged("Columns");
                OnPropertyChanged("Buttons");
            }
        }


        public ObservableCollection<string> Buttons { get; }
        public virtual bool DisplayLines { get; protected set; } = true;

        protected Window DisplayWindow { get; }

        private void AdjustButtonList() {
            var total = Rows * Columns;
            if (total <= 0) Buttons.Clear();
            else if (total > Buttons.Count) {
                var count = Buttons.Count;
                for (var i = 0; i < total - count; i++)
                    Buttons.Add("Button" + (i + count + 1));
            } else {
                Buttons.Clear();
                for (int i = 0; i < total; i++) Buttons.Add(_config.Buttons[i]);
            }
            _config.Buttons = Buttons.ToList();
        }
    }
    /*public class DisplayViewModel : ViewModelBase
    {
        public DisplayViewModel(Config activeConfig, string server, int port, RelayCommand close)
        {
            CloseDisplay = close;
            Buttons = activeConfig.Buttons;
            Rows = activeConfig.RowCount;
            Columns = activeConfig.ColumnCount;
            SendMessage = new RelayCommand(async p =>
            {

                int t = await Task<int>.Factory.StartNew(() =>
                {
                    try
                    {
                        TcpClient client = new TcpClient();

                        if (!client.ConnectAsync(server, port).Wait(2000))
                        {
                            return 2;
                        }
                        Byte[] data = Encoding.ASCII.GetBytes((string)p);

                        NetworkStream stream = client.GetStream();
                        stream.Write(data, 0, data.Length);

                        stream.Close();
                        client.Close();
                        return 0;
                    }
                    catch (IOException)
                    {
                        return 1;
                    }
                    catch (SocketException)
                    {
                        return 2;
                    }
                });
                if (bbb == null && t==1)
                {
                    CreateErrorDialog("Connection lost, message not sent.");
                }
                if (bbb == null && t==2)
                {
                    CreateErrorDialog("Unable to connect to host.");
                }
            });
        }

        public void CreateErrorDialog(string errorMessage)
        {
            CustomWindow window = null;
            window = new CustomWindow(new ErrorViewModel(errorMessage,
                    () =>
                    {
                        window.Close();
                        bbb = null;
                    }), "Error");
            window.MinimizeVisibility = Visibility.Collapsed;
            window.XVisibility = Visibility.Collapsed;
            window.Height = 100;
            window.Width = 250;
            bbb = window;
            window.ShowDialog();
        }

        private bool _displayLines = false;
        public bool DisplayLines
        {
            get { return _displayLines;}
            set
            {
                _displayLines = value;
                OnPropertyChanged("DisplayLines");
            }
        }

        private CustomWindow bbb = null;
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public ICommand CloseDisplay { get; private set; }
        public ICommand SendMessage { get; private set; }
        public List<string> Buttons { get; private set; }
    }*/
}
