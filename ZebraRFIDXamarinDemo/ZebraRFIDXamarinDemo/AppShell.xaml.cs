using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZebraRFIDXamarinDemo.ViewModels.AppShell;
using ZebraRFIDXamarinDemo.Views.Inventory;
using ZebraRFIDXamarinDemo.Views.Person;
using ZebraRFIDXamarinDemo.Views.Setting;

namespace ZebraRFIDXamarinDemo
{
	public partial class AppShell : Xamarin.Forms.Shell
	{
		AppShellIndexViewModel appShellIndexViewModel;
		public AppShell()
		{
			InitializeComponent();
			BindingContext = appShellIndexViewModel = new AppShellIndexViewModel();
			Routing.RegisterRoute(nameof(PersonCreate), typeof(PersonCreate));
			Routing.RegisterRoute(nameof(InventoryDetail), typeof(InventoryDetail));
			Routing.RegisterRoute(nameof(SettingCreate), typeof(SettingCreate));
		}
	}
}