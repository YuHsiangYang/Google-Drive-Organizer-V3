using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Google_Drive_Organizer_V3
{
    public class Button_CustomProperties : UIElement
    {
        public static readonly DependencyProperty CornerRadius_Custom = DependencyProperty.RegisterAttached("Corner Radius", typeof(double), typeof(Button_CustomProperties), new PropertyMetadata(default(double)));
        public static double GetCornerRadius(UIElement target) => (double)target.GetValue(CornerRadius_Custom);
        public static void SetCornetRadius(UIElement target, double value)
        {

            //target.SetValue(CornerRadius_Custom, value);
            //Style corner = new Style(typeof(Button));
            //Setter corner_setter = new Setter(Border.CornerRadiusProperty, value);
            //corner.Setters.Add(corner_setter);
            ////var targe = ((Button)target).Template;
            //Button s = (Button)target;
            //s.Template = (ControlTemplate)Application.Current.Resources["CustomButtonTemplate"];
            //Border d = (Border) s.Template.FindName("border", s);
            //((Button)target).Resources.Add("KEY", corner);
            //var s = ((Button)target).Template.FindName("border", (Button)target);
            //Console.WriteLine(((Button)target).Style.Resources.Count);
        }
    }
}