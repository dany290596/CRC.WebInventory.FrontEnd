using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using ZebraRFIDXamarinDemo.Repositories.Implements;
using System.Linq;
using Location = ZebraRFIDXamarinDemo.Models.Startup.Location;
using InventoryDetail = ZebraRFIDXamarinDemo.Models.Startup.InventoryDetail;
using ZebraRFIDXamarinDemo.Models.Startup;
using Android.Service.Autofill;

namespace ZebraRFIDXamarinDemo.ViewModels.Inventory
{
    public class InventoryIndexViewModel : InventoryBaseViewModel
    {
        DeviceRepository _deviceRepository = new DeviceRepository();

        public Xamarin.Forms.Command LoadInventoryCommand { get; }
        public Xamarin.Forms.Command DetailInventoryCommand { get; }
        public Command SyncInventoryCommand { get; }
        public Command UploadInventoryCommand { get; }
        public ObservableCollection<Models.Startup.Inventory> InventoryData { get; }

        public InventoryIndexViewModel(INavigation _navigation)
        {
            LoadInventoryCommand = new Command(async () => await ExecuteLoadPersonCommand());
            InventoryData = new ObservableCollection<Models.Startup.Inventory>();
            DetailInventoryCommand = new Command<Models.Startup.Inventory>(OnDetailInventory);
            SyncInventoryCommand = new Command(OnSyncInventoryCommand);
            UploadInventoryCommand = new Command(OnUploadInventoryCommand);
            Navigation = _navigation;
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        /*
        public async void OnAppearing()
        {
            IsBusy = true;
            var listDevice = await App.deviceRepository.GetAllAsync();
            if (listDevice.Count() == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Mensaje", "Ingrese el identificador del dispositivo en la sección de configuración para sincronizar inventarios", "Aceptar");
            }
            else
            {
                InventoryData.Clear();
                var token = Preferences.Get("token", "default_value");
                var company = Preferences.Get("company", "default_value");

                var device = await App.deviceRepository.GetByLastOrDefaultAsync();
                var inventoryList = await _deviceRepository.InventorySync(token, company, device.Id);
                if (inventoryList.Data != null)
                {
                    foreach (var item in inventoryList.Data)
                    {
                        InventoryData.Add(item);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Mensaje", "No hay inventarios disponibles en este momento", "Aceptar");
                }
            }
        }
        */

        async Task ExecuteLoadPersonCommand()
        {
            IsBusy = true;
            try
            {
                /*
                var listDevice = await App.deviceRepository.GetAllAsync();
                if (listDevice.Count() == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Mensaje", "Ingrese el identificador del dispositivo en la sección de configuración para sincronizar inventarios", "Aceptar");
                }
                else
                {
                    InventoryData.Clear();
                    var token = Preferences.Get("token", "default_value");
                    var company = Preferences.Get("company", "default_value");

                    var device = await App.deviceRepository.GetByLastOrDefaultAsync();
                    var inventoryList = await App.deviceRepository.InventorySync(token, company, device.Id);
                    foreach (var item in inventoryList.Data)
                    {
                        InventoryData.Add(item);
                    }
                }
                */
                InventoryData.Clear();
                var inventoryAllSQLITE = await App.inventoryRepository.GetAllAsync();
                if (inventoryAllSQLITE.Count() > 0)
                {
                    foreach (var item in inventoryAllSQLITE)
                    {
                        InventoryData.Add(item);
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

        private async void OnDetailInventory(Models.Startup.Inventory inventorySync)
        {
            await Navigation.PushAsync(new Views.Inventory.InventoryDetail(inventorySync));
        }

        private async void OnSyncInventoryCommand()
        {
            try
            {
                var list = await App.settingRepository.GetAllAsync();
                if (list.Count() == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Mensaje", "Ingrese el identificador del dispositivo en la sección de configuración para sincronizar inventarios", "Aceptar");
                }
                else
                {
                    var settingGetByLasSQLITE = await App.settingRepository.GetByLastOrDefaultAsync();
                    var userInformationGetByLasSQLITE = await App.userInformationRepository.GetByLastOrDefaultAsync();
                    var inventoryList = await App.settingRepository.GetInventorySync(userInformationGetByLasSQLITE.Token, userInformationGetByLasSQLITE.EmpresaId.ToString(), settingGetByLasSQLITE.Id);

                    if (inventoryList.Data != null)
                    {
                        foreach (var iteminventory in inventoryList.Data)
                        {
                            var inventoryByIdSQLITE = await App.inventoryRepository.GetByIdAsync(iteminventory.Inventario.Id);
                            if (inventoryByIdSQLITE == null)
                            {
                                Models.Startup.Inventory dataInventory = new Models.Startup.Inventory();

                                dataInventory.UsuarioCreadorId = iteminventory.Inventario.UsuarioCreadorId;
                                dataInventory.UsuarioModificadorId = iteminventory.Inventario.UsuarioModificadorId;
                                dataInventory.UsuarioBajaId = iteminventory.Inventario.UsuarioBajaId;
                                dataInventory.UsuarioReactivadorId = iteminventory.Inventario.UsuarioReactivadorId;
                                dataInventory.FechaCreacion = iteminventory.Inventario.FechaCreacion;
                                dataInventory.FechaModificacion = iteminventory.Inventario.FechaModificacion;
                                dataInventory.FechaBaja = iteminventory.Inventario.FechaBaja;
                                dataInventory.FechaReactivacion = iteminventory.Inventario.FechaReactivacion;
                                dataInventory.Estado = iteminventory.Inventario.Estado;
                                dataInventory.EmpresaId = iteminventory.Inventario.EmpresaId;
                                dataInventory.Id = iteminventory.Inventario.Id;
                                dataInventory.ColaboradorResponsableId = iteminventory.Inventario.ColaboradorResponsableId;
                                dataInventory.Descripcion = iteminventory.Inventario.Descripcion;
                                dataInventory.Observaciones = iteminventory.Inventario.Observaciones;
                                dataInventory.FechaInventario = iteminventory.Inventario.FechaInventario;
                                dataInventory.Inventariado = iteminventory.Inventario.Inventariado;
                                dataInventory.NoInventario = iteminventory.Inventario.NoInventario;

                                var addAsync = await App.inventoryRepository.AddAsync(dataInventory);
                                if (addAsync == true)
                                {
                                    var collaboratorByIdSQLITE = await App.collaboratorRepository.GetByIdAsync(iteminventory.Inventario.ColaboradorResponsable.Id);
                                    if (collaboratorByIdSQLITE == null)
                                    {
                                        Collaborator dataCollaborator = new Collaborator();

                                        dataCollaborator.UsuarioCreadorId = iteminventory.Inventario.ColaboradorResponsable.UsuarioCreadorId;
                                        dataCollaborator.UsuarioModificadorId = iteminventory.Inventario.ColaboradorResponsable.UsuarioModificadorId;
                                        dataCollaborator.UsuarioBajaId = iteminventory.Inventario.ColaboradorResponsable.UsuarioBajaId;
                                        dataCollaborator.UsuarioReactivadorId = iteminventory.Inventario.ColaboradorResponsable.UsuarioReactivadorId;
                                        dataCollaborator.FechaCreacion = iteminventory.Inventario.ColaboradorResponsable.FechaCreacion;
                                        dataCollaborator.FechaModificacion = iteminventory.Inventario.ColaboradorResponsable.FechaModificacion;
                                        dataCollaborator.FechaBaja = iteminventory.Inventario.ColaboradorResponsable.FechaBaja;
                                        dataCollaborator.FechaReactivacion = iteminventory.Inventario.ColaboradorResponsable.FechaReactivacion;
                                        dataCollaborator.Estado = iteminventory.Inventario.ColaboradorResponsable.Estado;
                                        dataCollaborator.EmpresaId = iteminventory.Inventario.ColaboradorResponsable.EmpresaId;
                                        dataCollaborator.Id = iteminventory.Inventario.ColaboradorResponsable.Id;
                                        dataCollaborator.Nombre = iteminventory.Inventario.ColaboradorResponsable.Nombre;
                                        dataCollaborator.ApellidoPaterno = iteminventory.Inventario.ColaboradorResponsable.ApellidoPaterno;
                                        dataCollaborator.ApellidoMaterno = iteminventory.Inventario.ColaboradorResponsable.ApellidoMaterno;
                                        dataCollaborator.EstadoCivilId = iteminventory.Inventario.ColaboradorResponsable.EstadoCivilId;
                                        dataCollaborator.Genero = iteminventory.Inventario.ColaboradorResponsable.Genero;
                                        dataCollaborator.TipoIdentificacionId = iteminventory.Inventario.ColaboradorResponsable.TipoIdentificacionId;
                                        dataCollaborator.Identificacion = iteminventory.Inventario.ColaboradorResponsable.Identificacion;
                                        dataCollaborator.Foto = iteminventory.Inventario.ColaboradorResponsable.Foto;
                                        dataCollaborator.ExtensionFoto = iteminventory.Inventario.ColaboradorResponsable.ExtensionFoto;
                                        dataCollaborator.NumEmpleado = iteminventory.Inventario.ColaboradorResponsable.NumEmpleado;
                                        dataCollaborator.PuestoId = iteminventory.Inventario.ColaboradorResponsable.PuestoId;
                                        dataCollaborator.UbicacionId = iteminventory.Inventario.ColaboradorResponsable.UbicacionId;
                                        dataCollaborator.AreaId = iteminventory.Inventario.ColaboradorResponsable.AreaId;
                                        dataCollaborator.TipoColaboradorId = iteminventory.Inventario.ColaboradorResponsable.TipoColaboradorId;
                                        dataCollaborator.TelefonoMovil = iteminventory.Inventario.ColaboradorResponsable.TelefonoMovil;
                                        dataCollaborator.TelefonoOficina = iteminventory.Inventario.ColaboradorResponsable.TelefonoOficina;
                                        dataCollaborator.Email = iteminventory.Inventario.ColaboradorResponsable.Email;
                                        dataCollaborator.Email_secundario = iteminventory.Inventario.ColaboradorResponsable.Email_secundario;
                                        dataCollaborator.PortalWV = iteminventory.Inventario.ColaboradorResponsable.PortalWV;

                                        await App.collaboratorRepository.AddAsync(dataCollaborator);
                                    }

                                    foreach (var itemlocation in iteminventory.Inventario.Ubicaciones)
                                    {
                                        var locationByIdSQLITE = await App.locationRepository.GetByIdAsync(itemlocation.Id);
                                        if (locationByIdSQLITE == null)
                                        {
                                            Location dataLocation = new Location();

                                            dataLocation.Id = itemlocation.Id;
                                            dataLocation.Nombre = itemlocation.Nombre;

                                            await App.locationRepository.AddAsync(dataLocation);
                                        }
                                        foreach (var itemdetalleinventario in itemlocation.DetalleInventario)
                                        {
                                            Console.Write("ITEM DETALLE INVENTORY ::: ", itemdetalleinventario);
                                            var detalleInventarioByIdSQLITE = await App.inventoryDetailRepository.GetByIdAsync(itemdetalleinventario.Id);
                                            if (detalleInventarioByIdSQLITE == null)
                                            {
                                                var inventoryDetailByIdSQLITE = await App.inventoryDetailRepository.GetByIdAsync(itemdetalleinventario.Id);
                                                if (inventoryDetailByIdSQLITE == null)
                                                {
                                                    InventoryDetail dataInventoryDetail = new InventoryDetail();
                                                    dataInventoryDetail.UsuarioCreadorId = itemdetalleinventario.UsuarioCreadorId;
                                                    dataInventoryDetail.UsuarioModificadorId = itemdetalleinventario.UsuarioModificadorId;
                                                    dataInventoryDetail.UsuarioBajaId = itemdetalleinventario.UsuarioBajaId;
                                                    dataInventoryDetail.UsuarioReactivadorId = itemdetalleinventario.UsuarioReactivadorId;
                                                    dataInventoryDetail.FechaCreacion = itemdetalleinventario.FechaCreacion;
                                                    dataInventoryDetail.FechaModificacion = itemdetalleinventario.FechaModificacion;
                                                    dataInventoryDetail.FechaBaja = itemdetalleinventario.FechaBaja;
                                                    dataInventoryDetail.FechaReactivacion = itemdetalleinventario.FechaReactivacion;
                                                    dataInventoryDetail.Estado = itemdetalleinventario.Estado;
                                                    dataInventoryDetail.EmpresaId = itemdetalleinventario.EmpresaId;
                                                    dataInventoryDetail.Id = itemdetalleinventario.Id;
                                                    dataInventoryDetail.DispositivoId = itemdetalleinventario.DispositivoId;
                                                    dataInventoryDetail.InventarioId = itemdetalleinventario.InventarioId;
                                                    dataInventoryDetail.CentroDeCostosId = itemdetalleinventario.CentroDeCostosId;
                                                    dataInventoryDetail.EstadoFisicoId = itemdetalleinventario.EstadoFisicoId;
                                                    dataInventoryDetail.UbicacionId = itemdetalleinventario.UbicacionId;
                                                    dataInventoryDetail.ActivoId = itemdetalleinventario.ActivoId;
                                                    dataInventoryDetail.PresenciaAusensia = itemdetalleinventario.PresenciaAusensia;
                                                    dataInventoryDetail.Observaciones = itemdetalleinventario.Observaciones;
                                                    dataInventoryDetail.Mantenimiento = itemdetalleinventario.Mantenimiento;
                                                    await App.inventoryDetailRepository.AddAsync(dataInventoryDetail);
                                                }

                                                if (itemdetalleinventario.Activo != null)
                                                {
                                                    var assetByIdSQLITE = await App.assetRepository.GetByIdAsync(itemdetalleinventario.Activo.Id);
                                                    if (assetByIdSQLITE == null)
                                                    {
                                                        Asset dataAsset = new Asset();
                                                        dataAsset.UsuarioCreadorId = itemdetalleinventario.Activo.UsuarioCreadorId;
                                                        dataAsset.UsuarioModificadorId = itemdetalleinventario.Activo.UsuarioModificadorId;
                                                        dataAsset.UsuarioBajaId = itemdetalleinventario.Activo.UsuarioBajaId;
                                                        dataAsset.UsuarioReactivadorId = itemdetalleinventario.Activo.UsuarioReactivadorId;
                                                        dataAsset.FechaCreacion = itemdetalleinventario.Activo.FechaCreacion;
                                                        dataAsset.FechaModificacion = itemdetalleinventario.Activo.FechaModificacion;
                                                        dataAsset.FechaBaja = itemdetalleinventario.Activo.FechaBaja;
                                                        dataAsset.FechaReactivacion = itemdetalleinventario.Activo.FechaReactivacion;
                                                        dataAsset.Estado = itemdetalleinventario.Activo.Estado;
                                                        dataAsset.EmpresaId = itemdetalleinventario.Activo.EmpresaId;
                                                        dataAsset.Id = itemdetalleinventario.Activo.Id;
                                                        dataAsset.UbicacionId = itemdetalleinventario.Activo.UbicacionId;
                                                        dataAsset.GrupoActivoId = itemdetalleinventario.Activo.GrupoActivoId;
                                                        dataAsset.TipoActivoId = itemdetalleinventario.Activo.TipoActivoId;
                                                        dataAsset.Codigo = itemdetalleinventario.Activo.Codigo;
                                                        dataAsset.Serie = itemdetalleinventario.Activo.Serie;
                                                        dataAsset.Marca = itemdetalleinventario.Activo.Marca;
                                                        dataAsset.Modelo = itemdetalleinventario.Activo.Modelo;
                                                        dataAsset.Descripcion = itemdetalleinventario.Activo.Descripcion;
                                                        dataAsset.Nombre = itemdetalleinventario.Activo.Nombre;
                                                        dataAsset.Observaciones = itemdetalleinventario.Activo.Observaciones;
                                                        dataAsset.EstadoFisicoId = itemdetalleinventario.Activo.EstadoFisicoId;
                                                        dataAsset.TagId = itemdetalleinventario.Activo.TagId;
                                                        dataAsset.ColaboradorHabitualId = itemdetalleinventario.Activo.ColaboradorHabitualId;
                                                        dataAsset.ColaboradorResponsableId = itemdetalleinventario.Activo.ColaboradorResponsableId;
                                                        dataAsset.ValorCompra = itemdetalleinventario.Activo.ValorCompra;
                                                        dataAsset.FechaCompra = itemdetalleinventario.Activo.FechaCompra;
                                                        dataAsset.Proveedor = itemdetalleinventario.Activo.Proveedor;
                                                        dataAsset.FechaFinGarantia = itemdetalleinventario.Activo.FechaFinGarantia;
                                                        dataAsset.TieneFoto = itemdetalleinventario.Activo.TieneFoto;
                                                        dataAsset.TieneArchivo = itemdetalleinventario.Activo.TieneArchivo;
                                                        dataAsset.FechaCapitalizacion = itemdetalleinventario.Activo.FechaCapitalizacion;
                                                        dataAsset.FichaResguardo = itemdetalleinventario.Activo.FichaResguardo;
                                                        dataAsset.CampoLibre1 = itemdetalleinventario.Activo.CampoLibre1;
                                                        dataAsset.CampoLibre2 = itemdetalleinventario.Activo.CampoLibre2;
                                                        dataAsset.CampoLibre3 = itemdetalleinventario.Activo.CampoLibre3;
                                                        dataAsset.CampoLibre4 = itemdetalleinventario.Activo.CampoLibre4;
                                                        dataAsset.CampoLibre5 = itemdetalleinventario.Activo.CampoLibre5;
                                                        dataAsset.AreaId = itemdetalleinventario.Activo.AreaId;
                                                        await App.assetRepository.AddAsync(dataAsset);

                                                        try
                                                        {
                                                            if (dataAsset.EstadoFisicoId != null && dataAsset.EstadoFisicoId != Guid.Empty)
                                                            {
                                                                var physicalStateByIdSQLITE = await App.physicalStateRepository.GetByIdAsync((Guid)dataAsset.EstadoFisicoId);
                                                                if (physicalStateByIdSQLITE == null)
                                                                {
                                                                    var physicalStateById = await App.physicalStateRepository.GetPhysicalStateById(userInformationGetByLasSQLITE.Token, (Guid)userInformationGetByLasSQLITE.EmpresaId, (Guid)dataAsset.EstadoFisicoId);
                                                                    if (physicalStateById.Respuesta == true)
                                                                    {

                                                                        PhysicalState dataPhysicalState = new PhysicalState();
                                                                        dataPhysicalState.UsuarioCreadorId = physicalStateById.Data.UsuarioCreadorId;
                                                                        dataPhysicalState.UsuarioModificadorId = physicalStateById.Data.UsuarioModificadorId;
                                                                        dataPhysicalState.UsuarioBajaId = physicalStateById.Data.UsuarioBajaId;
                                                                        dataPhysicalState.UsuarioReactivadorId = physicalStateById.Data.UsuarioReactivadorId;
                                                                        dataPhysicalState.FechaCreacion = physicalStateById.Data.FechaCreacion;
                                                                        dataPhysicalState.FechaModificacion = physicalStateById.Data.FechaModificacion;
                                                                        dataPhysicalState.FechaBaja = physicalStateById.Data.FechaBaja;
                                                                        dataPhysicalState.FechaReactivacion = physicalStateById.Data.FechaReactivacion;
                                                                        dataPhysicalState.Estado = physicalStateById.Data.Estado;
                                                                        dataPhysicalState.EmpresaId = physicalStateById.Data.EmpresaId;
                                                                        dataPhysicalState.Id = physicalStateById.Data.Id;
                                                                        dataPhysicalState.Nombre = physicalStateById.Data.Nombre;
                                                                        dataPhysicalState.Descripcion = physicalStateById.Data.Descripcion;
                                                                        await App.physicalStateRepository.AddAsync(dataPhysicalState);
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

                                                if (itemdetalleinventario.Dispositivo != null)
                                                {
                                                    var deviceByIdSQLITE = await App.deviceRepository.GetByIdAsync(itemdetalleinventario.Dispositivo.Id);
                                                    if (deviceByIdSQLITE == null)
                                                    {
                                                        Models.Startup.Device dataDevice = new Models.Startup.Device();
                                                        dataDevice.UsuarioCreadorId = itemdetalleinventario.Dispositivo.UsuarioCreadorId;
                                                        dataDevice.UsuarioModificadorId = itemdetalleinventario.Dispositivo.UsuarioModificadorId;
                                                        dataDevice.UsuarioBajaId = itemdetalleinventario.Dispositivo.UsuarioBajaId;
                                                        dataDevice.UsuarioReactivadorId = itemdetalleinventario.Dispositivo.UsuarioReactivadorId;
                                                        dataDevice.FechaCreacion = itemdetalleinventario.Dispositivo.FechaCreacion;
                                                        dataDevice.FechaModificacion = itemdetalleinventario.Dispositivo.FechaModificacion;
                                                        dataDevice.FechaBaja = itemdetalleinventario.Dispositivo.FechaBaja;
                                                        dataDevice.FechaReactivacion = itemdetalleinventario.Dispositivo.FechaReactivacion;
                                                        dataDevice.Estado = itemdetalleinventario.Dispositivo.Estado;
                                                        dataDevice.EmpresaId = itemdetalleinventario.Dispositivo.EmpresaId;
                                                        dataDevice.Id = itemdetalleinventario.Dispositivo.Id;
                                                        dataDevice.Numero = itemdetalleinventario.Dispositivo.Numero;
                                                        dataDevice.Nombre = itemdetalleinventario.Dispositivo.Nombre;
                                                        dataDevice.TipoDispositivoId = itemdetalleinventario.Dispositivo.TipoDispositivoId;
                                                        dataDevice.UbicacionId = itemdetalleinventario.Dispositivo.UbicacionId;
                                                        dataDevice.Ip = itemdetalleinventario.Dispositivo.Ip;
                                                        dataDevice.Mac = itemdetalleinventario.Dispositivo.Mac;
                                                        dataDevice.Identifier = itemdetalleinventario.Dispositivo.Identifier;
                                                        dataDevice.PuertoEscucha = itemdetalleinventario.Dispositivo.PuertoEscucha;
                                                        dataDevice.PuertoTransmision = itemdetalleinventario.Dispositivo.PuertoTransmision;
                                                        dataDevice.HabilitadoParaAlta = itemdetalleinventario.Dispositivo.HabilitadoParaAlta;
                                                        dataDevice.OperacionId = itemdetalleinventario.Dispositivo.OperacionId;
                                                        dataDevice.ModoOperacionId = itemdetalleinventario.Dispositivo.ModoOperacionId;
                                                        dataDevice.EstadoActual = itemdetalleinventario.Dispositivo.EstadoActual;
                                                        dataDevice.Perimetral = itemdetalleinventario.Dispositivo.Perimetral;
                                                        dataDevice.Configurado = itemdetalleinventario.Dispositivo.Configurado;
                                                        await App.deviceRepository.AddAsync(dataDevice);
                                                    }
                                                }

                                                if (itemdetalleinventario.Inventario != null)
                                                {
                                                    var inventorydiByIdSQLITE = await App.inventoryRepository.GetByIdAsync(itemdetalleinventario.Inventario.Id);
                                                    if (inventorydiByIdSQLITE == null)
                                                    {
                                                        Models.Startup.Inventory dataInventorydi = new Models.Startup.Inventory();
                                                        dataInventorydi.UsuarioCreadorId = itemdetalleinventario.Inventario.UsuarioCreadorId;
                                                        dataInventorydi.UsuarioModificadorId = itemdetalleinventario.Inventario.UsuarioModificadorId;
                                                        dataInventorydi.UsuarioBajaId = itemdetalleinventario.Inventario.UsuarioBajaId;
                                                        dataInventorydi.UsuarioReactivadorId = itemdetalleinventario.Inventario.UsuarioReactivadorId;
                                                        dataInventorydi.FechaCreacion = itemdetalleinventario.Inventario.FechaCreacion;
                                                        dataInventorydi.FechaModificacion = itemdetalleinventario.Inventario.FechaModificacion;
                                                        dataInventorydi.FechaBaja = itemdetalleinventario.Inventario.FechaBaja;
                                                        dataInventorydi.FechaReactivacion = itemdetalleinventario.Inventario.FechaReactivacion;
                                                        dataInventorydi.Estado = itemdetalleinventario.Inventario.Estado;
                                                        dataInventorydi.EmpresaId = itemdetalleinventario.Inventario.EmpresaId;
                                                        dataInventorydi.Id = itemdetalleinventario.Inventario.Id;
                                                        dataInventorydi.ColaboradorResponsableId = itemdetalleinventario.Inventario.ColaboradorResponsableId;
                                                        dataInventorydi.Descripcion = itemdetalleinventario.Inventario.Descripcion;
                                                        dataInventorydi.Observaciones = itemdetalleinventario.Inventario.Observaciones;
                                                        dataInventorydi.FechaInventario = itemdetalleinventario.Inventario.FechaInventario;
                                                        dataInventorydi.Inventariado = itemdetalleinventario.Inventario.Inventariado;
                                                        dataInventorydi.NoInventario = itemdetalleinventario.Inventario.NoInventario;
                                                        await App.inventoryRepository.AddAsync(dataInventorydi);
                                                    }
                                                }

                                                if (itemdetalleinventario.Ubicacion != null)
                                                {
                                                    var locationdiByIdSQLITE = await App.locationRepository.GetByIdAsync(itemdetalleinventario.Ubicacion.Id);
                                                    if (locationdiByIdSQLITE == null)
                                                    {
                                                        Models.Startup.Location dataLocation = new Location();
                                                        dataLocation.Id = itemdetalleinventario.Ubicacion.Id;
                                                        dataLocation.Nombre = itemdetalleinventario.Ubicacion.Nombre;
                                                        await App.locationRepository.AddAsync(dataLocation);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Mensaje", "No hay inventarios disponibles en este momento", "Aceptar");
                    }

                    InventoryData.Clear();
                    var inventoryAllSQLITE = await App.inventoryRepository.GetAllAsync();
                    foreach (var item in inventoryAllSQLITE)
                    {
                        InventoryData.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void OnUploadInventoryCommand()
        {
            try
            {
                var list = await App.settingRepository.GetAllAsync();
                if (list.Count() == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Mensaje", "Ingrese el identificador del dispositivo en la sección de configuración para cargar inventarios", "Aceptar");
                }
                else
                {
                    var inventoryAllSQLITE = await App.inventoryRepository.GetAllLoad();
                    if (inventoryAllSQLITE.Count() > 0)
                    {
                        var userInformationGetByLasSQLITE = await App.userInformationRepository.GetByLastOrDefaultAsync();

                        foreach (var item in inventoryAllSQLITE)
                        {
                            var load = await App.settingRepository.GetInventoryLoad(userInformationGetByLasSQLITE.Token, userInformationGetByLasSQLITE.EmpresaId.ToString(), item);

                            await Application.Current.MainPage.DisplayAlert("Mensaje", "Los inventarios se cargaron correctamente.", "Aceptar");
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Mensaje", "No hay inventarios disponibles en este momento", "Aceptar");
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