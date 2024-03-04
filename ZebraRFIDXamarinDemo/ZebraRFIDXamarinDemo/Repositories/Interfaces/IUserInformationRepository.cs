using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Models.Sesion;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface IUserInformationRepository
    {
        Task<bool> AddAsync(UserInformation data);
        Task<bool> UpdateAsync(UserInformation data);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllAsync();
        Task<UserInformation> GetByIdAsync(Guid id);
        Task<List<UserInformation>> GetAllAsync();
    }
}