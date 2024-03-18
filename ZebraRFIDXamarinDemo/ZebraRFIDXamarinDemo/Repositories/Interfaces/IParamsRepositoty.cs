using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Models.Setting;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface IParamsRepositoty
    {
        Task<bool> AddAsync(Params data);
        Task<bool> UpdateAsync(Params data);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllAsync();
        Task<Params> GetByIdAsync(Guid id);
        Task<List<Params>> GetAllAsync();
    }
}