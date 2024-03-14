using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZebraRFIDXamarinDemo.Controls;
using ZebraRFIDXamarinDemo.Droid.Controls;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace ZebraRFIDXamarinDemo.Droid.Controls
{
    public class CustomLabelRenderer : Xamarin.Forms.Platform.Android.LabelRenderer
    {
        public CustomLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            Control?.SetIncludeFontPadding(false);
        }
    }
}