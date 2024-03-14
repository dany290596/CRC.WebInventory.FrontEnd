using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.ViewModels.Inventory;

namespace ZebraRFIDXamarinDemo.Views.Inventory
{
    public partial class InventoryLocationEdit : ContentPage
    {
        public Models.Startup.InventoryDetail InventoryDetailSync { get; set; }
        InventoryLocationEditViewModel inventoryLocationEditViewModel;

        public ObservableCollection<PhysicalState> _pickerItemss;
        public ObservableCollection<PhysicalState> PickerItemss
        {
            get { return _pickerItemss; }
            set
            {
                _pickerItemss = value;
            }
        }

        public InventoryLocationEdit(Models.Startup.InventoryDetail inventoryDetail)
        {
            InventoryDetailSync = inventoryDetail;
            PickerItemss = new ObservableCollection<PhysicalState>();
            InitializeComponent();
            BindingContext = new InventoryLocationEditViewModel();
            if (inventoryDetail != null)
            {
                ((InventoryLocationEditViewModel)BindingContext).InventoryDetailSync = inventoryDetail;
            }
        }

        protected async override void OnAppearing()
        {
            var physicalStateAll = await App.physicalStateRepository.GetAllAsync();

            for (int i = 0; i < physicalStateAll.Count(); i++)
            {
                PickerItemss.Add(physicalStateAll[i]);
                PickerSelectedIndexx.ItemsSource = PickerItemss;
            }

            for (int i = 0; i < physicalStateAll.Count(); i++)
            {
                if (InventoryDetailSync != null)
                {
                    if (InventoryDetailSync.Activo != null)
                    {
                        if (physicalStateAll[i].Id == InventoryDetailSync.Activo.EstadoFisicoId)
                        {
                            PickerSelectedIndexx.SelectedIndex = i;
                        }
                    }
                }
            }
        }

        void SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var picker = sender as Picker;
            if (picker.SelectedItem != null)
            {
                var data = picker.SelectedItem as PhysicalState;
                if (data.Id != null) {
                    InventoryDetailSync.Activo.EstadoFisicoId = data.Id;
                }
            }
        }
    }
}