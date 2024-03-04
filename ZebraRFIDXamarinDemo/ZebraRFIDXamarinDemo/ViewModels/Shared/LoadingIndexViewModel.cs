using System;
using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Controls;
using ZebraRFIDXamarinDemo.Views.Authorization;
using ZebraRFIDXamarinDemo.Views.Dashboard;

namespace ZebraRFIDXamarinDemo.ViewModels.Shared
{
	public class LoadingIndexViewModel
	{
		public LoadingIndexViewModel()
		{
			CheckUserLoginDetail();
		}

		private async void CheckUserLoginDetail()
		{
			try
			{
				var userInformationGetByLast = await App.userInformationRepository.GetByLastOrDefaultAsync();
				if (userInformationGetByLast != null)
				{
                    ZebraRFIDXamarinDemo.AppShell.Current.FlyoutHeader = new FlyoutHeaderControl(userInformationGetByLast);
                    await Shell.Current.GoToAsync($"//{nameof(DashboardIndex)}");
				}
				else
				{
					await App.userInformationRepository.DeleteAllAsync();
					await Shell.Current.GoToAsync($"//{nameof(LoginIndex)}");
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}