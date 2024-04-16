using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using Location = ZebraRFIDXamarinDemo.Models.Startup.Location;
using InventoryDetail = ZebraRFIDXamarinDemo.Models.Startup.InventoryDetail;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.Views.Authorization;

namespace ZebraRFIDXamarinDemo.ViewModels.Inventory
{
    public class InventoryIndexViewModel : InventoryBaseViewModel
    {
        public Command LoadInventoryCommand { get; }
        public Command DetailInventoryCommand { get; }
        public Command SyncInventoryCommand { get; }
        public Command UploadInventoryCommand { get; }
        public Command DeleteInventoryCommand { get; }
        public ObservableCollection<InventoryLocationAssetQuery> InventoryData { get; }
        public InventoryIndexViewModel(INavigation _navigation)
        {
            LoadInventoryCommand = new Command(async () => await ExecuteLoadPersonCommand());
            InventoryData = new ObservableCollection<InventoryLocationAssetQuery>();
            DetailInventoryCommand = new Command<InventoryLocationAssetQuery>(OnDetailInventory);
            SyncInventoryCommand = new Command(OnSyncInventoryCommand);
            UploadInventoryCommand = new Command(OnUploadInventoryCommand);
            DeleteInventoryCommand = new Command(OnDeleteInventoryCommand);
            Navigation = _navigation;
        }

        public async void OnAppearing()
        {
            IsBusy = true;
        }

