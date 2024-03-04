using Com.Zebra.Rfid.Api3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models;

namespace ZebraRFIDXamarinDemo.ViewModels
{
    class ReaderWiFiViewModel : BaseViewModel
    {

        private ObservableCollection<SavedProfiles> _savedProfiles;
        private ObservableCollection<AvilableProfiles> _avilableProfiles;
        public ObservableCollection<SavedProfiles> SavedProfilesList { get => _savedProfiles; set { _savedProfiles = value; OnPropertyChanged(); } }
        public ObservableCollection<AvilableProfiles> AvilableProfilesList { get => _avilableProfiles; set { _avilableProfiles = value; OnPropertyChanged(); } }


        public ReaderWiFiViewModel()
        {
            _savedProfiles = new ObservableCollection<SavedProfiles>();
            _avilableProfiles = new ObservableCollection<AvilableProfiles>();

            string status = rfidModel.GetWiFiStatus();
            if (!status.Equals("ENABLED"))
            {
                rfidModel.enableWiFi();
            }

            RefreshSavedProfiles();

            bool result = rfidModel.ScanWiFi();
            if (result)
            {

            }
        }

        internal void SelectedAvilableProfile(ItemTappedEventArgs e)
        {
            var ssid = (e.Item as AvilableProfiles).AvilableProfileSSID;
            PasswordDialogAsync(ssid);
        }

        internal void SelectedSavedProfile(ItemTappedEventArgs e)
        {
            var ssid = (e.Item as SavedProfiles).SavedProfileSSID;
            ConnectDeleteDialogAsync(ssid);
        }

        internal void Refresh()
        {
            rfidModel.ScanWiFi();
        }

        private async void PasswordDialogAsync(string ssid)
        {
            string password = await Application.Current.MainPage.DisplayPromptAsync(ssid, "Enter Password");

            bool result = rfidModel.AddWiFiProfile(ssid, password);
            if (result)
            {
                RefreshSavedProfiles();
            }
        }

        private async void ConnectDeleteDialogAsync(string ssid)
        {
            string action = await Application.Current.MainPage.DisplayActionSheet(ssid, "Share Access", "Delete Profile", "Share WiFi Access with connected reader");
            if (action.Equals("Share Access"))
            {
                bool result = rfidModel.WiFiConnect(ssid);
                if (result)
                {

                }
            }
            else if (action.Equals("Delete Profile"))
            {
                bool result = rfidModel.DeleteWiFiProfile(ssid);
                if (result)
                {
                    RefreshSavedProfiles();
                }
            }
        }

        private void RefreshSavedProfiles()
        {

            SavedProfilesList.Clear();
            List<string> ssidList = rfidModel.GetSavedWiFiProfiles();
            foreach (string ssid in ssidList)
            {
                SavedProfilesList.Add(new SavedProfiles { SavedProfileSSID = ssid });
            }
        }

        public override void WiFiNotificationEvent(string scanStatus)
        {
            switch (scanStatus)
            {
                case "ScanStart":
                    AvilableProfilesList.Clear();
                    break;
                case "connect":
                case "disconnect":
                    break;
                case "ScanStop":
                    RefreshSavedProfiles();
                    break;
            }
        }

        public override void WiFiScanResultsEvent(WifiScanData data)
        {
            AvilableProfilesList.Add(new AvilableProfiles { AvilableProfileSSID = data.Getssid() });
        }





        public class SavedProfiles
        {
            public string SavedProfileSSID { get; set; }
        }

        public class AvilableProfiles
        {
            public string AvilableProfileSSID { get; set; }
        }

    }

  
}
