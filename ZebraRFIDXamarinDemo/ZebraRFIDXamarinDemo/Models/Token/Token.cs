using System;
namespace ZebraRFIDXamarinDemo.Models.Token
{
    public class Token
    {
        public Guid UsuarioId { get; set; }
        public string Email { get; set; }
        public Guid PerfilId { get; set; }
        public Guid EmpresaId { get; set; }
    }
}