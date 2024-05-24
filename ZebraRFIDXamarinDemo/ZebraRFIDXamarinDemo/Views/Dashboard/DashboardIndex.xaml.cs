using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Views.Authorization;
using ZebraRFIDXamarinDemo.ViewModels.Inventory;
using ZebraRFIDXamarinDemo.Views.Inventory;
using Com.Zebra.Rfid.Api3;
using System.Collections;
using System.Threading;

namespace ZebraRFIDXamarinDemo.Views.Dashboard
{
    public partial class DashboardIndex : ContentPage
    {
        private static Readers readers;
        private static IList availableRFIDReaderList;
        private static ReaderDevice readerDevice;
        private static RFIDReader Reader;
        private EventHandler eventHandler;
        private string _status;
        public string Status { get => _status; set { _status = value; OnPropertyChanged(); } }

        InventoryIndexViewModel inventoryIndexViewModel;
        public DashboardIndex()
        {
            InitializeComponent();
            BindingContext = inventoryIndexViewModel = new InventoryIndexViewModel(Navigation);
            BindingContext = this;
            // SDK
            if (readers == null)
            {
                readers = new Readers(Android.App.Application.Context, ENUM_TRANSPORT.ServiceSerial);
            }
            GetAvailableReaders();
        }

        private void GetAvailableReaders()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    if (readers != null && readers.AvailableRFIDReaderList != null)
                    {
                        availableRFIDReaderList =
                            (IList)readers.AvailableRFIDReaderList;
                        if (availableRFIDReaderList.Count > 0)
                        {
                            if (Reader == null)
                            {
                                // get first reader from list
                                readerDevice = (ReaderDevice)availableRFIDReaderList[0];
                                Reader = readerDevice.RFIDReader;
                                // Establish connection to the RFID Reader
                                Reader.Connect();
                                if (Reader.IsConnected)
                                {
                                    Console.Out.WriteLine("Lector conectado");
                                    Status = "Lector conectado";
                                    //ConfigureReader();
                                }
                                else
                                {
                                    Console.Out.WriteLine("Lector desconectado");
                                    Status = "Lector desconectado";
                                }
                            }
                        }
                        else
                        {
                            Console.Out.WriteLine("Lector fuera de línea");
                            Status = "Lector fuera de línea";
                        }
                    }
                }
                catch (InvalidUsageException e)
                {
                    e.PrintStackTrace();
                }
                catch (OperationFailureException e)
                {
                    e.PrintStackTrace();
                    Console.Out.WriteLine("OperationFailureException " + e.VendorMessage);
                    Status = "OperationFailureException " + e.VendorMessage;
                }
            });
        }

        public class EventHandler : Java.Lang.Object, IRfidEventsListener
        {
            public EventHandler(RFIDReader Reader)
            {
            }
            // Read Event Notification
            public void EventReadNotify(RfidReadEvents e)
            {
                TagData[] myTags = Reader.Actions.GetReadTags(100);
            }

            // Status Event Notification
            public void EventStatusNotify(RfidStatusEvents rfidStatusEvents)
            {
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            inventoryIndexViewModel.OnAppearing();
        }

        private async void OnLogoutButton(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginIndex)}");
        }

        protected void GoGoogle(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://youtu.be/NWFOgVw8dGU"));
        }

        protected async void GoInventory(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(InventoryIndex)}");
        }
    }
}