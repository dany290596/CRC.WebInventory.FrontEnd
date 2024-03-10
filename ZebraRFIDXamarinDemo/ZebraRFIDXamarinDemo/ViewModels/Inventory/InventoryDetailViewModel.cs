using System;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.Views.Inventory;

namespace ZebraRFIDXamarinDemo.ViewModels.Inventory
{
    public class InventoryDetailViewModel : InventoryBaseViewModel
    {

        public Command SeeDetailInventoryCommand { get; }
        public Command InventoryInventoryCommand { get; }

        public InventoryDetailViewModel()
        {
            SeeDetailInventoryCommand = new Command<Location>(OnSeeDetailInventory);
        }

        private async void OnSeeDetailInventory(Location locationSync)
        {
            var asa = "";
          await Application.Current.MainPage.Navigation.PushAsync(new Views.Inventory.InventoryLocationDetail(locationSync));

           // var secondPage = new InventoryLocationDetail(InventorySync);
           // await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.Navigation.PushAsync(secondPage, true));
        }
    }
}