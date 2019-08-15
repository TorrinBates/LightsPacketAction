using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LightsPacketAction
{
    public class LauncherViewModel : ViewModelBase
    {
        private string _address = "";

        public List<string> ButtonsList { get; private set; } = new List<string>();
        public string ServerAddress
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged("ServerAddress"); }
        }

        private string _port = "";
        public string ServerPort
        {
            get { return _port; }
            set { _port = value; OnPropertyChanged("ServerPort"); }
        }

        private string _overlayImagePath = "";
        public string OverlayImagePath
        {
            get { return _overlayImagePath; }
            set { _overlayImagePath = value;  OnPropertyChanged("OverlayImagePath"); }
        }
        public ICommand BrowseOverlayImageCommand { get; private set; }
        public ICommand LaunchDisplayCommand { get; private set; }

        public LauncherViewModel()
        {
            for (int i = 1; i < 41; i++)
            {
                ButtonsList.Add("Button"+i.ToString()+"\r");
            }
            BrowseOverlayImageCommand = new RelayCommand((p) => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "Select Image to Overlay";
                openFileDialog.Filter = "JPEG Files: (*.JPG;*.JPEG;*.JPE;*.JFIF)|*.JPG;*.JPEG;*.JPE;*.JFIF";

                if (openFileDialog.ShowDialog() == true)
                    OverlayImagePath = openFileDialog.FileName;
            });

            LaunchDisplayCommand = new RelayCommand(
                (p) => OverlayImagePath != "" && ServerPort != "" && ServerAddress != "",
                (p) => {
                    try
                    {
                        var window = new Window();
                        window.Owner = Application.Current.MainWindow;
                        window.ResizeMode = ResizeMode.NoResize;
                        window.WindowState = WindowState.Maximized;
                        window.WindowStyle = WindowStyle.None;
                        window.Content = new DisplayViewModel(ButtonsList, ServerAddress, Convert.ToInt32(ServerPort));

                        window.InputBindings.Add(new KeyBinding(new RelayCommand((param) => window.Close()), Key.Escape,
                            ModifierKeys.None));

                        window.Background = new ImageBrush(new BitmapImage(new Uri(OverlayImagePath)));

                        window.ShowDialog();
                    }
                    catch (UriFormatException e)
                    {

                    }
                }
            );

        }
    }
}
