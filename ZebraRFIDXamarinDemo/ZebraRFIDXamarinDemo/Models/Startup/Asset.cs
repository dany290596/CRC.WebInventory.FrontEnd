using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    [Table(@"Asset")]
    public class Asset : BaseEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid? UbicacionId { get; set; }
        public Guid? GrupoActivoId { get; set; }
        public Guid? TipoActivoId { get; set; }
        public string? Codigo { get; set; }
        public string? Serie { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Descripcion { get; set; }
        public string? Nombre { get; set; }
        public string? Observaciones { get; set; } // Este campo se va a modificar

        [ForeignKey(typeof(PhysicalState))]
        public Guid? EstadoFisicoId { get; set; } // Este campo se va a modificar

        public Guid? TagId { get; set; }
        public Guid? ColaboradorHabitualId { get; set; }
        public Guid? ColaboradorResponsableId { get; set; }
        public double? ValorCompra { get; set; }
        public DateTime? FechaCompra { get; set; }
        public string? Proveedor { get; set; }
        public DateTime? FechaFinGarantia { get; set; }
        public string? TieneFoto { get; set; }
        public string? TieneArchivo { get; set; }
        public DateTime? FechaCapitalizacion { get; set; }
        public string? FichaResguardo { get; set; }
        public string? CampoLibre1 { get; set; }
        public string? CampoLibre2 { get; set; }
        public string? CampoLibre3 { get; set; }
        public string? CampoLibre4 { get; set; }
        public string? CampoLibre5 { get; set; }
        public Guid? AreaId { get; set; }
        public bool Status { get; set; }
        public string StatusNombre { get { return OnStatusNombre(Status); } }
        public Guid? Motivo { get; set; } = null;

        [ManyToOne]
        public PhysicalState EstadoFisico { get; set; }

        public string OnStatusNombre(bool status)
        {
            string s = "";
            if (status == true)
            {
                s = "Sí";
            }
            if (status == false)
            {
                s = "No";
            }
            return s;
        }
    }
}