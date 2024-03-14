using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.ViewModels.Inventory
{
    public class InventoryLocationEditViewModel : InventoryBaseViewModel
    {
        public ObservableCollection<PhysicalState> _pickerItemss;
        public ObservableCollection<PhysicalState> PickerItemss
        {
            get { return _pickerItemss; }
            set
            {
                _pickerItemss = value;
            }
        }

        private int _pickerSelectedIndexx;
        public int PickerSelectedIndexx
        {
            get
            {
                return _pickerSelectedIndexx;
            }
            set
            {
                if (_pickerSelectedIndexx != value)
                {
                    _pickerSelectedIndexx = value;
                    //OnPropertyChanged(nameof(PickerSelectedIndex));
                }
            }
        }

        public Command SaveAssetCommand { get; }
        public Command CancelAssetCommand { get; }
        private PhysicalState _pickerSelected;
        public PhysicalState PickerSelected
        {
            get
            {
                return _pickerSelected;
            }
            set
            {
                if (_pickerSelected != value)
                {
                    _pickerSelected = value;
                }
            }
        }

        public InventoryLocationEditViewModel()
        {
            InventoryDetailSync = new InventoryDetail();
            //EstadoFisicoId = InventoryDetailSync.Activo.EstadoFisicoId;
            PickerItemss = new ObservableCollection<PhysicalState>();
            SaveAssetCommand = new Command(OnSaveAsset);
            CancelAssetCommand = new Command(OnCancelAsset);
            _pickerSelectedIndexx = 1;

            PropertyChanged += (_, __) => SaveAssetCommand.ChangeCanExecute();
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            var physicalStateAll = await App.physicalStateRepository.GetAllAsync();

            for (int i = 0; i < physicalStateAll.Count(); i++)
            {
                /*
                if (PickerItems[i].Nombre == myEstadoFisico)
                {
                    EstadoFisicoPicker.SelectedIndex = i;
                }
                */
                PickerItemss.Add(physicalStateAll[i]);
            }

            for (int i = 0; i < physicalStateAll.Count(); i++)
            {
                /*
                if (PickerItems[i].Nombre == myEstadoFisico)
                {
                    EstadoFisicoPicker.SelectedIndex = i;
                }
                */

                if (InventoryDetailSync != null)
                {
                    if (PickerItemss[i].Id == InventoryDetailSync.Activo.EstadoFisicoId)
                    {
                        _pickerSelectedIndexx = i;
                    }
                }
            }

            /*
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
            */
        }


        async Task ExecuteLoadPersonCommand()
        {
            IsBusy = true;
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                IsBusy = false;

            }
        }
        private async void OnSaveAsset()
        {
            var asset = InventoryDetailSync.Activo;
            if (asset.EstadoFisicoId == null)
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Ingrese el estado fisico", "Aceptar");
                return;
            }

            if (asset.Observaciones == null)
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Ingrese las observaciones", "Aceptar");
                return;
            }

            if (asset.EstadoFisicoId != null && asset.Observaciones != null && asset.Observaciones != "")
            {
                await App.assetRepository.UpdateAsync(asset);
                await Shell.Current.GoToAsync("../../../");
            }
        }

        private async void OnCancelAsset()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}