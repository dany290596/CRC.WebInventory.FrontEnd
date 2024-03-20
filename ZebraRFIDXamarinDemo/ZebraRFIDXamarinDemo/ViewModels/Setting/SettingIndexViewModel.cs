using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Views.Setting;

namespace ZebraRFIDXamarinDemo.ViewModels.Setting
{
    public class SettingIndexViewModel : SettingBaseViewModel
    {
        public Command LoadSettingCommand { get; }
        public ObservableCollection<Models.Setting.Setting> SettingData { get; }
        public Command CreateSettingCommand { get; }
        public Command DeleteSettingCommand { get; }

        public SettingIndexViewModel(INavigation _navigation)
        {
            LoadSettingCommand = new Command(async () => await ExecuteLoadPersonCommand());
            SettingData = new ObservableCollection<Models.Setting.Setting>();
            Navigation = _navigation;
            CreateSettingCommand = new Command(OnCreateSetting);
            Device = new Models.Setting.Setting();
            DeleteSettingCommand = new Command<Models.Setting.Setting>(OnDeleteSetting);
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
                SettingData.Clear();
                var list = await App.settingRepository.GetAllAsync();
                foreach (var item in list)
                {
                    SettingData.Add(item);
                }
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

        private async void OnCreateSetting(object obj)
        {
            await Shell.Current.GoToAsync(nameof(SettingCreate));
            //var device = Device;
            //proyectos@crcdemexico.com.mx
            //Preferences.Set("device", device.Id);
        }

        private async void OnDeleteSetting(Models.Setting.Setting data)
        {
            if (data == null)
            {
                return;
            }

            var action = await Application.Current.MainPage.DisplayAlert("Advertencia", "Al realizar esta acción, todos los inventarios que estén asociados a este identificador serán borrados definitivamente del aplicativo. ¿Desea continuar?", "Sí", "No");
            if (action == true)
            {
                await App.settingRepository.DeleteAsync(data.Id);
                await App.personRepository.DeleteAllAsync();
                await App.inventoryRepository.DeleteAllAsync();
                await App.deviceRepository.DeleteAllAsync();
                await App.inventoryRepository.DeleteAllAsync();
                await App.collaboratorRepository.DeleteAllAsync();
                await App.locationRepository.DeleteAllAsync();
                await App.inventoryDetailRepository.DeleteAllAsync();
                await App.assetRepository.DeleteAllAsync();
                await App.paramsRepositoty.DeleteAllAsync();
                await App.inventoryLocationRepository.DeleteAllAsync();
                await App.inventoryLocationAssetRepository.DeleteAllAsync();

                await ExecuteLoadPersonCommand();
                return;
            }

            return;
        }
    }
}