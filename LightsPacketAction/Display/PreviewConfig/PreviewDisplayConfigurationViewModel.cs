﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace LightsPacketAction {
    public class PreviewDisplayConfigurationViewModel : DisplayViewModel {
        bool _isExceptionBeingAcknowledged = false;
        public PreviewDisplayConfigurationViewModel(Config activeConfig, string address, int port, ImageBrush backgroundBrush) : base(activeConfig) {
            DisplayWindow.Background = backgroundBrush;
            DisplayWindow.Cursor = Cursors.None;

            DisplayWindow.InputBindings.Add(new KeyBinding(new RelayCommand((param) => DisplayLines = !DisplayLines), Key.M, ModifierKeys.Control));

            ToggleButtonMapCommand = new RelayCommand(x => DisplayLines = !DisplayLines);

            ButtonClickCommand = new RelayCommand(async p => {
                int t = await Task<int>.Factory.StartNew(() => {
                    try {
                        using (TcpClient client = new TcpClient()) {
                            if (!client.ConnectAsync(address, port).Wait(2000)) return 2;

                            byte[] data = Encoding.ASCII.GetBytes(((ButtonViewModel)p).Message+"\r");

                            using (NetworkStream stream = client.GetStream()) {
                                stream.Write(data, 0, data.Length);
                            }
                            return 0;
                        }
                    } catch (AggregateException e) {
                        int exceptionCode = -1;
                        e.Handle(ex => {
                            if (exceptionCode != -1) return true;
                            if (ex is IOException) exceptionCode = 1;
                            else if (ex is SocketException) exceptionCode = 2;
                            else return false;
                            return true;
                        });
                        return exceptionCode;
                    }
                });

                if (!_isExceptionBeingAcknowledged && t == 1) {
                    _isExceptionBeingAcknowledged = true;
                    DialogFactory.CreateErrorDialog(Constants.C_ConnectionError, "Connection lost, message not sent.", DisplayWindow);
                    _isExceptionBeingAcknowledged = false;
                } else if (!_isExceptionBeingAcknowledged && t == 2) {
                    _isExceptionBeingAcknowledged = true;
                    DialogFactory.CreateErrorDialog(Constants.C_ConnectionError, "Unable to connect to host.", DisplayWindow);
                    _isExceptionBeingAcknowledged = false;
                }
            });

            DisplayWindow.ShowDialog();
        }

        public ICommand ToggleButtonMapCommand { get; }
        public override ICommand ButtonClickCommand { get; }

        public override bool IsOverlayEnabled { 
            get => base.IsOverlayEnabled;
            protected set {
                base.IsOverlayEnabled = value;
                DisplayWindow.Cursor = value ? null : Cursors.None;
            }
        }
    }
}