        async Task ExecuteLoadPersonCommand()
        {
            IsBusy = true;
            try
            {
                InventoryData.Clear();
                var inventoryAllSQLITE = await App.inventoryRepository.GetAllByInventoryLocationAssetAsync();
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

        private async void OnDetailInventory(InventoryLocationAssetQuery inventoryLocationAsset)
        {
            await Navigation.PushAsync(new Views.Inventory.InventoryDetail(inventoryLocationAsset));
        }

        private async void OnSyncInventoryCommand()
        {
            try
            {
                IsRunning = true;
                // var list = await App.settingRepository.GetAllAsync();
                // if (list.Count() == 0)
                // {
                //     await Application.Current.MainPage.DisplayAlert("Mensaje", "Ingrese el identificador del dispositivo en la sección de configuración para sincronizar inventarios", "Aceptar");
                // }
                // else
                // {
                var settingGetByLasSQLITE = await App.settingRepository.GetByLastOrDefaultAsync();
                var userInformationGetByLasSQLITE = await App.userInformationRepository.GetByLastOrDefaultAsync();
                var inventoryList = await App.settingRepository.GetInventorySync(userInformationGetByLasSQLITE.Token, userInformationGetByLasSQLITE.EmpresaId.ToString(), new Guid("962CD5F7-CF54-4124-B0CF-60F9E90CCD76").ToString());
                
                if (inventoryList.Data != null)
                {
                    IsRunning = false;
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
                                        dataLocation.Status = 1;

                                        await App.locationRepository.AddAsync(dataLocation);
                                    }
                                    foreach (var itemdetalleinventario in itemlocation.DetalleInventario)
                                    {
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
                                                    dataAsset.Status = false;
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
                                                    Location dataLocation = new Location();
                                                    dataLocation.Id = itemdetalleinventario.Ubicacion.Id;
                                                    dataLocation.Nombre = itemdetalleinventario.Ubicacion.Nombre;
                                                    dataLocation.Status = 1;
                                                    await App.locationRepository.AddAsync(dataLocation);
                                                }
                                            }

                                            if (itemdetalleinventario.Ubicacion != null && itemdetalleinventario.Inventario != null)
                                            {
                                                InventoryLocation dataInventoryLocation = new InventoryLocation();
                                                dataInventoryLocation.Id = Guid.NewGuid();
                                                dataInventoryLocation.InventarioId = itemdetalleinventario.Inventario.Id;
                                                dataInventoryLocation.UbicacionId = itemdetalleinventario.Ubicacion.Id;
                                                dataInventoryLocation.Status = 1;

                                                var addInventoryLocation = await App.inventoryLocationRepository.AddAsync(dataInventoryLocation);
                                                if (addInventoryLocation == true)
                                                {
                                                    if (dataInventoryLocation.Id != null && dataInventoryLocation.Id != Guid.Empty && itemdetalleinventario.Activo != null)
                                                    {
                                                        InventoryLocationAsset dataInventoryLocationAsset = new InventoryLocationAsset();
                                                        dataInventoryLocationAsset.Id = Guid.NewGuid();
                                                        dataInventoryLocationAsset.InventarioUbicacionId = dataInventoryLocation.Id;
                                                        dataInventoryLocationAsset.ActivoId = itemdetalleinventario.Activo.Id;

                                                        await App.inventoryLocationAssetRepository.AddAsync(dataInventoryLocationAsset);
                                                    }
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
                    IsRunning = false;
                    // await Application.Current.MainPage.DisplayAlert("Mensaje", "No hay inventarios disponibles en este momento", "Aceptar");
                    await Application.Current.MainPage.DisplayAlert("Message", "Session expired", "Ok");
                    await App.userInformationRepository.DeleteAllAsync();
                    await Shell.Current.GoToAsync($"//{nameof(LoginIndex)}");
                }

                try
                {
                    InventoryData.Clear();
                    var inventoryAllSQLITE = await App.inventoryRepository.GetAllByInventoryLocationAssetAsync();
                    foreach (var item in inventoryAllSQLITE)
                    {
                        InventoryData.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    var physicalStateGetAll = await App.physicalStateRepository.GetAllPhysicalState(userInformationGetByLasSQLITE.Token, (Guid)userInformationGetByLasSQLITE.EmpresaId);
                    if (physicalStateGetAll.Respuesta == true)
                    {
                        if (physicalStateGetAll.Data.Count() > 0)
                        {
                            foreach (var itemPhysicalState in physicalStateGetAll.Data)
                            {
                                var physicalStateByIdSQLITE = await App.physicalStateRepository.GetByIdAsync(itemPhysicalState.Id);
                                if (physicalStateByIdSQLITE == null)
                                {
                                    await App.physicalStateRepository.AddAsync(itemPhysicalState);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    var paramsGetAll = await App.paramsRepositoty.GetParams(userInformationGetByLasSQLITE.Token, (Guid)userInformationGetByLasSQLITE.EmpresaId);
                    if (paramsGetAll.Respuesta == true)
                    {
                        if (paramsGetAll.Data.Param != null)
                        {
                            if (paramsGetAll.Data.Param.Count() > 0)
                            {
                                foreach (var itemParams in paramsGetAll.Data.Param)
                                {
                                    var paramsByIdSQLITE = await App.paramsRepositoty.GetByIdAsync(itemParams.Id);
                                    if (paramsByIdSQLITE == null)
                                    {
                                        await App.paramsRepositoty.AddAsync(itemParams);
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
                // }
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
                        if (userInformationGetByLasSQLITE != null)
                        {
                            foreach (var item in inventoryAllSQLITE)
                            {


                                if (item.DetalleInventario.Count() > 0)
                                {

                                    var inventoryLoadSQLITE = await App.inventoryRepository.GetAllCountLoad(item.Id);
                                    if (inventoryLoadSQLITE.UbicacionTotal == inventoryLoadSQLITE.UbicacionFinalizada)
                                    {

                                        await App.settingRepository.PutInventoryLoad(userInformationGetByLasSQLITE.Token, userInformationGetByLasSQLITE.EmpresaId.ToString(), item);

                                        await Application.Current.MainPage.DisplayAlert("Mensaje", "El inventario " + item.NoInventario + " se cargo correctamente.", "Aceptar");
                                    }
                                    /*
                                    if (item.DetalleInventario.Count() == item.DetalleInventarioTotal)
                                    {
                                        // await App.settingRepository.PutInventoryLoad(userInformationGetByLasSQLITE.Token, userInformationGetByLasSQLITE.EmpresaId.ToString(), item);
                                        //await Application.Current.MainPage.DisplayAlert("Mensaje", "Los inventarios se cargaron correctamente.", "Aceptar");
                                    }
                                    else
                                    {
                                        //await Application.Current.MainPage.DisplayAlert("Advertencia", "No hay inventarios disponibles para procesar en este momento.", "Aceptar");
                                    }
                                */
                                }

                            }
                        }
                        //await Application.Current.MainPage.DisplayAlert("Mensaje", "Los inventarios se cargaron correctamente.", "Aceptar");
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

        private async void OnDeleteInventoryCommand()
        {
            await App.settingRepository.DeleteAllAsync();
            await App.personRepository.DeleteAllAsync();
            await App.inventoryRepository.DeleteAllAsync();
            await App.deviceRepository.DeleteAllAsync();
            await App.inventoryRepository.DeleteAllAsync();
            await App.collaboratorRepository.DeleteAllAsync();
            await App.locationRepository.DeleteAllAsync();
            await App.inventoryDetailRepository.DeleteAllAsync();
            await App.assetRepository.DeleteAllAsync();
            await App.paramsRepositoty.DeleteAllAsync();
            await App.inventoryLocationRepository.DeleteAllAsync();
            await App.inventoryLocationAssetRepository.DeleteAllAsync();

            await ExecuteLoadPersonCommand();
        }
    }
}