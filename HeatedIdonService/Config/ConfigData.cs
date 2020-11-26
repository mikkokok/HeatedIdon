using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace HeatedIdonService.Config
{
    public class ConfigData
    {
        [XmlElement]
        public Messaging Messaging;
        [XmlElement]
        public RestlessFalcon RestlessFalcon;
    }

    [XmlRoot("ConfigData")]
    public class Messaging
    {
        [XmlElement]
        public string mqttServer;
        [XmlElement]
        public string mqttUser;
        [XmlElement]
        public string mqttPassword;
        [XmlElement]
        public string mqttTopic;
    }
    [XmlRoot("ConfigData")]
    public class RestlessFalcon
    {
        [XmlElement]
        public string url;
        [XmlElement]
        public string key;
        [XmlElement]
        public string sslThumbprint;
    }
}
