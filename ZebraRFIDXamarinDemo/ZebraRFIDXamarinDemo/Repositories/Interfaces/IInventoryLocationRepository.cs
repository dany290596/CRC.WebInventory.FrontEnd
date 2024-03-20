using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface IInventoryLocationRepository
    {
        Task<bool> AddAsync(InventoryLocation data);
        Task<bool> UpdateAsync(InventoryLocation data);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllAsync();
        Task<InventoryLocation> GetByIdAsync(Guid id);
        Task<List<InventoryLocation>> GetAllAsync();
    }
}