using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using ZebraRFIDXamarinDemo.Repositories.Interfaces;

namespace ZebraRFIDXamarinDemo.Repositories.Implements
{
    public class TagRepository : ITagRepository
    {
        public SQLiteAsyncConnection _database;
        public TagRepository(string pathDatabase)
        {
            _database = new SQLiteAsyncConnection(pathDatabase);
            _database.CreateTableAsync<Models.Startup.Tag>().Wait();
        }

        public async Task<bool> AddAsync(Models.Startup.Tag data)
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

        public async Task<bool> UpdateAsync(Models.Startup.Tag data)
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool ok = false;

            try
            {
                await _database.DeleteAsync<Models.Startup.Tag>(id);
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
                await _database.DeleteAllAsync<Models.Startup.Tag>();
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<Models.Startup.Tag> GetByIdAsync(Guid id)
        {
            return await _database.Table<Models.Startup.Tag>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Models.Startup.Tag>> GetAllAsync()
        {
            return await Task.FromResult(await _database.Table<Models.Startup.Tag>().ToListAsync());
        }
    }
}