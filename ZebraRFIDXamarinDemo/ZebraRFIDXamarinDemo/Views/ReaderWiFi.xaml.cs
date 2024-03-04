using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZebraRFIDXamarinDemo.ViewModels;

namespace ZebraRFIDXamarinDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReaderWiFi : ContentPage
    {
        private ReaderWiFiViewModel viewModel;
        public ReaderWiFi()
        {
            BindingContext = viewModel = new ReaderWiFiViewModel();
            InitializeComponent();
            Title = "WiFi Settings";
        }

        private void RefreshClicked(object sender, EventArgs e)
        {
            viewModel.Refresh();
        }

        private void OnItemTappedAvilableProfiles(object sender, ItemTappedEventArgs e)
        {
            viewModel.SelectedAvilableProfile(e);
        }

        private void OnItemTappedSavedProfiles(object sender, ItemTappedEventArgs e)
        {
            viewModel.SelectedSavedProfile(e);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.UpdateReaderWiFiEventsIn();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.UpdateReaderWiFiEventsOut();
        }
    }
}