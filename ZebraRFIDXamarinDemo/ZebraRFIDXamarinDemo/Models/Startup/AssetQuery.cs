using System;
namespace ZebraRFIDXamarinDemo.Models.Startup
{
    public class AssetQuery
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }
        public TagQuery Tag { get; set; }
    }
}