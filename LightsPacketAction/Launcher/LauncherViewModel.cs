using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace LightsPacketAction
{
    public class LauncherViewModel
    {
        public string ServerAddress { get; set; }
        public string ServerPort { get; set; }
        public string OverlayImagePath { get; set; }

        public ICommand BrowseOverlayImageCommand { get; private set; }
        public ICommand LaunchDisplayCommand { get; private set; }

        public LauncherViewModel()
        {
            BrowseOverlayImageCommand = new RelayCommand((p) => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "Select Image to Overlay";
                openFileDialog.Filter = "*.jpg | *.jpeg";

                if (openFileDialog.ShowDialog() == true)
                    OverlayImagePath = openFileDialog.FileName;
            });

            LaunchDisplayCommand = new RelayCommand((p) => {
                Console.WriteLine("Not Implemented");
            });
        }
    }
}
