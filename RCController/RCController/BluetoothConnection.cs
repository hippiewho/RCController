using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Extensions;
using RCController;
using System.Collections;

namespace BluetoothConnector
{

    sealed class BluetoothConnection
    {
        private static BluetoothConnection instance = null;
        private static readonly object instanceLocker = new object();

        IBluetoothLE ble;
        IAdapter adapter;
        bool isSearching = false;
        private ArrayList DeviceList = new ArrayList();

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

            adapter.ScanTimeout = 10000;
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

                    var device = await GetSpecifiedDevice();

                    await adapter.ConnectToDeviceAsync(device);
                    Console.WriteLine(device.Id);
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
    }
}