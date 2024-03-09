using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.ViewModels.Inventory
{
    public class InventoryLocationDetailViewModel : InventoryBaseViewModel
    {
        public Command SaveAssetCommand { get; }
        // public List<PhysicalState> PhysicalStateData { get; set; }

        public PhysicalState ReasonValue { get; set; }
        public ObservableCollection<PhysicalState> PhysicalStateData = new ObservableCollection<PhysicalState>();

        public InventoryLocationDetailViewModel()
        {
            SaveAssetCommand = new Command(OnSaveAsset);
            InventorySync = new Models.Startup.Inventory();
            // PhysicalStateData = new List<PhysicalState>();
            PhysicalStateData = new ObservableCollection<PhysicalState>();
            // ReasonValue.Id = new Guid("4a7b5ca4-cc71-49c6-9466-f1039c3ffc07");
        }

        public ObservableCollection<PhysicalState> ShowResponseReason
        {
            get => PhysicalStateData;
            set
            {
                PhysicalStateData = value;
                OnPropertyChanged(nameof(ShowResponseReason));
            }
        }

        public async void OnAppearing()
        {
            IsBusy = true;

            PhysicalStateData.Clear();
            var physicalStateAll = await App.physicalStateRepository.GetAllAsync();
            foreach (var item in physicalStateAll)
            {
                PhysicalStateData.Add(item);
            }

            ShowResponseReason = PhysicalStateData;
        }

        private async void OnSaveAsset()
        {
            var asset = InventorySync;
            await Shell.Current.GoToAsync("..");
        }

            void SelectedAssetChanged(object sender, EventArgs e)
        {
            if (sender is Picker picker)
            {

            }
                //int key = ((KeyValuePair<int, string>)picker.SelectedItem).Key;
        }
    }
}