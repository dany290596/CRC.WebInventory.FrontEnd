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
        public Command GetAssetsInventoriedCommand { get; }
        public Command GetAssetsWithoutInventoryCommand { get; }
        public Command FinalizeLocationCommand { get; }
        public Command PlanoPagamentoAlteradoCommand { get; }
        public Command PhysicalStatePickerCommand { get; }
        public Command ParamsPickerCommand { get; }
        public Command StatusSwitchCommand { get; }

        public bool _getAssetsInventoried;
        public bool GetAssetsInventoried
        {
            get { return _getAssetsInventoried; }
            set { SetProperty(ref _getAssetsInventoried, value); }
        }
        public bool _getAssetsWithoutInventory;
        public bool GetAssetsWithoutInventory
        {
            get { return _getAssetsWithoutInventory; }
            set { SetProperty(ref _getAssetsWithoutInventory, value); }
        }

        public ObservableCollection<Asset> listAsset = new ObservableCollection<Asset>();
        public ObservableCollection<Asset> ListAsset
        {
            get { return listAsset; }
            set { listAsset = value; }
        }

        public string listAssetCount = "Activos inventariados: 0";
        public string ListAssetCount
        {
            get { return listAssetCount; }
            set { SetProperty(ref listAssetCount, value); }
        }

        public ObservableCollection<Asset> listAssetInventariado = new ObservableCollection<Asset>();
        public ObservableCollection<Asset> ListAssetInventariado
        {
            get { return listAssetInventariado; }
            set { listAssetInventariado = value; }
        }

        public string listAssetInventariadoCount = "Activos no inventariados: 0";
        public string ListAssetInventariadoCount
        {
            get { return listAssetInventariadoCount; }
            set { SetProperty(ref listAssetInventariadoCount, value); }
        }

        public InventoryLocationDetailViewModel()
        {
            LoadAssetCommand = new Command(async () => await ExecuteLoadPersonCommand());
            SaveAssetCommand = new Command(OnSaveAsset);
            FinalizeLocationCommand = new Command(OnFinalizeLocation);

            GetAssetsInventoriedCommand = new Command(OnGetAssetsInventoried);
            GetAssetsWithoutInventoryCommand = new Command(OnGetAssetsWithoutInventory);

            PhysicalStatePickerItems = new ObservableCollection<PhysicalState>();
            ParamsPickerItems = new ObservableCollection<Params>();
            ListAsset = new ObservableCollection<Asset>();
            ListAssetInventariado = new ObservableCollection<Asset>();

            PlanoPagamentoAlteradoCommand = new Command<InventoryDetail>(WhenSelectedIndexChanged);
            PhysicalStatePickerCommand = new Command<object>(WhenPhysicalStateSelectedIndexChanged);
            ParamsPickerCommand = new Command<object>(WhenParamsSelectedIndexChanged);
            StatusSwitchCommand = new Command<object>(WhenStatusToggled);
            PhysicalStatePickerSelectedItem = new PhysicalState();

            GetAssetsInventoried = false;
            GetAssetsWithoutInventory = true;
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

                ListAsset.Clear();
                ListAssetInventariado.Clear();
                if (InventoryLocationSync.UbicacionId != null && InventoryLocationSync.InventarioId != null)
                {
                    var dataSQLITEInventory = await App.inventoryRepository.GetByLocationIdAsync(InventoryLocationSync.UbicacionId, InventoryLocationSync.InventarioId);
                    if (dataSQLITEInventory.Activo.Count() > 0)
                    {
                        for (int i = 0; i < dataSQLITEInventory.Activo.Count(); i++)
                        {
                            if (dataSQLITEInventory.Activo[i].Status == true)
                            {
                                ListAsset.Add(dataSQLITEInventory.Activo[i]);
                                ListAssetCount = "Activos inventariados: " + ListAsset.Count();
                            }
                            if (dataSQLITEInventory.Activo[i].Status == false)
                            {
                                ListAssetInventariado.Add(dataSQLITEInventory.Activo[i]);
                                ListAssetInventariadoCount = "Activos no inventariados: " + ListAssetInventariado.Count();
                            }
                        }
                    }
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
            //var inventoryDetailAllById = await App.inventoryDetailRepository.GetByIdLocation(LocationSync.Id);

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
            ListAssetInventariado.Clear();
            if (InventoryLocationSync.UbicacionId != null && InventoryLocationSync.InventarioId != null)
            {
                var dataSQLITEInventory = await App.inventoryRepository.GetByLocationIdAsync(InventoryLocationSync.UbicacionId, InventoryLocationSync.InventarioId);
                if (dataSQLITEInventory.Activo.Count() > 0)
                {
                    for (int i = 0; i < dataSQLITEInventory.Activo.Count(); i++)
                    {
                        if (dataSQLITEInventory.Activo[i].Status == true)
                        {
                            ListAsset.Add(dataSQLITEInventory.Activo[i]);
                            ListAssetCount = "Activos inventariados: " + ListAsset.Count();
                        }
                        if (dataSQLITEInventory.Activo[i].Status == false)
                        {
                            ListAssetInventariado.Add(dataSQLITEInventory.Activo[i]);
                            ListAssetInventariadoCount = "Activos no inventariados: " + ListAssetInventariado.Count();
                        }
                    }
                }
            }
        }

        private async void OnSaveAsset()
        {
            try
            {
                var dataAssetInventariado = ListAssetInventariado;
                if (dataAssetInventariado != null)
                {
                    if (dataAssetInventariado.Count() > 0)
                    {
                        foreach (var itemAsset in dataAssetInventariado)
                        {
                            if (itemAsset != null)
                            {
                                var dataAssetSQLITE = await App.assetRepository.GetByIdAsync(itemAsset.Id);
                                if (dataAssetSQLITE != null)
                                {
                                    if (itemAsset.EstadoFisicoId == null)
                                    {
                                        // await Application.Current.MainPage.DisplayAlert("Advertencia", "Ingrese el estado fisico", "Aceptar");
                                        return;
                                    }


                                    if (itemAsset.Observaciones == null || itemAsset.Observaciones == "")
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
                                    asset.Observaciones = itemAsset.Observaciones; // Este campo se va a modificar
                                    asset.EstadoFisicoId = itemAsset.EstadoFisicoId; // Este campo se va a modificar
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
                                    asset.Status = itemAsset.Status; // Este campo se va a modificar
                                    if (itemAsset.MotivoId != null)
                                    {
                                        asset.MotivoId = itemAsset.MotivoId; // Este campo se va a modificar
                                    }
                                    await App.assetRepository.UpdateAsync(asset);
                                    // await Application.Current.MainPage.DisplayAlert("Mensaje", "La información se actualizó correctamente.", "Aceptar");
                                }
                            }
                        }
                    }
                }

                var dataAsset = ListAsset;
                if (dataAsset != null)
                {
                    if (dataAsset.Count() > 0)
                    {
                        foreach (var itemAsset in dataAsset)
                        {
                            if (itemAsset != null)
                            {
                                var dataAssetSQLITE = await App.assetRepository.GetByIdAsync(itemAsset.Id);
                                if (dataAssetSQLITE != null)
                                {
                                    if (itemAsset.EstadoFisicoId == null)
                                    {
                                        // await Application.Current.MainPage.DisplayAlert("Advertencia", "Ingrese el estado fisico", "Aceptar");
                                        return;
                                    }


                                    if (itemAsset.Observaciones == null || itemAsset.Observaciones == "")
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
                                    asset.Observaciones = itemAsset.Observaciones; // Este campo se va a modificar
                                    asset.EstadoFisicoId = itemAsset.EstadoFisicoId; // Este campo se va a modificar
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
                                    asset.Status = itemAsset.Status; // Este campo se va a modificar
                                    if (itemAsset.MotivoId != null)
                                    {
                                        asset.MotivoId = itemAsset.MotivoId; // Este campo se va a modificar
                                    }
                                    await App.assetRepository.UpdateAsync(asset);
                                    // await Application.Current.MainPage.DisplayAlert("Mensaje", "La información se actualizó correctamente.", "Aceptar");
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
            try
            {
                if (ListAssetInventariado != null)
                {
                    if (ListAssetInventariado.Count() > 0)
                    {
                        var picker = (Asset)sender;
                        if (picker != null)
                        {
                            if (picker != null)
                            {
                                if (picker.EstadoFisicoId != null)
                                {
                                    var index = PhysicalStatePickerSelectedIndex;
                                    var item = PhysicalStatePickerSelectedItem = PhysicalStatePickerItems.FirstOrDefault(f => f.Id == picker.EstadoFisicoId);
                                    if (item.Id != null)
                                    {
                                        var data = ListAssetInventariado.FirstOrDefault(f => f.Id == picker.Id);
                                        if (data != null)
                                        {
                                            if (picker.EstadoFisicoId != null)
                                            {
                                                data.EstadoFisicoId = PhysicalStatePickerItems[index].Id;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (ListAsset != null)
                {
                    if (ListAsset.Count() > 0)
                    {
                        var picker = (Asset)sender;
                        if (picker != null)
                        {
                            if (picker != null)
                            {
                                if (picker.EstadoFisicoId != null)
                                {
                                    var index = PhysicalStatePickerSelectedIndex;
                                    var item = PhysicalStatePickerSelectedItem = PhysicalStatePickerItems.FirstOrDefault(f => f.Id == picker.EstadoFisicoId);
                                    if (item.Id != null)
                                    {
                                        var data = ListAsset.FirstOrDefault(f => f.Id == picker.Id);
                                        if (data != null)
                                        {
                                            if (picker.EstadoFisicoId != null)
                                            {
                                                data.EstadoFisicoId = PhysicalStatePickerItems[index].Id;
                                            }
                                        }
                                    }
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

        private async void WhenParamsSelectedIndexChanged(object sender)
        {
            try
            {
                if (ListAssetInventariado != null)
                {
                    if (ListAssetInventariado.Count() > 0)
                    {
                        var picker = (Asset)sender;
                        if (picker != null)
                        {
                            if (picker != null)
                            {
                                if (picker.MotivoId != null)
                                {
                                    var index = ParamsPickerSelectedIndex;
                                    var item = ParamsPickerSelectedItem = ParamsPickerItems.FirstOrDefault(f => f.Id == picker.MotivoId);
                                    if (item.Id != null)
                                    {
                                        var data = ListAssetInventariado.FirstOrDefault(f => f.Id == picker.Id);
                                        if (data != null)
                                        {
                                            if (picker.MotivoId != null)
                                            {
                                                data.MotivoId = ParamsPickerItems[index].Id;
                                            }
                                        }
                                    }
                                }
                                if (picker.MotivoId == null)
                                {
                                    var item = ParamsPickerSelectedItem;
                                    if (item != null)
                                    {
                                        var itemData = ParamsPickerItems.FirstOrDefault(f => f.Id == item.Id);
                                        if (itemData != null)
                                        {
                                            var index = ParamsPickerSelectedIndex;
                                            var data = ListAssetInventariado.FirstOrDefault(f => f.Id == picker.Id);
                                            if (data != null)
                                            {
                                                data.MotivoId = ParamsPickerItems[index].Id;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (ListAsset != null)
                {
                    if (ListAsset.Count() > 0)
                    {
                        var picker = (Asset)sender;
                        if (picker != null)
                        {
                            if (picker != null)
                            {
                                if (picker.MotivoId != null)
                                {
                                    var index = ParamsPickerSelectedIndex;
                                    var item = ParamsPickerSelectedItem = ParamsPickerItems.FirstOrDefault(f => f.Id == picker.MotivoId);
                                    if (item.Id != null)
                                    {
                                        var data = ListAsset.FirstOrDefault(f => f.Id == picker.Id);
                                        if (data != null)
                                        {
                                            if (picker.MotivoId != null)
                                            {
                                                data.MotivoId = ParamsPickerItems[index].Id;
                                            }
                                        }
                                    }
                                }
                                if (picker.MotivoId == null)
                                {
                                    var item = ParamsPickerSelectedItem;
                                    if (item != null)
                                    {
                                        var itemData = ParamsPickerItems.FirstOrDefault(f => f.Id == item.Id);
                                        if (itemData != null)
                                        {
                                            var index = ParamsPickerSelectedIndex;
                                            var data = ListAsset.FirstOrDefault(f => f.Id == picker.Id);
                                            if (data != null)
                                            {
                                                data.MotivoId = ParamsPickerItems[index].Id;
                                            }
                                        }
                                    }
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

        private async void WhenStatusToggled(object sender)
        {
        }

        private async void OnFinalizeLocation()
        {
            try
            {
                var countData = ListAsset.Count();
                var data = ListAsset.Where(w => w.MotivoId != null || w.Status == true).ToList();
                if (countData == data.Count())
                {
                    var dataSQLITE = await App.inventoryLocationRepository.GetByFilter(InventoryLocationSync.InventarioId, InventoryLocationSync.UbicacionId);
                    foreach (var item in dataSQLITE)
                    {
                        InventoryLocation inventoryLocation = new InventoryLocation();
                        inventoryLocation.Id = item.Id;
                        inventoryLocation.InventarioId = item.InventarioId;
                        inventoryLocation.UbicacionId = item.UbicacionId;
                        inventoryLocation.Status = item.Status = 2;
                        await App.inventoryLocationRepository.UpdateAsync(inventoryLocation);
                    }
                    await Application.Current.MainPage.DisplayAlert("Mensaje", "La ubicación se ha finalizado correctamente", "Aceptar");
                    await Shell.Current.GoToAsync("..");
                    /*
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
                    */
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", "Todos los activos deben de contar con un motivo o un estatus activo.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void OnGetAssetsInventoried(object sender)
        {
            try
            {
                GetAssetsInventoried = true;
                GetAssetsWithoutInventory = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void OnGetAssetsWithoutInventory(object sender)
        {
            try
            {
                GetAssetsInventoried = false;
                GetAssetsWithoutInventory = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}