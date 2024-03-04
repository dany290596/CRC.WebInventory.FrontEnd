using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Models.Setting;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface ISettingRepository
    {
        Task<bool> AddAsync(Setting data);
        Task<bool> UpdateAsync(Setting data);
        Task<bool> DeleteAsync(string id);
        Task<bool> DeleteAllAsync();
        Task<Setting> GetByIdAsync(string id);
        Task<List<Setting>> GetAllAsync();
    }
}