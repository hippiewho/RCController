using System;

using UIKit;
using BluetoothConnector;

namespace RCController.iOS
{
    public partial class ViewController : UIViewController
    {
        private BluetoothConnection bluetoothConnection = null;
        
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

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
                    if (bluetoothConnection == null) bluetoothConnection = new BluetoothConnection();
                    sender.Enabled = false;
                    await bluetoothConnection.StartSearchingForDevicesAsync();
                    sender.Enabled = true;
                    break;
                case 2:
                    break;
            }
        }
    }
}
