using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Api;
using static AndroidX.ConstraintLayout.Core.Motion.Utils.HyperSpline;

namespace ZebraRFIDXamarinDemo.ViewModels.Api
{
    public class ApiBaseViewModel : INotifyPropertyChanged
    {
        private Url _url;
        public INavigation Navigation { get; set; }
        public Url Url
        {
            get { return _url; }
            set { _url = value; OnPropertyChanged(); }
        }
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                SetProperty(ref isBusy, value);
            }
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChaged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChaged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}