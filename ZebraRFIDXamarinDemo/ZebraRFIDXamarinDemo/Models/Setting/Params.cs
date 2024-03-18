using System;
using SQLite;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.Models.Setting
{
    [Table(@"Params")]
    public class Params : BaseEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public Guid TipoParamId { get; set; }
        public int Orden { get; set; }
    }
}