using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.ViewModels.Inventory;

namespace ZebraRFIDXamarinDemo.Views.Inventory
{
    public partial class InventoryLocationDetail : ContentPage
    {
        public InventoryLocationAQ InventoryLocationSync { get; set; }
        InventoryLocationDetailViewModel inventoryLocationDetailViewModel;
        public Command PhysicalStatePickerCommand { get; }

        public InventoryLocationDetail()
        {
            InitializeComponent();
            BindingContext = inventoryLocationDetailViewModel = new InventoryLocationDetailViewModel();
        }

        public InventoryLocationDetail(InventoryLocationAQ inventoryLocation)
        {
            InitializeComponent();
            BindingContext = inventoryLocationDetailViewModel = new InventoryLocationDetailViewModel();
            PhysicalStatePickerCommand = new Command<InventoryDetail>(WhenPhysicalStateSelectedIndexChanged);
            if (inventoryLocation != null)
            {
                InventoryLocationSync = inventoryLocation;
                ((InventoryLocationDetailViewModel)BindingContext).InventoryLocationSync = inventoryLocation;
            }

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            inventoryLocationDetailViewModel.OnAppearing();
        }

        void SelectedAssetChanged(System.Object sender, System.EventArgs e)
        {
        }

        void SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void BindingContext_PhysicalStatePickerSelectedIndex(System.Object sender, System.EventArgs e)
        {
        }

        void _SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        private async void WhenPhysicalStateSelectedIndexChanged(InventoryDetail sender)
        {
            var ss = sender;
        }
    }
}