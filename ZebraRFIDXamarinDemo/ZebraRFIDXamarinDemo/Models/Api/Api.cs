using System;
namespace ZebraRFIDXamarinDemo.Models.Api
{
    public class Api<T>
    {
        public bool Respuesta { get; set; }

        public string Mensaje { get; set; }

        public int Codigo { get; set; }

        public T Data { get; set; }

        public Api(bool respuesta, string mensaje, int codigo, T data)
        {
            Respuesta = respuesta;
            Mensaje = mensaje;
            Codigo = codigo;
            Data = data;
        }
    }
}