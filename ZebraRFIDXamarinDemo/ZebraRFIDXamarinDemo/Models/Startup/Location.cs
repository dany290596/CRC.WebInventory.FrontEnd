using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    [Table(@"Location")]
    public class Location
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Nombre { get; set; }

        [OneToMany]
        public List<InventoryDetail>? DetalleInventario { get; set; } = null;
    }
}