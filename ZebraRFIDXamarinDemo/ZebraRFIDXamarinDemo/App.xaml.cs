using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZebraRFIDXamarinDemo.Repositories.Implements;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace ZebraRFIDXamarinDemo
{
    public partial class App : Application
    {
        /*
        private MainPage mainPage;
        */
        static PersonRepository _personRepository;
        static DeviceRepository _deviceRepository;
        static InventoryRepository _inventoryRepository;
        static CollaboratorRepository _collaboratorRepository;
        static LocationRepository _locationRepository;
        static SettingRepository _settingRepository;
        static InventoryDetailRepository _inventoryDetailRepository;
        static AssetRepository _assetRepository;
        static UserInformationRepository _userInformationRepository;
        static PhysicalStateRepository _physicalStateRepository;

        public static PersonRepository personRepository
        {
            get
            {
                if (_personRepository == null)
                {
                    _personRepository = new PersonRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebInventory.db"));
                }
                return _personRepository;
            }
        }

        public static DeviceRepository deviceRepository
        {
            get
            {
                if (_deviceRepository == null)
                {
                    _deviceRepository = new DeviceRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebInventory.db"));
                }
                return _deviceRepository;
            }
        }

        public static InventoryRepository inventoryRepository
        {
            get
            {
                if (_inventoryRepository == null)
                {
                    _inventoryRepository = new InventoryRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebInventory.db"));
                }
                return _inventoryRepository;
            }
        }

        public static CollaboratorRepository collaboratorRepository
        {
            get
            {
                if (_collaboratorRepository == null)
                {
                    _collaboratorRepository = new CollaboratorRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebInventory.db"));
                }
                return _collaboratorRepository;
            }
        }

        public static LocationRepository locationRepository
        {
            get
            {
                if (_locationRepository == null)
                {
                    _locationRepository = new LocationRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebInventory.db"));
                }
                return _locationRepository;
            }
        }

        public static SettingRepository settingRepository
        {
            get
            {
                if (_settingRepository == null)
                {
                    _settingRepository = new SettingRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebInventory.db"));
                }
                return _settingRepository;
            }
        }

        public static InventoryDetailRepository inventoryDetailRepository
        {
            get
            {
                if (_inventoryDetailRepository == null)
                {
                    _inventoryDetailRepository = new InventoryDetailRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebInventory.db"));
                }
                return _inventoryDetailRepository;
            }
        }

        public static AssetRepository assetRepository
        {
            get
            {
                if (_assetRepository == null)
                {
                    _assetRepository = new AssetRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebInventory.db"));
                }
                return _assetRepository;
            }
        }

        public static UserInformationRepository userInformationRepository
        {
            get
            {
                if (_userInformationRepository == null)
                {
                    _userInformationRepository = new UserInformationRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebInventory.db"));
                }
                return _userInformationRepository;
            }
        }

        public static PhysicalStateRepository physicalStateRepository
        {
            get
            {
                if (_physicalStateRepository == null)
                {
                    _physicalStateRepository = new PhysicalStateRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebInventory.db"));
                }
                return _physicalStateRepository;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // Handle when your app starts

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps

        }

        protected override void OnResume()
        {
            // Handle when your app resumes

        }
    }
}