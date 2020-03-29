using Microsoft.Win32;
using System;
using System.Drawing.Imaging;
using System.Windows;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System.Xml;

namespace LightsPacketAction
{
    public class LauncherViewModel : ViewModelBase
    {
        ConfigHandler _configHandler;

        public LauncherViewModel(ConfigHandler configHandler)
        {
            _configHandler = configHandler;

            BrowseOverlayImageCommand = new RelayCommand((p) => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "";

                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                string format = "{0}{1}{2} ({3})|{3}";
                openFileDialog.Filter = string.Format(format, openFileDialog.Filter, string.Empty, "All Files", "*.*");

                foreach (var c in codecs) {
                    string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                    openFileDialog.Filter = string.Format(format, openFileDialog.Filter, "|", codecName, c.FilenameExtension);
                }

                if (openFileDialog.ShowDialog() == true) OverlayImagePath = openFileDialog.FileName;
            });

            ConfigureCommand = new RelayCommand((p) => {
                CustomWindow window = null;
                window = new CustomWindow(new ConfigureViewModel(), "Configure");
                window.Height = 500;
                window.Width = 400;
                window.ShowDialog();
            });

            LaunchDisplayCommand = new RelayCommand(
                (p) => !string.IsNullOrWhiteSpace(OverlayImagePath) && 
                       !string.IsNullOrWhiteSpace(ServerPort) && 
                       !string.IsNullOrWhiteSpace(ServerAddress),
                (p) => {
                    if (!IsValidIP(ServerAddress)) {
                        DialogFactory.CreateErrorDialog("The IP address is invalid.");
                        return;
                    }

                    if(!int.TryParse(ServerPort, out int port)) {
                        DialogFactory.CreateErrorDialog("Port must only contain numbers.");
                        return;
                    }

                    if(!Uri.TryCreate(OverlayImagePath, UriKind.RelativeOrAbsolute, out Uri imageUri)) {
                        DialogFactory.CreateErrorDialog("The image path is incorrect.");
                        return;
                    }

                    ImageBrush brush;
                    try {
                        brush = new ImageBrush(new BitmapImage(imageUri));
                    } catch(NotSupportedException) {
                        DialogFactory.CreateErrorDialog("Image file type not supported.");
                        return;
                    }
                    
                    Window window = null;
                    var closeCommand = new RelayCommand((param) => window.Close());
                    window = new Window() {
                        Owner = Application.Current.MainWindow,
                        ResizeMode = ResizeMode.NoResize,
                        WindowState = WindowState.Maximized,
                        WindowStyle = WindowStyle.None,
                        Cursor = Cursors.None,
                        Background = brush,
                        Content = new DisplayViewModel(_configHandler.ActiveConfig, ServerAddress, port, closeCommand)
                    };

                    window.InputBindings.Add(new KeyBinding(
                        new RelayCommand((param) => ((DisplayViewModel)window.Content).DisplayLines = !((DisplayViewModel)window.Content).DisplayLines), 
                        Key.M, ModifierKeys.Control)
                    );

                    window.InputBindings.Add(new KeyBinding(closeCommand, Key.Escape, ModifierKeys.None));

                    window.ShowDialog();
                });
        }

        public ICommand BrowseOverlayImageCommand { get; }
        public ICommand LaunchDisplayCommand { get; }
        public ICommand ConfigureCommand { get; }

        string _address = "";
        public string ServerAddress {
            get { return _address; }
            set { _address = value; OnPropertyChanged("ServerAddress"); }
        }

        string _port = "";
        public string ServerPort {
            get { return _port; }
            set { _port = value; OnPropertyChanged("ServerPort"); }
        }

        string _overlayImagePath = "";
        public string OverlayImagePath {
            get { return _overlayImagePath; }
            set { _overlayImagePath = value; OnPropertyChanged("OverlayImagePath"); }
        }

        private bool IsValidIP(string Address) {
            //Match pattern for IP address    
            string Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$"; 
            Regex check = new Regex(Pattern);
 
            return check.IsMatch(Address, 0);
        }
    }
}
