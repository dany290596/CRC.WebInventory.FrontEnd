using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Zebra.Rfid.Api3;
using SQLite;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.Repositories.Interfaces;

namespace ZebraRFIDXamarinDemo.Repositories.Implements
{
    public class InventoryDetailRepository : IInventoryDetailRepository
    {
        public SQLiteAsyncConnection _database;
        public InventoryDetailRepository(string pathDatabase)
        {
            _database = new SQLiteAsyncConnection(pathDatabase);
            _database.CreateTableAsync<InventoryDetail>().Wait();
        }

        public async Task<bool> AddAsync(InventoryDetail data)
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
                await _database.DeleteAsync<InventoryDetail>(id);
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
                await _database.DeleteAllAsync<InventoryDetail>();
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<List<InventoryDetail>> GetAllAsync()
        {
            return await Task.FromResult(await _database.Table<InventoryDetail>().ToListAsync());
        }

        public async Task<InventoryDetail> GetByIdAsync(Guid id)
        {
            return await _database.Table<InventoryDetail>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(InventoryDetail data)
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

        public async Task<List<InventoryDetail>> GetByIdLocation(Guid idLocation)
        {
            var assetdi = await App.assetRepository.GetAllAsync();

            var locationdi = await App.locationRepository.GetAllAsync();

            var inventoryDetaildi = await _database.QueryAsync<InventoryDetail>("SELECT * FROM InventoryDetail");

            var physicalStatedi = await App.physicalStateRepository.GetAllAsync();

            var querydi = (from id in inventoryDetaildi
                           where id.UbicacionId == idLocation
                           /*
                            join u in locationdi on id.UbicacionId equals u.Id
                            into InventoryDetailLocation
                            from idl in InventoryDetailLocation.DefaultIfEmpty()
                           */

                           select new InventoryDetail()
                           {
                               UsuarioCreadorId = id.UsuarioCreadorId,
                               UsuarioModificadorId = id.UsuarioModificadorId,
                               UsuarioBajaId = id.UsuarioBajaId,
                               UsuarioReactivadorId = id.UsuarioReactivadorId,
                               FechaCreacion = id.FechaCreacion,
                               FechaModificacion = id.FechaModificacion,
                               FechaBaja = id.FechaBaja,
                               FechaReactivacion = id.FechaReactivacion,
                               Estado = id.Estado,
                               EmpresaId = id.EmpresaId,
                               Id = id.Id,
                               DispositivoId = id.DispositivoId,
                               InventarioId = id.InventarioId,
                               CentroDeCostosId = id.CentroDeCostosId,
                               EstadoFisicoId = id.EstadoFisicoId,
                               UbicacionId = id.UbicacionId,
                               ActivoId = id.ActivoId,
                               PresenciaAusensia = id.PresenciaAusensia,
                               Observaciones = id.Observaciones,
                               Mantenimiento = id.Mantenimiento,
                               Activo = new Asset()
                               {
                                   UsuarioCreadorId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).UsuarioCreadorId,
                                   UsuarioModificadorId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).UsuarioModificadorId,
                                   UsuarioBajaId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).UsuarioBajaId,
                                   UsuarioReactivadorId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).UsuarioReactivadorId,
                                   FechaCreacion = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).FechaCreacion,
                                   FechaModificacion = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).FechaModificacion,
                                   FechaBaja = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).FechaBaja,
                                   FechaReactivacion = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).FechaReactivacion,
                                   Estado = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).Estado,
                                   EmpresaId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).EmpresaId,
                                   Id = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).Id,
                                   UbicacionId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).UbicacionId,
                                   GrupoActivoId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).GrupoActivoId,
                                   TipoActivoId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).TipoActivoId,
                                   Codigo = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).Codigo,
                                   Serie = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).Serie,
                                   Marca = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).Marca,
                                   Modelo = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).Modelo,
                                   Descripcion = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).Descripcion,
                                   Nombre = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).Nombre,
                                   Observaciones = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).Observaciones,
                                   EstadoFisicoId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).EstadoFisicoId,
                                   TagId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).TagId,
                                   ColaboradorHabitualId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).ColaboradorHabitualId,
                                   ColaboradorResponsableId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).ColaboradorResponsableId,
                                   ValorCompra = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).ValorCompra,
                                   FechaCompra = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).FechaCompra,
                                   Proveedor = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).Proveedor,
                                   FechaFinGarantia = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).FechaFinGarantia,
                                   TieneFoto = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).TieneFoto,
                                   TieneArchivo = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).TieneArchivo,
                                   FechaCapitalizacion = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).FechaCapitalizacion,
                                   FichaResguardo = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).FichaResguardo,
                                   CampoLibre1 = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).CampoLibre1,
                                   CampoLibre2 = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).CampoLibre2,
                                   CampoLibre3 = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).CampoLibre3,
                                   CampoLibre4 = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).CampoLibre4,
                                   CampoLibre5 = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).CampoLibre5,
                                   AreaId = assetdi.FirstOrDefault(x => x.Id == id.ActivoId).AreaId,
                                   EstadoFisico = physicalStatedi.FirstOrDefault(x => x.Id == assetdi.FirstOrDefault(x => x.Id == id.ActivoId).EstadoFisicoId)
                               }
                           }).ToList();

            return await Task.FromResult(querydi);
        }
    }
}