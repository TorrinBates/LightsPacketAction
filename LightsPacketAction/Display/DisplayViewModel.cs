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
