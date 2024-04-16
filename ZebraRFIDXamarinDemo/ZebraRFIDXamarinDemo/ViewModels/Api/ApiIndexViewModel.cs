using System;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.ViewModels.Authorization;
using ZebraRFIDXamarinDemo.Views.Api;
using ZebraRFIDXamarinDemo.Views.Authorization;

namespace ZebraRFIDXamarinDemo.ViewModels.Api
{
    public class ApiIndexViewModel : ApiBaseViewModel
    {
        public Command StartBackCommand { get; }

        public ApiIndexViewModel()
        {
            StartBackCommand = new Command(OnStartBackCommand);
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async void OnStartBackCommand()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginIndex)}");
        }
    }
}