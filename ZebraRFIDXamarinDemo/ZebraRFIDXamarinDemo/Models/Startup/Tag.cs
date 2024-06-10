using System;
using SQLite;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    [Table(@"Tag")]
    public class Tag : BaseEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid? TipoTagId { get; set; }
        public string? Numero { get; set; }
        public int? Fc { get; set; }
        public byte Vence { get; set; }
    }
}