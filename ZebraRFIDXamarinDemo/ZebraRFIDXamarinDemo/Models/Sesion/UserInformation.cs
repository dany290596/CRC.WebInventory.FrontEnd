using System;
using SQLite;

namespace ZebraRFIDXamarinDemo.Models.Sesion
{
    [Table(@"UserInformation")]
    public class UserInformation
    {
        [PrimaryKey]
        public Guid? Id { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? EmailColaborador { get; set; }
        public string? EmailUsuario { get; set; }
        public string? Token { get; set; }
        public Guid? EmpresaId { get; set; }
        public Guid? PerfilId { get; set; }
    }
}