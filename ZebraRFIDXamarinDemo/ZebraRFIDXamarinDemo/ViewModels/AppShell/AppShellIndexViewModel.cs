using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Views.Authorization;

namespace ZebraRFIDXamarinDemo.ViewModels.AppShell
{
    public class AppShellIndexViewModel : AppShellBaseViewModel
    {
        public Command LogoutCommand { get; }

        public AppShellIndexViewModel()
        {
            LogoutCommand = new Command(OnLogoutButton);
        }

        public void OnAppearing()
        {
            IsBusy = true;

        }

        async Task ExecuteLoadPersonCommand()
        {
            IsBusy = true;
            try
            {
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnLogoutButton(object sender)
        {
            await App.userInformationRepository.DeleteAllAsync();
            await Shell.Current.GoToAsync($"//{nameof(LoginIndex)}");
        }
    }
}