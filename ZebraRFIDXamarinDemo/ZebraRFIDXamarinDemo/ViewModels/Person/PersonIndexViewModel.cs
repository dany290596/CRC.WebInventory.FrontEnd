using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Views.Person;

namespace ZebraRFIDXamarinDemo.ViewModels.Person
{
    public class PersonIndexViewModel : PersonBaseViewModel
    {
        public Command LoadPersonCommand { get; }
        public ObservableCollection<Models.Person.Person> PersonData { get; }
        public Command CreatePersonCommand { get; }
        public Command EditPersonCommand { get; }
        public Command DeletePersonCommand { get; }

        public PersonIndexViewModel(INavigation _navigation)
        {
            LoadPersonCommand = new Command(async () => await ExecuteLoadPersonCommand());
            PersonData = new ObservableCollection<Models.Person.Person>();
            CreatePersonCommand = new Command(OnCreatePerson);
            EditPersonCommand = new Command<Models.Person.Person>(OnEditPerson);
            DeletePersonCommand = new Command<Models.Person.Person>(OnDeletePerson);
            Navigation = _navigation;
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
                PersonData.Clear();
                var personList = await App.personRepository.GetAllAsync();
                foreach (var item in personList)
                {
                    PersonData.Add(item);
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

        private async void OnCreatePerson(object obj)
        {
            await Shell.Current.GoToAsync(nameof(PersonCreate));
        }

        private async void OnEditPerson(Models.Person.Person person)
        {
            await Navigation.PushAsync(new PersonCreate(person));
        }

        private async void OnDeletePerson(Models.Person.Person person)
        {
            if (person == null)
            {
                return;
            }

            await App.personRepository.DeleteAsync(person.Id);
            await ExecuteLoadPersonCommand();
        }
    }
}