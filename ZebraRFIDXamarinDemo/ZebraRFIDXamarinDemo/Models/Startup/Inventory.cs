using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    [Table(@"Inventory")]
    public class Inventory : BaseEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [ForeignKey(typeof(Collaborator))]
        public Guid ColaboradorResponsableId { get; set; }
        public string? Descripcion { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaInventario { get; set; }
        public byte? Inventariado { get; set; }
        public string? NoInventario { get; set; }



        //[OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
        //[OneToOne(CascadeOperations = CascadeOperation.All)]
        [ManyToOne]
        public Collaborator ColaboradorResponsable { get; set; }
        ///public virtual ICollection<DetailInventory>? DetalleInventario { get; set; } = null;
        //[OneToMany(CascadeOperations = CascadeOperation.All)]
        [OneToMany]
        public List<Location>? Ubicaciones { get; set; } = null;
    }
}