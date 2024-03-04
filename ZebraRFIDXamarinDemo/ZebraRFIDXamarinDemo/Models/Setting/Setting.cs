using System;
using SQLite;

namespace ZebraRFIDXamarinDemo.Models.Setting
{
    [Table(@"Setting")]
    public class Setting
    {
        [PrimaryKey]
        public string Id { get; set; }
    }
}