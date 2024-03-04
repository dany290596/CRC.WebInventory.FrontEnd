using System;
using Xamarin.Forms;

namespace ZebraRFIDXamarinDemo.ViewModels.Person
{
	public class PersonCreateViewModel : PersonBaseViewModel
	{
		public Command SavePersonCommand { get; }
		public Command CancelPersonCommand { get; }

        public PersonCreateViewModel()
		{
			SavePersonCommand = new Command(OnSavePerson);
			CancelPersonCommand = new Command(OnCancelPerson);
			this.PropertyChanged += (_, __) => SavePersonCommand.ChangeCanExecute();
            Person = new Models.Person.Person();
        }

		private async void OnSavePerson()
		{
			var person = Person;
			await App.personRepository.AddAsync(person);
            await Shell.Current.GoToAsync("..");
        }

		private async void OnCancelPerson()
		{
			await Shell.Current.GoToAsync("..");
		}
    }
}