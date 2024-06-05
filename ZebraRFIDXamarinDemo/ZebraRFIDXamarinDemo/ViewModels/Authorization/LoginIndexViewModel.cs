using System;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Api;
using ZebraRFIDXamarinDemo.Models.Login;
using ZebraRFIDXamarinDemo.Repositories.Implements;
using ZebraRFIDXamarinDemo.Views.Dashboard;
using ZebraRFIDXamarinDemo.Models.Token;
using System.Text;
using Xamarin.Essentials;
using System.Linq;
using ZebraRFIDXamarinDemo.Models.Sesion;
using ZebraRFIDXamarinDemo.Controls;
using ZebraRFIDXamarinDemo.Views.Api;

namespace ZebraRFIDXamarinDemo.ViewModels.Authorization
{
    public class LoginIndexViewModel : LoginBaseViewModel
    {
        public Command StartLoginCommand { get; }
        public Command StartSettingCommand { get; }

        public LoginIndexViewModel(INavigation _navigation)
        {
            StartLoginCommand = new Command(OnStartLoginCommand);
            StartSettingCommand = new Command(OnStartSettingCommand);
            Login = new Login();
            Navigation = _navigation;
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async void OnStartLoginCommand()
        {
            try
            {
                IsRunning = true;

                var login = Login;

                if (login.Email == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Mensaje", "Ingrese un correo", "Aceptar");
                    IsRunning = false;
                    return;
                }
                if (login.Contrasena == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Mensaje", "Ingrese una contraseña", "Aceptar");
                    IsRunning = false;
                    return;
                }
                Api<string> token = new Api<string>(false, "", 200, "");
                Api<Token> tokenData = new Api<Token>(false, "", 200, null);

                ServicePointManager.ServerCertificateValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;
                var httpClient = new HttpClient(httpClientHandler);

                Uri uri = new Uri("https://crcdemexico.gets-it.net:7001/api/");

                var jsonLogin = JsonConvert.SerializeObject(login);
                var requestLogin = new StringContent(jsonLogin, Encoding.UTF8, "application/json");
                var responseLogin = await httpClient.PostAsync(uri + "Login", requestLogin);
                if (responseLogin.IsSuccessStatusCode)
                {
                    IsRunning = false;
                    if (responseLogin.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string contentLogin = responseLogin.Content.ReadAsStringAsync().Result;
                        token = JsonConvert.DeserializeObject<Api<string>>(contentLogin);
                        if (token.Respuesta)
                        {
                            httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token.Data));
                            var responseUsuario = await httpClient.GetAsync(uri + "Usuario/GetUsuarioData");
                            if (responseUsuario.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string contentUsuario = responseUsuario.Content.ReadAsStringAsync().Result;
                                tokenData = JsonConvert.DeserializeObject<Api<Token>>(contentUsuario);
                                if (tokenData.Respuesta)
                                {
                                    var userInformationGetAll = await App.userInformationRepository.GetAllAsync();
                                    if (userInformationGetAll.Count() > 0)
                                    {
                                        await App.userInformationRepository.DeleteAllAsync();
                                    }
                                    UserInformation user = new UserInformation();
                                    user.Token = token.Data;
                                    user.EmpresaId = tokenData.Data.EmpresaId;
                                    user.Id = tokenData.Data.UsuarioId;
                                    user.EmailUsuario = login.Email;
                                    var userInformation = await App.userInformationRepository.AddAsync(user);
                                    if (userInformation == true)
                                    {
                                        //await App.collaboratorRepository.GetCollaboratorByIdAsync();
                                        /*
                                        Preferences.Set("token", token.Data);
                                        Preferences.Set("company", tokenData.Data.EmpresaId.ToString());
                                        Preferences.Set("user", tokenData.Data.UsuarioId.ToString());
                                        */
                                        var information = await App.userInformationRepository.GetByLastOrDefaultAsync();
                                        if (information != null)
                                        {
                                            ZebraRFIDXamarinDemo.AppShell.Current.FlyoutHeader = new FlyoutHeaderControl(information);
                                            await Shell.Current.GoToAsync($"//{nameof(DashboardIndex)}");
                                        }
                                    }
                                }
                                else
                                {
                                    await Application.Current.MainPage.DisplayAlert("Mensaje", "El token no se validó correctamente", "Aceptar");
                                }
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Mensaje", "El token no se validó correctamente", "Aceptar");
                            }
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Mensaje", "El email y la contraseña son incorrectos", "Aceptar");
                    }
                }
                else
                {
                    IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert("Mensaje", "El email y la contraseña son incorrectos", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                IsRunning = false;
                throw ex;
            }
        }

        private async void OnStartSettingCommand()
        {
            await Shell.Current.GoToAsync($"//{nameof(ApiIndex)}");
        }
    }
}