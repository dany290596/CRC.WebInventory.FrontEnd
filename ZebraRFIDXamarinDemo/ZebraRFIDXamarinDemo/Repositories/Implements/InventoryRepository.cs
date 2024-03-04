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

        public async Task<List<Inventory>> GetAllAsync()
        {
            var collaboratordi = await _database.QueryAsync<Collaborator>("SELECT * FROM Collaborator");
            var assetdi = await _database.QueryAsync<Asset>("SELECT * FROM Asset");
            var devicedi = await _database.QueryAsync<Device>("SELECT * FROM Device");
            var inventorydi = await _database.QueryAsync<Inventory>("SELECT * FROM Inventory");
            var locationdi = await _database.QueryAsync<Location>("SELECT * FROM Location");
            var inventoryDetaildi = await _database.QueryAsync<InventoryDetail>("SELECT * FROM InventoryDetail");
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
                                  DetalleInventario = inventoryDetaildi.Where(a => a.Id == id.Id).Select(p => new InventoryDetail()
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
            var inventorydi = await _database.QueryAsync<Inventory>("SELECT * FROM Inventory");
            var inventoryDetaildi = await _database.QueryAsync<InventoryDetail>("SELECT * FROM InventoryDetail");
            var querydi = from id in inventoryDetaildi

                          join i in inventorydi on id.InventarioId equals i.Id
                          into InventoryDetailInventory
                          from idi in InventoryDetailInventory.DefaultIfEmpty()

                          select new InventoryLoad()
                          {
                              Id = idi.Id,
                              Observaciones = idi.Observaciones,
                              FechaInventario = idi.FechaInventario,
                              DetalleInventario = inventoryDetaildi.Where(w => w.Id == id.Id).Select(s => new InventoryDetailLoad()
                              {
                                  Id = s.Id,
                                  ActivoId = s.ActivoId,
                                  Observaciones = s.Observaciones,
                                  EstadoFisicoId = s.EstadoFisicoId
                              }).ToList()
                          };
            return await Task.FromResult(querydi.ToList());
        }
    }
}