using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.Repositories.Interfaces;

namespace ZebraRFIDXamarinDemo.Repositories.Implements
{
    public class InventoryLocationRepository : IInventoryLocationRepository
    {
        public SQLiteAsyncConnection _database;
        public InventoryLocationRepository(string pathDatabase)
        {
            _database = new SQLiteAsyncConnection(pathDatabase);
            _database.CreateTableAsync<InventoryLocation>().Wait();
        }

        public async Task<bool> AddAsync(InventoryLocation data)
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
                await _database.DeleteAsync<InventoryLocation>(id);
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
                await _database.DeleteAllAsync<InventoryLocation>();
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<List<InventoryLocation>> GetAllAsync()
        {
            var inventoryLocationdi = await _database.QueryAsync<InventoryLocation>("SELECT * FROM InventoryLocation");

            return await Task.FromResult(inventoryLocationdi.ToList());
        }

        public async Task<InventoryLocation> GetByIdAsync(Guid id)
        {
            return await _database.Table<InventoryLocation>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(InventoryLocation data)
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