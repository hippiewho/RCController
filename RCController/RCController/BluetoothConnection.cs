using System;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Extensions;
using RCController;

namespace BluetoothConnector
{

    sealed class BluetoothConnection
    {
        private static BluetoothConnection instance = null;
        private static readonly object instanceLocker = new object();

        IBluetoothLE ble;
        IAdapter adapter;
        bool isSearching = false;

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
        }

        public async Task StartSearchingForDevicesAsync()
        {
            if (!isSearching)
            {

                isSearching = true;
                await adapter.StartScanningForDevicesAsync();
                var device = await adapter.DiscoverDeviceAsync(dev => dev.Name.Equals(Settings.GetDeviceName()));
                if (device != null)
                {
                    await ConnectToDevice((IDevice)device);
                }
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
    }
}