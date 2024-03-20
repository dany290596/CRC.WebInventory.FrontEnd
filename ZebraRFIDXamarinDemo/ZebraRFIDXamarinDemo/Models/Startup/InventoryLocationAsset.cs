using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
	[Table(@"InventoryLocationAsset")]
	public class InventoryLocationAsset
	{
		[PrimaryKey]
		public Guid Id { get; set; }

		[ForeignKey(typeof(InventoryLocation))]
		public Guid InventarioUbicacionId { get; set; }

		[ForeignKey(typeof(Asset))]
		public Guid ActivoId { get; set; }

		[ManyToOne]
		public InventoryLocation InventarioUbicacion { get; set; }

		[ManyToOne]
		public Asset Activo { get; set; }
	}
}