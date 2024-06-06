using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZebraRFIDXamarinDemo.ViewModels.Inventory;
using ZebraRFIDXamarinDemo.ViewModels.Tag;

namespace ZebraRFIDXamarinDemo.Views.Tag
{
    public partial class TagIndex : ContentPage
    {
        TagIndexViewModel tagIndexViewModel;
        public TagIndex()
        {
            InitializeComponent();
            BindingContext = tagIndexViewModel = new TagIndexViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            tagIndexViewModel.UpdateIn();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            tagIndexViewModel.UpdateOut();
        }
    }
}