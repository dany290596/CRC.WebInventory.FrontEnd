using System;
using ZebraRFIDXamarinDemo.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZebraRFIDXamarinDemo.Controls;
using Android.Graphics.Drawables;
using Android.Text;
using Xamarin.Essentials;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace ZebraRFIDXamarinDemo.Droid.Controls
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        { }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                GradientDrawable gradientDramable = new GradientDrawable();
                gradientDramable.SetColor(global::Android.Graphics.Color.Transparent);
                Control.SetBackground(gradientDramable);
                this.Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                Control.SetHintTextColor(global::Android.Graphics.Color.Gray);
            }
        }
    }
}