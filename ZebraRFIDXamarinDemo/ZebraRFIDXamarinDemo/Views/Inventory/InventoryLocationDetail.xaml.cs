using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.ViewModels.Inventory;

namespace ZebraRFIDXamarinDemo.Views.Inventory
{
    public partial class InventoryLocationDetail : ContentPage
    {
        public Location LocationSync { get; set; }
        InventoryLocationDetailViewModel inventoryLocationDetailViewModel;

        public InventoryLocationDetail()
        {
            InitializeComponent();
            BindingContext = inventoryLocationDetailViewModel = new InventoryLocationDetailViewModel(Navigation);
        }

        public InventoryLocationDetail(Location locationSync)
        {
            InitializeComponent();
            BindingContext = inventoryLocationDetailViewModel = new InventoryLocationDetailViewModel(Navigation);
            if (locationSync != null)
            {
                ((InventoryLocationDetailViewModel)BindingContext).LocationSync = locationSync;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            inventoryLocationDetailViewModel.OnAppearing();
        }

        void Picker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void SelectedAssetChanged(System.Object sender, System.EventArgs e)
        {
            var aa = sender;
        }
    }
}