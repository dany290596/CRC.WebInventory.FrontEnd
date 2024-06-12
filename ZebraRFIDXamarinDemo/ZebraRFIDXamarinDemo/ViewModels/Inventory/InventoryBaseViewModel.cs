using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Com.Zebra.Rfid.Api3;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.ViewModels.Inventory
{
    public class InventoryBaseViewModel : INotifyPropertyChanged
    {
        private InventoryLocationAssetQuery _inventoryLocationAsset;
        private Location _locationSync;
        private InventoryDetail _inventoryDetailSync;

        public INavigation Navigation { get; set; }
        public InventoryLocationAssetQuery InventoryLocationAsset
        {
            get { return _inventoryLocationAsset; }
            set { _inventoryLocationAsset = value; OnPropertyChanged(); }
        }
        public Location LocationSync
        {
            get { return _locationSync; }
            set { _locationSync = value; OnPropertyChanged(); }
        }
        public InventoryDetail InventoryDetailSync
        {
            get { return _inventoryDetailSync; }
            set { _inventoryDetailSync = value; OnPropertyChanged(); }
        }
        private InventoryLocationAssetQuery _inventoryLocationAssetQuery;
        public InventoryLocationAssetQuery InventoryLocationAssetQuery
        {
            get { return _inventoryLocationAssetQuery; }
            set
            {
                _inventoryLocationAssetQuery = value; OnPropertyChanged();
            }
        }
        private InventoryLocationAQ _inventoryLocationSync;
        public InventoryLocationAQ InventoryLocationSync
        {
            get { return _inventoryLocationSync; }
            set
            {
                _inventoryLocationSync = value; OnPropertyChanged();
            }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                SetProperty(ref isBusy, value);
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        public static Models.Reader.Reader rfidModel = Models.Reader.Reader.readerModel;
        private static Models.Reader.Scanner scannerModel = Models.Reader.Scanner.scannerModel;

        public virtual void HHTriggerEvent(bool pressed)
        {

        }

        public virtual void TagReadEvent(TagData[] tags)
        {

        }

        public virtual void StatusEvent(Events.StatusEventData statusEvent)
        {

        }

        public virtual void ReaderConnectionEvent(bool connection)
        {
            isConnected = connection;
        }

        public virtual void ReaderAppearanceEvent(bool appeared)
        {

        }

        internal void UpdateIn()
        {
            rfidModel.TagRead += TagReadEvent;
            rfidModel.TriggerEvent += HHTriggerEvent;
            rfidModel.StatusEvent += StatusEvent;
            rfidModel.ReaderConnectionEvent += ReaderConnectionEvent;
            rfidModel.ReaderAppearanceEvent += ReaderAppearanceEvent;
        }

        internal void UpdateOut()
        {
            rfidModel.TagRead -= TagReadEvent;
            rfidModel.TriggerEvent -= HHTriggerEvent;
            rfidModel.StatusEvent -= StatusEvent;
            rfidModel.ReaderConnectionEvent -= ReaderConnectionEvent;
            rfidModel.ReaderAppearanceEvent -= ReaderAppearanceEvent;
        }

        public bool isConnected { get => rfidModel.isConnected; set => OnPropertyChanged(); }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void ScannerConnectionEvent(string deviceName)
        {

        }

        public virtual void CurrentProgressUpdate(int currentProgress)
        {

        }

        public virtual void FWVersion(string scannerFWVersion)
        {

        }

        internal void UpdateScannerIn()
        {
            scannerModel.ScannerConnectionEvent += ScannerConnectionEvent;
            scannerModel.CurrentProgress += CurrentProgressUpdate;
            scannerModel.FWVersion += FWVersion;
        }

        internal void UpdateScannerOut()
        {
            scannerModel.ScannerConnectionEvent -= ScannerConnectionEvent;
            scannerModel.CurrentProgress -= CurrentProgressUpdate;
            scannerModel.FWVersion -= FWVersion;
        }

        public bool scannerConnected { get => scannerModel.IsConnected; set => OnPropertyChanged(); }
        public string devicName { get => scannerModel.DeviceName; set => OnPropertyChanged(); }
        public string sFWVersion { get => scannerModel.getFWVersion; set => OnPropertyChanged(); }

        public virtual void BarcodeEvent(string barcode, string barcodeType)
        {

        }

        internal void UpdateBarcodeIn()
        {
            scannerModel.BarcodeEvent += BarcodeEvent;
            scannerModel.ScannerConnectionEvent += ScannerConnectionEvent;
        }

        internal void UpdateBarcodeOut()
        {
            scannerModel.BarcodeEvent -= BarcodeEvent;
            scannerModel.ScannerConnectionEvent -= ScannerConnectionEvent;
        }

        internal void UpdateReaderWiFiEventsIn()
        {
            rfidModel.WiFiNotificationEvent += WiFiNotificationEvent;
            rfidModel.WiFiScanResultsEvent += WiFiScanResultsEvent;
        }

        internal void UpdateReaderWiFiEventsOut()
        {
            rfidModel.WiFiNotificationEvent -= WiFiNotificationEvent;
            rfidModel.WiFiScanResultsEvent -= WiFiScanResultsEvent;
        }

        public virtual void WiFiNotificationEvent(string scanStatus)
        {

        }

        public virtual void WiFiScanResultsEvent(WifiScanData data)
        {

        }
    }
}