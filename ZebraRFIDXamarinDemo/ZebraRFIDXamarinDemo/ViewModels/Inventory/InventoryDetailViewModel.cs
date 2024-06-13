using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Com.Zebra.Rfid.Api3;
using System.Runtime.CompilerServices;
using System.Timers;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.Views.Inventory;
using Android.Widget;

namespace ZebraRFIDXamarinDemo.ViewModels.Inventory
{
    public class InventoryDetailViewModel : InventoryBaseViewModel
    {

        public Command SeeDetailInventoryCommand { get; }
        public Command InventoryInventoryCommand { get; }
        public string tags = "";

        private static ObservableCollection<Models.Tag.Tag> _allItems;
        private static Models.Tag.Tag _mySelectedItem;
        private static Dictionary<String, int> tagListDict = new Dictionary<string, int>();
        private DateTime startime;
        private int totalTagCount = 0;
        private static string _uniquetags, _totaltags, _totaltime;
        private string _connectionStatus, _readerStatus;
        private System.Timers.Timer aTimer;
        private bool _listAvailable;
        public Command ReadTagsCommand { get; }

        public InventoryDetailViewModel()
        {
            // collection
            if (_allItems == null)
                _allItems = new ObservableCollection<Models.Tag.Tag>();
            // UI for hint
            updateHints();
            SeeDetailInventoryCommand = new Command<InventoryLocationAQ>(OnSeeDetailInventory);
            InventoryInventoryCommand = new Command<InventoryLocationAQ>(OnInventoryInventory);
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            try
            {
                var data = InventoryLocationAsset;
                var dataSQLITE = await App.inventoryRepository.GetByInventoryIdAsync(data.InventarioId);
                if (dataSQLITE != null)
                {
                    InventoryLocationAssetQuery = dataSQLITE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void OnSeeDetailInventory(InventoryLocationAQ inventoryLocationSync)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new InventoryLocationDetail(inventoryLocationSync));

            // var secondPage = new InventoryLocationDetail(InventorySync);
            // await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.Navigation.PushAsync(secondPage, true));
        }

        private async void OnInventoryInventory(InventoryLocationAQ inventoryLocationSync)
        {
            try
            {
                
                IsRunning = true;
               
                Preferences.Remove("Activo_Tag");
                if (inventoryLocationSync.Activo.Count() > 0)
                {
                    IsRunning = false;
                    var json = inventoryLocationSync.Activo.Select(s => new AssetQuery
                    {
                        Id = s.Id,
                        Nombre = s.Nombre,
                        Status = s.Status,
                        Tag = new TagQuery
                        {
                            Id = s.Tag.Id,
                            Numero = s.Tag.Numero
                        }
                    }).Where(w => w.Status == false).ToList();
                    if (json.Count() > 0)
                    {
                        string jsonData = JsonConvert.SerializeObject(json);
                        Preferences.Set("Activo_Tag", jsonData);
                        bool hasKey = Preferences.ContainsKey("Activo_Tag");
                        if (hasKey)
                        {
                            var jsonGet = Preferences.Get("Activo_Tag", "");
                            // Toast.MakeText(Android.App.Application.Context, "JSON ::: ACTIVOS ::: " + jsonGet, ToastLength.Short).Show();
                            await Application.Current.MainPage.DisplayAlert("¡Advertencia!", "Estado del Lector: " + readerConnection + " \n" +
                                "Ubicación: " + inventoryLocationSync.UbicacionNombre + "\n" +
                                "Total de Activos: " + json.Count() + "\n" +
                                readerStatus + "", "Continuar");
                        }
                    }
                    else
                    {
                        Preferences.Remove("Activo_Tag");
                    }
                }
                else
                {
                    Preferences.Remove("Activo_Tag");
                    IsRunning = false;
                }
           
            }
            catch (Exception ex)
            {
                IsRunning = false;
                throw ex;
            }
        }





        public ObservableCollection<Models.Tag.Tag> AllItems { get => _allItems; set => _allItems = value; }

        public Models.Tag.Tag MySelectedItem { get => _mySelectedItem; set => _mySelectedItem = value; }

        public static String SelectedItem
        {
            get { return _mySelectedItem?.InvID; }
        }

        public string UniqueTags { get => _uniquetags; set { _uniquetags = value; OnPropertyChanged(); } }
        public string TotalTags { get => _totaltags; set { _totaltags = value; OnPropertyChanged(); } }
        public string TotalTime { get => _totaltime; set { _totaltime = value; OnPropertyChanged(); } }
        public string readerConnection { get => _connectionStatus; set { _connectionStatus = value; OnPropertyChanged(); } }
        public bool listAvailable { get => _listAvailable; set { _listAvailable = value; OnPropertyChanged(); } }
        public bool hintAvailable { get => !_listAvailable; set { OnPropertyChanged(); } }
        public string readerStatus { get => _readerStatus; set { _readerStatus = value; OnPropertyChanged(); } }

        private Object tagreadlock = new object();

        // Tag event
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void TagReadEvent(TagData[] aryTags)
        {
            //Toast.MakeText(Android.App.Application.Context, "TEG READ EVENT", ToastLength.Short).Show();
            //if (aryTags != null)
            //{
            //    if (aryTags.Count() > 0)
            //    {
            //        var tag = aryTags.Select(s => s.TagID).ToList();
            //        var tagString = Newtonsoft.Json.JsonConvert.SerializeObject(tag);
            //        Toast.MakeText(Android.App.Application.Context, tagString, ToastLength.Short).Show();
            //    }
            //}


            lock (tagreadlock)
            {
                for (int index = 0; index < aryTags.Length; index++)
                {
                    Console.WriteLine("Tag ID " + aryTags[index].TagID);
                    // Toast.MakeText(Android.App.Application.Context, aryTags[index].TagID, ToastLength.Short).Show();
                    String tagID = aryTags[index].TagID;
                    if (tagID != null)
                    {
                        if (tagListDict.ContainsKey(tagID))
                        {
                            tagListDict[tagID] = tagListDict[tagID] + aryTags[index].TagSeenCount;
                            UpdateCount(tagID, tagListDict[tagID], aryTags[index].PeakRSSI);
                        }
                        else
                        {
                            tagListDict.Add(tagID, aryTags[index].TagSeenCount);
                            UpdateList(tagID, aryTags[index].TagSeenCount, aryTags[index].PeakRSSI);
                        }
                    }
                    totalTagCount += aryTags[index].TagSeenCount;
                    updateCounts();
                    if (aryTags[index].OpCode == ACCESS_OPERATION_CODE.AccessOperationRead &&
                        aryTags[index].OpStatus == ACCESS_OPERATION_STATUS.AccessSuccess)
                    {
                        if (aryTags[index].MemoryBankData.Length > 0)
                        {
                            Console.WriteLine(" Mem Bank Data " + aryTags[index].MemoryBankData);
                        }
                    }
                }
            }
        }

        private void UpdateList(String tag, int count, short rssi)
        {
            // Toast.MakeText(Android.App.Application.Context, count, ToastLength.Short).Show();
            
            // await Application.Current.MainPage.DisplayAlert("¡Advertencia!", "Tag: " + tags + " \n", "Inventariar");
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                // tags += tag;
                _allItems.Add(new Models.Tag.Tag { InvID = tag, TagCount = count, RSSI = rssi });
                // Toast.MakeText(Android.App.Application.Context, "TAG  \n\n" + tags, ToastLength.Short).Show();
            });
        }

