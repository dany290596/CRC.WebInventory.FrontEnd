using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Com.Zebra.Rfid.Api3;

namespace ZebraRFIDXamarinDemo
{
	public partial class MainPage : ContentPage
	{
		public ObservableCollection<string> Items { get; set; }
        private Models.Reader.Reader rfidModel;

		public MainPage()
		{
			InitializeComponent();
            rfidModel = Models.Reader.Reader.readerModel;
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

		private void ToolbarItem_Clicked(object sender, EventArgs e)
		{
		}
	}
}
