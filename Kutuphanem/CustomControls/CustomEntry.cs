using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Kutuphanem.CustomControls
{
    public class CustomEntry : Entry
    {
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(CustomEntry), (double)10);
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty); 
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomEntry), Color.Black);
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(CustomEntry), (int)1);
        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }
    }
}
