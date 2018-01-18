using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Extensions;
using RCController;
using System.Collections;
using System.Collections.Generic;

namespace BluetoothConnector
{

    sealed class BluetoothConnection
    {
        private static BluetoothConnection instance = null;
        private static readonly object instanceLocker = new object();

        private IBluetoothLE ble;
        private IAdapter adapter;
        private bool isSearching = false;
        private ArrayList DeviceList = new ArrayList();
        private IList<ICharacteristic> _characteristics;
        private IDevice ConnectedDevice = null;

        public static BluetoothConnection Instance
        {
            get
            {
                lock (instanceLocker)
                {
                    if (instance == null)
                    {
                        instance = new BluetoothConnection();
                    }
                    return instance;
                }
            }
        }

        public BluetoothConnection()
        {
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;

            ble.StateChanged += (s, e) =>
            {
                Debug.WriteLine($"State Changed To {e.NewState}");
            };

            adapter.DeviceDiscovered += (s, a) =>
            {
                DeviceList.Add(a);
                Debug.WriteLine($"Device Discovered: {a.Device.Name}, ID: {a.Device.Id}, State: {a.Device.State}");
                
            };

            adapter.ScanTimeout = Settings.ScanOutTime;
        }

        public async Task StartSearchingForDevicesAsync()
        {
            if (!isSearching)
            {
                try
                {
                    isSearching = true;
                    await adapter.StartScanningForDevicesAsync();
                    //await adapter.StartScanningForDevicesAsync(dev => dev.Name.Contains(Settings.GetDeviceName()));

                    ConnectedDevice = await GetSpecifiedDevice();

                    await adapter.ConnectToDeviceAsync(ConnectedDevice);
                    Console.WriteLine(ConnectedDevice.Id);
                }
                catch (Exception e)
                {
                    Console.WriteLine("----- Error Has Occurred -----");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("------------------------------");
                    StopSearchingForDevices();
                }
            }
        }

        private async Task<IDevice> GetSpecifiedDevice()
        {
            try
            {
                var suspectedDevice =  await adapter.DiscoverDeviceAsync(dev => dev.Name.Equals(Settings.GetDeviceName()));
                return suspectedDevice;
            } catch(NullReferenceException e)
            {
                return null;
            }
        }

        private async Task ConnectToDevice(IDevice device)
        {
            await adapter.ConnectToDeviceAsync(device);
        }

        public async void StopSearchingForDevices()
        {
            if (isSearching)
            {
                await adapter.StopScanningForDevicesAsync();
                isSearching = false;
            }
        }

        public void ShowArrayList(int length = 5)
        {
            foreach(Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs curr in DeviceList)
            {
                Debug.WriteLine(curr.Device.Name);
            }
        }

        public async void PushData(int data = 0)
        {
            if (ConnectedDevice != null) {
                var services = await ConnectedDevice.GetServicesAsync();
                Debug.WriteLine("Print Services");
                foreach(IService s in services)
                {
                    Debug.WriteLine("Service Name: " + s.Name + " - isPrimary: " + s.IsPrimary + " - With ID: " + s.Id);
                    var characteristics = await s.GetCharacteristicsAsync();
                    char charCount = 'H';
                    foreach (ICharacteristic c in characteristics)
                    {
                        Debug.WriteLine("\tCharacteristic Name: " + c.Name + " - ID: " + c.Id + " - Properties: " + c.Properties + " - UUID: " + c.Uuid + "\n\t\t\tCan R W U: " + c.CanRead + c.CanWrite + c.CanUpdate);
                        if (c.CanWrite && "0000ffe1-0000-1000-8000-00805f9b34fb".Equals(c.Uuid))
                        {
                            byte[] dataa = {0x48};
                            await c.WriteAsync(dataa);
                            Debug.WriteLine("----------------------------------\n---------------------------SENT " + charCount);
                            c.ValueUpdated += (aas, e) =>
                            {
                                Debug.WriteLine("New value: {0}", e.Characteristic.Value);
                            };
                        }
                    }
                }
                //var characteristics = await services.GetCharacteristicsAsync();

            }


        }
    }
}