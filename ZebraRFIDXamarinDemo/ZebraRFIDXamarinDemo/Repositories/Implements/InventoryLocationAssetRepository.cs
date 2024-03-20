using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.Repositories.Interfaces;

namespace ZebraRFIDXamarinDemo.Repositories.Implements
{
    public class InventoryLocationAssetRepository : IInventoryLocationAssetRepository
    {
        public SQLiteAsyncConnection _database;
        public InventoryLocationAssetRepository(string pathDatabase)
        {
            _database = new SQLiteAsyncConnection(pathDatabase);
            _database.CreateTableAsync<InventoryLocationAsset>().Wait();
        }

        public async Task<bool> AddAsync(InventoryLocationAsset data)
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
                await _database.DeleteAsync<InventoryLocationAsset>(id);
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
                await _database.DeleteAllAsync<InventoryLocationAsset>();
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<List<InventoryLocationAsset>> GetAllAsync()
        {
            return await Task.FromResult(await _database.Table<InventoryLocationAsset>().ToListAsync());
        }

        public async Task<InventoryLocationAsset> GetByIdAsync(Guid id)
        {
            return await _database.Table<InventoryLocationAsset>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<InventoryLocationAsset> GetByLastOrDefaultAsync()
        {
            var list = await _database.Table<InventoryLocationAsset>().ToListAsync();
            return await Task.FromResult(list.LastOrDefault());
        }

        public async Task<bool> UpdateAsync(InventoryLocationAsset data)
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
    }
}