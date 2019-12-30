using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LightsPacketAction
{
    class DisplayViewModel : ViewModelBase
    {
        public DisplayViewModel(List<string> buttons, string server, Int32 port, RelayCommand close)
        {
            CloseDisplay = close;
            Buttons = buttons;
            SendMessage = new RelayCommand(async p =>
            {
                try
                {
                    await Task.Run(() =>
                    {
                        TcpClient client = new TcpClient(server, port);

                        Byte[] data = Encoding.ASCII.GetBytes((string) p);

                        NetworkStream stream = client.GetStream();
                        stream.Write(data, 0, data.Length);

                        stream.Close();
                        client.Close();
                    });
                }
                catch (IOException)
                {
                    if (bbb == null)
                    {
                        CreateErrorDialog("Connection lost, message not sent.");
                    }
                }
                catch (SocketException)
                {
                    if (bbb == null)
                    {
                        CreateErrorDialog("Unable to connect to host.");
                    }
                }
            });
        }

        public void CreateErrorDialog(string errorMessage)
        {
            CustomWindow window = null;
            window = new CustomWindow(
                new ErrorViewModel(errorMessage,
                    new RelayCommand((param) =>
                    {
                        window.Close();
                        bbb = null;
                    })), "Error");
            window.MinimizeVisibility = Visibility.Collapsed;
            window.XVisibility = Visibility.Collapsed;
            window.Owner = Application.Current.MainWindow;
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
        public ICommand CloseDisplay { get; private set; }
        public ICommand SendMessage { get; private set; }
        public List<string> Buttons { get; private set; }
    }
}
