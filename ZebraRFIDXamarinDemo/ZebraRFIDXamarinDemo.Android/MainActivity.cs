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
            CreateDirectory();
        }

        /// <summary>
        /// Create directory for store the firmware plugin(Plugin should be ".SCNPLG")
        /// </summary>
        private void CreateDirectory()
        {
            CheckFileReadWritePermissions();
        }

        /// <summary>
        /// Check application permissions
        /// </summary>
        private void CheckFileReadWritePermissions()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }
            else
            {
                if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
                {
                    var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                    RequestPermissions(permissions, 2226);

                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            // Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if (requestCode == 2226)
            {
                try
                {

                    var firmwareDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads) + FIRMWARE_FOLDER;
                    var outputDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads) + OUTPUT_FOLDER;
                    Directory.CreateDirectory(firmwareDirectory);
                    Directory.CreateDirectory(outputDirectory);

                    if (Directory.Exists(firmwareDirectory))
                    {
                        Console.WriteLine("That path firmwareDirectory  exists already.");

                    }

                }
                catch (Java.Lang.Exception e)
                {
                    Console.WriteLine("Sample app Exception " + e.Message);
                }
            }
            if (requestCode == BLUETOOTH_PERMISSION_REQUEST_CODE)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    if (_onRequestPermissionsResult != null)
                    {
                        _onRequestPermissionsResult(true);
                        _onRequestPermissionsResult = null;
                    }
                }
                else
                {
                    if (_onRequestPermissionsResult != null)
                    {
                        _onRequestPermissionsResult(false);
                        _onRequestPermissionsResult = null;
                    }
                }
            }
        }

        internal void CheckBTPermission(Action<bool> onRequestPermissionsResult)
        {
            _onRequestPermissionsResult = onRequestPermissionsResult;

            if (Build.VERSION.SdkInt <= BuildVersionCodes.R)
            {
                if (_onRequestPermissionsResult != null)
                {
                    _onRequestPermissionsResult(true);
                    _onRequestPermissionsResult = null;
                }
            }
            else
            {
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothConnect) == (int)Permission.Granted)
                {
                    if (_onRequestPermissionsResult != null)
                    {
                        _onRequestPermissionsResult(true);
                        _onRequestPermissionsResult = null;
                    }
                }
                else
                {
                    var permissions = new string[] { Manifest.Permission.BluetoothConnect, Manifest.Permission.BluetoothScan };
                    RequestPermissions(permissions, BLUETOOTH_PERMISSION_REQUEST_CODE);
                }
            }
        }

        internal void CheckBTEnable(Action<bool> onRequestBTEnable)
        {
            _onRequestBTEnable = onRequestBTEnable;
            Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
            StartActivityForResult(enableBtIntent, BLUETOOTH_ENABLE_REQUEST_CODE);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == BLUETOOTH_ENABLE_REQUEST_CODE)
            {
                if (resultCode == Result.Ok)
                {
                    if (_onRequestBTEnable != null)
                    {
                        _onRequestBTEnable(true);
                        _onRequestBTEnable = null;
                    }
                }
                else
                {
                    if (_onRequestBTEnable != null)
                    {
                        _onRequestBTEnable(false);
                        _onRequestBTEnable = null;
                    }
                }
            }
        }
    }
}