using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.ViewModels.Inventory;
using ZebraRFIDXamarinDemo.ViewModels.Tag;

namespace ZebraRFIDXamarinDemo.Views.Inventory
{
    public partial class InventoryDetail : ContentPage
    {
        public InventoryLocationAssetQuery InventoryLocationAsset { get; set; }
        InventoryDetailViewModel inventoryDetailViewModel;
        public InventoryDetail()
        {
            InitializeComponent();
            BindingContext = inventoryDetailViewModel = new InventoryDetailViewModel();
        }

        public InventoryDetail(InventoryLocationAssetQuery inventoryLocationAsset)
        {
            InitializeComponent();
            BindingContext = inventoryDetailViewModel = new InventoryDetailViewModel();
            if (inventoryLocationAsset != null)
            {
                ((InventoryDetailViewModel)BindingContext).InventoryLocationAsset = inventoryLocationAsset;
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            inventoryDetailViewModel.UpdateIn();
            inventoryDetailViewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            inventoryDetailViewModel.UpdateOut();
        }
    }
}