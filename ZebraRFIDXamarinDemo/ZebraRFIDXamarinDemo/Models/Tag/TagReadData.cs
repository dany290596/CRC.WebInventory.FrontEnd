using System;
namespace ZebraRFIDXamarinDemo.Models.Tag
{
    public class TagReadData
    {
        public string EPC { get; internal set; }
        public string TID { get; internal set; }
        public string UMB { get; internal set; }
    }
}