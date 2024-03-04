using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZebraRFIDXamarinDemo.ViewModels.Inventory;
using ZebraRFIDXamarinDemo.ViewModels.Shared;

namespace ZebraRFIDXamarinDemo.Views.Shared
{
	public partial class LoadingIndex : ContentPage
	{
		LoadingIndexViewModel loadingIndexViewModel;
        public LoadingIndex()
		{
			InitializeComponent();
			BindingContext = loadingIndexViewModel = new LoadingIndexViewModel();
        }
    }
}