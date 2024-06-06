using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ZebraRFIDXamarinDemo.Models.Tag
{
    public class Tag : INotifyPropertyChanged
    {
        private int _count;
        private int _rssi;
        private int _rdistance;

        public string InvID { get; set; }

        public int TagCount { get { return _count; } set { _count = value; OnPropertyChanged(); } }

        public int RSSI { get { return _rssi; } set { _rssi = value; OnPropertyChanged(); } }

        public int RelativeDistance { get { return _rdistance; } set { _rdistance = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}