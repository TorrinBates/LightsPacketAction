using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;

namespace LightsPacketAction
{
    public class LauncherViewModel : ViewModelBase
    {
        private string _address = "";
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
        public ICommand IndexScreenCommand { get; private set; }

        public LauncherViewModel()
        {
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
                    Console.WriteLine("Not Implemented");
                }
            );

            IndexScreenCommand = new RelayCommand((p) => {
                CustomWindow window = new CustomWindow(null);
                window.MinimizeVisibility = Visibility.Collapsed;
                window.Title = "Index";
                window.Owner = Application.Current.MainWindow;

                window.ShowDialog();
            });
        }
    }
}
