using System;
using System.Collections.Generic;
using Java.Lang;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
    public class InventoryLocationAssetQuery
    {
        public Guid InventarioId { get; set; }
        public Guid InventarioColaboradorResponsableId { get; set; }
        public string? InventarioDescripcion { get; set; }
        public string? InventarioObservaciones { get; set; }
        public DateTime? InventarioFechaInventario { get; set; }
        public byte? InventarioInventariado { get; set; }
        public string? InventarioNoInventario { get; set; }
        public byte? InventarioEstado { get; set; }


        public string? ColaboradorNombre { get; set; }
        public string? ColaboradorApellidoPaterno { get; set; }
        public string? ColaboradorApellidoMaterno { get; set; }
        public string? ColaboradorNumEmpleado { get; set; }
        public string? ColaboradorNombreCompleto { get { return ColaboradorNombre + " " + ColaboradorApellidoPaterno + " " + ColaboradorApellidoMaterno; } }


        public string? InventarioEstadoNombre { get { return OnEstadoNombre(InventarioEstado); } }
        public string? InventarioEstadoColor { get { return OnEstadoColor(InventarioEstado); } }

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

        //public Guid InventarioUbicacionActivoId { get; set; }

        public List<InventoryLocationAQ> Ubicacion { get; set; }
    }

    public class InventoryLocationAQ
    {
        public Guid UbicacionId { get; set; }
        public string UbicacionNombre { get; set; }
        public byte InventarioUbicacionStatus { get; set; }
        public Guid InventarioId { get; set; }
        public string InventarioUbicacionStatusNombre { get { return OnStatusNombre(InventarioUbicacionStatus); } }    /* 1.Por inventariar - 2.Finalizado */


        public List<Asset> Activo { get; set; }


        public string OnStatusNombre(byte? status)
        {
            string s = "";
            if (status == 1)
            {
                s = "Por inventariar";
            }
            if (status == 2)
            {
                s = "Finalizado";
            }
            return s;
        }
    }

    public class CollaboratorAQ
    {
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? NumEmpleado { get; set; }
    }
}