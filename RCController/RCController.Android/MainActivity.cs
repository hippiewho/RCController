using Android.App;
using Android.Widget;
using Android.OS;
using System;
using BluetoothConnector;

namespace RCController.Droid
{
    [Activity(Label = "RCController", MainLauncher = true, Icon = "@mipmap/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : Activity
    {
        Button UpButton, 
               DownButton, 
               LeftButton, 
               RightButton, 
               StartConnectionButton, 
               EndConnectionButton;

        BluetoothConnection bluetoothConnection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            SetupViewButtons();

        }

        private void SetupViewButtons()
        {
            // movement buttons
            UpButton = (Button)FindViewById(Resource.Id.MoveUpButton);
            DownButton = (Button)FindViewById(Resource.Id.MoveDownButton);
            LeftButton = (Button)FindViewById(Resource.Id.MoveLeftButton);
            RightButton = (Button)FindViewById(Resource.Id.MoveRightButton);
            
            UpButton.Click += MoveButtonClick;
            DownButton.Click += MoveButtonClick;
            LeftButton.Click += MoveButtonClick;
            RightButton.Click += MoveButtonClick;

            // connection buttons
            StartConnectionButton = (Button)FindViewById(Resource.Id.ConnectButton);
            EndConnectionButton = (Button)FindViewById(Resource.Id.DisconnectButton);

            StartConnectionButton.Click += ConnectionButtonClick;
            EndConnectionButton.Click += ConnectionButtonClick;

        }

        private void MoveButtonClick(object Sender, EventArgs Events)
        {   
            Button button = (Button)Sender;
            switch (button.Id)
            {
                case Resource.Id.MoveUpButton:
                    bluetoothConnection.PushData(1);
                    Console.WriteLine("MoveUp");
                    break;
                case Resource.Id.MoveDownButton:
                    bluetoothConnection.PushData(2);
                    Console.WriteLine("MoveDown");
                    break;
                case Resource.Id.MoveLeftButton:
                    bluetoothConnection.PushData(3);
                    Console.WriteLine("MoveLeft");
                    break;
                case Resource.Id.MoveRightButton:
                    bluetoothConnection.PushData(4);
                    Console.WriteLine("MoveRight");
                    break;
            }
        }

        private async void ConnectionButtonClick(object Sender, EventArgs Events)
        {
            Button button = (Button)Sender;
            switch (button.Id)
            {
                case Resource.Id.ConnectButton:
                    if (bluetoothConnection == null) bluetoothConnection = new BluetoothConnection();
                    button.Enabled = false;
                    await bluetoothConnection.StartSearchingForDevicesAsync();
                    button.Enabled = true;
                    break;
                case Resource.Id.DisconnectButton:
                    bluetoothConnection.StopSearchingForDevices();
                    //bluetoothConnection.ShowArrayList();
                    break;
            }
        }
    }
}

