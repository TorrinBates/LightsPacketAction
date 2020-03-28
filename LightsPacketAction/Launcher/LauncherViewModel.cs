using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Windows;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace LightsPacketAction
{
    public class LauncherViewModel : ViewModelBase
    {
        public List<string> ButtonsList { get; private set; } = new List<string>();

        string _address = "";
        public string ServerAddress
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged("ServerAddress"); }
        }

        string _port = "";
        public string ServerPort
        {
            get { return _port; }
            set { _port = value; OnPropertyChanged("ServerPort"); }
        }

        string _overlayImagePath = "";
        public string OverlayImagePath
        {
            get { return _overlayImagePath; }
            set { _overlayImagePath = value;  OnPropertyChanged("OverlayImagePath"); }
        }

        public ICommand BrowseOverlayImageCommand { get; }
        public ICommand LaunchDisplayCommand { get; }
        public ICommand ConfigureCommand { get; }

        public LauncherViewModel()
        {
            BuildConfig();

            BrowseOverlayImageCommand = new RelayCommand((p) => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "";

                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                string sep = string.Empty;

                openFileDialog.Filter = String.Format("{0}{1}{2} ({3})|{3}", openFileDialog.Filter, sep, "All Files", "*.*");

                foreach (var c in codecs)
                {
                    sep = "|";
                    string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                    openFileDialog.Filter = String.Format("{0}{1}{2} ({3})|{3}", openFileDialog.Filter, sep, codecName, c.FilenameExtension);
                }

                if (openFileDialog.ShowDialog() == true)
                    OverlayImagePath = openFileDialog.FileName;
            });

            ConfigureCommand = new RelayCommand((p) => {
                CustomWindow window = null;
                window = new CustomWindow(new ConfigureViewModel(), "Configure");
                window.Owner = Application.Current.MainWindow;
                window.Height = 500;
                window.Width = 400;
                window.ShowDialog();

            });

            LaunchDisplayCommand = new RelayCommand(
                (p) => OverlayImagePath != "" && ServerPort != "" && ServerAddress != "",
                (p) => {
                    try
                    {
                        if (IsValidIP(ServerAddress)) {
                            XmlDocument doc = new XmlDocument();
                            doc.Load("config.xml");
                            XmlNode r = doc.DocumentElement.SelectSingleNode("/Config/Rows");
                            int numRows = int.Parse(r.Attributes["count"]?.InnerText);
                            XmlNode c = doc.DocumentElement.SelectSingleNode("/Config/Columns");
                            int numColumns = int.Parse(c.Attributes["count"]?.InnerText);
                            XmlNode m = doc.DocumentElement.SelectSingleNode("/Config/Buttons");
                            for(var i = 0; i < m.ChildNodes.Count; i++)
                                ButtonsList.Add(m.ChildNodes.Item(i).Attributes["message"].InnerText + "\r");

                            var window = new Window();
                            window.Owner = Application.Current.MainWindow;
                            window.ResizeMode = ResizeMode.NoResize;
                            window.WindowState = WindowState.Maximized;
                            window.WindowStyle = WindowStyle.None;
                            window.Cursor = Cursors.None;
                            window.Content = new DisplayViewModel(ButtonsList, numRows, numColumns, ServerAddress,
                                Convert.ToInt32(ServerPort),
                                new RelayCommand((param) => window.Close()));

                            window.InputBindings.Add(new KeyBinding(
                                new RelayCommand((param) =>
                                    ((DisplayViewModel)window.Content).DisplayLines =
                                    !((DisplayViewModel)window.Content).DisplayLines), Key.M,
                                ModifierKeys.Control));
                            window.InputBindings.Add(new KeyBinding(new RelayCommand((param) => window.Close()),
                                Key.Escape,
                                ModifierKeys.None));

                            window.Background = new ImageBrush(new BitmapImage(new Uri(OverlayImagePath)));

                            window.ShowDialog();
                        }
                        else
                        {
                            CreateErrorDialog("The IP address is invalid.");
                        }
                    }
                    catch (XmlException)
                    {
                        CreateErrorDialog("Invalid Configuration File.");
                    }
                    catch (NullReferenceException)
                    {
                        CreateErrorDialog("Invalid Configuration File.");
                    }
                    catch (ArgumentNullException)
                    {
                        CreateErrorDialog("Invalid Configuration File.");
                    }
                    catch (FileNotFoundException)
                    {
                        CreateErrorDialog("Invalid Configuration File.");
                    }
                    catch (UriFormatException)
                    {
                        CreateErrorDialog("The image path is incorrect.");
                    }
                    catch (FormatException)
                    {
                        CreateErrorDialog("Port must only contain numbers.");
                    }
                    catch (NotSupportedException)
                    {
                        CreateErrorDialog("Image file type not supported.");
                    }
                });
        }

        private bool IsValidIP(string Address)
        {
            //Match pattern for IP address    
            string Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$"; 
            Regex check = new Regex(Pattern);
 
            return check.IsMatch(Address, 0);
        }

        private void CreateErrorDialog(string errorMessage)
        {
            CustomWindow window = null;
            window = new CustomWindow(
                new ErrorViewModel(errorMessage,
                    new RelayCommand((param) => window.Close())), "Error");
            window.MinimizeVisibility = Visibility.Collapsed;
            window.XVisibility = Visibility.Collapsed;
            window.Owner = Application.Current.MainWindow;
            window.Height = 100;
            window.Width = 250;
            window.ShowDialog();
        }

        public void BuildConfig()
        {
            if (!File.Exists("config.xml"))
            {
                List<string> buttonList = new List<string>();
                for (int i = 1; i < 73; i++)
                {
                    buttonList.Add("Button" + i.ToString());
                }

                XmlWriter xmlWriter = XmlWriter.Create("config.xml");

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Config");

                xmlWriter.WriteStartElement("Buttons");
                xmlWriter.WriteAttributeString("count", "72");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Rows");
                xmlWriter.WriteAttributeString("count", "6");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Columns");
                xmlWriter.WriteAttributeString("count", "12");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Buttons");

                foreach(var button in buttonList) {
                    xmlWriter.WriteStartElement("Button");
                    xmlWriter.WriteAttributeString("message", button);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }
        }
    }
}
