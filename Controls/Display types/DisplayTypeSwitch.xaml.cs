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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Google_Drive_Organizer_V3.Controls
{
    /// <summary>
    /// DisplayType.xaml 的互動邏輯
    /// </summary>
    public partial class DisplayType : UserControl
    {
        public DisplayType()
        {
            InitializeComponent();
        }
        Thickness list = new Thickness(2, 2, 53, 2);
        Thickness images = new Thickness(2, 2, -49, 2);
        Duration animation_duration = TimeSpan.FromSeconds(.15);
        public event Action<TypeOfDisplay> DisplayTypeChanged;
        Button previous_displaytype = new Button();
        public TypeOfDisplay Display_Type { get; private set; } = new TypeOfDisplay();
        private void ChangeDisplayTypeTrigger_Clicked(object sender, RoutedEventArgs e)
        {
            ThicknessAnimation animation = new ThicknessAnimation();
            Button send = (Button)sender;
            if (previous_displaytype != send)
            {
                TypeOfDisplay display = new TypeOfDisplay();
                if (send == ListItem)//From Images to list
                {
                    display = TypeOfDisplay.ListView;
                    animation = new ThicknessAnimation(images, list, animation_duration);
                }
                else if (send == Images)
                {
                    display = TypeOfDisplay.ImageView;
                    animation = new ThicknessAnimation(list, images, animation_duration);
                }
                animation.AccelerationRatio = 0.3;
                animation.DecelerationRatio = 0.3;
                SelectedDisplayType_Rectangle.BeginAnimation(MarginProperty, animation);
                DisplayTypeChanged(display);
                Display_Type = display;
                previous_displaytype = send;
            }
        }
    }
    [Flags]
    public enum TypeOfDisplay
    {
        ListView,
        ImageView
    }
}
