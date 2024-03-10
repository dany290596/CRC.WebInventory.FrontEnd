using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        Task<bool> AddAsync(Inventory data);
        Task<bool> UpdateAsync(Inventory data);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllAsync();
        Task<Inventory> GetByIdAsync(Guid id);
        Task<List<InventoryQuery>> GetAllAsync();
    }
}