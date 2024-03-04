using System;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    public class InventorySync
    {
        public Inventory Inventario { get; set; }
        public Guid? UsuarioId { get; set; }
        public string? correo { get; set; }
        public string? contrasena { get; set; }
    }
}