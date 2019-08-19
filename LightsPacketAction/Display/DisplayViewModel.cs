﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
                await Task.Run(() =>
                {
                    try
                    {
                        TcpClient client = new TcpClient(server, port);

                        Byte[] data = Encoding.ASCII.GetBytes((string)p);

                        NetworkStream stream = client.GetStream();
                        stream.Write(data, 0, data.Length);

                        stream.Close();
                        client.Close();
                    }
                    catch (SocketException e)
                    {

                    }
                });
            });
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
        public ICommand CloseDisplay { get; private set; }
        public ICommand SendMessage { get; private set; }
        public List<string> Buttons { get; private set; }
    }
}
