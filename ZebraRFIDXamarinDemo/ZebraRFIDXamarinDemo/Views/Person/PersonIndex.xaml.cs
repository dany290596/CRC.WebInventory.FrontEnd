using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZebraRFIDXamarinDemo.ViewModels.Person;

namespace ZebraRFIDXamarinDemo.Views.Person
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PersonIndex : ContentPage
	{
		PersonIndexViewModel personIndexViewModel;

        public PersonIndex ()
		{
			InitializeComponent();
			BindingContext = personIndexViewModel = new PersonIndexViewModel(Navigation);
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
			personIndexViewModel.OnAppearing();
        }
    }
}

