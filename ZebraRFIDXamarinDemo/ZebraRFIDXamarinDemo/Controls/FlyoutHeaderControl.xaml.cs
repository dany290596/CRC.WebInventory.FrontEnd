using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZebraRFIDXamarinDemo.Models.Sesion;
using ZebraRFIDXamarinDemo.ViewModels.Authorization;

namespace ZebraRFIDXamarinDemo.Controls
{
    public partial class FlyoutHeaderControl : StackLayout
    {
        public FlyoutHeaderControl(UserInformation userInformation)
        {
            InitializeComponent();

            if (userInformation != null)
            {
                lblEmail.Text = userInformation.EmailUsuario;
                /*
                lblEmail = new Label
                {
                    Text = userInformation.EmailUsuario,
                    FontSize = 28,
                    TextColor = Color.DarkOrange
                };
                */
            }
        }
    }
}