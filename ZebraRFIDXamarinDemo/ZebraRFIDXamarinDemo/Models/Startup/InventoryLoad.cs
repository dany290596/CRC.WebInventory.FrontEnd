using System;
using System.Collections.Generic;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    public class InventoryLoad
    {
        public Guid Id { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaInventario { get; set; }
        public List<InventoryDetailLoad>? DetalleInventario { get; set; }
    }

    public class InventoryDetailLoad
    {
        public Guid Id { get; set; }
        public Guid? ActivoId { get; set; }
        public string? Observaciones { get; set; }
        public Guid? EstadoFisicoId { get; set; }
    }
}