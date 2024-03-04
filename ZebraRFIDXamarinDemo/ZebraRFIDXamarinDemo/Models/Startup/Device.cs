using System;
using SQLite;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    [Table(@"Device")]
    public class Device : BaseEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public int Numero { get; set; }
        public string Nombre { get; set; }
        public Guid? TipoDispositivoId { get; set; }
        public Guid? UbicacionId { get; set; }
        public string Ip { get; set; }
        public string Mac { get; set; }
        public string Identifier { get; set; } = null!;
        public int? PuertoEscucha { get; set; }
        public int? PuertoTransmision { get; set; }
        public byte? HabilitadoParaAlta { get; set; }
        public Guid? OperacionId { get; set; }
        public Guid? ModoOperacionId { get; set; }
        public byte? EstadoActual { get; set; }
        public byte? Perimetral { get; set; }
        public byte? Configurado { get; set; }
    }
}