        private void UpdateCount(String tag, int count, short rssi)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                var found = _allItems.FirstOrDefault(x => x.InvID == tag);
                if (found != null)
                {
                    found.TagCount = count;
                    found.RSSI = rssi;
                }
            });
        }

        public override void HHTriggerEvent(bool pressed)
        {
            if (pressed)
            {
                PerformInventory();
                listAvailable = true;
                hintAvailable = false;
            }
            else
            {
                StopInventory();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void StopInventory()
        {
            rfidModel.StopInventory();
            aTimer?.Stop();
            aTimer?.Dispose();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void PerformInventory()
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => { tagListDict.Clear(); _allItems.Clear(); });
            totalTagCount = 0;
            startime = DateTime.Now;
            SetTimer();
            rfidModel.PerformInventory();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void StatusEvent(Events.StatusEventData statusEvent)
        {
            if (statusEvent.StatusEventType == STATUS_EVENT_TYPE.InventoryStartEvent)
            {
                //startime = DateTime.Now;
            }
            if (statusEvent.StatusEventType == STATUS_EVENT_TYPE.InventoryStopEvent)
            {
                updateCounts();
                int total = 0;
                foreach (var entry in tagListDict)
                    total += entry.Value;
                Console.WriteLine("Unique tags " + tagListDict.Count + " Total tags" + total);
                
            }
        }

        private async void updateCounts()
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                bool hasKey = Preferences.ContainsKey("Activo_Tag");
                if (hasKey)
                {
                    if (tagListDict.Count() > 0)
                    {
                        var jsonGet = Preferences.Get("Activo_Tag", "");
                        var jsonData = JsonConvert.DeserializeObject<List<AssetQuery>>(jsonGet);
                        var jsonDataQuery = jsonData.Select(s => s.Nombre).ToList();
                        if (jsonData.Count() > 0)
                        {
                            Preferences.Remove("Activo_Tag");
                            UniqueTags = tagListDict.Count.ToString();
                            TotalTags = totalTagCount.ToString();
                            TimeSpan span = (DateTime.Now - startime);
                            TotalTime = span.ToString("hh\\:mm\\:ss");

                            var tags = Newtonsoft.Json.JsonConvert.SerializeObject(tagListDict);
                            // Toast.MakeText(Android.App.Application.Context, "NÚMERO DE TAGS: " + UniqueTags + "\n\n" + "TAGS IDS  \n\n" + data, ToastLength.Short).Show();
                            await Application.Current.MainPage.DisplayAlert("¡Advertencia!", "NÚMERO DE TAGS: " + UniqueTags + "\n\n" + "LISTA DE TAGS \n\n" + string.Join("\n", tagListDict.Keys) + "\n\nLISTA DE ACTIVOS A INVENTARIAR  \n\n" + string.Join("\n", jsonDataQuery), "Continuar");
                            // IsRunning = true;

                            // var iteracion = 

                            //foreach (var itemtags in tagListDict)
                            //{
                            //    foreach (var itemactivos in jsonData)
                            //    {
                            //        if (itemtags.Key == itemactivos.Tag.Numero)
                            //        {

                            //        }
                            //    }
                            //}
                        }
                        else
                        {
                            Preferences.Remove("Activo_Tag");
                        }
                    }
                }
            });
        }

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            updateCounts();
        }

        public override void ReaderConnectionEvent(bool connection)
        {
            base.ReaderConnectionEvent(connection);
            updateHints();
            aTimer?.Stop();
            aTimer?.Dispose();
        }

        private void updateHints()
        {
            if (_allItems.Count == 0)
            {
                _listAvailable = false;
                readerConnection = isConnected ? "Conectado" : "Desconectado";
                if (isConnected)
                {
                    readerStatus = rfidModel.isBatchMode ? "El inventario se está ejecutando en modo de lectura" : "Mantenga presionado el gatillo para leer los Tags";
                }
            }
            else
                _listAvailable = true;
        }
    }
}