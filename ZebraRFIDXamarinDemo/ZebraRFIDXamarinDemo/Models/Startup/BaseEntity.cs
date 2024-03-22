using System;
using SQLite;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    public class BaseEntity
    {
        public Guid UsuarioCreadorId { get; set; }
        public Guid? UsuarioModificadorId { get; set; }
        public Guid? UsuarioBajaId { get; set; }
        public Guid? UsuarioReactivadorId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaBaja { get; set; }
        public DateTime? FechaReactivacion { get; set; }
        public byte? Estado { get; set; }
        public Guid? EmpresaId { get; set; }
        public string? EstadoNombre { get { return OnEstadoNombre(Estado); } }
        public string? EstadoColor { get { return OnEstadoColor(Estado); } }

        public string OnEstadoNombre(byte? estado)
        {
            string s = "";
            if (estado == 1)
            {
                s = "Activo";
            }
            if (estado == 2)
            {
                s = "Inactivo";
            }
            return s;
        }

        public string OnEstadoColor(byte? estado)
        {
            string s = "";
            if (estado == 1)
            {
                s = "#00d391";
            }
            if (estado == 2)
            {
                s = "#c5000f";
            }
            return s;
        }
    }
}