using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using System.IO;
using AndroidX.Core.Content;
using Android.Bluetooth;
using Android.Content;
using System.Net;

namespace ZebraRFIDXamarinDemo.Droid
{
    [Activity(Label = "WebInventory", Icon = "@mipmap/webinventory", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const string FIRMWARE_FOLDER = "/ZebraFirmware";
        const string OUTPUT_FOLDER = "/ZebraOutput";

        const int BLUETOOTH_PERMISSION_REQUEST_CODE = 1001;
        const int BLUETOOTH_ENABLE_REQUEST_CODE = 1002;
        private Action<bool> _onRequestPermissionsResult;
        private Action<bool> _onRequestBTEnable;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            ServicePointManager
            .ServerCertificateValidationCallback +=
            (sender, cert, chain, sslPolicyErrors) => true;
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}