using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}