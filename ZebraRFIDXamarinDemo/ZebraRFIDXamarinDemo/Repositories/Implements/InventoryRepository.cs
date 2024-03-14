using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Graphics;
using SQLite;
using SQLiteNetExtensions.Attributes;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.Repositories.Interfaces;
using SQLiteNetExtensions.Extensions;
using Android.Service.Autofill;

namespace ZebraRFIDXamarinDemo.Repositories.Implements
{
    public class InventoryRepository : IInventoryRepository
    {
        public SQLiteAsyncConnection _database;
        public InventoryRepository(string pathDatabase)
        {
            _database = new SQLiteAsyncConnection(pathDatabase);
            _database.CreateTableAsync<Inventory>().Wait();
        }

        public async Task<bool> AddAsync(Inventory data)
        {
            bool ok = false;

            try
            {
                await _database.InsertAsync(data);
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool ok = false;

            try
            {
                await _database.DeleteAsync<Inventory>(id);
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<bool> DeleteAllAsync()
        {
            bool ok = false;

            try
            {
                await _database.DeleteAllAsync<Inventory>();
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<List<InventoryQuery>> GetAllAsync()
        {
            var collaboratordi = await App.collaboratorRepository.GetAllAsync();
            //var collaboratordi = await _database.QueryAsync<Collaborator>("SELECT * FROM Collaborator");

            var assetdi = await App.assetRepository.GetAllAsync();
            //var assetdi = await _database.QueryAsync<Asset>("SELECT * FROM Asset");

            var devicedi = await App.deviceRepository.GetAllAsync();
            //var devicedi = await _database.QueryAsync<Device>("SELECT * FROM Device");

            //var inventorydi = await App.inventoryRepository.GetAllAsync();
            var inventorydi = await _database.QueryAsync<Inventory>("SELECT * FROM Inventory");

            var locationdi = await App.locationRepository.GetAllAsync();
            //var locationdi = await _database.QueryAsync<Location>("SELECT * FROM Location");

            var inventoryDetaildi = await App.inventoryDetailRepository.GetAllAsync();
            //var inventoryDetaildi = await _database.QueryAsync<InventoryDetail>("SELECT * FROM InventoryDetail");

            var physicalStatedi = await App.physicalStateRepository.GetAllAsync();

            var querydi = (from i in inventorydi

                           join c in collaboratordi on i.ColaboradorResponsableId equals c.Id
                           into InventoryCollaborator
                           from cic in InventoryCollaborator.DefaultIfEmpty()

                           join id in inventoryDetaildi on i.Id equals id.InventarioId
                           into InventoryDetailInventory
                           from ididi in InventoryDetailInventory.DefaultIfEmpty()

                           group i by new { i, cic } into g

                           select new InventoryQuery()
                           {
                               Inventario = new Inventory()
                               {
                                   UsuarioCreadorId = g.Key.i.UsuarioCreadorId,
                                   UsuarioModificadorId = g.Key.i.UsuarioModificadorId,
                                   UsuarioBajaId = g.Key.i.UsuarioBajaId,
                                   UsuarioReactivadorId = g.Key.i.UsuarioReactivadorId,
                                   FechaCreacion = g.Key.i.FechaCreacion,
                                   FechaModificacion = g.Key.i.FechaModificacion,
                                   FechaBaja = g.Key.i.FechaBaja,
                                   FechaReactivacion = g.Key.i.FechaReactivacion,
                                   Estado = g.Key.i.Estado,
                                   EmpresaId = g.Key.i.EmpresaId,
                                   Id = g.Key.i.Id,
                                   ColaboradorResponsableId = g.Key.i.ColaboradorResponsableId,
                                   Descripcion = g.Key.i.Descripcion,
                                   Observaciones = g.Key.i.Observaciones,
                                   FechaInventario = g.Key.i.FechaInventario,
                                   Inventariado = g.Key.i.Inventariado,
                                   NoInventario = g.Key.i.NoInventario,
                                   ColaboradorResponsable = new Collaborator
                                   {
                                       Id = g.Key.cic.Id,
                                       Nombre = g.Key.cic.Nombre,
                                       ApellidoPaterno = g.Key.cic.ApellidoPaterno,
                                       ApellidoMaterno = g.Key.cic.ApellidoMaterno,
                                       EstadoCivilId = g.Key.cic.EstadoCivilId,
                                       Genero = g.Key.cic.Genero,
                                       TipoIdentificacionId = g.Key.cic.TipoIdentificacionId,
                                       Identificacion = g.Key.cic.Identificacion,
                                       Foto = g.Key.cic.Foto,
                                       ExtensionFoto = g.Key.cic.ExtensionFoto,
                                       NumEmpleado = g.Key.cic.NumEmpleado,
                                       PuestoId = g.Key.cic.PuestoId,
                                       UbicacionId = g.Key.cic.UbicacionId,
                                       AreaId = g.Key.cic.AreaId,
                                       TipoColaboradorId = g.Key.cic.TipoColaboradorId,
                                       TelefonoMovil = g.Key.cic.TelefonoMovil,
                                       TelefonoOficina = g.Key.cic.TelefonoOficina,
                                       Email = g.Key.cic.Email,
                                       Email_secundario = g.Key.cic.Email_secundario
                                   },
                                   Ubicaciones = inventoryDetaildi.Where(a => a.InventarioId == g.Key.i.Id).Select(s => new Location()
                                   {
                                       Id = locationdi.FirstOrDefault(f => f.Id == s.UbicacionId).Id,
                                       Nombre = locationdi.FirstOrDefault(f => f.Id == s.UbicacionId).Nombre,
                                       Status = locationdi.FirstOrDefault(f => f.Id == s.UbicacionId).Status,
                                       DetalleInventario = inventoryDetaildi.Where(a => a.InventarioId == g.Key.i.Id).Select(q => new InventoryDetail()
                                       {
                                           UsuarioCreadorId = q.UsuarioCreadorId,
                                           UsuarioModificadorId = q.UsuarioModificadorId,
                                           UsuarioBajaId = q.UsuarioBajaId,
                                           UsuarioReactivadorId = q.UsuarioReactivadorId,
                                           FechaCreacion = q.FechaCreacion,
                                           FechaModificacion = q.FechaModificacion,
                                           FechaBaja = q.FechaBaja,
                                           FechaReactivacion = q.FechaReactivacion,
                                           Estado = q.Estado,
                                           EmpresaId = q.EmpresaId,
                                           Id = q.Id,
                                           DispositivoId = q.DispositivoId,
                                           InventarioId = q.InventarioId,
                                           CentroDeCostosId = q.CentroDeCostosId,
                                           EstadoFisicoId = q.EstadoFisicoId,
                                           UbicacionId = q.UbicacionId,
                                           ActivoId = q.ActivoId,
                                           PresenciaAusensia = q.PresenciaAusensia,
                                           Observaciones = q.Observaciones,
                                           Mantenimiento = q.Mantenimiento,
                                           Activo = new Asset()
                                           {
                                               UsuarioCreadorId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).UsuarioCreadorId,
                                               UsuarioModificadorId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).UsuarioModificadorId,
                                               UsuarioBajaId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).UsuarioBajaId,
                                               UsuarioReactivadorId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).UsuarioReactivadorId,
                                               FechaCreacion = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).FechaCreacion,
                                               FechaModificacion = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).FechaModificacion,
                                               FechaBaja = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).FechaBaja,
                                               FechaReactivacion = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).FechaReactivacion,
                                               Estado = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).Estado,
                                               EmpresaId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).EmpresaId,
                                               Id = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).Id,
                                               UbicacionId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).UbicacionId,
                                               GrupoActivoId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).GrupoActivoId,
                                               TipoActivoId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).TipoActivoId,
                                               Codigo = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).Codigo,
                                               Serie = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).Serie,
                                               Marca = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).Marca,
                                               Modelo = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).Modelo,
                                               Descripcion = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).Descripcion,
                                               Nombre = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).Nombre,
                                               Observaciones = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).Observaciones,
                                               EstadoFisicoId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).EstadoFisicoId,
                                               TagId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).TagId,
                                               ColaboradorHabitualId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).ColaboradorHabitualId,
                                               ColaboradorResponsableId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).ColaboradorResponsableId,
                                               ValorCompra = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).ValorCompra,
                                               FechaCompra = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).FechaCompra,
                                               Proveedor = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).Proveedor,
                                               FechaFinGarantia = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).FechaFinGarantia,
                                               TieneFoto = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).TieneFoto,
                                               TieneArchivo = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).TieneArchivo,
                                               FechaCapitalizacion = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).FechaCapitalizacion,
                                               FichaResguardo = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).FichaResguardo,
                                               CampoLibre1 = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).CampoLibre1,
                                               CampoLibre2 = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).CampoLibre2,
                                               CampoLibre3 = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).CampoLibre3,
                                               CampoLibre4 = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).CampoLibre4,
                                               CampoLibre5 = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).CampoLibre5,
                                               AreaId = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).AreaId,
                                               EstadoFisico = physicalStatedi.FirstOrDefault(x => x.Id == assetdi.FirstOrDefault(x => x.Id == q.ActivoId).EstadoFisicoId)
                                           }
                                       }).Where(y => y.UbicacionId == s.UbicacionId).ToList()
                                   }).ToList()
                               }
                           }).ToList();

            /*
                join u in locationdi on ididi.UbicacionId equals u.Id
                into InventoryDetailLocation
                from idl in InventoryDetailLocation.DefaultIfEmpty()
            */

            //select g.ToList();






            /*
                          select new InventoryQuery()
                          {
                              UsuarioCreadorId = g.UsuarioCreadorId,
                              UsuarioModificadorId = i.UsuarioModificadorId,
                              UsuarioBajaId = i.UsuarioBajaId,
                              UsuarioReactivadorId = i.UsuarioReactivadorId,
                              FechaCreacion = i.FechaCreacion,
                              FechaModificacion = i.FechaModificacion,
                              FechaBaja = i.FechaBaja,
                              FechaReactivacion = i.FechaReactivacion,
                              Estado = i.Estado,
                              EmpresaId = i.EmpresaId,
                              Id = i.Id,
                              ColaboradorResponsableId = i.ColaboradorResponsableId,
                              Descripcion = i.Descripcion,
                              Observaciones = i.Observaciones,
                              FechaInventario = i.FechaInventario,
                              Inventariado = i.Inventariado,
                              NoInventario = i.NoInventario,
                              ColaboradorResponsable = new Collaborator
                              {
                                  Id = cic.Id,
                                  Nombre = cic.Nombre,
                                  ApellidoPaterno = cic.ApellidoPaterno,
                                  ApellidoMaterno = cic.ApellidoMaterno,
                                  EstadoCivilId = cic.EstadoCivilId,
                                  Genero = cic.Genero,
                                  TipoIdentificacionId = cic.TipoIdentificacionId,
                                  Identificacion = cic.Identificacion,
                                  Foto = cic.Foto,
                                  ExtensionFoto = cic.ExtensionFoto,
                                  NumEmpleado = cic.NumEmpleado,
                                  PuestoId = cic.PuestoId,
                                  UbicacionId = cic.UbicacionId,
                                  AreaId = cic.AreaId,
                                  TipoColaboradorId = cic.TipoColaboradorId,
                                  TelefonoMovil = cic.TelefonoMovil,
                                  TelefonoOficina = cic.TelefonoOficina,
                                  Email = cic.Email,
                                  Email_secundario = cic.Email_secundario
                              },
                              DetalleInventario = inventoryDetaildi.Where(r => r.InventarioId == i.Id).GroupBy(d => d.UbicacionId).Select(s => new InventoryDetailQuery()
                              {
                                  Ubicacion = s.ToList()
                                  //Ubicacion = locationdi.FirstOrDefault(f => f.Id == s.UbicacionId),
                                  //Ubicacion = s.Ubicacion
                                  //UbicacionId = s.UbicacionId
                              }).ToList()
                              /*
                              Ubicaciones = inventoryDetaildi.Where(a => a.InventarioId == i.Id).Select(s => new Location()
                              {
                                  Id = s.Ubicacion.Id,
                                  Nombre = s.Ubicacion.Nombre
                              }).ToList()
                              */
            /*
                          };
            */





            /*
            var querydi = from id in inventoryDetaildi

                          join a in assetdi on id.ActivoId equals a.Id
                          into InventoryDetailAsset
                          from ida in InventoryDetailAsset.DefaultIfEmpty()

                          join d in devicedi on id.DispositivoId equals d.Id
                          into InventoryDetailDevice
                          from idd in InventoryDetailDevice.DefaultIfEmpty()

                          join i in inventorydi on id.InventarioId equals i.Id
                          into InventoryDetailInventory
                          from idi in InventoryDetailInventory.DefaultIfEmpty()

                          join c in collaboratordi on idi.ColaboradorResponsableId equals c.Id
                          into InventoryCollaborator
                          from ic in InventoryCollaborator.DefaultIfEmpty()

                          join u in locationdi on id.UbicacionId equals u.Id
                          into InventoryDetailLocation
                          from idl in InventoryDetailLocation.DefaultIfEmpty()

                          select new Inventory()
                          {
                              UsuarioCreadorId = idi.UsuarioCreadorId,
                              UsuarioModificadorId = idi.UsuarioModificadorId,
                              UsuarioBajaId = idi.UsuarioBajaId,
                              UsuarioReactivadorId = idi.UsuarioReactivadorId,
                              FechaCreacion = idi.FechaCreacion,
                              FechaModificacion = idi.FechaModificacion,
                              FechaBaja = idi.FechaBaja,
                              FechaReactivacion = idi.FechaReactivacion,
                              Estado = idi.Estado,
                              EmpresaId = idi.EmpresaId,
                              Id = idi.Id,
                              ColaboradorResponsableId = idi.ColaboradorResponsableId,
                              Descripcion = idi.Descripcion,
                              Observaciones = idi.Observaciones,
                              FechaInventario = idi.FechaInventario,
                              Inventariado = idi.Inventariado,
                              NoInventario = idi.NoInventario,
                              ColaboradorResponsable = new Collaborator
                              {
                                  Id = ic.Id,
                                  Nombre = ic.Nombre,
                                  ApellidoPaterno = ic.ApellidoPaterno,
                                  ApellidoMaterno = ic.ApellidoMaterno,
                                  EstadoCivilId = ic.EstadoCivilId,
                                  Genero = ic.Genero,
                                  TipoIdentificacionId = ic.TipoIdentificacionId,
                                  Identificacion = ic.Identificacion,
                                  Foto = ic.Foto,
                                  ExtensionFoto = ic.ExtensionFoto,
                                  NumEmpleado = ic.NumEmpleado,
                                  PuestoId = ic.PuestoId,
                                  UbicacionId = ic.UbicacionId,
                                  AreaId = ic.AreaId,
                                  TipoColaboradorId = ic.TipoColaboradorId,
                                  TelefonoMovil = ic.TelefonoMovil,
                                  TelefonoOficina = ic.TelefonoOficina,
                                  Email = ic.Email,
                                  Email_secundario = ic.Email_secundario
                              },
                              Ubicaciones = inventorydi.Where(a => a.Id == id.InventarioId).Select(s => new Inventory()
                              {
                                  DetalleInventario = null
                              })
                              */





            /*
            Ubicaciones = locationdi.Where(w => w.Id == id.UbicacionId).Select(s => new Location()
            {
                Id = s.Id,
                Nombre = s.Nombre,
                DetalleInventario = inventoryDetaildi.Where(a => a.Id == id.Id && s.Id == a.UbicacionId).Select(p => new InventoryDetail()
                {
                    UsuarioCreadorId = p.UsuarioCreadorId,
                    UsuarioModificadorId = p.UsuarioModificadorId,
                    UsuarioBajaId = p.UsuarioBajaId,
                    UsuarioReactivadorId = p.UsuarioReactivadorId,
                    FechaCreacion = p.FechaCreacion,
                    FechaModificacion = p.FechaModificacion,
                    FechaBaja = p.FechaBaja,
                    FechaReactivacion = p.FechaReactivacion,
                    Estado = p.Estado,
                    EmpresaId = p.EmpresaId,
                    Id = p.Id,
                    DispositivoId = p.DispositivoId,
                    InventarioId = p.InventarioId,
                    CentroDeCostosId = p.CentroDeCostosId,
                    EstadoFisicoId = p.EstadoFisicoId,
                    UbicacionId = p.UbicacionId,
                    ActivoId = p.ActivoId,
                    PresenciaAusensia = p.PresenciaAusensia,
                    Observaciones = p.Observaciones,
                    Mantenimiento = p.Mantenimiento,
                    Activo = new Asset()
                    {
                        UsuarioCreadorId = ida.UsuarioCreadorId,
                        UsuarioModificadorId = ida.UsuarioModificadorId,
                        UsuarioBajaId = ida.UsuarioBajaId,
                        UsuarioReactivadorId = ida.UsuarioReactivadorId,
                        FechaCreacion = ida.FechaCreacion,
                        FechaModificacion = ida.FechaModificacion,
                        FechaBaja = ida.FechaBaja,
                        FechaReactivacion = ida.FechaReactivacion,
                        Estado = ida.Estado,
                        EmpresaId = ida.EmpresaId,
                        Id = ida.Id,
                        UbicacionId = ida.UbicacionId,
                        GrupoActivoId = ida.GrupoActivoId,
                        TipoActivoId = ida.TipoActivoId,
                        Codigo = ida.Codigo,
                        Serie = ida.Serie,
                        Marca = ida.Marca,
                        Modelo = ida.Modelo,
                        Descripcion = ida.Descripcion,
                        Nombre = ida.Nombre,
                        Observaciones = ida.Observaciones,
                        EstadoFisicoId = ida.EstadoFisicoId,
                        TagId = ida.TagId,
                        ColaboradorHabitualId = ida.ColaboradorHabitualId,
                        ColaboradorResponsableId = ida.ColaboradorResponsableId,
                        ValorCompra = ida.ValorCompra,
                        FechaCompra = ida.FechaCompra,
                        Proveedor = ida.Proveedor,
                        FechaFinGarantia = ida.FechaFinGarantia,
                        TieneFoto = ida.TieneFoto,
                        TieneArchivo = ida.TieneArchivo,
                        FechaCapitalizacion = ida.FechaCapitalizacion,
                        FichaResguardo = ida.FichaResguardo,
                        CampoLibre1 = ida.CampoLibre1,
                        CampoLibre2 = ida.CampoLibre2,
                        CampoLibre3 = ida.CampoLibre3,
                        CampoLibre4 = ida.CampoLibre4,
                        CampoLibre5 = ida.CampoLibre5,
                        AreaId = ida.AreaId,
                    },
                    Dispositivo = new Device()
                    {
                        UsuarioCreadorId = idd.UsuarioCreadorId,
                        UsuarioModificadorId = idd.UsuarioModificadorId,
                        UsuarioBajaId = idd.UsuarioBajaId,
                        UsuarioReactivadorId = idd.UsuarioReactivadorId,
                        FechaCreacion = idd.FechaCreacion,
                        FechaModificacion = idd.FechaModificacion,
                        FechaBaja = idd.FechaBaja,
                        FechaReactivacion = idd.FechaReactivacion,
                        Estado = idd.Estado,
                        EmpresaId = idd.EmpresaId,
                        Id = idd.Id,
                        Numero = idd.Numero,
                        Nombre = idd.Nombre,
                        TipoDispositivoId = idd.TipoDispositivoId,
                        UbicacionId = idd.UbicacionId,
                        Ip = idd.Ip,
                        Mac = idd.Mac,
                        Identifier = idd.Identifier,
                        PuertoEscucha = idd.PuertoEscucha,
                        PuertoTransmision = idd.PuertoTransmision,
                        HabilitadoParaAlta = idd.HabilitadoParaAlta,
                        OperacionId = idd.OperacionId,
                        ModoOperacionId = idd.ModoOperacionId,
                        EstadoActual = idd.EstadoActual,
                        Perimetral = idd.Perimetral,
                        Configurado = idd.Configurado,
                    },
                    Inventario = new Inventory()
                    {
                        UsuarioCreadorId = idi.UsuarioCreadorId,
                        UsuarioModificadorId = idi.UsuarioModificadorId,
                        UsuarioBajaId = idi.UsuarioBajaId,
                        UsuarioReactivadorId = idi.UsuarioReactivadorId,
                        FechaCreacion = idi.FechaCreacion,
                        FechaModificacion = idi.FechaModificacion,
                        FechaBaja = idi.FechaBaja,
                        FechaReactivacion = idi.FechaReactivacion,
                        Estado = idi.Estado,
                        EmpresaId = idi.EmpresaId,
                        Id = idi.Id,
                        ColaboradorResponsableId = idi.ColaboradorResponsableId,
                        Descripcion = idi.Descripcion,
                        Observaciones = idi.Observaciones,
                        FechaInventario = idi.FechaInventario,
                        Inventariado = idi.Inventariado,
                        NoInventario = idi.NoInventario
                    },
                    Ubicacion = new Location()
                    {
                        Id = idl.Id,
                        Nombre = idl.Nombre
                    }
                }).ToList()
            }).ToList()

        };
            */






            /*
            var querydi = from id in inventoryDetaildi

                          join a in assetdi on id.ActivoId equals a.Id
                          into InventoryDetailAsset
                          from ida in InventoryDetailAsset.DefaultIfEmpty()

                          join d in devicedi on id.DispositivoId equals d.Id
                          into InventoryDetailDevice
                          from idd in InventoryDetailDevice.DefaultIfEmpty()

                          join i in inventorydi on id.InventarioId equals i.Id
                          into InventoryDetailInventory
                          from idi in InventoryDetailInventory.DefaultIfEmpty()

                          join c in collaboratordi on idi.ColaboradorResponsableId equals c.Id
                          into InventoryCollaborator
                          from ic in InventoryCollaborator.DefaultIfEmpty()

                          join u in locationdi on id.UbicacionId equals u.Id
                          into InventoryDetailLocation
                          from idl in InventoryDetailLocation.DefaultIfEmpty()

                          select new Inventory()
                          {
                              UsuarioCreadorId = idi.UsuarioCreadorId,
                              UsuarioModificadorId = idi.UsuarioModificadorId,
                              UsuarioBajaId = idi.UsuarioBajaId,
                              UsuarioReactivadorId = idi.UsuarioReactivadorId,
                              FechaCreacion = idi.FechaCreacion,
                              FechaModificacion = idi.FechaModificacion,
                              FechaBaja = idi.FechaBaja,
                              FechaReactivacion = idi.FechaReactivacion,
                              Estado = idi.Estado,
                              EmpresaId = idi.EmpresaId,
                              Id = idi.Id,
                              ColaboradorResponsableId = idi.ColaboradorResponsableId,
                              Descripcion = idi.Descripcion,
                              Observaciones = idi.Observaciones,
                              FechaInventario = idi.FechaInventario,
                              Inventariado = idi.Inventariado,
                              NoInventario = idi.NoInventario,
                              ColaboradorResponsable = new Collaborator
                              {
                                  Id = ic.Id,
                                  Nombre = ic.Nombre,
                                  ApellidoPaterno = ic.ApellidoPaterno,
                                  ApellidoMaterno = ic.ApellidoMaterno,
                                  EstadoCivilId = ic.EstadoCivilId,
                                  Genero = ic.Genero,
                                  TipoIdentificacionId = ic.TipoIdentificacionId,
                                  Identificacion = ic.Identificacion,
                                  Foto = ic.Foto,
                                  ExtensionFoto = ic.ExtensionFoto,
                                  NumEmpleado = ic.NumEmpleado,
                                  PuestoId = ic.PuestoId,
                                  UbicacionId = ic.UbicacionId,
                                  AreaId = ic.AreaId,
                                  TipoColaboradorId = ic.TipoColaboradorId,
                                  TelefonoMovil = ic.TelefonoMovil,
                                  TelefonoOficina = ic.TelefonoOficina,
                                  Email = ic.Email,
                                  Email_secundario = ic.Email_secundario
                              },
                              Ubicaciones = locationdi.Where(w => w.Id == id.UbicacionId).Select(s => new Location()
                              {
                                  Id = s.Id,
                                  Nombre = s.Nombre,
                                  DetalleInventario = inventoryDetaildi.Where(a => a.Id == id.Id && s.Id == a.UbicacionId).Select(p => new InventoryDetail()
                                  {
                                      UsuarioCreadorId = p.UsuarioCreadorId,
                                      UsuarioModificadorId = p.UsuarioModificadorId,
                                      UsuarioBajaId = p.UsuarioBajaId,
                                      UsuarioReactivadorId = p.UsuarioReactivadorId,
                                      FechaCreacion = p.FechaCreacion,
                                      FechaModificacion = p.FechaModificacion,
                                      FechaBaja = p.FechaBaja,
                                      FechaReactivacion = p.FechaReactivacion,
                                      Estado = p.Estado,
                                      EmpresaId = p.EmpresaId,
                                      Id = p.Id,
                                      DispositivoId = p.DispositivoId,
                                      InventarioId = p.InventarioId,
                                      CentroDeCostosId = p.CentroDeCostosId,
                                      EstadoFisicoId = p.EstadoFisicoId,
                                      UbicacionId = p.UbicacionId,
                                      ActivoId = p.ActivoId,
                                      PresenciaAusensia = p.PresenciaAusensia,
                                      Observaciones = p.Observaciones,
                                      Mantenimiento = p.Mantenimiento,
                                      Activo = new Asset()
                                      {
                                          UsuarioCreadorId = ida.UsuarioCreadorId,
                                          UsuarioModificadorId = ida.UsuarioModificadorId,
                                          UsuarioBajaId = ida.UsuarioBajaId,
                                          UsuarioReactivadorId = ida.UsuarioReactivadorId,
                                          FechaCreacion = ida.FechaCreacion,
                                          FechaModificacion = ida.FechaModificacion,
                                          FechaBaja = ida.FechaBaja,
                                          FechaReactivacion = ida.FechaReactivacion,
                                          Estado = ida.Estado,
                                          EmpresaId = ida.EmpresaId,
                                          Id = ida.Id,
                                          UbicacionId = ida.UbicacionId,
                                          GrupoActivoId = ida.GrupoActivoId,
                                          TipoActivoId = ida.TipoActivoId,
                                          Codigo = ida.Codigo,
                                          Serie = ida.Serie,
                                          Marca = ida.Marca,
                                          Modelo = ida.Modelo,
                                          Descripcion = ida.Descripcion,
                                          Nombre = ida.Nombre,
                                          Observaciones = ida.Observaciones,
                                          EstadoFisicoId = ida.EstadoFisicoId,
                                          TagId = ida.TagId,
                                          ColaboradorHabitualId = ida.ColaboradorHabitualId,
                                          ColaboradorResponsableId = ida.ColaboradorResponsableId,
                                          ValorCompra = ida.ValorCompra,
                                          FechaCompra = ida.FechaCompra,
                                          Proveedor = ida.Proveedor,
                                          FechaFinGarantia = ida.FechaFinGarantia,
                                          TieneFoto = ida.TieneFoto,
                                          TieneArchivo = ida.TieneArchivo,
                                          FechaCapitalizacion = ida.FechaCapitalizacion,
                                          FichaResguardo = ida.FichaResguardo,
                                          CampoLibre1 = ida.CampoLibre1,
                                          CampoLibre2 = ida.CampoLibre2,
                                          CampoLibre3 = ida.CampoLibre3,
                                          CampoLibre4 = ida.CampoLibre4,
                                          CampoLibre5 = ida.CampoLibre5,
                                          AreaId = ida.AreaId,
                                      },
                                      Dispositivo = new Device()
                                      {
                                          UsuarioCreadorId = idd.UsuarioCreadorId,
                                          UsuarioModificadorId = idd.UsuarioModificadorId,
                                          UsuarioBajaId = idd.UsuarioBajaId,
                                          UsuarioReactivadorId = idd.UsuarioReactivadorId,
                                          FechaCreacion = idd.FechaCreacion,
                                          FechaModificacion = idd.FechaModificacion,
                                          FechaBaja = idd.FechaBaja,
                                          FechaReactivacion = idd.FechaReactivacion,
                                          Estado = idd.Estado,
                                          EmpresaId = idd.EmpresaId,
                                          Id = idd.Id,
                                          Numero = idd.Numero,
                                          Nombre = idd.Nombre,
                                          TipoDispositivoId = idd.TipoDispositivoId,
                                          UbicacionId = idd.UbicacionId,
                                          Ip = idd.Ip,
                                          Mac = idd.Mac,
                                          Identifier = idd.Identifier,
                                          PuertoEscucha = idd.PuertoEscucha,
                                          PuertoTransmision = idd.PuertoTransmision,
                                          HabilitadoParaAlta = idd.HabilitadoParaAlta,
                                          OperacionId = idd.OperacionId,
                                          ModoOperacionId = idd.ModoOperacionId,
                                          EstadoActual = idd.EstadoActual,
                                          Perimetral = idd.Perimetral,
                                          Configurado = idd.Configurado,
                                      },
                                      Inventario = new Inventory()
                                      {
                                          UsuarioCreadorId = idi.UsuarioCreadorId,
                                          UsuarioModificadorId = idi.UsuarioModificadorId,
                                          UsuarioBajaId = idi.UsuarioBajaId,
                                          UsuarioReactivadorId = idi.UsuarioReactivadorId,
                                          FechaCreacion = idi.FechaCreacion,
                                          FechaModificacion = idi.FechaModificacion,
                                          FechaBaja = idi.FechaBaja,
                                          FechaReactivacion = idi.FechaReactivacion,
                                          Estado = idi.Estado,
                                          EmpresaId = idi.EmpresaId,
                                          Id = idi.Id,
                                          ColaboradorResponsableId = idi.ColaboradorResponsableId,
                                          Descripcion = idi.Descripcion,
                                          Observaciones = idi.Observaciones,
                                          FechaInventario = idi.FechaInventario,
                                          Inventariado = idi.Inventariado,
                                          NoInventario = idi.NoInventario
                                      },
                                      Ubicacion = new Location()
                                      {
                                          Id = idl.Id,
                                          Nombre = idl.Nombre
                                      }
                                  }).ToList()
                              }).ToList()
                          };
            */

            /*
            group id by new
            {
                id.Activo,
                id.Dispositivo,
                id.Inventario,
                id.Ubicacion
            } into gby
            select new Inventory
            {
                NoInventario = gby.Key.NoInventario,
                Ubicaciones = 
            };
            */

            /*
            group idd by new
            {
                idi.NoInventario,
            } into gby
            select new Inventory
            {
                NoInventario = gby.Key.NoInventario
            };
            */


            /*
            var collaborator = await _database.QueryAsync<Collaborator>("SELECT * FROM Collaborator");
            var inventory = await _database.QueryAsync<Inventory>("SELECT * FROM Inventory");
            var query = from inv in inventory
                        join c in collaborator on inv.ColaboradorResponsableId equals c.Id
                        into InventoryCollaborator
                        from ic in InventoryCollaborator.DefaultIfEmpty()
                        select new Inventory
                        {
                            UsuarioCreadorId = inv.UsuarioCreadorId,
                            UsuarioModificadorId = inv.UsuarioModificadorId,
                            UsuarioBajaId = inv.UsuarioBajaId,
                            UsuarioReactivadorId = inv.UsuarioReactivadorId,
                            FechaCreacion = inv.FechaCreacion,
                            FechaModificacion = inv.FechaModificacion,
                            FechaBaja = inv.FechaBaja,
                            FechaReactivacion = inv.FechaReactivacion,
                            Estado = inv.Estado,
                            EmpresaId = inv.EmpresaId,
                            Id = inv.Id,
                            ColaboradorResponsableId = inv.ColaboradorResponsableId,
                            Descripcion = inv.Descripcion,
                            Observaciones = inv.Observaciones,
                            FechaInventario = inv.FechaInventario,
                            Inventariado = inv.Inventariado,
                            NoInventario = inv.NoInventario,
                            ColaboradorResponsable = new Collaborator
                            {
                                Id = ic.Id,
                                Nombre = ic.Nombre,
                                ApellidoPaterno = ic.ApellidoPaterno,
                                ApellidoMaterno = ic.ApellidoMaterno,
                                EstadoCivilId = ic.EstadoCivilId,
                                Genero = ic.Genero,
                                TipoIdentificacionId = ic.TipoIdentificacionId,
                                Identificacion = ic.Identificacion,
                                Foto = ic.Foto,
                                ExtensionFoto = ic.ExtensionFoto,
                                NumEmpleado = ic.NumEmpleado,
                                PuestoId = ic.PuestoId,
                                UbicacionId = ic.UbicacionId,
                                AreaId = ic.AreaId,
                                TipoColaboradorId = ic.TipoColaboradorId,
                                TelefonoMovil = ic.TelefonoMovil,
                                TelefonoOficina = ic.TelefonoOficina,
                                Email = ic.Email,
                                Email_secundario = ic.Email_secundario,
                            }
                        };
            var list = query.ToList();
            */

            //var io = await _database.QueryAsync<Inventory>(@"SELECT * FROM Inventory AS i inner join Collaborator AS c on i.ColaboradorResponsableId = c.Id");
            //var ds = io.ToList();
            return await Task.FromResult(querydi.ToList());
        }

        public async Task<Inventory> GetByIdAsync(Guid id)
        {
            return await _database.Table<Inventory>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Inventory data)
        {
            bool ok = false;

            try
            {
                await _database.UpdateAsync(data);
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<List<InventoryLoad>> GetAllLoad()
        {
            var collaboratordi = await App.collaboratorRepository.GetAllAsync();
            //var collaboratordi = await _database.QueryAsync<Collaborator>("SELECT * FROM Collaborator");

            var assetdi = await App.assetRepository.GetAllAsync();
            //var assetdi = await _database.QueryAsync<Asset>("SELECT * FROM Asset");

            var devicedi = await App.deviceRepository.GetAllAsync();
            //var devicedi = await _database.QueryAsync<Device>("SELECT * FROM Device");

            //var inventorydi = await App.inventoryRepository.GetAllAsync();
            var inventorydi = await _database.QueryAsync<Inventory>("SELECT * FROM Inventory");

            var locationdi = await App.locationRepository.GetAllAsync();
            //var locationdi = await _database.QueryAsync<Location>("SELECT * FROM Location");

            var inventoryDetaildi = await App.inventoryDetailRepository.GetAllAsync();
            //var inventoryDetaildi = await _database.QueryAsync<InventoryDetail>("SELECT * FROM InventoryDetail");

            var physicalStatedi = await App.physicalStateRepository.GetAllAsync();

            var querydi = (from i in inventorydi

                           join c in collaboratordi on i.ColaboradorResponsableId equals c.Id
                           into InventoryCollaborator
                           from cic in InventoryCollaborator.DefaultIfEmpty()

                           join id in inventoryDetaildi on i.Id equals id.InventarioId
                           into InventoryDetailInventory
                           from ididi in InventoryDetailInventory.DefaultIfEmpty()

                           group i by new { i, cic } into g

                           select new InventoryLoad()
                           {
                               Id = g.Key.i.Id,
                               Observaciones = g.Key.i.Observaciones,
                               FechaInventario = g.Key.i.FechaInventario,
                               DetalleInventario = inventoryDetaildi.Where(a => a.InventarioId == g.Key.i.Id).Select(q => new InventoryDetailLoad()
                               {
                                   Id = q.Id,
                                   ActivoId = q.ActivoId,
                                   Observaciones = q.Observaciones,
                                   EstadoFisicoId = q.EstadoFisicoId,
                                   UbicacionId = q.UbicacionId,
                                   Ubicacion = locationdi.FirstOrDefault(p => p.Id == q.UbicacionId)
                               }).Where(x => x.Ubicacion.Status == 2).ToList(),
                               //}).Where(x => x.Ubicacion.Status == 2).ToList()
                               DetalleInventarioTotal = inventoryDetaildi.Where(a => a.InventarioId == g.Key.i.Id).Select(q => new InventoryDetailLoad()
                               {
                                   Id = q.Id,
                                   ActivoId = q.ActivoId,
                                   Observaciones = q.Observaciones,
                                   EstadoFisicoId = q.EstadoFisicoId,
                                   UbicacionId = q.UbicacionId,
                                   Ubicacion = locationdi.FirstOrDefault(p => p.Id == q.UbicacionId)
                               }).ToList().Count(),
                           }).ToList();

            return await Task.FromResult(querydi);
        }
    }
}