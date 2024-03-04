using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        Task<bool> AddAsync(Location data);
        Task<bool> UpdateAsync(Location data);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllAsync();
        Task<Location> GetByIdAsync(Guid id);
        Task<List<Location>> GetAllAsync();
    }
}