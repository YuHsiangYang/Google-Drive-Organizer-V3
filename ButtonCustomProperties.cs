using Google_Drive_Organizer_V3.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Google_Drive_Organizer_V3
{
    public class ButtonCustomProperties : Button
    {
        public static readonly DependencyProperty Corner_Radius = DependencyProperty.Register("Corner Radius", typeof(double), typeof(ButtonCustomProperties));
        public static readonly DependencyProperty HoverColor = DependencyProperty.Register("Hover Color", typeof(Color), typeof(ButtonCustomProperties));
        public static ResourceDictionary resourceDictionary { get; set; } = new ResourceDictionary()
        {
            Source = new Uri(@".\ApplicationResources.xaml", UriKind.Relative)
        };
        public static double GetCornerRadius(Button target)
        {
            return (double)target.GetValue(Corner_Radius);
        }

        public static void SetCornerRadius(Button target, double value)
        {
            target.SetValue(Corner_Radius, value);
            target.Template = resourceDictionary["CustomButtonTemplate"] as ControlTemplate;
            target.ApplyTemplate();
            Border front = (Border)target.Template.FindName("Front", target);
            Border border = (Border)target.Template.FindName("border", target);
            Border mainborder = (Border)target.Template.FindName("MainBorder", target);
            front.CornerRadius = new CornerRadius(value);
            border.CornerRadius = new CornerRadius(value);
            mainborder.CornerRadius = new CornerRadius(value);
            target.ApplyTemplate();
            target.UpdateDefaultStyle();
            target.UpdateLayout();
        }
        public static Color GetHoverColor(Button target)
        {
            return (Color)target.GetValue(HoverColor);
        }
        public static void SetHoverColor(Button target, Color value)
        {
            target.SetValue(HoverColor, value);
            target.ApplyTemplate();
            Border front = (Border)target.Template.FindName("Front", target);
            front.Background = new SolidColorBrush(value);
            target.ApplyTemplate();
            target.UpdateDefaultStyle();
            target.UpdateLayout();
        }
    }
}
