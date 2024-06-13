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
                                    // ConfigureReader();
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
                                Application.Current.MainPage.DisplayAlert("Alert", "Hello", "Cancel", "ok");
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
            
        }

        protected async void GoRead(object sender, EventArgs e)
        {
            List<string> tags = new List<string>();
            tags.Add("4E20198C9E01000000000001");
            tags.Add("4E20190353BB800000000000");
            tags.Add("4E2019845286800000000000");

            foreach (var item in tags)
            {
                var data = DecodificarEpc(item);
            }
        }

        public static Tuple<int, int> DecodificarEpc(string epc)
        {
            // Verificar que el epc sea una cadena hexadecimal de 24 caracteres
            if (epc == null || epc.Length != 24 || !IsHex(epc))
            {
                return Tuple.Create(-1, -1); // EPC invÃ¡lido
            }
            // Extraer el nÃºmero de identificaciÃ³n en hexadecimal
            string idHex = epc.Substring(6, 8);
            // Convertir el nÃºmero de identificaciÃ³n a binario
            string idBin = Convert.ToString(Convert.ToInt32(idHex, 16), 2).PadLeft(32, '0');
            // Quitar los dos Ãºltimos bits para obtener una longitud de 26 bits
            idBin = idBin.Substring(0, 26);
            // Eliminar el primer y el Ãºltimo bit de paridad
            idBin = idBin.Substring(1, 24);
            // Separar los primeros 8 bits del cÃ³digo de instalaciÃ³n y los Ãºltimos 16 bits del nÃºmero de identificaciÃ³n
            string fcBin = idBin.Substring(0, 8);
            idBin = idBin.Substring(8, 16);
            // Convertir los bits a decimal
            int fcDec = Convert.ToInt32(fcBin, 2);
            int idDec = Convert.ToInt32(idBin, 2);
            // Devolver el resultado como una tupla
            return Tuple.Create(fcDec, idDec);
        }

        public static bool IsHex(string s)
        {
            // Verificar si una cadena es hexadecimal
            foreach (char c in s)
            {
                if (!"0123456789ABCDEF".Contains(c.ToString()))
                {
                    return false;
                }
            }
            return true;
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