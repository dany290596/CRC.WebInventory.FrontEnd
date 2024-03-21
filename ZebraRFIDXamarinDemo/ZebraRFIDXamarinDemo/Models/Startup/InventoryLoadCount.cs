using System;
using System.Collections.Generic;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    public class InventoryLoadCount
    {
        public Guid InventarioId { get; set; }
        public int UbicacionTotal { get; set; }
        public int UbicacionPorInventariar { get; set; }
        public int UbicacionFinalizada { get; set; }

        public List<InventoryLocationAQ> Ubicacion { get; set; }
    }
}