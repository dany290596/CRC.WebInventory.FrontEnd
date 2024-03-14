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
        public byte Status { get; set; }    /* 1.Por inventariar - 2.Finalizado */
        public string StatusNombre { get { return OnStatusNombre(Status); } }    /* 1.Por inventariar - 2.Finalizado */

        [OneToMany]
        public List<InventoryDetail>? DetalleInventario { get; set; } = null;

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