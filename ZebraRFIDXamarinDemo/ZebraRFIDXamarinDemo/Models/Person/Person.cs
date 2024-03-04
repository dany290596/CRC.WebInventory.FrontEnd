using System;
using SQLite;

namespace ZebraRFIDXamarinDemo.Models.Person
{
    [Table(@"Person")]
    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Pais { get; set; }
    }
}