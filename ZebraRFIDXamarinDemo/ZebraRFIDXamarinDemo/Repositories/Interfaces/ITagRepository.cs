using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task<bool> AddAsync(Models.Startup.Tag data);
        Task<bool> UpdateAsync(Models.Startup.Tag data);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllAsync();
        Task<Models.Startup.Tag> GetByIdAsync(Guid id);
        Task<List<Models.Startup.Tag>> GetAllAsync();
    }
}