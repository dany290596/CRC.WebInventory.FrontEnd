using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Api;
using ZebraRFIDXamarinDemo.Models.Login;
using ZebraRFIDXamarinDemo.Models.Token;
using ZebraRFIDXamarinDemo.ViewModels.Authorization;
using ZebraRFIDXamarinDemo.Views.Dashboard;

namespace ZebraRFIDXamarinDemo.Views.Authorization
{
    public partial class LoginIndex : ContentPage
    {
        LoginIndexViewModel loginIndexViewModel;
        public LoginIndex()
        {
            InitializeComponent();
            BindingContext = loginIndexViewModel = new LoginIndexViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loginIndexViewModel.OnAppearing();
        }

        private async void OnLoginButton(object sender, EventArgs e)
        {
            Login login = new Login
            {
                Email = txtEmail.Text,
                Contrasena = txtPassword.Text
            };
            if (login.Email == null)
            {
                await DisplayAlert("Mensaje", "Ingrese un correo", "Aceptar");
                return;
            }
            if (login.Contrasena == null)
            {
                await DisplayAlert("Mensaje", "Ingrese una contraseña", "Aceptar");
                return;
            }
            Api<string> token = new Api<string>(false, "", 200, "");
            Api<Token> tokenData = new Api<Token>(false, "", 200, null);
            ServicePointManager.ServerCertificateValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;

            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
                (message, certificate, chain, sslPolicyErrors) => true;
            var httpClient = new HttpClient(httpClientHandler);



            Uri uri = new Uri("https://crcdemexico.gets-it.net:7001/api/");



            var jsonLogin = JsonConvert.SerializeObject(login);
            var requestLogin = new StringContent(jsonLogin, Encoding.UTF8, "application/json");
            var responseLogin = await httpClient.PostAsync(uri + "login", requestLogin);
            if (responseLogin.IsSuccessStatusCode)
            {
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
                                Preferences.Set("token", token.Data);
                                Preferences.Set("company", tokenData.Data.EmpresaId.ToString());
                                Preferences.Set("user", tokenData.Data.UsuarioId.ToString());
                                await Shell.Current.GoToAsync($"//{nameof(DashboardIndex)}");
                            }
                            else
                            {
                                await DisplayAlert("Mensaje", "El token no se validó correctamente", "Aceptar");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Mensaje", "El token no se validó correctamente", "Aceptar");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Mensaje", "El email y la contraseña son incorrectos", "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("Mensaje", "El email y la contraseña son incorrectos", "Aceptar");
            }
        }
    }
}