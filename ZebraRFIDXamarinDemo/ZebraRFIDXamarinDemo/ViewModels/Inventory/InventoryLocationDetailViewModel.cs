using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Setting;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.ViewModels.Inventory
{
    public class InventoryLocationDetailViewModel : InventoryBaseViewModel
    {
        public Command LoadAssetCommand { get; }
        public Command SaveAssetCommand { get; }
        public Command FinalizeLocationCommand { get; }
        public Command PlanoPagamentoAlteradoCommand { get; }
        public Command PhysicalStatePickerCommand { get; }
        public Command ParamsPickerCommand { get; }
        public Command StatusSwitchCommand { get; }

        public ObservableCollection<InventoryDetail> listAsset = new ObservableCollection<InventoryDetail>();
        public ObservableCollection<InventoryDetail> ListAsset
        {
            get { return listAsset; }
            set { listAsset = value; }
        }

        public InventoryLocationDetailViewModel()
        {
            LoadAssetCommand = new Command(async () => await ExecuteLoadPersonCommand());
            SaveAssetCommand = new Command(OnSaveAsset);
            FinalizeLocationCommand = new Command(OnFinalizeLocation);

            PhysicalStatePickerItems = new ObservableCollection<PhysicalState>();
            ParamsPickerItems = new ObservableCollection<Params>();
            ListAsset = new ObservableCollection<InventoryDetail>();

            PlanoPagamentoAlteradoCommand = new Command<InventoryDetail>(WhenSelectedIndexChanged);
            PhysicalStatePickerCommand = new Command<object>(WhenPhysicalStateSelectedIndexChanged);
            ParamsPickerCommand = new Command<object>(WhenParamsSelectedIndexChanged);
            StatusSwitchCommand = new Command<object>(WhenStatusToggled);
            PhysicalStatePickerSelectedItem = new PhysicalState();
        }

        private ObservableCollection<PhysicalState> _physicalStatePickerItems;
        public ObservableCollection<PhysicalState> PhysicalStatePickerItems
        {
            get { return _physicalStatePickerItems; }
            set
            {
                _physicalStatePickerItems = value;
            }
        }

        private ObservableCollection<Params> _paramsPickerItems;
        public ObservableCollection<Params> ParamsPickerItems
        {
            get { return _paramsPickerItems; }
            set
            {
                _paramsPickerItems = value;
            }
        }

        private PhysicalState _physicalStatePickerSelectedItem;
        public PhysicalState PhysicalStatePickerSelectedItem
        {
            get { return _physicalStatePickerSelectedItem; }
            set
            {
                _physicalStatePickerSelectedItem = value;
            }
        }

        private Params _paramsPickerSelectedItem;
        public Params ParamsPickerSelectedItem
        {
            get { return _paramsPickerSelectedItem; }
            set
            {
                _paramsPickerSelectedItem = value;
            }
        }

        private int _physicalStatePickerSelectedIndex;
        public int PhysicalStatePickerSelectedIndex
        {
            get
            {
                return _physicalStatePickerSelectedIndex;
            }
            set
            {
                if (_physicalStatePickerSelectedIndex != value)
                {
                    _physicalStatePickerSelectedIndex = value;
                }
            }
        }

        private int _paramsPickerSelectedIndex;
        public int ParamsPickerSelectedIndex
        {
            get
            {
                return _paramsPickerSelectedIndex;
            }
            set
            {
                if (_paramsPickerSelectedIndex != value)
                {
                    _paramsPickerSelectedIndex = value;
                }
            }
        }

        private bool _statusSwitchToggled = false;
        public bool StatusSwitchToggled
        {
            get { return _statusSwitchToggled; }
            set { _statusSwitchToggled = value; }
        }

        async Task ExecuteLoadPersonCommand()
        {
            IsBusy = true;
            try
            {
                var paramsAll = await App.paramsRepositoty.GetAllAsync();
                var physicalStateAll = await App.physicalStateRepository.GetAllAsync();
                var inventoryDetailAllById = await App.inventoryDetailRepository.GetByIdLocation(LocationSync.Id);

                PhysicalStatePickerItems.Clear();
                for (int i = 0; i < physicalStateAll.Count(); i++)
                {
                    PhysicalStatePickerItems.Add(physicalStateAll[i]);
                }

                ParamsPickerItems.Clear();
                for (int i = 0; i < paramsAll.Count(); i++)
                {
                    ParamsPickerItems.Add(paramsAll[i]);
                }

                /*
                ListAsset.Clear();
                for (int i = 0; i < LocationSync.DetalleInventario.Count(); i++)
                {
                    ListAsset.Add(LocationSync.DetalleInventario[i]);
                }
                */
                ListAsset.Clear();
                LocationSync.DetalleInventario = inventoryDetailAllById;
                for (int i = 0; i < LocationSync.DetalleInventario.Count(); i++)
                {
                    ListAsset.Add(LocationSync.DetalleInventario[i]);
                }
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

        public async void OnAppearing()
        {
            IsBusy = true;

            var paramsAll = await App.paramsRepositoty.GetAllAsync();
            var physicalStateAll = await App.physicalStateRepository.GetAllAsync();
            var inventoryDetailAllById = await App.inventoryDetailRepository.GetByIdLocation(LocationSync.Id);

            PhysicalStatePickerItems.Clear();
            for (int i = 0; i < physicalStateAll.Count(); i++)
            {
                PhysicalStatePickerItems.Add(physicalStateAll[i]);
            }

            ParamsPickerItems.Clear();
            for (int i = 0; i < paramsAll.Count(); i++)
            {
                ParamsPickerItems.Add(paramsAll[i]);
            }

            /*
            ListAsset.Clear();
            for (int i = 0; i < LocationSync.DetalleInventario.Count(); i++)
            {
                ListAsset.Add(LocationSync.DetalleInventario[i]);
            }
            */
            ListAsset.Clear();
            LocationSync.DetalleInventario = inventoryDetailAllById;
            for (int i = 0; i < LocationSync.DetalleInventario.Count(); i++)
            {
                ListAsset.Add(LocationSync.DetalleInventario[i]);
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
                                if (itemDetalleInventario.Activo.EstadoFisicoId == null)
                                {
                                    // await Application.Current.MainPage.DisplayAlert("Advertencia", "Ingrese el estado fisico", "Aceptar");
                                    return;
                                }


                                if (itemDetalleInventario.Activo.Observaciones == null || itemDetalleInventario.Activo.Observaciones == "")
                                {
                                    // await Application.Current.MainPage.DisplayAlert("Advertencia", "Ingrese las observaciones", "Aceptar");
                                    return;
                                }

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
                                asset.Status = itemDetalleInventario.Activo.Status; // Este campo se va a modificar
                                await App.assetRepository.UpdateAsync(asset);
                                // await Application.Current.MainPage.DisplayAlert("Mensaje", "La información se actualizó correctamente.", "Aceptar");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // await ExecuteLoadPersonCommand();
            await Shell.Current.GoToAsync("..");
            await Application.Current.MainPage.DisplayAlert("Mensaje", "La información se actualizó correctamente.", "Aceptar");
        }

        private async void WhenSelectedIndexChanged(InventoryDetail inventoryDetail)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ZebraRFIDXamarinDemo.Views.Inventory.InventoryLocationEdit(inventoryDetail));
        }

        private async void WhenPhysicalStateSelectedIndexChanged(object sender)
        {
            var picker = (InventoryDetail)sender;
            if (picker != null)
            {
                if (picker.Activo != null)
                {
                    if (picker.Activo.EstadoFisicoId != null)
                    {
                        var index = PhysicalStatePickerSelectedIndex;
                        var item = PhysicalStatePickerSelectedItem = PhysicalStatePickerItems.FirstOrDefault(f => f.Id == picker.Activo.EstadoFisicoId);
                        if (item.Id != null)
                        {
                            var data = ListAsset.FirstOrDefault(f => f.Id == picker.Id);
                            if (data != null)
                            {
                                if (picker.Activo.EstadoFisicoId != null)
                                {
                                    data.Activo.EstadoFisicoId = PhysicalStatePickerItems[index].Id;
                                }
                            }
                        }
                    }
                }
            }
        }

        private async void WhenParamsSelectedIndexChanged(object sender)
        {
            var picker = (InventoryDetail)sender;
            if (picker != null)
            {
                if (picker.Activo != null)
                {
                    if (picker.Activo.MotivoId != null)
                    {
                        var index = ParamsPickerSelectedIndex;
                        var item = ParamsPickerSelectedItem = ParamsPickerItems.FirstOrDefault(f => f.Id == picker.Activo.MotivoId);
                    }
                }
            }
        }

        private async void WhenStatusToggled(object sender)
        {
            /*
            var switchToggled = StatusSwitchToggled;
            var toggled = (InventoryDetail)sender;
            if (toggled != null)
            {
              
                var data = ListAsset.FirstOrDefault(f => f.Id == toggled.Value);
                if (data != null)
                {
                   // data.Activo.Status = toggled.Va.Status;
                }
                
            }*/

        }

        private async void OnFinalizeLocation()
        {
            try
            {
                var data = ListAsset;
                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        if (item.UbicacionId != null)
                        {
                            var dataLocationSQLITE = await App.locationRepository.GetByIdAsync((Guid)item.UbicacionId);
                            if (dataLocationSQLITE != null)
                            {
                                if (dataLocationSQLITE.Status == 1)
                                {
                                    Location dataLocation = new Location();
                                    dataLocation.Id = dataLocationSQLITE.Id;
                                    dataLocation.Nombre = dataLocationSQLITE.Nombre;
                                    dataLocation.Status = 2;

                                    await App.locationRepository.UpdateAsync(dataLocation);
                                    await Application.Current.MainPage.DisplayAlert("Mensaje", "La ubicación se ha finalizado correctamente", "Aceptar");
                                    await Shell.Current.GoToAsync("../../");
                                }
                                else
                                {
                                    await Application.Current.MainPage.DisplayAlert("Advertencia", "No hay ubicaciones disponibles para finalizar en este momento.", "Aceptar");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}