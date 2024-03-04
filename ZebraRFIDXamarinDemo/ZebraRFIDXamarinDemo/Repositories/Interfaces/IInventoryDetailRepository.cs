using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface IInventoryDetailRepository
    {
        Task<bool> AddAsync(InventoryDetail data);
        Task<bool> UpdateAsync(InventoryDetail data);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllAsync();
        Task<InventoryDetail> GetByIdAsync(Guid id);
        Task<List<InventoryDetail>> GetAllAsync();
    }
}