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
        public Location LocationSync { get; set; }
        InventoryLocationDetailViewModel inventoryLocationDetailViewModel;
        public Command PhysicalStatePickerCommand { get; }

        public InventoryLocationDetail()
        {
            InitializeComponent();
            BindingContext = inventoryLocationDetailViewModel = new InventoryLocationDetailViewModel();
        }

        public InventoryLocationDetail(Location locationSync)
        {
            InitializeComponent();
            BindingContext = inventoryLocationDetailViewModel = new InventoryLocationDetailViewModel();
            PhysicalStatePickerCommand = new Command<InventoryDetail>(WhenPhysicalStateSelectedIndexChanged);
            if (locationSync != null)
            {
                LocationSync = locationSync;
                ((InventoryLocationDetailViewModel)BindingContext).LocationSync = locationSync;
            }
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            inventoryLocationDetailViewModel.OnAppearing();

            var physicalStateAll = await App.physicalStateRepository.GetAllAsync();

            for (int i = 0; i < physicalStateAll.Count(); i++)
            {
                if (LocationSync != null)
                {
                    if (LocationSync.DetalleInventario != null)
                    {
                        if (LocationSync.DetalleInventario.Count() > 0)
                        {
                            //PhysicalStatePickerSelectedIndex.SelectedIndex = 1;
                            /*
                            if (physicalStateAll[i].Id == LocationSync.Activo.EstadoFisicoId)
                            {
                                PhysicalStatePickerSelectedIndex.SelectedIndex = i;
                            }
                            */
                   
                        }
                    }
                }
            }
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