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
        private bool isConnected = false;
        private ArrayList DeviceList = new ArrayList();
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

        public bool IsSearching { get => isSearching; set => isSearching = value; }
        public bool IsConnected { get => isConnected; set => isConnected = value; }

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

        public async Task StartSearchingForArduinoToConnectAsync()
        {
            if (!IsSearching)
            {
                try
                {
                    IsSearching = true;
                    await adapter.StartScanningForDevicesAsync();
                    ConnectedDevice = await GetSpecifiedDevice();
                    if (ConnectedDevice != null)
                    {
                        IsConnected = true;
                        StopSearchingForDevices();
                        await adapter.ConnectToDeviceAsync(ConnectedDevice);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("----- Error Has Occurred -----");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("------------------------------");
                    StopSearchingForDevices();
                }
            }
            IsSearching = false;
        }

        private async Task<IDevice> GetSpecifiedDevice()
        {
            try
            {
                var suspectedDevice =  await adapter.DiscoverDeviceAsync(dev => dev.Name.Equals(Settings.BluetoothDeviceName));
                return suspectedDevice;
            } catch(NullReferenceException e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        private async Task ConnectToDevice(IDevice device)
        {
            await adapter.ConnectToDeviceAsync(device);
            
        }

        public async void StopSearchingForDevices()
        {
            if (IsSearching)
            {
                await adapter.StopScanningForDevicesAsync();
                IsSearching = false;
            }
        }

        public void ShowArrayList(int length = 5)
        {
            foreach(Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs curr in DeviceList)
            {
                Debug.WriteLine(curr.Device.Name);
            }
        }

        public async void ListServicesAndCharacteristics()
        {
            if (ConnectedDevice != null) {
                var services = await ConnectedDevice.GetServicesAsync();
                Debug.WriteLine("Print Services and Characteristics:");
                foreach(IService s in services)
                {
                    Debug.WriteLine("Service Name: " + s.Name + " - isPrimary: " + s.IsPrimary + " - With ID: " + s.Id);
                    var characteristics = await s.GetCharacteristicsAsync();
                    foreach (ICharacteristic c in characteristics)
                    {
                        Debug.WriteLine("\tCharacteristic Name: " + c.Name + " - ID: " + c.Id + " - Properties: " + c.Properties + " - UUID: " + c.Uuid + "\n\t\t\tCan R W U: " + c.CanRead + c.CanWrite + c.CanUpdate);
                    }
                }
            }
        }

        public async void PushData(int direction = 0)
        {
            if (ConnectedDevice != null && IsConnected)
            {
                var service = await ConnectedDevice.GetServiceAsync(Settings.ServiceGuid);
                var characteristic = await service.GetCharacteristicAsync(Settings.CharacteristicGuid);

                if (characteristic.CanWrite)
                {
                    byte[] payload = { GetByteFormat(direction) };
                    await characteristic.WriteAsync(payload);
                    characteristic.ValueUpdated += (aas, e) =>
                    {
                        Debug.WriteLine($"New value: {e.Characteristic.Value}");
                    };
                }
            }
        }

        private byte GetByteFormat(int direction = 0)
        {
            switch (direction)
            {
                case 1:
                    return 0x1;
                case 2:
                    return 0x2;
                case 3:
                    return 0x3;
                case 4:
                    return 0x4;
                default:
                    return 0x0;

            }
        }
        public void DisconnectDevice()
        {
            if (ConnectedDevice != null && IsConnected)
            {
                adapter.DisconnectDeviceAsync(ConnectedDevice);
                IsConnected = false;
            }
        }
    }

}