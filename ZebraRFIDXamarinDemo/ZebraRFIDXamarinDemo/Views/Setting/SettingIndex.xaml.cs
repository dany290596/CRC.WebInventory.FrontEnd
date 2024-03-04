using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZebraRFIDXamarinDemo.ViewModels.Person;
using ZebraRFIDXamarinDemo.ViewModels.Setting;

namespace ZebraRFIDXamarinDemo.Views.Setting
{
	public partial class SettingIndex : ContentPage
	{
		SettingIndexViewModel settingIndexViewModel;
		public SettingIndex()
		{
			InitializeComponent();
			BindingContext = settingIndexViewModel = new SettingIndexViewModel(Navigation);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			settingIndexViewModel.OnAppearing();
		}
	}
}