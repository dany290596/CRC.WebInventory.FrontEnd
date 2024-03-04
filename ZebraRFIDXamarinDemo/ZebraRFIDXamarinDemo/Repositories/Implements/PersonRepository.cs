using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Repositories.Interfaces;
using ZebraRFIDXamarinDemo.Models.Person;

namespace ZebraRFIDXamarinDemo.Repositories.Implements
{
    public class PersonRepository : IPersonRepository
    {
        public SQLiteAsyncConnection _database;
        public PersonRepository(string pathDatabase)
        {
            _database = new SQLiteAsyncConnection(pathDatabase);
            _database.CreateTableAsync<Person>().Wait();
        }

        public async Task<bool> AddAsync(Person person)
        {
            if (person.Id > 0)
            {
                await _database.UpdateAsync(person);
            }
            else
            {
                await _database.InsertAsync(person);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _database.DeleteAsync<Person>(id);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAllAsync()
        {
            await _database.DeleteAllAsync<Person>();
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await Task.FromResult(await _database.Table<Person>().ToListAsync());
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await _database.Table<Person>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Person person)
        {
            await _database.UpdateAsync(person);
            return await Task.FromResult(true);
        }
    }
}