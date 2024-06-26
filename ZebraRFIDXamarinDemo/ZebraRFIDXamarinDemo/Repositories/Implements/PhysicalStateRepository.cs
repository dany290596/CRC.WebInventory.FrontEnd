using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using SQLite;
using ZebraRFIDXamarinDemo.Models.Api;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.Repositories.Interfaces;
using ZebraRFIDXamarinDemo.Common;
using System.Net.NetworkInformation;

namespace ZebraRFIDXamarinDemo.Repositories.Implements
{
    public class PhysicalStateRepository : IPhysicalStateRepository
    {
        public SQLiteAsyncConnection _database;
        public static Common.HttpBase httpBase = new Common.HttpBase();
        public PhysicalStateRepository(string pathDatabase)
        {
            _database = new SQLiteAsyncConnection(pathDatabase);
            _database.CreateTableAsync<PhysicalState>().Wait();
        }

        public async Task<bool> AddAsync(PhysicalState data)
        {
            bool ok = false;

            try
            {
                if (data != null)
                {
                    if (data.Id != null)
                    {
                        await _database.InsertAsync(data);
                        ok = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool ok = false;

            try
            {
                await _database.DeleteAsync<PhysicalState>(id);
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
                await _database.DeleteAllAsync<PhysicalState>();
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<List<PhysicalState>> GetAllAsync()
        {
            return await Task.FromResult(await _database.Table<PhysicalState>().ToListAsync());
        }

        public async Task<PhysicalState> GetByIdAsync(Guid id)
        {
            return await _database.Table<PhysicalState>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<PhysicalState> GetByLastOrDefaultAsync()
        {
            var list = await _database.Table<PhysicalState>().ToListAsync();
            return await Task.FromResult(list.LastOrDefault());
        }

        public async Task<bool> UpdateAsync(PhysicalState data)
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

        public async Task<Api<PhysicalState>> GetPhysicalStateById(string token, Guid company, Guid id)
        {
            Api<PhysicalState> data = new Api<PhysicalState>(false, "", 200, null);

            ServicePointManager.ServerCertificateValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
            var httpClient = new HttpClient(httpClientHandler);
            //962CD5F7-CF54-4124-B0CF-60F9E90CCD76
            Uri uri = new Uri("https://crcdemexico.gets-it.net:7001/api/EstadoFisico/GetEstadoFisicoById/" + id);

            // Uri uri = new Uri("https://192.168.1.111:8084/api/EstadoFisico/GetEstadoFisicoById/" + id);
            httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
            httpClient.DefaultRequestHeaders.Add("Empresa", company.ToString());
            var response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<Api<PhysicalState>>(content);
                }
            }
            else
            {
                return await Task.FromResult(data);
            }

            return await Task.FromResult(data);
        }

        public async Task<Api<List<PhysicalState>>> GetAllPhysicalState(string token, Guid company)
        {
            Api<List<PhysicalState>> data = new Api<List<PhysicalState>>(false, "", 501, null);

            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    var urlIsValid = httpBase.UrlIsValid("https://crcdemexico.gets-it.net:7001/swagger/index.html");
                    if (urlIsValid)
                    {
                        ServicePointManager.ServerCertificateValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
                        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                        var httpClientHandler = new HttpClientHandler();
                        httpClientHandler.ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
                        var httpClient = new HttpClient(httpClientHandler);
                        //962CD5F7-CF54-4124-B0CF-60F9E90CCD76
                        Uri uri = new Uri("https://crcdemexico.gets-it.net:7001/api/EstadoFisico/GetAllEstadoFisico?PageSize=1");

                        httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
                        httpClient.DefaultRequestHeaders.Add("Empresa", company.ToString());
                        var response = await httpClient.GetAsync(uri);

                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string content = response.Content.ReadAsStringAsync().Result;
                                data = JsonConvert.DeserializeObject<Api<List<PhysicalState>>>(content);
                            }
                        }
                        else
                        {

                            return await Task.FromResult(data);
                        }

                    }
                    else
                    {
                        data = new Api<List<PhysicalState>>(false, "La conexión a Internet no está disponible. Conéctate a Internet e inténtalo de nuevo", 501, null);
                        return data;
                    }
                }
                else
                {
                    data = new Api<List<PhysicalState>>(false, "La conexión a Internet no está disponible. Conéctate a Internet e inténtalo de nuevo", 501, null);
                    return data;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return await Task.FromResult(data);
        }
    }
}