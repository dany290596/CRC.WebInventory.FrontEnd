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

            SeeDetailInventoryCommand = new Command<InventoryLocationAQ>(OnSeeDetailInventory);
        }



        public async void OnAppearing()
        {
            IsBusy = true;
            try
            {
                var data = InventoryLocationAsset;
                var dataSQLITE = await App.inventoryRepository.GetByInventoryIdAsync(data.InventarioId);
                if (dataSQLITE != null)
                {
                    InventoryLocationAssetQuery = dataSQLITE;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private async void OnSeeDetailInventory(InventoryLocationAQ inventoryLocationSync)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new InventoryLocationDetail(inventoryLocationSync));

            // var secondPage = new InventoryLocationDetail(InventorySync);
            // await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.Navigation.PushAsync(secondPage, true));
        }
    }
}