using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ZebraRFIDXamarinDemo
{
	public partial class MainPage : ContentPage
	{
		public ObservableCollection<string> Items { get; set; }

		public MainPage()
		{
			InitializeComponent();
		}

		internal void OnResume()
		{
			Console.WriteLine("OnResume");
		}

		internal void OnSleep()
		{
			Console.WriteLine("OnSleep");
		}

		private void ToolbarItem_Clicked(object sender, EventArgs e)
		{
		}
	}
}