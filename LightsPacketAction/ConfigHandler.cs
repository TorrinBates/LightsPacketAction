using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LightsPacketAction {
    public class ConfigHandler {
        const string C_ConfigPath = "config.xml";
        Config _config;

        public Tuple<int, int, List<string>, ConfigHandlerReturnCode> OpenConfig(string path) {
            XmlDocument doc = new XmlDocument();
            int numRows = 0;
            int numColumns = 0;
            List<string> buttons = new List<string>();
            ConfigHandlerReturnCode rc = new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.Success);

            try
            {
                doc.Load(path);

                numRows = int.Parse(doc.DocumentElement.SelectSingleNode("/Config/Rows").Attributes["count"]?.InnerText);
                numColumns = int.Parse(doc.DocumentElement.SelectSingleNode("/Config/Columns").Attributes["count"]?.InnerText);
                XmlNodeList m = doc.DocumentElement.SelectSingleNode("/Config/Buttons").ChildNodes;

                for (var i = 0; i < m.Count; i++)
                    buttons.Add(m.Item(i).Attributes["message"].InnerText);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                    rc = new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.FileNotFound);
                else if (e is IOException)
                    rc = new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.FileIOException, e.Message);
                else if (e is NullReferenceException || e is XmlException || e is ArgumentNullException)
                    rc = new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.InvalidConfiguration);
                else 
                    rc = new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.Unknown, e.Message);
            }

            return new Tuple<int, int, List<string>, ConfigHandlerReturnCode>(numRows, numColumns, buttons, rc);
        } 

        public ConfigHandlerReturnCode LoadConfig(string path=C_ConfigPath) {
            var configInfo = OpenConfig(path);
            SetActiveConfig(configInfo.Item1, configInfo.Item2, configInfo.Item3);
            return configInfo.Item4;
        }

        //Prevent accidental modification to config
        public Config GetActiveConfig() => _config == null ? null : new Config(_config);
        public void SetActiveConfig(int rows, int columns, List<string> buttons) => _config = new Config(rows, columns, buttons);

        public ConfigHandlerReturnCode SaveConfig(string path = C_ConfigPath, Config config=null) {
            var tmpConfig = config == null ? _config : config;
            try {
                XmlWriter xmlWriter = XmlWriter.Create(path);

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Config");

                var row1Elements = new[] { new[] { "Rows", tmpConfig.RowCount.ToString() }, new[] { "Columns", tmpConfig.ColumnCount.ToString() } };
                foreach (var element in row1Elements) {
                    xmlWriter.WriteStartElement(element[0]);
                    xmlWriter.WriteAttributeString("count", element[1]);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteStartElement("Buttons");
                foreach (var button in tmpConfig.Buttons) {
                    xmlWriter.WriteStartElement("Button");
                    xmlWriter.WriteAttributeString("message", button);
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
                return new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.Success);
            } catch(Exception e) {
                if (e is IOException) return new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.FileIOException, e.Message);
                return new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.Unknown, e.Message);
            }
        }

        public void CreateNewConfig() {
            var buttons = new List<string>();
            for (int i = 0; i < 72; i++)
                buttons.Add("Button"+(i + 1).ToString());

            SetActiveConfig(6, 12, buttons);
        }
    }

    public class Config {
        public Config(Config settings) {
            SetValues(settings.RowCount, settings.ColumnCount, settings.Buttons);
        }

        public Config(int rowCount, int columnCount, List<string> buttonList) {
            SetValues(rowCount, columnCount, buttonList);
        }

        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        //If we add placement in the future make this a list of custom button objects
        public List<string> Buttons { get; set; }

        private void SetValues(int row, int column, List<string> buttons) {
            RowCount = row;
            ColumnCount = column;
            Buttons = new List<string>(buttons);
        }
    }

    public class ConfigHandlerReturnCode {
        public ConfigHandlerReturnCode(ConfigHandlerReturnCodeType type, string reason = null) {
            ReturnCode = type;
            Reason = reason;
        }

        public ConfigHandlerReturnCodeType ReturnCode { get; }
        public string Reason { get; }
    }

    public enum ConfigHandlerReturnCodeType {
        Success,
        InvalidConfiguration,
        FileNotFound,
        FileIOException,
        Unknown
    }
}
