using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Kutuphanem.CustomControls;
using Kutuphanem.Droid.CustomControlsRenderer;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(CustomEntry),typeof(MyEntryRenderer))]
namespace Kutuphanem.Droid.CustomControlsRenderer
{
    public class MyEntryRenderer : EntryRenderer
    {
        public MyEntryRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if(Control != null)
            {
                CustomEntry view = (CustomEntry)Element;
                var backGround = new GradientDrawable();
                backGround.SetShape(ShapeType.Rectangle);
                backGround.SetColor(view.BackgroundColor.ToAndroid());

                backGround.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());
                backGround.SetCornerRadius(DpToPixels(Context, Convert.ToSingle(view.CornerRadius)));

                Control.SetBackground(backGround);
                Control.SetPadding(30, 30, 30, 30);
            }
        }
        public static float DpToPixels(Context context, float valueInDp)
        {
            if (context.Resources != null)
            {
                DisplayMetrics metrics = context.Resources.DisplayMetrics;
                return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
            }

            return 0;
        }
    }
}