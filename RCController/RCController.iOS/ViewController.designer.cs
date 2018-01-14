// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace RCController.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UIButton Button { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ConnectBluetoothButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DisconnectButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MoveDownButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MoveLeftButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MovementView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MoveRightButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MoveUpButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView OptionsView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ConnectBluetoothButton != null) {
                ConnectBluetoothButton.Dispose ();
                ConnectBluetoothButton = null;
            }

            if (DisconnectButton != null) {
                DisconnectButton.Dispose ();
                DisconnectButton = null;
            }

            if (MoveDownButton != null) {
                MoveDownButton.Dispose ();
                MoveDownButton = null;
            }

            if (MoveLeftButton != null) {
                MoveLeftButton.Dispose ();
                MoveLeftButton = null;
            }

            if (MovementView != null) {
                MovementView.Dispose ();
                MovementView = null;
            }

            if (MoveRightButton != null) {
                MoveRightButton.Dispose ();
                MoveRightButton = null;
            }

            if (MoveUpButton != null) {
                MoveUpButton.Dispose ();
                MoveUpButton = null;
            }

            if (OptionsView != null) {
                OptionsView.Dispose ();
                OptionsView = null;
            }
        }
    }
}