using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using ZebraRFIDXamarinDemo.Models.Api;
using ZebraRFIDXamarinDemo.Models.Setting;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.Repositories.Interfaces;

namespace ZebraRFIDXamarinDemo.Repositories.Implements
{
    public class SettingRepository : ISettingRepository
    {
        public SQLiteAsyncConnection _database;
        public SettingRepository(string pathDatabase)
        {
            _database = new SQLiteAsyncConnection(pathDatabase);
            _database.CreateTableAsync<Setting>().Wait();
        }

        public async Task<bool> AddAsync(Setting data)
        {
            bool ok = false;

            try
            {
                await _database.InsertAsync(data);
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            bool ok = false;

            try
            {
                await _database.DeleteAsync<Setting>(id);
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<bool> DeleteAllAsync()
        {
            bool ok = false;

            try
            {
                await _database.DeleteAllAsync<Setting>();
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<List<Setting>> GetAllAsync()
        {
            return await Task.FromResult(await _database.Table<Setting>().ToListAsync());
        }

        public async Task<Setting> GetByIdAsync(string id)
        {
            return await _database.Table<Setting>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Setting> GetByLastOrDefaultAsync()
        {
            var list = await _database.Table<Setting>().ToListAsync();
            return await Task.FromResult(list.LastOrDefault());
        }

        public async Task<bool> UpdateAsync(Setting data)
        {
            bool ok = false;

            try
            {
                await _database.UpdateAsync(data);
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<Api<List<InventorySync>>> GetInventorySync(string token, string company, string device)
        {
            List<InventorySync> inventorySync = new List<InventorySync>();
            Api<List<InventorySync>> data = new Api<List<InventorySync>>(false, "", 200, null);

            ServicePointManager.ServerCertificateValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
            var httpClient = new HttpClient(httpClientHandler);
            //962CD5F7-CF54-4124-B0CF-60F9E90CCD76
            Uri uri = new Uri("https://crcdemexico.gets-it.net:7001/api/Inventario/GetInventarioSync/" + device);

            httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            httpClient.DefaultRequestHeaders.Add("Empresa", company);
            var response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<Api<List<InventorySync>>>(content);
                }
            }
            else
            {
                return data;
            }

            return data;
        }

        public async Task<Api<string>> GetInventoryLoad(string token, string company, InventoryLoad inventory)
        {
            List<InventorySync> inventorySync = new List<InventorySync>();
            Api<string> data = new Api<string>(false, "", 200, null);

            ServicePointManager.ServerCertificateValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
            var httpClient = new HttpClient(httpClientHandler);
            //962CD5F7-CF54-4124-B0CF-60F9E90CCD76
            Uri uri = new Uri("https://crcdemexico.gets-it.net:7001/api/Inventario/SubirInventario?id=" + inventory.Id);

            var json = JsonConvert.SerializeObject(inventory);
            var request = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            httpClient.DefaultRequestHeaders.Add("Empresa", company);
            var response = await httpClient.PutAsync(uri, request);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<Api<string>>(content);
                }
            }
            else
            {
                return data;
            }

            return data;
        }
    }
}