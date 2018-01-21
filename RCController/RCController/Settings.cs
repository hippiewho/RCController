using System;
using System.Xml;
using System.Reflection;
using System.IO;
using BluetoothConnector;

namespace RCController
{
    static class Settings
    {
        public static int ScanOutTime
        { get
            {
                return 10000;
            }
        }

        public static String CharacteristicName
        {
            get
            {
                return "0000ffe1-0000-1000-8000-00805f9b34fb";
            }
        }
                    
        public static String BluetoothDeviceName
        {
            get
            {
                return GetDeviceName();
            }
        }

        public static Guid ServiceGuid
        {
            get
            {
                return new Guid("0000ffe0-0000-1000-8000-00805f9b34fb");
            }
        }

        public static Guid CharacteristicGuid
        {
            get
            {
                return new Guid("0000ffe1-0000-1000-8000-00805f9b34fb");
            }
        }

        private static String GetDeviceName()
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
