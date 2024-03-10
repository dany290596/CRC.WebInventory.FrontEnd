using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.ViewModels.Inventory
{
    public class InventoryLocationDetailViewModel : InventoryBaseViewModel
    {
        public ObservableCollection<object> StartValue
        {
            get;
            set;
        }

        public Command SaveAssetCommand { get; }
        // public List<PhysicalState> PhysicalStateData { get; set; }

        PhysicalState selectedItem;
        PhysicalState selectedFilterItem;

        public PhysicalState ReasonValue { get; set; }
        Guid selectedFilter;

        //selectedFilter. new Guid("4a7b5ca4-cc71-49c6-9466-f1039c3ffc07");


        public ObservableCollection<PhysicalState> PhysicalStateData = new ObservableCollection<PhysicalState>();

        public InventoryLocationDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            StartValue = new ObservableCollection<object>();
            SaveAssetCommand = new Command(OnSaveAsset);
            //LocationSync = new Location();
            // PhysicalStateData = new List<PhysicalState>();
            PhysicalStateData = new ObservableCollection<PhysicalState>();
            // ReasonValue.Id = new Guid("4a7b5ca4-cc71-49c6-9466-f1039c3ffc07");

            /*
            selectedFilter = new PhysicalState
            {
                Nombre = "A300",
                Id = new Guid("4a7b5ca4-cc71-49c6-9466-f1039c3ffc07")
            };
            */
            //selectedFilter = "A300";

            selectedItem = new PhysicalState
            {
                Nombre = "A300",
                Id = new Guid("4a7b5ca4-cc71-49c6-9466-f1039c3ffc07")
            };

            selectedFilter = new Guid("4a7b5ca4-cc71-49c6-9466-f1039c3ffc07");

            selectedFilterItem = new PhysicalState
            {
                Nombre = "A300",
                Id = new Guid("4a7b5ca4-cc71-49c6-9466-f1039c3ffc07")
            };

            // pickerSelectedIndex = 3;
        }

        public PhysicalState SelectedFilterItem
        {
            get
            {
                return selectedFilterItem;
            }
            set
            {
                if (selectedFilterItem != value)
                {
                    selectedFilterItem = value;
                    this.RaisedOnPropertyChanged("SelectedFilterItem");
                }
                //if (SetProperty(ref selectedFilter, value))
                //    FilterItems();
            }
        }

        public Guid SelectedFilter
        {
            get
            {
                return selectedFilter;
            }
            set
            {
                if (selectedFilter != value)
                {
                    selectedFilter = value;
                    this.RaisedOnPropertyChanged("SelectedFilter");
                }
                //if (SetProperty(ref selectedFilter, value))
                //    FilterItems();
            }
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

        private ObservableCollection<PhysicalState> pickerItems;

        public ObservableCollection<PhysicalState> PickerItems
        {
            get { return pickerItems; }
            set { pickerItems = value; }
        }



        private int pickerSelectedIndex;

        public int PickerSelectedIndex
        {
            get
            {
                return pickerSelectedIndex;
            }
            set
            {
                if (pickerSelectedIndex != value)
                {
                    pickerSelectedIndex = value;
                }
            }
        }


        public async void OnAppearing()
        {
            IsBusy = true;
            var physicalStateAll = await App.physicalStateRepository.GetAllAsync();
            /*
            PhysicalStateData.Clear();
            var physicalStateAll = await App.physicalStateRepository.GetAllAsync();
            foreach (var item in physicalStateAll)
            {
                PhysicalStateData.Add(item);
                //if (item.Id == selectedFilter)
                //  PhysicalStateData.SelectedIndex = item.Id;
            }

            ShowResponseReason = PhysicalStateData;
            //FilterItems();

            StartValue.Add(selectedItem);
            */

            string myEstadoFisico = "A300";

            PickerItems = new ObservableCollection<PhysicalState>();
            for (int i = 0; i < physicalStateAll.Count(); i++)
            {
                /*
                if (PickerItems[i].Nombre == myEstadoFisico)
                {
                    EstadoFisicoPicker.SelectedIndex = i;
                }
                */

                PickerItems.Add(physicalStateAll[i]);
            }

            foreach (var itemInventoryDetail in LocationSync.DetalleInventario)
            {
                for (int i = 0; i < physicalStateAll.Count(); i++)
                {
                    if (PickerItems[i].Id == itemInventoryDetail.EstadoFisicoId)
                    {
                        pickerSelectedIndex = i;
                    }
                }
            }

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

        /*
        void FilterItems()
        {
            PhysicalStateData.Where(a => a.Id == SelectedFilter.Id);
        }
        */


        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }
    }
}