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
using System.Runtime.CompilerServices;

namespace ZebraRFIDXamarinDemo.Views.Dashboard
{
    public partial class DashboardIndex : ContentPage
    {
        private Readers readers;
        private IList<ReaderDevice> availableRFIDReaderList = new List<ReaderDevice>();
        private ReaderDevice readerDevice;
        private static RFIDReader Reader;
        private EventHandler eventHandler;
        private string _status;
        public string Status { get => _status; set { _status = value; OnPropertyChanged(); } }
        public string _tag;
        public string Tag { get => _tag; set { _tag = value; OnPropertyChanged(); } }

        bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                SetProperty(ref isRunning, value);
            }
        }
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChaged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChaged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

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
            IsRunning = true;
            ThreadPool.QueueUserWorkItem(async o =>
            {
                try
                {
                    IsRunning = false;
                    if (readers != null && readers.AvailableRFIDReaderList != null)
                    {
                        availableRFIDReaderList =
                            readers.AvailableRFIDReaderList;
                        if (availableRFIDReaderList.Count > 0)
                        {
                            if (Reader == null)
                            {
                                // get first reader from list
                                readerDevice = availableRFIDReaderList[0];
                                Reader = readerDevice.RFIDReader;
                                // Establish connection to the RFID Reader
                                Reader.Connect();
                                if (Reader.IsConnected)
                                {
                                    Console.Out.WriteLine("Lector conectado");
                                    Status = "Lector conectado";
                                    // await Application.Current.MainPage.DisplayAlert("Status: ", Status, "Aceptar");
                                    ConfigureReader();
                                }
                                else
                                {
                                    Console.Out.WriteLine("Lector desconectado");
                                    Status = "Lector desconectado";
                                    // await Application.Current.MainPage.DisplayAlert("Status: ", Status, "Aceptar");
                                }
                            }
                        }
                        else
                        {
                            Console.Out.WriteLine("Lector fuera de línea");
                            Status = "Lector fuera de línea";
                            // await Application.Current.MainPage.DisplayAlert("Status: ", Status, "Aceptar");
                        }
                    }
                }
                catch (InvalidUsageException e)
                {
                    IsRunning = false;
                    e.PrintStackTrace();
                }
                catch (OperationFailureException e)
                {
                    IsRunning = false;
                    e.PrintStackTrace();
                    Console.Out.WriteLine("OperationFailureException " + e.VendorMessage);
                    Status = "OperationFailureException " + e.VendorMessage;
                    // await Application.Current.MainPage.DisplayAlert("Status: ", Status, "Aceptar");
                }
            });
        }

        private void ConfigureReader()
        {
            if (Reader.IsConnected)
            {
                TriggerInfo triggerInfo = new TriggerInfo();
                triggerInfo.StartTrigger.TriggerType = START_TRIGGER_TYPE.StartTriggerTypeImmediate;
                triggerInfo.StopTrigger.TriggerType = STOP_TRIGGER_TYPE.StopTriggerTypeImmediate;
                try
                {
                    // receive events from reader
                    if (eventHandler == null)
                    {
                        eventHandler = new EventHandler(Reader);
                    }

                    Reader.Events.AddEventsListener(eventHandler);
                    // HH event
                    Reader.Events.SetHandheldEvent(true);
                    // tag event with tag data
                    Reader.Events.SetTagReadEvent(true);
                    Reader.Events.SetAttachTagDataWithReadEvent(false);
                    // set trigger mode as rfid so scanner beam will not come
                    Reader.Config.SetTriggerMode(ENUM_TRIGGER_MODE.RfidMode, true);
                    // set start and stop triggers
                    Reader.Config.StartTrigger = triggerInfo.StartTrigger;
                    Reader.Config.StopTrigger = triggerInfo.StopTrigger;
                }
                catch (InvalidUsageException e)
                {
                    e.PrintStackTrace();
                }
                catch (OperationFailureException e)
                {
                    e.PrintStackTrace();
                }
            }
        }

        // Read/Status Notify handler
        // Implement the RfidEventsLister class to receive event notifications
        public class EventHandler : Java.Lang.Object, IRfidEventsListener
        {
            public EventHandler(RFIDReader Reader)
            {

            }

            // Read Event Notification
            public void EventReadNotify(RfidReadEvents e)
            {
                TagData[] myTags = Reader.Actions.GetReadTags(100);
                if (myTags != null)
                {
                    for (int index = 0; index < myTags.Length; index++)
                    {
                        Console.Out.WriteLine("Tag ID " + myTags[index].TagID);
                        
                        if (myTags[index].OpCode ==
                                ACCESS_OPERATION_CODE.AccessOperationRead &&
                                myTags[index].OpStatus ==
                                        ACCESS_OPERATION_STATUS.AccessSuccess)
                        {
                            if (myTags[index].MemoryBankData.Length > 0)
                            {
                                Console.Out.WriteLine(" Mem Bank Data " + myTags[index].MemoryBankData);
                            }
                        }
                    }
                }
            }

            // Status Event Notification
            public void EventStatusNotify(RfidStatusEvents rfidStatusEvents)
            {
                Console.Out.WriteLine("Status Notification: " + rfidStatusEvents.StatusEventData.StatusEventType);
                if (rfidStatusEvents.StatusEventData.StatusEventType == STATUS_EVENT_TYPE.HandheldTriggerEvent)
                {
                    if (rfidStatusEvents.StatusEventData.HandheldTriggerEventData.HandheldEvent ==
                            HANDHELD_TRIGGER_EVENT_TYPE.HandheldTriggerPressed)
                    {
                        ThreadPool.QueueUserWorkItem(o =>
                        {
                            try
                            {
                                Reader.Actions.Inventory.Perform();
                            }
                            catch (InvalidUsageException e)
                            {
                                e.PrintStackTrace();
                            }
                            catch (OperationFailureException e)
                            {
                                e.PrintStackTrace();
                            }
                        });

                    }
                    if (rfidStatusEvents.StatusEventData.HandheldTriggerEventData.HandheldEvent ==
                            HANDHELD_TRIGGER_EVENT_TYPE.HandheldTriggerReleased)
                    {
                        ThreadPool.QueueUserWorkItem(o =>
                        {
                            try
                            {
                                Reader.Actions.Inventory.Stop();
                            }
                            catch (InvalidUsageException e)
                            {
                                e.PrintStackTrace();
                            }
                            catch (OperationFailureException e)
                            {
                                e.PrintStackTrace();
                            }
                        });
                    }
                }
            }
        }

        protected async void GoConnect(object sender, EventArgs e)
        {
            // SDK
            if (readers == null)
            {
                readers = new Readers(Android.App.Application.Context, ENUM_TRANSPORT.ServiceSerial);
            }
            GetAvailableReaders();
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