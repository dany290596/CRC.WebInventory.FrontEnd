using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZebraRFIDXamarinDemo.ViewModels.AppShell;
using ZebraRFIDXamarinDemo.Views.Api;
using ZebraRFIDXamarinDemo.Views.Inventory;
using ZebraRFIDXamarinDemo.Views.Person;
using ZebraRFIDXamarinDemo.Views.Setting;

namespace ZebraRFIDXamarinDemo
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        AppShellIndexViewModel appShellIndexViewModel;
        private Models.Reader.Reader rfidModel;
        public AppShell()
        {
            InitializeComponent();
            rfidModel = Models.Reader.Reader.readerModel;
            BindingContext = appShellIndexViewModel = new AppShellIndexViewModel();
            Routing.RegisterRoute(nameof(PersonCreate), typeof(PersonCreate));
            Routing.RegisterRoute(nameof(InventoryDetail), typeof(InventoryDetail));
            Routing.RegisterRoute(nameof(SettingCreate), typeof(SettingCreate));
            Routing.RegisterRoute(nameof(InventoryLocationDetail), typeof(InventoryLocationDetail));
            Routing.RegisterRoute(nameof(InventoryLocationEdit), typeof(InventoryLocationEdit));
        }

        internal void OnResume()
        {
            rfidModel?.SetTriggerMode();
            Console.WriteLine("OnResume");
        }

        internal void OnSleep()
        {
            //rfidModel?.Disconnect();
            Console.WriteLine("OnSleep");
        }
    }
}