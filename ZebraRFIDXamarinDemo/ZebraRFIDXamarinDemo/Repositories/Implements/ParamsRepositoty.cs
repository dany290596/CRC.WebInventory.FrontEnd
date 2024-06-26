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
using ZebraRFIDXamarinDemo.Models.Setting;
using ZebraRFIDXamarinDemo.Repositories.Interfaces;
using ZebraRFIDXamarinDemo.Common;
using ZebraRFIDXamarinDemo.Models.Startup;
using System.Net.NetworkInformation;

namespace ZebraRFIDXamarinDemo.Repositories.Implements
{
    public class ParamsRepositoty : IParamsRepositoty
    {
        public SQLiteAsyncConnection _database;
        public static Common.HttpBase httpBase = new Common.HttpBase();
        public ParamsRepositoty(string pathDatabase)
        {
            _database = new SQLiteAsyncConnection(pathDatabase);
            _database.CreateTableAsync<Params>().Wait();
        }

        public async Task<bool> AddAsync(Params data)
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool ok = false;

            try
            {
                await _database.DeleteAsync<Params>(id);
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
                await _database.DeleteAllAsync<Params>();
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<List<Params>> GetAllAsync()
        {
            return await Task.FromResult(await _database.Table<Params>().ToListAsync());
        }

        public async Task<Params> GetByIdAsync(Guid id)
        {
            return await _database.Table<Params>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Params> GetByLastOrDefaultAsync()
        {
            var list = await _database.Table<Params>().ToListAsync();
            return await Task.FromResult(list.LastOrDefault());
        }

        public async Task<bool> UpdateAsync(Params data)
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

        public async Task<Api<ParamsSync>> GetParams(string token, Guid company)
        {
            Api<ParamsSync> data = new Api<ParamsSync>(false, "", 501, null);

            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    Console.WriteLine("The internet connection is available.");
                    var urlIsValid = httpBase.UrlIsValid("https://crcdemexico.gets-it.net:7001/swagger/index.html");
                    if (urlIsValid)
                    {
                        ServicePointManager.ServerCertificateValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
                        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                        var httpClientHandler = new HttpClientHandler();
                        httpClientHandler.ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
                        var httpClient = new HttpClient(httpClientHandler);
                        //962CD5F7-CF54-4124-B0CF-60F9E90CCD76
                        Uri uri = new Uri("https://crcdemexico.gets-it.net:7001/api/TipoParam/GetConParams/eb2245f4-6c99-4793-a893-066f65f8be85");

                        httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
                        httpClient.DefaultRequestHeaders.Add("Empresa", company.ToString());
                        var response = await httpClient.GetAsync(uri);

                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string content = response.Content.ReadAsStringAsync().Result;
                                data = JsonConvert.DeserializeObject<Api<ParamsSync>>(content);
                            }
                        }
                        else
                        {
                            return data;
                        }
                    }
                    else
                    {
                        data = new Api<ParamsSync>(false, "La conexión a Internet no está disponible. Conéctate a Internet e inténtalo de nuevo", 501, null);
                        return data;
                    }
                }
                else
                {
                    data = new Api<ParamsSync>(false, "La conexión a Internet no está disponible. Conéctate a Internet e inténtalo de nuevo", 501, null);
                    return data;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return data;
        }
    }
}