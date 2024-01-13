using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Google_Drive_Organizer_V3.Controls
{
    /// <summary>
    /// This is a custom component to be used for switching things.
    /// </summary>
    public partial class CircleSwitch : UserControl
    {
        public CircleSwitch()
        {
            InitializeComponent();
            Off_Color_Circle = Color.FromRgb(234, 126, 122);
            Off_Color_Background = Color.FromArgb(30, 0, 0, 0);
            On_Color_Circle = Circle_Fill_SolidColorBrush.Color;
            On_Color_Backround = Color.FromArgb(20, 0, 0, 0);
            Circle_Fill_SolidColorBrush.Color = Off_Color_Circle;
        }
        public CircleSwitch(string on_text, string off_text)
        {
            InitializeComponent();
            Off_Color_Circle = Color.FromRgb(234, 126, 122);
            Off_Color_Background = Color.FromArgb(30, 0, 0, 0);
            On_Color_Circle = Circle_Fill_SolidColorBrush.Color;
            On_Color_Backround = Color.FromArgb(20, 0, 0, 0);
            Circle_Fill_SolidColorBrush.Color = Off_Color_Circle;
            On_Text = on_text;
            Off_Text = off_text;
        }
        Storyboard switch_animation = new Storyboard();
        Thickness Off = new Thickness(20, 20, 450, 20);
        Thickness On = new Thickness(450, 20, 20, 20);
        Duration Duration = new Duration(TimeSpan.FromSeconds(.15));
        Color Off_Color_Circle = new Color();
        Color Off_Color_Background = new Color();
        Color On_Color_Circle = new Color();
        Color On_Color_Backround = new Color();
        public string On_Text = "";
        public string Off_Text = "";
        public event Action<bool, string> On_Status_Changed;
        public bool On_Off = false;

        private void Switch_On()
        {
            ThicknessAnimation switch_on_Animation = new ThicknessAnimation(Off, On, Duration);
            switch_animation.AccelerationRatio = .1;
            switch_animation.DecelerationRatio = .1;
            switch_Rectangle.BeginAnimation(MarginProperty, switch_on_Animation);
            ColorAnimation background_color_animation = new ColorAnimation(Off_Color_Background, On_Color_Backround, Duration);
            ColorAnimation circle_color_animation = new ColorAnimation(Off_Color_Circle, On_Color_Circle, Duration);
            Circle_Fill_SolidColorBrush.BeginAnimation(SolidColorBrush.ColorProperty, circle_color_animation);
            Background_Fill_SolidColorBrush.BeginAnimation(SolidColorBrush.ColorProperty, background_color_animation);
            On_Off = true;
        }
        private void Switch_Off()
        {
            ThicknessAnimation switch_off_Animation = new ThicknessAnimation(On, Off, Duration);
            switch_off_Animation.AccelerationRatio = .1;
            switch_off_Animation.DecelerationRatio = .1;
            switch_Rectangle.BeginAnimation(MarginProperty, switch_off_Animation);
            ColorAnimation background_color_animation = new ColorAnimation(On_Color_Backround, Off_Color_Background, Duration);
            ColorAnimation circle_color_animation = new ColorAnimation(On_Color_Circle, Off_Color_Circle, Duration);
            Circle_Fill_SolidColorBrush.BeginAnimation(SolidColorBrush.ColorProperty, circle_color_animation);
            Background_Fill_SolidColorBrush.BeginAnimation(SolidColorBrush.ColorProperty, background_color_animation);
            On_Off = false;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void Toggle_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (On_Off == true)//on so switch to off
            {
                Switch_Off();
                On_Status_Changed(On_Off, Off_Text);
            }
            else
            {
                Switch_On();
                On_Status_Changed(On_Off, On_Text);
            }
            Console.WriteLine(On_Off);
        }
    }
}
