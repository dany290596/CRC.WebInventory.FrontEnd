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

/*
BioLite N2
962CD5F7-CF54-4124-B0CF-60F9E90CCD76


VERTEXMIGUE
A28BFB81-D463-4E2F-AAB6-6630C13CDEEE


Vertex 3
2A80F6AC-54F5-4662-A58D-645F2A98FD8D


Vertex 2
1FAA73CC-7883-4742-AE6D-59338A4480B0


AccWe Valeo1
B86FF636-333A-441E-AF2E-59C887AA6488


93256674-4964-41D3-B73F-246BDDF286D4
32B0C4B0-2E63-4104-A888-256A9DC65301
7BF62861-3932-42E5-B641-470B1F6D8F42
1FAA73CC-7883-4742-AE6D-59338A4480B0
B86FF636-333A-441E-AF2E-59C887AA6488
AC43A12E-C9CA-4FF3-A9F8-5C0480445FF4
CB7C5F08-E7D3-4D5E-A82B-5FAEF18E8658
962CD5F7-CF54-4124-B0CF-60F9E90CCD76
E88F7B55-FC29-4027-9B58-62545BF586F4
2A80F6AC-54F5-4662-A58D-645F2A98FD8D
A28BFB81-D463-4E2F-AAB6-6630C13CDEEE
0BA202A5-75FA-4992-AB17-7A19FD712A14
83B82534-2FDA-4DA8-94D1-A0A23619B70A
47A9A4FB-ECAC-4358-9D28-A8553B36C3AD
8F0B4D5A-BF20-4F52-BD63-DDA8AE811F68
D6D5AE7D-2DEE-4533-BE53-E72420DEDF19
ED23314B-F653-4D0B-9B19-F2C0A7CAFCB6
CFAF192D-960F-4A1F-A0ED-F6028F6368A5
*/