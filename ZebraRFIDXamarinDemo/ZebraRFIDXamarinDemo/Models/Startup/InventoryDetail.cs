using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    [Table(@"InventoryDetail")]
    public class InventoryDetail : BaseEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(Device))]
        public Guid DispositivoId { get; set; }

        [ForeignKey(typeof(Inventory))]
        public Guid? InventarioId { get; set; }

        public Guid? CentroDeCostosId { get; set; }
        public Guid? EstadoFisicoId { get; set; }

        [ForeignKey(typeof(Location))]
        public Guid? UbicacionId { get; set; }

        [ForeignKey(typeof(Asset))]
        public Guid? ActivoId { get; set; }

        public byte? PresenciaAusensia { get; set; }
        public string? Observaciones { get; set; }
        public byte? Mantenimiento { get; set; }

        [ManyToOne]
        public Asset? Activo { get; set; }
        [ManyToOne]
        public Device Dispositivo { get; set; } = null!;
        [ManyToOne]
        public Inventory Inventario { get; set; }
        [ManyToOne]
        public Location? Ubicacion { get; set; }
    }
}