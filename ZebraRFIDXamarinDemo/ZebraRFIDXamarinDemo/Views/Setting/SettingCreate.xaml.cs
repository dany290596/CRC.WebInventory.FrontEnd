using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZebraRFIDXamarinDemo.ViewModels.Setting;

namespace ZebraRFIDXamarinDemo.Views.Setting
{
	public partial class SettingCreate : ContentPage
	{
		public Models.Setting.Setting Device { get; set; }
		public SettingCreate()
		{
			InitializeComponent();
			BindingContext = new SettingCreateViewModel();
		}

		public SettingCreate(Models.Setting.Setting device)
		{
			InitializeComponent();
			BindingContext = new SettingCreateViewModel();
			if (device != null)
			{
				((SettingCreateViewModel)BindingContext).Device = device;
			}
		}
	}
}