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
    }
}