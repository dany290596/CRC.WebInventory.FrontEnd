using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    public class InventoryQuerySpecial
    {
        public Guid Id { get; set; }
        public Guid ColaboradorResponsableId { get; set; }
        public string? Descripcion { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaInventario { get; set; }
        public byte? Inventariado { get; set; }
        public string? NoInventario { get; set; }
        public byte? Estado { get; set; }

        public List<InventoryLocationQS> InventarioUbicacion { get; set; }
    }

    public class InventoryQS : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid ColaboradorResponsableId { get; set; }
        public string? Descripcion { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaInventario { get; set; }
        public byte? Inventariado { get; set; }
        public string? NoInventario { get; set; }

        public Collaborator ColaboradorResponsable { get; set; }

        public List<InventoryLocationQS>? Ubicaciones { get; set; } = null;
    }

    public class InventoryLocationQS
    {
        public Guid InventarioUbicacionId { get; set; }
        //public string InventarioNombre { get; set; }
        public string InventarioUbicacionNombre { get; set; }
        //public Guid InventarioId { get; set; }
        //public Guid UbicacionId { get; set; }
        //public byte Status { get; set; }

        //public Inventory Inventario { get; set; }
        public List<Location> Ubicacion { get; set; }
    }
}