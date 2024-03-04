using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using ZebraRFIDXamarinDemo.Models.Sesion;
using ZebraRFIDXamarinDemo.Repositories.Interfaces;

namespace ZebraRFIDXamarinDemo.Repositories.Implements
{
    public class UserInformationRepository : IUserInformationRepository
    {
        public SQLiteAsyncConnection _database;
        public UserInformationRepository(string pathDatabase)
        {
            _database = new SQLiteAsyncConnection(pathDatabase);
            _database.CreateTableAsync<UserInformation>().Wait();
        }

        public async Task<bool> AddAsync(UserInformation data)
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
                await _database.DeleteAsync<UserInformation>(id);
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
                await _database.DeleteAllAsync<UserInformation>();
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }
            return await Task.FromResult(ok);
        }

        public async Task<List<UserInformation>> GetAllAsync()
        {
            var location = await _database.QueryAsync<UserInformation>("SELECT * FROM UserInformation");
            return await Task.FromResult(location);
        }

        public async Task<UserInformation> GetByIdAsync(Guid id)
        {
            return await _database.Table<UserInformation>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(UserInformation data)
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

        public async Task<UserInformation> GetByLastOrDefaultAsync()
        {
            var list = await _database.Table<UserInformation>().ToListAsync();
            return await Task.FromResult(list.LastOrDefault());
        }
    }
}