using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZebraRFIDXamarinDemo.ViewModels.Inventory;
using ZebraRFIDXamarinDemo.ViewModels.Person;

namespace ZebraRFIDXamarinDemo.Views.Inventory
{
    public partial class InventoryLocationDetail : ContentPage
    {
        public Models.Startup.Inventory InventorySync { get; set; }
        InventoryLocationDetailViewModel inventoryLocationDetailViewModel;

        public InventoryLocationDetail()
        {
            InitializeComponent();
            BindingContext = inventoryLocationDetailViewModel = new InventoryLocationDetailViewModel();
        }

        public InventoryLocationDetail(Models.Startup.Inventory inventorySync)
        {
            InitializeComponent();
            BindingContext = inventoryLocationDetailViewModel = new InventoryLocationDetailViewModel();
            if (inventorySync != null)
            {
                ((InventoryLocationDetailViewModel)BindingContext).InventorySync = inventorySync;
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
        }
    }
}