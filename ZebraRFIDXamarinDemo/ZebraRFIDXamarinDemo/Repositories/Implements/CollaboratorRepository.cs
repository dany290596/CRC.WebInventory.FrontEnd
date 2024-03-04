using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using SQLite;
using ZebraRFIDXamarinDemo.Models.Startup;
using ZebraRFIDXamarinDemo.Repositories.Interfaces;

namespace ZebraRFIDXamarinDemo.Repositories.Implements
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        public SQLiteAsyncConnection _database;
        public CollaboratorRepository(string pathDatabase)
        {
            _database = new SQLiteAsyncConnection(pathDatabase);
            _database.CreateTableAsync<Collaborator>().Wait();
        }

        public async Task<bool> AddAsync(Collaborator data)
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
                await _database.DeleteAsync<Collaborator>(id);
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
                await _database.DeleteAllAsync<Collaborator>();
                ok = true;
            }
            catch (Exception ex)
            {
                ok = false;
                throw ex;
            }

            return await Task.FromResult(ok);
        }

        public async Task<List<Collaborator>> GetAllAsync()
        {
            return await Task.FromResult(await _database.Table<Collaborator>().ToListAsync());
        }

        public async Task<Collaborator> GetByIdAsync(Guid id)
        {
            return await _database.Table<Collaborator>().Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Collaborator data)
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

        public async Task<Collaborator> GetCollaboratorByIdAsync(Guid id, string token)
        {
            Collaborator collaborator = new Collaborator();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
                var httpClient = new HttpClient(httpClientHandler);

                Uri uri = new Uri("https://crcdemexico.gets-it.net:7001/api/Colaborador/GetColaborador/" + id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return await Task.FromResult(collaborator);
        }
    }
}


/*

//Metodo para obtener un colaborador por su ID
public async Task<Colaborador> GetColaborador(Guid id)
{

    Colaborador colaborador = new Colaborador();

    try
    {
        //Se obtiene la ruta de las imagenes por medio de la configuracion
        ConfiguracionGeneral confiCarpeta = await unitOfWork.ConfiguracionGeneralRepository.GetById(new Guid("9E5F87E5-9DD9-45F4-A7C1-B255DE5187E0"));

        //Se hace la referencia al repositorio el cual trae el colaborador de la base de datos
        colaborador = await unitOfWork.ColaboradorRepository.GetByIdDataComplete(id);

        //Try catch pata obtener la foto del colaborador
        //Si ocurre un error manda al colaborador sin foto
        try
        {
            //Se busca la foto dentro de la carpeta mas el nombre de la foto
            using (FileStream fileStream = File.OpenRead(confiCarpeta.Valor1 + "/" + colaborador.Foto))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                //Se convierte la foto a formato 64 para ser enviada en el json final
                fileStream.CopyTo(memoryStream);
                colaborador.Foto = Convert.ToBase64String(memoryStream.ToArray());
            }
        }
        catch (Exception ex)
        {
            //Si ocurre un error o la foto no existe el colaborador se manda sin foto
            colaborador.Foto = "Sin foto";

        }
    }
    catch (Exception ex)
    {

        logE logE = new logE();
        logE.Mensaje = ex.Message;
        logE.Controlador = "ColaboradorService";
        logE.Metodo = "GetColaborador";
        logE.AplicacionLogId = new Guid("892603D1-B81D-4782-8353-F0E8CC0ECB75");

        await logService.InsertlogE(logE, new Guid("03340F13-CED3-4893-83F1-25A783C5A772"), new Guid("2DABCD06-394F-4CD6-A0D6-B5B4150C073E"));
    }


    return colaborador;
}
*/