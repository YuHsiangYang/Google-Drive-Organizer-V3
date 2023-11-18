using Google_Drive_Organizer_V3.Classes;
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
    /// SortMannerControl.xaml 的互動邏輯
    /// </summary>
    public partial class SortMannerControl : UserControl
    {
        public SortMannerControl()
        {
            InitializeComponent();
        }
        public SortManner current_manner = SortManner.Ascending;
        public event EventHandler<SortManner> SortMannerChanged_Event;
        // left side should be negative and right side should be positive
        public void ExpandArrows(RotateTransform left, RotateTransform right, TimeSpan duration)
        {
            double to_value = 45;
            //Animation for left element
            DoubleAnimation animation_left = new DoubleAnimation(-to_value, duration);
            left.BeginAnimation(RotateTransform.AngleProperty, animation_left);

            //animation for right element
            DoubleAnimation animation_right = new DoubleAnimation(to_value, duration);
            right.BeginAnimation(RotateTransform.AngleProperty, animation_right);
        }
        public void CollapseArrows(RotateTransform left, RotateTransform right, TimeSpan duration)
        {
            //to 0
            DoubleAnimation animation = new DoubleAnimation(0, duration);
            RotateTransform rotate = new RotateTransform();
            left.BeginAnimation(RotateTransform.AngleProperty, animation);
            right.BeginAnimation(RotateTransform.AngleProperty, animation);
        }

        private void trigger_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan animation_duration = TimeSpan.FromSeconds(0.1); ;
            switch (current_manner)
            {
                case SortManner.Ascending:
                    CollapseArrows(Rotate_UpperArrowLeft, Rotate_UpperArrowRight, animation_duration);

                    ExpandArrows(Rotate_LowerArrowLeft, Rotate_LowerArrowRight, animation_duration);
                    current_manner = SortManner.Descending;
                    break;
                case SortManner.Descending:
                    CollapseArrows(Rotate_LowerArrowLeft, Rotate_LowerArrowRight, animation_duration);

                    ExpandArrows(Rotate_UpperArrowLeft, Rotate_UpperArrowRight, animation_duration);
                    current_manner = SortManner.Ascending;
                    break;
                default:
                    break;
            }
            SortMannerChanged_Event?.Invoke(this, current_manner);
        }
    }
}
