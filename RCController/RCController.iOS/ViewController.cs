using System;

using UIKit;
using BluetoothConnector;

namespace RCController.iOS
{
    public partial class ViewController : UIViewController
    {
        private BluetoothConnection bluetoothConnection = new BluetoothConnection();
        
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupButtons();

            
        }

        private void SetupButtons()
        {
            ConnectBluetoothButton.Tag = 1;
            DisconnectButton.Tag = 2;

            MoveUpButton.Tag = 1;
            MoveRightButton.Tag = 2;
            MoveDownButton.Tag = 3;
            MoveLeftButton.Tag = 4;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.		
        }

        async partial void ConnectionButtonPressed(UIButton sender)
        {
            switch (sender.Tag)
            {
                case 1:
                    if (!bluetoothConnection.IsSearching)
                    {
                        sender.Enabled = false;
                        await bluetoothConnection.StartSearchingForArduinoToConnectAsync();
                        sender.Enabled = true;
                    }
                    break;
                case 2:
                    if (bluetoothConnection.IsSearching)
                    {
                        bluetoothConnection.StopSearchingForDevices();
                    }
                    break;
            }
        }

        partial void MoveButtonPressed(UIButton sender)
        {
            switch (sender.Tag)
            {
                case 1:
                    bluetoothConnection.PushData(1);
                    break;
                case 2:
                    bluetoothConnection.PushData(2);
                    break;
                case 3:
                    bluetoothConnection.PushData(3);
                    break;
                case 4:
                    bluetoothConnection.PushData(4);
                    break;
            }
        }
    }
}
