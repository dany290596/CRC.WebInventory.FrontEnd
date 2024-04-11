using System;
using Xamarin.Forms;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ZebraRFIDXamarinDemo.ViewModels.Setting
{
    public class SettingCreateViewModel : SettingBaseViewModel
    {
        public Command SaveSettingCommand { get; }
        public Command CancelSettingCommand { get; }

        public SettingCreateViewModel()
        {
            SaveSettingCommand = new Command(OnSaveSetting);
            CancelSettingCommand = new Command(OnCancelSetting);
            Device = new Models.Setting.Setting();
        }

        private async void OnSaveSetting()
        {
            var device = Device;
            if (device.Id != null)
            {
                if (device.Id == "962CD5F7-CF54-4124-B0CF-60F9E90CCD76" || device.Id == "2A80F6AC-54F5-4662-A58D-645F2A98FD8D") {
                    var list = await App.settingRepository.GetAllAsync();
                    if (list.Count() == 0)
                    {
                        await App.settingRepository.AddAsync(device);
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Mensaje", "Ya existe un identificador de un dispositivo, para agregar un identificador nuevo, borre el identificador del dispositivo existente", "Aceptar");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Mensaje", "Ingrese un identificador de dispositivo válido", "Aceptar");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Mensaje", "Ingrese el identificador del dispositivo", "Aceptar");
                // await DisplayAlert("Mensaje", "Ingrese un identificador", "Aceptar");
                // return;
            }
        }

        private async void OnCancelSetting()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}