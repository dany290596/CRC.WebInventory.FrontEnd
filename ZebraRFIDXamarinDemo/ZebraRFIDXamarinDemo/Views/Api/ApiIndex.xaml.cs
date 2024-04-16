using Xamarin.Forms;
using ZebraRFIDXamarinDemo.ViewModels.Api;

namespace ZebraRFIDXamarinDemo.Views.Api
{
    public partial class ApiIndex : ContentPage
    {
        ApiIndexViewModel apiIndexViewModel;

        public ApiIndex()
        {
            InitializeComponent();
            BindingContext = apiIndexViewModel = new ApiIndexViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            apiIndexViewModel.OnAppearing();
        }
    }
}