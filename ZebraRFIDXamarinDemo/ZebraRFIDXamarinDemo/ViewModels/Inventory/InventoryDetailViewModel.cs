using System;
using ZebraRFIDXamarinDemo.Models.Startup;

namespace ZebraRFIDXamarinDemo.ViewModels.Inventory
{
	public class InventoryDetailViewModel : InventoryBaseViewModel
	{
		public InventoryDetailViewModel()
		{
			InventorySync = new Models.Startup.Inventory();
		}
	}
}