using System;
using SQLite;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    [Table(@"PhysicalState")]
    public class PhysicalState : BaseEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}