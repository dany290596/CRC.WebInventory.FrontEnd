using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface IPhysicalStateRepository
    {
        Task<bool> AddAsync(PhysicalState data);
        Task<bool> UpdateAsync(PhysicalState data);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllAsync();
        Task<PhysicalState> GetByIdAsync(Guid id);
        Task<List<PhysicalState>> GetAllAsync();
    }
}