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

namespace Google_Drive_Organizer_V3.Pages
{
    /// <summary>
    /// Progress bar for all the progress reporting throughout the application.
    /// </summary>
    public partial class ProgressBar : UserControl
    {
        public ProgressBar()
        {
            InitializeComponent();

        }
        /// <summary>
        /// Percentage in decimal
        /// ex: 10% = 0.1
        /// </summary>
        /// 
        private double Percentage;

        //When this attribute changes, the progress of the progress bar will also change.
        public double percentage
        {
            get { return Percentage; }
            set
            {
                if (1 >= value)
                {
                    Percentage = Math.Round(value, 2);
                    UpDate_Percentage(Percentage, TimeSpan.FromSeconds(.3));
                }
            }
        }

        //This is to dynamically adjust the width percentage bar and the main bar when the size of the component changes.
        private void ProgressBar_Main_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TimeSpan Animation_Duration = TimeSpan.FromSeconds(.2);
            UpDate_Background_Rec_Size(e.NewSize.Width, Animation_Duration);
            UpDate_Percentage(percentage, Animation_Duration);
        }
        
        //Update both the text and the width of the bar.
        private void UpDate_Percentage(double percentage, TimeSpan duration)
        {
            if (percentage * ProgressBar_Main.RenderSize.Width > ProgressBar_Main.RenderSize.Height)
            {
                DoubleAnimation Percentage_Animation = new DoubleAnimation(ProgressBar_Rec.RenderSize.Width, ProgressBar_Main.RenderSize.Width * percentage, duration);
                Percentage_Animation.DecelerationRatio = .7;
                ProgressBar_Rec.BeginAnimation(WidthProperty, Percentage_Animation);

            }
            Percentage_Label.Content = percentage * 100 + "%";
        }
        //Update the background rectangle (the bar behind the current progress)
        private void UpDate_Background_Rec_Size(double new_size, TimeSpan duration)
        {
            DoubleAnimation Background_Rec_Animation = new DoubleAnimation(Background_Rec.RenderSize.Width, new_size, duration);
            Background_Rec_Animation.DecelerationRatio = .7;
            Background_Rec.BeginAnimation(WidthProperty, Background_Rec_Animation);
        }

        private void ProgressBar_Main_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressBar_Rec.Width = ProgressBar_Main.RenderSize.Width * Percentage;
            Percentage_Label.Content = Percentage * 100 + "%";
            Background_Rec.RadiusX = ProgressBar_Main.RenderSize.Height / 2;
        }
    }
}
