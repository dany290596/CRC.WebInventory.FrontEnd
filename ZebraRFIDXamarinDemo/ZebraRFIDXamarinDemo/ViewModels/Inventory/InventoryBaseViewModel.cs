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
        private InventoryQuery _inventorySync;
        private Location _locationSync;
        private InventoryDetail _inventoryDetailSync;

        public INavigation Navigation { get; set; }
        public InventoryQuery InventorySync
        {
            get { return _inventorySync; }
            set { _inventorySync = value; OnPropertyChanged(); }
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