using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface IAssetRepository
    {
        Task<bool> AddAsync(Asset data);
        Task<bool> UpdateAsync(Asset data);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllAsync();
        Task<Asset> GetByIdAsync(Guid id);
        Task<List<Asset>> GetAllAsync();
    }
}