using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZebraRFIDXamarinDemo.ViewModels.Person;

namespace ZebraRFIDXamarinDemo.Views.Person
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonCreate : ContentPage
	{
        public Models.Person.Person Person { get; set; }
		public PersonCreate ()
		{
			InitializeComponent ();
            BindingContext = new PersonCreateViewModel();
		}

        public PersonCreate(Models.Person.Person person)
        {
            InitializeComponent();
            BindingContext = new PersonCreateViewModel();
            if (person != null)
            {
                ((PersonCreateViewModel)BindingContext).Person = person;
            }
        }
    }
}

