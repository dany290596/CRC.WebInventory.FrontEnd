using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZebraRFIDXamarinDemo.Models.Api;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.Repositories.Interfaces
{
    public interface IDeviceRepository
    {
        Task<Api<List<InventorySync>>> InventorySync(string token, string company, string device);
    }
}