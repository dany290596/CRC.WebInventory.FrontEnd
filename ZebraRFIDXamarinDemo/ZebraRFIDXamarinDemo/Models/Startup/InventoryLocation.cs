using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    [Table(@"InventoryLocation")]
    public class InventoryLocation
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(Inventory))]
        public Guid InventarioId { get; set; }

        [ForeignKey(typeof(Location))]
        public Guid UbicacionId { get; set; }

        public byte Status { get; set; }    /* 1.Por inventariar - 2.Finalizado */
        public string StatusNombre { get { return OnStatusNombre(Status); } }    /* 1.Por inventariar - 2.Finalizado */

        [ManyToOne]
        public Inventory Inventario { get; set; }

        [ManyToOne]
        public Location Ubicacion { get; set; }

        public string OnStatusNombre(byte? status)
        {
            string s = "";
            if (status == 1)
            {
                s = "Por inventariar";
            }
            if (status == 2)
            {
                s = "Finalizado";
            }
            return s;
        }
    }
}