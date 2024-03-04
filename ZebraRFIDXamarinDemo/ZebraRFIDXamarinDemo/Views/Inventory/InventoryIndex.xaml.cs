using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZebraRFIDXamarinDemo.ViewModels.Inventory;

namespace ZebraRFIDXamarinDemo.Views.Inventory
{
    public partial class InventoryIndex : ContentPage
    {
        InventoryIndexViewModel inventoryIndexViewModel;
        public InventoryIndex()
        {
            InitializeComponent();
            BindingContext = inventoryIndexViewModel = new InventoryIndexViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            inventoryIndexViewModel.OnAppearing();
        }
    }
}