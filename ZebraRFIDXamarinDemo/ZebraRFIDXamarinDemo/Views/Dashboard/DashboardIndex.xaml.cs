using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Views.Authorization;
using ZebraRFIDXamarinDemo.ViewModels.Inventory;

namespace ZebraRFIDXamarinDemo.Views.Dashboard
{
    public partial class DashboardIndex : ContentPage
    {
        InventoryIndexViewModel inventoryIndexViewModel;
        public DashboardIndex()
        {
            InitializeComponent();
            BindingContext = inventoryIndexViewModel = new InventoryIndexViewModel(Navigation);
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
    }
}