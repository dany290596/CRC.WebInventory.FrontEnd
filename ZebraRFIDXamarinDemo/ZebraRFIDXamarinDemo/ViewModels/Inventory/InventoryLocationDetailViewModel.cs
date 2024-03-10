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
            try
            {
                var dataLocation = LocationSync;
                if (dataLocation.DetalleInventario.Count() > 0)
                {
                    foreach (var itemDetalleInventario in dataLocation.DetalleInventario)
                    {
                        if (itemDetalleInventario.Activo != null)
                        {
                            var dataAssetSQLITE = await App.assetRepository.GetByIdAsync(itemDetalleInventario.Activo.Id);
                            if (dataAssetSQLITE != null)
                            {
                                Asset asset = new Asset();
                                asset.UsuarioCreadorId = dataAssetSQLITE.UsuarioCreadorId;
                                asset.UsuarioModificadorId = dataAssetSQLITE.UsuarioModificadorId;
                                asset.UsuarioBajaId = dataAssetSQLITE.UsuarioBajaId;
                                asset.UsuarioReactivadorId = dataAssetSQLITE.UsuarioReactivadorId;
                                asset.FechaCreacion = dataAssetSQLITE.FechaCreacion;
                                asset.FechaModificacion = dataAssetSQLITE.FechaModificacion;
                                asset.FechaBaja = dataAssetSQLITE.FechaBaja;
                                asset.FechaReactivacion = dataAssetSQLITE.FechaReactivacion;
                                asset.Estado = dataAssetSQLITE.Estado;
                                asset.EmpresaId = dataAssetSQLITE.EmpresaId;
                                asset.Id = dataAssetSQLITE.Id;
                                asset.UbicacionId = dataAssetSQLITE.UbicacionId;
                                asset.GrupoActivoId = dataAssetSQLITE.GrupoActivoId;
                                asset.TipoActivoId = dataAssetSQLITE.TipoActivoId;
                                asset.Codigo = dataAssetSQLITE.Codigo;
                                asset.Serie = dataAssetSQLITE.Serie;
                                asset.Marca = dataAssetSQLITE.Marca;
                                asset.Modelo = dataAssetSQLITE.Modelo;
                                asset.Descripcion = dataAssetSQLITE.Descripcion;
                                asset.Nombre = dataAssetSQLITE.Nombre;
                                asset.Observaciones = itemDetalleInventario.Activo.Observaciones; // Este campo se va a modificar
                                asset.EstadoFisicoId = itemDetalleInventario.Activo.EstadoFisicoId; // Este campo se va a modificar
                                asset.TagId = dataAssetSQLITE.TagId;
                                asset.ColaboradorHabitualId = dataAssetSQLITE.ColaboradorHabitualId;
                                asset.ColaboradorResponsableId = dataAssetSQLITE.ColaboradorResponsableId;
                                asset.ValorCompra = dataAssetSQLITE.ValorCompra;
                                asset.FechaCompra = dataAssetSQLITE.FechaCompra;
                                asset.Proveedor = dataAssetSQLITE.Proveedor;
                                asset.FechaFinGarantia = dataAssetSQLITE.FechaFinGarantia;
                                asset.TieneFoto = dataAssetSQLITE.TieneFoto;
                                asset.TieneArchivo = dataAssetSQLITE.TieneArchivo;
                                asset.FechaCapitalizacion = dataAssetSQLITE.FechaCapitalizacion;
                                asset.FichaResguardo = dataAssetSQLITE.FichaResguardo;
                                asset.CampoLibre1 = dataAssetSQLITE.CampoLibre1;
                                asset.CampoLibre2 = dataAssetSQLITE.CampoLibre2;
                                asset.CampoLibre3 = dataAssetSQLITE.CampoLibre3;
                                asset.CampoLibre4 = dataAssetSQLITE.CampoLibre4;
                                asset.CampoLibre5 = dataAssetSQLITE.CampoLibre5;
                                asset.AreaId = dataAssetSQLITE.AreaId;
                                await App.assetRepository.UpdateAsync(asset);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

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