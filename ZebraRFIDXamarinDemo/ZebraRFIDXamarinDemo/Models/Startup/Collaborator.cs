using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    [Table(@"Collaborator")]
    public class Collaborator : BaseEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public Guid? EstadoCivilId { get; set; }
        public byte? Genero { get; set; }
        public Guid? TipoIdentificacionId { get; set; }
        public string? Identificacion { get; set; }
        public string? Foto { get; set; }
        public string? ExtensionFoto { get; set; }
        public string? NumEmpleado { get; set; }
        public Guid? PuestoId { get; set; }
        public Guid? UbicacionId { get; set; }
        public Guid AreaId { get; set; }
        public Guid? TipoColaboradorId { get; set; }
        public string? TelefonoMovil { get; set; }
        public string? TelefonoOficina { get; set; }
        public string? Email { get; set; }
        public string? Email_secundario { get; set; }
        public byte? PortalWV { get; set; }
        public string? CollaboratorName { get { return Nombre + " " + ApellidoPaterno + " " + ApellidoMaterno; } }

        //[OneToMany(CascadeOperations = CascadeOperation.All)]      // One to many relationship with Valuation
        //[OneToMany]
        //[OneToMany(CascadeOperations = CascadeOperation.All)]
        [OneToMany]
        public List<Inventory> Inventory { get; set; }
    }
}