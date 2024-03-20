using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ZebraRFIDXamarinDemo.Models.Startup
{
	[Table(@"InventoryLocation")]
	public class InventoryLocation
	{
		[PrimaryKey]
		public Guid Id { get; set; }

		[ForeignKey(typeof(Inventory))]
		public Guid InventarioId { get; set; }

		[ForeignKey(typeof(Location))]
		public Guid UbicacionId { get; set; }

		[ManyToOne]
		public Inventory Inventario { get; set; }

		[ManyToOne]
		public Location Ubicacion { get; set; }
	}
}