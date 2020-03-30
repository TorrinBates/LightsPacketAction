using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LightsPacketAction {
    public class ConfigHandler {
        const string C_ConfigPath = "config.xml";
        Config _config;

        public ConfigHandlerReturnCode LoadConfig(string path = C_ConfigPath) {
            XmlDocument doc = new XmlDocument();
            try {
                doc.Load(path);

                int numRows = int.Parse(doc.DocumentElement.SelectSingleNode("/Config/Rows").Attributes["count"]?.InnerText);
                int numColumns = int.Parse(doc.DocumentElement.SelectSingleNode("/Config/Columns").Attributes["count"]?.InnerText);
                XmlNodeList m = doc.DocumentElement.SelectSingleNode("/Config/Buttons").ChildNodes;

                List<string> buttons = new List<string>();
                for (var i = 0; i < m.Count; i++)
                    buttons.Add(m.Item(i).Attributes["message"].InnerText + "\r");

                SetActiveConfig(new Config(numRows, numColumns, buttons));
            } catch (Exception e) {
                if (e is FileNotFoundException)
                    return new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.FileNotFound);
                else if (e is IOException) 
                    return new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.FileIOException, e.Message);
                else if (e is NullReferenceException || e is XmlException || e is ArgumentNullException) 
                    return new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.InvalidConfiguration);
                return new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.Unknown, e.Message);
            }
            
            return new ConfigHandlerReturnCode(ConfigHandlerReturnCodeType.Success);
        }

        //Prevent accidental modification to config
        public Config GetActiveConfig() => _config == null ? null : new Config(_config);
        public void SetActiveConfig(Config config) => _config = config;

        public ConfigHandlerReturnCode SaveConfig(string path = C_ConfigPath) {
            try {
                XmlWriter xmlWriter = XmlWriter.Create(C_ConfigPath);

                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Config");

                var config = GetActiveConfig();
                var row1Elements = new[] { new[] { "Rows", config.RowCount.ToString() }, new[] { "Columns", config.ColumnCount.ToString() } };
                foreach (var element in row1Elements) {
                    xmlWriter.WriteStartElement(element[0]);
                    xmlWriter.WriteAttributeString("count", element[1]);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteStartElement("Buttons");
                foreach (var button in config.Buttons) {
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
                buttons.Add("Button" + (i + 1).ToString());

            SetActiveConfig(new Config(6, 12, buttons));
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
