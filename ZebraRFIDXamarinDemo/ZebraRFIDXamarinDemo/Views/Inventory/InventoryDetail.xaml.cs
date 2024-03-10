using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.ViewModels.Inventory;
using ZebraRFIDXamarinDemo.ViewModels.Person;

namespace ZebraRFIDXamarinDemo.Views.Inventory
{
    public partial class InventoryDetail : ContentPage
    {
        public Models.Startup.Inventory InventorySync { get; set; }
        public InventoryDetail()
        {
            InitializeComponent();
            BindingContext = new InventoryDetailViewModel();
        }

        
        public InventoryDetail(InventoryQuery inventorySync)
        {
            InitializeComponent();
            BindingContext = new InventoryDetailViewModel();
            if (inventorySync != null)
            {
                ((InventoryDetailViewModel)BindingContext).InventorySync = inventorySync;
            }
        }
    }
}