using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface ICollaboratorRepository
    {
        Task<bool> AddAsync(Collaborator data);
        Task<bool> UpdateAsync(Collaborator data);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAllAsync();
        Task<Collaborator> GetByIdAsync(Guid id);
        Task<List<Collaborator>> GetAllAsync();
    }
}