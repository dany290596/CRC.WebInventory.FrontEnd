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
            InventoryInventoryCommand = new Command<InventoryLocationAQ>(OnInventoryInventory);
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

        private async void OnInventoryInventory(InventoryLocationAQ inventoryLocationSync)
        {
            foreach (var asset in inventoryLocationSync.Activo)
            {
                if (asset.Tag != null)
                {
                    var dataAsset = await App.assetRepository.GetByIdAsync(asset.Id);
                    if (dataAsset != null)
                    {
                        dataAsset.Status = true;
                        // await App.assetRepository.UpdateAsync(dataAsset);
                    }
                }
            }
        }
    }
}