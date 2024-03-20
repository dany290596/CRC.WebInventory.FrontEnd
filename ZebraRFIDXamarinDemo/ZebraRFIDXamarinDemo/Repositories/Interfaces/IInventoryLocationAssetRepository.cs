using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface IInventoryLocationAssetRepository
    {
        Task<bool> AddAsync(InventoryLocationAsset data);
        Task<bool> UpdateAsync(InventoryLocationAsset data);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllAsync();
        Task<InventoryLocationAsset> GetByIdAsync(Guid id);
        Task<List<InventoryLocationAsset>> GetAllAsync();
    }
}