using System;
using System.Xml;
using System.Reflection;
using System.IO;
using BluetoothConnector;

namespace RCController
{
    static class Settings
    {
        public static String GetDeviceName()
        {
#if __IOS__
            String XMLPrefix = "RCController.iOS";
#endif
#if __ANDROID__
                String XMLPrefix = "RCController.Android";
#endif

            //var assembly = typeof(BluetoothConnection).GetTypeInfo().Assembly;
            //Stream stream = assembly.GetManifestResourceStream(XMLPrefix + "GlobalSettings.xml");
            //XmlDocument settings = new XmlDocument();
            //settings.LoadXml(stream.ToString());
            //XmlNode node = settings.SelectSingleNode("Devices/Device");

            //return node.Attributes["Name"]?.InnerText;
            return "SH-HC-08";
        }
    }
}
