using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.Repositories.Interfaces;

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
                                   Ubicaciones = inventoryDetaildi.Where(a => a.InventarioId == g.Key.i.Id).GroupBy(ss => new { ss.Ubicacion, ss.UbicacionId }).Select(s => new Location()
                                   {
                                       Id = locationdi.FirstOrDefault(f => f.Id == s.Key.UbicacionId).Id,
                                       Nombre = locationdi.FirstOrDefault(f => f.Id == s.Key.UbicacionId).Nombre,
                                       Status = locationdi.FirstOrDefault(f => f.Id == s.Key.UbicacionId).Status,
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
                                               Status = assetdi.FirstOrDefault(x => x.Id == q.ActivoId).Status,
                                               //EstadoFisico = physicalStatedi.FirstOrDefault(x => x.Id == q.ActivoId)
                                               EstadoFisico = physicalStatedi.FirstOrDefault(x => x.Id == assetdi.FirstOrDefault(x => x.Id == q.ActivoId).EstadoFisicoId)
                                           }
                                       }).Where(y => y.UbicacionId == s.Key.UbicacionId).ToList()
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
                               NoInventario = g.Key.i.NoInventario,
                               DetalleInventario = inventoryDetaildi.Where(a => a.InventarioId == g.Key.i.Id).Select(q => new InventoryDetailLoad()
                               {
                                   Id = q.Id,
                                   ActivoId = q.ActivoId,
                                   Observaciones = q.Observaciones,
                                   EstadoFisicoId = q.EstadoFisicoId,
                                   UbicacionId = q.UbicacionId,
                                   //NoInventario = q.Inventario.NoInventario,
                                   Ubicacion = locationdi.FirstOrDefault(p => p.Id == q.UbicacionId)
                               }).ToList(),
                               //}).Where(x => x.Ubicacion.Status == 2).ToList()
                               DetalleInventarioTotal = inventoryDetaildi.Where(a => a.InventarioId == g.Key.i.Id).Select(q => new InventoryDetailLoad()
                               {
                                   Id = q.Id,
                                   ActivoId = q.ActivoId,
                                   Observaciones = q.Observaciones,
                                   EstadoFisicoId = q.EstadoFisicoId,
                                   UbicacionId = q.UbicacionId,
                                   //NoInventario = q.Inventario.NoInventario,
                                   Ubicacion = locationdi.FirstOrDefault(p => p.Id == q.UbicacionId)
                               }).ToList().Count(),
                           }).ToList();

            return await Task.FromResult(querydi);
        }

        public async Task<InventoryLoadCount> GetAllCountLoad(Guid inventoryId)
        {
            var collaboratordi = await App.collaboratorRepository.GetAllAsync();

            var inventorylocationdi = await App.inventoryLocationRepository.GetAllAsync();

            var inventorylocationassetdi = await App.inventoryLocationAssetRepository.GetAllAsync();

            var assetdi = await App.assetRepository.GetAllAsync();

            var inventorydi = await _database.QueryAsync<Inventory>("SELECT * FROM Inventory");

            var locationdi = await App.locationRepository.GetAllAsync();

            var inventoryDetaildi = await App.inventoryDetailRepository.GetAllAsync();

            var physicalStatedi = await App.physicalStateRepository.GetAllAsync();


            var queryila = (from ila in inventorylocationassetdi

                            join il in inventorylocationdi on ila.InventarioUbicacionId equals il.Id
                            into InventoryLocation_InventoryLocationAsset
                            from ilila in InventoryLocation_InventoryLocationAsset.DefaultIfEmpty()

                            join i in inventorydi on ilila.InventarioId equals i.Id
                            into Inventory_InventoryLocation
                            from iil in Inventory_InventoryLocation.DefaultIfEmpty()

                            join c in collaboratordi on iil.ColaboradorResponsableId equals c.Id
                            into Collaborator_Inventory
                            from ci in Collaborator_Inventory.DefaultIfEmpty()

                            join l in locationdi on ilila.UbicacionId equals l.Id
                            into Location_InventoryLocation
                            from lil in Location_InventoryLocation.DefaultIfEmpty()




                            join a in assetdi on ila.ActivoId equals a.Id
                            into Asset_InventoryLocationAsset
                            from aila in Asset_InventoryLocationAsset.DefaultIfEmpty()

                            join ps in physicalStatedi on aila.EstadoFisicoId equals ps.Id
                            into PhysicalState_Asset
                            from psa in PhysicalState_Asset.DefaultIfEmpty()

                                /*
                                join p in paramsdi on aila.MotivoId equals p.Id
                                into Params_Asset
                                from pa in Params_Asset.DefaultIfEmpty()
                                */

                            select new InventoryLocationAsset()
                            {
                                Id = ila.Id,
                                InventarioUbicacionId = ila.InventarioUbicacionId,
                                ActivoId = ila.ActivoId,
                                InventarioUbicacion = new InventoryLocation()
                                {
                                    Id = ilila.Id,
                                    InventarioId = ilila.InventarioId,
                                    UbicacionId = ilila.UbicacionId,
                                    Status = ilila.Status,
                                    Inventario = new Inventory()
                                    {
                                        UsuarioCreadorId = iil.UsuarioCreadorId,
                                        UsuarioModificadorId = iil.UsuarioModificadorId,
                                        UsuarioBajaId = iil.UsuarioBajaId,
                                        UsuarioReactivadorId = iil.UsuarioReactivadorId,
                                        FechaCreacion = iil.FechaCreacion,
                                        FechaModificacion = iil.FechaModificacion,
                                        FechaBaja = iil.FechaBaja,
                                        FechaReactivacion = iil.FechaReactivacion,
                                        Estado = iil.Estado,
                                        EmpresaId = iil.EmpresaId,
                                        Id = iil.Id,
                                        ColaboradorResponsableId = iil.ColaboradorResponsableId,
                                        Descripcion = iil.Descripcion,
                                        Observaciones = iil.Observaciones,
                                        FechaInventario = iil.FechaInventario,
                                        Inventariado = iil.Inventariado,
                                        NoInventario = iil.NoInventario,
                                        ColaboradorResponsable = new Collaborator()
                                        {
                                            Id = ci.Id,
                                            Nombre = ci.Nombre,
                                            ApellidoPaterno = ci.ApellidoPaterno,
                                            ApellidoMaterno = ci.ApellidoMaterno,
                                            EstadoCivilId = ci.EstadoCivilId,
                                            Genero = ci.Genero,
                                            TipoIdentificacionId = ci.TipoIdentificacionId,
                                            Identificacion = ci.Identificacion,
                                            Foto = ci.Foto,
                                            ExtensionFoto = ci.ExtensionFoto,
                                            NumEmpleado = ci.NumEmpleado,
                                            PuestoId = ci.PuestoId,
                                            UbicacionId = ci.UbicacionId,
                                            AreaId = ci.AreaId,
                                            TipoColaboradorId = ci.TipoColaboradorId,
                                            TelefonoMovil = ci.TelefonoMovil,
                                            TelefonoOficina = ci.TelefonoOficina,
                                            Email = ci.Email,
                                            Email_secundario = ci.Email_secundario
                                        }
                                    },
                                    Ubicacion = new Location()
                                    {
                                        Id = lil.Id,
                                        Nombre = lil.Nombre,
                                        Status = lil.Status
                                    }
                                },
                                Activo = new Asset()
                                {
                                    UsuarioCreadorId = aila.UsuarioCreadorId,
                                    UsuarioModificadorId = aila.UsuarioModificadorId,
                                    UsuarioBajaId = aila.UsuarioBajaId,
                                    UsuarioReactivadorId = aila.UsuarioReactivadorId,
                                    FechaCreacion = aila.FechaCreacion,
                                    FechaModificacion = aila.FechaModificacion,
                                    FechaBaja = aila.FechaBaja,
                                    FechaReactivacion = aila.FechaReactivacion,
                                    Estado = aila.Estado,
                                    EmpresaId = aila.EmpresaId,
                                    Id = aila.Id,
                                    UbicacionId = aila.UbicacionId,
                                    GrupoActivoId = aila.GrupoActivoId,
                                    TipoActivoId = aila.TipoActivoId,
                                    Codigo = aila.Codigo,
                                    Serie = aila.Serie,
                                    Marca = aila.Marca,
                                    Modelo = aila.Modelo,
                                    Descripcion = aila.Descripcion,
                                    Nombre = aila.Nombre,
                                    Observaciones = aila.Observaciones,
                                    EstadoFisicoId = aila.EstadoFisicoId,
                                    TagId = aila.TagId,
                                    ColaboradorHabitualId = aila.ColaboradorHabitualId,
                                    ColaboradorResponsableId = aila.ColaboradorResponsableId,
                                    ValorCompra = aila.ValorCompra,
                                    FechaCompra = aila.FechaCompra,
                                    Proveedor = aila.Proveedor,
                                    FechaFinGarantia = aila.FechaFinGarantia,
                                    TieneFoto = aila.TieneFoto,
                                    TieneArchivo = aila.TieneArchivo,
                                    FechaCapitalizacion = aila.FechaCapitalizacion,
                                    FichaResguardo = aila.FichaResguardo,
                                    CampoLibre1 = aila.CampoLibre1,
                                    CampoLibre2 = aila.CampoLibre2,
                                    CampoLibre3 = aila.CampoLibre3,
                                    CampoLibre4 = aila.CampoLibre4,
                                    CampoLibre5 = aila.CampoLibre5,
                                    AreaId = aila.AreaId,
                                    EstadoFisico = new PhysicalState()
                                    {
                                        UsuarioCreadorId = psa.UsuarioCreadorId,
                                        UsuarioModificadorId = psa.UsuarioModificadorId,
                                        UsuarioBajaId = psa.UsuarioBajaId,
                                        UsuarioReactivadorId = psa.UsuarioReactivadorId,
                                        FechaCreacion = psa.FechaCreacion,
                                        FechaModificacion = psa.FechaModificacion,
                                        FechaBaja = psa.FechaBaja,
                                        FechaReactivacion = psa.FechaReactivacion,
                                        Estado = psa.Estado,
                                        EmpresaId = psa.EmpresaId,
                                        Id = psa.Id,
                                        Nombre = psa.Nombre,
                                        Descripcion = psa.Descripcion
                                    },
                                    /*
                                    Motivo = new Models.Setting.Params()
                                    {
                                        UsuarioCreadorId = pa.UsuarioCreadorId,
                                        UsuarioModificadorId = pa.UsuarioModificadorId,
                                        UsuarioBajaId = pa.UsuarioBajaId,
                                        UsuarioReactivadorId = pa.UsuarioReactivadorId,
                                        FechaCreacion = pa.FechaCreacion,
                                        FechaModificacion = pa.FechaModificacion,
                                        FechaBaja = pa.FechaBaja,
                                        FechaReactivacion = pa.FechaReactivacion,
                                        Estado = pa.Estado,
                                        EmpresaId = pa.EmpresaId,
                                        Id = pa.Id,
                                        Nombre = pa.Nombre,
                                        TipoParamId = pa.TipoParamId,
                                        Orden = pa.Orden
                                    }
                                    */
                                },

                            }).ToList();


            var query = (from q in queryila

                         group q by new
                         {
                             q.InventarioUbicacion.Inventario.Id,
                         } into g
                         select new InventoryLoadCount()
                         {
                             InventarioId = g.Key.Id,
                             UbicacionTotal = g.GroupBy(u => new
                             {
                                 u.InventarioUbicacion.Ubicacion.Id,
                                 u.InventarioUbicacion.Ubicacion.Nombre,
                                 u.InventarioUbicacion.Status
                             }).Select(l => new InventoryLocationAQ()
                             {
                                 UbicacionId = l.Key.Id,
                                 UbicacionNombre = l.Key.Nombre,
                                 InventarioUbicacionStatus = l.Key.Status,
                                 Activo = l.GroupBy(w => new
                                 {
                                     w.Activo
                                 }).Select(t => new Asset()
                                 {
                                     UsuarioCreadorId = t.Key.Activo.UsuarioCreadorId,
                                     UsuarioModificadorId = t.Key.Activo.UsuarioModificadorId,
                                     UsuarioBajaId = t.Key.Activo.UsuarioBajaId,
                                     UsuarioReactivadorId = t.Key.Activo.UsuarioReactivadorId,
                                     FechaCreacion = t.Key.Activo.FechaCreacion,
                                     FechaModificacion = t.Key.Activo.FechaModificacion,
                                     FechaBaja = t.Key.Activo.FechaBaja,
                                     FechaReactivacion = t.Key.Activo.FechaReactivacion,
                                     Estado = t.Key.Activo.Estado,
                                     EmpresaId = t.Key.Activo.EmpresaId,
                                     Id = t.Key.Activo.Id,
                                     UbicacionId = t.Key.Activo.UbicacionId,
                                     GrupoActivoId = t.Key.Activo.GrupoActivoId,
                                     TipoActivoId = t.Key.Activo.TipoActivoId,
                                     Codigo = t.Key.Activo.Codigo,
                                     Serie = t.Key.Activo.Serie,
                                     Marca = t.Key.Activo.Marca,
                                     Modelo = t.Key.Activo.Modelo,
                                     Descripcion = t.Key.Activo.Descripcion,
                                     Nombre = t.Key.Activo.Nombre,
                                     Observaciones = t.Key.Activo.Observaciones,
                                     EstadoFisicoId = t.Key.Activo.EstadoFisicoId,
                                     TagId = t.Key.Activo.TagId,
                                     ColaboradorHabitualId = t.Key.Activo.ColaboradorHabitualId,
                                     ColaboradorResponsableId = t.Key.Activo.ColaboradorResponsableId,
                                     ValorCompra = t.Key.Activo.ValorCompra,
                                     FechaCompra = t.Key.Activo.FechaCompra,
                                     Proveedor = t.Key.Activo.Proveedor,
                                     FechaFinGarantia = t.Key.Activo.FechaFinGarantia,
                                     TieneFoto = t.Key.Activo.TieneFoto,
                                     TieneArchivo = t.Key.Activo.TieneArchivo,
                                     FechaCapitalizacion = t.Key.Activo.FechaCapitalizacion,
                                     FichaResguardo = t.Key.Activo.FichaResguardo,
                                     CampoLibre1 = t.Key.Activo.CampoLibre1,
                                     CampoLibre2 = t.Key.Activo.CampoLibre2,
                                     CampoLibre3 = t.Key.Activo.CampoLibre3,
                                     CampoLibre4 = t.Key.Activo.CampoLibre4,
                                     CampoLibre5 = t.Key.Activo.CampoLibre5,
                                     AreaId = t.Key.Activo.AreaId,
                                 }).ToList()
                             }).ToList().Count(),
                             UbicacionPorInventariar = g.GroupBy(u => new
                             {
                                 u.InventarioUbicacion.Ubicacion.Id,
                                 u.InventarioUbicacion.Ubicacion.Nombre,
                                 u.InventarioUbicacion.Status
                             }).Select(l => new InventoryLocationAQ()
                             {
                                 UbicacionId = l.Key.Id,
                                 UbicacionNombre = l.Key.Nombre,
                                 InventarioUbicacionStatus = l.Key.Status,
                                 Activo = l.GroupBy(w => new
                                 {
                                     w.Activo
                                 }).Select(t => new Asset()
                                 {
                                     UsuarioCreadorId = t.Key.Activo.UsuarioCreadorId,
                                     UsuarioModificadorId = t.Key.Activo.UsuarioModificadorId,
                                     UsuarioBajaId = t.Key.Activo.UsuarioBajaId,
                                     UsuarioReactivadorId = t.Key.Activo.UsuarioReactivadorId,
                                     FechaCreacion = t.Key.Activo.FechaCreacion,
                                     FechaModificacion = t.Key.Activo.FechaModificacion,
                                     FechaBaja = t.Key.Activo.FechaBaja,
                                     FechaReactivacion = t.Key.Activo.FechaReactivacion,
                                     Estado = t.Key.Activo.Estado,
                                     EmpresaId = t.Key.Activo.EmpresaId,
                                     Id = t.Key.Activo.Id,
                                     UbicacionId = t.Key.Activo.UbicacionId,
                                     GrupoActivoId = t.Key.Activo.GrupoActivoId,
                                     TipoActivoId = t.Key.Activo.TipoActivoId,
                                     Codigo = t.Key.Activo.Codigo,
                                     Serie = t.Key.Activo.Serie,
                                     Marca = t.Key.Activo.Marca,
                                     Modelo = t.Key.Activo.Modelo,
                                     Descripcion = t.Key.Activo.Descripcion,
                                     Nombre = t.Key.Activo.Nombre,
                                     Observaciones = t.Key.Activo.Observaciones,
                                     EstadoFisicoId = t.Key.Activo.EstadoFisicoId,
                                     TagId = t.Key.Activo.TagId,
                                     ColaboradorHabitualId = t.Key.Activo.ColaboradorHabitualId,
                                     ColaboradorResponsableId = t.Key.Activo.ColaboradorResponsableId,
                                     ValorCompra = t.Key.Activo.ValorCompra,
                                     FechaCompra = t.Key.Activo.FechaCompra,
                                     Proveedor = t.Key.Activo.Proveedor,
                                     FechaFinGarantia = t.Key.Activo.FechaFinGarantia,
                                     TieneFoto = t.Key.Activo.TieneFoto,
                                     TieneArchivo = t.Key.Activo.TieneArchivo,
                                     FechaCapitalizacion = t.Key.Activo.FechaCapitalizacion,
                                     FichaResguardo = t.Key.Activo.FichaResguardo,
                                     CampoLibre1 = t.Key.Activo.CampoLibre1,
                                     CampoLibre2 = t.Key.Activo.CampoLibre2,
                                     CampoLibre3 = t.Key.Activo.CampoLibre3,
                                     CampoLibre4 = t.Key.Activo.CampoLibre4,
                                     CampoLibre5 = t.Key.Activo.CampoLibre5,
                                     AreaId = t.Key.Activo.AreaId,
                                 }).ToList()
                             }).Where(oo => oo.InventarioUbicacionStatus == 1).ToList().Count(),
                             UbicacionFinalizada = g.GroupBy(u => new
                             {
                                 u.InventarioUbicacion.Ubicacion.Id,
                                 u.InventarioUbicacion.Ubicacion.Nombre,
                                 u.InventarioUbicacion.Status
                             }).Select(l => new InventoryLocationAQ()
                             {
                                 UbicacionId = l.Key.Id,
                                 UbicacionNombre = l.Key.Nombre,
                                 InventarioUbicacionStatus = l.Key.Status,
                                 Activo = l.GroupBy(w => new
                                 {
                                     w.Activo
                                 }).Select(t => new Asset()
                                 {
                                     UsuarioCreadorId = t.Key.Activo.UsuarioCreadorId,
                                     UsuarioModificadorId = t.Key.Activo.UsuarioModificadorId,
                                     UsuarioBajaId = t.Key.Activo.UsuarioBajaId,
                                     UsuarioReactivadorId = t.Key.Activo.UsuarioReactivadorId,
                                     FechaCreacion = t.Key.Activo.FechaCreacion,
                                     FechaModificacion = t.Key.Activo.FechaModificacion,
                                     FechaBaja = t.Key.Activo.FechaBaja,
                                     FechaReactivacion = t.Key.Activo.FechaReactivacion,
                                     Estado = t.Key.Activo.Estado,
                                     EmpresaId = t.Key.Activo.EmpresaId,
                                     Id = t.Key.Activo.Id,
                                     UbicacionId = t.Key.Activo.UbicacionId,
                                     GrupoActivoId = t.Key.Activo.GrupoActivoId,
                                     TipoActivoId = t.Key.Activo.TipoActivoId,
                                     Codigo = t.Key.Activo.Codigo,
                                     Serie = t.Key.Activo.Serie,
                                     Marca = t.Key.Activo.Marca,
                                     Modelo = t.Key.Activo.Modelo,
                                     Descripcion = t.Key.Activo.Descripcion,
                                     Nombre = t.Key.Activo.Nombre,
                                     Observaciones = t.Key.Activo.Observaciones,
                                     EstadoFisicoId = t.Key.Activo.EstadoFisicoId,
                                     TagId = t.Key.Activo.TagId,
                                     ColaboradorHabitualId = t.Key.Activo.ColaboradorHabitualId,
                                     ColaboradorResponsableId = t.Key.Activo.ColaboradorResponsableId,
                                     ValorCompra = t.Key.Activo.ValorCompra,
                                     FechaCompra = t.Key.Activo.FechaCompra,
                                     Proveedor = t.Key.Activo.Proveedor,
                                     FechaFinGarantia = t.Key.Activo.FechaFinGarantia,
                                     TieneFoto = t.Key.Activo.TieneFoto,
                                     TieneArchivo = t.Key.Activo.TieneArchivo,
                                     FechaCapitalizacion = t.Key.Activo.FechaCapitalizacion,
                                     FichaResguardo = t.Key.Activo.FichaResguardo,
                                     CampoLibre1 = t.Key.Activo.CampoLibre1,
                                     CampoLibre2 = t.Key.Activo.CampoLibre2,
                                     CampoLibre3 = t.Key.Activo.CampoLibre3,
                                     CampoLibre4 = t.Key.Activo.CampoLibre4,
                                     CampoLibre5 = t.Key.Activo.CampoLibre5,
                                     AreaId = t.Key.Activo.AreaId,
                                 }).ToList()
                             }).Where(oo => oo.InventarioUbicacionStatus == 2).ToList().Count(),
                             Ubicacion = g.GroupBy(u => new
                             {
                                 u.InventarioUbicacion.Ubicacion.Id,
                                 u.InventarioUbicacion.Ubicacion.Nombre,
                                 u.InventarioUbicacion.Status
                             }).Select(l => new InventoryLocationAQ()
                             {
                                 UbicacionId = l.Key.Id,
                                 UbicacionNombre = l.Key.Nombre,
                                 InventarioUbicacionStatus = l.Key.Status,
                                 Activo = l.GroupBy(w => new
                                 {
                                     w.Activo
                                 }).Select(t => new Asset()
                                 {
                                     UsuarioCreadorId = t.Key.Activo.UsuarioCreadorId,
                                     UsuarioModificadorId = t.Key.Activo.UsuarioModificadorId,
                                     UsuarioBajaId = t.Key.Activo.UsuarioBajaId,
                                     UsuarioReactivadorId = t.Key.Activo.UsuarioReactivadorId,
                                     FechaCreacion = t.Key.Activo.FechaCreacion,
                                     FechaModificacion = t.Key.Activo.FechaModificacion,
                                     FechaBaja = t.Key.Activo.FechaBaja,
                                     FechaReactivacion = t.Key.Activo.FechaReactivacion,
                                     Estado = t.Key.Activo.Estado,
                                     EmpresaId = t.Key.Activo.EmpresaId,
                                     Id = t.Key.Activo.Id,
                                     UbicacionId = t.Key.Activo.UbicacionId,
                                     GrupoActivoId = t.Key.Activo.GrupoActivoId,
                                     TipoActivoId = t.Key.Activo.TipoActivoId,
                                     Codigo = t.Key.Activo.Codigo,
                                     Serie = t.Key.Activo.Serie,
                                     Marca = t.Key.Activo.Marca,
                                     Modelo = t.Key.Activo.Modelo,
                                     Descripcion = t.Key.Activo.Descripcion,
                                     Nombre = t.Key.Activo.Nombre,
                                     Observaciones = t.Key.Activo.Observaciones,
                                     EstadoFisicoId = t.Key.Activo.EstadoFisicoId,
                                     TagId = t.Key.Activo.TagId,
                                     ColaboradorHabitualId = t.Key.Activo.ColaboradorHabitualId,
                                     ColaboradorResponsableId = t.Key.Activo.ColaboradorResponsableId,
                                     ValorCompra = t.Key.Activo.ValorCompra,
                                     FechaCompra = t.Key.Activo.FechaCompra,
                                     Proveedor = t.Key.Activo.Proveedor,
                                     FechaFinGarantia = t.Key.Activo.FechaFinGarantia,
                                     TieneFoto = t.Key.Activo.TieneFoto,
                                     TieneArchivo = t.Key.Activo.TieneArchivo,
                                     FechaCapitalizacion = t.Key.Activo.FechaCapitalizacion,
                                     FichaResguardo = t.Key.Activo.FichaResguardo,
                                     CampoLibre1 = t.Key.Activo.CampoLibre1,
                                     CampoLibre2 = t.Key.Activo.CampoLibre2,
                                     CampoLibre3 = t.Key.Activo.CampoLibre3,
                                     CampoLibre4 = t.Key.Activo.CampoLibre4,
                                     CampoLibre5 = t.Key.Activo.CampoLibre5,
                                     AreaId = t.Key.Activo.AreaId,
                                 }).ToList()
                             }).ToList()
                         }).ToList();

            return await Task.FromResult(query.FirstOrDefault(l => l.InventarioId == inventoryId));
        }

        public async Task<List<InventoryLocationAssetQuery>> GetAllByInventoryLocationAssetAsync()
        {
            var inventorylocationdi = await App.inventoryLocationRepository.GetAllAsync();

            var inventorylocationassetdi = await App.inventoryLocationAssetRepository.GetAllAsync();

            var paramsdi = await App.paramsRepositoty.GetAllAsync();

            var collaboratordi = await App.collaboratorRepository.GetAllAsync();

            var assetdi = await App.assetRepository.GetAllAsync();

            var devicedi = await App.deviceRepository.GetAllAsync();

            var inventorydi = await _database.QueryAsync<Inventory>("SELECT * FROM Inventory");

            var locationdi = await App.locationRepository.GetAllAsync();

            var inventoryDetaildi = await App.inventoryDetailRepository.GetAllAsync();

            var physicalStatedi = await App.physicalStateRepository.GetAllAsync();

            var tagi = await App.tagRepository.GetAllAsync();

            var queryila = (from ila in inventorylocationassetdi

                            join il in inventorylocationdi on ila.InventarioUbicacionId equals il.Id
                            into InventoryLocation_InventoryLocationAsset
                            from ilila in InventoryLocation_InventoryLocationAsset.DefaultIfEmpty()

                            join i in inventorydi on ilila.InventarioId equals i.Id
                            into Inventory_InventoryLocation
                            from iil in Inventory_InventoryLocation.DefaultIfEmpty()

                            join c in collaboratordi on iil.ColaboradorResponsableId equals c.Id
                            into Collaborator_Inventory
                            from ci in Collaborator_Inventory.DefaultIfEmpty()

                            join l in locationdi on ilila.UbicacionId equals l.Id
                            into Location_InventoryLocation
                            from lil in Location_InventoryLocation.DefaultIfEmpty()




                            join a in assetdi on ila.ActivoId equals a.Id
                            into Asset_InventoryLocationAsset
                            from aila in Asset_InventoryLocationAsset.DefaultIfEmpty()

                            join ps in physicalStatedi on aila.EstadoFisicoId equals ps.Id
                            into PhysicalState_Asset
                            from psa in PhysicalState_Asset.DefaultIfEmpty()

                            join t in tagi on aila.TagId equals t.Id
                            into Tag_Asset
                            from ta in Tag_Asset.DefaultIfEmpty()


                                /*
                                join p in paramsdi on aila.MotivoId equals p.Id
                                into Params_Asset
                                from pa in Params_Asset.DefaultIfEmpty()
                                */

                            select new InventoryLocationAsset()
                            {
                                Id = ila.Id,
                                InventarioUbicacionId = ila.InventarioUbicacionId,
                                ActivoId = ila.ActivoId,
                                InventarioUbicacion = new InventoryLocation()
                                {
                                    Id = ilila.Id,
                                    InventarioId = ilila.InventarioId,
                                    UbicacionId = ilila.UbicacionId,
                                    Status = ilila.Status,
                                    Inventario = new Inventory()
                                    {
                                        UsuarioCreadorId = iil.UsuarioCreadorId,
                                        UsuarioModificadorId = iil.UsuarioModificadorId,
                                        UsuarioBajaId = iil.UsuarioBajaId,
                                        UsuarioReactivadorId = iil.UsuarioReactivadorId,
                                        FechaCreacion = iil.FechaCreacion,
                                        FechaModificacion = iil.FechaModificacion,
                                        FechaBaja = iil.FechaBaja,
                                        FechaReactivacion = iil.FechaReactivacion,
                                        Estado = iil.Estado,
                                        EmpresaId = iil.EmpresaId,
                                        Id = iil.Id,
                                        ColaboradorResponsableId = iil.ColaboradorResponsableId,
                                        Descripcion = iil.Descripcion,
                                        Observaciones = iil.Observaciones,
                                        FechaInventario = iil.FechaInventario,
                                        Inventariado = iil.Inventariado,
                                        NoInventario = iil.NoInventario,
                                        ColaboradorResponsable = new Collaborator()
                                        {
                                            Id = ci.Id,
                                            Nombre = ci.Nombre,
                                            ApellidoPaterno = ci.ApellidoPaterno,
                                            ApellidoMaterno = ci.ApellidoMaterno,
                                            EstadoCivilId = ci.EstadoCivilId,
                                            Genero = ci.Genero,
                                            TipoIdentificacionId = ci.TipoIdentificacionId,
                                            Identificacion = ci.Identificacion,
                                            Foto = ci.Foto,
                                            ExtensionFoto = ci.ExtensionFoto,
                                            NumEmpleado = ci.NumEmpleado,
                                            PuestoId = ci.PuestoId,
                                            UbicacionId = ci.UbicacionId,
                                            AreaId = ci.AreaId,
                                            TipoColaboradorId = ci.TipoColaboradorId,
                                            TelefonoMovil = ci.TelefonoMovil,
                                            TelefonoOficina = ci.TelefonoOficina,
                                            Email = ci.Email,
                                            Email_secundario = ci.Email_secundario
                                        }
                                    },
                                    Ubicacion = new Location()
                                    {
                                        Id = lil.Id,
                                        Nombre = lil.Nombre,
                                        Status = lil.Status
                                    }
                                },
                                Activo = new Asset()
                                {
                                    UsuarioCreadorId = aila.UsuarioCreadorId,
                                    UsuarioModificadorId = aila.UsuarioModificadorId,
                                    UsuarioBajaId = aila.UsuarioBajaId,
                                    UsuarioReactivadorId = aila.UsuarioReactivadorId,
                                    FechaCreacion = aila.FechaCreacion,
                                    FechaModificacion = aila.FechaModificacion,
                                    FechaBaja = aila.FechaBaja,
                                    FechaReactivacion = aila.FechaReactivacion,
                                    Estado = aila.Estado,
                                    EmpresaId = aila.EmpresaId,
                                    Id = aila.Id,
                                    UbicacionId = aila.UbicacionId,
                                    GrupoActivoId = aila.GrupoActivoId,
                                    TipoActivoId = aila.TipoActivoId,
                                    Codigo = aila.Codigo,
                                    Serie = aila.Serie,
                                    Marca = aila.Marca,
                                    Modelo = aila.Modelo,
                                    Descripcion = aila.Descripcion,
                                    Nombre = aila.Nombre,
                                    Observaciones = aila.Observaciones,
                                    EstadoFisicoId = aila.EstadoFisicoId,
                                    TagId = aila.TagId,
                                    ColaboradorHabitualId = aila.ColaboradorHabitualId,
                                    ColaboradorResponsableId = aila.ColaboradorResponsableId,
                                    ValorCompra = aila.ValorCompra,
                                    FechaCompra = aila.FechaCompra,
                                    Proveedor = aila.Proveedor,
                                    FechaFinGarantia = aila.FechaFinGarantia,
                                    TieneFoto = aila.TieneFoto,
                                    TieneArchivo = aila.TieneArchivo,
                                    FechaCapitalizacion = aila.FechaCapitalizacion,
                                    FichaResguardo = aila.FichaResguardo,
                                    CampoLibre1 = aila.CampoLibre1,
                                    CampoLibre2 = aila.CampoLibre2,
                                    CampoLibre3 = aila.CampoLibre3,
                                    CampoLibre4 = aila.CampoLibre4,
                                    CampoLibre5 = aila.CampoLibre5,
                                    AreaId = aila.AreaId,
                                    EstadoFisico = new PhysicalState()
                                    {
                                        UsuarioCreadorId = psa.UsuarioCreadorId,
                                        UsuarioModificadorId = psa.UsuarioModificadorId,
                                        UsuarioBajaId = psa.UsuarioBajaId,
                                        UsuarioReactivadorId = psa.UsuarioReactivadorId,
                                        FechaCreacion = psa.FechaCreacion,
                                        FechaModificacion = psa.FechaModificacion,
                                        FechaBaja = psa.FechaBaja,
                                        FechaReactivacion = psa.FechaReactivacion,
                                        Estado = psa.Estado,
                                        EmpresaId = psa.EmpresaId,
                                        Id = psa.Id,
                                        Nombre = psa.Nombre,
                                        Descripcion = psa.Descripcion
                                    },
                                    Tag = new Tag()
                                    {
                                        UsuarioCreadorId = ta.UsuarioCreadorId,
                                        UsuarioModificadorId = ta.UsuarioModificadorId,
                                        UsuarioBajaId = ta.UsuarioBajaId,
                                        UsuarioReactivadorId = ta.UsuarioReactivadorId,
                                        FechaCreacion = ta.FechaCreacion,
                                        FechaModificacion = ta.FechaModificacion,
                                        FechaBaja = ta.FechaBaja,
                                        FechaReactivacion = ta.FechaReactivacion,
                                        Estado = ta.Estado,
                                        EmpresaId = ta.EmpresaId,
                                        Id = ta.Id,
                                        TipoTagId = ta.TipoTagId,
                                        Numero = ta.Numero,
                                        Fc = ta.Fc,
                                        Vence = ta.Vence
                                    }

                                    /*
                                    Motivo = new Models.Setting.Params()
                                    {
                                        UsuarioCreadorId = pa.UsuarioCreadorId,
                                        UsuarioModificadorId = pa.UsuarioModificadorId,
                                        UsuarioBajaId = pa.UsuarioBajaId,
                                        UsuarioReactivadorId = pa.UsuarioReactivadorId,
                                        FechaCreacion = pa.FechaCreacion,
                                        FechaModificacion = pa.FechaModificacion,
                                        FechaBaja = pa.FechaBaja,
                                        FechaReactivacion = pa.FechaReactivacion,
                                        Estado = pa.Estado,
                                        EmpresaId = pa.EmpresaId,
                                        Id = pa.Id,
                                        Nombre = pa.Nombre,
                                        TipoParamId = pa.TipoParamId,
                                        Orden = pa.Orden
                                    }
                                    */
                                },

                            }).ToList();



            var query = (from q in queryila

                         group q by new
                         {
                             q.InventarioUbicacion.Inventario.ColaboradorResponsableId,
                             q.InventarioUbicacion.Inventario.Descripcion,
                             q.InventarioUbicacion.Inventario.Observaciones,
                             q.InventarioUbicacion.Inventario.FechaInventario,
                             q.InventarioUbicacion.Inventario.Id,
                             q.InventarioUbicacion.Inventario.Inventariado,
                             q.InventarioUbicacion.Inventario.NoInventario,
                             q.InventarioUbicacion.Inventario.Estado,
                             q.InventarioUbicacion.Inventario.ColaboradorResponsable.Nombre,
                             q.InventarioUbicacion.Inventario.ColaboradorResponsable.ApellidoPaterno,
                             q.InventarioUbicacion.Inventario.ColaboradorResponsable.ApellidoMaterno,
                             q.InventarioUbicacion.Inventario.ColaboradorResponsable.NumEmpleado
                             //InventarioUbicacionActivoId = q.Id,
                         } into g
                         select new InventoryLocationAssetQuery()
                         {
                             InventarioColaboradorResponsableId = g.Key.ColaboradorResponsableId,
                             InventarioDescripcion = g.Key.Descripcion,
                             InventarioObservaciones = g.Key.Observaciones,
                             InventarioFechaInventario = g.Key.FechaInventario,
                             InventarioId = g.Key.Id,
                             InventarioInventariado = g.Key.Inventariado,
                             InventarioNoInventario = g.Key.NoInventario,
                             InventarioEstado = g.Key.Estado,
                             ColaboradorNombre = g.Key.Nombre,
                             ColaboradorApellidoPaterno = g.Key.ApellidoPaterno,
                             ColaboradorApellidoMaterno = g.Key.ApellidoMaterno,
                             ColaboradorNumEmpleado = g.Key.NumEmpleado,
                             //InventarioUbicacionActivoId = g.Key.InventarioUbicacionActivoId,
                             Ubicacion = g.GroupBy(u => new
                             {
                                 u.InventarioUbicacion.Ubicacion.Id,
                                 u.InventarioUbicacion.Ubicacion.Nombre,
                                 u.InventarioUbicacion.Status
                             }).Select(l => new InventoryLocationAQ()
                             {
                                 UbicacionId = l.Key.Id,
                                 UbicacionNombre = l.Key.Nombre,
                                 InventarioUbicacionStatus = l.Key.Status,
                                 Activo = l.GroupBy(w => new
                                 {
                                     w.Activo
                                 }).Select(t => new Asset()
                                 {
                                     UsuarioCreadorId = t.Key.Activo.UsuarioCreadorId,
                                     UsuarioModificadorId = t.Key.Activo.UsuarioModificadorId,
                                     UsuarioBajaId = t.Key.Activo.UsuarioBajaId,
                                     UsuarioReactivadorId = t.Key.Activo.UsuarioReactivadorId,
                                     FechaCreacion = t.Key.Activo.FechaCreacion,
                                     FechaModificacion = t.Key.Activo.FechaModificacion,
                                     FechaBaja = t.Key.Activo.FechaBaja,
                                     FechaReactivacion = t.Key.Activo.FechaReactivacion,
                                     Estado = t.Key.Activo.Estado,
                                     EmpresaId = t.Key.Activo.EmpresaId,
                                     Id = t.Key.Activo.Id,
                                     UbicacionId = t.Key.Activo.UbicacionId,
                                     GrupoActivoId = t.Key.Activo.GrupoActivoId,
                                     TipoActivoId = t.Key.Activo.TipoActivoId,
                                     Codigo = t.Key.Activo.Codigo,
                                     Serie = t.Key.Activo.Serie,
                                     Marca = t.Key.Activo.Marca,
                                     Modelo = t.Key.Activo.Modelo,
                                     Descripcion = t.Key.Activo.Descripcion,
                                     Nombre = t.Key.Activo.Nombre,
                                     Observaciones = t.Key.Activo.Observaciones,
                                     EstadoFisicoId = t.Key.Activo.EstadoFisicoId,
                                     TagId = t.Key.Activo.TagId,
                                     ColaboradorHabitualId = t.Key.Activo.ColaboradorHabitualId,
                                     ColaboradorResponsableId = t.Key.Activo.ColaboradorResponsableId,
                                     ValorCompra = t.Key.Activo.ValorCompra,
                                     FechaCompra = t.Key.Activo.FechaCompra,
                                     Proveedor = t.Key.Activo.Proveedor,
                                     FechaFinGarantia = t.Key.Activo.FechaFinGarantia,
                                     TieneFoto = t.Key.Activo.TieneFoto,
                                     TieneArchivo = t.Key.Activo.TieneArchivo,
                                     FechaCapitalizacion = t.Key.Activo.FechaCapitalizacion,
                                     FichaResguardo = t.Key.Activo.FichaResguardo,
                                     CampoLibre1 = t.Key.Activo.CampoLibre1,
                                     CampoLibre2 = t.Key.Activo.CampoLibre2,
                                     CampoLibre3 = t.Key.Activo.CampoLibre3,
                                     CampoLibre4 = t.Key.Activo.CampoLibre4,
                                     CampoLibre5 = t.Key.Activo.CampoLibre5,
                                     AreaId = t.Key.Activo.AreaId,
                                     Tag = new Tag()
                                     {
                                         UsuarioCreadorId = t.Key.Activo.Tag.UsuarioCreadorId,
                                         UsuarioModificadorId = t.Key.Activo.Tag.UsuarioModificadorId,
                                         UsuarioBajaId = t.Key.Activo.Tag.UsuarioBajaId,
                                         UsuarioReactivadorId = t.Key.Activo.Tag.UsuarioReactivadorId,
                                         FechaCreacion = t.Key.Activo.Tag.FechaCreacion,
                                         FechaModificacion = t.Key.Activo.Tag.FechaModificacion,
                                         FechaBaja = t.Key.Activo.Tag.FechaBaja,
                                         FechaReactivacion = t.Key.Activo.Tag.FechaReactivacion,
                                         Estado = t.Key.Activo.Tag.Estado,
                                         EmpresaId = t.Key.Activo.Tag.EmpresaId,
                                         Id = t.Key.Activo.Tag.Id,
                                         TipoTagId = t.Key.Activo.Tag.TipoTagId,
                                         Numero = t.Key.Activo.Tag.Numero,
                                         Fc = t.Key.Activo.Tag.Fc,
                                         Vence = t.Key.Activo.Tag.Vence
                                     }
                                 }).ToList()
                             }).ToList()
                         }).ToList();



            var queryil = (from il in inventorylocationdi

                           join i in inventorydi on il.InventarioId equals i.Id
                           into Inventory_InventoryLocation
                           from iil in Inventory_InventoryLocation.DefaultIfEmpty()

                           join l in locationdi on il.UbicacionId equals l.Id
                           into Location_InventoryLocation
                           from lil in Location_InventoryLocation.DefaultIfEmpty()

                           select new InventoryLocation()
                           {
                               Id = il.Id,
                               InventarioId = il.InventarioId,
                               UbicacionId = il.UbicacionId,
                               Status = il.Status,
                               Inventario = new Inventory()
                               {
                                   UsuarioCreadorId = iil.UsuarioCreadorId,
                                   UsuarioModificadorId = iil.UsuarioModificadorId,
                                   UsuarioBajaId = iil.UsuarioBajaId,
                                   UsuarioReactivadorId = iil.UsuarioReactivadorId,
                                   FechaCreacion = iil.FechaCreacion,
                                   FechaModificacion = iil.FechaModificacion,
                                   FechaBaja = iil.FechaBaja,
                                   FechaReactivacion = iil.FechaReactivacion,
                                   Estado = iil.Estado,
                                   EmpresaId = iil.EmpresaId,
                                   Id = iil.Id,
                                   ColaboradorResponsableId = iil.ColaboradorResponsableId,
                                   Descripcion = iil.Descripcion,
                                   Observaciones = iil.Observaciones,
                                   FechaInventario = iil.FechaInventario,
                                   Inventariado = iil.Inventariado,
                                   NoInventario = iil.NoInventario
                               },
                               Ubicacion = new Location()
                               {
                                   Id = lil.Id,
                                   Nombre = lil.Nombre,
                                   Status = lil.Status
                               }
                           }).ToList();


            var querydi = (from i in queryil

                           group i by new
                           {
                               i.Inventario.Id,
                               i.Inventario.ColaboradorResponsableId,
                               i.Inventario.Descripcion,
                               i.Inventario.Observaciones,
                               i.Inventario.FechaInventario,
                               i.Inventario.Inventariado,
                               i.Inventario.NoInventario,
                               i.Inventario.Estado
                           } into g

                           select new InventoryQuerySpecial()
                           {
                               Id = g.Key.Id,
                               ColaboradorResponsableId = g.Key.ColaboradorResponsableId,
                               Descripcion = g.Key.Descripcion,
                               Observaciones = g.Key.Observaciones,
                               FechaInventario = g.Key.FechaInventario,
                               Inventariado = g.Key.Inventariado,
                               NoInventario = g.Key.NoInventario,
                               Estado = g.Key.Estado,
                               InventarioUbicacion = g.GroupBy(sa => new
                               {
                                   sa.Id,
                                   sa.Ubicacion.Nombre
                               }).Select(s => new InventoryLocationQS()
                               {
                                   InventarioUbicacionId = s.Key.Id,
                                   InventarioUbicacionNombre = s.Key.Nombre,
                                   Ubicacion = s.GroupBy(sss => new
                                   {
                                       sss.Id,
                                       sss.Ubicacion
                                   }).Select(aa => new Location()
                                   {
                                       Nombre = aa.Key.Ubicacion.Nombre
                                   }).ToList()
                               }).ToList()
                           }).ToList();

            return await Task.FromResult(query.ToList());
        }

        public async Task<InventoryLocationAssetQuery> GetByInventoryIdAsync(Guid inventoryId)
        {
            var inventorylocationdi = await App.inventoryLocationRepository.GetAllAsync();

            var inventorylocationassetdi = await App.inventoryLocationAssetRepository.GetAllAsync();

            var paramsdi = await App.paramsRepositoty.GetAllAsync();

            var collaboratordi = await App.collaboratorRepository.GetAllAsync();

            var assetdi = await App.assetRepository.GetAllAsync();

            var devicedi = await App.deviceRepository.GetAllAsync();

            var inventorydi = await _database.QueryAsync<Inventory>("SELECT * FROM Inventory");

            var locationdi = await App.locationRepository.GetAllAsync();

            var inventoryDetaildi = await App.inventoryDetailRepository.GetAllAsync();

            var physicalStatedi = await App.physicalStateRepository.GetAllAsync();

            var tagi = await App.tagRepository.GetAllAsync();

            var queryila = (from ila in inventorylocationassetdi

                            join il in inventorylocationdi on ila.InventarioUbicacionId equals il.Id
                            into InventoryLocation_InventoryLocationAsset
                            from ilila in InventoryLocation_InventoryLocationAsset.DefaultIfEmpty()

                            join i in inventorydi on ilila.InventarioId equals i.Id
                            into Inventory_InventoryLocation
                            from iil in Inventory_InventoryLocation.DefaultIfEmpty()

                            join c in collaboratordi on iil.ColaboradorResponsableId equals c.Id
                            into Collaborator_Inventory
                            from ci in Collaborator_Inventory.DefaultIfEmpty()

                            join l in locationdi on ilila.UbicacionId equals l.Id
                            into Location_InventoryLocation
                            from lil in Location_InventoryLocation.DefaultIfEmpty()




                            join a in assetdi on ila.ActivoId equals a.Id
                            into Asset_InventoryLocationAsset
                            from aila in Asset_InventoryLocationAsset.DefaultIfEmpty()

                            join ps in physicalStatedi on aila.EstadoFisicoId equals ps.Id
                            into PhysicalState_Asset
                            from psa in PhysicalState_Asset.DefaultIfEmpty()

                            join t in tagi on aila.TagId equals t.Id
                            into Tag_Asset
                            from ta in Tag_Asset.DefaultIfEmpty()

                                /*
                                join p in paramsdi on aila.MotivoId equals p.Id
                                into Params_Asset
                                from pa in Params_Asset.DefaultIfEmpty()
                                */

                            select new InventoryLocationAsset()
                            {
                                Id = ila.Id,
                                InventarioUbicacionId = ila.InventarioUbicacionId,
                                ActivoId = ila.ActivoId,
                                InventarioUbicacion = new InventoryLocation()
                                {
                                    Id = ilila.Id,
                                    InventarioId = ilila.InventarioId,
                                    UbicacionId = ilila.UbicacionId,
                                    Status = ilila.Status,
                                    Inventario = new Inventory()
                                    {
                                        UsuarioCreadorId = iil.UsuarioCreadorId,
                                        UsuarioModificadorId = iil.UsuarioModificadorId,
                                        UsuarioBajaId = iil.UsuarioBajaId,
                                        UsuarioReactivadorId = iil.UsuarioReactivadorId,
                                        FechaCreacion = iil.FechaCreacion,
                                        FechaModificacion = iil.FechaModificacion,
                                        FechaBaja = iil.FechaBaja,
                                        FechaReactivacion = iil.FechaReactivacion,
                                        Estado = iil.Estado,
                                        EmpresaId = iil.EmpresaId,
                                        Id = iil.Id,
                                        ColaboradorResponsableId = iil.ColaboradorResponsableId,
                                        Descripcion = iil.Descripcion,
                                        Observaciones = iil.Observaciones,
                                        FechaInventario = iil.FechaInventario,
                                        Inventariado = iil.Inventariado,
                                        NoInventario = iil.NoInventario,
                                        ColaboradorResponsable = new Collaborator()
                                        {
                                            Id = ci.Id,
                                            Nombre = ci.Nombre,
                                            ApellidoPaterno = ci.ApellidoPaterno,
                                            ApellidoMaterno = ci.ApellidoMaterno,
                                            EstadoCivilId = ci.EstadoCivilId,
                                            Genero = ci.Genero,
                                            TipoIdentificacionId = ci.TipoIdentificacionId,
                                            Identificacion = ci.Identificacion,
                                            Foto = ci.Foto,
                                            ExtensionFoto = ci.ExtensionFoto,
                                            NumEmpleado = ci.NumEmpleado,
                                            PuestoId = ci.PuestoId,
                                            UbicacionId = ci.UbicacionId,
                                            AreaId = ci.AreaId,
                                            TipoColaboradorId = ci.TipoColaboradorId,
                                            TelefonoMovil = ci.TelefonoMovil,
                                            TelefonoOficina = ci.TelefonoOficina,
                                            Email = ci.Email,
                                            Email_secundario = ci.Email_secundario
                                        }
                                    },
                                    Ubicacion = new Location()
                                    {
                                        Id = lil.Id,
                                        Nombre = lil.Nombre,
                                        Status = lil.Status
                                    }
                                },
                                Activo = new Asset()
                                {
                                    UsuarioCreadorId = aila.UsuarioCreadorId,
                                    UsuarioModificadorId = aila.UsuarioModificadorId,
                                    UsuarioBajaId = aila.UsuarioBajaId,
                                    UsuarioReactivadorId = aila.UsuarioReactivadorId,
                                    FechaCreacion = aila.FechaCreacion,
                                    FechaModificacion = aila.FechaModificacion,
                                    FechaBaja = aila.FechaBaja,
                                    FechaReactivacion = aila.FechaReactivacion,
                                    Estado = aila.Estado,
                                    EmpresaId = aila.EmpresaId,
                                    Id = aila.Id,
                                    UbicacionId = aila.UbicacionId,
                                    GrupoActivoId = aila.GrupoActivoId,
                                    TipoActivoId = aila.TipoActivoId,
                                    Codigo = aila.Codigo,
                                    Serie = aila.Serie,
                                    Marca = aila.Marca,
                                    Modelo = aila.Modelo,
                                    Descripcion = aila.Descripcion,
                                    Nombre = aila.Nombre,
                                    Observaciones = aila.Observaciones,
                                    EstadoFisicoId = aila.EstadoFisicoId,
                                    TagId = aila.TagId,
                                    ColaboradorHabitualId = aila.ColaboradorHabitualId,
                                    ColaboradorResponsableId = aila.ColaboradorResponsableId,
                                    ValorCompra = aila.ValorCompra,
                                    FechaCompra = aila.FechaCompra,
                                    Proveedor = aila.Proveedor,
                                    FechaFinGarantia = aila.FechaFinGarantia,
                                    TieneFoto = aila.TieneFoto,
                                    TieneArchivo = aila.TieneArchivo,
                                    FechaCapitalizacion = aila.FechaCapitalizacion,
                                    FichaResguardo = aila.FichaResguardo,
                                    CampoLibre1 = aila.CampoLibre1,
                                    CampoLibre2 = aila.CampoLibre2,
                                    CampoLibre3 = aila.CampoLibre3,
                                    CampoLibre4 = aila.CampoLibre4,
                                    CampoLibre5 = aila.CampoLibre5,
                                    AreaId = aila.AreaId,
                                    Status = aila.Status,
                                    MotivoId = aila.MotivoId,
                                    EstadoFisico = new PhysicalState()
                                    {
                                        UsuarioCreadorId = psa.UsuarioCreadorId,
                                        UsuarioModificadorId = psa.UsuarioModificadorId,
                                        UsuarioBajaId = psa.UsuarioBajaId,
                                        UsuarioReactivadorId = psa.UsuarioReactivadorId,
                                        FechaCreacion = psa.FechaCreacion,
                                        FechaModificacion = psa.FechaModificacion,
                                        FechaBaja = psa.FechaBaja,
                                        FechaReactivacion = psa.FechaReactivacion,
                                        Estado = psa.Estado,
                                        EmpresaId = psa.EmpresaId,
                                        Id = psa.Id,
                                        Nombre = psa.Nombre,
                                        Descripcion = psa.Descripcion
                                    },
                                    Tag = new Tag()
                                    {
                                        UsuarioCreadorId = ta.UsuarioCreadorId,
                                        UsuarioModificadorId = ta.UsuarioModificadorId,
                                        UsuarioBajaId = ta.UsuarioBajaId,
                                        UsuarioReactivadorId = ta.UsuarioReactivadorId,
                                        FechaCreacion = ta.FechaCreacion,
                                        FechaModificacion = ta.FechaModificacion,
                                        FechaBaja = ta.FechaBaja,
                                        FechaReactivacion = ta.FechaReactivacion,
                                        Estado = ta.Estado,
                                        EmpresaId = ta.EmpresaId,
                                        Id = ta.Id,
                                        TipoTagId = ta.TipoTagId,
                                        Numero = ta.Numero,
                                        Fc = ta.Fc,
                                        Vence = ta.Vence
                                    }
                                    /*
                                    Motivo = new Models.Setting.Params()
                                    {
                                        UsuarioCreadorId = pa.UsuarioCreadorId,
                                        UsuarioModificadorId = pa.UsuarioModificadorId,
                                        UsuarioBajaId = pa.UsuarioBajaId,
                                        UsuarioReactivadorId = pa.UsuarioReactivadorId,
                                        FechaCreacion = pa.FechaCreacion,
                                        FechaModificacion = pa.FechaModificacion,
                                        FechaBaja = pa.FechaBaja,
                                        FechaReactivacion = pa.FechaReactivacion,
                                        Estado = pa.Estado,
                                        EmpresaId = pa.EmpresaId,
                                        Id = pa.Id,
                                        Nombre = pa.Nombre,
                                        TipoParamId = pa.TipoParamId,
                                        Orden = pa.Orden
                                    }
                                    */
                                },

                            }).ToList();


            var query = (from q in queryila
                         where q.InventarioUbicacion.Inventario.Id == inventoryId
                         group q by new
                         {
                             q.InventarioUbicacion.Inventario.ColaboradorResponsableId,
                             q.InventarioUbicacion.Inventario.Descripcion,
                             q.InventarioUbicacion.Inventario.Observaciones,
                             q.InventarioUbicacion.Inventario.FechaInventario,
                             q.InventarioUbicacion.Inventario.Id,
                             q.InventarioUbicacion.Inventario.Inventariado,
                             q.InventarioUbicacion.Inventario.NoInventario,
                             q.InventarioUbicacion.Inventario.Estado,
                             q.InventarioUbicacion.Inventario.ColaboradorResponsable.Nombre,
                             q.InventarioUbicacion.Inventario.ColaboradorResponsable.ApellidoPaterno,
                             q.InventarioUbicacion.Inventario.ColaboradorResponsable.ApellidoMaterno,
                             q.InventarioUbicacion.Inventario.ColaboradorResponsable.NumEmpleado,
                             //InventarioUbicacionId = q.InventarioUbicacion.Id
                             //InventarioUbicacionActivoId = q.Id,
                         } into g
                         select new InventoryLocationAssetQuery()
                         {
                             InventarioColaboradorResponsableId = g.Key.ColaboradorResponsableId,
                             InventarioDescripcion = g.Key.Descripcion,
                             InventarioObservaciones = g.Key.Observaciones,
                             InventarioFechaInventario = g.Key.FechaInventario,
                             InventarioId = g.Key.Id,
                             InventarioInventariado = g.Key.Inventariado,
                             InventarioNoInventario = g.Key.NoInventario,
                             InventarioEstado = g.Key.Estado,
                             ColaboradorNombre = g.Key.Nombre,
                             ColaboradorApellidoPaterno = g.Key.ApellidoPaterno,
                             ColaboradorApellidoMaterno = g.Key.ApellidoMaterno,
                             ColaboradorNumEmpleado = g.Key.NumEmpleado,
                             //InventarioUbicacionActivoId = g.Key.InventarioUbicacionActivoId,
                             Ubicacion = g.GroupBy(u => new
                             {
                                 u.InventarioUbicacion.Ubicacion.Id,
                                 u.InventarioUbicacion.Ubicacion.Nombre,
                                 u.InventarioUbicacion.Status,
                                 InventarioId = u.InventarioUbicacion.Inventario.Id
                                 //InventarioUbicacionId = u.InventarioUbicacion.Id,

                             }).Select(l => new InventoryLocationAQ()
                             {
                                 UbicacionId = l.Key.Id,
                                 UbicacionNombre = l.Key.Nombre,
                                 InventarioUbicacionStatus = l.Key.Status,
                                 InventarioId = l.Key.InventarioId,
                                 AssetTotal = l.GroupBy(w => new
                                 {
                                     w.Activo
                                 }).Select(t => new Asset()
                                 {
                                     UsuarioCreadorId = t.Key.Activo.UsuarioCreadorId,
                                     UsuarioModificadorId = t.Key.Activo.UsuarioModificadorId,
                                     UsuarioBajaId = t.Key.Activo.UsuarioBajaId,
                                     UsuarioReactivadorId = t.Key.Activo.UsuarioReactivadorId,
                                     FechaCreacion = t.Key.Activo.FechaCreacion,
                                     FechaModificacion = t.Key.Activo.FechaModificacion,
                                     FechaBaja = t.Key.Activo.FechaBaja,
                                     FechaReactivacion = t.Key.Activo.FechaReactivacion,
                                     Estado = t.Key.Activo.Estado,
                                     EmpresaId = t.Key.Activo.EmpresaId,
                                     Id = t.Key.Activo.Id,
                                     UbicacionId = t.Key.Activo.UbicacionId,
                                     GrupoActivoId = t.Key.Activo.GrupoActivoId,
                                     TipoActivoId = t.Key.Activo.TipoActivoId,
                                     Codigo = t.Key.Activo.Codigo,
                                     Serie = t.Key.Activo.Serie,
                                     Marca = t.Key.Activo.Marca,
                                     Modelo = t.Key.Activo.Modelo,
                                     Descripcion = t.Key.Activo.Descripcion,
                                     Nombre = t.Key.Activo.Nombre,
                                     Observaciones = t.Key.Activo.Observaciones,
                                     EstadoFisicoId = t.Key.Activo.EstadoFisicoId,
                                     TagId = t.Key.Activo.TagId,
                                     ColaboradorHabitualId = t.Key.Activo.ColaboradorHabitualId,
                                     ColaboradorResponsableId = t.Key.Activo.ColaboradorResponsableId,
                                     ValorCompra = t.Key.Activo.ValorCompra,
                                     FechaCompra = t.Key.Activo.FechaCompra,
                                     Proveedor = t.Key.Activo.Proveedor,
                                     FechaFinGarantia = t.Key.Activo.FechaFinGarantia,
                                     TieneFoto = t.Key.Activo.TieneFoto,
                                     TieneArchivo = t.Key.Activo.TieneArchivo,
                                     FechaCapitalizacion = t.Key.Activo.FechaCapitalizacion,
                                     FichaResguardo = t.Key.Activo.FichaResguardo,
                                     CampoLibre1 = t.Key.Activo.CampoLibre1,
                                     CampoLibre2 = t.Key.Activo.CampoLibre2,
                                     CampoLibre3 = t.Key.Activo.CampoLibre3,
                                     CampoLibre4 = t.Key.Activo.CampoLibre4,
                                     CampoLibre5 = t.Key.Activo.CampoLibre5,
                                     AreaId = t.Key.Activo.AreaId,
                                     Status = t.Key.Activo.Status,
                                     MotivoId = t.Key.Activo.MotivoId,
                                     EstadoFisico = new PhysicalState()
                                     {
                                         Id = t.Key.Activo.EstadoFisico.Id,
                                         Nombre = t.Key.Activo.EstadoFisico.Nombre,
                                         Descripcion = t.Key.Activo.EstadoFisico.Descripcion
                                     },
                                     Tag = new Tag()
                                     {
                                         UsuarioCreadorId = t.Key.Activo.Tag.UsuarioCreadorId,
                                         UsuarioModificadorId = t.Key.Activo.Tag.UsuarioModificadorId,
                                         UsuarioBajaId = t.Key.Activo.Tag.UsuarioBajaId,
                                         UsuarioReactivadorId = t.Key.Activo.Tag.UsuarioReactivadorId,
                                         FechaCreacion = t.Key.Activo.Tag.FechaCreacion,
                                         FechaModificacion = t.Key.Activo.Tag.FechaModificacion,
                                         FechaBaja = t.Key.Activo.Tag.FechaBaja,
                                         FechaReactivacion = t.Key.Activo.Tag.FechaReactivacion,
                                         Estado = t.Key.Activo.Tag.Estado,
                                         EmpresaId = t.Key.Activo.Tag.EmpresaId,
                                         Id = t.Key.Activo.Tag.Id,
                                         TipoTagId = t.Key.Activo.Tag.TipoTagId,
                                         Numero = t.Key.Activo.Tag.Numero,
                                         Fc = t.Key.Activo.Tag.Fc,
                                         Vence = t.Key.Activo.Tag.Vence
                                     }
                                 }).ToList().Count(),
                                 Activo = l.GroupBy(w => new
                                 {
                                     w.Activo
                                 }).Select(t => new Asset()
                                 {
                                     UsuarioCreadorId = t.Key.Activo.UsuarioCreadorId,
                                     UsuarioModificadorId = t.Key.Activo.UsuarioModificadorId,
                                     UsuarioBajaId = t.Key.Activo.UsuarioBajaId,
                                     UsuarioReactivadorId = t.Key.Activo.UsuarioReactivadorId,
                                     FechaCreacion = t.Key.Activo.FechaCreacion,
                                     FechaModificacion = t.Key.Activo.FechaModificacion,
                                     FechaBaja = t.Key.Activo.FechaBaja,
                                     FechaReactivacion = t.Key.Activo.FechaReactivacion,
                                     Estado = t.Key.Activo.Estado,
                                     EmpresaId = t.Key.Activo.EmpresaId,
                                     Id = t.Key.Activo.Id,
                                     UbicacionId = t.Key.Activo.UbicacionId,
                                     GrupoActivoId = t.Key.Activo.GrupoActivoId,
                                     TipoActivoId = t.Key.Activo.TipoActivoId,
                                     Codigo = t.Key.Activo.Codigo,
                                     Serie = t.Key.Activo.Serie,
                                     Marca = t.Key.Activo.Marca,
                                     Modelo = t.Key.Activo.Modelo,
                                     Descripcion = t.Key.Activo.Descripcion,
                                     Nombre = t.Key.Activo.Nombre,
                                     Observaciones = t.Key.Activo.Observaciones,
                                     EstadoFisicoId = t.Key.Activo.EstadoFisicoId,
                                     TagId = t.Key.Activo.TagId,
                                     ColaboradorHabitualId = t.Key.Activo.ColaboradorHabitualId,
                                     ColaboradorResponsableId = t.Key.Activo.ColaboradorResponsableId,
                                     ValorCompra = t.Key.Activo.ValorCompra,
                                     FechaCompra = t.Key.Activo.FechaCompra,
                                     Proveedor = t.Key.Activo.Proveedor,
                                     FechaFinGarantia = t.Key.Activo.FechaFinGarantia,
                                     TieneFoto = t.Key.Activo.TieneFoto,
                                     TieneArchivo = t.Key.Activo.TieneArchivo,
                                     FechaCapitalizacion = t.Key.Activo.FechaCapitalizacion,
                                     FichaResguardo = t.Key.Activo.FichaResguardo,
                                     CampoLibre1 = t.Key.Activo.CampoLibre1,
                                     CampoLibre2 = t.Key.Activo.CampoLibre2,
                                     CampoLibre3 = t.Key.Activo.CampoLibre3,
                                     CampoLibre4 = t.Key.Activo.CampoLibre4,
                                     CampoLibre5 = t.Key.Activo.CampoLibre5,
                                     AreaId = t.Key.Activo.AreaId,
                                     Status = t.Key.Activo.Status,
                                     MotivoId = t.Key.Activo.MotivoId,
                                     EstadoFisico = new PhysicalState()
                                     {
                                         Id = t.Key.Activo.EstadoFisico.Id,
                                         Nombre = t.Key.Activo.EstadoFisico.Nombre,
                                         Descripcion = t.Key.Activo.EstadoFisico.Descripcion
                                     },
                                     Tag = new Tag()
                                     {
                                         UsuarioCreadorId = t.Key.Activo.Tag.UsuarioCreadorId,
                                         UsuarioModificadorId = t.Key.Activo.Tag.UsuarioModificadorId,
                                         UsuarioBajaId = t.Key.Activo.Tag.UsuarioBajaId,
                                         UsuarioReactivadorId = t.Key.Activo.Tag.UsuarioReactivadorId,
                                         FechaCreacion = t.Key.Activo.Tag.FechaCreacion,
                                         FechaModificacion = t.Key.Activo.Tag.FechaModificacion,
                                         FechaBaja = t.Key.Activo.Tag.FechaBaja,
                                         FechaReactivacion = t.Key.Activo.Tag.FechaReactivacion,
                                         Estado = t.Key.Activo.Tag.Estado,
                                         EmpresaId = t.Key.Activo.Tag.EmpresaId,
                                         Id = t.Key.Activo.Tag.Id,
                                         TipoTagId = t.Key.Activo.Tag.TipoTagId,
                                         Numero = t.Key.Activo.Tag.Numero,
                                         Fc = t.Key.Activo.Tag.Fc,
                                         Vence = t.Key.Activo.Tag.Vence
                                     }
                                 }).ToList()
                             }).ToList()
                         }).ToList();

            return await Task.FromResult(query.FirstOrDefault(l => l.InventarioId == inventoryId));
        }

        public async Task<InventoryLocationAQ> GetByLocationIdAsync(Guid locationId, Guid inventoryId)
        {
            var inventorylocationdi = await App.inventoryLocationRepository.GetAllAsync();

            var inventorylocationassetdi = await App.inventoryLocationAssetRepository.GetAllAsync();

            var paramsdi = await App.paramsRepositoty.GetAllAsync();

            var collaboratordi = await App.collaboratorRepository.GetAllAsync();

            var assetdi = await App.assetRepository.GetAllAsync();

            var devicedi = await App.deviceRepository.GetAllAsync();

            var inventorydi = await _database.QueryAsync<Inventory>("SELECT * FROM Inventory");

            var locationdi = await App.locationRepository.GetAllAsync();

            var inventoryDetaildi = await App.inventoryDetailRepository.GetAllAsync();

            var physicalStatedi = await App.physicalStateRepository.GetAllAsync();

            var tagi = await App.tagRepository.GetAllAsync();

            var queryila = (from ila in inventorylocationassetdi

                            join il in inventorylocationdi on ila.InventarioUbicacionId equals il.Id
                            into InventoryLocation_InventoryLocationAsset
                            from ilila in InventoryLocation_InventoryLocationAsset.DefaultIfEmpty()

                            join i in inventorydi on ilila.InventarioId equals i.Id
                            into Inventory_InventoryLocation
                            from iil in Inventory_InventoryLocation.DefaultIfEmpty()

                            join c in collaboratordi on iil.ColaboradorResponsableId equals c.Id
                            into Collaborator_Inventory
                            from ci in Collaborator_Inventory.DefaultIfEmpty()

                            join l in locationdi on ilila.UbicacionId equals l.Id
                            into Location_InventoryLocation
                            from lil in Location_InventoryLocation.DefaultIfEmpty()




                            join a in assetdi on ila.ActivoId equals a.Id
                            into Asset_InventoryLocationAsset
                            from aila in Asset_InventoryLocationAsset.DefaultIfEmpty()

                            join ps in physicalStatedi on aila.EstadoFisicoId equals ps.Id
                            into PhysicalState_Asset
                            from psa in PhysicalState_Asset.DefaultIfEmpty()

                            join t in tagi on aila.TagId equals t.Id
                            into Tag_Asset
                            from ta in Tag_Asset.DefaultIfEmpty()

                                /*
                                join p in paramsdi on aila.MotivoId equals p.Id
                                into Params_Asset
                                from pa in Params_Asset.DefaultIfEmpty()
                                */

                            select new InventoryLocationAsset()
                            {
                                Id = ila.Id,
                                InventarioUbicacionId = ila.InventarioUbicacionId,
                                ActivoId = ila.ActivoId,
                                InventarioUbicacion = new InventoryLocation()
                                {
                                    Id = ilila.Id,
                                    InventarioId = ilila.InventarioId,
                                    UbicacionId = ilila.UbicacionId,
                                    Status = ilila.Status,
                                    Inventario = new Inventory()
                                    {
                                        UsuarioCreadorId = iil.UsuarioCreadorId,
                                        UsuarioModificadorId = iil.UsuarioModificadorId,
                                        UsuarioBajaId = iil.UsuarioBajaId,
                                        UsuarioReactivadorId = iil.UsuarioReactivadorId,
                                        FechaCreacion = iil.FechaCreacion,
                                        FechaModificacion = iil.FechaModificacion,
                                        FechaBaja = iil.FechaBaja,
                                        FechaReactivacion = iil.FechaReactivacion,
                                        Estado = iil.Estado,
                                        EmpresaId = iil.EmpresaId,
                                        Id = iil.Id,
                                        ColaboradorResponsableId = iil.ColaboradorResponsableId,
                                        Descripcion = iil.Descripcion,
                                        Observaciones = iil.Observaciones,
                                        FechaInventario = iil.FechaInventario,
                                        Inventariado = iil.Inventariado,
                                        NoInventario = iil.NoInventario,
                                        ColaboradorResponsable = new Collaborator()
                                        {
                                            Id = ci.Id,
                                            Nombre = ci.Nombre,
                                            ApellidoPaterno = ci.ApellidoPaterno,
                                            ApellidoMaterno = ci.ApellidoMaterno,
                                            EstadoCivilId = ci.EstadoCivilId,
                                            Genero = ci.Genero,
                                            TipoIdentificacionId = ci.TipoIdentificacionId,
                                            Identificacion = ci.Identificacion,
                                            Foto = ci.Foto,
                                            ExtensionFoto = ci.ExtensionFoto,
                                            NumEmpleado = ci.NumEmpleado,
                                            PuestoId = ci.PuestoId,
                                            UbicacionId = ci.UbicacionId,
                                            AreaId = ci.AreaId,
                                            TipoColaboradorId = ci.TipoColaboradorId,
                                            TelefonoMovil = ci.TelefonoMovil,
                                            TelefonoOficina = ci.TelefonoOficina,
                                            Email = ci.Email,
                                            Email_secundario = ci.Email_secundario
                                        }
                                    },
                                    Ubicacion = new Location()
                                    {
                                        Id = lil.Id,
                                        Nombre = lil.Nombre,
                                        Status = lil.Status
                                    }
                                },
                                Activo = new Asset()
                                {
                                    UsuarioCreadorId = aila.UsuarioCreadorId,
                                    UsuarioModificadorId = aila.UsuarioModificadorId,
                                    UsuarioBajaId = aila.UsuarioBajaId,
                                    UsuarioReactivadorId = aila.UsuarioReactivadorId,
                                    FechaCreacion = aila.FechaCreacion,
                                    FechaModificacion = aila.FechaModificacion,
                                    FechaBaja = aila.FechaBaja,
                                    FechaReactivacion = aila.FechaReactivacion,
                                    Estado = aila.Estado,
                                    EmpresaId = aila.EmpresaId,
                                    Id = aila.Id,
                                    UbicacionId = aila.UbicacionId,
                                    GrupoActivoId = aila.GrupoActivoId,
                                    TipoActivoId = aila.TipoActivoId,
                                    Codigo = aila.Codigo,
                                    Serie = aila.Serie,
                                    Marca = aila.Marca,
                                    Modelo = aila.Modelo,
                                    Descripcion = aila.Descripcion,
                                    Nombre = aila.Nombre,
                                    Observaciones = aila.Observaciones,
                                    EstadoFisicoId = aila.EstadoFisicoId,
                                    TagId = aila.TagId,
                                    ColaboradorHabitualId = aila.ColaboradorHabitualId,
                                    ColaboradorResponsableId = aila.ColaboradorResponsableId,
                                    ValorCompra = aila.ValorCompra,
                                    FechaCompra = aila.FechaCompra,
                                    Proveedor = aila.Proveedor,
                                    FechaFinGarantia = aila.FechaFinGarantia,
                                    TieneFoto = aila.TieneFoto,
                                    TieneArchivo = aila.TieneArchivo,
                                    FechaCapitalizacion = aila.FechaCapitalizacion,
                                    FichaResguardo = aila.FichaResguardo,
                                    CampoLibre1 = aila.CampoLibre1,
                                    CampoLibre2 = aila.CampoLibre2,
                                    CampoLibre3 = aila.CampoLibre3,
                                    CampoLibre4 = aila.CampoLibre4,
                                    CampoLibre5 = aila.CampoLibre5,
                                    AreaId = aila.AreaId,
                                    Status = aila.Status,
                                    MotivoId = aila.MotivoId,
                                    EstadoFisico = new PhysicalState()
                                    {
                                        UsuarioCreadorId = psa.UsuarioCreadorId,
                                        UsuarioModificadorId = psa.UsuarioModificadorId,
                                        UsuarioBajaId = psa.UsuarioBajaId,
                                        UsuarioReactivadorId = psa.UsuarioReactivadorId,
                                        FechaCreacion = psa.FechaCreacion,
                                        FechaModificacion = psa.FechaModificacion,
                                        FechaBaja = psa.FechaBaja,
                                        FechaReactivacion = psa.FechaReactivacion,
                                        Estado = psa.Estado,
                                        EmpresaId = psa.EmpresaId,
                                        Id = psa.Id,
                                        Nombre = psa.Nombre,
                                        Descripcion = psa.Descripcion
                                    },
                                    Tag = new Tag()
                                    {
                                        UsuarioCreadorId = ta.UsuarioCreadorId,
                                        UsuarioModificadorId = ta.UsuarioModificadorId,
                                        UsuarioBajaId = ta.UsuarioBajaId,
                                        UsuarioReactivadorId = ta.UsuarioReactivadorId,
                                        FechaCreacion = ta.FechaCreacion,
                                        FechaModificacion = ta.FechaModificacion,
                                        FechaBaja = ta.FechaBaja,
                                        FechaReactivacion = ta.FechaReactivacion,
                                        Estado = ta.Estado,
                                        EmpresaId = ta.EmpresaId,
                                        Id = ta.Id,
                                        TipoTagId = ta.TipoTagId,
                                        Numero = ta.Numero,
                                        Fc = ta.Fc,
                                        Vence = ta.Vence
                                    }
                                    /*
                                    Motivo = new Models.Setting.Params()
                                    {
                                        UsuarioCreadorId = pa.UsuarioCreadorId,
                                        UsuarioModificadorId = pa.UsuarioModificadorId,
                                        UsuarioBajaId = pa.UsuarioBajaId,
                                        UsuarioReactivadorId = pa.UsuarioReactivadorId,
                                        FechaCreacion = pa.FechaCreacion,
                                        FechaModificacion = pa.FechaModificacion,
                                        FechaBaja = pa.FechaBaja,
                                        FechaReactivacion = pa.FechaReactivacion,
                                        Estado = pa.Estado,
                                        EmpresaId = pa.EmpresaId,
                                        Id = pa.Id,
                                        Nombre = pa.Nombre,
                                        TipoParamId = pa.TipoParamId,
                                        Orden = pa.Orden
                                    }
                                    */
                                },

                            }).ToList();


            var queryq = (from q in queryila
                          where q.InventarioUbicacion.Inventario.Id == inventoryId
                          group q by new
                          {
                              q.InventarioUbicacion.Inventario.ColaboradorResponsableId,
                              q.InventarioUbicacion.Inventario.Descripcion,
                              q.InventarioUbicacion.Inventario.Observaciones,
                              q.InventarioUbicacion.Inventario.FechaInventario,
                              q.InventarioUbicacion.Inventario.Id,
                              q.InventarioUbicacion.Inventario.Inventariado,
                              q.InventarioUbicacion.Inventario.NoInventario,
                              q.InventarioUbicacion.Inventario.Estado,
                              q.InventarioUbicacion.Inventario.ColaboradorResponsable.Nombre,
                              q.InventarioUbicacion.Inventario.ColaboradorResponsable.ApellidoPaterno,
                              q.InventarioUbicacion.Inventario.ColaboradorResponsable.ApellidoMaterno,
                              q.InventarioUbicacion.Inventario.ColaboradorResponsable.NumEmpleado,
                              //InventarioUbicacionId = q.InventarioUbicacion.Id
                              //InventarioUbicacionActivoId = q.Id,
                          } into g
                          select new InventoryLocationAssetQuery()
                          {
                              InventarioColaboradorResponsableId = g.Key.ColaboradorResponsableId,
                              InventarioDescripcion = g.Key.Descripcion,
                              InventarioObservaciones = g.Key.Observaciones,
                              InventarioFechaInventario = g.Key.FechaInventario,
                              InventarioId = g.Key.Id,
                              InventarioInventariado = g.Key.Inventariado,
                              InventarioNoInventario = g.Key.NoInventario,
                              InventarioEstado = g.Key.Estado,
                              ColaboradorNombre = g.Key.Nombre,
                              ColaboradorApellidoPaterno = g.Key.ApellidoPaterno,
                              ColaboradorApellidoMaterno = g.Key.ApellidoMaterno,
                              ColaboradorNumEmpleado = g.Key.NumEmpleado,
                              //InventarioUbicacionActivoId = g.Key.InventarioUbicacionActivoId,
                              Ubicacion = g.GroupBy(u => new
                              {
                                  u.InventarioUbicacion.Ubicacion.Id,
                                  u.InventarioUbicacion.Ubicacion.Nombre,
                                  u.InventarioUbicacion.Status,
                                  InventarioId = u.InventarioUbicacion.Inventario.Id
                                  //InventarioUbicacionId = u.InventarioUbicacion.Id,

                              }).Select(l => new InventoryLocationAQ()
                              {
                                  UbicacionId = l.Key.Id,
                                  UbicacionNombre = l.Key.Nombre,
                                  InventarioUbicacionStatus = l.Key.Status,
                                  InventarioId = l.Key.InventarioId,
                                  AssetTotal = l.GroupBy(w => new
                                  {
                                      w.Activo
                                  }).Select(t => new Asset()
                                  {
                                      UsuarioCreadorId = t.Key.Activo.UsuarioCreadorId,
                                      UsuarioModificadorId = t.Key.Activo.UsuarioModificadorId,
                                      UsuarioBajaId = t.Key.Activo.UsuarioBajaId,
                                      UsuarioReactivadorId = t.Key.Activo.UsuarioReactivadorId,
                                      FechaCreacion = t.Key.Activo.FechaCreacion,
                                      FechaModificacion = t.Key.Activo.FechaModificacion,
                                      FechaBaja = t.Key.Activo.FechaBaja,
                                      FechaReactivacion = t.Key.Activo.FechaReactivacion,
                                      Estado = t.Key.Activo.Estado,
                                      EmpresaId = t.Key.Activo.EmpresaId,
                                      Id = t.Key.Activo.Id,
                                      UbicacionId = t.Key.Activo.UbicacionId,
                                      GrupoActivoId = t.Key.Activo.GrupoActivoId,
                                      TipoActivoId = t.Key.Activo.TipoActivoId,
                                      Codigo = t.Key.Activo.Codigo,
                                      Serie = t.Key.Activo.Serie,
                                      Marca = t.Key.Activo.Marca,
                                      Modelo = t.Key.Activo.Modelo,
                                      Descripcion = t.Key.Activo.Descripcion,
                                      Nombre = t.Key.Activo.Nombre,
                                      Observaciones = t.Key.Activo.Observaciones,
                                      EstadoFisicoId = t.Key.Activo.EstadoFisicoId,
                                      TagId = t.Key.Activo.TagId,
                                      ColaboradorHabitualId = t.Key.Activo.ColaboradorHabitualId,
                                      ColaboradorResponsableId = t.Key.Activo.ColaboradorResponsableId,
                                      ValorCompra = t.Key.Activo.ValorCompra,
                                      FechaCompra = t.Key.Activo.FechaCompra,
                                      Proveedor = t.Key.Activo.Proveedor,
                                      FechaFinGarantia = t.Key.Activo.FechaFinGarantia,
                                      TieneFoto = t.Key.Activo.TieneFoto,
                                      TieneArchivo = t.Key.Activo.TieneArchivo,
                                      FechaCapitalizacion = t.Key.Activo.FechaCapitalizacion,
                                      FichaResguardo = t.Key.Activo.FichaResguardo,
                                      CampoLibre1 = t.Key.Activo.CampoLibre1,
                                      CampoLibre2 = t.Key.Activo.CampoLibre2,
                                      CampoLibre3 = t.Key.Activo.CampoLibre3,
                                      CampoLibre4 = t.Key.Activo.CampoLibre4,
                                      CampoLibre5 = t.Key.Activo.CampoLibre5,
                                      AreaId = t.Key.Activo.AreaId,
                                      Status = t.Key.Activo.Status,
                                      MotivoId = t.Key.Activo.MotivoId,
                                      EstadoFisico = new PhysicalState()
                                      {
                                          Id = t.Key.Activo.EstadoFisico.Id,
                                          Nombre = t.Key.Activo.EstadoFisico.Nombre,
                                          Descripcion = t.Key.Activo.EstadoFisico.Descripcion
                                      },
                                      Tag = new Tag()
                                      {
                                          UsuarioCreadorId = t.Key.Activo.Tag.UsuarioCreadorId,
                                          UsuarioModificadorId = t.Key.Activo.Tag.UsuarioModificadorId,
                                          UsuarioBajaId = t.Key.Activo.Tag.UsuarioBajaId,
                                          UsuarioReactivadorId = t.Key.Activo.Tag.UsuarioReactivadorId,
                                          FechaCreacion = t.Key.Activo.Tag.FechaCreacion,
                                          FechaModificacion = t.Key.Activo.Tag.FechaModificacion,
                                          FechaBaja = t.Key.Activo.Tag.FechaBaja,
                                          FechaReactivacion = t.Key.Activo.Tag.FechaReactivacion,
                                          Estado = t.Key.Activo.Tag.Estado,
                                          EmpresaId = t.Key.Activo.Tag.EmpresaId,
                                          Id = t.Key.Activo.Tag.Id,
                                          TipoTagId = t.Key.Activo.Tag.TipoTagId,
                                          Numero = t.Key.Activo.Tag.Numero,
                                          Fc = t.Key.Activo.Tag.Fc,
                                          Vence = t.Key.Activo.Tag.Vence
                                      }
                                  }).ToList().Count(),
                                  Activo = l.GroupBy(w => new
                                  {
                                      w.Activo
                                  }).Select(t => new Asset()
                                  {
                                      UsuarioCreadorId = t.Key.Activo.UsuarioCreadorId,
                                      UsuarioModificadorId = t.Key.Activo.UsuarioModificadorId,
                                      UsuarioBajaId = t.Key.Activo.UsuarioBajaId,
                                      UsuarioReactivadorId = t.Key.Activo.UsuarioReactivadorId,
                                      FechaCreacion = t.Key.Activo.FechaCreacion,
                                      FechaModificacion = t.Key.Activo.FechaModificacion,
                                      FechaBaja = t.Key.Activo.FechaBaja,
                                      FechaReactivacion = t.Key.Activo.FechaReactivacion,
                                      Estado = t.Key.Activo.Estado,
                                      EmpresaId = t.Key.Activo.EmpresaId,
                                      Id = t.Key.Activo.Id,
                                      UbicacionId = t.Key.Activo.UbicacionId,
                                      GrupoActivoId = t.Key.Activo.GrupoActivoId,
                                      TipoActivoId = t.Key.Activo.TipoActivoId,
                                      Codigo = t.Key.Activo.Codigo,
                                      Serie = t.Key.Activo.Serie,
                                      Marca = t.Key.Activo.Marca,
                                      Modelo = t.Key.Activo.Modelo,
                                      Descripcion = t.Key.Activo.Descripcion,
                                      Nombre = t.Key.Activo.Nombre,
                                      Observaciones = t.Key.Activo.Observaciones,
                                      EstadoFisicoId = t.Key.Activo.EstadoFisicoId,
                                      TagId = t.Key.Activo.TagId,
                                      ColaboradorHabitualId = t.Key.Activo.ColaboradorHabitualId,
                                      ColaboradorResponsableId = t.Key.Activo.ColaboradorResponsableId,
                                      ValorCompra = t.Key.Activo.ValorCompra,
                                      FechaCompra = t.Key.Activo.FechaCompra,
                                      Proveedor = t.Key.Activo.Proveedor,
                                      FechaFinGarantia = t.Key.Activo.FechaFinGarantia,
                                      TieneFoto = t.Key.Activo.TieneFoto,
                                      TieneArchivo = t.Key.Activo.TieneArchivo,
                                      FechaCapitalizacion = t.Key.Activo.FechaCapitalizacion,
                                      FichaResguardo = t.Key.Activo.FichaResguardo,
                                      CampoLibre1 = t.Key.Activo.CampoLibre1,
                                      CampoLibre2 = t.Key.Activo.CampoLibre2,
                                      CampoLibre3 = t.Key.Activo.CampoLibre3,
                                      CampoLibre4 = t.Key.Activo.CampoLibre4,
                                      CampoLibre5 = t.Key.Activo.CampoLibre5,
                                      AreaId = t.Key.Activo.AreaId,
                                      Status = t.Key.Activo.Status,
                                      MotivoId = t.Key.Activo.MotivoId,
                                      EstadoFisico = new PhysicalState()
                                      {
                                          Id = t.Key.Activo.EstadoFisico.Id,
                                          Nombre = t.Key.Activo.EstadoFisico.Nombre,
                                          Descripcion = t.Key.Activo.EstadoFisico.Descripcion
                                      },
                                      Tag = new Tag()
                                      {
                                          UsuarioCreadorId = t.Key.Activo.Tag.UsuarioCreadorId,
                                          UsuarioModificadorId = t.Key.Activo.Tag.UsuarioModificadorId,
                                          UsuarioBajaId = t.Key.Activo.Tag.UsuarioBajaId,
                                          UsuarioReactivadorId = t.Key.Activo.Tag.UsuarioReactivadorId,
                                          FechaCreacion = t.Key.Activo.Tag.FechaCreacion,
                                          FechaModificacion = t.Key.Activo.Tag.FechaModificacion,
                                          FechaBaja = t.Key.Activo.Tag.FechaBaja,
                                          FechaReactivacion = t.Key.Activo.Tag.FechaReactivacion,
                                          Estado = t.Key.Activo.Tag.Estado,
                                          EmpresaId = t.Key.Activo.Tag.EmpresaId,
                                          Id = t.Key.Activo.Tag.Id,
                                          TipoTagId = t.Key.Activo.Tag.TipoTagId,
                                          Numero = t.Key.Activo.Tag.Numero,
                                          Fc = t.Key.Activo.Tag.Fc,
                                          Vence = t.Key.Activo.Tag.Vence
                                      }
                                  }).ToList()
                              }).Where(ee => ee.UbicacionId == locationId).ToList()
                          }).ToList();

            var query = queryq.Select(s
                => s.Ubicacion.FirstOrDefault(f => f.UbicacionId == locationId)).ToList();

            var queryy = query.FirstOrDefault(f => f.UbicacionId == locationId);


            return await Task.FromResult(queryy);
        }
    }
}